namespace Sms.Cpu.Tests.Cli.Json;

public class Z80Test
{
    public string Name { get; set; }
    public Initial Initial { get; set; }
    public Final Final { get; set; }
    public object[][] Cycles { get; set; }
}

public class Initial
{
    public ushort PC { get; set; }
    public ushort SP { get; set; }
    public byte A { get; set; }
    public byte B { get; set; }
    public byte C { get; set; }
    public byte D { get; set; }
    public byte E { get; set; }
    public byte F { get; set; }
    public byte H { get; set; }
    public byte L { get; set; }
    public byte I { get; set; }
    public byte R { get; set; }
    //public ushort EI { get; set; }
    //public ushort WZ { get; set; }
    public ushort IX { get; set; }
    public ushort IY { get; set; }
    public ushort AF_ { get; set; }
    public ushort BC_ { get; set; }
    public ushort DE_ { get; set; }
    public ushort HL_ { get; set; }
    //public ushort IM { get; set; }
    //public int P { get; set; }
    //public int Q { get; set; }
    public byte IFF1 { get; set; }
    public byte IFF2 { get; set; }
    public int[][] Ram { get; set; }
}

public class Final
{
    public byte A { get; set; }
    public byte B { get; set; }
    public byte C { get; set; }
    public byte D { get; set; }
    public byte E { get; set; }
    public byte F { get; set; }
    public byte H { get; set; }
    public byte L { get; set; }
    public byte I { get; set; }
    public byte R { get; set; }
    public ushort AF_ { get; set; }
    public ushort BC_ { get; set; }
    public ushort DE_ { get; set; }
    public ushort HL_ { get; set; }
    public ushort IX { get; set; }
    public ushort IY { get; set; }
    public ushort PC { get; set; }
    public ushort SP { get; set; }
    //public ushort WZ { get; set; }
    public byte IFF1 { get; set; }
    public byte IFF2 { get; set; }
    //public int IM { get; set; }
    //public int EI { get; set; }
    //public int P { get; set; }
    //public int Q { get; set; }
    public int[][] Ram { get; set; }
}
