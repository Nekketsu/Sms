using System.Diagnostics;
using System.Text;

namespace Sms.Cpu
{
    public abstract class Instruction
    {
        private static int instructionCounter = 0;
        private static StringBuilder stringBuilder = new StringBuilder();

        protected Z80 Z80 { get; }

        public abstract uint Cycles { get; }
        public abstract byte[] OpCodes { get; }

        public Instruction(Z80 z80)
        {
            Z80 = z80;
        }

        public uint Execute(byte opCode)
        {
            //Console.WriteLine($"{Z80.Registers.PC - 100:X}: {opCode:X} - {ToString(opCode)}");
            Console.WriteLine($"{instructionCounter++:d4} {Z80.Registers.PC:x4}: {opCode:x2}   {ToString(opCode).PadRight(16)}\t({GetType().Name})");

            if (Z80.Registers.PC == 0x28f4)
            {
                stringBuilder.Clear();
            }

            if (Z80.Registers.PC == 0x28fd)
            {
                stringBuilder.Append((char)Z80.Registers.A);
            }

            if (Z80.Registers.PC == 0x2907)
            {
                Debug.WriteLine($"OutputText: {stringBuilder}");
            }


            Z80.Registers.R = (byte)(Z80.Registers.R % 128);
            Z80.Registers.PC++; // In SMS it actually executes after "ExecuteOpCode",
                                // but doing it here we prevent having to control jmp instructions

            //if (IsTracingEnabled)
            //{
            //    Console.WriteLine(this);
            //}
            InnerExecute(opCode);

            return Cycles;
        }

        protected abstract void InnerExecute(byte opCode);

        public virtual string ToString(byte opCode)
        {
            return GetType().Name;
        }
    }
}
