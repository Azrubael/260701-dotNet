using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;

class Program
{
    [LibraryImport("winmm.dll")]
    private static partial int MidiOutOpen(out IntPtr hmo, int uDeviceID, int dwCallback, int dwInstance, int dwFlags);

    [LibraryImport("winmm.dll")]
    private static partial int MidiOutShortMsg(IntPtr hmo, int dwMsg);

    [LibraryImport("winmm.dll")]
    private static partial int MidiOutClose(IntPtr hmo);

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

            var melody = new (int note, int ms, int vel)[]
            {
                (72, 300, 88), (74, 300, 84), (76, 450, 86), (74, 250, 80),
                (72, 350, 84), (69, 300, 82), (71, 500, 86),

                (72, 300, 88), (74, 300, 84), (76, 300, 86), (77, 300, 84),
                (76, 450, 86), (74, 250, 80), (72, 500, 84),

                (69, 300, 82), (71, 300, 84), (72, 600, 90),
                (71, 250, 80), (69, 250, 80), (67, 700, 86)
            };

            foreach (var (note, ms, vel) in melody)
            {
                int noteOnResult = midiOutShortMsg(hMidi, MakeNoteOn(ch, note, vel));
                if (noteOnResult != 0)
                {
                    Console.WriteLine($"Note on failed. Error: {noteOnResult}");
                    break;
                }

                Thread.Sleep(ms);

                int noteOffResult = midiOutShortMsg(hMidi, MakeNoteOff(ch, note));
                if (noteOffResult != 0)
                {
                    Console.WriteLine($"Note off failed. Error: {noteOffResult}");
                    break;
                }

                Thread.Sleep(30);
            }
        }
        finally
        {
            midiOutClose(hMidi);
        }
    }
}
