using Sms.Cpu;
using Sms.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sms
{
    public class Z80
    {
        public Alu Alu { get; }
        public Mapper Memory { get; }
        public Registers Registers { get; }
        public Ports Ports { get; }
        public State State { get; }

        private Dictionary<byte, Instruction> instructions;
        private Dictionary<byte, CbInstruction> cbInstructions;
        private Dictionary<byte, DdInstruction> ddInstructions;
        private Dictionary<byte, EdInstruction> edInstructions;
        private Dictionary<byte, FdInstruction> fdInstructions;

        public Z80()
        {
            Alu = new Alu(this);
            Registers = new Registers();
            Ports = new Ports();
            State = new State();

            instructions = GetInstructions<Instruction>();
            cbInstructions = GetInstructions<CbInstruction>();
            ddInstructions = GetInstructions<DdInstruction>();
            edInstructions = GetInstructions<EdInstruction>();
            fdInstructions = GetInstructions<FdInstruction>();

        }

        public uint ExecuteNextInstruction()
        {
            byte opCode = Memory[Registers.PC];

            return ExecuteOpCode(opCode);
        }

        private uint ExecuteOpCode(byte opCode)
        {
            if (instructions.TryGetValue(opCode, out var instruction))
            {
                return instruction.Execute(opCode);
            }

            throw new NotImplementedException($"{opCode}");
        }

        public uint ExecuteCbOpCodebyte(byte opCode)
        {
            if (cbInstructions.TryGetValue(opCode, out var instruction))
            {
                return instruction.Execute(opCode);
            }

            throw new NotImplementedException($"{opCode}");
        }

        public uint ExecuteDdOpCodebyte(byte opCode)
        {
            if (ddInstructions.TryGetValue(opCode, out var instruction))
            {
                return instruction.Execute(opCode);
            }

            throw new NotImplementedException($"{opCode}");
        }

        public uint ExecuteFdOpCodebyte(byte opCode)
        {
            if (fdInstructions.TryGetValue(opCode, out var instruction))
            {
                return instruction.Execute(opCode);
            }

            throw new NotImplementedException($"{opCode}");
        }

        private Dictionary<byte, TInstruction> GetInstructions<TInstruction>() where TInstruction : Instruction
        {
            var instructionType = typeof(TInstruction);

            var instructions = instructionType
                .Assembly
                .GetTypes()
                .Where(instruction => instruction.BaseType == instructionType && !instruction.IsAbstract)
                .Select(instruction => (TInstruction)Activator.CreateInstance(instruction, this))
                .SelectMany(instruction => instruction.OpCodes.Select(opCode => new { OpCode = opCode, Instruction = instruction }))
                .ToDictionary(i => i.OpCode, i => i.Instruction);

            return instructions;
        }
    }
}
