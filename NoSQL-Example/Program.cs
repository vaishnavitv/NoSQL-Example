using System;
using System.Collections.Generic;

namespace NoSQL_Example
{

    public class Student : IIdentifiableObject
    {
        public Student()
        {
        }

        public Student(string id, string name, List<string> courseIDs = null)
        {
            this.ID = id;
            this.Name = name;
            this.CourseIDs = courseIDs;
        }
        public string Name
        {
            get;
            set;
        }
        public string ID
        {
            get; set;
        }

        public List<string> CourseIDs
        {
            get; set;
        }

    }

    public class Course : IIdentifiableObject
    {
        public Course()
        {
        }
        public Course(string id, string name)
        {
            ID = id;
            CourseName = name;
        }
        public string ID
        {
            get; set;
        }
        public string CourseName
        {
            get; set;
        }
    }


    class Program
    {

        private static IKeyValueStore getKVStore(IObjectValueConverter converter)
        {
            //return new InMemoryKeyValueStore(converter);
            //return new RedisKeyValueStore("localhost", converter);
            String azureConnectionString = "BLAH";
            return new AzureTableKeyValueStore(azureConnectionString, "azureKV", converter);
        }

        static void Main(string[] args)
        {
            IKeyValueStore db = getKVStore(new JSONObjectValueConverter());
            db.saveObjectToDatabase<Student>(new Student("1", "Vaishnavi", new List<string>{ "001", "002" }));
            db.saveObjectToDatabase<Student>(new Student("2", "Karthik"));
            db.saveObjectToDatabase<Student>(new Student("3", "Baby", new List<string> { "003" }));

            db.saveObjectToDatabase<Course>(new Course("001", "fr"));
            db.saveObjectToDatabase<Course>(new Course("002", "en"));
            db.saveObjectToDatabase<Course>(new Course("003", "hn"));

            String[] idsToFetch = { "1", "2", "3" };
            foreach (var id in idsToFetch)
            {
                Student obj = db.getObjectFromDatabase<Student>(id);
                Console.WriteLine("Student: " + id + " = " + obj.Name);
                if (obj.CourseIDs != null)
                    foreach (var courseId in obj.CourseIDs)
                    {
                        Console.WriteLine(" \\_ Enrolled in: " + db.getObjectFromDatabase<Course>(courseId).CourseName);
                    }
            }
            Console.ReadLine(); //Pause.
        }

    }
}
