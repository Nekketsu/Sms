namespace Sms.Cpu
{
    public class TraceData
    {
        public ushort PC { get; set; }
        public byte OpCode { get; set; }
        public string Instruction { get; set; }
        public string InstructionName { get; set; }
    }
}
