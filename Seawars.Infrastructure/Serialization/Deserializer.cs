using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Infrastructure.Serialization
{
    public static class Deserializer
    {
        public static T Deserialize<T>(string content) where T : class
        {
            var @class = (T)typeof(T).GetConstructor(new Type[0]).Invoke(new object[0]);
            var properties = TypeDescriptor.GetProperties(@class);
            var Type = @class.GetType();
            var Fields = Type.GetProperties();

            var JsonToarray = content
                .Replace("{", "")
                .Replace("}", "")
                .Split(',');

            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                for (int j = 0; j < properties.Count; j++)
                {
                    var str = JsonToarray[i].Split(':');
                    if (str[0].ToString().Contains(Fields[j].Name.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        property.SetValue(@class, Convert.ChangeType(str[1], property.PropertyType));
                        break;
                    }
                }
            }

            return @class;
        }
    }
}
