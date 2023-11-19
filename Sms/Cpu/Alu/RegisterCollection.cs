using System.Linq.Expressions;
using System.Reflection;

namespace Sms.Cpu
{
    public class RegisterCollection<T>
    {
        public Dictionary<int, string> Names { get; }
        public int[] Indices { get; }

        Registers registers;
        Dictionary<int, Expression<Func<Registers, T>>> registerPointers;

        public RegisterCollection(Registers registers, Dictionary<int, Expression<Func<Registers, T>>> registerPointers)
        {
            this.registers = registers;
            this.registerPointers = new Dictionary<int, Expression<Func<Registers, T>>>(registerPointers);

            Indices = this.registerPointers.Keys
                .ToArray();

            Names = new Dictionary<int, string>(this.registerPointers
                .Select(r => KeyValuePair.Create(r.Key, ((PropertyInfo)((MemberExpression)r.Value.Body).Member).Name.ToLower()))
                .ToArray());
        }

        public T this[int index]
        {
            get
            {
                var expression = (MemberExpression)registerPointers[index].Body;
                var property = (PropertyInfo)expression.Member;

                return (T)property.GetValue(registers);
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
