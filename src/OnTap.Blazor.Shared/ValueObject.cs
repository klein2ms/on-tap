using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OnTap.Blazor.Shared
{
    public abstract class ValueObject<T> where T : class
    {
        protected virtual IEnumerable<object> GetEqualityComponents()
            => GetPublicPropertyValues(this);

        public bool Equals(ValueObject<T> obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return GetEqualityComponents()
                .SequenceEqual(obj.GetEqualityComponents());
        }

        public override bool Equals(object obj)
            => Equals(obj as ValueObject<T>);

        public override int GetHashCode()
            => GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
            => !(a == b);

        private static IEnumerable<object> GetPublicPropertyValues(object obj)
            => typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .OrderBy(p => p.Name)
                .Select(p => p.GetValue(obj));
    }
}