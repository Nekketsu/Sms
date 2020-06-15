using Prism.Mvvm;
using Sms.Tools.Models;
using Sms.Tools.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sms.Tools.ViewModels
{
    public class CoverageViewModel : BindableBase
    {
        Type[] instructionTypes;
        Dictionary<Type, InstructionCollectionInfo> instructionsByType;
        Dictionary<Type, InstructionCollectionInfo> fullInstructionsByType;
        Type instructionType;

        int total;

        public CoverageViewModel(InstructionService instructionService)
        {
            InstructionTypes = instructionService.GetInstructionTypes();

            var z80 = new Z80();

            InstructionsByType = InstructionTypes
                .Select(instructionType => new { Type = instructionType, Instructions = new InstructionCollectionInfo(instructionService.GetInstructions(instructionType, z80)) }).ToDictionary(i => i.Type, i => i.Instructions);

            Total = InstructionsByType.Aggregate(0, (result, element) => result + element.Value.Instructions.Length);

            FullInstructionsByType = CreateFilledInstructions(InstructionsByType);

            InstructionType = InstructionTypes.FirstOrDefault();
        }

        private Dictionary<Type, InstructionCollectionInfo> CreateFilledInstructions(Dictionary<Type, InstructionCollectionInfo> instructionsByType)
        {
            var fullInstructionsByType = new Dictionary<Type, InstructionCollectionInfo>();

            foreach (var instructions in instructionsByType)
            {
                var fullInstructions = new Instruction[256];
                foreach (var instruction in instructions.Value.Instructions)
                {
                    fullInstructions[instruction.OpCode] = instruction;
                }

                fullInstructionsByType.Add(instructions.Key, new InstructionCollectionInfo(fullInstructions));
            }

            return fullInstructionsByType;
        }

        public Type[] InstructionTypes
        {
            get => instructionTypes;
            set => SetProperty(ref instructionTypes, value);
        }

        public Dictionary<Type, InstructionCollectionInfo> InstructionsByType
        {
            get => instructionsByType;
            set => SetProperty(ref instructionsByType, value);
        }

        public Dictionary<Type, InstructionCollectionInfo> FullInstructionsByType
        {
            get => fullInstructionsByType;
            set => SetProperty(ref fullInstructionsByType, value);
        }

        public Type InstructionType
        {
            get => instructionType;
            set => SetProperty(ref instructionType, value);
        }

        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }
    }
}
