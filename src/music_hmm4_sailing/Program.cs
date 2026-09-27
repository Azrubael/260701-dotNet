using System;
using System.Runtime.InteropServices;
using System.Threading;

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

      int programResult = midiOutShortMsg(hMidi, MakeProgramChange(ch, 73));
      if (programResult != 0)
      {
        Console.WriteLine($"Program change failed. Error: {programResult}");
        return;
      }

      (int note, int ms, int vel)[] melody =
      [
        // A
        (74, 333, 88), (77, 333, 84), (81, 333, 86), (79, 333, 84),
        (77, 333, 82), (76, 333, 84), (74, 333, 86), (-1, 333, 0),

        // A'
        (74, 333, 88), (77, 333, 84), (79, 333, 86), (81, 333, 84),
        (79, 333, 82), (77, 333, 84), (76, 333, 82), (-1, 333, 0),

        // A
        (74, 333, 88), (77, 333, 84), (81, 333, 86), (79, 333, 84),
        (77, 333, 82), (76, 333, 84), (74, 333, 86), (-1, 333, 0),

        // A'
        (74, 333, 88), (77, 333, 84), (79, 333, 86), (81, 333, 84),
        (79, 333, 82), (77, 333, 84), (76, 333, 82), (-1, 333, 0),

        // B
        (76, 333, 86), (79, 333, 84), (81, 333, 86), (84, 333, 84),
        (82, 333, 84), (81, 333, 82), (79, 333, 84), (-1, 333, 0),

        // C
        (77, 333, 86), (76, 333, 84), (74, 333, 84), (76, 333, 86),
        (77, 333, 84), (79, 333, 86), (81, 333, 84), (-1, 333, 0),

        // D
        (74, 333, 88), (77, 333, 84), (81, 333, 86), (79, 333, 84),
        (77, 333, 82), (76, 333, 84), (74, 333, 86), (-1, 333, 0),

        // Finale
        (74, 333, 88), (76, 333, 84), (77, 333, 86), (79, 333, 84),
        (81, 333, 86), (82, 333, 84), (81, 666, 88), (-1, 333, 0)
      ];

      foreach (var (note, ms, vel) in melody)
      {
        if (note >= 0)
        {
          int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(ch, note, vel));
          if (noteOnResult != 0) break;

          Thread.Sleep(ms);

          int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(ch, note));
          if (noteOffResult != 0) break;
        }
        else
        {
          Thread.Sleep(ms);
        }
      }
    }
    finally
    {
      midiOutClose(hMidi);
    }
  }
}
