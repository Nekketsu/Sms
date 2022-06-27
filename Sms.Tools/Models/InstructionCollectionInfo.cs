namespace Sms.Tools.Models
{
    public class InstructionCollectionInfo
    {
        public int ImplementedCount { get; }
        public Instruction[] Instructions { get; }

        public InstructionCollectionInfo(Instruction[] instructions)
        {
            Instructions = instructions;

            ImplementedCount = Instructions.Count(i => i is { });
        }
    }
}
