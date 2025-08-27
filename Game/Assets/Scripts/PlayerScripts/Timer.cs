using System;

namespace PlayerScripts
{
    public enum Timer
    {
        Dodge,
        Parry
    }
    public class EnumArray<TEnum, TValue> where TEnum : Enum
    {
        private readonly TValue[] _values;

        public EnumArray()
        {
            _values = new TValue[Enum.GetValues(typeof(TEnum)).Length];
        }

        public TValue this[TEnum key]
        {
            get => _values[Convert.ToInt32(key)];
            set => _values[Convert.ToInt32(key)] = value;
        }
    }
}