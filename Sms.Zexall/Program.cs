using Sms;
using Sms.Cpu;
using Sms.Zexall;

var fileName = "Roms/zexall.sms";
var data = await File.ReadAllBytesAsync(fileName);
var mapper = new CpmMapper(data);

var isTracingEnabled = false;
var trace = new Progress<TraceData>();
trace.ProgressChanged += Trace_ProgressChanged;

var z80 = new Z80(mapper);
z80.Trace = trace;

var keyboardJoypad = new KeyboardJoyPad();
z80.Ports.MapPorts(keyboardJoypad);

var vpd = new TMS9918A();
z80.Ports.MapPorts(vpd);

var sdscPorts = new SdscPorts();
z80.Ports.MapPorts(sdscPorts);

//z80.Registers.PC = mapper.StartingAddress;

while (true)
{
    z80.ExecuteNextInstruction();
    while (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Spacebar)
        {
            isTracingEnabled = !isTracingEnabled;
        }
    }

    //keyboardJoypad.Update();
}

void Trace_ProgressChanged(object? sender, TraceData e)
{
    if (isTracingEnabled)
    {
        Console.WriteLine($"{e.PC:x4}: {e.OpCode:x2}    {e.Instruction.PadRight(16)}\t({e.InstructionName})");
    }
}