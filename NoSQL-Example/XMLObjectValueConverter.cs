using System;
using System.IO;
using System.Xml.Serialization;

namespace NoSQL_Example
{
    public class XMLObjectValueConverter : IObjectValueConverter
    {

        public String objectAsString(Object obj, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, obj);
            String value = sw.ToString();
            return value;
        }

        public Object stringAsObject(String value, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            return xs.Deserialize(new StringReader(value));
        }

    }
}
