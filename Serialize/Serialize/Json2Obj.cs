using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serialize
{
    class Json2Obj
    {
        public static object Json2Ob<T, TValue>(string json)
        {
            //string s1 = "\"ss\":\"qwe\"}";
            //string asda = "\"ss\":1}";
            //Console.WriteLine(s1.Substring(5) + "--" + asda.Substring(5));



            if (string.IsNullOrEmpty(json))
                return default(T);
            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                //list/map
                //获取第一个片段来判断是map还是list，需要去掉最外围的括号
                //string s;
                //GetAllValueInString(json.Substring(1), out s);

                //string ss;
                //if (s.IndexOf(':') == -1)//不含:,一定是list
                //{
                //    //list
                //    Console.WriteLine("list");
                //    return ListString2List<T>(Json2ListString(json));
                //}
                ////再拆一次s若是整体则是list，否则是map
                //else if (GetAllValueInString(s, out ss) == s.Length - 1)
                //{
                //    Console.WriteLine("list");
                //    return ListString2List<T>(Json2ListString(json));
                //}
                //else
                //{
                //    Console.WriteLine("map");
                //    //DicSS2Dic
                //    return DicSS2Dic<T, TValue>(Json2MapString(json));
                //}
                if (typeof(TValue) != typeof(object))
                {
                    //map
                    return DicSS2Dic<T, TValue>(Json2MapString(json));
                }
                else
                {
                    //list
                    return ListString2List<T>(Json2ListString(json));
                }
            }
            else if (json.StartsWith("{") && json.EndsWith("}"))
            {
                //obj/keyValuePair
                if (typeof(TValue) != typeof(object))
                {

                }
                else
                {
                    Dictionary<string, string> rObjMap = json2ObjMap(json);
                    PropertyInfo[] props = null;
                    try
                    {
                        Type rType = typeof(T);
                        object obj = Activator.CreateInstance(rType);
                        props = rType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (PropertyInfo prop in props)
                        {
                            SetValueByMap(rObjMap, prop, obj);
                        }
                        return (T)obj;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(typeof(T) + "" + typeof(TValue) + rObjMap);
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                return json;
            }
            return default(T);
        }

        private static List<T> ListString2List<T>(List<string> rListString)
        {
            List<T> list = new List<T>();
            foreach (string json in rListString)
            {
                list.Add(SetValue<T>(json));
            }
            return list;
        }

        private static List<string> Json2ListString(string json)
        {
            List<string> list = new List<string>();
            string value = "";
            json = json.Substring(1, json.Length - 2);
            int i = GetAllValueInString(json, out value);
            while (i != -1)
            {
                i = GetAllValueInString(json, out value);
                list.Add(value);
                if (json.Length > i + 2)
                    json = json.Substring(i + 2);
                else break;
            }
            return list;
        }

        private static Dictionary<TKey, TValue> DicSS2Dic<TKey, TValue>(Dictionary<string, string> rDicString)
        {
            Dictionary<TKey, TValue> rDic = new Dictionary<TKey, TValue>();
            foreach (var rPair in rDicString)
            {
                var Pair = SetMapValue<TKey, TValue>(rPair.Key, rPair.Value);
                rDic.Add(Pair.Key, Pair.Value);
            }
            return rDic;
        }


        private static Dictionary<string, string> Json2MapString(string json)
        {
            Dictionary<string, string> rDicString = new Dictionary<string, string>();
            json = json.Substring(1, json.Length - 1);
            string rMapKey = "";
            string rMapValue = "";
            int i = 0;
            List<string> rPair = new List<string>();
            do
            {
                string rPairString = "";
                i = GetAllValueInString(json, out rPairString);
                json = json.Substring(i < json.Length - 1 ? i + 2 : 0);
                if (!string.IsNullOrEmpty(rPairString))
                    rPair.Add(rPairString);
            }
            while (i != -1);
            i = 0;
            for (int j = 0; j < rPair.Count; j++)
            {
                rPair[j] = rPair[j].Substring(1);
                i = GetAllValueInString(rPair[j], out rMapKey);
                rPair[j] = rPair[j].Substring(i + 2);
                i = GetAllValueInString(rPair[j], out rMapValue);
                rDicString.Add(rMapKey, rMapValue);
            }
            return rDicString;
        }

        private static Dictionary<string, string> json2ObjMap(string json)
        {
            Dictionary<string, string> rObjMap = new Dictionary<string, string>();
            string rStringValue = json.Substring(1, json.Length - 1);
            while (rStringValue.IndexOf(':') != -1)
            {
                string rKey = rStringValue.Substring(0, rStringValue.IndexOf(':'));
                rStringValue = rStringValue.Substring(rStringValue.IndexOf(':') + 1);
                string rValue;
                int i = GetAllValueInString(rStringValue, out rValue);
                rStringValue = rStringValue.Substring(i + 2);//加上自身与分隔符','
                Console.WriteLine(rValue);
                rObjMap.Add(rKey.Replace("\"", ""), rValue);
            }
            return rObjMap;
        }

        private static void SetValueByMap(Dictionary<string, string> rDic, PropertyInfo rProp, object obj)
        {
            try
            {

                string value = "";
                if (rProp.PropertyType == typeof(int))
                {
                    rProp.SetValue(obj, int.Parse(rDic[rProp.Name].Replace("\"", "")));
                }
                else if (rProp.PropertyType == typeof(long))
                {
                    rProp.SetValue(obj, long.Parse(rDic[rProp.Name].Replace("\"", "")));
                }
                else if (rProp.PropertyType == typeof(float))
                {
                    rProp.SetValue(obj, float.Parse(rDic[rProp.Name].Replace("\"", "")));
                }
                else if (rProp.PropertyType == typeof(double))
                {
                    rProp.SetValue(obj, double.Parse(rDic[rProp.Name].Replace("\"", "")));
                }
                else if (rProp.PropertyType == typeof(string))
                {
                    rProp.SetValue(obj, rDic[rProp.Name].Replace("\"", ""));
                }
                else if (rProp.PropertyType == typeof(bool))
                {
                    rProp.SetValue(obj, bool.Parse(rDic[rProp.Name].Replace("\"", "")));
                }
                else if (rDic.TryGetValue(rProp.Name, out value) && value == "null")
                {
                    rProp.SetValue(obj, null);
                }
                else if (rProp.PropertyType.IsGenericType)//list 或者map
                {
                    Type[] rEleType = rProp.PropertyType.GenericTypeArguments;
                    //for (int i = 0; i < rEleType.Length; i++)
                    //{
                    //    Console.WriteLine($"-------------------------{rProp.Name}----{i}{rEleType[i]}");

                    //}
                    if (rEleType.Length == 1)
                        rEleType = new Type[] { rEleType[0], typeof(object) };

                    rProp.SetValue(obj, Type2T("Json2Ob", new string[] { rDic[rProp.Name] }, rEleType));
                }
                else
                {
                    // Console.WriteLine(rProp.PropertyType + "," + typeof(int) + "," + (-1).GetType());
                    rProp.SetValue(obj, Type2T("Json2Ob", new string[] { rDic[rProp.Name] }, new Type[] { rProp.PropertyType, typeof(object) }));
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        private static KeyValuePair<T, TValue> SetMapValue<T, TValue>(string key, string value)
        {
            T rKey = default(T);
            TValue rValue = default(TValue);
            rKey = SetValue<T>(key);
            rValue = SetValue<TValue>(value);
            KeyValuePair<T, TValue> rPair = new KeyValuePair<T, TValue>(rKey, rValue);
            return rPair;
        }

        private static T SetValue<T>(string value)
        {
            T t = default(T);
            if (typeof(T) == typeof(int))
            {
                t = (T)(object)int.Parse(value);
            }
            else if (typeof(T) == typeof(string))
            {
                t = (T)(object)value.Replace("\"", "");
            }
            else if (typeof(T).IsGenericType)//list 或者map
            {
                Type[] rEleType = typeof(T).GenericTypeArguments;
                if (rEleType.Length == 1)
                    rEleType = new Type[] { rEleType[0], typeof(object) };

                t = (T)Type2T("Json2Ob", new string[] { value }, rEleType);
            }
            else
            {
                // Console.WriteLine(rProp.PropertyType + "," + typeof(int) + "," + (-1).GetType());
                t = (T)Type2T("Json2Ob", new string[] { value }, new Type[] { typeof(T), typeof(object) });
            }
            return t;
        }

        private static int GetAllValueInString(string s, out string value)
        {
            value = "";
            if (string.IsNullOrEmpty(s))
            {
                value = "";
                return -1;
            }
            if (s.StartsWith("\""))
            {
                int nNext = s.IndexOf("\"", 1);
                value = s.Substring(0, nNext + 1);
                return nNext;
            }
            else if (s.StartsWith("{"))
            {
                int nNextStart = 0;
                int nNextEnd = 0;
                do
                {
                    nNextStart = s.IndexOf('{', nNextStart + 1);
                    nNextEnd = s.IndexOf('}', nNextEnd + 1);
                }
                while (nNextStart < nNextEnd && nNextStart != -1);
                value = s.Substring(0, nNextEnd + 1);
                return nNextEnd;
            }
            else if (s.StartsWith("["))
            {
                int nNextStart = 0;
                int nNextEnd = 0;
                do
                {
                    nNextStart = s.IndexOf('[', nNextStart + 1);
                    nNextEnd = s.IndexOf(']', nNextEnd + 1);
                }
                while (nNextStart < nNextEnd && nNextStart != -1);
                value = s.Substring(0, nNextEnd + 1);
                return nNextEnd;
            }
            else
            {
                int nNext = s.IndexOf(',');
                value = nNext != -1 ? s.Substring(0, nNext) : s;
                if (!(value.StartsWith("{") || value.StartsWith("[")) && (value.EndsWith("}") || value.EndsWith("]")))
                    value = value.Substring(0, value.Length - 1);
                return nNext != -1 ? nNext - 1 : 0;
            }
        }

        private static object Type2T(string method, object[] rParas, Type[] rTypes)
        {
            return typeof(Json2Obj).GetMethod(method).MakeGenericMethod(rTypes).Invoke(new Json2Obj(), rParas);
        }
    }
}
