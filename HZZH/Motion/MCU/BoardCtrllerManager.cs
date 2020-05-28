using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Device
{
    public class BoardCtrllerManager
    {
        /// <summary>
        /// 控制卡
        /// </summary>
        Modbus.Common hzaux = new Modbus.Common();

        #region 字段
        /// <summary>
        /// lock对象
        /// </summary>
        private object netOlock = new object();
        /// <summary>
        /// 通信队列
        /// </summary>
        private ConcurrentQueue<BaseData> NetQueue = new ConcurrentQueue<BaseData>();
        /// <summary>
        /// 被动读取线程
        /// </summary>
        private BackgroundWorker backworkerRead;
        /// <summary>
        /// 写数据线程
        /// </summary>
        private BackgroundWorker backworkerWrite;
        /// <summary>
        /// 重连线程
        /// </summary>
		private BackgroundWorker backworkerConnect;
        /// <summary>
        /// 线程自动操作事件
        /// </summary>
        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        /// <summary>
        /// 线程手动操作事件
        /// </summary>
        private ManualResetEvent readResetEvent = new ManualResetEvent(true);
        /// <summary>
        /// IP
        /// </summary>
		private string _ip = "192.168.1.30";
        /// <summary>
        /// 端口
        /// </summary>
		private int _port = 8089;
        /// <summary>
        /// 连接状态
        /// </summary>
        private bool netSucceed = false;
        #endregion 字段
        
		public BoardCtrllerManager()
		{
			backworkerConnect = new BackgroundWorker();
			backworkerConnect.WorkerSupportsCancellation = true;
			backworkerConnect.DoWork += ConnectWork;//重连
	    }

        #region 属性
        /// <summary>
        /// 队列数据量
        /// </summary>
        public int QueueCount
        {
            get
            {
                return NetQueue.Count;
            }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Succeed
        {
            get
            {
                return netSucceed;
            }
        }


        #endregion 属性

        #region 可读变量
        /// <summary>
        /// 被动读取队列
        /// </summary>
        private List<BaseData> ReadData = new List<BaseData>();
        /// <summary>
        /// 轴当前位置
        /// </summary>
        public BaseData CurrentPos = new BaseData(100, 16, DataType.Float);
        /// <summary>
        /// 轴当前状态
        /// </summary>
        public BaseData AxisStatus = new BaseData(300, 5);     
        /// <summary>
        /// 输入口
        /// </summary>
        public BaseData InputStatus = new BaseData(310, 20); 
        /// <summary>
        /// 报警码
        /// </summary>
        public BaseData ErrorCode = new BaseData(390, 20);    
        /// <summary>
        /// 报警等级
        /// </summary>
        public BaseData ErrorLevel = new BaseData(430, 1);  
        /// <summary>
        /// 设备状态
        /// </summary>
        public BaseData DeviceStatus = new BaseData(500, 1);  
        /// <summary>
        /// 输出口
        /// </summary>
        public BaseData OutputStatus = new BaseData(1020, 20);

        public BaseData Uph = new BaseData(1518, 1);
        public BaseData ProNum = new BaseData(4300, 1);
        public BaseData Label_Num = new BaseData(4338, 1);
        public BaseData SoftWare_Ver = new BaseData(20,12);


        /// <summary>
        /// 平台工作
        /// </summary>
        public BaseData Y_Start = new BaseData(1502, 2);
        #endregion

        #region 方法

        /// <summary>
        /// 读寄存器
        /// </summary>
        /// <param name="_var"></param>
        public void ReadRegister(BaseData _var)
        {
            lock (netOlock)
            {
                readResetEvent.Reset();
                NetQueue.Enqueue(_var);
                autoResetEvent.Set();
            }
        }

        /// <summary>
        /// 写寄存器
        /// </summary>
        /// <param name="_var"></param>
        public void WriteRegister(BaseData _var)
        {
            lock (netOlock)
            {
                readResetEvent.Reset();
                NetQueue.Enqueue(_var);
                autoResetEvent.Set();
            }
        }

        /// <summary>
        /// 读整模块参数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data"></param>
        /// <param name="structure"></param>
        /// <param name="step"></param>
        /// <param name="count"></param>
        public void ReadRegister<T>(BaseData data, Object structure, ref int step, ref int count)
        {
            switch (step)
            {
                case 0:
                    ushort number = (ushort)(BytesConverter.ObjToBytes(structure).Length / 2);
                    ushort address = data.Address;
                    data = new BaseData(address, number, DataType.Byte);
                    ReadRegister(data);
                    step = 1;
                    break;
                case 1:
                    if (data.Succeed)
                    {
                        structure = BytesConverter.BytesToObj<T>(data.ByteValue);
                    }
                    else if (Succeed)
                    {
                        count++;
                        if (count > 15)
                        {
                            count = 0;
                            step = 0;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化读队列函数
        /// </summary>
        public void InitializeReadData()
        {
            try
            {
                ReadData.Clear();
                ReadData.Add(CurrentPos);
                ReadData.Add(AxisStatus);
                ReadData.Add(InputStatus);
                ReadData.Add(ErrorCode);
                ReadData.Add(ErrorLevel);
                ReadData.Add(DeviceStatus);
                ReadData.Add(OutputStatus);
                ReadData.Add(Uph);
                ReadData.Add(ProNum);
                ReadData.Add(Label_Num);
                ReadData.Add(SoftWare_Ver);

                ReadData.Add(Y_Start);//读取
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 初始化连接调用
        /// </summary>
        public void ConnectCtrller(string ip, int port)
        {
            try
            {

                _ip = ip;
				_port = port;
                hzaux.InitializeUdp(ip, port);
                InitializeReadData();
                netSucceed = true;
                backworkerRead = new BackgroundWorker();
                backworkerRead.WorkerSupportsCancellation = true;
                backworkerRead.DoWork += ReadWork;//被动读取使用
                backworkerRead.RunWorkerAsync();
                backworkerWrite = new BackgroundWorker();
                backworkerWrite.WorkerSupportsCancellation = true;
                backworkerWrite.DoWork += WriteWork;//主动读写
                backworkerWrite.RunWorkerAsync();
            }
            catch
            {
                if (!backworkerConnect.IsBusy)
				{
					backworkerConnect.RunWorkerAsync();
				}
            }
        }

        /// <summary>
        /// 被动读取线程实体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadWork(Object sender, DoWorkEventArgs e)
        {
            ushort addr, num;
            int readLoopCnt = 0;
            uint[] uintData;
            ushort[] ushortData;
            int[] intData;
            float[] floatData;
            short[] shortData;
            byte[] byteData;
            while (netSucceed)
            {
                try
                {
                    if (ReadData.Count > 0)
                    {
                        addr = ReadData[readLoopCnt].Address;
                        num = ReadData[readLoopCnt].RegisterNum;
                        DataType dt = ReadData[readLoopCnt].DataTypes;
                        switch (dt)
                        {
                            case DataType.Int:
                                hzaux.Read(1, addr, num, out intData);
                                Buffer.BlockCopy(intData, 0, ReadData[readLoopCnt].IntValue, 0, intData.Length * 4);
                                break;
                            case DataType.Float:
                                hzaux.Read(1, addr, num, out floatData);
                                Buffer.BlockCopy(floatData, 0, ReadData[readLoopCnt].FloatValue, 0, floatData.Length * 4);
                                break;
                            case DataType.Uint:
                                hzaux.Read(1, addr, num, out uintData);
                                Buffer.BlockCopy(uintData, 0, ReadData[readLoopCnt].UintValue, 0, uintData.Length * 4);
                                break;
                            case DataType.Ushort:
                                hzaux.Read(1, addr, num, out ushortData);
                                Buffer.BlockCopy(ushortData, 0, ReadData[readLoopCnt].UshortValue, 0, ushortData.Length * 2);
                                break;
                            case DataType.Short:
                                hzaux.Read(1, addr, num, out shortData);
                                Buffer.BlockCopy(shortData, 0, ReadData[readLoopCnt].ShortValue, 0, shortData.Length * 2);
                                break;
                            case DataType.Byte:
                                hzaux.Read(1, addr, num, out byteData);
                                byte[] bt = new byte[byteData.Length];
                                for (int i = 0; i < byteData.Length; i++)
                                {
                                    if (i % 2 != 0)
                                    {
                                        bt[i - 1] = byteData[i];
                                        bt[i] = byteData[i - 1];
                                    }
                                }
                                Buffer.BlockCopy(bt, 0, ReadData[readLoopCnt].ByteValue, 0, bt.Length);
                                break;
                            default:
                                break;
                        }
                        try
                        {
                            DiDoStatus.CurrInputStatus = InputStatus.IntValue;
                            DiDoStatus.CurrOutputStatus = OutputStatus.IntValue;
                        }
                        catch { }
                        readLoopCnt++;
                        if (readLoopCnt >= ReadData.Count)
                        {
                            readLoopCnt = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    netSucceed = false;
                    autoResetEvent.Set();
                    LogWriter.WriteLog("控制器掉线：被动读取 " + ex.ToString());
                    if (!backworkerConnect.IsBusy)
                    {
                        backworkerConnect.RunWorkerAsync();
                    }
                }
                readResetEvent.WaitOne();
            }
        }

        /// <summary>
        /// 写数据线程实体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteWork(Object sender, DoWorkEventArgs e)
        {
            uint[] uintData;
            ushort[] ushortData;
            int[] intData;
            float[] floatData;
            short[] shortData;
            byte[] byteData;
            bool queueState;
			while (Succeed)
            {
                try
                {
                    if (NetQueue.Count > 0)
                    {
                        BaseData _var;
                        queueState = NetQueue.TryPeek(out _var);
                        if (queueState)
                        {
                            if (!_var.ReadHand)
                            {
                                try
                                {
                                    switch (_var.DataTypes)
                                    {
                                        case DataType.Int:
                                            hzaux.Write(1, _var.Address, _var.IntValue);
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Float:
                                            hzaux.Write(1, _var.Address, _var.FloatValue);
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Uint:
                                            hzaux.Write(1, _var.Address, _var.UintValue);
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Ushort:
                                            hzaux.Write(1, _var.Address, _var.UshortValue);
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Short:
                                            hzaux.Write(1, _var.Address, _var.ShortValue);
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Byte:
                                            hzaux.Write(1, _var.Address, _var.ByteValue);
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Object:
                                            hzaux.Write(1, _var.Address, _var.ObjectValue);
                                            _var.Succeed = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("写数据失败：" + ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    switch (_var.DataTypes)
                                    {
                                        case DataType.Int:
                                            hzaux.Read(1, _var.Address, _var.RegisterNum, out intData);
                                            _var.IntValue = (int[])intData.Clone();
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Float:
                                            hzaux.Read(1, _var.Address, _var.RegisterNum, out floatData);
                                            _var.FloatValue = (float[])floatData.Clone();
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Uint:
                                            hzaux.Read(1, _var.Address, _var.RegisterNum, out uintData);
                                            _var.UintValue = (uint[])uintData.Clone();
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Ushort:
                                            hzaux.Read(1, _var.Address, _var.RegisterNum, out ushortData);
                                            _var.UshortValue = (ushort[])ushortData.Clone();
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Short:
                                            hzaux.Read(1, _var.Address, _var.RegisterNum, out shortData);
                                            _var.ShortValue = (short[])shortData.Clone();
                                            _var.Succeed = true;
                                            break;
                                        case DataType.Byte:
                                            hzaux.Read(1, _var.Address, _var.RegisterNum, out byteData);
                                            byte[] bt = new byte[byteData.Length];
                                            for (int i = 0; i < byteData.Length; i++)
                                            {
                                                if (i % 2 != 0)
                                                {
                                                    bt[i - 1] = byteData[i];
                                                    bt[i] = byteData[i - 1];
                                                }
                                            }
                                            _var.ByteValue = (byte[])bt.Clone();
                                            _var.Succeed = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("主动读取数据失败：" + ex.Message);
                                }
                            }
                            queueState = NetQueue.TryDequeue(out _var);
                        }
                        else
                        {
                            throw new Exception("取数据不成功");
                        }
                    }
                    else
                    {
                        readResetEvent.Set();
                        autoResetEvent.WaitOne();
                    }
                }
                catch (Exception ex)
                {
                    netSucceed = false;
                    readResetEvent.Set();
                    LogWriter.WriteLog("控制器掉线：" + ex.ToString());
                    if (!backworkerConnect.IsBusy)
                    {
                        backworkerConnect.RunWorkerAsync();
                    }
                }
            }
        }

        /// <summary>
        /// 重连线程实体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ConnectWork(Object sender, DoWorkEventArgs e)
		{
            while (!Succeed)
            {
                try
                {
                    Thread.Sleep(500);
                    Disposed();
                    Thread.Sleep(500);
                    ConnectCtrller(_ip, _port);
                }
                catch { }
            }
        }


        /// <summary>
        /// 释放通信
        /// </summary>
        public  void Disposed()
        {
            hzaux.Dispose();
        }



        /// <summary>
        /// 单轴相对定位
        /// </summary>
        /// <param name="axisNum">轴号</param>
        /// <param name="Speed">速度</param>
        /// <param name="targetPos">位置</param>
        public void MoveRel(int axisNum, float Speed, float targetPos)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Functions.NetworkBytes(1));
			list.AddRange(Functions.NetworkBytes(1));
			list.AddRange(Functions.NetworkBytes(targetPos));
			list.AddRange(Functions.NetworkBytes(Speed));
			WriteRegister(new BaseData((ushort)(MoveAdrr + 8 * axisNum), list.ToArray()));
		}

        /// <summary>
        /// 单轴回零
        /// </summary>
        /// <param name="axisNum">轴号</param>
        /// <param name="Speed">速度</param>
        public void MoveHome(int axisNum, float Speed)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Functions.NetworkBytes(1));
			list.AddRange(Functions.NetworkBytes(3));
			list.AddRange(Functions.NetworkBytes(0));
			list.AddRange(Functions.NetworkBytes(Speed));
			WriteRegister(new BaseData((ushort)(MoveAdrr + 8 * axisNum), list.ToArray()));
		}

        /// <summary>
        /// 单轴速度模式
        /// </summary>
        /// <param name="axisNum">轴号</param>
        /// <param name="Speed">速度</param>
        /// <param name="targetPos">位置</param>
        public void MoveSpd(int axisNum, float Speed, float targetPos)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Functions.NetworkBytes(1));
			list.AddRange(Functions.NetworkBytes(2));
			list.AddRange(Functions.NetworkBytes(targetPos));
			list.AddRange(Functions.NetworkBytes(Speed));
			WriteRegister(new BaseData((ushort)(MoveAdrr + 8 * axisNum), list.ToArray()));
		}


        /// <summary>
        /// 单轴停止
        /// </summary>
        /// <param name="axisNum">轴号</param>
        public void MoveStop(int axisNum)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Functions.NetworkBytes(1));
			list.AddRange(Functions.NetworkBytes(4));
			list.AddRange(Functions.NetworkBytes(0));
			list.AddRange(Functions.NetworkBytes(0));
			WriteRegister(new BaseData((ushort)(MoveAdrr + 8 * axisNum), list.ToArray()));
		}

        /// <summary>
        /// 单轴绝对定位
        /// </summary>
        /// <param name="axisNum">轴号</param>
        /// <param name="Speed">速度</param>
        /// <param name="targetPos">位置</param>
		public void MoveAbs(int axisNum, float Speed, float targetPos)
		{
			List<byte> list = new List<byte>();
			list.AddRange(Functions.NetworkBytes(1));
			list.AddRange(Functions.NetworkBytes(0));
			list.AddRange(Functions.NetworkBytes(targetPos));
			list.AddRange(Functions.NetworkBytes(Speed));
			WriteRegister(new BaseData((ushort)(MoveAdrr + 8 * axisNum), list.ToArray()));
		}

        private ushort MoveAdrr = 1142;
        //XY轴绝对定位
        public void MoveXYAbs(int axisX, int axisY, float Speed, float targetPosX, float targetPosY)
        {
            List<byte> list = new List<byte>();
            list.AddRange(Functions.NetworkBytes(1));
            list.AddRange(Functions.NetworkBytes(Speed));
            list.AddRange(Functions.NetworkBytes(targetPosX));
            WriteRegister(new BaseData((ushort)(MoveAdrr + axisX * 10), list.ToArray()));
            list.Clear();
            list.AddRange(Functions.NetworkBytes(1));
            list.AddRange(Functions.NetworkBytes(Speed));
            list.AddRange(Functions.NetworkBytes(targetPosY));
            WriteRegister(new BaseData((ushort)(MoveAdrr + axisY * 10), list.ToArray()));
        }

        //获取轴状态
        public int AxisGetSta(int axisNum)
        {
            BaseData redBaseData = new BaseData((ushort)(MoveAdrr + 8 + axisNum * 10), 1);
            ReadRegister(redBaseData);
            if (redBaseData.Succeed == true)
            {
                redBaseData.Succeed = false;
                return redBaseData.IntValue[0];
            }
            else
            {
                return -1;
            }

        }


        #endregion 方法
    }

}