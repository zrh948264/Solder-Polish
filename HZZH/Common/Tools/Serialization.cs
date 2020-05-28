using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 序列化类
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// 私有构造
        /// </summary>
        private Serialization() { }

        /// <summary>
        /// 从文件加载对象(反序列化XML格式为该类类型)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object LoadFromFile(System.Type type, string file)
        {
            object obj = new object();

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);

            if (System.IO.File.Exists(file))
            {
                System.IO.FileStream fs = null;

                try
                {
                    fs = System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                }
                catch
                {
                    obj = type.InvokeMember(null, System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.CreateInstance, null, null, null);
                }

                try
                {
                    obj = xs.Deserialize(fs);
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    obj = type.InvokeMember(null, System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.CreateInstance, null, null, null);
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                try
                {
                    obj = type.InvokeMember(null, System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.CreateInstance, null, null, null);
                    SaveToFile(obj, file);
                }
                catch { }
            }

            return obj;
        }

        /// <summary>
        /// 将对象保存到文件(序列化为XML格式)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        public static void SaveToFile(object obj, string file)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.IO.FileStream fs = System.IO.File.Open(file, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            try
            {
                xs.Serialize(fs, obj);
            }
            catch { }
            finally
            {
                fs.Close();
            }
        }
    }

}
