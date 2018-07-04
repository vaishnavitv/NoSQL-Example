using System;

namespace NoSQL_Example
{
    public interface IKeyValueStore
    {
        void saveObjectToDatabase<T>(T obj) where T : IIdentifiableObject;
        T getObjectFromDatabase<T>(String uniqId) where T : IIdentifiableObject;
    }
}
