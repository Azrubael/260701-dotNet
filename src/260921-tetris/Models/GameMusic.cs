using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace _260921_tetris.Models;

partial class GameMusic
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


  readonly string KickPattern = "x...x...x...x.x.";
  readonly string SnarePattern = "....x.......x..x";
  readonly string HatPattern = "x.x.x.x.x.x.x.xx";
  readonly string[] ThemeBars =
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

  readonly string[] PassageBars1 =
  [
    "E5 - G5 - A5 - C6 - B5 - A5 - G5 - E5 -",
    "F5 - A5 - C6 - A5 - G5 - E5 - D5 - E5 -",
    "D5 - F5 - A5 - D6 - C6 - A5 - F5 - A5 -",
    "E5 - G#5 - B5 - D6 - C6 - B5 - G#5 - B5 -"
  ];

  readonly string[] PassageBars2 =
  [
    "C5 - E5 G5 - A5 - C6 - D6 - C6 A5 - G5 -",
    "A5 - G5 - E5 - C5 - D5 - E5 G5 - A5 - G5",
    "F5 - A5 C6 - D6 - C6 - A5 - F5 G5 - A5 -",
    "G#5 - B5 - D6 - E6 - D6 C6 - B5 G#5 - C5 -"
  ];


  public void PlayMusic(CancellationToken cancellationToken)
  {
    int openResult = midiOutOpen(out var hMidi, 0, 0, 0, 0);
    if (openResult != 0)
    {
      Console.WriteLine($"Could not open MIDI device. Error: {openResult}");
      return;
    }

    try
    {
      int channel = 0;

      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(channel, 87));
      if (programResult != 0)
      {
        Console.WriteLine($"Program change failed. Error: {programResult}");
        return;
      }

      const int stepMs = 60000 / 38 / 16;

      string[] leadBars = [.. ThemeBars, .. PassageBars1,
                           .. ThemeBars, .. PassageBars2,
                           .. ThemeBars, .. PassageBars1,
                           .. PassageBars2,  .. PassageBars1];

      var melody = new List<(int note, int ms, int vel)>();
      foreach (string bar in leadBars)
      {
        foreach (string token in bar.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
          melody.Add((token == "-" ? -1 : ParsePitch(token), stepMs, 36));
        }
      }
      for (int i = 0; i < melody.Count && !cancellationToken.IsCancellationRequested; i++)
      {
        var (note, ms, vel) = melody[i];
        int step = i % 16;

        if (note >= 0)
        {
          int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(channel, note, vel));
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

        // Thread.Sleep(ms);
        bool cancelled = cancellationToken.WaitHandle.WaitOne(ms);

        if (note >= 0)
        {
          int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(channel, note));
          if (noteOffResult != 0)
            Console.WriteLine($"Note off failed (note {note}). Error: {noteOffResult}");
        }

        if (cancelled)
          break;
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
