using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace NoSQL_Example
{
    public class AzureTableKeyValueStore : IKeyValueStore
    {
        public AzureTableKeyValueStore(String azureCredentials, String tableName, IObjectValueConverter converter)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureCredentials);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            this.table = tableClient.GetTableReference(tableName);
            this.table.CreateIfNotExists();
            this.converter = converter;
        }

        public class AzureTableKeyValue : TableEntity
        {
            public AzureTableKeyValue()
            {
            }
            public static String derivePartitionKeyFor(String Key)
            {
                return (Key.GetHashCode() % 16).ToString();
            }

            public AzureTableKeyValue(String Key, String Value)
            {
                this.RowKey = Key;
                this.PartitionKey = derivePartitionKeyFor(Key);
                this.Value = Value;
            }

            public String Value
            {
                get;
                set;
            }

        }

        public T getObjectFromDatabase<T>(String uniqId) where T : IIdentifiableObject
        {
            String key = typeof(T).Name + "-" + uniqId;
            String partitionKey = AzureTableKeyValue.derivePartitionKeyFor(key);
            TableQuery<AzureTableKeyValue> query = new TableQuery<AzureTableKeyValue>().Where(
                TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, key)
                    )
            );
            var results = table.ExecuteQuery<AzureTableKeyValue>(query);
            foreach (AzureTableKeyValue azureKv in results)
            {
                String value = azureKv.Value;
                return (T)converter.stringAsObject(value, typeof(T));
            }
            return default(T);
        }

        public void saveObjectToDatabase<T>(T obj) where T : IIdentifiableObject
        {
            String key = typeof(T).Name + "-" + obj.ID;
            String value = converter.objectAsString(obj, typeof(T));
            var azureKV = new AzureTableKeyValue(key, value);
            table.Execute(TableOperation.InsertOrReplace(azureKV));
        }

        IObjectValueConverter converter;
        CloudTable table;

    }
}
