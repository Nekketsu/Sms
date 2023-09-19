using Sms;
using Sms.Cpu;
using Sms.Zexall;

var data = await File.ReadAllBytesAsync("Roms/zexall.sms");
var mapper = new CpmMapper(data);

var trace = new Progress<TraceData>();
trace.ProgressChanged += Trace_ProgressChanged;

var z80 = new Z80(mapper);
z80.Trace = trace;

var keyboardJoypad = new KeyboardJoyPad();
z80.Ports.MapPorts(keyboardJoypad);

var vpd = new TMS9918A();
z80.Ports.MapPorts(vpd);

var nullPorts = new SdscPorts();
z80.Ports.MapPorts(nullPorts);

//z80.Registers.PC = mapper.StartingAddress;

while (true)
{
    z80.ExecuteNextInstruction();

    keyboardJoypad.Update();
}

void Trace_ProgressChanged(object? sender, TraceData e)
{
    Console.WriteLine($"{e.PC:x4}: {e.OpCode:x2}    {e.Instruction.PadRight(16)}\t({e.InstructionName})");
}