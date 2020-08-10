
using System;
using System.Collections.Generic;
using Common;
using System.Windows.Forms;
using System.Threading;
using Motion;
using Config;
using System.Diagnostics;
using Device;
using Vision;
using HZZH.Vision.Logic;
using MyControl;
using UI;
using System.Drawing;
using HZZH.Vision.Algorithm;
using System.Linq;

namespace Logic
{
    public class TaskDef//每个任务运行的控制参数	下位的LogicParaDef
    {
        public int ID;//分左右平台
        public int execute;
        public int step;
        public int done;
        public int cnt;
        public Stopwatch Timer = new Stopwatch();

        public void Start() //通用的任务开始方法
        {
            if (execute == 1 && step == 0)//当启动=1且step=1启动任务
            {
                step = 1;//启动 后进入switch里的case
                done = 0;
                cnt = 0;
            }
        }
        public void End() //通用的任务开始方法
        {
            execute = 0;//
            step = 0;
            done = 0;
            cnt = 0;
        }
    }

    public class LogicTaskDef//流程任务集合	下位的LogicTaskDef
    {
        public TaskDef AngingTest = new TaskDef();//老化测试

        public TaskDef[] PolishTask = new TaskDef[2] { new TaskDef(), new TaskDef() };//打磨任务
        public TaskDef[] SolderTask = new TaskDef[2] { new TaskDef(), new TaskDef() };//上锡任务
        public TaskDef[] WorkTask = new TaskDef[2] { new TaskDef(), new TaskDef() };//工作任务
    }

    public class LogicMain//设备流程 下位的Logic.c
    {
        public static float StopMove = 1073741825;

        #region 在这里实例化logic所有需要用到的资源

        public static bool LogicThreadLife = true;//线程使能
        public BoardCtrllerManager movedriverZm = new BoardCtrllerManager();//板卡
        public Thread LogicThread;//逻辑线程


        System.DateTime[] mytime_start = new DateTime[2];//开始时间
        System.DateTime[] mytime_end = new DateTime[2];//结束时间
        System.TimeSpan[] TimeCount = new TimeSpan[2];//时间计数
        public static double[] time = new double[2] { 0, 0 };

        #endregion

        public VisionAPIDef VisionAPI { get; set; }					//视觉提供的操作接口

        public LogicAPIDef LogicAPI { get; set; }					//下位提供的模块接口
        public SettingDataDef LogicData { get; set; }				//下位模块运行所需要的数据

        public ProcessDataDef ProcessData { get; set; }				//过程数据，流程运行的临时数据，比如说料盒到第几层啦，点胶到第几个点
        public LogicTaskDef LogicTask { get; set; }					//上位的流程任务集

        public FsmDef FSM { get; set; }                             //状态机，只提供状态切换和当前状态

        public LogicMain()
        {
            VisionAPI = new VisionAPIDef();//实例化视觉接口

            LogicAPI = new LogicAPIDef(movedriverZm);//实例化逻辑接口
            LogicData = new SettingDataDef();//实例化逻辑数据

            ProcessData = new ProcessDataDef();//实力化过程数据
            LogicTask = new LogicTaskDef();//实例化逻辑任务
            PolishBusy = false;

            FSM = new FsmDef(movedriverZm);//实例化状态机


            LogicThread = new Thread(LogicThreadFunc);//线程实例化
            LogicThread.IsBackground = true;//后台线程实例化
            LogicThread.Start();//线程开始

            VisionProject.Instance.visionAPIDef = VisionAPI;
        }

        /// <summary>
        /// 触发打磨
        /// </summary>
        /// <param name="index"></param>
        /// <param name="templateIndex"></param>
        /// <returns></returns>
        public bool TriggerPolish(int index, int templateIndex)//
        {
            
            if (templateIndex == -1 && FormMain.PanoramaEnd)
                return false;

            if (index == 0)
            {
                return VisionAPI.TriggerCamera2(templateIndex);
            }
            else
            {
                return VisionAPI.TriggerCamera3(templateIndex);
            }
        }

        /// <summary>
        /// 触发上锡
        /// </summary>
        /// <param name="index">正反面</param>
        /// <param name="templateIndex"></param>
        /// <returns></returns>
        public bool TriggerSolder(int index, int templateIndex)//
        {
            if (templateIndex == -1 && FormMain.PanoramaEnd)
                return false;

            if (index == 0)
            {
                return VisionAPI.TriggerCamera0(templateIndex);
            }
            else
            {
                return VisionAPI.TriggerCamera1(templateIndex);
            }
        }


        #region 在这里写流程模块

        public TaskDef[] MainCtl = new TaskDef[2] { new TaskDef(), new TaskDef() };
        public string[] ID = new string[2] { "", "" };
        public DialogResult[] results = new DialogResult[] { DialogResult.None, DialogResult.None };

        public bool PolishBusy = false;//老化流程

        /// <summary>
        /// 读取平台工作
        /// </summary>
        void ReadLogic()
        {
            FsmStaDef stadef = FSM.GetStatus();//下位机寄存器读写
            if (stadef == FsmStaDef.INIT || stadef == FsmStaDef.SCRAM || stadef == FsmStaDef.RESET)
            {
                if(LogicTask.WorkTask[0].execute == 1|| LogicTask.WorkTask[1].execute == 1)
                    Tools.WriteLog.AddLog("设备切换到：" + stadef.ToString()+",运行动作清零");

                LogicTask = new LogicTaskDef();
                PolishBusy = false;
            }

            if (FSM.RunMode == RunModeDef.AGING)//老化
            {
                if (stadef == FsmStaDef.RUN)
                {
                    LogicTask.WorkTask[0].execute = 1;//一直运行
                    LogicTask.WorkTask[1].execute = 1;//一直运行
                }
            }
            else
            {
                if (movedriverZm.Y_Start.IntValue[0] == 1)//左边工作
                {
                    if (stadef == FsmStaDef.RUN)
                        LogicTask.WorkTask[0].execute = 1;

                    movedriverZm.Y_Start.IntValue[0] = 0;
                    movedriverZm.WriteRegister(new BaseData((ushort)1502, new int[] { 0 }));//写寄存器
                }

                if (movedriverZm.Y_Start.IntValue[1] == 1)
                {
                    if (stadef == FsmStaDef.RUN)
                        LogicTask.WorkTask[1].execute = 1;

                    movedriverZm.Y_Start.IntValue[1] = 0;
                    movedriverZm.WriteRegister(new BaseData((ushort)1504, new int[] { 0 }));
                }
            }
        }

        /// <summary>
        /// 主调度
        /// </summary>
        void WorkLogic(TaskDef my)
        {
            my.Start();//启动
            switch (my.step)
            {
                case 1://确定时间/两边的打磨状态/写日志
                    mytime_start[my.ID] = DateTime.Now;
                    if (LogicTask.PolishTask[my.ID].execute == 0 && LogicTask.SolderTask[my.ID].execute == 0)
                    {
                        LogicTask.PolishTask[my.ID].execute = 1;
                        Tools.WriteLog.AddLog(my.ID.ToString() + "边打磨");
                        my.step = 2;
                    }
                    break;
                case 2://一边打磨结束后开始焊锡/写日志
                    if (LogicTask.PolishTask[my.ID].execute == 0)
                    {
                        LogicTask.SolderTask[my.ID].execute = 1;
                        Tools.WriteLog.AddLog(my.ID.ToString() + "边上锡");
                        my.step = 3;
                    }
                    break;
                case 3://上锡结束/确定相关时间/写日志
                    if (LogicTask.SolderTask[my.ID].execute == 0)
                    {
                        mytime_end[my.ID] = DateTime.Now;
                        TimeCount[my.ID] = mytime_end[my.ID] - mytime_start[my.ID];
                        time[my.ID] = TimeCount[my.ID].TotalSeconds;//记录焊点时间
                        FormMain.uph_list.Add(new uphDef((float)time[my.ID], my.ID));//显示

                        Tools.WriteLog.AddLog(my.ID.ToString() + "边工作完结，耗时："+ time[my.ID].ToString());

                        my.End();
                    }
                    break;
            }
        }
        /// <summary>
        /// 打磨流程
        /// </summary>
        /// <param name="my"></param>
        wPointF[] pP = new wPointF[2] { new wPointF(), new wPointF() };
        void PolishLogic(TaskDef my)
        {
            my.Start();
            switch (my.step)
            {
                case 1://不是老化/打磨计数大于0/清除数据/正反面分类/写日志
                    if (!PolishBusy)
                    {
                        if (LogicData.RunData.wPointFs_Polish[my.ID].Count > 0)
                        {
                            PolishBusy = true;//忙碌状态
                            ProcessData.wPointFs_PolishV[my.ID].Clear();
                            ProcessData.wPointFs_PolishF[my.ID].Clear();

                            ProcessData.SolderList_EnableF[my.ID].Clear();
                            ProcessData.SolderList_EnableV[my.ID].Clear();

                            #region 分类

                            foreach (wPointF p in LogicData.RunData.wPointFs_Polish[my.ID])
                            {
                                if (!p.enable)
                                {
                                    if (p.T == 1)
                                    {
                                        wPointF pos = new wPointF();
                                        pos.X = p.X;
                                        pos.Y = p.Y;
                                        pos.T = p.T;
                                        pos.templateIndex = p.templateIndex;
                                        ProcessData.wPointFs_PolishV[my.ID].Add(pos);

                                        bool aa = false;
                                        ProcessData.SolderList_EnableV[my.ID].Add(aa);
                                    }
                                    else
                                    {
                                        wPointF pos = new wPointF();
                                        pos.X = p.X;
                                        pos.Y = p.Y;
                                        pos.T = p.T;
                                        pos.templateIndex = p.templateIndex;
                                        ProcessData.wPointFs_PolishF[my.ID].Add(pos);

                                        bool aa = false;
                                        ProcessData.SolderList_EnableF[my.ID].Add(aa);
                                    }
                                }

                            }

                            #endregion

                            ProcessData.PolishList[my.ID].Clear();
                            VisionAPI.Polishs(my.ID).Clear();

                            Tools.WriteLog.AddLog(my.ID.ToString() + "开始打磨正面拍照");
                            my.step = 2;
                        }
                        else
                        {
                            my.End();
                        }
                    }
                    break;

                #region 正面
                case 2://到拍照位
                    if (my.cnt < ProcessData.wPointFs_PolishF[my.ID].Count)//还没有走完牌照位
                    {
                        if (LogicAPI.polishcameras.sta() && LogicAPI.polishcameras.start != 1)//到位
                        {
                            pP[my.ID] = ProcessData.wPointFs_PolishF[my.ID][my.cnt];

                            int end = 0;
                            if(ProcessData.wPointFs_PolishF[my.ID].Count <= my.cnt + 1)//一面的最后一个点
                            {
                                end = 1;//Z抬起
                            }

                            float Hight = 0;
                            if (my.ID == 0)//判断打磨Z的高度使用哪一边的高度
                            {
                                Hight = LogicData.slaverData.basics.polish_z_Lpos;
                            }
                            else
                            {
                                Hight = LogicData.slaverData.basics.polish_z_Rpos;
                            }

                            while (!LogicAPI.polishcameras.exe(my.ID, end, pP[my.ID].X, pP[my.ID].Y, Hight, 0, pP[my.ID].T))
                            {
                                Thread.Sleep(1);
                            }
                            my.step = 3;
                        }
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始打磨正面");//写日志
                        my.step = 4;
                    }
                    break;

                case 3://到位,拍照
                    if (LogicAPI.polishcameras.sta() && LogicAPI.polishcameras.start != 1)//到位
                    {
                        if (TriggerPolish(my.ID, pP[my.ID].templateIndex))//拍照
                        {
                            List<PolishPosdata> polishorderlistF = new List<PolishPosdata>();//实例化点队列
                            if (ProcessData.wPointFs_PolishF[my.ID].Count <= my.cnt + 1 )
                            {
                                int end = 1;
                                movedriverZm.WriteRegister(new BaseData(1626, new int[] { 1 }));
                            }

                            foreach (VisionResult result in VisionAPI.Polishs(my.ID))//获取视觉结果
                            {
                                int type = result.Type;

                                foreach (PolishPosdata p in LogicData.vData.polishdata(my.ID)[type].pos)//获取打磨数据位置
                                {
                                    PolishPosdata _pos = new PolishPosdata();
                                    _pos.pos.X = result.X + p.pos.X + pP[my.ID].X;
                                    _pos.pos.Y = result.Y + p.pos.Y + pP[my.ID].Y;
                                    _pos.pos.R = p.pos.R;
                                    _pos.pos.Z = p.pos.Z;
                                    _pos.polishDef = p.polishDef.Clone();
                                    polishorderlistF.Add(_pos);//增加到list里
                                    Debug.WriteLine(string.Format("X:{0}，Y:{1}", _pos.pos.X, _pos.pos.Y));
                                }
                            }

                            foreach (PolishPosdata data in polishorderlistF.OrderBy(a => a.pos.X).ThenBy(a => a.pos.Y))//对list里的点进行排序
                            {
                                ProcessData.PolishList[my.ID].Add(data);//把排列好的点写进打磨list里
                            }



                            ProcessData.SolderList_EnableF[my.ID][my.cnt] = true;
                            my.cnt++;
                            my.step = 2;

                            //ProcessData.SolderList_EnableF[my.ID][my.cnt] = true;

                        }
                        else
                        {
                            if (ProcessData.wPointFs_PolishF[my.ID].Count <= my.cnt + 1)//是不是一面的最后一个点
                            {
                                int end = 1;
                                movedriverZm.WriteRegister(new BaseData(1626, new int[] { 1 }));//给下位机写抬到安全高度标志
                            }
                            my.cnt++;
                            my.step = 2;
                        }

                    }
                    break;
                case 20:
                    if (results[my.ID] == DialogResult.Yes)
                    {
                        my.step = 3;
                    }
                    else if (results[my.ID] == DialogResult.No)
                    {
                        foreach (VisionResult result in VisionAPI.Polishs(my.ID))
                        {
                            int type = result.Type;

                            foreach (PolishPosdata p in LogicData.vData.polishdata(my.ID)[type].pos)
                            {
                                PolishPosdata _pos = new PolishPosdata();
                                _pos.pos.X = result.X + p.pos.X + pP[my.ID].X;
                                _pos.pos.Y = result.Y + p.pos.Y + pP[my.ID].Y;
                                _pos.pos.R = p.pos.R;
                                _pos.pos.Z = p.pos.Z;
                                _pos.polishDef = p.polishDef.Clone();

                                ProcessData.PolishList[my.ID].Add(_pos);
                            }
                        }
                        my.cnt++;
                        my.step = 2;
                    }
                    else if (results[my.ID] == DialogResult.Cancel)
                    {
                        my.cnt++;
                        my.step = 2;

                    }
                    break;

                case 4:
                    if (ProcessData.PolishList[my.ID].Count > 0)//有打磨点
                    {
                        PolishPosdata p = ProcessData.PolishList[my.ID][0];//实例化
                        int up = 0;//0:开打磨头1：关打磨头

                        if (ProcessData.PolishList[my.ID].Count <= 1)//最后一个打磨点
                        {
                            p.polishDef.LiftHeight = p.pos.Z - LogicData.slaverData.basics.Safe_Z;//高度
                            up = 1;
                        }

                        while (!LogicAPI.polish.exe((int)AxisDef.AxX3,
                            ((int)AxisDef.AxY1 + my.ID * 6),
                            (int)AxisDef.AxZ3,
                            (int)AxisDef.AxR3,
                            ((int)AxisDef.AxT1 + my.ID * 6),
                            p.pos.X, p.pos.Y, p.pos.Z, p.pos.R, 2, p.polishDef, up))
                        {
                            Thread.Sleep(1);
                        }

                        my.step = 5;
                    }
                    else
                    {
                        VisionAPI.Polishs(my.ID).Clear();
                        ProcessData.PolishList[my.ID].Clear();

                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始打磨反面拍照");
                        my.step = 6;
                        my.cnt = 0;
                    }
                    break;
                case 5:
                    if (LogicAPI.polish.sta() && LogicAPI.polish.start != 1)
                    {
                        ProcessData.PolishList[my.ID].RemoveAt(0);//删除该点
                        my.step = 4;
                    }
                    break;
                #endregion

                #region 反面
                case 6://到拍照位
                    if (my.cnt < ProcessData.wPointFs_PolishV[my.ID].Count)//我的计数小于我统计的数字
                    {
                        if (LogicAPI.polishcameras.sta() && LogicAPI.polishcameras.start != 1)//到位置
                        {
                            pP[my.ID] = ProcessData.wPointFs_PolishV[my.ID][my.cnt];

                            int end = 0;
                            if (ProcessData.wPointFs_PolishV[my.ID].Count <= my.cnt+1)//是不是最后一个点
                            {
                                end = 1;
                            }

                            float Hight = 0;
                            if (my.ID == 0)//选择哪边的Z轴高度
                            {
                                Hight = LogicData.slaverData.basics.polish_z_Lpos;
                            }
                            else
                            {
                                Hight = LogicData.slaverData.basics.polish_z_Rpos;
                            }

                            while (!LogicAPI.polishcameras.exe(my.ID, end, pP[my.ID].X, pP[my.ID].Y, Hight, 0, pP[my.ID].T))
                            {
                                Thread.Sleep(1);
                            }
                            my.step = 7;
                        }
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始打磨反面");
                        my.step = 8;
                    }
                    break;

                case 7://到位,拍照
                    if (LogicAPI.polishcameras.sta() && LogicAPI.polishcameras.start != 1)//到位
                    {
                        if (TriggerPolish(my.ID, pP[my.ID].templateIndex))//触发拍照
                        {
                            List<PolishPosdata> polishorderlistV = new List<PolishPosdata>();//增加list
                            if (ProcessData.wPointFs_PolishV[my.ID].Count <= my.cnt + 1)//是不是最后一个点
                            {
                                int end = 1;
                                movedriverZm.WriteRegister(new BaseData(1626, new int[] { 1 }));
                            }
                            foreach (VisionResult result in VisionAPI.Polishs(my.ID))//遍历视觉结果
                            {
                                int type = result.Type;
                                
                                foreach (PolishPosdata p in LogicData.vData.polishdata(my.ID)[type].pos)//遍历打磨点位置
                                {
                                    PolishPosdata _pos = new PolishPosdata();
                                    _pos.pos.X = result.X + p.pos.X + pP[my.ID].X;
                                    _pos.pos.Y = result.Y + p.pos.Y + pP[my.ID].Y;
                                    _pos.pos.R = p.pos.R;
                                    _pos.pos.Z = p.pos.Z;
                                    _pos.polishDef = p.polishDef.Clone();
                                    polishorderlistV.Add(_pos);//添加进list
                                    Debug.WriteLine(string.Format("X:{0}，Y:{1}",_pos.pos.X, _pos.pos.Y)); 
                                }
                            }

                            foreach (PolishPosdata data in polishorderlistV.OrderBy(a => a.pos.X).ThenBy(a => a.pos.Y))//遍历排列后的list
                            {
                                ProcessData.PolishList[my.ID].Add(data);//加进打磨列表里
                            }

                            ProcessData.SolderList_EnableV[my.ID][my.cnt] = true;

                            my.cnt++;
                            my.step = 6;


                            //ProcessData.SolderList_EnableV[my.ID][my.cnt] = true;
                        }
                        else
                        {
                            if (ProcessData.wPointFs_PolishV[my.ID].Count <= my.cnt + 1)
                            {
                                int end = 1;
                                movedriverZm.WriteRegister(new BaseData(1626, new int[] { 1 }));//结束
                            }
                            my.cnt++;
                            my.step = 6;
                        }
                    }
                    break;
                case 60:
                    if (results[my.ID] == DialogResult.Yes)
                    {
                        my.step = 7;
                    }
                    else if (results[my.ID] == DialogResult.No)
                    {
                        foreach (VisionResult result in VisionAPI.Polishs(my.ID))
                        {
                            int type = result.Type;
                            foreach (PolishPosdata p in LogicData.vData.polishdata(my.ID)[type].pos)
                            {
                                PolishPosdata _pos = new PolishPosdata();
                                _pos.pos.X = result.X + p.pos.X + pP[my.ID].X;
                                _pos.pos.Y = result.Y + p.pos.Y + pP[my.ID].Y;
                                _pos.pos.R = p.pos.R;
                                _pos.pos.Z = p.pos.Z;
                                _pos.polishDef = p.polishDef.Clone();

                                ProcessData.PolishList[my.ID].Add(_pos);
                            }
                        }
                        my.cnt++;
                        my.step = 6;
                    }
                    else if (results[my.ID] == DialogResult.Cancel)
                    {
                        my.cnt++;
                        my.step = 6;

                    }
                    break;


                case 8:
                    if (ProcessData.PolishList[my.ID].Count > 0)
                    {
                        PolishPosdata p = ProcessData.PolishList[my.ID][0];
                        int up = 0;//0:开打磨头1：关打磨头

                        if (ProcessData.PolishList[my.ID].Count <= 1)
                        {
                            p.polishDef.LiftHeight = p.pos.Z - LogicData.slaverData.basics.Safe_Z;
                            up = 1;
                        }

                        while (!LogicAPI.polish.exe((int)AxisDef.AxX3,
                            ((int)AxisDef.AxY1 + my.ID * 6),
                            (int)AxisDef.AxZ3,
                            (int)AxisDef.AxR3,
                            ((int)AxisDef.AxT1 + my.ID * 6),
                            p.pos.X, p.pos.Y, p.pos.Z, p.pos.R, 2, p.polishDef, up))
                        {
                            Thread.Sleep(1);
                        }
                        my.step = 9;
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "打磨反面结束");
                        ProcessData.PolishList[my.ID].Clear();
                        PolishBusy = false;
                        my.End();
                    }
                    break;
                case 9:
                    if (LogicAPI.polish.sta() && LogicAPI.polish.start != 1)
                    {
                        ProcessData.PolishList[my.ID].RemoveAt(0);//删除该点
                        my.step = 8;
                    }
                    break;
                    #endregion

            }
        }
        
        /// <summary>
        /// 上锡流程
        /// </summary>
        /// <param name="my"></param>
        /// 

        wPointF[] pS = new wPointF[2] { new wPointF(), new wPointF() };
        int[] cnt = new int[2] { 0, 0 };
        void SolderLogic(TaskDef my)
        {
            my.Start();//开始
            switch (my.step)
            {
                case 1://有焊锡点/数据清除/分类/列表清除/写日志
                    if (LogicData.RunData.wPointFs_Solder[my.ID].Count > 0)
                    {
                        ProcessData.wPointFs_SolderV[my.ID].Clear();
                        ProcessData.wPointFs_SolderF[my.ID].Clear();
                        cnt[my.ID] = 0;

                        #region 分类

                        foreach (wPointF p in LogicData.RunData.wPointFs_Solder[my.ID])
                        {
                            if (!p.enable)
                            {
                                if (p.T == 1)
                                {
                                    wPointF pos = new wPointF();
                                    pos.X = p.X;
                                    pos.Y = p.Y;
                                    pos.T = p.T;
                                    pos.templateIndex = p.templateIndex;
                                    ProcessData.wPointFs_SolderV[my.ID].Add(pos);
                                }
                                else
                                {
                                    wPointF pos = new wPointF();
                                    pos.X = p.X;
                                    pos.Y = p.Y;
                                    pos.T = p.T;
                                    pos.templateIndex = p.templateIndex;
                                    ProcessData.wPointFs_SolderF[my.ID].Add(pos);
                                }
                            }
                        }

                        #endregion

                        ProcessData.SolderList[my.ID].Clear();
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡反面拍照");
                        my.step = 6;
                    }
                    else
                    {

                        Tools.WriteLog.AddLog(my.ID.ToString() + "焊锡没点");//没焊点
                        my.End();//结束
                        while (!LogicAPI.PlatformMove[my.ID].exe(((int)AxisDef.AxX1 + my.ID * 6),
                                                        ((int)AxisDef.AxY1 + my.ID * 6),
                                                        ((int)AxisDef.AxZ1 + my.ID * 6),
                                                        ((int)AxisDef.AxR1 + my.ID * 6),
                                                        ((int)AxisDef.AxT1 + my.ID * 6),
                                                        LogicData.slaverData.endPosS(my.ID).X,
                                                        LogicData.slaverData.endPosS(my.ID).Y,
                                                        LogicData.slaverData.endPosS(my.ID).Z,
                                                        LogicData.slaverData.endPosS(my.ID).R,
                                                        LogicData.slaverData.endPosS(my.ID).T, 0))
                        {
                            Thread.Sleep(1);
                        }
                    }
                    break;

                #region 正面
                case 2://到拍照位
                    if (my.cnt < ProcessData.wPointFs_SolderF[my.ID].Count)//还有拍照位没有拍到
                    {
                        if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)//移动到位置
                        {
                            if (FSM.RunMode == RunModeDef.AGING)//老化
                            {
                                pS[my.ID] = ProcessData.wPointFs_SolderF[my.ID][my.cnt];
                                float safe_Z = 0;
                                if (my.ID == 0)//选择哪一边
                                {
                                    safe_Z = LogicData.slaverData.basics.Safe_ZL;
                                }
                                else
                                {
                                    safe_Z = LogicData.slaverData.basics.Safe_ZR;
                                }
                                while (!LogicAPI.PlatformMove[my.ID].exe(((int)AxisDef.AxX1 + my.ID * 6),
                                                            ((int)AxisDef.AxY1 + my.ID * 6),
                                                            ((int)AxisDef.AxZ1 + my.ID * 6),
                                                            ((int)AxisDef.AxR1 + my.ID * 6),
                                                            ((int)AxisDef.AxT1 + my.ID * 6),
                                                            pS[my.ID].X, pS[my.ID].Y, safe_Z,
                                                            0, pS[my.ID].T, 0))
                                {
                                    Thread.Sleep(1);
                                }
                                my.step = 3;
                            }
                            else
                            {
                                if (my.cnt >= ProcessData.SolderList_EnableF[my.ID].Count)//防止越界
                                {
                                    my.cnt++;
                                    my.step = 2;
                                    break;
                                }
                                if (ProcessData.SolderList_EnableF[my.ID][my.cnt])
                                {
                                    pS[my.ID] = ProcessData.wPointFs_SolderF[my.ID][my.cnt];
                                    float safe_Z = 0;
                                    if (my.ID == 0)//选择哪一边
                                    {
                                        safe_Z = LogicData.slaverData.basics.Safe_ZL;
                                    }
                                    else
                                    {
                                        safe_Z = LogicData.slaverData.basics.Safe_ZR;
                                    }
                                    while (!LogicAPI.PlatformMove[my.ID].exe(((int)AxisDef.AxX1 + my.ID * 6),
                                                                ((int)AxisDef.AxY1 + my.ID * 6),
                                                                ((int)AxisDef.AxZ1 + my.ID * 6),
                                                                ((int)AxisDef.AxR1 + my.ID * 6),
                                                                ((int)AxisDef.AxT1 + my.ID * 6),
                                                                pS[my.ID].X, pS[my.ID].Y, safe_Z,
                                                                0, pS[my.ID].T, 0))
                                    {
                                        Thread.Sleep(1);
                                    }
                                    my.step = 3;
                                }
                                else
                                {
                                    my.cnt++;
                                    my.step = 2;
                                }
                            }
                        }
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡正面");

                        my.cnt = 0;
                        my.step = 4;
                    }
                    break;

                case 3://到位,拍照
                    if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)//到位
                    {
                        if (TriggerSolder(my.ID, pS[my.ID].templateIndex))//触发拍照
                        {
                            List<SolderPosdata> orderlistF = new List<SolderPosdata>();//实例化list

                            int count = ProcessData.SolderList[my.ID].Count;
                            foreach (VisionResult result in VisionAPI.Solders(my.ID))//遍历视觉结果
                            {
                                int type = result.Type;
                                foreach (SolderPosdata p in LogicData.vData.soliderdata(my.ID)[type].pos)//遍历上锡位置
                                {
                                    float cAng = (float)(result.R * Math.PI/180);

                                    float x = 0;
                                    float y = 0;

                                    float Tx = 0;
                                    float Ty = 0;
                                    float Tr = 0;

                                    if (LogicData.RunData.rotate(my.ID))
                                    {
                                        x = result.X + pS[my.ID].X;
                                        y = result.Y + pS[my.ID].Y;
                                        Tr = result.R + p.pos.R;

                                        Transorm((UsingPlatformSelect)my.ID, x, y, p.pos.X + x, p.pos.Y + y, p.pos.R, cAng, out Tx, out Ty);

                                    }
                                    else
                                    {
                                        Tx = result.X + p.pos.X + pS[my.ID].X;
                                        Ty = result.Y + p.pos.Y + pS[my.ID].Y;
                                        Tr = p.pos.R;
                                    }
                                    
                                    SolderPosdata _pos = new SolderPosdata();
                                    _pos.pos.X = Tx;
                                    _pos.pos.Y = Ty;
                                    _pos.pos.R = Tr;
                                    _pos.pos.Z = p.pos.Z;
                                    _pos.solderDef = p.solderDef.Clone();

                                    Debug.WriteLine(string.Format("取得视觉结果：X={0}，Y={1}，R={2}，ID={3}", result.X, result.Y, result.R, result.Type));
                                    Debug.WriteLine(string.Format("运行工作位：X={0}，Y={1}，R={2}", _pos.pos.X, _pos.pos.Y, _pos.pos.R));


                                    if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                    {
                                        _pos.rinse = true;
                                    }

                                    orderlistF.Add(_pos);
                                    Debug.WriteLine(string.Format("X:{0}，Y:{1}", _pos.pos.X, _pos.pos.Y));
                                }
                            }

                            foreach (SolderPosdata data in orderlistF.OrderBy(a => a.pos.X).ThenBy(a => a.pos.Y))//排序
                            {
                                ProcessData.SolderList[my.ID].Add(data);//加到上锡位置
                            }
                            my.cnt++;
                            my.step = 2;
                        }
                        else
                        {
                            my.cnt++;
                            my.step = 2;
                        }
                    }
                    break;
                case 4:
                    if (ProcessData.SolderList[my.ID].Count > 0)
                    {
                        SolderPosdata p = ProcessData.SolderList[my.ID][0];

                        if (LogicData.RunData.rinseMode == 2 && my.cnt == 0)
                        {
                            p.rinse = true;
                        }

                        my.cnt++;
                        if (ProcessData.SolderList[my.ID].Count <= 1)
                        {
                            p.solderDef.LiftHeight = p.pos.Z - LogicData.slaverData.basics.Safe_ZL;
                        }

                        if ((cnt[my.ID] % (LogicData.RunData.clearnum == 0 ? 1 : LogicData.RunData.clearnum) == 0) && LogicData.RunData.rinseMode == 3)//每几个点去清洗
                        {
                            p.rinse = true;
                        }


                        while (!LogicAPI.solderTin[my.ID].exe(0, p.pos.X, p.pos.Y, p.pos.Z, p.pos.R, 2, p.solderDef, p.rinse ? 1 : 0))
                        {
                            Thread.Sleep(1);
                        }
                        cnt[my.ID]++;
                        my.step = 5;
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "焊锡正面结束");
                        my.End();
                        while (!LogicAPI.PlatformMove[my.ID].exe(((int)AxisDef.AxX1 + my.ID * 6),
                                                       ((int)AxisDef.AxY1 + my.ID * 6),
                                                       ((int)AxisDef.AxZ1 + my.ID * 6),
                                                       ((int)AxisDef.AxR1 + my.ID * 6),
                                                       ((int)AxisDef.AxT1 + my.ID * 6),
                                                       LogicData.slaverData.endPosS(my.ID).X,
                                                       LogicData.slaverData.endPosS(my.ID).Y,
                                                       LogicData.slaverData.endPosS(my.ID).Z,
                                                       LogicData.slaverData.endPosS(my.ID).R,
                                                       LogicData.slaverData.endPosS(my.ID).T, 0))
                        {
                            Thread.Sleep(1);
                        }
                    }
                    break;
                case 5:
                    if (LogicAPI.solderTin[my.ID].sta() && LogicAPI.solderTin[my.ID].start != 1)
                    {
                        Thread.Sleep(100);
                        ProcessData.SolderList[my.ID].RemoveAt(0);//删除该点
                        my.step = 4;
                    }
                    break;
                #endregion

                #region 反面
                case 6://到拍照位
                    if (my.cnt < ProcessData.wPointFs_SolderV[my.ID].Count)//还有拍照位没有拍
                    {
                        if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)//到位
                        {
                            if (FSM.RunMode == RunModeDef.AGING)//老化
                            {
                                pS[my.ID] = ProcessData.wPointFs_SolderV[my.ID][my.cnt];
                                float safe_Z = 0;
                                if (my.ID == 0)//选择哪一边的Z高度
                                {
                                    safe_Z = LogicData.slaverData.basics.Safe_ZL;
                                }
                                else
                                {
                                    safe_Z = LogicData.slaverData.basics.Safe_ZR;
                                }
                                while (!LogicAPI.PlatformMove[my.ID].exe(((int)AxisDef.AxX1 + my.ID * 6),
                                                            ((int)AxisDef.AxY1 + my.ID * 6),
                                                            ((int)AxisDef.AxZ1 + my.ID * 6),
                                                            ((int)AxisDef.AxR1 + my.ID * 6),
                                                            ((int)AxisDef.AxT1 + my.ID * 6),
                                                            pS[my.ID].X, pS[my.ID].Y, safe_Z, 0, pS[my.ID].T, 0))
                                {
                                    Thread.Sleep(1);
                                }
                                my.step = 7;
                            }
                            else
                            {
                                if (my.cnt >= ProcessData.SolderList_EnableV[my.ID].Count)//防止越界
                                {
                                    my.cnt++;
                                    my.step = 6;
                                    break;
                                }

                                if (ProcessData.SolderList_EnableV[my.ID][my.cnt])
                                {
                                    pS[my.ID] = ProcessData.wPointFs_SolderV[my.ID][my.cnt];
                                    float safe_Z = 0;
                                    if (my.ID == 0)//选择哪一边的Z高度
                                    {
                                        safe_Z = LogicData.slaverData.basics.Safe_ZL;
                                    }
                                    else
                                    {
                                        safe_Z = LogicData.slaverData.basics.Safe_ZR;
                                    }
                                    while (!LogicAPI.PlatformMove[my.ID].exe(((int)AxisDef.AxX1 + my.ID * 6),
                                                                ((int)AxisDef.AxY1 + my.ID * 6),
                                                                ((int)AxisDef.AxZ1 + my.ID * 6),
                                                                ((int)AxisDef.AxR1 + my.ID * 6),
                                                                ((int)AxisDef.AxT1 + my.ID * 6),
                                                                pS[my.ID].X, pS[my.ID].Y, safe_Z, 0, pS[my.ID].T, 0))
                                    {
                                        Thread.Sleep(1);
                                    }
                                    my.step = 7;
                                }
                                else
                                {
                                    my.cnt++;
                                    my.step = 6;
                                }
                            }
                        }
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡反面");
                        my.cnt = 0;
                        my.step = 8;
                    }
                    break;

                case 7://到位,拍照
                    if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)//到位
                    {
                        if (TriggerSolder(my.ID, pS[my.ID].templateIndex))//触发拍照
                        {
                            List<SolderPosdata> orderlistV = new List<SolderPosdata>();//实例化
                            int count = ProcessData.SolderList[my.ID].Count;

                            #region 遍历视觉结果
                            foreach (VisionResult result in VisionAPI.Solders(my.ID))//
                            {
                                int type = result.Type;
                               
                                foreach (SolderPosdata p in LogicData.vData.soliderdata(my.ID)[type].pos)//遍历上锡点位置
                                {

                                    float cAng = (float)(result.R * Math.PI / 180);

                                    float x = 0;
                                    float y = 0;

                                    float Tx = 0;
                                    float Ty = 0;
                                    float Tr = 0;

                                    if (LogicData.RunData.rotate(my.ID))
                                    {
                                        x = result.X + pS[my.ID].X;
                                        y = result.Y + pS[my.ID].Y;
                                        Tr = result.R + p.pos.R;

                                        Transorm((UsingPlatformSelect)my.ID, x, y, p.pos.X + x, p.pos.Y + y, p.pos.R, cAng, out Tx, out Ty);

                                    }
                                    else
                                    {
                                        Tx = result.X + p.pos.X + pS[my.ID].X;
                                        Ty = result.Y + p.pos.Y + pS[my.ID].Y;
                                        Tr = p.pos.R;
                                    }

                                    SolderPosdata _pos = new SolderPosdata();
                                    _pos.pos.X = Tx;
                                    _pos.pos.Y = Ty;
                                    _pos.pos.R = Tr;
                                    _pos.pos.Z = p.pos.Z;
                                    _pos.solderDef = p.solderDef.Clone();

                                    Debug.WriteLine(string.Format("取得视觉结果：X={0}，Y={1}，R={2}，ID={3}", result.X, result.Y, result.R, result.Type));
                                    Debug.WriteLine(string.Format("运行工作位：X={0}，Y={1}，R={2}", _pos.pos.X, _pos.pos.Y, _pos.pos.R));


                                    if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                    {
                                        _pos.rinse = true;
                                    }
                                    orderlistV.Add(_pos);//加到list里
                                    Debug.WriteLine(string.Format("X:{0}，Y:{1}", _pos.pos.X, _pos.pos.Y));
                                }
                            }
                            #endregion

                            foreach (SolderPosdata data in orderlistV.OrderBy(a => a.pos.X).ThenBy(a => a.pos.Y))//排序
                            {
                                ProcessData.SolderList[my.ID].Add(data);//加到上锡列表
                            }
                            
                            my.cnt++;
                            my.step = 6;
                        }
                        else
                        {
                            //results[my.ID] = DialogResult.None;
                            //new Thread(() =>
                            //{
                            //    movedriverZm.WriteRegister(new BaseData((ushort)(1604 + 2 * my.ID), new int[] { 1 }));
                            //    results[my.ID] = MessageBox.Show("平台" + my.ID.ToString() + "识别失败,是否重新拍照\n\r\n\r确定：重新拍照\n\r\n\r否：强制工作\n\r\n\r取消：跳过该点", "识别错误", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                            //    movedriverZm.WriteRegister(new BaseData((ushort)(1604 + 2 * my.ID), new int[] { 0 }));
                            //}).Start();
                            //my.step = 60;

                            my.cnt++;
                            my.step = 6;
                        }
                    }
                    break;
                case 60:
                    if (results[my.ID] == DialogResult.Yes)
                    {
                        my.step = 7;
                    }
                    else if (results[my.ID] == DialogResult.No)
                    {
                        int count = ProcessData.SolderList[my.ID].Count;
                        foreach (VisionResult result in VisionAPI.Solders(my.ID))
                        {
                            int type = result.Type;

                            foreach (SolderPosdata p in LogicData.vData.soliderdata(my.ID)[type].pos)
                            {

                                float cAng = (float)(result.R * Math.PI / 180);

                                float x = 0;
                                float y = 0;

                                float Tx = 0;
                                float Ty = 0;
                                float Tr = 0;

                                if (LogicData.RunData.rotate(my.ID))
                                {
                                    x = result.X + pS[my.ID].X;
                                    y = result.Y + pS[my.ID].Y;
                                    Tr = result.R + p.pos.R;

                                    Transorm((UsingPlatformSelect)my.ID, x, y, p.pos.X + x, p.pos.Y + y, p.pos.R, cAng, out Tx, out Ty);

                                }
                                else
                                {
                                    Tx = result.X + p.pos.X + pS[my.ID].X;
                                    Ty = result.Y + p.pos.Y + pS[my.ID].Y;
                                    Tr = p.pos.R;
                                }

                                SolderPosdata _pos = new SolderPosdata();
                                _pos.pos.X = Tx;
                                _pos.pos.Y = Ty;
                                _pos.pos.R = Tr;
                                _pos.pos.Z = p.pos.Z;
                                _pos.solderDef = p.solderDef.Clone();


                                if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                {
                                    _pos.rinse = true;
                                }

                                ProcessData.SolderList[my.ID].Add(_pos);
                            }
                        }
                        my.cnt++;
                        my.step = 6;
                    }
                    else if (results[my.ID] == DialogResult.Cancel)
                    {
                        my.cnt++;
                        my.step = 6;

                    }
                    break;

                case 8:
                    if (ProcessData.SolderList[my.ID].Count > 0)//有上锡点
                    {
                        SolderPosdata p = ProcessData.SolderList[my.ID][0];

                        if (LogicData.RunData.rinseMode == 2 && my.cnt == 0)
                        {
                            p.rinse = true;
                        }
                        my.cnt++;

                        if (ProcessData.SolderList[my.ID].Count <= 1)
                        {
                            p.solderDef.LiftHeight = p.pos.Z - LogicData.slaverData.basics.Safe_ZL;
                        }

                        if ((cnt[my.ID] % (LogicData.RunData.clearnum == 0 ? 1 : LogicData.RunData.clearnum) == 0) && LogicData.RunData.rinseMode == 3)//每几个点去清洗
                        {
                            p.rinse = true;
                        }

                        while (!LogicAPI.solderTin[my.ID].exe(0, p.pos.X, p.pos.Y, p.pos.Z, p.pos.R, 2, p.solderDef, p.rinse ? 1 : 0))
                        {
                            Thread.Sleep(1);
                        }
                        cnt[my.ID]++;
                        my.step = 9;
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡正面拍照");
                        ProcessData.SolderList[my.ID].Clear();
                        my.step = 2;
                        my.cnt = 0;
                    }
                    break;

                case 9:
                    if (LogicAPI.solderTin[my.ID].sta() && LogicAPI.solderTin[my.ID].start != 1)
                    {
                        Thread.Sleep(100);
                        ProcessData.SolderList[my.ID].RemoveAt(0);//删除该点
                        my.step = 8;
                    }
                    break;
                    #endregion

            }
        }

        #endregion

        #region 下位机数据块下发
        public void DataToSlaver()
        {
            List<byte> temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos.mode));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos.X));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos.Z));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos.R));
            movedriverZm.WriteRegister(new BaseData(5300, temp.ToArray()));


            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.StartRunMode));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.DevcieMode));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.TinDetectEn));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.CleanEn));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.ShakeEn));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.TurnAvoidPos_XL));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.TurnAvoidPos_XR));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.Safe_ZL));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.Safe_ZR));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.WeldSpeedL));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.WeldSpeedR));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.TeachSpeedL));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.TeachSpeedR));
            movedriverZm.WriteRegister(new BaseData(4024, temp.ToArray()));

            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.Safe_Z));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.PolishSpeed));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.TeachSpeed));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.PolishOffset));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.PolishTimes));

            movedriverZm.WriteRegister(new BaseData(4074, temp.ToArray()));

            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.basics.PolishBlowDelay));
            movedriverZm.WriteRegister(new BaseData(4088, temp.ToArray()));


            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos1.mode));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos1.X));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos1.Y));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos1.Z));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos1.R));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos1.T));
            movedriverZm.WriteRegister(new BaseData(4600, temp.ToArray()));

            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos2.mode));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos2.X));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos2.Y));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos2.Z));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos2.R));
            temp.AddRange(Functions.NetworkBytes(LogicData.slaverData.rstPos2.T));
            movedriverZm.WriteRegister(new BaseData(5000, temp.ToArray()));

            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posL.X));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posL.Y));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posL.Z));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posL.R));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posL.T));

            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.FrontLen));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.FrontSpeed));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.BackLen));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.BackSpeed));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.CleanTime));
            movedriverZm.WriteRegister(new BaseData(4506, temp.ToArray()));

            temp = new List<byte>();
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posR.X));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posR.Y));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posR.Z));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posR.R));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.posR.T));

            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.FrontLen));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.FrontSpeed));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.BackLen));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.BackSpeed));
            temp.AddRange(Functions.NetworkBytes(LogicData.rinseData.CleanTime));
            movedriverZm.WriteRegister(new BaseData(4906, temp.ToArray()));

        }
        #endregion

        /// <summary>
        /// 计算旋转后位置
        /// </summary>
        /// <param name="usingPlatform">哪边</param>
        /// <param name="X">相机中心位置</param>
        /// <param name="Y"></param>
        /// <param name="Sx">焊头位置</param>
        /// <param name="Sy"></param>
        /// <param name="Sr"></param>
        /// <param name="Ang"></param>
        /// <param name="Tx"></param>
        /// <param name="Ty"></param>
        public void Transorm(UsingPlatformSelect usingPlatform, float X, float Y, float Sx, float Sy, float Sr, float Ang, out float Tx, out float Ty)
        {
            TeachingMechinePra mechinePra = new TeachingMechinePra();

            PointF rotateCur = new PointF();
            rotateCur.X = Sx;
            rotateCur.Y = Sy;//装换前的角度和位置

            PointF rotateC = new PointF();//旋转中心
            double r = 0;


            if (usingPlatform == UsingPlatformSelect.Left)
            {
                mechinePra = LogicData.RunData.TeachingMechinePra_Left;
                r = Ang;
            }
            else 
            {
                mechinePra = LogicData.RunData.TeachingMechinePra_Right;
                r = -Ang;
            }

                rotateC.X = X - mechinePra.RotatePstionCameraSize.X;
                rotateC.Y = Y - mechinePra.RotatePstionCameraSize.Y;//计算旋转中心

            double radius = mechinePra.Radius;//圆心半径
            Circle circle = new Circle(rotateC, (float)radius);

            PointF pos = circle.Rotate(rotateCur, r);

            Tx = pos.X;
            Ty = pos.Y;

        }

        public void LogicThreadFunc()//逻辑最外层函数，需放在线程中一直运行，下位的Logic()
        {
            Stopwatch TC = new Stopwatch();
            while (LogicThreadLife)
            {
                try
                {
                    if (movedriverZm.Succeed == true)
                    {

                        ReadLogic();

                        if (FSM.GetStatus() == FsmStaDef.RUN)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                LogicTask.WorkTask[i].ID = i;
                                LogicTask.PolishTask[i].ID = i;
                                LogicTask.SolderTask[i].ID = i;

                                WorkLogic(LogicTask.WorkTask[i]);
                                PolishLogic(LogicTask.PolishTask[i]);
                                SolderLogic(LogicTask.SolderTask[i]);
                             }

                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Logic流程错误：" + ex.ToString());
                }
            }
        }
    }
    public enum UsingPlatformSelect
    {
        Left,
        Right
    };

}
