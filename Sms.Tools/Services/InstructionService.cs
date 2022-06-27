using Sms.Tools.Models;

namespace Sms.Tools.Services
{
    public class InstructionService
    {
        public Instruction[] GetInstructions<TInstruction>() where TInstruction : Cpu.Instruction
        {
            var instructionType = typeof(TInstruction);

            var instructions = instructionType
                .Assembly
                .GetTypes()
                .Where(instruction => instruction.BaseType == instructionType && !instruction.IsAbstract)
                .Select(instruction => (TInstruction)Activator.CreateInstance(instruction, this))
                .SelectMany(instruction => instruction.OpCodes.Select(opCode => new { OpCode = opCode, Instruction = instruction }))
                .Select(instruction => new Instruction { Name = instruction.Instruction.GetType().Name, OpCode = instruction.OpCode })
                .OrderBy(instruction => instruction.OpCode)
                .ToArray();

            return instructions;
        }

        public Instruction[] GetInstructions(Type instructionType, Z80 z80)
        {
            var instructions = instructionType
                .Assembly
                .GetTypes()
                .Where(instruction => instruction.BaseType == instructionType && !instruction.IsAbstract)
                .Select(instruction => (Cpu.Instruction)Activator.CreateInstance(instruction, z80))
                .SelectMany(instruction => instruction.OpCodes.Select(opCode => new { OpCode = opCode, Instruction = instruction }))
                .Select(instruction => new Instruction { Name = instruction.Instruction.GetType().Name, OpCode = instruction.OpCode })
                .OrderBy(instruction => instruction.OpCode)
                .ToArray();

            return instructions;
        }

        public Type[] GetInstructionTypes()
        {
            var instructionType = typeof(Cpu.Instruction);

            var instructions = instructionType
                .Assembly
                .GetTypes()
                .Where(instruction => (instruction.IsAssignableFrom(instructionType) || instruction.BaseType == instructionType) && instruction.IsAbstract)
                .ToArray();

            return instructions;
        }
    }
}
