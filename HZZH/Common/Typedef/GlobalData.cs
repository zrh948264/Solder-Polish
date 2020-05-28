using System;
using System.Runtime.InteropServices;

namespace Common
{
    /// <summary>
    /// 两位浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF2 
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    /// <summary>
    /// 三位浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF3 
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    /// <summary>
    /// 四位浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF4:ICloneable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float R { get; set; }

        public override string ToString()
        {
            return string.Format("[X={0},Y={1},Z={2},R={3}]", X, Y, Z, R);
        }
        object ICloneable.Clone()
        {
            PointF4 f4 = new PointF4();
            f4.X = this.X;
            f4.Y = this.Y;
            f4.Z = this.Z;
            f4.R = this.R;
            return f4;
        }
        public PointF4 Clone()
        {
            return (PointF4)((ICloneable)this).Clone();
        }
    } 
    /// <summary>
    /// 工单浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF5
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float R { get; set; }
        public float T { get; set; }//翻转轴
    }

    /// <summary>
    /// 工单浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class wPointF:ICloneable
    {
        public bool enable { get; set; }
        public int templateIndex { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float T { get; set; }//翻转轴

        public wPointF()
        {
            enable = false;
            templateIndex = -1;
            X = 0;
            Y = 0;
            T = 0;

        }

        object ICloneable.Clone()
        {
            wPointF wP = new wPointF();
            wP.enable = this.enable;
            wP.templateIndex = this.templateIndex;
            wP.X = this.X;
            wP.Y = this.Y;
            wP.T = this.T;
            return wP;

        }
        public wPointF Clone()
        {
            return (wPointF)((ICloneable)this).Clone();
        }
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class RstPos
    {
        public bool modeChecked
        {
            get { return mode > 0 ? true : false; }
            set
            {
                if (value)
                { mode = 1; }
                else
                { mode = 0; }
            }
        }
        public int mode { get; set; }
        public float X { get; set; }
        public float Z { get; set; }
        public float R { get; set; }//翻转轴
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class TMPos
    {
        public float X { get; set; }
        public float Z { get; set; }
        public float R { get; set; }
    }

    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class RstPos_S
    {
        public bool modeChecked
        {
            get { return mode > 0 ? true : false; }
            set
            {
                if (value)
                { mode = 1; }
                else
                { mode = 0; }
            }
        }
        public int mode { get; set;}
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float R { get; set; }
        public float T { get; set; }//翻转轴
    }

    /// <summary>
    /// Combobox绑定显示类
    /// </summary>
    [Serializable]
    public class EffectiveMode
    {
        public int Index { get; set; }//有效值
        public string Name { get; set; }

        public EffectiveMode()
        {
            Index = 0;
            Name = "";
        }
    }


    /// <summary>
    /// 通信读写数据
    /// </summary>
    public class BaseData
    {
        public ushort Address { get; set; }
        public int[] IntValue { get; set; }
        public float[] FloatValue { get; set; }
        public ushort[] UshortValue { get; set; }
        public uint[] UintValue { get; set; }
        public short[] ShortValue { get; set; }
        public byte[] ByteValue { get; set; }
        public object ObjectValue { get; set; }
        public ushort RegisterNum { get; set; }
        public bool Succeed { get; set; }
        public DataType DataTypes { get; set; }
        public bool ReadHand { get; set; }//主动读取
         
        public BaseData()
        {
            Succeed = false;
        }

        /// <summary>
        /// 构造函数写整形
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, int[] arrValue)
        {
            this.Address = address;
            this.IntValue = (int[])arrValue.Clone();
            this.DataTypes = DataType.Int;
            this.Succeed = false;
            this.ReadHand = false;
        }

        /// <summary>
        /// 构造函数写浮点型
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, float[] arrValue)
        {
            this.Address = address;
            this.FloatValue = (float[])arrValue.Clone();
            this.DataTypes = DataType.Float;
            this.Succeed = false;
            this.ReadHand = false;
        }
        /// <summary>
        /// 构造函数写无符号短整型
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, ushort[] arrValue)
        {
            this.Address = address;
            this.UshortValue = (ushort[])arrValue.Clone();
            this.DataTypes = DataType.Ushort;
            this.Succeed = false;
            this.ReadHand = false;
        }
        /// <summary>
        /// 构造函数写短整型
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, short[] arrValue)
        {
            this.Address = address;
            this.ShortValue = (short[])arrValue.Clone();
            this.DataTypes = DataType.Short;
            this.Succeed = false;
            this.ReadHand = false;
        }
        /// <summary>
        /// 构造函数写无符号整型
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, uint[] arrValue)
        {
            this.Address = address;
            this.UintValue = (uint[])arrValue.Clone();
            this.DataTypes = DataType.Uint;
            this.Succeed = false;
            this.ReadHand = false;
        }
        /// <summary>
        /// 构造函数写字节
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, byte[] arrValue)
        {
            this.Address = address;
            this.ByteValue = (byte[])arrValue.Clone();
            this.DataTypes = DataType.Byte;
            this.Succeed = false;
            this.ReadHand = false;
        }
        /// <summary>
        /// 构造函数写对象
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="arrValue">数据</param>
        public BaseData(ushort address, object objectValue)
        {
            this.Address = address;
            this.ObjectValue = objectValue;
            this.DataTypes = DataType.Object;
            this.Succeed = false;
            this.ReadHand = false;
        }

        /// <summary>
        /// 构造函数读取数据
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="numbers">数量</param>
        /// <param name="dt">读取的数据类型</param>
        public BaseData(ushort addr, int numbers, DataType dt = DataType.Int)
        {
            this.Address = addr;
            this.RegisterNum = (ushort)numbers;
            switch (dt)
            {
                case DataType.Int:
                    this.IntValue = new int[numbers];
                    break;
                case DataType.Float:
                    this.FloatValue = new float[numbers];
                    break;
                case DataType.Uint:
                    this.UintValue = new uint[numbers];
                    break;
                case DataType.Ushort:
                    this.UshortValue = new ushort[numbers];
                    break;
                case DataType.Short:
                    this.ShortValue = new short[numbers];
                    break;
                case DataType.Byte:
                    this.ByteValue = new byte[numbers * 2];
                    break;
                default:
                    break;
            }
            this.DataTypes = dt;
            this.Succeed = false;
            this.ReadHand = true;
        }

    }
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        Int,
        Float,
        Ushort,
        Short,
        Uint,
        Byte,
        Object
    }





}
