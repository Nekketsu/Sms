using Sms;
using Sms.Cpu;
using Sms.Cpu.Tests.Cli;
using Sms.Cpu.Tests.Cli.Json;
using System.Text;
using System.Text.Json;

var size = 65536;
var mapper = new TestMapper(size);
var z80 = new Z80(mapper);

var progress = new Progress<TraceData>();
progress.ProgressChanged += Progress_ProgressChanged;
z80.Trace = progress;


var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
};


var summary = new StringBuilder();
var lastInstruction = string.Empty;
var lastMnemonic = string.Empty;
var lastOpCode = 0;

var fileNames = Directory.GetFiles("z80/v1").Order();

var i = 0;
var totalOkCount = 0;
var totalKoCount = 0;
var firstKo = default(string);

var notImplemented = new HashSet<string>();
var failedInstructions = new HashSet<string>();
var kos = new HashSet<string>();

var failedFileNames = new[]
{
    //"z80/v1\\db.json", // IN_A__n_
    //"z80/v1\\ed 40.json", // IN_r__C_
    //"z80/v1\\ed 48.json", // IN_r__C_
    //"z80/v1\\ed 50.json", // IN_r__C_
    //"z80/v1\\ed 58.json", // IN_r__C_
    //"z80/v1\\ed 60.json", // IN_r__C_
    //"z80/v1\\ed 68.json", // IN_r__C_
    //"z80/v1\\ed 78.json", // IN_r__C_
    //"z80/v1\\ed a2.json", // INI
    //"z80/v1\\ed aa.json", // IND
    //"z80/v1\\ed ab.json", // OUTD, OK
    //"z80/v1\\ed b1.json", // CPIR, OK
    //"z80/v1\\ed b2.json", // INIR
    "z80/v1\\ed b3.json", // OTIR
    //"z80/v1\\ed ba.json", // INDR
    "z80/v1\\ed bb.json", // OTDR
};

foreach (var fileName in fileNames)
{
    var json = await File.ReadAllTextAsync(fileName);
    var tests = JsonSerializer.Deserialize<Z80Test[]>(json, options)!;

    var okCount = 0;
    var koCount = 0;
    try
    {
        foreach (var test in tests.Take(1))
        {
            bool isOk;
            SetInitialState(z80, test.Initial);
            try
            {
                z80.ExecuteNextInstruction();
                isOk = CheckFinalState(z80, test.Final);
            }
            catch (NotImplementedException)
            {
                throw;
            }
            catch
            {
                isOk = false;
            }
            var resultString = isOk ? "[OK]" : "[KO]";

            //if (!isOk)
            //{
            Console.WriteLine($"{resultString} {i}: {lastInstruction}");
            //}
            if (isOk)
            {
                okCount++;
            }
            else
            {
                koCount++;
                kos.Add(fileName);
                if (firstKo is null)
                {
                    firstKo = fileName;
                }
                failedInstructions.Add(lastMnemonic);
            }

            i++;
        }
    }
    catch (NotImplementedException)
    {
        notImplemented.Add(Path.GetFileNameWithoutExtension(fileName));
    }

    summary.AppendLine($"Summary {fileName}: OK = {okCount}, KO = {koCount}");

    totalOkCount += okCount;
    totalKoCount += koCount;

    //if (koCount > 0)
    //{
    //    break;
    //}
}

Console.WriteLine(summary);
Console.WriteLine();
Console.WriteLine($"Total OK:  {totalOkCount}, Total KO: {totalKoCount}");
if (failedInstructions.Any())
{
    Console.WriteLine($"KOS {kos.Count}: {string.Join(", ", kos)}");
    Console.WriteLine($"KO {failedInstructions.Count}: {string.Join(", ", failedInstructions.Order())}");
}
if (notImplemented.Any())
{
    Console.WriteLine($"Not implemented {notImplemented.Count}: {string.Join(", ", notImplemented.Order())}");
}

void SetInitialState(Z80 z80, Initial initial)
{
    SetInitialCpuState(z80, initial);
    SetInitialRamState(z80, initial);
}

bool CheckFinalState(Z80 z80, Final final)
{
    var ramCheck = CheckFinalRamState(z80, final);
    var cpuCheck = CheckFinalCpuState(z80, final);

    return ramCheck && cpuCheck;
}

void SetInitialCpuState(Z80 z80, Initial initial)
{
    z80.Registers.PC = initial.PC;
    z80.Registers.SP = initial.SP;
    z80.Registers.A = initial.A;
    z80.Registers.B = initial.B;
    z80.Registers.C = initial.C;
    z80.Registers.D = initial.D;
    z80.Registers.E = initial.E;
    z80.Registers.F = (Registers.Flags)initial.F;
    z80.Registers.H = initial.H;
    z80.Registers.L = initial.L;
    z80.Registers.I = initial.I;
    z80.Registers.R = initial.R;

    z80.Registers.IX = initial.IX;
    z80.Registers.IY = initial.IY;

    z80.Registers.AFShadow = initial.AF_;
    z80.Registers.BCShadow = initial.BC_;
    z80.Registers.DEShadow = initial.DE_;
    z80.Registers.HLShadow = initial.HL_;
    z80.Registers.IFF1 = initial.IFF1 == 1;
    z80.Registers.IFF2 = initial.IFF2 == 1;
}

void SetInitialRamState(Z80 z80, Initial initial)
{
    mapper.Reset();

    foreach (var entry in initial.Ram)
    {
        var address = (ushort)entry[0];
        var value = (byte)entry[1];

        z80.Memory[address] = value;
    }
}

bool CheckFinalRamState(Z80 z80, Final final)
{
    foreach (var entry in final.Ram)
    {
        var address = (ushort)entry[0];
        var value = (byte)entry[1];

        if (z80.Memory[address] != value)
        {
            return false;
        }
    }

    return true;
}

bool CheckFinalCpuState(Z80 z80, Final final)
{
    var unusedFlags = Registers.Flags.X1 | Registers.Flags.X2;

    var conditions = new bool[]
    {
        z80.Registers.A == final.A,
        z80.Registers.B == final.B,
        z80.Registers.C == final.C,
        z80.Registers.D == final.D,
        z80.Registers.E == final.E,
        (z80.Registers.F & ~unusedFlags) == (((Registers.Flags)final.F) & ~unusedFlags),
        z80.Registers.H == final.H,
        z80.Registers.L == final.L,
        z80.Registers.I == final.I,
        z80.Registers.R == final.R,

        z80.Registers.AFShadow == final.AF_,
        z80.Registers.BCShadow == final.BC_,
        z80.Registers.DEShadow == final.DE_,
        z80.Registers.HLShadow == final.HL_,

        z80.Registers.IX == final.IX,
        z80.Registers.IY == final.IY,
        z80.Registers.PC == final.PC,
        z80.Registers.SP == final.SP,

        z80.Registers.IFF1 == (final.IFF1 == 1),
        z80.Registers.IFF2 == (final.IFF2 == 1)
    };

    return conditions.All(c => c);
}

void Progress_ProgressChanged(object? sender, TraceData e)
{
    lastInstruction = $"{e.PC:x4}: {e.OpCode:x2}    {e.Instruction.PadRight(16)}\t({e.InstructionName})";
    lastMnemonic = e.InstructionName;
    lastOpCode = e.OpCode;
}