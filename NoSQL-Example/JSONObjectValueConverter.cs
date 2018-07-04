using System;
using System.IO;
using Newtonsoft.Json;

namespace NoSQL_Example
{
    public class JSONObjectValueConverter : IObjectValueConverter
    {
        public String objectAsString(Object obj, Type type)
        {
            JsonSerializer js = new JsonSerializer();
            StringWriter sw = new StringWriter();
            js.Serialize(sw, obj, type);
            String value = sw.ToString();
            return value;
        }

        public Object stringAsObject(String value, Type type)
        {
            JsonSerializer js = new JsonSerializer();
            return js.Deserialize(new StringReader(value), type);
        }
    }
}
