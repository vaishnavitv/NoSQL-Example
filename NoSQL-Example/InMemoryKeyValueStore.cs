using System;
using System.Collections.Generic;

namespace NoSQL_Example
{
    public class InMemoryKeyValueStore : IKeyValueStore
    {
        public InMemoryKeyValueStore(IObjectValueConverter converter)
        {
            this.converter = converter;
        }

        public T getObjectFromDatabase<T>(string uniqId) where T : IIdentifiableObject
        {
            String key = typeof(T).Name + "-" + uniqId;
            String value = dictionary[key];
            return (T)converter.stringAsObject(value, typeof(T));
        }

        public void saveObjectToDatabase<T>(T obj) where T : IIdentifiableObject
        {
            String key = typeof(T).Name + "-" + obj.ID;
            String value = converter.objectAsString(obj, typeof(T));
            dictionary[key] = value;
        }

        IObjectValueConverter converter;
        Dictionary<String, String> dictionary = new Dictionary<String, String>();

    }
}
