using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sms.Cpu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace Sms.Debugger.ViewModels
{
    [ObservableObject]
    public partial class DebugViewModel
    {
        [ObservableProperty]
        private ObservableCollection<TraceData> trace = new();

        [ObservableProperty]
        private int instructions = 1;

        [ObservableProperty]
        private int programCounter = 0;

        [ObservableProperty]
        private int opCode = 0;

        [ObservableProperty]
        private Z80 z80;

        [ObservableProperty]
        private string[] stack = Array.Empty<string>();

        [ObservableProperty]
        private string[] memory = Array.Empty<string>();

        private TMS9918A vdp;

        public DebugViewModel()
        {
            var zexallFile = "Roms/zexall.sms";
            var zexallData = File.ReadAllBytes(zexallFile);

            var mapper = new CpmMapper(zexallData);

            Z80 = new Z80(mapper);

            var keyboardJoypad = new KeyboardJoyPad();
            Z80.Ports.MapPorts(keyboardJoypad);

            //vdp = new TMS9918A();
            //Z80.Ports.MapPorts(vdp);
            Z80.Ports.MapPorts(new TestVdp());

            var sdscPorts = new SdscPorts();
            Z80.Ports.MapPorts(sdscPorts);

            var progress = new Progress<TraceData>();
            progress.ProgressChanged += Progress_ProgressChanged;

            //Z80.Trace = progress;
        }

        private void Progress_ProgressChanged(object? sender, TraceData traceData)
        {
            Trace.Add(traceData);
        }

        [RelayCommand]
        public void Step()
        {
            for (int i = 0; i < Instructions; i++)
            {
                Z80.ExecuteNextInstruction();
            }

            RefreshStack();
            RefreshMemory();
        }

        [RelayCommand]
        public void RunUntilPc()
        {
            var notImplemented = new HashSet<string>();

            //try
            //{
                while (Z80.Registers.PC != ProgramCounter)
                {
                    var pc = Z80.Registers.PC;
                    try
                    {
                        Z80.ExecuteNextInstruction();
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.ToString());
                        if (e is NotImplementedException)
                        {
                            notImplemented.Add($"{Z80.Memory[pc]:x2} {Z80.Memory[(ushort)(pc + 1)]:x2} {Z80.Memory[(ushort)(pc + 2)]:x2} {Z80.Memory[(ushort)(pc + 3)]:x2}");
                        }
                    }
                }

                MessageBox.Show(string.Join(", ", notImplemented), "Not implemented");

                RefreshStack();
                RefreshMemory();
            //} catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
        }

        [RelayCommand]
        public void RunUntilOpCode()
        {
            try
            {
                while (Z80.Memory[Z80.Registers.PC] != OpCode)
                {
                    Z80.ExecuteNextInstruction();
                }

                RefreshStack();
                RefreshMemory();
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        [RelayCommand]
        public void RunTest()
        {
            const ushort outputTextEnd = 0x2907;
            const int outputsPerTest = 4;

            try
            {
                for (var i = 0; i < outputsPerTest; i++)
                {
                    do
                    {
                        Z80.ExecuteNextInstruction();
                    } while (Z80.Registers.PC != outputTextEnd);

                    Z80.ExecuteNextInstruction();
                }

                RefreshStack();
                RefreshMemory();
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void RefreshStack()
        {
            var minIndex = Math.Max(Z80.Registers.SP - 2, 0);
            var length = 16;
            Stack = Z80.Memory
                .Skip(minIndex)
                .Take(length)
                .Select((e, i) => $"0x{i + minIndex:x4}: 0x{e:x2}")
                .ToArray();
        }

        private void RefreshMemory()
        {
            var minIndex = Math.Max(Z80.Registers.PC - 2, 0);
            var length = 16;
            Memory = Z80.Memory
                .Skip(minIndex)
                .Take(length)
                .Select((e, i) => $"0x{i + minIndex:x4}: 0x{e:x2}")
                .ToArray();
        }
    }
}
