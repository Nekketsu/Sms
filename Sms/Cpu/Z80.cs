﻿using Sms.Cpu;
using Sms.Memory;

namespace Sms
{
    public class Z80
    {
        public IProgress<TraceData> Trace { get; set; }

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
        private Dictionary<byte, DdCbInstruction> ddCbInstructions;
        private Dictionary<byte, FdCbInstruction> fdCbInstructions;

        public Z80()
        {
            Registers = new Registers();
            Alu = new Alu(this);
            Ports = new Ports();
            State = new State();

            instructions = GetInstructions<Instruction>();
            cbInstructions = GetInstructions<CbInstruction>();
            ddInstructions = GetInstructions<DdInstruction>();
            edInstructions = GetInstructions<EdInstruction>();
            fdInstructions = GetInstructions<FdInstruction>();
            ddCbInstructions = GetInstructions<DdCbInstruction>();
            fdCbInstructions = GetInstructions<FdCbInstruction>();
        }

        public Z80(Mapper memory) : this()
        {
            Memory = memory;
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

            Registers.PC++;
            Registers.R = (byte)((Registers.R + 1) % 128);

            throw new NotImplementedException($"{opCode}");
        }

        public uint ExecuteEdOpCodebyte(byte opCode)
        {
            if (edInstructions.TryGetValue(opCode, out var instruction))
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

        public uint ExecuteDdCbOpCodebyte(byte opCode)
        {
            if (ddCbInstructions.TryGetValue(opCode, out var instruction))
            {
                return instruction.Execute(opCode);
            }

            throw new NotImplementedException($"{opCode}");
        }

        public uint ExecuteFdCbOpCodebyte(byte opCode)
        {
            if (fdCbInstructions.TryGetValue(opCode, out var instruction))
            {
                return instruction.Execute(opCode);
            }

            throw new NotImplementedException($"{opCode}");
        }

        private Dictionary<byte, TInstruction> GetInstructions<TInstruction>() where TInstruction : Instruction
        {
            var instructionType = typeof(TInstruction);

            var test = instructionType
                .Assembly
                .GetTypes()
                .Where(instruction => instruction.BaseType == instructionType && !instruction.IsAbstract)
                .Select(instruction => (TInstruction)Activator.CreateInstance(instruction, this))
                .SelectMany(instruction => instruction.OpCodes.Select(opCode => new { OpCode = opCode, Instruction = instruction }))
                .Where(i => i.OpCode == 0x37)
                .ToArray();

            var instructions = instructionType
                .Assembly
                .GetTypes()
                .Where(instruction => instruction.BaseType == instructionType && !instruction.IsAbstract)
                .Select(instruction => (TInstruction)Activator.CreateInstance(instruction, this))
                .SelectMany(instruction => instruction.OpCodes.Select(opCode => new { OpCode = opCode, Instruction = instruction }))
                .ToDictionary(i => i.OpCode, i => i.Instruction);

            return instructions;
        }

        public ushort Mlt(byte operand1, byte operand2)
        {
            var result = (ushort)(operand1 * operand2);

            return result;
        }
    }
}
