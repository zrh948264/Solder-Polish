using Dispenser;
using HalconDotNet;
using ProArmband.Device;
using ProArmband.Manager;
using ProCommon.Communal;
using ProDriver.APIHandle;
using ProDriver.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision;
using Vision.Tool;
using Vision.Tool.Calibrate;
using ProVision.InteractiveROI;
using Vision.Tool.Model;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Drawing;
using Common;
using UI;
using Vision.Logic;
using Motion;
using System.IO;

namespace HZZH.Vision.Logic
{
    class VisionProject
    {
        #region 静态单例

        private static object _syncObj = new object();
        private static VisionProject _instance;

        private VisionProject()
        {
            //InitVisionProject();
        }

        public static VisionProject Instance
        {
            get
            {
                lock (_syncObj)
                {
                    if (_instance == null)
                    {
                        _instance = new VisionProject();
                    }
                }

                return _instance;
            }
        }

        #endregion 静态单例



        /// <summary>
        /// 与控制中的逻辑交互数据
        /// </summary>
        public VisionAPIDef visionAPIDef { get; set; }

        /// <summary>
        /// 链接的相机的管理列表
        /// </summary>
        public Device_Camera DevCam;

        /// <summary>
        /// 相机的标定数据
        /// </summary>
        public CalibPointToPoint[] Calib = new CalibPointToPoint[4];

        public VisionTool VisionTools = new VisionTool();


        /// <summary>
        /// 使用4个显示窗口
        /// </summary>
        private HWindowControl[] windowctrl = new HWindowControl[10];


        /// <summary>
        /// 设置显示的窗口
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hWindow"></param>
        public void SetDisplayWindow(int index, HWindowControl hWindow)
        {
            windowctrl[index] = hWindow;

            if (hWndCtrller[index] != null)
            {
                windowctrl[index].SizeChanged -= (s, ev) => { hWndCtrller[0].Repaint(); };
                ((HWndCtrllerEx)hWndCtrller[index]).Dispose();
            }
            hWndCtrller[index] = new HWndCtrllerEx(windowctrl[index]) { UseThreadEnable = true };
            windowctrl[index].SizeChanged += (s, ev) => { hWndCtrller[0].Repaint(); };
            HOperatorSet.SetFont(windowctrl[index].HalconWindow, "-Arial-40-*-1-*-*-1-ANSI_CHARSET-");
        }


        public void InitVisionProject()
        {
            HOperatorSet.SetSystem("width", 2592);
            HOperatorSet.SetSystem("height", 1964);
            HOperatorSet.SetSystem("border_shape_models", "true");

            InitCameraDevice();
            InitCalibration();


            //visionLogic = new Thread(VisionLogicFun);
            //visionLogic.IsBackground = true;
            //visionLogic.Start();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Log("视觉初始化完成");
        }

        public void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SaveCalib();

            // 关闭相机
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                for (int i = 0; i < DevCam.CamHandleList.Values.Count; i++)
                {
                    float exposuretime, gain;
                    DevCam.CamHandleList.Values[i].GetExposureTime(out exposuretime);
                    DevCam.CamHandleList.Values[i].GetGain(out gain);
                    DevCam.CamHandleList.Values[i].SetExposureTime(exposuretime);
                    DevCam.CamHandleList.Values[i].SetGain(gain);
                }

                ConfigManager.Instance.Save();

                for (int i = 0; i < DevCam.CamHandleList.Values.Count; i++)
                {
                    DevCam.CamHandleList.Values[i].Close();
                }
            }
            Log("关闭相机");

        }

        private void Log(string log)
        {
#if DEBUG
            Tools.WriteLog.AddLog(string.Format("[{0}]  \t{1}", this.GetType().Name, log));
#endif
        }


        //#region  主界面操作相关

        //FormMain formMain = null;
        //public void BindActionEvent(FormMain _formMain)
        //{
        //    this.formMain = _formMain;

        //    SetDisplayWindow(0, formMain.hWindowControl0);
        //    formMain.创建模板A0.Click += 创建模板A0_Click;
        //    formMain.创建模板A1.Click += 创建模板A0_Click;
        //    formMain.标定.Click += 创建模板A0_Click;
        //    formMain.移动.Click += 创建模板A0_Click;
        //    formMain.缩放.Click += 创建模板A0_Click;
        //    formMain.恢复.Click += 创建模板A0_Click;

        //    formMain.触发相机.Click += 创建模板A0_Click;
        //    formMain.相机实时.Click += 创建模板A0_Click;
        //    formMain.相机设置.Click += 创建模板A0_Click;
        //    formMain.toolStripTextBox1.TextChanged += ToolStripTextBox1_TextChanged;
        //    formMain.toolStripComboBox1.SelectedIndexChanged += ToolStripComboBox1_SelectedIndexChanged;

        //    Functions.SetBinding(formMain.numericUpDown8, "Value", VisionTools, "LoadBoard");
        //    Functions.SetBinding(formMain.numericUpDown5, "Value", VisionTools, "LocateBoard");

        //    formMain.numericUpDown1.Validating += numericUpDown1_ValueChanged;
        //    //formMain.numericUpDown8.Validating += (s, e) => { VisionTools.LoadBoard = (float)formMain.numericUpDown8.Value; };
        //    //formMain.numericUpDown5.Validating += (s, e) =>
        //    //{ 
        //    //    VisionTools.LocateBoard = (float)formMain.numericUpDown5.Value;
        //    //};
        //    formMain.FormClosed += FormMain_FormClosed;

        //}

        //void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //{
        //    VisionTools.shapes[0].shapeParam.mMinScore = (float)formMain.numericUpDown1.Value;
        //}

        //int cameraImageAnalysis = 0;
        //private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cameraImageAnalysis = formMain.toolStripComboBox1.SelectedIndex;
        //}

        //private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    SaveCalib();
        //    ConfigManager.Instance.Save();

        //    grabTimer.Enabled = false;
        //    grabTimer.Dispose();
        //    try
        //    {
        //        if (DevCam != null && DevCam.CamHandleList != null)
        //        {
        //            for (int i = 0; i < DevCam.CamHandleList.Values.Count(); i++)
        //            {
        //                DevCam.CamHandleList.Values[i].StopGrab();
        //                DevCam.CamHandleList.Values[i].Close();
        //            }
        //        }
        //    }
        //    catch { }
        //}



        //private void ToolStripTextBox1_TextChanged(object sender, EventArgs e)
        //{
        //    int val = 0;
        //    if (int.TryParse(formMain.toolStripTextBox1.Text, out val))
        //    {
        //        SetCameraExposureTime(0, val);
        //    }
        //}




        //private void 创建模板A0_Click(object sender, EventArgs e)
        //{
        //    switch (((ToolStripItem)sender).Name)
        //    {
        //        case "创建模板A0":
        //            SetTempleteModel(VisionTools.shapes[0]);
        //            break;
        //        case "创建模板A1":
        //            SetTempleteModel(VisionTools.shapes[1]);
        //            break;
        //        case "标定":
        //            SetCalib1();
        //            break;
        //        case "移动":
        //            SetViewModeMove();
        //            break;
        //        case "缩放":
        //            SetViewModeZoom();
        //            break;
        //        case "恢复":
        //            SetViewModeNone();
        //            break;
        //        case "触发相机":
        //            CamState[0] = false;
        //            CameraSoft(0);
        //            break;
        //        case "相机实时":
        //            CamState[0] = true;
        //            break;
        //        case "相机设置":
        //            ShowCameraSetPage(0);
        //            break;
        //    }
        //}

        //private void RefreshDispaly()
        //{
        //    if (formMain == null)
        //    {
        //        return;
        //    }
        //    formMain.toolStripTextBox1.Text = GetCameraExposureTime(0).ToString();
        //    formMain.numericUpDown1.Value = (decimal)VisionTools.shapes[0].shapeParam.mMinScore;
        //    //formMain.numericUpDown8.Value = (decimal)VisionTools.LoadBoard;
        //    //formMain.numericUpDown5.Value = (decimal)VisionTools.LocateBoard;

        //    Functions.SetBinding(formMain.numericUpDown8, "Value", VisionTools, "LoadBoard");
        //    Functions.SetBinding(formMain.numericUpDown5, "Value", VisionTools, "LocateBoard");
        //    //formMain.toolStripLabel2.Text = Math.Min(VisionTools.shapes[0].shapeParam.mMinScore, VisionTools.shapes[1].shapeParam.mMinScore).ToString("f2");
        //}

        //#endregion


        #region   相机

        private ManualResetEvent[] CompleteSoft = new ManualResetEvent[3];
        public bool[] CamState = new bool[3];
        System.Timers.Timer grabTimer = new System.Timers.Timer();
        public Size[] ImageSize = new Size[3];
        HImage[] hHImage = new HImage[3];

        /// <summary>
        /// 通过相机采集图片的回调事件，将图像从相机类中拷贝到当前，用于表示当前图片
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="hoImage"></param>
        private void VisionProject_ImageGrabbedEvt(Camera cam, HObject hoImage)
        {
            lock (this)
            {
                if (hHImage[cam.Number] != null)
                {
                    hHImage[cam.Number].Dispose();
                }
                hHImage[cam.Number] = PercondHImage(hoImage);
                HTuple width, height;
                HOperatorSet.GetImageSize(hHImage[cam.Number], out width, out height);
                ImageSize[cam.Number].Width = width[0].I;
                ImageSize[cam.Number].Height = height[0].I;
            }

            CompleteSoft[cam.Number].Set();
        }

        /// <summary>
        /// 相机是否成功链接
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool CameraConnected(int index)
        {
            bool connected = false;
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                if (index < DevCam.CamHandleList.Count)
                {
                    connected = ((CamDriver)DevCam.CamHandleList.Values[index].ICamDriverable).Camera.IsConnected;
                }
            }

            return connected;
        }


        /// <summary>
        /// 软触发拍图
        /// </summary>
        /// <param name="index"></param>
        public void CameraSoft(int index)
        {
            if (DevCam != null && DevCam.CamHandleList != null && index < DevCam.CamHandleList.Count)
            {
                CompleteSoft[index].Reset();
                DevCam.CamHandleList.Values[index].SoftTriggerOnce();
            }
        }

        /// <summary>
        /// 等待获取图片
        /// </summary>
        /// <param name="index"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool WaiteGetImage(int index, int time = -1)
        {
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                return CompleteSoft[index].WaitOne(time);
            }
            return false;
        }


        /// <summary>
        /// 获取当前的操作图片，注意是拷贝出来的
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HImage GetCurrentImage(int index)
        {
            HImage img = null;
            lock (this)
            {
                img = hHImage[index].Clone();
            }

            return img;
        }

        /// <summary>
        /// 从文件中读取图片到当前操作的图片中
        /// </summary>
        /// <param name="index"></param>
        public void OpenDialogReadImg(int index)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofDialog = new System.Windows.Forms.OpenFileDialog();
                ofDialog.InitialDirectory = Application.StartupPath;
                ofDialog.Filter = "位图文件(*.bmp)|*.bmp|PNG(*.png)|*.png|JPGE(*.jpge,*.jpg)|*.jpeg;*.jpg|所有图片|*.bmp;*.png;*.jpge;*.jpg";
                ofDialog.FilterIndex = -1;
                ofDialog.Multiselect = false;
                ofDialog.Title = "打开一张图片";

                string fPath, fName;
                if (ofDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fPath = ofDialog.FileName;
                    fName = ofDialog.SafeFileName;

                    lock (this)
                    {
                        if (hHImage[index] != null)
                        {
                            hHImage[index].Dispose();
                        }

                        hHImage[index].ReadImage(fPath);
                    }

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("读取图片失败\r" + ex.Message);
            }
        }


        /// <summary>
        /// 设置相机的曝光时间
        /// </summary>
        /// <param name="index"></param>
        /// <param name="exposureTime"></param>
        public void SetCameraExposureTime(int index, float exposureTime)
        {
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                DevCam.CamHandleList.Values[index].SetExposureTime(exposureTime);
                ((CamDriver)DevCam.CamHandleList.Values[index].ICamDriverable).Camera.ExposureTime = exposureTime;
            }

        }

        /// <summary>
        /// 设置相机的增益时间
        /// </summary>
        /// <param name="index"></param>
        /// <param name="gain"></param>
        public void SetCameraGain(int index, float gain)
        {
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                DevCam.CamHandleList.Values[index].SetGain(gain);
                ((CamDriver)DevCam.CamHandleList.Values[index].ICamDriverable).Camera.Gain = gain;
            }
        }


        /// <summary>
        /// 获取相机的曝光时间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetCameraExposureTime(int index)
        {
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                float exposuretime = 0;
                DevCam.CamHandleList.Values[index].GetExposureTime(out exposuretime);
                return exposuretime;
                //return ((CamDriver)DevCam.CamHandleList.Values[index].ICamDriverable).Camera.ExposureTime;
            }

            return 0;
        }

        /// <summary>
        /// 显示相机的设置页面
        /// </summary>
        /// <param name="index"></param>
        public void ShowCameraSetPage(int index)
        {
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                DevCam.CamHandleList.Values[index].CreateCameraSetPage(IntPtr.Zero, "相机" + index);
                DevCam.CamHandleList.Values[index].ShowCameraSetPage();
            }
        }

        /// <summary>
        /// 一个定时器用于模拟相机的连续触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinueTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < CamState.Length; i++)
            {
                if (CamState[i] == true)
                {
                    CameraSoft(i);
                }
            }
        }

        /// <summary>
        /// 初始化相机的驱动
        /// </summary>
        private void InitCameraDevice()
        {
            // 初始化参数
            ConfigManager.Instance.Load();
            DeviceManager.Instance.DeviceListInit();
            DeviceManager.Instance.DeviceListStart();
            Log("相机配置文件读取成功");
            // 配置相机
            DevCam = (Device_Camera)DeviceManager.Instance.DeviceList[CtrllerCategory.Camera];
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                CamState = new bool[DevCam.CamHandleList.Count];
                CompleteSoft = new ManualResetEvent[DevCam.CamHandleList.Count];
                for (int i = 0; i < CompleteSoft.Length; i++)
                {
                    CompleteSoft[i] = new ManualResetEvent(false);
                    CamState[i] = true;
                }

                for (int i = 0; i < DevCam.CamHandleList.Values.Count; i++)
                {
                    DevCam.CamHandleList.Values[i].ImageGrabbedEvt += VisionProject_ImageGrabbedEvt;
                    DevCam.CamHandleList.Values[i].ImageGrabbedEvt += Camera_ImageGrabbedEvt;

                    DevCam.CamHandleList.Values[i].SetAcquisitionMode(AcquisitionMode.SoftTrigger, 1);
                }
            }

            for (int i = 0; i < hHImage.Length; i++)
            {
                hHImage[i] = new HImage("byte", 512, 512);
            }


            grabTimer.AutoReset = true;
            grabTimer.Interval = 100;
            grabTimer.Elapsed += ContinueTimerElapsed;
            grabTimer.Enabled = true;
            Log("相机初始化正常");
        }

        #endregion




        #region 标定
        public CalibPPSetting[] calibrateSetting = null;

        /// <summary>
        /// 标定数据与功能的初始化
        /// </summary>
        private void InitCalibration()
        {
            calibrateSetting = new CalibPPSetting[Calib.Length];
            for (int i = 0; i < Calib.Length; i++)
            {
                Calib[i] = global::Vision.Tool.Serialization.LoadFromXml(typeof(CalibPointToPoint), @"AppRequired\Calib" + i + ".Xml") as CalibPointToPoint;
                if (Calib[i] == null)
                {
                    Calib[i] = new CalibPointToPoint();
                    Log("初始化时候标定" + i + "无数据，重新生成默认值");
                }
                calibrateSetting[i] = new CalibPPSetting(Calib[i]);
            }
            calibrateSetting[0].GetImage = CaptureHImage0;
            calibrateSetting[1].GetImage = CaptureHImage1;
            calibrateSetting[2].GetImage = CaptureHImage2;
            calibrateSetting[3].GetImage = CaptureHImage2;

            Log("标定初始化完成");

        }

        private HImage CaptureHImage0()
        {
            CameraSoft(0);
            HImage img = null;
            if (WaiteGetImage(0, 3000) == true)
            {
                img = GetCurrentImage(0);
            }
            if (img == null || img.IsInitialized() == false)
            {
                img = new HImage("byte", 800, 600);
                Log("相机0采集图片失败，生成默认图片");
            }
            return img;
        }
        private HImage CaptureHImage1()
        {
            CameraSoft(1);
            HImage img = null;
            if (WaiteGetImage(1, 3000) == true)
            {
                img = GetCurrentImage(1);
            }
            if (img == null || img.IsInitialized() == false)
            {
                img = new HImage("byte", 800, 600);
                Log("相机1采集图片失败，生成默认图片");
            }
            return img;
        }
        private HImage CaptureHImage2()
        {
            CameraSoft(2);
            HImage img = null;
            if (WaiteGetImage(2, 3000) == true)
            {
                img = GetCurrentImage(2);
            }
            if (img == null || img.IsInitialized() == false)
            {
                img = new HImage("byte", 800, 600);
                Log("相机2采集图片失败，生成默认图片");
            }
            return img;
        }

        /// <summary>
        /// 保存标定数据
        /// </summary>
        public void SaveCalib()
        {
            for (int i = 0; i < Calib.Length; i++)
            {
                if (global::Vision.Tool.Serialization.CanSerializaXml(Calib[i]))
                {
                    global::Vision.Tool.Serialization.SaveToXml(Calib[i], @"AppRequired\Calib" + i + ".Xml", true);
                }
                Log("标定数据" + i + "保存完成");
            }
        }

        /// <summary>
        /// 显示标定设置
        /// </summary>
        /// <param name="index"></param>
        public void ShowCalibSet(int index)
        {
            calibrateSetting[index].ShowSetting(null);
            Log("标定数据" + index + "打开设置");
        }

        #endregion



        #region  用于显示Halcon数据窗口
        private HWndCtrller[] hWndCtrller = new HWndCtrller[10];

        private void DisplayHobject(HWndCtrller hWndCtrller, HImage hObject)
        {
            try
            {
                hWndCtrller.AddIconicVar(hObject.Clone());
                hWndCtrller.Repaint();
            }
            catch { }
        }

        /// <summary>
        /// 显示形状匹配的模板于图像窗口
        /// </summary>
        /// <param name="hWndCtrller"></param>
        /// <param name="shape"></param>
        private void DisplayModelResult(HWndCtrller hWndCtrller, ShapeModel shape)
        {
            try
            {
                //HImage img = shape.InputImg.Clone();
                //hWndCtrller.AddIconicVar(img);
                hWndCtrller.ChangeGraphicSettings(GraphicContext.GC_COLOR, "green");

                //int imgWidth, imgHeight;
                //img.GetImageSize(out imgWidth, out imgHeight);
                //HXLDCont m_cross = new HXLDCont();
                //m_cross.GenCrossContourXld(imgHeight / 2.0, imgWidth / 2.0, 2999, 0);
                //hWndCtrller.AddIconicVar(m_cross);

                hWndCtrller.AddIconicVar(shape.GetMatchModelCont());
                for (int i = 0; i < shape.OutputResult.Count; i++)
                {
                    string msg = string.Format("Row:{0:0.0}\nCol:{1:0.0}\nAng{2:0.00}\nScor:{3:0.00}",
                          shape.OutputResult.Row[i].D, shape.OutputResult.Col[i].D, shape.OutputResult.Angle.TupleDeg()[i].D, shape.OutputResult.Score[i].D);

                    HObjectString objectString = new HObjectString(msg)
                    {
                        Row = shape.OutputResult.Row[i].F,
                        Col = shape.OutputResult.Col[i].F,
                        Color = "blue",
                        Window = "image"
                    };

                    hWndCtrller.AddIconicVar(objectString);
                }
                if (shape.OutputResult.Count > 0)
                {
                    HObject cross;
                    HOperatorSet.GenCrossContourXld(out cross, shape.OutputResult.Row, shape.OutputResult.Col, 16, 0);
                    hWndCtrller.AddIconicVar(cross);
                }

                if (shape.OutputResult.Count == 0)
                {
                    string msg = "NG";
                    HObjectString objectString = new HObjectString(msg)
                    {
                        Row = 60,
                        Col = 20,
                        Color = "red",
                        Window = "image"
                    };

                    hWndCtrller.AddIconicVar(objectString);
                }



                hWndCtrller.Repaint();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("刷新显示错误:" + ex.Message);
            }
        }

        /// <summary>
        /// 显示字符串于窗口
        /// </summary>
        /// <param name="hWndCtrller"></param>
        /// <param name="msg"></param>
        private void DisplayStringInHWindow(HWndCtrller hWndCtrller, string msg)
        {
            HObjectString objectString = new HObjectString(msg)
            {
                Row = 0,
                Col = 0,
                Color = "blue",
                Window = "window"
            };
            hWndCtrller.AddIconicVar(objectString);
            hWndCtrller.Repaint();
        }


        private void DisplayModelResult(HWndCtrller hWndCtrller, NCCModel nCC)
        {
            try
            {
                //HImage img = shape.InputImg.Clone();
                //hWndCtrller.AddIconicVar(img);
                hWndCtrller.ChangeGraphicSettings(GraphicContext.GC_COLOR, "green");
                hWndCtrller.ChangeGraphicSettings(GraphicContext.GC_DRAWMODE, "margin");

                //int imgWidth, imgHeight;
                //img.GetImageSize(out imgWidth, out imgHeight);
                //HXLDCont m_cross = new HXLDCont();
                //m_cross.GenCrossContourXld(imgHeight / 2.0, imgWidth / 2.0, 2999, 0);
                //hWndCtrller.AddIconicVar(m_cross);

                for (int i = 0; i < nCC.OutputResult.Count; i++)
                {
                    HHomMat2D hHomMat2D = new HHomMat2D();
                    hHomMat2D.VectorAngleToRigid(0, 0, 0, nCC.OutputResult.Row[i].D, nCC.OutputResult.Col[i].D, nCC.OutputResult.Angle[i].D);
                    hHomMat2D = hHomMat2D.HomMat2dTranslateLocal(-nCC.ModelRegion.Row, -nCC.ModelRegion.Column);

                    HRegion region = nCC.ModelRegion.AffineTransRegion(hHomMat2D, "constant");
                    hWndCtrller.AddIconicVar(region);
                }

                for (int i = 0; i < nCC.OutputResult.Count; i++)
                {
                    string msg = string.Format("Row:{0:0.0}\nCol:{1:0.0}\nAng{2:0.00}\nScor:{3:0.00}",
                          nCC.OutputResult.Row[i].D, nCC.OutputResult.Col[i].D, nCC.OutputResult.Angle.TupleDeg()[i].D, nCC.OutputResult.Score[i].D);

                    HObjectString objectString = new HObjectString(msg)
                    {
                        Row = nCC.OutputResult.Row[i].F,
                        Col = nCC.OutputResult.Col[i].F,
                        Color = "blue",
                        Window = "image"
                    };

                    hWndCtrller.AddIconicVar(objectString);
                }
                if (nCC.OutputResult.Count > 0)
                {
                    HObject cross;
                    HOperatorSet.GenCrossContourXld(out cross, nCC.OutputResult.Row, nCC.OutputResult.Col, 16, 0);
                    hWndCtrller.AddIconicVar(cross);
                }

                if (nCC.OutputResult.Count == 0)
                {
                    string msg = "NG";
                    HObjectString objectString = new HObjectString(msg)
                    {
                        Row = 60,
                        Col = 20,
                        Color = "red",
                        Window = "image"
                    };

                    hWndCtrller.AddIconicVar(objectString);
                }

                hWndCtrller.Repaint();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("刷新显示错误:" + ex.Message);
            }
        }

        private void DisplayModelResult(HWndCtrller hWndCtrller, Model model)
        {
            if (model is ShapeModel)
            {
                DisplayModelResult(hWndCtrller, (ShapeModel)model);
            }

            if (model is NCCModel)
            {
                DisplayModelResult(hWndCtrller, (NCCModel)model);
            }
        }

        /// <summary>
        /// 设置窗口为移动显示模式
        /// </summary>
        /// <param name="index"></param>
        public void SetViewModeMove(int index)
        {
            hWndCtrller[index].SetViewMode(HWndCtrller.MODE_VIEW_MOVE);
        }

        /// <summary>
        /// 设置窗口为正常模式
        /// </summary>
        /// <param name="index"></param>
        public void SetViewModeNone(int index)
        {
            hWndCtrller[index].SetViewMode(HWndCtrller.MODE_VIEW_NONE);
            hWndCtrller[index].ResetWindow();
            hWndCtrller[index].Repaint();
        }

        /// <summary>
        /// 设置窗口为缩放模式
        /// </summary>
        /// <param name="index"></param>
        public void SetViewModeZoom(int index)
        {
            hWndCtrller[index].SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);

        }

        /// <summary>
        /// 设置halcon窗口的字体的大小
        /// </summary>
        /// <param name="hWindowIndex"></param>
        /// <param name="size"></param>
        private void SetWindowFontSize(int hWindowIndex, int size)
        {
            try
            {
                if (windowctrl[hWindowIndex] != null)
                {
                    string font = string.Format("-Arial-{0}-*-1-*-*-1-ANSI_CHARSET-", size);
                    HOperatorSet.SetFont(windowctrl[hWindowIndex].HalconWindow, font);

                }
            }
            catch { }
        }
        #endregion



        #region   运行时候的控制逻辑与方法

        private void Camera_ImageGrabbedEvt(Camera cam, HObject hoImage)
        {
            HObject _hoImage = GetCurrentImage(cam.Number);
            if (_hoImage != null && _hoImage.IsInitialized() && hWndCtrller[cam.Number] != null)
            {
                HTuple imgWidth, imgHeight;
                HOperatorSet.GetImageSize(_hoImage, out imgWidth, out imgHeight);
                hWndCtrller[cam.Number].AddIconicVar(_hoImage);
                hWndCtrller[cam.Number].ChangeGraphicSettings(GraphicContext.GC_COLOR, "green");
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(imgHeight / 2.0, imgWidth / 2.0, 2999, 0);
                hWndCtrller[cam.Number].AddIconicVar(cross);
                hWndCtrller[cam.Number].Repaint();
            }
        }

        /// <summary>
        /// 做焊锡模板定位
        /// </summary>
        public void LocateSolderLeftShape()
        {
            for (int i = 0; i < VisionTools.SolderLeft.Count; i++)
            {
                LocateSolderLeftShape(i);
            }
        }
        public void LocateSolderLeftShape(int index)
        {
            Model shape = VisionTools.SolderLeft[index].Shape;

            // 模板匹配
            HImage image = GetCurrentImage(0);
            if (shape.InputImg != null)
            {
                shape.InputImg.Dispose();
            }

            shape.InputImg = image;
            shape.FindModel();

            // 显示
            ShapeMatchResult match = shape.OutputResult;
            DisplayModelResult(hWndCtrller[0], shape);
            SetWindowFontSize(3, match.Count > 0 ? 40 : 120);
            DisplayHobject(hWndCtrller[3], shape.InputImg);
            DisplayModelResult(hWndCtrller[3], shape);

            // 结果转换
            VisionResult result = new VisionResult();
            for (int n = 0; n < match.Count; n++)
            {
                int imgWidth, imgHeight;
                image.GetImageSize(out imgWidth, out imgHeight);

                PointF worldPoint;
                Calib[0].PixelPointToWorldPoint(new PointF(match.Col[n].F, match.Row[n].F),
                    out worldPoint,
                    new PointF(imgWidth / 2f, imgHeight / 2f),
                    new PointF());

                result.X = worldPoint.X;
                result.Y = worldPoint.Y;
                result.R = match.Angle.TupleDeg()[n].F;
                result.Type = index;

                if (visionAPIDef != null)
                {
                    visionAPIDef.SolderLeft.Add(result);
                }
            }

            switch (ImagePathSolderLeftState)
            {
                case 0:
                    break;
                case 1:
                    if (visionAPIDef.SolderLeft.Count == 0)
                    {
                        SaveImage(GetCurrentImage(0), ImagePathSolderLeft + "NG\\" + GenerateImageName());
                    }
                    break;
                case 2:
                    if (visionAPIDef.SolderLeft.Count > 0)
                    {
                        SaveImage(GetCurrentImage(0), ImagePathSolderLeft + "OK\\" + GenerateImageName());
                    }
                    break;
                case 3:
                    SaveImage(GetCurrentImage(0), ImagePathSolderLeft + GenerateImageName());
                    break;
            }

            Log(string.Format("左{0}焊锡模板定位", index));
        }



        /// <summary>
        /// 右焊锡模板定位
        /// </summary>
        public void LocateSolderRightShape()
        {
            for (int i = 0; i < VisionTools.SolderRight.Count; i++)
            {
                LocateSolderRightShape(i);
            }
        }
        public void LocateSolderRightShape(int index)
        {
            Model shape = VisionTools.SolderRight[index].Shape;

            // 模板匹配
            HImage image = GetCurrentImage(1);
            if (shape.InputImg != null)
            {
                shape.InputImg.Dispose();
            }

            shape.InputImg = image;
            shape.FindModel();

            // 显示
            ShapeMatchResult match = shape.OutputResult;
            DisplayModelResult(hWndCtrller[1], shape);
            SetWindowFontSize(5, match.Count > 0 ? 40 : 120);
            DisplayHobject(hWndCtrller[5], shape.InputImg);
            DisplayModelResult(hWndCtrller[5], shape);

            // 结果转换
            VisionResult result = new VisionResult();
            for (int n = 0; n < match.Count; n++)
            {
                int imgWidth, imgHeight;
                image.GetImageSize(out imgWidth, out imgHeight);

                PointF worldPoint;
                Calib[1].PixelPointToWorldPoint(new PointF(match.Col[n].F, match.Row[n].F),
                    out worldPoint,
                    new PointF(imgWidth / 2f, imgHeight / 2f),
                    new PointF());

                result.X = worldPoint.X;
                result.Y = worldPoint.Y;
                result.R = match.Angle.TupleDeg()[n].F;
                result.Type = index;
                if (visionAPIDef != null)
                {
                    visionAPIDef.SolderRight.Add(result);
                }
            }

            switch (ImagePathSolderRightState)
            {
                case 0:
                    break;
                case 1:
                    if (visionAPIDef.SolderLeft.Count == 0)
                    {
                        SaveImage(GetCurrentImage(1), ImagePathSolderRight + "NG\\" + GenerateImageName());
                    }
                    break;
                case 2:
                    if (visionAPIDef.SolderLeft.Count > 0)
                    {
                        SaveImage(GetCurrentImage(1), ImagePathSolderRight + "OK\\" + GenerateImageName());
                    }
                    break;
                case 3:
                    SaveImage(GetCurrentImage(1), ImagePathSolderRight + GenerateImageName());
                    break;
            }

            Log(string.Format("右{0}焊锡模板定位", index));
        }




        /// <summary>
        /// 左打磨模板定位
        /// </summary>
        public void LocatePolishLeftShape()
        {
            for (int i = 0; i < VisionTools.PolishLeft.Count; i++)
            {
                LocatePolishLeftShape(i);
            }
        }
        public void LocatePolishLeftShape(int index)
        {
            Model shape = VisionTools.PolishLeft[index].Shape;

            // 模板匹配
            HImage image = GetCurrentImage(2);
            if (shape.InputImg != null)
            {
                shape.InputImg.Dispose();
            }
            shape.InputImg = image;
            shape.FindModel();

            // 显示
            ShapeMatchResult match = shape.OutputResult;
            DisplayModelResult(hWndCtrller[2], shape);
            SetWindowFontSize(4, match.Count > 0 ? 40 : 120);
            DisplayHobject(hWndCtrller[4], shape.InputImg);
            DisplayModelResult(hWndCtrller[4], shape);

            // 结果转换
            for (int n = 0; n < match.Count; n++)
            {
                int imgWidth, imgHeight;
                image.GetImageSize(out imgWidth, out imgHeight);

                PointF worldPoint;
                Calib[2].PixelPointToWorldPoint(new PointF(match.Col[n].F, match.Row[n].F),
                    out worldPoint,
                    new PointF(imgWidth / 2f, imgHeight / 2f),
                    new PointF());

                VisionResult result = new VisionResult();
                result.X = worldPoint.X;
                result.Y = worldPoint.Y;
                result.R = match.Angle.TupleDeg()[n].F;
                result.Type = index;

                if (visionAPIDef != null)
                {
                    visionAPIDef.PolishLeft.Add(result);
                }
            }


            switch (ImagePathPolishLeftState)
            {
                case 0:
                    break;
                case 1:
                    if (visionAPIDef.SolderLeft.Count == 0)
                    {
                        SaveImage(GetCurrentImage(2), ImagePathPolishLeft + "NG\\" + GenerateImageName());
                    }
                    break;
                case 2:
                    if (visionAPIDef.SolderLeft.Count > 0)
                    {
                        SaveImage(GetCurrentImage(2), ImagePathPolishLeft + "OK\\" + GenerateImageName());
                    }
                    break;
                case 3:
                    SaveImage(GetCurrentImage(2), ImagePathPolishLeft + GenerateImageName());
                    break;
            }

            Log(string.Format("左{0}打磨模板定位", index));
        }





        /// <summary>
        /// 右打磨模板定位
        /// </summary>
        public void LocatePolishRightShape()
        {
            for (int i = 0; i < VisionTools.PolishRight.Count; i++)
            {
                LocatePolishRightShape(i);
            }
        }
        public void LocatePolishRightShape(int index)
        {
            Model shape = VisionTools.PolishRight[index].Shape;

            // 模板匹配
            HImage image = GetCurrentImage(2);
            if (shape.InputImg != null)
            {
                shape.InputImg.Dispose();
            }
            shape.InputImg = image;
            shape.FindModel();

            // 显示
            ShapeMatchResult match = shape.OutputResult;
            DisplayModelResult(hWndCtrller[2], shape);
            SetWindowFontSize(6, match.Count > 0 ? 40 : 120);
            DisplayHobject(hWndCtrller[6], shape.InputImg);
            DisplayModelResult(hWndCtrller[6], shape);

            // 结果转换
            for (int n = 0; n < match.Count; n++)
            {
                int imgWidth, imgHeight;
                image.GetImageSize(out imgWidth, out imgHeight);

                PointF worldPoint;
                Calib[3].PixelPointToWorldPoint(new PointF(match.Col[n].F, match.Row[n].F),
                    out worldPoint,
                    new PointF(imgWidth / 2f, imgHeight / 2f),
                    new PointF());

                VisionResult result = new VisionResult();
                result.X = worldPoint.X;
                result.Y = worldPoint.Y;
                result.R = match.Angle.TupleDeg()[n].F;
                result.Type = index;

                if (visionAPIDef != null)
                {
                    visionAPIDef.PolishRight.Add(result);
                }
            }


            switch (ImagePathPolishRightState)
            {
                case 0:
                    break;
                case 1:
                    if (visionAPIDef.SolderLeft.Count == 0)
                    {
                        SaveImage(GetCurrentImage(2), ImagePathPolishRight + "NG\\" + GenerateImageName());
                    }
                    break;
                case 2:
                    if (visionAPIDef.SolderLeft.Count > 0)
                    {
                        SaveImage(GetCurrentImage(2), ImagePathPolishRight + "OK\\" + GenerateImageName());
                    }
                    break;
                case 3:
                    SaveImage(GetCurrentImage(2), ImagePathPolishRight + GenerateImageName());
                    break;
            }

            Log(string.Format("右{0}打磨模板定位", index));
        }






        public PointF? LocateSolderLeftShape(HImage hImage, int shapeIndex, HWndCtrller hWndCtrller,out float Ang)
        {
            PointF? point = null;
            if (shapeIndex >= 0 && shapeIndex < VisionTools.SolderLeft.Count)
            {
                Model shape = VisionTools.SolderLeft[shapeIndex].Shape;

                // 模板匹配
                HImage image = hImage;
                if (shape.InputImg != null)
                {
                    shape.InputImg.Dispose();
                }
                shape.InputImg = image;


                //if (shape is NCCModel)
                //{
                //    ((NCCModel)shape).nCCParam.NumMatches = 1;
                //}

                //if (shape is ShapeModel)
                //{
                //    ((ShapeModel)shape).shapeParam.mNumMatches = 1;
                //}


                shape.FindModel();
                DisplayHobject(hWndCtrller, hImage);//显示图片
                DisplayModelResult(hWndCtrller, shape);//显示模板

                // 结果转换
                ShapeMatchResult match = shape.OutputResult;
                if (match.Count > 0)
                {
                    int imgWidth, imgHeight;
                    image.GetImageSize(out imgWidth, out imgHeight);

                    PointF worldPoint;
                    Calib[0].PixelPointToWorldPoint(new PointF(match.Col[0].F, match.Row[0].F),
                        out worldPoint,
                        new PointF(imgWidth / 2f, imgHeight / 2f),
                        new PointF());

                    point = worldPoint;
                    Ang = match.Angle[0].F;
                }
                else
                {
                    Ang = 0;
                }
            }
            else
            {
                Ang = 0;
            }

            return point;
        }
        public PointF? LocateSolderRightShape(HImage hImage, int shapeIndex, HWndCtrller hWndCtrller,out float Ang)
        {
            PointF? point = null;
            if (shapeIndex >= 0 && shapeIndex < VisionTools.SolderRight.Count)
            {
                Model shape = VisionTools.SolderRight[shapeIndex].Shape;

                // 模板匹配
                HImage image = hImage;
                if (shape.InputImg != null)
                {
                    shape.InputImg.Dispose();
                }
                shape.InputImg = image;
                shape.FindModel();
                DisplayHobject(hWndCtrller, hImage);
                DisplayModelResult(hWndCtrller, shape);

                // 结果转换
                ShapeMatchResult match = shape.OutputResult;
                if (match.Count > 0)
                {
                    int imgWidth, imgHeight;
                    image.GetImageSize(out imgWidth, out imgHeight);

                    PointF worldPoint;
                    Calib[1].PixelPointToWorldPoint(new PointF(match.Col[0].F, match.Row[0].F),
                        out worldPoint,
                        new PointF(imgWidth / 2f, imgHeight / 2f),
                        new PointF());

                    point = worldPoint;
                    Ang = match.Angle[0].F;
                }
                else
                {
                    Ang = 0;
                }
            }
            else
            {
                Ang = 0;
            }
            return point;
        }
        public PointF? LocatePolishLeftShape(HImage hImage, int shapeIndex, HWndCtrller hWndCtrller)
        {
            PointF? point = null;
            if (shapeIndex >= 0 && shapeIndex < VisionTools.PolishLeft.Count)
            {
                Model shape = VisionTools.PolishLeft[shapeIndex].Shape;

                // 模板匹配
                HImage image = hImage;
                if (shape.InputImg != null)
                {
                    shape.InputImg.Dispose();
                }
                shape.InputImg = image;
                shape.FindModel();
                DisplayHobject(hWndCtrller, hImage);
                DisplayModelResult(hWndCtrller, shape);

                // 结果转换
                ShapeMatchResult match = shape.OutputResult;
                if (match.Count > 0)
                {
                    int imgWidth, imgHeight;
                    image.GetImageSize(out imgWidth, out imgHeight);

                    PointF worldPoint;
                    Calib[2].PixelPointToWorldPoint(new PointF(match.Col[0].F, match.Row[0].F),
                        out worldPoint,
                        new PointF(imgWidth / 2f, imgHeight / 2f),
                        new PointF());

                    point = worldPoint;
                }
            }

            return point;
        }
        public PointF? LocatePolishRightShape(HImage hImage, int shapeIndex, HWndCtrller hWndCtrller)
        {
            PointF? point = null;
            if (shapeIndex >= 0 && shapeIndex < VisionTools.PolishRight.Count)
            {
                Model shape = VisionTools.PolishRight[shapeIndex].Shape;

                // 模板匹配
                HImage image = hImage;
                if (shape.InputImg != null)
                {
                    shape.InputImg.Dispose();
                }
                shape.InputImg = image;
                shape.FindModel();
                DisplayHobject(hWndCtrller, hImage);
                DisplayModelResult(hWndCtrller, shape);

                // 结果转换
                ShapeMatchResult match = shape.OutputResult;
                if (match.Count > 0)
                {
                    int imgWidth, imgHeight;
                    image.GetImageSize(out imgWidth, out imgHeight);

                    PointF worldPoint;
                    Calib[3].PixelPointToWorldPoint(new PointF(match.Col[0].F, match.Row[0].F),
                        out worldPoint,
                        new PointF(imgWidth / 2f, imgHeight / 2f),
                        new PointF());

                    point = worldPoint;
                }
            }

            return point;
        }




        private readonly string ImagePathSolderLeft = Application.StartupPath + "\\Image\\SolderLeft\\";
        private readonly string ImagePathSolderRight = Application.StartupPath + "\\Image\\SolderRight\\";
        private readonly string ImagePathPolishLeft = Application.StartupPath + "\\Image\\PolishLeft\\";
        private readonly string ImagePathPolishRight = Application.StartupPath + "\\Image\\PolishRight\\";

        public int ImagePathSolderLeftState { get; set; }
        public int ImagePathSolderRightState { get; set; }
        public int ImagePathPolishLeftState { get; set; }
        public int ImagePathPolishRightState { get; set; }




        private string GenerateImageName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.ffff");
        }

        private void SaveImage(HObject img, string fileName)
        {
            try
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(fileName));
                if (directory.Exists == false)
                {
                    directory.Create();
                }
                else
                {
                    var va = directory.GetFiles("*.bmp");
                    long size = va.Sum(e => e.Length);
                    long maxSize = 1024 * 1024 * 1024 * 1;
                    while (size > maxSize)
                    {
                        Array.Sort(va, ComparerFiles);
                        va[0].Delete();
                        va = directory.GetFiles("*.bmp");
                        size = va.Sum(e => e.Length);
                    }
                }

                HOperatorSet.WriteImage(img, "bmp", 0, fileName);
            }
            catch
            {

            }
        }

        int ComparerFiles(FileInfo file1, FileInfo file2)
        {
            return (int)(file1.CreationTime - file2.CreationTime).TotalSeconds;
        }
        #endregion



        #region  模板创建

        private bool creatingModel = false;

        private void SetTempleteModel(ShapeModel shape, HImage img)
        {
            creatingModel = true;
            if (img != null)
            {
                if (shape.InputImg != null)
                {
                    shape.InputImg.Dispose();
                }
                shape.InputImg = img;
                shape.ShowSetting();
            }
            creatingModel = false;
        }

        private void SetTempleteModel(NCCModel nCC, HImage img)
        {
            creatingModel = true;
            if (img != null)
            {
                if (nCC.InputImg != null)
                {
                    nCC.InputImg.Dispose();
                }
                else
                {

                }
                nCC.InputImg = img;
                nCC.ShowSetting();
            }
            creatingModel = false;
        }

        /// <summary>
        /// 设置焊锡左模板
        /// </summary>
        public void SolderLeftShape(int index, HImage img = null)
        {
            if (img == null) img = GetCurrentImage(0);
            SetTempleteModel(VisionTools.SolderLeft[index].Shape, img);
        }
        /// <summary>
        /// 设置焊锡右模板
        /// </summary>
        public void SolderRightShape(int index, HImage img = null)
        {
            if (img == null) img = GetCurrentImage(0);
            SetTempleteModel(VisionTools.SolderRight[index].Shape, GetCurrentImage(1));
        }
        /// <summary>
        /// 设置打磨左模板
        /// </summary>
        public void PolishLeftShape(int index, HImage img = null)
        {
            if (img == null) img = GetCurrentImage(0);
            SetTempleteModel(VisionTools.PolishLeft[index].Shape, GetCurrentImage(2));
        }
        /// <summary>
        /// 设置打磨右模板
        /// </summary>
        public void PolishRightShape(int index, HImage img = null)
        {
            if (img == null) img = GetCurrentImage(0);
            SetTempleteModel(VisionTools.PolishRight[index].Shape, GetCurrentImage(2));
        }

        /// <summary>
        /// 识别模板中心位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PointF GetSolderLeftShapeDeviation(int index)
        {
            return VisionTools.SolderLeft[index].GetShapeDeviation(Calib[0]);
        }
        public PointF GetSolderRightShapeDeviation(int index)
        {
            return VisionTools.SolderRight[index].GetShapeDeviation(Calib[1]);
        }
        public PointF GetPolishLeftShapeDeviation(int index)
        {
            return VisionTools.PolishLeft[index].GetShapeDeviation(Calib[2]);
        }
        public PointF PolishRightShapeDeviation(int index)
        {
            return VisionTools.PolishRight[index].GetShapeDeviation(Calib[3]);
        }

        #endregion



        private HImage PercondHImage(HObject hoImage)
        {
            HObject imageClosing = null, imageMean = null;
            HImage perImage = null;
            try
            {
                if (hoImage != null && hoImage.IsInitialized())
                {
                    HOperatorSet.GrayClosingRect(hoImage, out imageClosing, 7, 7);
                    HOperatorSet.MeanImage(imageClosing, out imageMean, 3, 3);
                    perImage = new HImage(imageMean);
                }
                return perImage;
            }
            finally
            {
                if (imageClosing != null) imageClosing.Dispose();
                if (imageMean != null) imageMean.Dispose();
            }
        }


        public void NewVisionTool()
        {
            this.VisionTools = new VisionTool();
        }

        public void SaveVisionTool(string fileName)
        {
            global::Vision.Tool.Serialization.SaveToFile(VisionTools, fileName, true);
        }


        public void LoadVisionTool(string fileName)
        {
            VisionTool tools = global::Vision.Tool.Serialization.LoadFromFile<VisionTool>(fileName);
            if (tools != null)
            {
                VisionTools = global::Vision.Tool.Serialization.LoadFromFile<VisionTool>(fileName);
            }
        }


    }








}
