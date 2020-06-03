using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Sms.Cpu
{
    public class Registers8Bit
    {
        public int[] Indices { get; } = { 0b111, 0b000, 0b001, 0b010, 0b011, 0b100, 0b101 };

        Registers registers;
        Dictionary<int, Expression<Func<Registers, byte>>> registerPointers;

        public Registers8Bit(Registers registers)
        {
            this.registers = registers;

            registerPointers = new Dictionary<int, Expression<Func<Registers, byte>>>
            {
                [0b111] = r => r.A,
                [0b000] = r => r.B,
                [0b001] = r => r.C,
                [0b010] = r => r.D,
                [0b011] = r => r.E,
                [0b100] = r => r.H,
                [0b101] = r => r.L
            };
        }

        public byte this[int index]
        {
            get
            {
                var expression = (MemberExpression)registerPointers[index].Body;
                var property = (PropertyInfo)expression.Member;

                return (byte)property.GetValue(registers);
            }

            set
            {
                var expression = (MemberExpression)registerPointers[index].Body;
                var property = (PropertyInfo)expression.Member;

                property.SetValue(registers, value);
            }
        }
    }
}
