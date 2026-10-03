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

  /// <summary>
  /// Makes the sount fade out with "value".
  /// </summary>
  /// <param name="channel"></param>
  /// <param name="program"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  static int MakeControlChange(int channel, int program, int value)
    => 0xB0 | channel | (program << 8) | (value << 16);


  static void Main()
  {
    string KickPattern = "x...x...x...x.x.";
    string SnarePattern = "....x.......x..x";
    string HatPattern = "x.x.x.x.x.x.x.xx";
    string[] FirstMelodyBars =
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

    string[] FirstThemeLeadBars = [.. FirstMelodyBars,
      .. PassageBars1, .. FirstMelodyBars,
      .. PassageBars2, .. FirstMelodyBars,
      .. PassageBars1, .. PassageBars2];

    Console.WriteLine("""
        Select the melody to play:
        f - FirstMelody
        s - SecondMelody
        k - KalynaMelody
        m - MagicSound
        """);
    var selection = char.ToLowerInvariant(Console.ReadKey(intercept: true).KeyChar);
    Console.WriteLine();

    switch (selection)
    {
      case 'f':
        FirstMelody(FirstThemeLeadBars, KickPattern, SnarePattern, HatPattern);
        break;
      case 's':
        SecondMelody();
        break;
      case 'k':
        KalynaMelody();
        break;
      case 'm':
        MagicSound();
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

      const int eighthMs = 150;
      const int leadVelocity = 100;

      (string pitch, int eighths)[] kalynaBars1 =
      [
        // --- Measures 1 to 4 (4/4 time) ---
        // "Oi u lu - zi cher-vo-na ka-li - na"
        ("E4", 2), ("E4", 2), ("F#4", 3), ("E4", 1),
        ("B4", 2), ("A4", 2), ("G4", 2), ("F#4", 2),
        // "po - xi-li - la - sia,"
        ("G4", 3), ("F#4", 1), ("E4", 2), ("E4", 2),
        ("E4", 6), ("-", 2), // Dotted half note + quarter rest

        // --- Measures 5 to 8 (4/4 & 2/4 time) ---
        // "Cho-hos na - sha slav-na Uk-ra - i - na"
        ("E4", 2), ("G4", 2), ("B4", 3), ("A4", 1),
        ("B4", 2), ("A4", 2), ("G4", 2), ("F#4", 2),
        // "za - zhu-ri - la - sia."
        ("G4", 3), ("F#4", 1), ("E4", 2), ("E4", 2),
        ("E4", 3), ("-", 1), // Measure 8 (2/4 time): Quarter tied to eighth, then eighth rest
      ];

      (string pitch, int eighths)[] kalynaBars1mod =
      [
        // --- Measures 1 to 4 (4/4 time) ---
        // "Oi u lu - zi cher-vo-na ka-li - na"
        ("E4", 2), ("G4", 2), ("B4", 3), ("A4", 1),
        ("B4", 2), ("A4", 2), ("G4", 2), ("F#4", 2),
        // "po - xi-li - la - sia,"
        ("G4", 3), ("F#4", 1), ("E4", 2), ("E4", 2),
        ("E4", 6), ("-", 2), // Dotted half note + quarter rest

        // --- Measures 5 to 8 (4/4 & 2/4 time) ---
        // "Cho-hos na - sha slav-na Uk-ra - i - na"
        ("E4", 2), ("G4", 2), ("B4", 3), ("A4", 1),
        ("B4", 2), ("A4", 2), ("G4", 2), ("F#4", 2),
        // "za - zhu-ri - la - sia."
        ("G4", 3), ("F#4", 1), ("E4", 2), ("E4", 2),
        ("E4", 3), ("-", 1), // Measure 8 (2/4 time): Quarter tied to eighth, then eighth rest

        // --- Measures 9 to 12 (4/4 & 3/4 time) ---
        // "A my tu - iu"
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
        // "cher-vo-nu ka-li-ny"
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
        // "pi-dii-me - mo,"
        ("A4", 2), ("B4", 2), ("C5", 3), ("B4", 1), // C5 is dotted quarter
        // "A my na - shu"
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),

        // --- Measure 13 to 15: First Ending ---
        // "slav-nu Uk-ra - i - nu,"
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
        // "hei, hei, roz - ve"
        ("A4", 2), ("A4", 2), ("B4", 2), ("C5", 2),
        // "se-li-mo!___"
        ("B4", 1), ("A4", 1), ("G4", 2), ("F#4", 2), // Eighths + Quarters

        // --- Second Pass (Repeated Refrain: Measures 9 to 13) ---
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
        ("A4", 2), ("B4", 2), ("C5", 3), ("B4", 1),
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),

        // --- Measure 16 to 17: Second Ending ---
        // "roz - ve - se-li - mo!"
        ("B4", 2), ("A4", 2), ("G4", 2), ("F#4", 2),
        ("E4", 6), ("-", 2) // Final tied resolve on E4 + quarter rest
      ];

      (string pitch, int eighths)[] kalynaBars2 =
      [
        // --- Measures 9 to 12 (4/4 & 3/4 time) ---
        // "A my tu - iu"
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
        // "cher-vo-nu ka-li-ny"
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
        // "pi-dii-me - mo,"
        ("A4", 2), ("B4", 2), ("C5", 3), ("B4", 1), // C5 is dotted quarter
        // "A my na - shu"
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
      ];

      (string pitch, int eighths)[] kalynaEnding1 =
      [
        // --- Measure 13 to 15: First Ending ---
        // "slav-nu Uk-ra - i - nu,"
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
        // "hei, hei, roz - ve"
        ("A4", 2), ("A4", 2), ("B4", 2), ("C5", 2),
        // "se-li-mo!___"
        ("B4", 1), ("A4", 1), ("G4", 2), ("F#4", 2), // Eighths + Quarters
      ];

      (string pitch, int eighths)[] kalynaEnding2 =
      [
        // --- Measure 16 to 17: Second Ending ---
        // "roz - ve - se-li - mo!"
        ("B4", 2), ("A4", 2), ("G4", 2), ("F#4", 2),
        ("E4", 6), ("-", 2) // Final tied resolve on E4 + quarter rest
      ];

      (string pitch, int eighths)[] kalynaSecondPass =
      [
        // --- Second Pass (Repeated Refrain: Measures 9 to 13) ---
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
        ("A4", 2), ("B4", 2), ("C5", 3), ("B4", 1),
        ("E4", 2), ("G4", 2), ("B4", 3), ("D#5", 1),
        ("E5", 1), ("E5", 1), ("E5", 1), ("D5", 1), ("C5", 1), ("B4", 1),
      ];

      (string pitch, int eighths)[] compositeScore =
      [
        .. kalynaBars1mod, .. kalynaBars2, .. kalynaEnding1, .. kalynaEnding1,
        .. kalynaEnding2, .. kalynaBars2, .. kalynaEnding1
      ];

      // (string pitch, int eighths)[] compositeScore =
      // [
      //   .. kalynaBars1mod
      // ];

      int currentEighth = 0;

      foreach (var (pitchToken, eighths) in compositeScore)
      {
        bool isRest = pitchToken == "-";
        int note = isRest ? -1 : ParsePitch(pitchToken);

        if (!isRest)
        {
          int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(ch, note, leadVelocity));
          if (noteOnResult != 0) break;
        }

        for (int i = 0; i < eighths; i++)
        {
          // Bass drum on quarter-note downbeats
          if (currentEighth % 2 == 0)
          {
            PlayDrumHit(hMidi, 36, 85);
          }
          // Hi-hat on every eighth step
          PlayDrumHit(hMidi, 42, 50);

          if (i == eighths - 1)
          {
            int playMs = (int)(eighthMs * 0.85);
            int restMs = eighthMs - playMs;

            Thread.Sleep(playMs);
            if (!isRest)
            {
              int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOff(ch, note));
              if (noteOnResult != 0) break;
            }
            Thread.Sleep(restMs);
          }
          else
          {
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


  public static void MagicSound()
  {
    int openResult = midiOutOpen(out var hMidi, 0, 0, 0, 0);
    if (openResult != 0)
    {
      Console.WriteLine($"Could not open MIDI device. Error: {openResult}");
      return;
    }

    const int channel = 1; // MIDI channel 2
    string melody = "C6 E6 G6 C7 E7 G7";
    var activeNotes = new List<int>();

    try
    {
      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(channel, 98));
      if (programResult != 0)
      {
        Console.WriteLine($"Magic sound program change failed. Error: {programResult}");
        return;
      }

      foreach (string token in melody.Split(' ', StringSplitOptions.RemoveEmptyEntries))
      {
        var note = ParsePitch(token);
        int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(channel, note, 100));
        if (noteOnResult != 0)
        {
          Console.WriteLine($"Magic sound note-on failed (note {note}). Error: {noteOnResult}");
          break;
        }

        activeNotes.Add(note);
        Thread.Sleep(30);
      }

      // Thread.Sleep(200);
      for (int step = 16; step >= 0; step--)
      {
        int value = step * 127 / 16;
        int noteOnResult = midiOutShortMsg(hMidi, MakeControlChange(channel, 87, value));
        if (noteOnResult != 0)
        {
          Console.WriteLine($"Magic sound note-on fail error: {noteOnResult}");
          break;
        }
        Thread.Sleep(30);
      }
    }
    finally
    {
      foreach (int note in activeNotes)
      {
        int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(channel, note));
        if (noteOffResult != 0)
          Console.WriteLine($"Magic sound note-off failed (note {note}). Error: {noteOffResult}");
      }

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
