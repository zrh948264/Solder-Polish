using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    /// <summary>
    /// 字节与对象转换工具
    /// 定义类或者其他obj的时候这样定义
    /// 
    /// [StructLayout(LayoutKind.Sequential, Pack = 1)]
    /// class A
    /// {
    ///		xxx
    ///		xxx
    /// }
    /// 
    /// yc 2020.2.11
    /// </summary>
    public class BytesConverter
    {
        /// <summary>
        /// 丢进去obj，出来字节数组
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static Byte[] ObjToBytes(Object structure)
        {
            int size;
            IntPtr buffer = new IntPtr();
            try
            {
                size = Marshal.SizeOf(structure);
                buffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];

                Marshal.Copy(buffer, bytes, 0, size);

                for (int i = 0; i < bytes.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        Byte a = bytes[i + 1];
                        bytes[i + 1] = bytes[i];
                        bytes[i] = a;
                    }
                }
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("确保丢进来的obj以及他的所有子obj上面都有这句话\r[StructLayout(LayoutKind.Sequential, Pack = 1)]" + ex.ToString(), ex);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary>
        /// 丢进去字节数组，设置类型T，出来T类型的东西
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T BytesToObj<T>(Byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return (T)Marshal.PtrToStructure(buffer, typeof(T));

            }
            catch (Exception ex)
            {
                throw new Exception("确保丢进来的obj以及他的所有子obj上面都有这句话\r[StructLayout(LayoutKind.Sequential, Pack = 1)]" + ex.ToString(), ex);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }


}
