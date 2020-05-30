using System;
using System.Collections.Generic;
using Common;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Vision;
using Motion;
using System.ComponentModel;
using System.Drawing;
using Common.PointLayout;

namespace Motion
{
    public enum SiteRegion : int
    {
        SOLDER_LIFT,
        SOLDER_RIGHT,//
        POLISH_LIFT,//
        POLISH_RIGHT,//
    }

    /// <summary>
    /// 运行参数，就是杂类，不需要下发 
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RunDataDef : ILayoutPoint
    {
        #region 数据

        /// <summary>
        /// 打磨点拍照
        /// </summary>
        public List<wPointF>[] wPointFs_Polish = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };

        /// <summary>
        /// 上锡点拍照
        /// </summary>
        public List<wPointF>[] wPointFs_Solder = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };

        /// <summary>
        /// 拍照前延时
        /// </summary>
        public int vDeley { get; set; }

        /// <summary>
        /// 清洗模式：0不清洗，1逐产品清洗，2焊前清洗
        /// </summary>
        public int rinseMode { get; set; }

        public int clearnum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int pNumL { get; set; }
        public int pNumR { get; set; }
        public int sNumL { get; set; }
        public int sNumR { get; set; }

        /// <summary>
        /// UPH
        /// </summary>
        public int UPH { set; get; }
        public int polishtimes { set; get; }
        public int moveSpd { get; set; }
        
        public int leftSoldertintimes { set; get; }
        public int rightSoldertintimes { set; get; }
        #endregion

        public RunDataDef()
        {
            wPointFs_Polish = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
            wPointFs_Solder = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };

            vDeley = 100;
            rinseMode = 0;

            UPH = 0;

            pNumL = 1;
            pNumR = 1;
            sNumL = 1;
            sNumR = 1;
        }

        [OnDeserialized()]
        private void OnDeserializedMed(StreamingContext context)
        {

        }

        #region   显示
        SiteRegion _index = 0;//
        int TRegion = 0;
        public void GetLayoutPointsShow(int index,int t)
        {
           this._index = (SiteRegion)index;
           this.TRegion = t;

        }


        private class SiteLayoutPoint : LayoutPoint
        {
            public const int MarkRidus = 2;

            public SiteLayoutPoint(wPointF obj) : base(obj)
            {
            }

            public override void Drawing(Graphics gc)
            {
                //switch (PointAttribute)
                //{
                //    case SitePointAttribute.Normal:
                //        gc.DrawEllipse(Pens.Blue, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
                //        gc.DrawLine(Pens.Blue, Point.X - MarkRidus, Point.Y - MarkRidus, Point.X + MarkRidus, Point.Y + MarkRidus);
                //        gc.DrawLine(Pens.Blue, Point.X - MarkRidus, Point.Y + MarkRidus, Point.X + MarkRidus, Point.Y - MarkRidus);
                //        break;
                //    case SitePointAttribute.Working:
                //        gc.DrawEllipse(Pens.Green, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
                //        gc.DrawLine(Pens.Green, Point.X - MarkRidus, Point.Y - MarkRidus, Point.X + MarkRidus, Point.Y + MarkRidus);
                //        gc.DrawLine(Pens.Green, Point.X - MarkRidus, Point.Y + MarkRidus, Point.X + MarkRidus, Point.Y - MarkRidus);
                //        break;
                //    case SitePointAttribute.Finish:
                //        gc.FillEllipse(Brushes.Green, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
                //        break;
                //}

                gc.FillEllipse(Brushes.Green, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
                if (Selected)
                {
                    gc.FillEllipse(Brushes.Green, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
                }

            }
            
        }
        
        public List<LayoutPoint> GetLayoutPoints()
        {
            List<LayoutPoint> layouts = new List<LayoutPoint>();
            switch (_index)
            {
                case SiteRegion.POLISH_LIFT:
                    foreach (var p in wPointFs_Polish[0])
                    {
                        if (p.T == (float)this.TRegion)
                        {
                            SiteLayoutPoint site = new SiteLayoutPoint(p)
                            {
                                Point = new PointF(p.X, p.Y)
                            };

                            layouts.Add(site);
                        }
                    }
                    break;
                case SiteRegion.POLISH_RIGHT:
                    foreach (var p in wPointFs_Polish[1])
                    {
                        if(p.T == (float)this.TRegion)
                        {
                            SiteLayoutPoint site = new SiteLayoutPoint(p)
                            {
                                Point = new PointF(p.X, p.Y)
                            };
                            layouts.Add(site);
                        }
                    }
                    break;
                case SiteRegion.SOLDER_LIFT:
                    foreach (var p in wPointFs_Solder[0])
                    {
                        if (p.T == (float)this.TRegion)
                        {
                            SiteLayoutPoint site = new SiteLayoutPoint(p)
                            {
                                Point = new PointF(p.X, p.Y)
                            };
                            layouts.Add(site);
                        }
                    }
                    break;
                case SiteRegion.SOLDER_RIGHT:
                    foreach (var p in wPointFs_Solder[1])
                    {
                        if (p.T == (float)this.TRegion)
                        {
                            SiteLayoutPoint site = new SiteLayoutPoint(p)
                            {
                                Point = new PointF(p.X, p.Y)
                            };
                            layouts.Add(site);
                        }
                    }
                    break;
            }
            
            return layouts;
        }

        public IEnumerable<LayoutPoint> GenLayoutPointEnumerable()
        {
            return new LayoutPointEnumerable(this);
        }

        #endregion


    }

    /// <summary>
    /// 上锡参数
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SolderDef : ICloneable 
    {
        #region
        [CategoryAttribute("第一段"), DisplayNameAttribute("第一段送锡长度")]
        public float FrontLen { get; set; }
        [CategoryAttribute("第一段"), DisplayNameAttribute("第一段送锡速度")]
        public float FrontSpeed { get; set; }
        [CategoryAttribute("第一段"), DisplayNameAttribute("第一段退锡长度")]
        public float BackLen { get; set; }
        [CategoryAttribute("第一段"), DisplayNameAttribute("第一段退锡速度")]
        public float BsckSpeed { get; set; }

        [CategoryAttribute("第二段"), DisplayNameAttribute("第二段送锡长度")]
        public float FrontLen2 { get; set; }
        [CategoryAttribute("第二段"), DisplayNameAttribute("第二段送锡速度")]
        public float FrontSpeed2 { get; set; }
        [CategoryAttribute("第二段"), DisplayNameAttribute("第二段退锡长度")]
        public float BackLen2 { get; set; }
        [CategoryAttribute("第二段"), DisplayNameAttribute("第二段退锡速度")]
        public float BsckSpeed2 { get; set; }

        [CategoryAttribute("第三段"), DisplayNameAttribute("第三段送锡长度")]
        public float FrontLen3 { get; set; }
        [CategoryAttribute("第三段"), DisplayNameAttribute("第三段送锡速度")]
        public float FrontSpeed3 { get; set; }
        [CategoryAttribute("第三段"), DisplayNameAttribute("第三段退锡长度")]
        public float BackLen3 { get; set; }
        [CategoryAttribute("第三段"), DisplayNameAttribute("第三段退锡速度")]
        public float BsckSpeed3 { get; set; }

        [CategoryAttribute("第一段"), DisplayNameAttribute("第一段送锡延时")]
        public int SendDelay { get; set; }
        [CategoryAttribute("第二段"), DisplayNameAttribute("第二段送锡延时")]
        public int SendDelay2 { get; set; }
        [CategoryAttribute("第三段"), DisplayNameAttribute("第三段送锡延时")]
        public int SendDelay3 { get; set; }

        [DisplayNameAttribute("抖动模式，"), DescriptionAttribute("抖动模式：0：左右，1：前后")]
        public int mode { get; set; }
        [DisplayNameAttribute("抖动次数")]
        public int times { get; set; }
        [DisplayNameAttribute("抖动幅度")]
        public float interval { get; set; }
        [DisplayNameAttribute("抖动高度")]
        public float height { get; set; }
        [DisplayNameAttribute("抖动速度")]
        public float speed { get; set; }
        [DisplayNameAttribute("抖动送锡长度")]
        public float sendlen { get; set; }
        [DisplayNameAttribute("抖动送锡速度")]
        public float sendSpeed { get; set; }
        [DisplayNameAttribute("返回方式")]
        public int Backmode { get; set; }
        [DisplayNameAttribute("返回高度")]
        public float BackHeight { get; set; }
        [DisplayNameAttribute("提起高度")]
        public float LiftHeight { get; set; }

        #endregion
        public SolderDef()
        {
            FrontLen = 0.25f;
            FrontSpeed = 100;
            BackLen = 0.25f;
            BsckSpeed = 100;

            FrontLen2 = 0.25f;
            FrontSpeed2 = 50;
            BackLen2 = 0.25f;
            BsckSpeed2 = 50;

            FrontLen3 = 0.25f;
            FrontSpeed3 = 30;
            BackLen3 = 0.25f;
            BsckSpeed3 = 30;

            SendDelay = 10;
            SendDelay2 = 10;
            SendDelay3 = 10;
            mode = 1;
            times = 3;
            interval = 0.25f;
            height = 1;
            speed = 100;
            sendlen = 1;
            sendSpeed = 100;
            Backmode = 0;
            BackHeight = 1;
            LiftHeight = 1;
        }

        object ICloneable.Clone()
        { 
            SolderDef pro = new SolderDef();
            pro.FrontLen = this.FrontLen;
            pro.FrontSpeed = this.FrontSpeed;
            pro.BackLen = this.BackLen;
            pro.BsckSpeed = this.BsckSpeed;

            pro.FrontLen2 = this.FrontLen2;
            pro.FrontSpeed2 = this.FrontSpeed2;
            pro.BackLen2 = this.BackLen2;
            pro.BsckSpeed2 = this.BsckSpeed2;

            pro.FrontLen3 = this.FrontLen3;
            pro.FrontSpeed3 = this.FrontSpeed;
            pro.BackLen3 = this.BackLen3;
            pro.BsckSpeed3 = this.BsckSpeed3;

            pro.SendDelay = this.SendDelay;
            pro.SendDelay2 = this.SendDelay2;
            pro.SendDelay3 = this.SendDelay3;

            pro.mode = this.mode;
            pro.times = this.times;
            pro.interval = this.interval;
            pro.height = this.height;
            pro.speed = this.speed;
            pro.sendlen = this.sendlen;
            pro.sendSpeed = this.sendSpeed;
            pro.Backmode = this.Backmode;
            pro.BackHeight = this.BackHeight;
            pro.LiftHeight = this.LiftHeight;

            return pro;
        }
        public SolderDef Clone()
        {
            return (SolderDef)((ICloneable)this).Clone();
        }

    }

    /// <summary>
    /// 打磨参数
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PolishDef : ICloneable
    {
        #region

        [DisplayNameAttribute("打磨方式"),DescriptionAttribute("打磨方式：0：一字左右，1：二字左右， 2：一字前后， 3：二字前后")]
        public int mode { get; set; }

        [ DisplayNameAttribute("往返次数")]
        public int GoBackTimes { get; set; }

        [DisplayNameAttribute("打磨往返速度")]
        public float PolishSpeed { get; set; }

        [DisplayNameAttribute("打磨往返幅度")]
        public float GoBackRange { get; set; }

        [DisplayNameAttribute("二字打磨间距")]
        public float PolishInterval { get; set; }
        [DisplayNameAttribute("提起高度")]
        public float LiftHeight { get; set; }
        #endregion
        public PolishDef()
        {
            mode = 3;
            GoBackTimes = 1;
            PolishSpeed = 100;
            GoBackRange = 0.1f;
            PolishInterval = 0.1f;
            LiftHeight = 2f;
        }
        object ICloneable.Clone()
        {
            PolishDef pro = new PolishDef();
            pro.mode = this.mode;
            pro.GoBackTimes = this.GoBackTimes;
            pro.PolishSpeed = this.PolishSpeed;
            pro.GoBackRange = this.GoBackRange;
            pro.PolishInterval = this.PolishInterval;
            pro.LiftHeight = this.LiftHeight;
            return pro;
        }
        public PolishDef Clone()
        {
            return (PolishDef)((ICloneable)this).Clone();
        }
    }

    /// <summary>
    /// 清洗参数
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CleanDef
    {
        #region

        public PointF5 posL { get; set; }
        public PointF5 posR { get; set; }

        public float FrontLen { get; set; }
        public float FrontSpeed { get; set; }
        public float BackLen { get; set; }
        public float BackSpeed { get; set; }
        public int CleanTime { get; set; }

        #endregion
        public CleanDef()
        {
            posL = new PointF5();
            posR = new PointF5();

            FrontLen = 5;
            FrontSpeed = 100;
            BackLen = 1;
            BackSpeed = 100;
            CleanTime = 100;
        }
    }

    /// <summary>
    /// 基础参数
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Basics
    {
        #region
        public int StartRunMode { get; set; }
        public int DevcieMode { get; set; }
        public int TinDetectEn { get; set; }
        public int CleanEn { get; set; }
        public int ShakeEn { get; set; }

        public float TurnAvoidPos_XL { get; set; }
        public float TurnAvoidPos_XR { get; set; }

        public float Safe_ZL { get; set; }
        public float Safe_ZR { get; set; }

        public float WeldSpeedL { get; set; }
        public float WeldSpeedR { get; set; }

        public float TeachSpeedL { get; set; }
        public float TeachSpeedR { get; set; }

        public int polish_z_pos { get; set; }
        public float Safe_Z { get; set; }
        public float PolishSpeed { get; set; }
        public float TeachSpeed { get; set; }

        public float PolishOffset { get; set; }
        public int PolishTimes { get; set; }
        public int PolishBlowDelay { get; set; }
        public int PolishCounts { get; set; }//打磨次数统计，更换打磨头后数据必须清零
        public int PolishTotalOffset { get; set; }//打磨次数的补偿，更换打磨头必须清零

        #endregion
        public Basics()
        {
            StartRunMode = 0;
            DevcieMode = 1;
            TinDetectEn = 0;
            CleanEn = 0;
            ShakeEn = 1;

            TurnAvoidPos_XL = 200;
            TurnAvoidPos_XR = 200;
            Safe_ZL = 2;
            Safe_ZR = 2;
            polish_z_pos = 0;
            WeldSpeedL = 20;
            WeldSpeedR = 20;
            TeachSpeedL = 30;
            TeachSpeedR = 30;

            Safe_Z = 2;
            PolishSpeed = 20;
            TeachSpeed = 20;

            PolishOffset = 0.1f;
            PolishTimes = 100;
            PolishBlowDelay = 100;
            PolishCounts = 0;
            PolishTotalOffset = 0;
        }


    }

    /***************视觉*******************/

    /// <summary>
    /// 视觉识别后参数
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class VDataDef
    {
        /// <summary>
        /// 打磨
        /// </summary>
        public List<PolishPos> vPolishDatasL = new List<PolishPos>();
        public List<PolishPos> vPolishDatasR = new List<PolishPos>();
        /// <summary>
        /// 上锡
        /// </summary>
        public List<SolderPos> vSolderDatasL = new List<SolderPos>();
        public List<SolderPos> vSolderDatasR = new List<SolderPos>();

        public List<PolishPos> polishdata(int index)
        {
            if (index == 0)
                return vPolishDatasL;
            else
                return vPolishDatasR;
        }

        public List<SolderPos> soliderdata(int index)
        {
            if (index == 0)
                return vSolderDatasL;
            else
                return vSolderDatasR;
        }

        public VDataDef()
        {
            vPolishDatasL = new List<PolishPos>();
            vSolderDatasL = new List<SolderPos>();

            vPolishDatasR = new List<PolishPos>();
            vSolderDatasR = new List<SolderPos>();
        }

    }

    /// <summary>
    /// 对应打磨模板参数
    /// </summary>
    /// 
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PolishPos
    {
        public PointF2 Vpos { get; set; }
        public List<PolishPosdata> pos { get; set; }
        public PolishDef polishDef { get; set; }
        public PolishPos()
        {
            Vpos = new PointF2();
            polishDef = new PolishDef();
            pos = new List<PolishPosdata>();
        }
    }
    /// <summary>
    /// 对应上锡模板参数
    /// </summary>
    /// 
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SolderPos
    {
        public PointF2 Vpos { get; set; }
        public List<SolderPosdata> pos { get; set; }
        public SolderDef solderDef { get; set; }
        public SolderPos()
        {
            Vpos = new PointF2();
            solderDef = new SolderDef();
            pos = new List<SolderPosdata>();
        }
    }

    /**********************************/

    /// <summary>
    /// 下发参数
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SlaverData
    {
        /// <summary>
        /// 打磨复位后位置
        /// </summary>
        public RstPos rstPos { get; set; }

        /// <summary>
        /// 平台1复位后位置
        /// </summary>
        public RstPos_S rstPos1 { get; set; }
        /// <summary>
        /// 平台2复位后位置
        /// </summary>
        public RstPos_S rstPos2 { get; set; }


        /// <summary>
        /// 打磨复位后位置
        /// </summary>
        public RstPos endPos { get; set; }

        /// <summary>
        /// 平台1复位后位置
        /// </summary>
        public RstPos_S endPos1 { get; set; }
        /// <summary>
        /// 平台2复位后位置
        /// </summary>
        public RstPos_S endPos2 { get; set; }

        public RstPos_S endPosS(int index)
        {
            if (index == 0)
            {
                return endPos1;
            }
            else
            {
                return endPos2;
            }
        }


        /// <summary>
        /// 基础参数
        /// </summary>
        public Basics basics { get; set; }

        public SlaverData()
        {
            rstPos = new RstPos();
            rstPos1 = new RstPos_S();
            rstPos2 = new RstPos_S();

            endPos = new RstPos();
            endPos1 = new RstPos_S();
            endPos2 = new RstPos_S();

            basics = new Basics();
        }


    }

    /***************************整个逻辑参数****************************************/

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SettingDataDef  //
    {
        /// <summary>
        /// 运行参数
        /// </summary>
		public RunDataDef RunData { set; get; }

        /// <summary>
        /// 视觉参数
        /// </summary>
        public VDataDef vData { get; set; }


        /// <summary>
        /// 清洗参数
        /// </summary>
        public CleanDef rinseData { get; set; }

        /// <summary>
        /// 下发参数
        /// </summary>
        public SlaverData slaverData { get; set; }

        public SettingDataDef()
        {
            RunData = new RunDataDef();
            vData = new VDataDef();
            rinseData = new CleanDef();
            slaverData = new SlaverData();
        }


        /// <summary>
        /// 新建配置
        /// </summary>
        public void CreatProject()
        {
            try
            {
                this.RunData = new RunDataDef();
                this.vData = new VDataDef();
            }
            catch (Exception ex)
            {
                Common.LogWriter.WriteException(ex);
                Common.LogWriter.WriteLog(string.Format("错误：加载配置文件失败!\n异常描述:{0}\n时间：{1}", ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public void OpenProject(string path)
        {
            try
            {
                SettingDataDef data = (SettingDataDef)Common.CreateProject.OpenProject(typeof(SettingDataDef), path);

                this.RunData = data.RunData;
                this.vData = data.vData;
                this.slaverData = data.slaverData;
                this.rinseData = data.rinseData;
            }
            catch (Exception ex)
            {
                Common.LogWriter.WriteException(ex);
                Common.LogWriter.WriteLog(string.Format("错误：加载配置文件失败!\n异常描述:{0}\n时间：{1}", ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveProject(string path)
        {
            Common.CreateProject.SaveProject(this, path);
        }
    }
}
