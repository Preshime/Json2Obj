using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Serialize
{
    public class Serialize
    {
        public bool Save<T>(string name, string path, DataType type, T obj)
        {
            string fileEnd = "";
            switch (type)
            {
                case DataType.Binary:
                    fileEnd = ".byte";
                    break;
                case DataType.Json:
                    fileEnd = ".json";
                    break;
                case DataType.XML:
                    fileEnd = ".xml";
                    break;
            }
            //Console.WriteLine(path);
            FileStream fs = new FileStream(path + name + fileEnd, FileMode.OpenOrCreate);


            return false;
        }

        public bool Load<T>(string name, string path, DataType type, out T obj)
        {
            throw new NotImplementedException();
        }

        public static bool ToJson<T>(T obj, out string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, obj);
            json = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine(json);
            ms.Close();
            return !string.IsNullOrEmpty(json);
        }

        public static bool ToXml<T>(T obj, out string xml)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, obj);
            xml = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine(xml);
            ms.Close();
            return !string.IsNullOrEmpty(xml);
        }

        public static bool ToBinary<T>(T obj, out byte[] bytes)
        {
            MemoryStream ms = new MemoryStream();
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            bytes = ms.ToArray();
            return false;
        }

        public static bool ToBinarySelf<T>(T obj, out byte[] bytes)
        {



            bytes = null;
            return false;
        }


        public static bool ToSerialize<T>(T obj, DataType type, out byte[] bytes)
        {
            MemoryStream ms = new MemoryStream();
            if (type != DataType.Binary)
            {
                XmlObjectSerializer ser;
                switch (type)
                {
                    case DataType.Json:
                        ser = new DataContractJsonSerializer(typeof(T));
                        break;
                    case DataType.XML:
                        ser = new DataContractSerializer(typeof(T));
                        break;
                    default:
                        ser = new DataContractSerializer(typeof(T));
                        break;
                }
                ser.WriteObject(ms, obj);
                bytes = ms.ToArray();
                ms.Close();
                return bytes != null && bytes.Length > 0;
            }
            else
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                bytes = ms.ToArray();
                ms.Close();
                return bytes != null && bytes.Length > 0;
            }
        }


        public static bool ToObj<T>(byte[] bytes, DataType type, out T obj)
        {
            if (type != DataType.Binary)
            {
                XmlObjectSerializer ser;
                switch (type)
                {
                    case DataType.XML:
                        ser = new DataContractSerializer(typeof(T));
                        break;
                    case DataType.Json:
                        ser = new DataContractJsonSerializer(typeof(T));
                        break;
                    default:
                        ser = new DataContractSerializer(typeof(T));
                        break;
                }
                MemoryStream ms = new MemoryStream(bytes);
                obj = (T)ser.ReadObject(ms);
                ms.Close();
                return true;
            }
            else if (type == DataType.Binary && bytes != null)
            {
                IFormatter iFormatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(bytes);
                obj = (T)iFormatter.Deserialize(ms);
                ms.Close();
                return true;
            }
            obj = default(T);
            return false;
        }
    }
}
