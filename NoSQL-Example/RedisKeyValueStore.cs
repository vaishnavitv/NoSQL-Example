using StackExchange.Redis;
using System;

namespace NoSQL_Example
{
    public class RedisKeyValueStore : IKeyValueStore
    {
        public RedisKeyValueStore(String dbServerName, IObjectValueConverter converter)
        {
            this.db = ConnectionMultiplexer.Connect(dbServerName).GetDatabase(0);
            this.converter = converter;
        }

        public T getObjectFromDatabase<T>(String uniqId) where T : IIdentifiableObject
        {
            String key = typeof(T).Name + "-" + uniqId;
            String value = db.StringGet(key);
            return (T)converter.stringAsObject(value, typeof(T));
        }

        public void saveObjectToDatabase<T>(T obj) where T : IIdentifiableObject
        {
            String key = typeof(T).Name + "-" + obj.ID;
            String value = converter.objectAsString(obj, typeof(T));
            db.StringSet(key, value);
        }

        IObjectValueConverter converter;
        IDatabase db;
    }
}
