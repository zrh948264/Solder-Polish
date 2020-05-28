using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class Functions
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = true)]
        internal static extern void CopyMemory(int Destination, int Source, int Length);

        /// <summary>
        /// 方法：字节数组转换为整型指针
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static System.IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            System.IntPtr bfIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, bfIntPtr, size);
            return bfIntPtr;
        }


        /// <summary>
        /// 方法：获取枚举值上的Description特性的说明
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetEnumDescription(object obj)
        {
            var type = obj.GetType();
            System.Reflection.FieldInfo field = type.GetField(Enum.GetName(type, obj));
            System.ComponentModel.DescriptionAttribute descAttr =
                Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }

            return descAttr.Description;
        }

        /// <summary>
        /// 方法:十六进制字符串转十进制数
        /// </summary>
        /// <param name="hexstr"></param>
        /// <returns></returns>
        public static double HexToDec(string hexstr)
        {
            double rt = 0;
            string HEXSTR = hexstr.ToUpper();
            for (int i = 0; i < HEXSTR.Length; i++)
            {
                string temp = HEXSTR.Substring(HEXSTR.Length - i - 1, 1);//从右往左，低位到高位
                switch (temp)
                {
                    case "0":
                        rt += Math.Pow(16, i) * 0;
                        break;
                    case "1":
                        rt += Math.Pow(16, i) * 1;
                        break;
                    case "2":
                        rt += Math.Pow(16, i) * 2;
                        break;
                    case "3":
                        rt += Math.Pow(16, i) * 3;
                        break;
                    case "4":
                        rt += Math.Pow(16, i) * 4;
                        break;
                    case "5":
                        rt += Math.Pow(16, i) * 5;
                        break;
                    case "6":
                        rt += Math.Pow(16, i) * 6;
                        break;
                    case "7":
                        rt += Math.Pow(16, i) * 7;
                        break;
                    case "8":
                        rt += Math.Pow(16, i) * 8;
                        break;
                    case "9":
                        rt += Math.Pow(16, i) * 9;
                        break;
                    case "A":
                        rt += Math.Pow(16, i) * 10;
                        break;
                    case "B":
                        rt += Math.Pow(16, i) * 11;
                        break;
                    case "C":
                        rt += Math.Pow(16, i) * 12;
                        break;
                    case "D":
                        rt += Math.Pow(16, i) * 13;
                        break;
                    case "E":
                        rt += Math.Pow(16, i) * 14;
                        break;
                    case "F":
                        rt += Math.Pow(16, i) * 15;
                        break;
                }
            }
            return rt;
        }

        /// <summary>
        /// 方法:数据字符串高低位互换
        /// 注：4个字符作为一个有效数据
        /// </summary>
        /// <param name="str">数据字符串</param>
        /// <returns></returns>
        public static string[] ReverseHighLow(string str)
        {
            string[] rt = null;
            if (!string.IsNullOrEmpty(str))
            {
                int num = str.Length;
                if (num % 4 == 0)                                          //字符串字符个数是4的整数倍
                {
                    string[] tempstr = new string[(int)(num / 4)];
                    int k = 0;
                    for (int i = 0; i < (str.Length); i += 4)              //每四个字符作为一个数据
                    {
                        string H8 = str.Substring(2 + i, 2);               //提取返回字符串指定位置字符（高低位互换）高位
                        string L8 = str.Substring(i, 2);                   //提取返回字符串指定位置字符（高低位互换）低位
                        tempstr[k] = H8 + L8;                              //临时结果字符串
                        k += 1;
                    }

                    rt = tempstr;
                }
            }
            return rt;
        }

        /// <summary>
        /// 方法:字符串添加校验码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AddCheckCode(string str)
        {
            string r = "**";
            try
            {
                byte[] arrbyte = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                byte temp = arrbyte[0];
                for (int i = 1; i < arrbyte.Length; i++)
                {
                    temp ^= arrbyte[i];
                }

                r = Convert.ToString(temp, 16).PadLeft(2, '0').ToUpper();
            }
            catch { }
            return str + r;
        }


        /// <summary>
        /// 方法：获取byte[]中指定起始和长度的字节段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetSubData(byte[] data, int startIndex, int length)
        {
            byte[] ret = new byte[length];
            System.Array.Copy(data, startIndex, ret, 0, length);
            return ret;
        }

        /// <summary>
        /// 根据对话框指定的目录加载所有文件
        /// </summary>
        /// <returns></returns>
        public static System.IO.FileInfo[] GetFilesFromFolderWithDialog()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = fbd.SelectedPath;
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(folderPath);
                if (dirInfo.GetFiles().Length + dirInfo.GetDirectories().Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("指定目录中无图像文件，需重新加载！", "信息提示",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return null;
                }
                else
                {
                    return dirInfo.GetFiles();
                }
            }
            return null;
        }


        /// <summary>
        /// 根据Int类型的值,返回用1或0(对应true或false)填充的数组
        /// 注:从右侧开始向左索引(0~31)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<bool> GetBitList(int value)
        {
            var list = new System.Collections.Generic.List<bool>(32);
            for (var i = 0; i <= 31; i++)
            {
                var val = 1 << i;
                list.Add((value & val) == val);
            }

            return list;
        }

        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">
        /// 32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>位置值
        /// true,1
        /// false,0</returns>
        public static bool GetBitValue(int value, ushort index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index");

            var val = 1 << index;
            return ((value & val) == val);
        }

        /// <summary>
        /// 设置Int数据中的某一位的值
        /// </summary>
        /// <param name="value">位设置前的值</param>
        /// <param name="index">
        /// 32位数据的从右向左的偏移位索引(0~31)</param>
        /// <param name="bitValue">设置值
        /// true,设置1
        /// false,设置0</param>
        /// <returns>返回位设置后的值</returns>
        public static int SetBitValue(int value, ushort index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index");
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }

        /// <summary>
        /// 设置控件绑定
        /// </summary>
        /// <param name="ctrl">控件名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="obj">对象实体</param>
        /// <param name="name">对象所属属性名</param>
       public static void SetBinding(System.Windows.Forms.Control ctrl, string propertyName, object obj, string name)
        {
            if (ctrl.DataBindings[propertyName] != null) ctrl.DataBindings.Remove(ctrl.DataBindings[propertyName]);
            ctrl.DataBindings.Add(propertyName, obj, name, true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="n">限定读取行数，默认设置0</param>
        /// <returns></returns>
        public static List<string[]> CsvToStringArray(string filePath, int n =0)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);// ,System.Text.Encoding.UTF8
            List<string[]> listStrArr = new List<string[]>();
            int m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    string[] split = str.Split(',');
                    listStrArr.Add(split);
                }
            }
            return listStrArr;
        }

        #region API函数声明

        /// <summary>
        /// 将指定的键和值写到指定的节点，如果已经存在则替换
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键名称。如果为null，则删除指定的节点及其所有的项目</param>
        /// <param name="val">值内容。如果为null，则删除指定节点中指定的键</param>
        /// <param name="filePath">INI文件</param>
        /// <returns>操作是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string section, string key,
            string val, string filePath);

        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="section">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="key">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="def">读取失败时的默认值</param>
        /// <param name="retVal">读取的内容缓冲区</param>
        /// <param name="size">内容缓冲区的长度</param>
        /// <param name="filePath">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        #endregion API函数声明

        /// <summary>
        /// 读取INI文件中指定KEY的字符串型值
        /// </summary>
        /// <param name="iniFilePath">Ini文件</param>
        /// <param name="section">节点名称</param>
        /// <param name="key">键名称</param>
        /// <param name="defaultValue">如果没此KEY所使用的默认值</param>
        /// <returns>读取到的值</returns>
        public static string INIGetStringValue(string iniFilePath, string section, string key, string defaultValue)
        {
            string value = defaultValue;
            const int SIZE = 1024 * 1;

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称(key)", "key");
            }

            StringBuilder sb = new StringBuilder(SIZE);
            uint bytesReturned = GetPrivateProfileString(section, key, defaultValue, sb, SIZE, iniFilePath);

            if (bytesReturned != 0)
            {
                value = sb.ToString();
            }
            sb = null;

            return value;
        }

        /// <summary>
        /// 在INI文件中，指定节点写入指定的键及值。如果已经存在，则替换。如果没有则创建。
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <param name="NoText">值</param>
        /// <param name="iniFilePath">INI文件</param>
        /// <returns>操作是否成功</returns>
        public static bool INIWriteValue(string iniFilePath, string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            if (value == null)
            {
                throw new ArgumentException("值不能为null", "value");
            }

            return WritePrivateProfileString(section, key, value, iniFilePath);
        }

        /// <summary>
        /// 在INI文件中，删除指定节点中的指定的键。
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <returns>操作是否成功</returns>
        public static bool INIDeleteKey(string iniFile, string section, string key)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            return WritePrivateProfileString(section, key, null, iniFile);
        }

        /// <summary>
        /// 在INI文件中，删除指定的节点。
        /// </summary>
        /// <param name="iniFile">INI文件</param>
        /// <param name="section">节点</param>
        /// <returns>操作是否成功</returns>
        public static bool INIDeleteSection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileString(section, null, null, iniFile);
        }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="Text"> </param>
        /// <param name="Pattern"></param>
        /// <returns></returns>
        public static Match RegexMatch(string Text, string Pattern)
        {
            Match result = Regex.Match(
                    Text,
                    Pattern,
                    RegexOptions.IgnoreCase |         //忽略大小写
                    RegexOptions.ExplicitCapture |    //提高检索效率
                    RegexOptions.RightToLeft          //从左向右匹配字符串
                    );
            return result;
        }
        /// <summary>
        /// short[]转byte list
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        private static List<byte> NetworkBytes(short[] registers)
        {
            List<byte> list = new List<byte>();
            try
            {
                foreach (short num in registers)
                {
                    list.AddRange(BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)num)));
                }
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }

        /// <summary>
        /// float 转 byte list
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        public static List<byte> NetworkBytes(float registers)
        {
            List<byte> list = new List<byte>();
            try
            {
                List<short> value = new List<short>();
                byte[] val = BitConverter.GetBytes(registers);
                value.Add(BitConverter.ToInt16(val, 0));
                value.Add(BitConverter.ToInt16(val, 2));
                if (value.Count == 0)
                {
                    throw new Exception("数组长度错误");
                }
                list.AddRange(NetworkBytes(value.ToArray()));
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }

        /// <summary>
        /// int 转 byte list
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        public static List<byte> NetworkBytes(int registers)
        {
            List<byte> list = new List<byte>();
            try
            {
                List<short> value = new List<short>();
                byte[] val = BitConverter.GetBytes(registers);
                value.Add(BitConverter.ToInt16(val, 0));
                value.Add(BitConverter.ToInt16(val, 2));
                if (value.Count == 0)
                {
                    throw new Exception("数组长度错误");
                }
                list.AddRange(NetworkBytes(value.ToArray()));
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }
    }







}
