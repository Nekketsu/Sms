using Sms.Cpu;
using System;
using System.Linq;
using Xunit;

namespace Sms.Tests
{
    public class InstructionsTests
    {
        [Fact]
        public void NoInstructionOpCodeRepeated()
        {
            Assert.True(NoRepeated<Instruction>());
        }

        [Fact]
        public void NoCbInstructionOpCodeRepeated()
        {
            Assert.True(NoRepeated<CbInstruction>());
        }

        private bool NoRepeated<TInstruction>() where TInstruction : Instruction
        {
            var z80 = new Z80();

            var instructionType = typeof(TInstruction);

            var instructions = instructionType
                .Assembly
                .GetTypes()
                .Where(i => i.BaseType == instructionType && !i.IsAbstract)
                .Select(i => (TInstruction)Activator.CreateInstance(i, z80));

            return instructions.GroupBy(i => i.OpCode).All(i => i.Count() == 1);
        }
    }
}
