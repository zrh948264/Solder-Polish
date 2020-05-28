using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 外部程式保存
    /// </summary>
    public class CreateProject
    {
        /// <summary>
        /// 保存程式
        /// </summary>
        /// <param name="obj">保存的对象</param>
        /// <param name="file">保存的路径</param>
        public static void SaveProject(object obj, string file)
        {
            try
            {
                if (file == "")
                {
                    return;
                }
                string filePath = "";
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (file.Contains(".pro"))
                    filePath = file;
                else
                    filePath = file + "\\" + fileName + ".pro";
                string path = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, obj);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("项目保存时发生错误 ：\n" + ex.Message);
            }
        }


        /// <summary>
        /// 调用程式
        /// </summary>
        /// <param name="type">调用类型</param>
        /// <param name="file">调用路径</param>
        /// <returns></returns>
        public static object OpenProject(System.Type type, string file)
        {
            object obj = new object();
            BinaryFormatter bf = new BinaryFormatter();
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
                    obj = bf.Deserialize(fs);
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
                    SaveProject(obj, file);
                }
                catch { }
            }

            return obj;
        }
    }

}
