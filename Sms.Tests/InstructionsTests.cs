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

        [Fact]
        public void NoDdInstructionOpCodeRepeated()
        {
            Assert.True(NoRepeated<DdInstruction>());
        }

        [Fact]
        public void NoEdInstructionOpCodeRepeated()
        {
            Assert.True(NoRepeated<EdInstruction>());
        }

        [Fact]
        public void NoFdInstructionOpCodeRepeated()
        {
            Assert.True(NoRepeated<FdInstruction>());
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

            return instructions
                .SelectMany(instruction => instruction.OpCodes.Select(opCode => new { Instruction = instruction, OpCode = opCode }))
                .GroupBy(instruction => instruction.OpCode)
                .All(i => i.Count() == 1);
        }
    }
}
