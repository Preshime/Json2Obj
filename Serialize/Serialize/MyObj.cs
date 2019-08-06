using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Serialize
{
    [DataContract]
    [Serializable]
    class MyObj
    {
        [DataMember]
        public int MyInt { get; set; }

        [DataMember()]
        public string MyString { get; set; }
        [DataMember()]
        public bool MyBool { get; set; }
        //[DataMember]
        //public Object Obj { get; set; }
        [DataMember]
        public MyObj MObj { get; set; }
        //[DataMember]
        //public List<int> ListInt { get; set; }
        [DataMember]
        public Dictionary<string, string> Dic { get; set; }
        [DataMember]
        public Dictionary<string, int> RDic { get; set; }

        [NonSerialized]
        public DateTime Date;

        [DataMember]
        public long DateLong
        {
            get
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                TimeSpan toNow = Date.Subtract(dtStart);
                long timeStamp = toNow.Ticks;
                if (timeStamp > 0)
                {
                    timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
                    return timeStamp;
                }
                return 0;
            }
            set
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(value + "0000");
                TimeSpan toNow = new TimeSpan(lTime);
                Date = dtStart.Add(toNow);
            }
        }

        /// <summary>
        /// console.write方法
        /// </summary>
        public void ShowValue()
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = typeof(MyObj);
                object obj = Activator.CreateInstance(type);
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception e)
            { }
            foreach (PropertyInfo prop in props)
            {
                Console.WriteLine($"{prop.PropertyType} : {prop.GetValue(this)}");
                if (prop.PropertyType == typeof(MyObj))
                {
                    Console.WriteLine();
                    MyObj m = (MyObj)(prop.GetValue(this));
                    m?.ShowValue();
                    Console.WriteLine();
                }
            }
        }
    }
}
