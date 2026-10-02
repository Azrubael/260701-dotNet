using System.Runtime.InteropServices;

namespace music_1;

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
    string KickPattern = "x...x...x...x.x.";
    string SnarePattern = "....x.......x..x";
    string HatPattern = "x.x.x.x.x.x.x.xx";
    string[] ThemeBars =
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

    string[] PassageBars1 =
    [
      "E5 - G5 - A5 - C6 - B5 - A5 - G5 - E5 -",
      "F5 - A5 - C6 - A5 - G5 - E5 - D5 - E5 -",
      "D5 - F5 - A5 - D6 - C6 - A5 - F5 - A5 -",
      "E5 - G#5 - B5 - D6 - C6 - B5 - G#5 - B5 -"
    ];

    string[] PassageBars2 =
    [
      "C5 - E5 G5 - A5 - C6 - D6 - C6 A5 - G5 -",
      "A5 - G5 - E5 - C5 - D5 - E5 G5 - A5 - G5",
      "F5 - A5 C6 - D6 - C6 - A5 - F5 G5 - A5 -",
      "G#5 - B5 - D6 - E6 - D6 C6 - B5 G#5 - C5 -"
    ];

    string[] leadBars = [.. ThemeBars,
      .. PassageBars1, .. ThemeBars,
      .. PassageBars2, .. ThemeBars,
      .. PassageBars1, .. PassageBars2];

    Console.WriteLine("""
        Select the melody to play:
        f - FirstMelody
        s - SecondMelody
        k - KalynaMelody
        """);
    var selection = char.ToLowerInvariant(Console.ReadKey(intercept: true).KeyChar);
    Console.WriteLine();

    switch (selection)
    {
      case 'f':
        FirstMelody(leadBars, KickPattern, SnarePattern, HatPattern);
        break;
      case 's':
        SecondMelody();
        break;
      case 'k':
        KalynaMelody();
        break;
      default:
        Console.WriteLine("Unknown selection.");
        break;
    }

  }


  static void FirstMelody(
      string[] leadBars,
      string KickPattern,
      string SnarePattern,
      string HatPattern)
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

      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(ch, 87));
      if (programResult != 0)
      {
        Console.WriteLine($"Program change failed. Error: {programResult}");
        return;
      }

      const int stepMs = 60000 / 38 / 16;

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

        if (KickPattern[step] == 'x')
          PlayDrumHit(hMidi, 36, step is 0 or 8 ? 95 : 80);

        if (SnarePattern[step] == 'x')
          PlayDrumHit(hMidi, 38, step == 4 ? 100 : step == 8 ? 85 : 80);

        if (HatPattern[step] == 'x')
          PlayDrumHit(hMidi, 42, step % 4 == 0 ? 75 : 55);

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
      int closeResult = midiOutClose(hMidi);
      if (closeResult != 0)
        Console.WriteLine($"Could not close MIDI device. Error: {closeResult}");
    }
  }


  static void SecondMelody()
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

      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(ch, 34));
      if (programResult != 0)
      {
        Console.WriteLine($"Program change failed. Error: {programResult}");
        return;
      }

      const int stepMs = 60000 / 38 / 16;
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
          PlayDrumHit(hMidi, 36, step is 0 or 8 ? 95 : 80);

        if (snarePattern[step] == 'x')
          PlayDrumHit(hMidi, 38, step == 4 ? 100 : step == 16 ? 85 : 80);

        if (hatPattern[step] == 'x')
          PlayDrumHit(hMidi, 42, step % 4 == 0 ? 75 : 55);

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
      int closeResult = midiOutClose(hMidi);
      if (closeResult != 0)
        Console.WriteLine($"Could not close MIDI device. Error: {closeResult}");
    }
  }


static void KalynaMelody()
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

      // Program 0 = Acoustic Grand Piano
      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(ch, 0));
      if (programResult != 0)
      {
        Console.WriteLine($"Program change failed. Error: {programResult}");
        return;
      }

      // Base rhythmic unit: Eighth note (300 ms at ~100 BPM)
      const int eighthMs = 300;
      const int leadVelocity = 90;

      // Corrected Score: (Pitch Token, Duration in Eighth Notes)
      (string pitch, int eighths)[] score =
      [
        // Line 1: "Oi u luzi chervona kalyna,"
        ("A4", 1), ("A4", 1),
        ("D5", 4), // Half note
        ("C#5", 1), ("B4", 1), ("A4", 1), ("B4", 1),
        ("C#5", 1), ("D5", 1),
        ("E5", 8), // Half note tied to half note (4 + 4)

        // Line 2: "oi u luzi chervona kalyna, tam sto-"
        ("D5", 1), ("B4", 1),
        ("C#5", 1), ("D5", 1), ("C#5", 1), ("B4", 1),
        ("A4", 1), ("B4", 1), ("C#5", 1), ("D5", 1),
        ("E5", 1), ("E5", 1),

        // Line 3: "-yala moloda divchyna, oi u"
        ("F#5", 1), ("E5", 1),
        ("D5", 1), ("C#5", 1),
        ("B4", 1), ("A4", 1),
        ("B4", 2), // Quarter note
        ("B4", 1), ("-", 1), ("F#4", 1), ("F#4", 1), // 8th note, 8th rest, pickup "oi u"

        // Line 4: "luzi chervona kalyna, tam stoyala mo-"
        ("B4", 1), ("B4", 1),
        ("D5", 1), ("C#5", 1),
        ("B4", 1), ("A4", 1),
        ("B4", 1), ("C#5", 1),
        ("D5", 1), ("E5", 1),
        ("F#5", 1), ("E5", 1),

        // Line 5: "-loda divchyna."
        ("D5", 1), ("C#5", 1),
        ("B4", 1), ("A4", 1),
        ("B4", 8) // Half note tied to half note to resolve in B minor
      ];

      // Time tracker to keep drums perfectly on beat
      int currentEighth = 0;

      foreach (var (pitchToken, eighths) in score)
      {
        bool isRest = pitchToken == "-";
        int note = isRest ? -1 : ParsePitch(pitchToken);

        if (!isRest)
        {
          int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(ch, note, leadVelocity));
          if (noteOnResult != 0) break;
        }

        // Step through the duration of the note in eighth-note increments
        for (int i = 0; i < eighths; i++)
        {
          // Play a Kick drum exactly on every quarter-note beat (every 2 eighths)
          if (currentEighth % 2 == 0)
          {
            PlayDrumHit(hMidi, 36, 85);
          }
          // Play Hi-hat evenly on every eighth note
          PlayDrumHit(hMidi, 42, 50);

          // Articulation logic: only cut off the note briefly at the VERY end of its duration
          if (i == eighths - 1)
          {
            int playMs = (int)(eighthMs * 0.85);
            int restMs = eighthMs - playMs;

            Thread.Sleep(playMs);
            if (!isRest)
            {
              int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(ch, note));
              if (noteOffResult != 0)
              {
                Console.WriteLine($"Note off failed (note {note}). Error: {noteOffResult}");
                return;
              }
            }
            Thread.Sleep(restMs);
          }
          else
          {
            // If the note spans multiple eighths (e.g. half notes), hold it continuously
            Thread.Sleep(eighthMs);
          }

          currentEighth++;
        }
      }
    }
    finally
    {
      int closeResult = midiOutClose(hMidi);
      if (closeResult != 0)
        Console.WriteLine($"Could not close MIDI device. Error: {closeResult}");
    }
  }


  static int ParsePitch(string token)
  {
    string pitch = token[..^1];
    int octave = int.Parse(token[^1..]);

    int semitone = pitch switch
    {
      // До-Ре-Мі-Фа-Соль-Ля-Си
      // C -D -E -F -G   -A -B
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
