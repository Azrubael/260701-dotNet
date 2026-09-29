using System.Runtime.InteropServices;

partial class Program
{
  [LibraryImport("winmm.dll")]
  private static partial int midiOutOpen(out IntPtr hmo, int uDeviceID, int dwCallback, int dwInstance, int dwFlags);

  [LibraryImport("winmm.dll")]
  private static partial int midiOutShortMsg(IntPtr hmo, int dwMsg);

  [LibraryImport("winmm.dll")]
  private static partial int midiOutClose(IntPtr hmo);

  static int MakeNoteOn(int channel, int note, int velocity)
      => 0x90 | channel | (note << 8) | (velocity << 16);

  static int MakeNoteOff(int channel, int note)
      => 0x80 | channel | (note << 8);

  static int MakeProgramChange(int channel, int program)
      => 0xC0 | channel | (program << 8);

  static void Main()
  {
    int openResult = midiOutOpen(out var hMidi, 0, 0, 0, 0);
    if (openResult != 0)
    {
      Console.WriteLine($"Could not open MIDI device. Error: {openResult}");
      return;
    }

    try
    {
      int ch = 0;

      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(ch, 36));
      if (programResult != 0)
      {
        Console.WriteLine($"Program change failed. Error: {programResult}");
        return;
      }

      const int stepMs = 60000 / 152 / 4;
      string kickPattern = "x...x...x...x.x.";
      string snarePattern = "....x.......x..x";
      string hatPattern = "x.x.x.x.x.x.x.xx";
      string[] leadBars =
      [
        "A4 - C5 - E5 - A5 - G5 - E5 - D5 - E5 -",
        "C5 - - - B4 - A4 - B4 - C5 - D5 - - -",
        "C5 - A4 - F4 - A4 - C5 - F5 - E5 - C5 -",
        "D5 - - - B4 - G4 - B4 - D5 - G5 - - -",
        "A5 - - - G5 - E5 - C5 - E5 - A5 - C6 -",
        "G5 - - - E5 - C5 - E5 - G5 - C6 - B5 -",
        "B5 - A5 - G5 - D5 - B4 - D5 - G5 - A5 -",
        "G#5 - - - E5 - B4 - G#4 - B4 - E5 - - -"
      ];

      var melody = new List<(int note, int ms, int vel)>();
      foreach (string bar in leadBars)
      {
        foreach (string token in bar.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
          melody.Add((token == "-" ? -1 : ParsePitch(token), stepMs, 36));
        }
      }
for (int i = 0; i < melody.Count; i++)
      {
        var (note, ms, vel) = melody[i];
        int step = i % 16;

        if (note >= 0)
        {
          int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(ch, note, vel));
          if (noteOnResult != 0)
          {
            Console.WriteLine($"Note on failed (note {note}). Error: {noteOnResult}");
            break;
          }
        }

        if (kickPattern[step] == 'x')
          PlayDrumHit(hMidi, 42, step is 0 or 8 ? 124 : 108);

        if (snarePattern[step] == 'x')
          PlayDrumHit(hMidi, 80, step == 4 ? 120 : step == 15 ? 78 : 110);

        if (hatPattern[step] == 'x')
          PlayDrumHit(hMidi, 87, step % 4 == 0 ? 96 : 72);

        Thread.Sleep(ms);

        if (note >= 0)
        {
          int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(ch, note));
          if (noteOffResult != 0)
          {
            Console.WriteLine($"Note off failed (note {note}). Error: {noteOffResult}");
            break;
          }
        }
      }
    }
    finally
    {
      midiOutClose(hMidi);
    }
  }


  static int ParsePitch(string token)
  {
    string pitch = token.Substring(0, token.Length - 1);
    int octave = int.Parse(token.Substring(token.Length - 1));

    int semitone = pitch switch
    {
      "C" => 0,
      "C#" => 1,
      "D" => 2,
      "D#" => 3,
      "E" => 4,
      "F" => 5,
      "F#" => 6,
      "G" => 7,
      "G#" => 8,
      "A" => 9,
      "A#" => 10,
      "B" => 11,
      _ => throw new ArgumentException($"Invalid pitch: {token}")
    };

    return (octave + 1) * 12 + semitone;
  }


  /// <summary>
  /// A simple overload of the PlayDrumHit method.
  /// </summary>
  /// <param name="hMidi"></param>
  /// <param name="note"></param>
  static void PlayDrumHit(IntPtr hMidi, int note)
  {
    const int percussionChannel = 9; // MIDI channel 10

    int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(percussionChannel, note, 100));
    if (noteOnResult != 0)
    {
      Console.WriteLine($"Drum note-on failed (note {note}). Error: {noteOnResult}");
      return;
    }

    int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(percussionChannel, note));
    if (noteOffResult != 0)
      Console.WriteLine($"Drum note-off failed (note {note}). Error: {noteOffResult}");
  }


    static void PlayDrumHit(IntPtr hMidi, int note, int velocity)
  {
    const int percussionChannel = 9; // MIDI channel 10

    int noteOnResult = midiOutShortMsg(
      hMidi, MakeNoteOn(percussionChannel, note, velocity));

    if (noteOnResult != 0)
    {
      Console.WriteLine($"Drum note-on failed (note {note}). Error: {noteOnResult}");
      return;
    }

    int noteOffResult = midiOutShortMsg(
      hMidi, MakeNoteOff(percussionChannel, note));

    if (noteOffResult != 0)
      Console.WriteLine($"Drum note-off failed (note {note}). Error: {noteOffResult}");
  }
}
