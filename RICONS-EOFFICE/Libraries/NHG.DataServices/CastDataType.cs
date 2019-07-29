using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using NHG.Core.Functions;

namespace NHG.DataServices
{
    public class CastDataType
    {
        [Description("Cast Data from IList to Generic List which properties type only string. ")]
        public List<T> CastDataToList<T>(IList iList) where T : new()
        {
            List<T> list = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (Hashtable iItem in iList)
            {
                var item = new T();
                foreach (var prop in props)
                {
                    prop.SetValue(item, iItem[prop.Name.ToUpper()] == null ? DBNull.Value.ToString() : iItem[prop.Name.ToUpper()].ToString(), null);
                }
                list.Add(item);
            }
            return list;
        }

        [Description("Cast Data from IList to Generic List which properties have multiple types. ")]
        public List<T> AdvanceCastDataToList<T>(IList iList) where T : new()
        {
            List<T> list = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (Hashtable iItem in iList)
            {
                var item = new T();
                foreach (var prop in props)
                {
                    prop.SetValue(item, Functions.ConvertValue(iItem[prop.Name.ToLower()], prop.PropertyType.Name), null);
                }
                list.Add(item);
            }
            return list;
        }

        [Description("Cast Data from IList to Generic List which properties have multiple types. ")]
        public T AdvanceCastDataToModel<T>(Hashtable inputObject) where T : new()
        {
            T result = new T();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Hashtable iItem = inputObject;
            foreach (var prop in props)
            {
                prop.SetValue(result, Functions.ConvertValue(iItem[prop.Name.ToLower()], prop.PropertyType.Name), null);
            }
            return result;
        }

    }
}
