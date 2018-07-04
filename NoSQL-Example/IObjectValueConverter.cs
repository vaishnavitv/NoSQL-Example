using System;

namespace NoSQL_Example
{
    public interface IObjectValueConverter
    {
        String objectAsString(Object obj, Type type);
        Object stringAsObject(String value, Type type);
    }
}
