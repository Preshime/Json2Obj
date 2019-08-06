using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialize
{
    class Program
    {
        static void Main(string[] args)
        {
            MyObj myObj = new MyObj()
            {
                MyInt = -1,
                MyBool = true,
                MyString = "第一层",
                //Obj = new MyObj()
                //{
                //    MyString = "obj"
                //},
                // ListInt = new List<int>(new int[] { 1, 2, 3 }),
                MObj = new MyObj()
                {
                    MyInt = 2,
                    MyString = "第二层",
                    Date = DateTime.Now,
                    MObj = new MyObj()
                    {
                        MyString = "第三层",
                        //ListInt = new List<int>(new int[] { 1, 2 })
                    }
                },
                Dic = new Dictionary<string, string>(),
                RDic = new Dictionary<string, int>(),
            };
            myObj.Dic.Add("ss", "qwe");
            myObj.Dic.Add("ss1", "qwe");
            myObj.RDic.Add("ss", 1);
            myObj.RDic.Add("ss1", 2);
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string json = "";
            List<MyObj> ll = new List<MyObj>();
            ll.Add(myObj);
            ll.Add(myObj);
            Serialize.ToJson(ll, out json);
            string xml = "";
            //Serialize.ToXml(myObj, out xml);
            // Console.WriteLine(json);
            Console.WriteLine("-----------------------------------------");

            //MyObj m1, m2, m3;
            ////Serialize.ToObj(json, DataType.Json, out m1);
            ////m1.ShowValue();
            ////Console.ReadKey();
            ////Serialize.ToObj(xml, DataType.XML, out m2);
            ////m2.ShowValue();
            //Console.ReadKey();
            //Console.WriteLine();
            //byte[] bs/* = new byte[0]*/;
            //Serialize.ToSerialize(myObj, DataType.Binary, out bs);
            //Serialize.ToObj(bs, DataType.Binary, out m3);
            //m3.ShowValue();
            //string s;
            string s = "[{\"DateLong\":0,\"Dic\":[{\"ss\":\"qwe\"},{\"ss1\":\"qwe\"}]" +
                ",\"MObj\":{\"DateLong\":1565087225578,\"Dic\":null,\"MObj\":{\"DateLong\":0,\"Dic\":null,\"MObj\":null,\"MyBool\":false,\"MyInt\":0,\"MyString\":\"第三层\",\"RDic\":null},\"MyBool\":false,\"MyInt\":2,\"MyString\":\"第二层\",\"RDic\":null}," +
                "\"MyBool\":true,\"MyInt\":-1,\"MyString\":\"第一层\",\"RDic\":[{\"ss\":1},{\"ss1\":2}]},{\"DateLong\":0,\"Dic\":[{\"ss\":\"qwe\"},{\"ss1\":\"qwe\"}],\"MObj\":{\"DateLong\":1565087225578,\"Dic\":null,\"MObj\":" +
                "{\"DateLong\":0,\"Dic\":null,\"MObj\":null,\"MyBool\":false,\"MyInt\":0,\"MyString\":\"第三层\",\"RDic\":null},\"MyBool\":false,\"MyInt\":2,\"MyString\":\"第二层\",\"RDic\":null},\"MyBool\":true,\"MyInt\":-1,\"MyString\":\"第一层\"," +
                "\"RDic\":[{\"ss\":1},{\"ss1\":2}]}]";
            string ss = "{\"DateLong\":0,\"Dic\":[{\"ss\":\"qwe\"},{\"ss1\":\"qwe\"}]," +
                "\"MObj\":{\"DateLong\":1565087225578,\"Dic\":null,\"MObj\":" +
                "{\"DateLong\":0,\"Dic\":null,\"MObj\":null,\"MyBool\":false,\"MyInt\":0," +
                "\"MyString\":\"第三层\",\"RDic\":null},\"MyBool\":false,\"MyInt\":2,\"MyString\":\"第二层\"," +
                "\"RDic\":null},\"MyBool\":true,\"MyInt\":-1,\"MyString\":\"第一层\",\"RDic\":[{\"ss\":1},{\"ss1\":2}]}";
            Console.ReadKey();
            List<MyObj> m = (List<MyObj>)Json2Obj.Json2Ob<MyObj, object>(s);
            MyObj mm = (MyObj)Json2Obj.Json2Ob<MyObj, object>(ss);
            m?[0].ShowValue();
            Console.ReadKey();
        }

    }

    public enum DataType
    {
        Json = 1,
        XML = 2,
        Binary = 3
    }
}
