using Sms;
using Sms.Cpu;
using Sms.Cpu.Tests.Cli;
using Sms.Cpu.Tests.Cli.Json;
using System.Linq;
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

var folder = @"D:\Users\Nekketsu\source\repos\Sms\Sms.Cpu.Tests.Cli\bin\Debug\net7.0\z80\v1";
//var fileNames = Directory.GetFiles("z80/v1").Order();
var fileNames = Directory.GetFiles(folder);

var i = 0;
var totalOkCount = 0;
var totalKoCount = 0;

var notImplemented = new HashSet<string>();
var kos = new HashSet<string>();

var inputOutputNames = new[]
{
    "db", // in a, (n)
    "ed 40", // in b, (c)
    "ed 48", // in c, (c)
    "ed 50", // in d, (c)
    "ed 58", // in e, (c)
    "ed 60", // in h, (c)
    "ed 68", // in l, (c)
    "ed 70", // in (c)
    "ed 74", // testio n
    "ed 78", // in a, (c)
    "ed a2", // cpi
    "ed aa", // ind
    "ed b2", // inir
    "ed b3", // otir
    "ed ba", // indr
    "ed bb", // otdr
};

var failedNames = new[]
{
    "ed 4c", // mlt bc
    "ed 5c", // mlt de
    "ed 64", // tst n
    "ed 6c", // mlt hl
    "ed 7c", // mlt sp
    //"ed b1", // cpir
};

var notImplementedNames = new[]
{
    "dd 00",
    "dd 100",
    "dd 01",
    "dd 02",
    "dd 03",
    "dd 07",
    "dd 08",
    "dd 0a",
    "dd 0b",
    "dd 0f",
    "dd 10",
    "dd 101",
    "dd 11",
    "dd 12",
    "dd 13",
    "dd 17",
    "dd 18",
    "dd 1a",
    "dd 1b",
    "dd 1f",
    "dd 20",
    "dd 27",
    "dd 28",
    "dd 2f",
    "dd 30",
    "dd 31",
    "dd 32",
    "dd 33",
    "dd 37",
    "dd 38",
    "dd 3a",
    "dd 3b",
    "dd 3f",
    "dd 76",
    "dd c0",
    "dd c1",
    "dd c2",
    "dd c3",
    "dd c4",
    "dd c5",
    "dd c6",
    "dd c7",
    "dd c8",
    "dd c9",
    "dd ca",
    "dd cc",
    "dd cd",
    "dd ce",
    "dd cf",
    "dd d0",
    "dd d1",
    "dd d2",
    "dd d3",
    "dd d4",
    "dd d5",
    "dd d6",
    "dd d7",
    "dd d8",
    "dd d9",
    "dd da",
    "dd db",
    "dd dc",
    "dd de",
    "dd df",
    "dd e0",
    "dd e2",
    "dd e4",
    "dd e6",
    "dd e7",
    "dd e8",
    "dd ea",
    "dd eb",
    "dd ec",
    "dd ee",
    "dd ef",
    "dd f0",
    "dd f1",
    "dd f2",
    "dd f3",
    "dd f4",
    "dd f5",
    "dd f6",
    "dd f7",
    "dd f8",
    "dd fa",
    "dd fb",
    "dd fc",
    "dd fe",
    "dd ff",
    "ed 4e",
    "ed 54",
    "ed 55",
    "ed 5d",
    "ed 65",
    "ed 66",
    "ed 6d",
    "ed 6e",
    "ed 75",
    "ed 77",
    "ed 7d",
    "ed 7e",
    "ed 7f",
    "fd 00",
    "fd 01",
    "fd 02",
    "fd 03",
    "fd 07",
    "fd 08",
    "fd 0a",
    "fd 0b",
    "fd 0f",
    "fd 10",
    "fd 100",
    "fd 101",
    "fd 11",
    "fd 12",
    "fd 13",
    "fd 17",
    "fd 18",
    "fd 1a",
    "fd 1b",
    "fd 1f",
    "fd 20",
    "fd 27",
    "fd 28",
    "fd 2f",
    "fd 30",
    "fd 31",
    "fd 32",
    "fd 33",
    "fd 37",
    "fd 38",
    "fd 3a",
    "fd 3b",
    "fd 3f",
    "fd 76",
    "fd c0",
    "fd c1",
    "fd c2",
    "fd c3",
    "fd c4",
    "fd c5",
    "fd c6",
    "fd c7",
    "fd c8",
    "fd c9",
    "fd ca",
    "fd cc",
    "fd cd",
    "fd ce",
    "fd cf",
    "fd d0",
    "fd d1",
    "fd d2",
    "fd d3",
    "fd d4",
    "fd d5",
    "fd d6",
    "fd d7",
    "fd d8",
    "fd d9",
    "fd da",
    "fd db",
    "fd dc",
    "fd de",
    "fd df",
    "fd e0",
    "fd e2",
    "fd e4",
    "fd e6",
    "fd e7",
    "fd e8",
    "fd ea",
    "fd eb",
    "fd ec",
    "fd ee",
    "fd ef",
    "fd f0",
    "fd f1",
    "fd f2",
    "fd f3",
    "fd f4",
    "fd f5",
    "fd f6",
    "fd f7",
    "fd f8",
    "fd fa",
    "fd fb",
    "fd fc",
    "fd fe",
    "fd ff"
};

var notImplementedFileNames = notImplementedNames.Select(f => Path.Combine(folder, $"{f}.json"));
var inputOutputFileNames = inputOutputNames.Select(f => Path.Combine(folder, $"{f}.json"));
var failedFileNames = failedNames.Select(f => Path.Combine(folder, $"{f}.json"));

var correctFileNames = fileNames
    .Except(notImplementedFileNames)
    .Except(inputOutputFileNames)
    .Except(failedFileNames);

Console.WriteLine("Start");
foreach (var fileName in correctFileNames)
{
    try
    {
        var json = await File.ReadAllTextAsync(fileName);
        var tests = JsonSerializer.Deserialize<Z80Test[]>(json, options)!;

        var okCount = 0;
        var koCount = 0;
        var testCount = 0;
        foreach (var test in tests)
        {
            bool isOk;
            SetInitialState(z80, test.Initial);
            z80.ExecuteNextInstruction();
            isOk = CheckFinalState(z80, test.Final);

            var resultString = isOk ? "[OK]" : "[KO]";

            if (isOk)
            {
                okCount++;
            }
            else
            {
                Console.WriteLine($"Test: {testCount}");
                koCount++;
                kos.Add(Path.GetFileNameWithoutExtension(fileName));
            }

            i++;
            testCount++;
        }

        summary.AppendLine($"Summary {fileName}: OK = {okCount}, KO = {koCount}");

        totalOkCount += okCount;
        totalKoCount += koCount;

        //if (koCount > 0)
        //{
        //    break;
        //}
    }
    catch
    {
        kos.Add(Path.GetFileNameWithoutExtension(fileName));
    }
}

//Console.WriteLine(summary);
Console.WriteLine();
Console.WriteLine($"Total OK:  {totalOkCount}, Total KO: {totalKoCount}");
if (kos.Any())
{
    Console.WriteLine($"KOS {kos.Count}: {string.Join(", ", kos)}");
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

    if (!conditions.All(c => c))
    {
        Console.WriteLine($"PC: ({z80.Registers.PC}, {final.PC}), Flags: ({z80.Registers.F & ~unusedFlags}, {((Registers.Flags)final.F) & ~unusedFlags})");
    }

    return conditions.All(c => c);
}

void Progress_ProgressChanged(object? sender, TraceData e)
{
    lastInstruction = $"{e.PC:x4}: {e.OpCode:x2}    {e.Instruction.PadRight(16)}\t({e.InstructionName})";
    lastMnemonic = e.InstructionName;
    lastOpCode = e.OpCode;
}