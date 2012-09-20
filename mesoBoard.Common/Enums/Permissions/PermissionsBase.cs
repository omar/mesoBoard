using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace mesoBoard.Common
{
    public class PermissionBase<T> where T : struct, IConvertible
    {
        public PermissionBase() { }

        public PermissionBase(T value, string name)
        {
            Name = name;
            Value = value.ToInt32(CultureInfo.InvariantCulture.NumberFormat);
            Type = value;
        }

        public T Type { get; set; }

        public int Value { get; set; }

        public string Name { get; set; }
    }
}