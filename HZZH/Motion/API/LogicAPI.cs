using System.Collections.Generic;
using Common;
using System.Diagnostics;
using Device;
using System;
using UI;

namespace Motion
{
    public class BasicApiDef    //基础的api访问方式，不带参数
    {
        public ushort Addr { get; set; }//寄存器地址
        public int start { get; set; }//启动
        public int type { get; set; }//启动
        public int busy { get; set; }//状态
        public int done { get; set; }//完成
        public BoardCtrllerManager movedriverZm { get; set; }//板卡
        public BaseData CommData { get; set; }//通讯数据实例


        //内部用的东西
        public int StartStep = 0;//执行步骤
        public Stopwatch StartOT = new Stopwatch();//执行步骤定时器
        public int StatusStep = 0;//状态步骤
        public Stopwatch StatusOT = new Stopwatch();//状态步骤定时器
        public BasicApiDef(BoardCtrllerManager movedriverZm, ushort Addr)
        {
            this.movedriverZm = movedriverZm;
            this.Addr = Addr;

        }

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
		public bool exe()
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));
                    //temp.AddRange(Functions.NetworkBytes(0));
                    //temp.AddRange(Functions.NetworkBytes(1));
                    CommData = new BaseData(Addr, temp.ToArray());
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        StartStep = 0;
                        CommData.Succeed = false;
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                        ;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }

        /// <summary>
        /// 读取完成
        /// </summary>
        /// <returns></returns>
		public bool sta()
        {
            switch (StatusStep)
            {
                case 0:
                    CommData = new BaseData(Addr, 3, DataType.Int);
                    movedriverZm.ReadRegister(CommData);
                    StatusOT.Restart();
                    StatusStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        if (CommData.IntValue.Length >= 3)
                        {
                            start = CommData.IntValue[0];
                            type = CommData.IntValue[1];
                            busy = CommData.IntValue[2];

                            StatusStep = 0;
                            CommData.Succeed = false;
                            return true;
                        }
                        else
                        {
                            StatusStep = 0;
                            CommData.Succeed = false;
                            return false;
                        }
                    }
                    if (StatusOT.ElapsedMilliseconds > 1000)
                    {
                        StatusStep = 0;
                    }
                    return false;

                default:
                    StatusStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            StatusStep = 0;
            start = 0;
            type = 0;
            busy = 0;
            done = 0;
        }

    }

    /***********************业务逻辑接口***********************/

    /// <summary>
    /// 上锡接口，继承基础api访问类
    /// </summary>
    public class SolderTin : BasicApiDef
    {
        public bool exe(int type, float xPos, float yPos, float zPos, float rPos, float tPos, SolderDef solderDef,int Rinse)
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));
                    temp.AddRange(Functions.NetworkBytes(type));
                    temp.AddRange(Functions.NetworkBytes(1));

                    temp.AddRange(Functions.NetworkBytes(xPos));
                    temp.AddRange(Functions.NetworkBytes(yPos));
                    temp.AddRange(Functions.NetworkBytes(zPos));
                    temp.AddRange(Functions.NetworkBytes(rPos));
                    temp.AddRange(Functions.NetworkBytes(tPos));

                    byte[] aaa = BytesConverter.ObjToBytes(solderDef);
                    byte[] tempdata = temp.ToArray();
                    byte[] _rinsedata = BitConverter.GetBytes(Rinse);

                    for (int i = 0; i < _rinsedata.Length;i++ )
                    {
                        if(i%2==0)
                        {
                            byte rdata = _rinsedata[i];
                            _rinsedata[i] = _rinsedata[i + 1];
                            _rinsedata[i + 1] = rdata;
                        }
                    }
                    


                    byte[] ndata = new byte[tempdata.Length + aaa.Length + _rinsedata.Length];

                    tempdata.CopyTo(ndata, 0);
                    aaa.CopyTo(ndata, tempdata.Length);
                    _rinsedata.CopyTo(ndata, tempdata.Length + aaa.Length);


                    CommData = new BaseData(Addr, ndata);
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {

                        StartStep = 0;
                        CommData.Succeed = false;
                        if (Addr == 4400)
                        {
                               FormMain.RunProcess.LogicData.RunData.leftSoldertintimes++;
                        }
                        else
                        {
                            FormMain.RunProcess.LogicData.RunData.rightSoldertintimes++;
                        }
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }
        public SolderTin(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }
    }

    /// <summary>
    /// 平台移动接口(停靠，复位，拍照用)，继承基础api访问类
    /// </summary>
    public class PlatformMove : BasicApiDef
    {
        public bool exe(int x, int y, int z, int r, int t, float xPos, float yPos, float zPos, float rPos, float tPos,int Rinse)
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));
                    temp.AddRange(Functions.NetworkBytes(x));
                    temp.AddRange(Functions.NetworkBytes(y));
                    temp.AddRange(Functions.NetworkBytes(z));
                    temp.AddRange(Functions.NetworkBytes(r));
                    temp.AddRange(Functions.NetworkBytes(t));

                    temp.AddRange(Functions.NetworkBytes(xPos));
                    temp.AddRange(Functions.NetworkBytes(yPos));
                    temp.AddRange(Functions.NetworkBytes(zPos));//Z轴位置高度
                    temp.AddRange(Functions.NetworkBytes(rPos));
                    temp.AddRange(Functions.NetworkBytes(tPos));
                    temp.AddRange(Functions.NetworkBytes(FormMain.RunProcess.LogicData.RunData.moveSpd));//速度
                    temp.AddRange(Functions.NetworkBytes(Rinse));

                    CommData = new BaseData(Addr, temp.ToArray());
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        StartStep = 0;
                        CommData.Succeed = false;
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }
        public PlatformMove(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }
    }

    /// <summary>
    /// 打磨拍照接口
    /// </summary>
    public class Polishcamera : BasicApiDef
    {
        public bool exe(int Side,int VisionEnd, float  x, float y, float z, float r, float t)
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));
                    temp.AddRange(Functions.NetworkBytes(Side));
                    temp.AddRange(Functions.NetworkBytes(VisionEnd));
                    temp.AddRange(Functions.NetworkBytes(FormMain.RunProcess.LogicData.RunData.moveSpd));//速度

                    temp.AddRange(Functions.NetworkBytes(x));
                    temp.AddRange(Functions.NetworkBytes(y));
                    temp.AddRange(Functions.NetworkBytes(z));
                    temp.AddRange(Functions.NetworkBytes(r));
                    temp.AddRange(Functions.NetworkBytes(t));

                    CommData = new BaseData(Addr, temp.ToArray());
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        StartStep = 0;
                        CommData.Succeed = false;
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }
        public Polishcamera(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }
    }

    /// <summary>
    /// 打磨接口
    /// </summary>
    public class Polish : BasicApiDef
    {
        public bool exe(int x, int y, int z, int r, int t, float xPos, float yPos, float zPos, float rPos, float tPos, PolishDef polishDef,int PolishOpen)
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));
                    temp.AddRange(Functions.NetworkBytes(polishDef.mode));
                    temp.AddRange(Functions.NetworkBytes(1));

                    temp.AddRange(Functions.NetworkBytes(x));
                    temp.AddRange(Functions.NetworkBytes(y));
                    temp.AddRange(Functions.NetworkBytes(z));
                    temp.AddRange(Functions.NetworkBytes(r));
                    temp.AddRange(Functions.NetworkBytes(t));

                    temp.AddRange(Functions.NetworkBytes(xPos));
                    temp.AddRange(Functions.NetworkBytes(yPos));
                    temp.AddRange(Functions.NetworkBytes(zPos));
                    temp.AddRange(Functions.NetworkBytes(rPos));
                    temp.AddRange(Functions.NetworkBytes(tPos));

                    temp.AddRange(Functions.NetworkBytes(polishDef.GoBackTimes));
                    temp.AddRange(Functions.NetworkBytes(polishDef.PolishSpeed));
                    temp.AddRange(Functions.NetworkBytes(polishDef.GoBackRange));
                    temp.AddRange(Functions.NetworkBytes(polishDef.PolishInterval));
                    temp.AddRange(Functions.NetworkBytes(polishDef.LiftHeight));
                    temp.AddRange(Functions.NetworkBytes(PolishOpen));

                    CommData = new BaseData(Addr, temp.ToArray ());
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        FormMain.RunProcess.LogicData.RunData.polishtimes++;
                        StartStep = 0;
                        CommData.Succeed = false;
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }
        public Polish(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }
    }

    /// <summary>
    /// 清洗接口
    /// </summary>
    public class Rinse : BasicApiDef
    {
        //public bool exe(float xPos, float yPos, float zPos, float rPos, float tPos, CleanDef cleanDef)
        //{
        //    switch (StartStep)
        //    {
        //        case 0:
        //            List<byte> temp = new List<byte>();
        //            temp.AddRange(Functions.NetworkBytes(1));
        //            temp.AddRange(Functions.NetworkBytes(0));
        //            temp.AddRange(Functions.NetworkBytes(1));

        //            //temp.AddRange(Functions.NetworkBytes(xPos));
        //            //temp.AddRange(Functions.NetworkBytes(yPos));
        //            //temp.AddRange(Functions.NetworkBytes(zPos));
        //            //temp.AddRange(Functions.NetworkBytes(rPos));
        //            //temp.AddRange(Functions.NetworkBytes(tPos));

        //            //temp.AddRange(Functions.NetworkBytes(cleanDef.FrontLen));
        //            //temp.AddRange(Functions.NetworkBytes(cleanDef.FrontSpeed));
        //            //temp.AddRange(Functions.NetworkBytes(cleanDef.BackLen));
        //            //temp.AddRange(Functions.NetworkBytes(cleanDef.BackSpeed));
        //            //temp.AddRange(Functions.NetworkBytes(cleanDef.CleanTime));

        //            CommData = new BaseData(Addr, temp.ToArray());
        //            movedriverZm.WriteRegister(CommData);
        //            StartOT.Restart();
        //            StartStep = 1;
        //            return false;

        //        case 1:
        //            if (CommData.Succeed == true)
        //            {
        //                StartStep = 0;
        //                CommData.Succeed = false;
        //                return true;
        //            }
        //            if (StartOT.ElapsedMilliseconds > 10000)
        //            {
        //                StartStep = 0;
        //            }
        //            return false;

        //        default:
        //            StartStep = 0;
        //            CommData.Succeed = false;
        //            return false;
        //    }
        //}
        public Rinse(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }
        
    }

    public class PolishRinse : BasicApiDef
    {
        public bool exe(CleanDef cleanDef)
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));

                    temp.AddRange(Functions.NetworkBytes(cleanDef.Cleanmode));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.CleanPos_X));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.CleanPos_Z));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.CleanPos_R));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.GoBackTimes));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.CleanSpeed));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.GoBackRange));
                    temp.AddRange(Functions.NetworkBytes(cleanDef.CleanInterval));

                    CommData = new BaseData(Addr, temp.ToArray());
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        StartStep = 0;
                        CommData.Succeed = false;
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }
        public PolishRinse(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }

    }

    /// <summary>
    /// 翻转接口
    /// </summary>
    public class Reversal : BasicApiDef
    {
        public bool exe(float index)
        {
            switch (StartStep)
            {
                case 0:
                    List<byte> temp = new List<byte>();
                    temp.AddRange(Functions.NetworkBytes(1));
                    temp.AddRange(Functions.NetworkBytes(0));
                    temp.AddRange(Functions.NetworkBytes(index));

                    CommData = new BaseData(Addr, temp.ToArray());
                    movedriverZm.WriteRegister(CommData);
                    StartOT.Restart();
                    StartStep = 1;
                    return false;

                case 1:
                    if (CommData.Succeed == true)
                    {
                        StartStep = 0;
                        CommData.Succeed = false;
                        return true;
                    }
                    if (StartOT.ElapsedMilliseconds > 10000)
                    {
                        StartStep = 0;
                    }
                    return false;

                default:
                    StartStep = 0;
                    CommData.Succeed = false;
                    return false;
            }
        }
        public Reversal(Device.BoardCtrllerManager movedriverZm, ushort Addr) : base(movedriverZm, Addr) { }
    }

    /***********************END***********************/
    public class LogicAPIDef
    {
        //每组都有接口开始、和状态查询
        //传入板卡
        public BoardCtrllerManager movedriverZm { get; set; }
        //保持连接定时器
        private Stopwatch StopWatch_KeepAlive = new Stopwatch();

        #region 接口函数

        public SolderTin[] solderTin = new SolderTin[2]; //焊锡接口

        public PlatformMove[] PlatformMove = new PlatformMove[2];//平台移动接口

        public Rinse[] rinse = new Rinse[2];//清洗接口

        public Reversal[] reversals = new Reversal[2];//翻转接口

        public Polishcamera polishcameras { get; set; }//打磨拍照接口
        public Polish polish { get; set; }//打磨接口

        /// <summary>
        /// 打磨清洗
        /// </summary>
        public PolishRinse polishRinse{get;set;}
        #endregion

        public LogicAPIDef(BoardCtrllerManager movedriverZm)
        {
            StopWatch_KeepAlive.Restart();
            this.movedriverZm = movedriverZm;

            solderTin[0] = new SolderTin(this.movedriverZm, 4400);
            solderTin[1] = new SolderTin(this.movedriverZm, 4800);

            PlatformMove[0] = new PlatformMove(this.movedriverZm, 1520);
            PlatformMove[1] = new PlatformMove(this.movedriverZm, 1560);

            rinse[0] = new Rinse(this.movedriverZm, 4500);
            rinse[1] = new Rinse(this.movedriverZm, 4900);

            reversals[0] = new Reversal(this.movedriverZm, 4612);
            reversals[1] = new Reversal(this.movedriverZm, 5012);

            polishcameras = new Polishcamera(this.movedriverZm, 1608);

            polish = new Polish(this.movedriverZm, 5200);
            polishRinse = new PolishRinse(this.movedriverZm, 5308);
        }

        //输出口Set
        public void OutputSet(int idx, IOSTA val)
        {
            movedriverZm.WriteRegister(new BaseData(1700, new int[] { 1, idx, (int)val }));
        }
        //光源控制
        public void FrameCheckLight(IOSTA val)
        {
            movedriverZm.WriteRegister(new BaseData(1700, new int[] { 1, 23, (int)val }));
        }
        public void NeedleCheckLight(IOSTA val)
        {
            movedriverZm.WriteRegister(new BaseData(1700, new int[] { 1, 24, (int)val }));
        }
        public void FrameLocateLight(IOSTA val)
        {
            movedriverZm.WriteRegister(new BaseData(1700, new int[] { 1, 25, (int)val }));
        }

    }

}
