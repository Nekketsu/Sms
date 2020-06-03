namespace Sms.Cpu
{
    public abstract class Instruction
    {
        protected Z80 Z80 { get; }

        public abstract uint Cycles { get; }
        public abstract byte[] OpCodes { get; }

        public Instruction(Z80 z80)
        {
            Z80 = z80;
        }

        public uint Execute(byte opCode)
        {
            Z80.Registers.R = (byte)((Z80.Registers.R + 1) % 128);
            Z80.Registers.PC++; // In SMS it actually executes after "ExecuteOpCode",
                                // but doing it here we prevent having to control jmp instructions

            InnerExecute(opCode);

            return Cycles;
        }

        protected abstract void InnerExecute(byte opCode);
    }
}
