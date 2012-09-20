using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace mesoBoard.Common
{
    public class PermissionCollection<T, Q>
        where T : struct, IConvertible
        where Q : PermissionBase<T>
    {
        public List<Q> List { get; set; }

        public PermissionCollection()
        {
        }

        public PermissionCollection(List<Q> items)
        {
            List = items;
        }

        public PermissionCollection(params Q[] items)
        {
            List = new List<Q>();
            List.AddRange(items);
        }

        public Q Get(int value)
        {
            return List.FirstOrDefault(item => item.Value == value);
        }

        public Q Get(string name)
        {
            return List.FirstOrDefault(item => item.Name == name);
        }

        public Q Get(T type)
        {
            return List.FirstOrDefault(item => item.Value == type.ToInt32(CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}