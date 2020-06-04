﻿using System;
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

namespace Logic
{
    public class TaskDef//每个任务运行的控制参数	下位的LogicParaDef
    {
        public int ID;
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


        public bool TriggerPolish(int index, int templateIndex)//出发打磨诱因
        {
            if (templateIndex == -1)
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
        public bool TriggerSolder(int index, int templateIndex)
        {
            if (templateIndex == -1)
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
            FsmStaDef stadef = FSM.GetStatus();
            if (stadef == FsmStaDef.INIT || stadef == FsmStaDef.SCRAM || stadef == FsmStaDef.RESET)
            {
                if(LogicTask.WorkTask[0].execute == 1|| LogicTask.WorkTask[1].execute == 1)
                    Tools.WriteLog.AddLog("设备切换到：" + stadef.ToString()+",运行动作清零");

                LogicTask = new LogicTaskDef();
                PolishBusy = false;
            }

            if (FSM.RunMode == RunModeDef.AGING)
            {
                if (stadef == FsmStaDef.RUN)
                {
                    LogicTask.WorkTask[0].execute = 1;
                    LogicTask.WorkTask[1].execute = 1;
                }
            }
            else
            {
                if (movedriverZm.Y_Start.IntValue[0] == 1)
                {
                    if (stadef == FsmStaDef.RUN)
                        LogicTask.WorkTask[0].execute = 1;

                    movedriverZm.Y_Start.IntValue[0] = 0;
                    movedriverZm.WriteRegister(new BaseData((ushort)1502, new int[] { 0 }));
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
            my.Start();
            switch (my.step)
            {
                case 1:
                    mytime_start[my.ID] = DateTime.Now;
                    if (LogicTask.PolishTask[my.ID].execute == 0 && LogicTask.SolderTask[my.ID].execute == 0)
                    {
                        LogicTask.PolishTask[my.ID].execute = 1;
                        Tools.WriteLog.AddLog(my.ID.ToString() + "边打磨");
                        my.step = 2;
                    }
                    break;
                case 2:
                    if (LogicTask.PolishTask[my.ID].execute == 0)
                    {
                        LogicTask.SolderTask[my.ID].execute = 1;
                        Tools.WriteLog.AddLog(my.ID.ToString() + "边上锡");
                        my.step = 3;
                    }
                    break;
                case 3:
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
                case 1:
                    if (!PolishBusy)
                    {
                        if (LogicData.RunData.wPointFs_Polish[my.ID].Count > 0)
                        {
                            PolishBusy = true;//忙碌状态
                            ProcessData.wPointFs_PolishV[my.ID].Clear();
                            ProcessData.wPointFs_PolishF[my.ID].Clear();

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
                                    }
                                    else
                                    {
                                        wPointF pos = new wPointF();
                                        pos.X = p.X;
                                        pos.Y = p.Y;
                                        pos.T = p.T;
                                        pos.templateIndex = p.templateIndex;
                                        ProcessData.wPointFs_PolishF[my.ID].Add(pos);
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
                    if (my.cnt < ProcessData.wPointFs_PolishF[my.ID].Count)
                    {
                        if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                        {
                            pP[my.ID] = ProcessData.wPointFs_PolishF[my.ID][my.cnt];
                            while (!LogicAPI.PlatformMove[my.ID].exe((int)AxisDef.AxX3,
                                ((int)AxisDef.AxY1 + my.ID * 6),
                                (int)AxisDef.AxZ3, (int)AxisDef.AxR3,
                                ((int)AxisDef.AxT1 + my.ID * 6),
                                pP[my.ID].X, pP[my.ID].Y, LogicData.slaverData.basics.Safe_Z, 0, pP[my.ID].T, 0))
                            {
                                Thread.Sleep(1);
                            }
                            my.step = 3;
                        }
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始打磨正面");
                        my.step = 4;
                    }
                    break;

                case 3://到位,拍照
                    if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                    {
                        if (TriggerPolish(my.ID, pP[my.ID].templateIndex))
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
                        else
                        {
                            //results[my.ID] = DialogResult.None; 
                            //new Thread(() =>
                            //{
                            //    movedriverZm.WriteRegister(new BaseData((ushort)(1604 + 2 * my.ID), new int[] { 1}));
                            //    results[my.ID] = MessageBox.Show("平台" + my.ID.ToString() + "识别失败,是否重新拍照\n\r\n\r确定：重新拍照\n\r\n\r否：强制工作\n\r\n\r取消：跳过该点", "识别错误", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                            //    movedriverZm.WriteRegister(new BaseData((ushort)(1604 + 2 * my.ID), new int[] { 0 }));
                            //}).Start();
                            //my.step = 20;

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
                    if (my.cnt < ProcessData.wPointFs_PolishV[my.ID].Count)
                    {
                        if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                        {
                            pP[my.ID] = ProcessData.wPointFs_PolishV[my.ID][my.cnt];
                            while (!LogicAPI.PlatformMove[my.ID].exe((int)AxisDef.AxX3,
                                ((int)AxisDef.AxY1 + my.ID * 6),
                                (int)AxisDef.AxZ3, (int)AxisDef.AxR3,
                                ((int)AxisDef.AxT1 + my.ID * 6),
                                pP[my.ID].X, pP[my.ID].Y, LogicData.slaverData.basics.Safe_Z, 0, pP[my.ID].T, 0))
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
                    if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                    {
                        if (TriggerPolish(my.ID, pP[my.ID].templateIndex))
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
                        //LogicAPI.PlatformMove[my.ID].exe((int)AxisDef.AxX3, ((int)AxisDef.AxY1 + my.ID * 6), (int)AxisDef.AxZ3, (int)AxisDef.AxR3, ((int)AxisDef.AxT1 + my.ID * 6), LogicData.slaverData.endPos.X, StopMove, LogicData.slaverData.endPos.Z, 0, 0);
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
            my.Start();
           // LogicData.slaverData.basics.Safe_ZR = LogicData.slaverData.basics.Safe_ZL;

            switch (my.step)
            {
                case 1:
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
                        ProcessData.SolderList_Ang[my.ID].Clear();

                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡反面拍照");
                        my.step = 6;
                    }
                    else
                    {

                        Tools.WriteLog.AddLog(my.ID.ToString() + "焊锡没点");
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

                #region 正面
                case 2://到拍照位
                    if (my.cnt < ProcessData.wPointFs_SolderF[my.ID].Count)
                    {
                        if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                        {
                            pS[my.ID] = ProcessData.wPointFs_SolderF[my.ID][my.cnt];
                            float safe_Z = 0;
                            if (my.ID == 0)
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
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡正面");
                        my.cnt = 0;
                        my.step = 4;
                    }
                    break;

                case 3://到位,拍照
                    if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                    {
                        if (TriggerSolder(my.ID, pS[my.ID].templateIndex))
                        {
                            int count = ProcessData.SolderList[my.ID].Count;
                            foreach (VisionResult result in VisionAPI.Solders(my.ID))
                            {
                                int type = result.Type;
                                foreach (SolderPosdata p in LogicData.vData.soliderdata(my.ID)[type].pos)
                                {
                                    SolderPosdata _pos = new SolderPosdata();
                                    _pos.pos.X = result.X + p.pos.X + pS[my.ID].X;
                                    _pos.pos.Y = result.Y + p.pos.Y + pS[my.ID].Y;
                                    _pos.pos.R = p.pos.R;
                                    _pos.pos.Z = p.pos.Z;
                                    _pos.solderDef = p.solderDef.Clone();


                                    if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                    {
                                        _pos.rinse = true;
                                    }

                                    ProcessData.SolderList[my.ID].Add(_pos);

                                    float ang = result.R;
                                    ProcessData.SolderList_Ang[my.ID].Add(ang);
                                }
                            }

                            my.cnt++;
                            my.step = 2;
                        }
                        else
                        {
                            //results[my.ID] = DialogResult.None;
                            // new Thread(() =>
                            // {
                            //     movedriverZm.WriteRegister(new BaseData((ushort)(1604 + 2 * my.ID), new int[] { 1 }));
                            //     results[my.ID] = MessageBox.Show("平台" + my.ID.ToString() + "识别失败,是否重新拍照\n\r\n\r确定：重新拍照\n\r\n\r否：强制工作\n\r\n\r取消：跳过该点", "识别错误", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                            //     movedriverZm.WriteRegister(new BaseData((ushort)(1604 + 2 * my.ID), new int[] { 0 }));
                            // }).Start();


                            my.cnt++;
                            my.step = 2;
                            // my.step = 20;
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

                        int count = ProcessData.SolderList[my.ID].Count;
                        foreach (VisionResult result in VisionAPI.Solders(my.ID))
                        {
                            int type = result.Type;

                            foreach (SolderPosdata p in LogicData.vData.soliderdata(my.ID)[type].pos)
                            {
                                SolderPosdata _pos = new SolderPosdata();
                                _pos.pos.X = result.X + p.pos.X + pS[my.ID].X;
                                _pos.pos.Y = result.Y + p.pos.Y + pS[my.ID].Y;
                                _pos.pos.R = p.pos.R;
                                _pos.pos.Z = p.pos.Z;
                                _pos.solderDef = p.solderDef.Clone();

                                if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                {
                                    _pos.rinse = true;
                                }

                                ProcessData.SolderList[my.ID].Add(_pos);
                                float ang = result.R;
                                ProcessData.SolderList_Ang[my.ID].Add(ang);
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

                        if ((cnt[my.ID] % LogicData.RunData.clearnum == 0) && LogicData.RunData.rinseMode == 3)//每几个点去清洗
                        {
                            p.rinse = true;
                        }

                        if (LogicData.RunData.rotate(my.ID))
                        {
                            float cAng = (float)(ProcessData.SolderList_Ang[my.ID][0]*Math.PI/180);
                            float x = 0;
                            float y = 0;
                            float r = (float)(ProcessData.SolderList_Ang[my.ID][0] + p.pos.R);

                            Transorm((UsingPlatformSelect)my.ID, p.pos.X, p.pos.Y, p.pos.R, cAng, out x, out y);
                            while (!LogicAPI.solderTin[my.ID].exe(0, x, y, p.pos.Z, r, 2, p.solderDef, p.rinse ? 1 : 0))
                            {
                                Thread.Sleep(1);
                            }
                        }
                        else
                        {
                            while (!LogicAPI.solderTin[my.ID].exe(0, p.pos.X, p.pos.Y, p.pos.Z, p.pos.R, 2, p.solderDef, p.rinse ? 1 : 0))
                            {
                                Thread.Sleep(1);
                            }
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
                        ProcessData.SolderList_Ang[my.ID].RemoveAt(0);
                        my.step = 4;
                    }
                    break;
                #endregion

                #region 反面
                case 6://到拍照位
                    if (my.cnt < ProcessData.wPointFs_SolderV[my.ID].Count)
                    {
                        if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                        {
                            pS[my.ID] = ProcessData.wPointFs_SolderV[my.ID][my.cnt];
                            float safe_Z = 0;
                            if (my.ID == 0)
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
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡反面");
                        my.cnt = 0;
                        my.step = 8;
                    }
                    break;

                case 7://到位,拍照
                    if (LogicAPI.PlatformMove[my.ID].sta() && LogicAPI.PlatformMove[my.ID].start != 1)
                    {

                        if (TriggerSolder(my.ID, pS[my.ID].templateIndex))
                        {

                            int count = ProcessData.SolderList[my.ID].Count;
                            foreach (VisionResult result in VisionAPI.Solders(my.ID))
                            {
                                int type = result.Type;

                                foreach (SolderPosdata p in LogicData.vData.soliderdata(my.ID)[type].pos)
                                {
                                    SolderPosdata _pos = new SolderPosdata();
                                    _pos.pos.X = result.X + p.pos.X + pS[my.ID].X;
                                    _pos.pos.Y = result.Y + p.pos.Y + pS[my.ID].Y;
                                    _pos.pos.R = p.pos.R;
                                    _pos.pos.Z = p.pos.Z;
                                    _pos.solderDef = p.solderDef.Clone();

                                    if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                    {
                                        _pos.rinse = true;
                                    }

                                    ProcessData.SolderList[my.ID].Add(_pos);

                                    float ang = result.R;
                                    ProcessData.SolderList_Ang[my.ID].Add(ang);
                                }
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
                                SolderPosdata _pos = new SolderPosdata();
                                _pos.pos.X = result.X + p.pos.X + pS[my.ID].X;
                                _pos.pos.Y = result.Y + p.pos.Y + pS[my.ID].Y;
                                _pos.pos.R = p.pos.R;
                                _pos.pos.Z = p.pos.Z;
                                _pos.solderDef = p.solderDef.Clone();

                                if (LogicData.RunData.rinseMode == 1)//每几个清洗
                                {
                                    _pos.rinse = true;
                                }

                                ProcessData.SolderList[my.ID].Add(_pos);

                                float ang = result.R;
                                ProcessData.SolderList_Ang[my.ID].Add(ang);
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

                        if (LogicData.RunData.rotate(my.ID))
                        {
                            float cAng = (float)(ProcessData.SolderList_Ang[my.ID][0] * Math.PI / 180);
                            float x = 0;
                            float y = 0;
                            float r = (float)(ProcessData.SolderList_Ang[my.ID][0] + p.pos.R);


                            Transorm((UsingPlatformSelect)my.ID, p.pos.X, p.pos.Y, p.pos.R, cAng, out x, out y);
                            while (!LogicAPI.solderTin[my.ID].exe(0, x, y, p.pos.Z, r, 2, p.solderDef, p.rinse ? 1 : 0))
                            {
                                Thread.Sleep(1);
                            }
                        }
                        else
                        {
                            while (!LogicAPI.solderTin[my.ID].exe(0, p.pos.X, p.pos.Y, p.pos.Z, p.pos.R, 2, p.solderDef, p.rinse ? 1 : 0))
                            {
                                Thread.Sleep(1);
                            }
                        }


                        cnt[my.ID]++;
                        my.step = 9;
                    }
                    else
                    {
                        Tools.WriteLog.AddLog(my.ID.ToString() + "开始焊锡正面拍照");
                        ProcessData.SolderList[my.ID].Clear();
                        ProcessData.SolderList_Ang[my.ID].Clear();
                        my.step = 2;
                        my.cnt = 0;
                    }
                    break;

                case 9:
                    if (LogicAPI.solderTin[my.ID].sta() && LogicAPI.solderTin[my.ID].start != 1)
                    {
                        Thread.Sleep(100);
                        ProcessData.SolderList[my.ID].RemoveAt(0);//删除该点
                        ProcessData.SolderList_Ang[my.ID].RemoveAt(0);
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

        public void Transorm(UsingPlatformSelect usingPlatform, float X, float Y, float R, float Ang, out float Tx, out float Ty)
        {
            TeachingMechinePra mechinePra = new TeachingMechinePra();

            if (usingPlatform == UsingPlatformSelect.Left)
            {
                mechinePra = LogicData.RunData.TeachingMechinePra_Left;
            }
            else //if (usingPlatform == UsingPlatformSelect.Right)
            {
                mechinePra = LogicData.RunData.TeachingMechinePra_Right;
            }

            // 认为是将焊头的点转成0角度的点的位置
            float rotateAng = (float)(R + mechinePra.RotatePostionStartAngle);
            PointF rotateCur = new PointF();
            rotateCur.X = X;
            rotateCur.Y = Y;//装换前的角度和位置

            double radius = mechinePra.Radius;//圆心半径

            //
            PointF rotateC = new PointF();//旋转中心
            float ang = rotateAng;// + (float)mechinePra.RotatePostionStartAngle;//旋转的角度

            double cos = Math.Cos(ang* Math.PI / 180/* */);//对应弧度
            double sin = Math.Sin(ang * Math.PI / 180/**/);

            rotateC.X = rotateCur.X + (float)(radius * cos);
            rotateC.Y = rotateCur.Y + (float)(radius * sin);//计算旋转中心
            //

            Circle circle = new Circle(rotateC, (float)radius);

            PointF pos = circle.Rotate(rotateCur, Ang);

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
