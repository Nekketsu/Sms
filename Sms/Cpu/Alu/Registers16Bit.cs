using System.Linq.Expressions;
using System.Reflection;

namespace Sms.Cpu
{
    public class Registers16Bit
    {
        public int[] Indices { get; } = { 0b00, 0b01, 0b10, 0b11 };

        Registers registers;
        Dictionary<int, Expression<Func<Registers, ushort>>> registerPointers;

        public Registers16Bit(Registers registers)
        {
            this.registers = registers;

            registerPointers = new Dictionary<int, Expression<Func<Registers, ushort>>>
            {
                [0b00] = r => r.BC,
                [0b01] = r => r.DE,
                [0b10] = r => r.HL,
                [0b11] = r => r.SP,
            };
        }

        public ushort this[int index]
        {
            get
            {
                var expression = (MemberExpression)registerPointers[index].Body;
                var property = (PropertyInfo)expression.Member;

                return (ushort)property.GetValue(registers);
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
