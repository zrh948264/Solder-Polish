using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;
using Common;
using Config;
using MyControl;
using Motion;
using Logic;
using System.Collections.Generic;
using HZZH.Vision.Logic;
using SYMVLightDLL_CShare;
using Vision.Logic;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Linq;

namespace UI
{
	public partial class FormMain : Form
	{
		public static LogicMain RunProcess = new LogicMain();
        public FormMain()
		{
            try
            {
                InitializeComponent();
                StartUpdate.SendStartMsg("应用程序启动 请稍等>>>");                 
                Config.ConfigHandle.Instance.Load();
                InitializeControl();
                
                ShowMessge.StartMsg += new ShowMessge.SendStartMsgEventHandler(ShowMessage);
                StartUpdate.SendStartMsg("通信连接");

                RunProcess.movedriverZm.ConnectCtrller(Config.ConfigHandle.Instance.SystemDefine.IP, Config.ConfigHandle.Instance.SystemDefine.Port);

                VisionProject.Instance.InitVisionProject();
                VisionProject.Instance.calibrateSetting[0].PlatformMove = new MotionPlatform(RunProcess.movedriverZm, (int)AxisDef.AxX1, (int)AxisDef.AxY1);
                VisionProject.Instance.calibrateSetting[1].PlatformMove = new MotionPlatform(RunProcess.movedriverZm, (int)AxisDef.AxX2, (int)AxisDef.AxY2);
                VisionProject.Instance.calibrateSetting[2].PlatformMove = new MotionPlatform(RunProcess.movedriverZm, (int)AxisDef.AxX3, (int)AxisDef.AxY1);
                VisionProject.Instance.calibrateSetting[3].PlatformMove = new MotionPlatform(RunProcess.movedriverZm, (int)AxisDef.AxX3, (int)AxisDef.AxY2);
                
                StartUpdate.SendStartMsg("通信连接完成");
                StartUpdate.SendStartMsg("正在进入系统>>>");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
				timer1.Interval = 200;
                timer1.Enabled = true;
            }
		}

        #region 用户

        User CurrentUser;

        //设置按钮为不可用 
        private void BtnIsEnabled()
        {

            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled =
            toolStripButton4.Enabled = 主页Bt.Enabled = toolStripDropDownButton1.Enabled =
            电机参数Bt.Enabled = IO监控Bt.Enabled = 日志Bt.Enabled = toolStripDropDownButton2.Enabled =
            toolStripButton5.Enabled = toolStripButton25.Enabled = toolStripButton6.Enabled =
            toolStripButton16.Enabled = button8.Enabled = button7.Enabled = checkBox5.Enabled =
            checkBox7.Enabled = checkBox8.Enabled = ComBox.Enabled = comboBox1.Enabled = panel7.Enabled =button1.Enabled =comboBox2.Enabled=
            numericUpDown52.Enabled =
            //视觉
            tabControl3.Enabled = 缩放.Enabled = 恢复.Enabled =
            移动.Enabled = 触发相机.Enabled = 相机实时.Enabled = 相机设置.Enabled = 曝光.Enabled =
            toolStripTextBox1.Enabled = toolStripLabel3.Enabled = toolStripComboBox1.Enabled =

            false;


        }
        //设置按钮可用
        private void BtnEnabled()
        {

            toolStripButton1.Enabled = toolStripButton2.Enabled = toolStripButton3.Enabled =
            toolStripButton4.Enabled = 主页Bt.Enabled = toolStripDropDownButton1.Enabled =
            电机参数Bt.Enabled = IO监控Bt.Enabled = 日志Bt.Enabled = toolStripDropDownButton2.Enabled =
            toolStripButton5.Enabled = toolStripButton25.Enabled = toolStripButton6.Enabled =
            toolStripButton16.Enabled = button8.Enabled = button7.Enabled = checkBox5.Enabled =
            checkBox7.Enabled = checkBox8.Enabled = ComBox.Enabled = comboBox1.Enabled = panel7.Enabled = button1.Enabled = comboBox2.Enabled =
            numericUpDown52.Enabled=

            //视觉
            tabControl3.Enabled = 缩放.Enabled = 恢复.Enabled =
            移动.Enabled = 触发相机.Enabled = 相机实时.Enabled = 相机设置.Enabled = 曝光.Enabled =
            toolStripTextBox1.Enabled = toolStripLabel3.Enabled = toolStripComboBox1.Enabled =

            true;
        }
        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserLogin frm = new UserLogin();
            if (DialogResult.OK == frm.ShowDialog())
            {

                UserMgrLogos(frm.GetCurrentUser());
                userInfo.GetUserList(frm.GetCurrentUser());
            }
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUserPwd frm_ChangePwd = new ChangeUserPwd();
            frm_ChangePwd.SetUser(CurrentUser);
            if (DialogResult.OK == frm_ChangePwd.ShowDialog(this))
            {
                CurrentUser = frm_ChangePwd.GetCurrentUser();
            }
        }
        
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserMgrIntialize();
            BtnIsEnabled();
            登录ToolStripMenuItem.Enabled = true;
            tabControl1.SelectedIndex = 0;
            tabControl2.SelectedIndex = 0;
        }
       
        private void UserMgrIntialize()
        {
            修改密码ToolStripMenuItem.Enabled = 退出ToolStripMenuItem.Enabled = 
            用户管理ToolStripMenuItem.Enabled = false;

            CurrentUser = null;
            //tsslbl_loginUserMsg.Text = "";
            this.Text = "打磨上锡机 V00.00.00" + "";
        }
        
        private void UserMgrLogos(User user1)
        {
            try
            {
                if (user1.Type != "")
                {
                    CurrentUser = user1;
                    //tsslbl_loginUserMsg.Text = user1.Name;
                    this.Text = "打磨上锡机 V00.00.00" + user1.Name;
                    switch (user1.Type)
                    {
                        case "0":
                            toolStripButton5.Enabled = toolStripButton25.Enabled = toolStripButton6.Enabled =
                            toolStripButton16.Enabled = true;
                            用户管理ToolStripMenuItem.Enabled = false;
                            break;
                        case "1":
                            BtnEnabled();
                            用户管理ToolStripMenuItem.Enabled = true;
                            break;
                        case "2":
                            BtnEnabled();
                            用户管理ToolStripMenuItem.Enabled = true;
                            break;
                        case "3":
                            BtnEnabled();
                            用户管理ToolStripMenuItem.Enabled = true;
                            break;
                        default:
                            break;
                    }

                    修改密码ToolStripMenuItem.Enabled = 退出ToolStripMenuItem.Enabled = true;
                    登录ToolStripMenuItem.Enabled = false;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            userInfo.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
            userInfo.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
            userInfo.Size = this.panel4.Size;
            userInfo.Parent = this.panel4;//指定子窗体显示的容器
            userInfo.Dock = DockStyle.Fill;
            userInfo.Show();
            userInfo.Activate();
        }
        #endregion

        #region 菜单

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton toolbtn = sender as ToolStripButton;
            if (toolbtn.Tag != null)
            {
                switch (toolbtn.Tag.ToString())
                {
                    case "0"://生产
                        tabControl2.SelectedIndex = 0;
                        tabControl1.SelectedIndex = 0;
                        break;
                    case "1"://电机参数
                        tabControl1.SelectedIndex = 1;
                        Motor.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
                        Motor.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
                        Motor.Size = this.panel1.Size;
                        Motor.Parent = this.panel1;//指定子窗体显示的容器
                        Motor.Dock = DockStyle.Fill;
                        Motor.Show();
                        Motor.Activate();
                        break;
                    case "2"://IO
                        tabControl1.SelectedIndex = 2;
                        IOControl.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
                        IOControl.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
                        IOControl.Size = this.panel2.Size;
                        IOControl.Parent = this.panel2;//指定子窗体显示的容器
                        IOControl.Dock = DockStyle.Fill;
                        IOControl.Show();
                        IOControl.Activate();
                        break;
                    case "3"://日志
                        tabControl1.SelectedIndex = 3;
                        formLog.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
                        formLog.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
                        formLog.Size = this.panel3.Size;
                        formLog.Parent = this.panel3;//指定子窗体显示的容器
                        formLog.Dock = DockStyle.Fill;
                        formLog.Show();
                        formLog.Activate();
                        break;
                    case "退出":
                        if (MessageBox.Show("是否退出软件", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            Config.ConfigHandle.Instance.Save();
                            Application.Exit();
                            //System.Environment.Exit(0);
                        }
                        else
                        {
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void btn_PrjFileControl_Click(object sender, EventArgs e)
        {
            RunProcess.DataToSlaver();//
            ToolStripButton toolbtn = sender as ToolStripButton;
            if (toolbtn.Tag != null)
            {
                switch (toolbtn.Tag.ToString())
                {
                    case "PrjFileOpen":
                        btn_PrjFileOpen();
                        break;
                    case "PrjFileNew":
                        if(MessageBox.Show("是否新建程式", "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                        {
                            btn_PrjFileNew();
                            VisionProject.Instance.NewVisionTool();
                        }
                        break;
                    case "PrjFileSave":
                        btn_PrjFileSave();
                        break;
                    case "PrjFileSaveAs":
                        btn_PrjFileSaveAs();
                        break;
                    default:
                        break;
                }
            }
        }
        
        #endregion 

        #region  窗体事件

        private void Form_SubMain_Load(object sender, EventArgs e)
		{
             
            BtnIsEnabled();
            if (ConfigHandle.Instance.SystemDefine.ProjectDirectory != null && ConfigHandle.Instance.SystemDefine.ProjectDirectory != "")
            {
                OpenProject(ConfigHandle.Instance.SystemDefine.ProjectDirectory);
            }

            #region 加载视觉
            VisionProject.Instance.SetDisplayWindow(0, this.hWindowControl1);
            VisionProject.Instance.SetDisplayWindow(1, this.hWindowControl2);
            VisionProject.Instance.SetDisplayWindow(2, this.hWindowControl0);
            toolStripButton18.Click += (s, ev) => { VisionProject.Instance.CamState[0] = false; VisionProject.Instance.CameraSoft(0); };
            toolStripButton32.Click += (s, ev) => { VisionProject.Instance.CamState[1] = false; VisionProject.Instance.CameraSoft(1); };
            触发相机.Click += (s, ev) => { VisionProject.Instance.CamState[2] = false; VisionProject.Instance.CameraSoft(2); };

            toolStripButton19.Click += (s, ev) => { VisionProject.Instance.CamState[0] = true; };
            toolStripButton33.Click += (s, ev) => { VisionProject.Instance.CamState[1] = true; };
            相机实时.Click += (s, ev) => { VisionProject.Instance.CamState[2] = true; };

            toolStripButton20.Click += (s, ev) => { VisionProject.Instance.ShowCameraSetPage(0); };
            toolStripButton34.Click += (s, ev) => { VisionProject.Instance.ShowCameraSetPage(1); };
            相机设置.Click += (s, ev) => { VisionProject.Instance.ShowCameraSetPage(2); };

            toolStripTextBox2.Text = VisionProject.Instance.GetCameraExposureTime(0).ToString();
            toolStripTextBox4.Text = VisionProject.Instance.GetCameraExposureTime(1).ToString();
            toolStripTextBox1.Text = VisionProject.Instance.GetCameraExposureTime(2).ToString();
            toolStripTextBox2.TextChanged += toolStripTextBox2_TextChanged;
            toolStripTextBox4.TextChanged += toolStripTextBox4_TextChanged;
            toolStripTextBox1.TextChanged += toolStripTextBox1_TextChanged;

            toolStripButton14.Click += (s, ev) => { VisionProject.Instance.SetViewModeMove(0); };
            toolStripButton29.Click += (s, ev) => { VisionProject.Instance.SetViewModeMove(1); };
            移动.Click += (s, ev) => { VisionProject.Instance.SetViewModeMove(2); };

            toolStripButton15.Click += (s, ev) => { VisionProject.Instance.SetViewModeZoom(0); };
            toolStripButton30.Click += (s, ev) => { VisionProject.Instance.SetViewModeZoom(1); };
            缩放.Click += (s, ev) => { VisionProject.Instance.SetViewModeZoom(2); };

            toolStripButton17.Click += (s, ev) => { VisionProject.Instance.SetViewModeNone(0); };
            toolStripButton31.Click += (s, ev) => { VisionProject.Instance.SetViewModeNone(1); };
            恢复.Click += (s, ev) => { VisionProject.Instance.SetViewModeNone(2); };

            toolStripComboBox2.SelectedIndexChanged += (s, ev) => { VisionProject.Instance.ImagePathSolderLeftState = ((ToolStripComboBox)s).SelectedIndex; };
            toolStripComboBox3.SelectedIndexChanged += (s, ev) => { VisionProject.Instance.ImagePathSolderRightState = ((ToolStripComboBox)s).SelectedIndex; };
            toolStripComboBox1.SelectedIndexChanged += (s, ev) =>
            {
                VisionProject.Instance.ImagePathPolishLeftState = ((ToolStripComboBox)s).SelectedIndex;
                VisionProject.Instance.ImagePathPolishRightState = ((ToolStripComboBox)s).SelectedIndex;
            };
            #endregion

            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox2.SelectedIndex = 0;
            toolStripComboBox3.SelectedIndex = 0;

            #region 加载点动
            jog.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
            jog.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
            jog.Size = this.panel7.Size;
            jog.Parent = this.panel7;//指定子窗体显示的容器
            jog.Dock = DockStyle.Fill;
            jog.Show();
            jog.Activate();
            #endregion

        }
        void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (int.TryParse(((ToolStripTextBox)sender).Text, out val) == true)
            {
                VisionProject.Instance.SetCameraExposureTime(0, val);
            }
        }
        void toolStripTextBox4_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (int.TryParse(((ToolStripTextBox)sender).Text, out val) == true)
            {
                VisionProject.Instance.SetCameraExposureTime(1, val);
            }
        }
        void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (int.TryParse(((ToolStripTextBox)sender).Text, out val) == true)
            {
                VisionProject.Instance.SetCameraExposureTime(2, val);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logic.LogicMain.LogicThreadLife = false;
            Config.ConfigHandle.Instance.Save();
            VisionProject.Instance.CurrentDomain_ProcessExit(null, EventArgs.Empty);
            
        }

        #endregion

        #region 常用事件

        private void DataBinding()
		{
            try
            {
                #region 位置

                Functions.SetBinding(checkBox1, "Checked", RunProcess.LogicData.slaverData.rstPos1, "modeChecked");//功能选用：平台1复位停靠
                Functions.SetBinding(checkBox2, "Checked", RunProcess.LogicData.slaverData.rstPos2, "modeChecked");//功能选用：平台1复位停靠
                Functions.SetBinding(checkBox6, "Checked", RunProcess.LogicData.slaverData.rstPos, "modeChecked");//功能选用：平台1复位停靠
                
                Functions.SetBinding(numericUpDown1, "Value", RunProcess.LogicData.slaverData.rstPos1, "X");//平台1复位停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown2, "Value", RunProcess.LogicData.slaverData.rstPos1, "Y");//平台1复位停靠位：Y的数据绑定
                Functions.SetBinding(numericUpDown4, "Value", RunProcess.LogicData.slaverData.rstPos1, "Z");//平台1复位停靠位：Z的数据绑定
                Functions.SetBinding(numericUpDown3, "Value", RunProcess.LogicData.slaverData.rstPos1, "R");//平台1复位停靠位：R的数据绑定
                Functions.SetBinding(numericUpDown5, "Value", RunProcess.LogicData.slaverData.rstPos1, "T");//平台1复位停靠位：T的数据绑定

                Functions.SetBinding(numericUpDown10, "Value", RunProcess.LogicData.slaverData.rstPos2, "X");//平台2复位停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown9, "Value", RunProcess.LogicData.slaverData.rstPos2, "Y");//平台2复位停靠位：Y的数据绑定
                Functions.SetBinding(numericUpDown8, "Value", RunProcess.LogicData.slaverData.rstPos2, "Z");//平台2复位停靠位：Z的数据绑定
                Functions.SetBinding(numericUpDown7, "Value", RunProcess.LogicData.slaverData.rstPos2, "R");//平台2复位停靠位：R的数据绑定
                Functions.SetBinding(numericUpDown6, "Value", RunProcess.LogicData.slaverData.rstPos2, "T");//平台2复位停靠位：T的数据绑定
                
                Functions.SetBinding(numericUpDown15, "Value", RunProcess.LogicData.slaverData.endPos2, "X");//平台1工作后停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown14, "Value", RunProcess.LogicData.slaverData.endPos2, "Y");//平台1工作后停靠位：Y的数据绑定
                Functions.SetBinding(numericUpDown13, "Value", RunProcess.LogicData.slaverData.endPos2, "Z");//平台1工作后停靠位：Z的数据绑定m
                Functions.SetBinding(numericUpDown12, "Value", RunProcess.LogicData.slaverData.endPos2, "R");//平台1工作后停靠位：R的数据绑定
                Functions.SetBinding(numericUpDown11, "Value", RunProcess.LogicData.slaverData.endPos2, "T");//平台1工作后停靠位：T的数据绑定

                Functions.SetBinding(numericUpDown20, "Value", RunProcess.LogicData.slaverData.endPos1, "X");//平台2工作后停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown19, "Value", RunProcess.LogicData.slaverData.endPos1, "Y");//平台2工作后停靠位：Y的数据绑定
                Functions.SetBinding(numericUpDown18, "Value", RunProcess.LogicData.slaverData.endPos1, "Z");//平台2工作后停靠位：Z的数据绑定
                Functions.SetBinding(numericUpDown17, "Value", RunProcess.LogicData.slaverData.endPos1, "R");//平台2工作后停靠位：R的数据绑定
                Functions.SetBinding(numericUpDown16, "Value", RunProcess.LogicData.slaverData.endPos1, "T");//平台2工作后停靠位：T的数据绑定

                Functions.SetBinding(numericUpDown30, "Value", RunProcess.LogicData.slaverData.rstPos, "X");//打磨头复位停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown28, "Value", RunProcess.LogicData.slaverData.rstPos, "Z");//打磨头复位停靠位：Z的数据绑定
                Functions.SetBinding(numericUpDown27, "Value", RunProcess.LogicData.slaverData.rstPos, "R");//打磨头复位停靠位：R的数据绑定
                
                Functions.SetBinding(numericUpDown50, "Value", RunProcess.LogicData.slaverData.basics, "Safe_ZL");//位置：左安全位置的数据绑定
                Functions.SetBinding(numericUpDown49, "Value", RunProcess.LogicData.slaverData.basics, "Safe_ZR");//位置：右安全位置的数据绑定
                Functions.SetBinding(numericUpDown48, "Value", RunProcess.LogicData.slaverData.basics, "Safe_Z");//位置：打磨安全高度的数据绑定
                Functions.SetBinding(numericUpDown47, "Value", RunProcess.LogicData.slaverData.basics, "TurnAvoidPos_XL");//位置：左翻转的数据绑定
                Functions.SetBinding(numericUpDown46, "Value", RunProcess.LogicData.slaverData.basics, "TurnAvoidPos_XR");//位置：右翻转的数据绑定
                #endregion

                Functions.SetBinding(numericUpDown21, "Value", RunProcess.LogicData.RunData, "vDeley");//拍照前延时
                Functions.SetBinding(numericUpDown26, "Value", RunProcess.LogicData.RunData, "moveSpd");//拍照前延时
                Functions.SetBinding(numericUpDown53, "Value", RunProcess.LogicData.slaverData.basics, "polish_z_pos");//拍照前延时
             
                Functions.SetBinding(numericUpDown_LSpd, "Value", RunProcess.LogicData.slaverData.basics, "WeldSpeedL");//速度窗口：左上锡速度的数据绑定
                Functions.SetBinding(numericUpDown42, "Value", RunProcess.LogicData.slaverData.basics, "WeldSpeedR");//速度窗口：有上锡速度的数据绑定
                Functions.SetBinding(numericUpDown41, "Value", RunProcess.LogicData.slaverData.basics, "TeachSpeedL");//速度窗口：左示教速度的数据绑定
                Functions.SetBinding(numericUpDown40, "Value", RunProcess.LogicData.slaverData.basics, "TeachSpeedR");//速度窗口：右示教速度的数据绑定
                Functions.SetBinding(numericUpDown39, "Value", RunProcess.LogicData.slaverData.basics, "PolishSpeed");//速度窗口：打磨速度的数据绑定
                Functions.SetBinding(numericUpDown44, "Value", RunProcess.LogicData.slaverData.basics, "TeachSpeed");//速度窗口：示教速度的数据绑定

                #region 清洗参数
                Functions.SetBinding(numericUpDown25, "Value", RunProcess.LogicData.rinseData.posL, "X");//平台1复位停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown24, "Value", RunProcess.LogicData.rinseData.posL, "Y");//平台1复位停靠位：Y的数据绑定
                Functions.SetBinding(numericUpDown23, "Value", RunProcess.LogicData.rinseData.posL, "Z");//平台1复位停靠位：Z的数据绑定
                Functions.SetBinding(numericUpDown22, "Value", RunProcess.LogicData.rinseData.posL, "R");//平台1复位停靠位：R的数据绑定
                //Functions.SetBinding(numericUpDown21, "Value", RunProcess.LogicData.rinseData.posL, "T");//平台1复位停靠位：T的数据绑定

                Functions.SetBinding(numericUpDown33, "Value", RunProcess.LogicData.rinseData.posR, "X");//平台2复位停靠位：X的数据绑定
                Functions.SetBinding(numericUpDown32, "Value", RunProcess.LogicData.rinseData.posR, "Y");//平台2复位停靠位：Y的数据绑定
                Functions.SetBinding(numericUpDown31, "Value", RunProcess.LogicData.rinseData.posR, "Z");//平台2复位停靠位：Z的数据绑定
                Functions.SetBinding(numericUpDown29, "Value", RunProcess.LogicData.rinseData.posR, "R");//平台2复位停靠位：R的数据绑定
                //Functions.SetBinding(numericUpDown26, "Value", RunProcess.LogicData.rinseData.posR, "T");//平台2复位停靠位：T的数据绑定

                Functions.SetBinding(numericUpDown36, "Value", RunProcess.LogicData.rinseData, "BackLen");//清洗参数：退锡长度的数据绑定
                Functions.SetBinding(numericUpDown35, "Value", RunProcess.LogicData.rinseData, "BackSpeed");//清洗参数：退锡速度的数据绑定
                Functions.SetBinding(numericUpDown34, "Value", RunProcess.LogicData.rinseData, "CleanTime");//清洗参数：清洗时间的数据绑定
                Functions.SetBinding(numericUpDown38, "Value", RunProcess.LogicData.rinseData, "FrontLen");//清洗参数：送锡长度的数据绑定
                Functions.SetBinding(numericUpDown37, "Value", RunProcess.LogicData.rinseData, "FrontSpeed");//清洗参数：送锡速度的数据绑定

                #endregion


                Functions.SetBinding(checkBox5, "Checked", RunProcess.LogicData.slaverData.basics, "TinDetectEn");//功能选用：锡丝检测的数据绑定
                Functions.SetBinding(checkBox7, "Checked", RunProcess.LogicData.slaverData.basics, "CleanEn");//功能选用：清洗的数据绑定
                Functions.SetBinding(checkBox8, "Checked", RunProcess.LogicData.slaverData.basics, "ShakeEn");//功能选用：抖动的数据绑定

                Functions.SetBinding(ComBox, "SelectedIndex", RunProcess.LogicData.slaverData.basics, "StartRunMode");// 0按钮模式， 1工件感应模式，2工件按钮模式

                Functions.SetBinding(comboBox1, "SelectedIndex", RunProcess.LogicData.RunData, "rinseMode");//清洗方式
                Functions.SetBinding(comboBox2, "SelectedIndex", RunProcess.LogicData.slaverData.basics, "DevcieMode");

                Functions.SetBinding(numericUpDown45, "Value", RunProcess.LogicData.slaverData.basics, "PolishOffset");
                Functions.SetBinding(numericUpDown43, "Value", RunProcess.LogicData.slaverData.basics, "PolishTimes");
                Functions.SetBinding(numericUpDown51, "Value", RunProcess.LogicData.slaverData.basics, "PolishBlowDelay");
                Functions.SetBinding(numericUpDown52, "Value", RunProcess.LogicData.RunData, "clearnum");//每几个点清洗
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据绑定有问题 " + ex.ToString());
            }

        }

        private InputOutput IOControl;
        private UserMachinePrm Motor ;
        private UserInfo userInfo;
        private Jog jog;
        private FormLog formLog;
        private Form1_Layout form1;

        public void InitializeControl()
        {
            StartUpdate.SendStartMsg("初始化控件");
            formLog = new FormLog();
            userInfo = new UserInfo();

            IOControl = new InputOutput();
            Motor = new UserMachinePrm();
            jog = new Jog();

            uph_list.CollectionChanged += list_CollectionChanged;//计算UPH

            IOControl.SetMoveController(RunProcess.movedriverZm);
            Motor.SetMoveController(RunProcess.movedriverZm);
            jog.SetMoveController(RunProcess.movedriverZm);
            
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.SizeMode = TabSizeMode.Fixed;

            tabControl2.ItemSize = new Size(0, 1);
            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.SizeMode = TabSizeMode.Fixed;


            tabControl1.SelectedIndex = 0;
            StartUpdate.SendStartMsg("控件初始化完成");
        }
        

        #endregion

        #region 启动停止

        private void btn_FsmControl_Click(object sender, EventArgs e)
        {
            ToolStripButton toolbtn = sender as ToolStripButton;

            panel5.Enabled = toolStripButton4.Enabled = toolStripButton3.Enabled = toolStripButton1.Enabled = toolStripButton2.Enabled =
                主页Bt.Enabled = 日志Bt.Enabled = 电机参数Bt.Enabled = IO监控Bt.Enabled =
                toolStripDropDownButton1.Enabled = toolStripDropDownButton2.Enabled = true ;

            if (toolbtn.Tag != null)
            {
                switch (toolbtn.Tag.ToString())
                {
                    case "FsmStart":
                        btn_FsmStart();
                        VisionProject.Instance.CamState[0] = false;
                        VisionProject.Instance.CamState[1] = false;
                        VisionProject.Instance.CamState[2] = false;

                        tabControl2.SelectedIndex = tabControl1.SelectedIndex = 0;
                        panel5.Enabled = toolStripButton4.Enabled = toolStripButton3.Enabled = toolStripButton1.Enabled = toolStripButton2.Enabled =
                            主页Bt.Enabled = 日志Bt.Enabled = 电机参数Bt.Enabled = IO监控Bt.Enabled =
                            toolStripDropDownButton1.Enabled = toolStripDropDownButton2.Enabled = false;
                        break;
                    case "FsmPause":
                        btn_FsmPause();

                        break;
                    case "FsmStop":
                        btn_FsmStop();
                        VisionProject.Instance.CamState[0] = true;
                        VisionProject.Instance.CamState[1] = true;
                        VisionProject.Instance.CamState[2] = true;
                        break;
                    case "FsmReset":
                        btn_FsmReset();
                        break;
                    default:
                        break;
                }
            }
        }
        
        /// <summary>
        /// 启动
        /// </summary>
        public void btn_FsmStart()
        {
            if (RunProcess.movedriverZm.Succeed)
            {
                RunProcess.DataToSlaver();
                RunProcess.FSM.RunMode = (RunModeDef)RunProcess.LogicData.slaverData.basics.DevcieMode;
                RunProcess.FSM.Run(RunProcess.FSM.RunMode);
                userCtrlMsgListView1.AddUserMsg("设备启动", "提示");
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void btn_FsmPause()
        {
            if (RunProcess.movedriverZm.Succeed)
            {
                RunProcess.FSM.Pause();
                userCtrlMsgListView1.AddUserMsg("设备暂停", "提示");
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void btn_FsmStop()
        {
            if (RunProcess.movedriverZm.Succeed)
            {
                RunProcess.ProcessData = new ProcessDataDef();
                RunProcess.LogicTask = new LogicTaskDef();

                RunProcess.FSM.Stop();
                userCtrlMsgListView1.AddUserMsg("设备停止", "提示");
            }
        }

        /// <summary>
        /// 复位
        /// </summary>
        public void btn_FsmReset()
        {
            if (RunProcess.movedriverZm.Succeed)
            {
                if (lbl_RunStates.Text != "设备运行")
                {
                    RunProcess.DataToSlaver();
                    RunProcess.ProcessData = new ProcessDataDef();
                    RunProcess.LogicTask = new LogicTaskDef();

                    CloseWindow("识别错误");
                    RunProcess.FSM.Reset();
                    userCtrlMsgListView1.AddUserMsg("设备复位", "提示");
                }
            }
        }

        #endregion

        #region 工程调度

        string pathRoad = null;

        /// <summary>
        /// 打开文件
        /// </summary>
        public void btn_PrjFileOpen()
        {
            OpenFileDialog openDLG = new OpenFileDialog();
            openDLG.Title = "选择项目文件";
            openDLG.Multiselect = false;
            openDLG.Filter = "选择项目文件|*.pro";
            openDLG.InitialDirectory = "D:\\程式\\";
            if (openDLG.ShowDialog() == DialogResult.OK)
            {
                OpenProject(openDLG.FileName);

                VisionProject.Instance.LoadVisionTool(Path.Combine(Path.GetDirectoryName(openDLG.FileName), Path.GetFileNameWithoutExtension(openDLG.FileName)) + ".Vision");
               
            }
        }

        private void OpenProject(string path)
        {
            try
            {
                pathRoad = Path.GetDirectoryName(path);
                ConfigHandle.Instance.SystemDefine.ProjectDirectory = path;
                RunProcess.LogicData.OpenProject(path);
                label_projectPath.Text = pathRoad.Substring(pathRoad.LastIndexOf('\\'));
                DataBinding();

                List_Change();


                VisionProject.Instance.LoadVisionTool(Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".Vision");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 新建文件
        /// </summary>
        public void btn_PrjFileNew()
        {
            RunProcess.LogicData.CreatProject();
            pathRoad = null;
            DataBinding();

            List_Change();

            label_projectPath.Text = pathRoad.Substring(pathRoad.LastIndexOf('\\'));
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        public void btn_PrjFileSave()
        {
            try
            {
                #region 保存数据

                SaveData();

                #endregion

                if (pathRoad != null)
                {
                    RunProcess.LogicData.SaveProject(pathRoad);
                    VisionProject.Instance.SaveVisionTool(Path.Combine(pathRoad, Path.GetFileNameWithoutExtension(pathRoad)) + ".Vision");
                    MessageBox.Show("保存成功！");
                    return;
                }
                SaveFileDialog Save = new SaveFileDialog();
                Save.Title = "项目存为";
                Save.InitialDirectory = "D:\\程式\\";
                if (Save.ShowDialog() == DialogResult.OK)
                {
                    pathRoad = Save.FileName;
                    string fileName = Path.GetFileName(pathRoad);
                    ConfigHandle.Instance.SystemDefine.ProjectDirectory = pathRoad + "\\" + fileName + ".pro"; ;
                    RunProcess.LogicData.SaveProject(pathRoad);
                    label_projectPath.Text = pathRoad.Substring(pathRoad.LastIndexOf('\\'));
                    VisionProject.Instance.SaveVisionTool(Path.Combine(pathRoad, Path.GetFileNameWithoutExtension(pathRoad)) + ".Vision");

                }

                MessageBox.Show("保存成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 文件另存为
        /// </summary>
        public void btn_PrjFileSaveAs()
        {
            SaveFileDialog SaveAs = new SaveFileDialog();
            SaveAs.Title = "项目另存为";
            SaveAs.InitialDirectory = "D:\\程式\\";
            if (SaveAs.ShowDialog() == DialogResult.OK)
            {
                RunProcess.LogicData.SaveProject(SaveAs.FileName);
                VisionProject.Instance.SaveVisionTool(Path.Combine(SaveAs.FileName, Path.GetFileNameWithoutExtension(SaveAs.FileName)) + ".Vision");
            }
        }

        #endregion

        #region 定时器

        private int cnt = 0;
        private bool[,] b_statusError = new bool[20, 32];
        private void timer1_Tick(object sender, EventArgs e)
        {
			int[] error = (int[])RunProcess.movedriverZm.ErrorCode.IntValue.Clone();
			int[] errorLevel = (int[])RunProcess.movedriverZm.ErrorLevel.IntValue.Clone();
            foreach (string str in ConfigHandle.Instance.AlarmDefine.ErrorInformation(error, errorLevel[0], b_statusError))
            {
                userCtrlMsgListView1.AddUserMsg(str, "报警");
            }            

            switch ((FsmStaDef)RunProcess.movedriverZm.DeviceStatus.IntValue[0])
			{
				case FsmStaDef.INIT:
                    lbl_RunStates.Text = "设备初始";
                    lbl_RunStates.BackColor = SystemColors.ActiveCaption;
                    panel10.BackColor = SystemColors.ActiveCaption;
                    RunProcess.LogicTask.AngingTest.step = RunProcess.LogicTask.AngingTest.done = RunProcess.LogicTask.AngingTest.execute = 0;
                    break;

				case FsmStaDef.PAUSE:
                    lbl_RunStates.Text = "设备暂停";
                    lbl_RunStates.BackColor = Color.Yellow;
                    panel10.BackColor = Color.Yellow;
                    RunProcess.LogicTask.AngingTest.step = RunProcess.LogicTask.AngingTest.done = RunProcess.LogicTask.AngingTest.execute = 0;
                    break;

				case FsmStaDef.RESET:
                    lbl_RunStates.Text = "设备复位";
                    lbl_RunStates.BackColor = Color.Red;
                    panel10.BackColor = Color.Red;
                    RunProcess.LogicTask.AngingTest.step = RunProcess.LogicTask.AngingTest.done = RunProcess.LogicTask.AngingTest.execute = 0;
                    break;

                case FsmStaDef.RUN:
                    lbl_RunStates.Text = "设备运行";
                    lbl_RunStates.BackColor = Color.Green;
                    panel10.BackColor = Color.Green;
                    break;

                case FsmStaDef.SCRAM:
                    lbl_RunStates.Text = "设备急停";
                    lbl_RunStates.BackColor = Color.Red;
                    panel10.BackColor = Color.Red;
                    RunProcess.LogicTask.AngingTest.step = RunProcess.LogicTask.AngingTest.done = RunProcess.LogicTask.AngingTest.execute = 0;
                    break;

                case FsmStaDef.STOP:
                    lbl_RunStates.Text = "设备停止";
                    lbl_RunStates.BackColor = Color.Yellow;
                    panel10.BackColor = Color.Yellow;
                    RunProcess.LogicTask.AngingTest.step = RunProcess.LogicTask.AngingTest.done = RunProcess.LogicTask.AngingTest.execute = 0;
                    break;
			}
                                          
            //控制器状态
            tsslbl_ControllerStatus.Text = RunProcess.movedriverZm.Succeed? "控制器：在线": "控制器：离线";
            tsslbl_ControllerStatus.BackColor = RunProcess.movedriverZm.Succeed ? SystemColors.Control: Color.Red;
            tsslbl_Camera1.Text = VisionProject.Instance.CameraConnected(0) ? "相机1:链接" : "相机1:断开";
            tsslbl_Camera1.BackColor = VisionProject.Instance.CameraConnected(0) ? SystemColors.ActiveCaption : Color.Red;
            tsslbl_Camera2.Text = VisionProject.Instance.CameraConnected(1) ? "相机2:链接" : "相机2:断开";
            tsslbl_Camera2.BackColor = VisionProject.Instance.CameraConnected(1) ? SystemColors.ActiveCaption : Color.Red;
            tsslbl_Camera3.Text = VisionProject.Instance.CameraConnected(2) ? "相机3:链接" : "相机3:断开";
            tsslbl_Camera3.BackColor = VisionProject.Instance.CameraConnected(2) ? SystemColors.ActiveCaption : Color.Red;
            toolStripStatusLabel1.Text = "版本号：" + FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[0].ToString() + "." +
            FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[1].ToString() + "." +
            FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[2].ToString() + "." + 
            FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[3].ToString() + "年" +
            FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[4].ToString() + "月" +
            FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[5].ToString() + "日" +
            "_" + FormMain.RunProcess.movedriverZm.SoftWare_Ver.IntValue[6].ToString();
            label3.Text  = "焊头1使用次数：" + RunProcess.LogicData.RunData.leftSoldertintimes.ToString();
            label4.Text  = "焊头2使用次数：" + RunProcess.LogicData.RunData.rightSoldertintimes.ToString();
            label60.Text = "打磨头使用次数：" + RunProcess.LogicData.RunData.polishtimes.ToString();

            #region 日志

            if (cnt > 4500)
            {
                cnt = 0;
                userCtrlMsgListView1.ClearMsgItems();
            }
            else
            {
                cnt++;
            }

            #endregion 日志

        }



        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunProcess.movedriverZm.WriteRegister(new BaseData(1130, new int[]{1}));
            ConfigHandle.Instance.AlarmDefine.ClearAlarmMessage(b_statusError);
			userCtrlMsgListView1.ClearMsgItems();
		}

        private void ShowMessage(SendCmdArgs e)
        {
            userCtrlMsgListView1.AddUserMsg(e.StrReciseve, "提示");
        }

        private void tsslbl_ControllerStatus_TextChanged(object sender, EventArgs e)
        {
            if (tsslbl_ControllerStatus.Text == "控制器：在线")
            {
                Motor.DownMotorPrmToSlave();
            }
        }
        
        #endregion

        #region 工单操作

        //public IList<ComboBoxIndex> list_PL = new List<ComboBoxIndex>();
        //public IList<ComboBoxIndex> list_PR = new List<ComboBoxIndex>();
        //public IList<ComboBoxIndex> list_SL = new List<ComboBoxIndex>();
        //public IList<ComboBoxIndex> list_SR = new List<ComboBoxIndex>();

        public void LoadData()
        {
            LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1);
            LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2);
            LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3);
            LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4);
        }

        public void SaveData()
        {

            SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1);
            SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2);
            SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3);
            SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4);
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data = sender as DataGridView;

            if(e.RowIndex >= 0)
            {
                switch (e.ColumnIndex)
                {
                    //        case 4:
                    //            if (MessageBox.Show("是否读取", "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
                    //            {
                    //                switch (Convert.ToInt32(data.Tag.ToString()))
                    //                {
                    //                    case 0:
                    //                        data.Rows[e.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    //                        data.Rows[e.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    //                        data.Rows[e.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1]>1?1:0;
                    //                        break;
                    //                    case 1:
                    //                        data.Rows[e.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    //                        data.Rows[e.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    //                        data.Rows[e.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    //                        break;
                    //                    case 2:
                    //                        data.Rows[e.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    //                        data.Rows[e.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    //                        data.Rows[e.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    //                        break;
                    //                    case 3:
                    //                        data.Rows[e.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    //                        data.Rows[e.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    //                        data.Rows[e.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    //                        break;
                    //                }
                    //            }
                    //            break;

                    //        case 5:
                    //            float x = Convert.ToSingle(data.Rows[e.RowIndex].Cells[1].Value);
                    //            float y = Convert.ToSingle(data.Rows[e.RowIndex].Cells[2].Value);
                    //            float t = Convert.ToSingle(data.Rows[e.RowIndex].Cells[3].Value);

                    //            switch(Convert.ToInt32(data.Tag.ToString()))
                    //            {
                    //                case 0:
                    //                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, (int)AxisDef.AxY1, (int)AxisDef.AxZ3, (int)AxisDef.AxR3, (int)AxisDef.AxT1, x, y, RunProcess.LogicData.slaverData.basics.Safe_Z, 0, t, 0)){};
                    //                    break;
                    //                case 1:
                    //                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, (int)AxisDef.AxY2, (int)AxisDef.AxZ3, (int)AxisDef.AxR3, (int)AxisDef.AxT2, x, y, RunProcess.LogicData.slaverData.basics.Safe_Z, 0, t, 0)){};
                    //                    break;
                    //                case 2:
                    //                   while (! RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX1, (int)AxisDef.AxY1, (int)AxisDef.AxZ1, (int)AxisDef.AxR3, (int)AxisDef.AxT1, x, y, RunProcess.LogicData.slaverData.basics.Safe_ZL, 0, t, 0)){};
                    //                    break;
                    //                case 3:
                    //                    while (!RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, (int)AxisDef.AxY2, (int)AxisDef.AxZ2, (int)AxisDef.AxR3, (int)AxisDef.AxT2, x, y, RunProcess.LogicData.slaverData.basics.Safe_ZR, 0, t, 0)){};
                    //                    break;
                    //            }
                    //            break;
                    case 5:
                        //if(Convert.ToBoolean(data.Rows[e.RowIndex].Cells[5].EditedFormattedValue) == false )
                        //{
                        //    data.Rows[e.RowIndex].Cells[5].Value = false;
                        //}
                        //else
                        //{
                        //    data.Rows[e.RowIndex].Cells[5].Value = true;
                        //}

                        if (Convert.ToString(data.Rows[e.RowIndex].Cells[5].Value) == "禁用")
                        {
                            data.Rows[e.RowIndex].Cells[5].Value = "启用";
                        }
                        else
                        {
                            data.Rows[e.RowIndex].Cells[5].Value = "禁用";
                        }



                        break;
                }
                data.Rows[e.RowIndex].Selected = true;
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data = sender as DataGridView;
            if (e.RowIndex == -1 && e.ColumnIndex == 5 && data.Rows.Count > 0)
            {
                if(Convert.ToString( data.Rows[0].Cells[5].Value) == "启用")
                {
                    for(int i = 0;i< data.Rows.Count;i++)
                    {
                        data.Rows[i].Cells[5].Value = "禁用";
                    }
                }
                else
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        data.Rows[i].Cells[5].Value = "启用";
                    }
                }
            }
        }

        #region 列表控制

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //判断相应的列
            if (dgv.CurrentCell.GetType().Name == "DataGridViewComboBoxCell" && dgv.CurrentCell.RowIndex != -1)
            {
                //给这个DataGridViewComboBoxCell加上下拉事件
                (e.Control as ComboBox).SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            }
        }

        /// <summary>
        /// 组合框事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            //这里比较重要
            combox.Leave += new EventHandler(combox_Leave);
            try
            {
                //在这里就可以做值是否改变判断
                if (combox.SelectedItem != null)
                {
                }
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
          /// <summary>
          /// 离开combox时，把事件删除
          /// </summary>
          /// <param name="sender"></param>
          /// <param name="e"></param>
        public void combox_Leave(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            //做完处理，须撤销动态事件
            combox.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
        }

        #endregion


        public void List_Change()
        {
            LoadData();
        }

        public void LoadDGVData(List<wPointF> teachList,DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            for (int i = 0; i < teachList.Count; i++)
            {
                #region 方法1

                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell[] textboxcell = new DataGridViewTextBoxCell[7];
                for (int j = 0; j < 7; j++)
                {
                    textboxcell[j] = new DataGridViewTextBoxCell();
                }

                //DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                //comboBoxCell.DataSource = null;
                //comboBoxCell.DataSource = list;
                //comboBoxCell.ValueMember = "index";
                //comboBoxCell.DisplayMember = "name";
                
                DataGridViewCheckBoxCell checkBoxcell = new DataGridViewCheckBoxCell();

                textboxcell[0].Value = i + 1;
                row.Cells.Add(textboxcell[0]);
                textboxcell[1].Value = teachList[i].Clone().X.ToString("f3");
                row.Cells.Add(textboxcell[1]);
                textboxcell[2].Value = teachList[i].Clone().Y.ToString("f3");
                row.Cells.Add(textboxcell[2]);
                textboxcell[3].Value = teachList[i].Clone().T.ToString("f3");
                row.Cells.Add(textboxcell[3]);

                int aa = teachList[i].Clone().templateIndex;

                //if (list.Count - 1 > teachList[i].Clone().templateIndex)
                //    comboBoxCell.Value = teachList[i].Clone().templateIndex;
                //else
                //    comboBoxCell.Value = -1;
                //row.Cells.Add(comboBoxCell);

                textboxcell[4].Value = teachList[i].Clone().templateIndex.ToString();
                row.Cells.Add(textboxcell[4]);
                textboxcell[5].Value = teachList[i].Clone().enable ?"禁用" :"启用" ;
                row.Cells.Add(textboxcell[5]);
                //checkBoxcell.Value = teachList[i].Clone().enable;
                //row.Cells.Add(checkBoxcell);

                dataGridView.Rows.Add(row);

                if (i % 2 == 0)
                {
                    dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                }
                else
                {
                    dataGridView.Rows[dataGridView.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                }
                
                #endregion
            }
            int index = 0;
            dataGridView.Columns[index].FillWeight = 5;
            index++;
            dataGridView.Columns[index].FillWeight = 8;
            index++;
            dataGridView.Columns[index].FillWeight = 8;
            index++;
            dataGridView.Columns[index].FillWeight = 8;
            index++;
            dataGridView.Columns[index].FillWeight = 12;
            index++;
            dataGridView.Columns[index].FillWeight = 8;

            int rownum = dataGridView.Rows.Count;

            if (rownum != 0)
            {
                dataGridView.CurrentCell = dataGridView.Rows[rownum - 1].Cells[0];
            }
            //ArrayPointDraw();
        }

        private void SaveDataGridView(List<wPointF> teachList, DataGridView dataGridView)
        {
            try
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    teachList[i].X = Convert.ToSingle(dataGridView.Rows[i].Cells[1].Value);
                    teachList[i].Y = Convert.ToSingle(dataGridView.Rows[i].Cells[2].Value);
                    teachList[i].T = Convert.ToSingle(dataGridView.Rows[i].Cells[3].Value);
                    teachList[i].templateIndex = Convert.ToInt32(dataGridView.Rows[i].Cells[4].Value);
                    teachList[i].enable = dataGridView.Rows[i].Cells[5].Value == "启用" ? false :true ;
                }
                //ArrayPointDraw();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        
        private void button_add_Click(object sender, EventArgs e)
        {
            wPointF wPointF = new wPointF();
            ToolStripButton button = sender as ToolStripButton;

            switch (Convert.ToInt32(button.Tag.ToString()))
            {
                case 0:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Polish[0].Add(wPointF);

                    List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1, list_PL);
                    break;
                case 1:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Polish[1].Add(wPointF);
                    List_Change(); // LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2, list_PR);
                    break;
                case 2:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Solder[0].Add(wPointF);
                    List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3, list_SL);
                    break;
                case 3:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Solder[1].Add(wPointF);
                    List_Change(); // LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4,list_SR);
                    break;
            }

        }

        private void button_insert_Click(object sender, EventArgs e)
        {
            wPointF wPointF = new wPointF();
            ToolStripButton button = sender as ToolStripButton;
            switch (Convert.ToInt32(button.Tag.ToString()))
            {
                case 0:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Polish[0].Insert(dataGridView1.CurrentRow.Index, wPointF);
                    List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1,list_PL);
                    break;
                case 1:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Polish[1].Insert(dataGridView2.CurrentRow.Index, wPointF);
                    List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2, list_PR);
                    break;
                case 2:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Solder[0].Insert(dataGridView3.CurrentRow.Index, wPointF);
                    List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3, list_SL);
                    break;
                case 3:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4);
                    wPointF.X = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    wPointF.Y = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    wPointF.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Solder[1].Insert(dataGridView4.CurrentRow.Index, wPointF);
                    List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4, list_SR);
                    break;
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            int rowSele = 0; 
            switch (Convert.ToInt32(button.Tag.ToString()))
            {
                case 0:
                    if(dataGridView1.RowCount>0)
                    {
                        SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1);
                        dataGridView1.AllowUserToDeleteRows = true;

                        for (int i = RunProcess.LogicData.RunData.wPointFs_Polish[0].Count -1;i> -1;i--)
                        {
                            if(dataGridView1.Rows[i].Cells[0].Selected == true)
                            {
                                RunProcess.LogicData.RunData.wPointFs_Polish[0].RemoveAt(i);
                            }
                            rowSele = i;
                        }

                        List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1, list_PL);
                        dataGridView1.AllowUserToDeleteRows = false;

                    }
                    break;
                case 1:
                    if (dataGridView2.RowCount > 0)
                    {
                        SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2);
                        dataGridView2.AllowUserToDeleteRows = true;

                        for (int i = RunProcess.LogicData.RunData.wPointFs_Polish[1].Count - 1; i > -1; i--)
                        {
                            if (dataGridView2.Rows[i].Cells[0].Selected == true)
                            {
                                RunProcess.LogicData.RunData.wPointFs_Polish[1].RemoveAt(i);
                            }
                            rowSele = i;
                        }

                        List_Change(); // LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2, list_PR);
                        dataGridView2.AllowUserToDeleteRows = false;

                    }
                    break;
                case 2:
                    if (dataGridView3.RowCount > 0)
                    {
                        SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3);
                        dataGridView3.AllowUserToDeleteRows = true;

                        for (int i = RunProcess.LogicData.RunData.wPointFs_Solder[0].Count - 1; i > -1; i--)
                        {
                            if (dataGridView3.Rows[i].Cells[0].Selected == true)
                            {
                                RunProcess.LogicData.RunData.wPointFs_Solder[0].RemoveAt(i);
                            }
                            rowSele = i;
                        }

                        List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3, list_SL);
                        dataGridView3.AllowUserToDeleteRows = false;

                    }
                    break;
                case 3:
                    if (dataGridView4.RowCount > 0)
                    {
                        SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4);
                        dataGridView4.AllowUserToDeleteRows = true;

                        for (int i = RunProcess.LogicData.RunData.wPointFs_Solder[1].Count - 1; i > -1; i--)
                        {
                            if (dataGridView4.Rows[i].Cells[0].Selected == true)
                            {
                                RunProcess.LogicData.RunData.wPointFs_Solder[1].RemoveAt(i);
                            }
                            rowSele = i;
                        }

                        List_Change(); //LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4, list_SR);
                        dataGridView4.AllowUserToDeleteRows = false;

                    }
                    break;
            }
        }

        Frm_ThreePArray frm = null;
        private void button_Array_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;

            if (frm != null)
            {
                if (frm.IsDisposed)
                {
                    frm = new Frm_ThreePArray(Convert.ToInt32(button.Tag.ToString()));
                    frm.Owner = this;
                    frm.TopLevel = true;
                    frm.Show();
                    return;
                }
                else
                {
                    frm.Owner = this;
                    frm.TopLevel = true;
                    frm.Show();
                    return;
                }
            }
            frm = new Frm_ThreePArray(Convert.ToInt32(button.Tag.ToString()));
            frm.Owner = this;
            frm.TopLevel = true;
            frm.Show();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            switch (Convert.ToInt32(button.Tag.ToString()))
            {
                case 0:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1);
                    break;
                case 1:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2);
                    break;
                case 2:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3);
                    break;
                case 3:
                    SaveDataGridView(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4);
                    break;
            }
        }

        Form_SolderSet form_Solder = new Form_SolderSet();
        private void button_cancel_Click(object sender, EventArgs e)
        {
           
            ToolStripButton button = sender as ToolStripButton;
            SaveData();

            if (form1 != null&&!form1.IsDisposed)
            {
                form1.Close();
            }

            if (form_Solder != null)
            {
                if (form_Solder.IsDisposed)
                {
                    form_Solder = new Form_SolderSet(Convert.ToInt32(button.Tag.ToString()));
                    form_Solder.Owner = this;
                    form_Solder.TopLevel = true;
                    form_Solder.Show();
                    return;
                }
                else
                {
                    form_Solder.Owner = this;
                    form_Solder.TopLevel = true;
                    form_Solder.Show();
                    return;
                }
            }
            form_Solder = new Form_SolderSet(Convert.ToInt32(button.Tag.ToString()));
            form_Solder.Owner = this;
            form_Solder.TopLevel = true;
            form_Solder.Show();
        }
        private void button_Set_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            SaveData();

            if (form_Solder != null && !form_Solder.IsDisposed)
            {
                form_Solder.Close();
            }

            if (form1 != null)
            {
                if (form1.IsDisposed)
                {
                    form1 = new Form1_Layout(Convert.ToInt32(button.Tag.ToString()));
                    form1.Owner = this;
                    form1.TopLevel = true;
                    form1.Show();
                    return;
                }
                else
                {
                    form1.Owner = this;
                    form1.TopLevel = true;
                    form1.Show();
                    return;
                }
            }
            form1 = new Form1_Layout(Convert.ToInt32(button.Tag.ToString()));
            form1.Owner = this;
            form1.TopLevel = true;
            form1.Show();
        }

        /// <summary>
        /// 阵列
        /// </summary>
        /// <param name="listPointF3"></param>
        /// <param name="id"></param>
        public void LoadStickData(List<wPointF> listPointF3,int id)
        {
            for (int i = 0; i < listPointF3.Count; i++)
            {
                wPointF data = new wPointF();
                data.X = listPointF3[i].X;
                data.Y = listPointF3[i].Y;

                if(id == 0)
                {
                    data.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Polish[0].Add(data);
                }
                else if(id == 1)
                {
                    data.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Polish[1].Add(data);

                }
                else if (id == 2)
                {
                    data.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Solder[0].Add(data);

                }
                else if (id == 3)
                {
                    data.T = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                    RunProcess.LogicData.RunData.wPointFs_Solder[1].Add(data);

                }
            }

            List_Change(); //
            //switch (id)
            //{
            //    case 0:
            //        LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[0], dataGridView1, list_PL);
            //        break;
            //    case 1:
            //        LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Polish[1], dataGridView2, list_PR);
            //        break;
            //    case 2:
            //        LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[0], dataGridView3, list_SL);
            //        break;
            //    case 3:
            //        LoadDGVData(RunProcess.LogicData.RunData.wPointFs_Solder[1], dataGridView4, list_SR);
            //        break;
            //}
        }

        /// <summary>
        /// 修改点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton48_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;

            if (MessageBox.Show("是否读取", "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                switch (Convert.ToInt32(button.Tag.ToString()))
                {
                    case 0:
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                        break;
                    case 1:
                        dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                        dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                        dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                        break;
                    case 2:
                        dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                        dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                        dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1] > 1 ? 1 : 0;
                        break;
                    case 3:
                        dataGridView4.Rows[dataGridView4.CurrentCell.RowIndex].Cells[1].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                        dataGridView4.Rows[dataGridView4.CurrentCell.RowIndex].Cells[2].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                        dataGridView4.Rows[dataGridView4.CurrentCell.RowIndex].Cells[3].Value = RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2] > 1 ? 1 : 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 定位点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton47_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            float x = 0f;
            float y = 0;
            float t = 0;//z高度
            switch (Convert.ToInt32(button.Tag.ToString()))
            {
                case 0:
                    x = Convert.ToSingle(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value);
                    y = Convert.ToSingle(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value);
                    t = Convert.ToSingle(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value);
                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, (int)AxisDef.AxY1, (int)AxisDef.AxZ3, (int)AxisDef.AxR3, (int)AxisDef.AxT1, x, y, RunProcess.LogicData.slaverData.basics.Safe_Z, 0, t, 0)) { };
                    break;
                case 1:
                    x = Convert.ToSingle(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[1].Value);
                    y = Convert.ToSingle(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[2].Value);
                    t = Convert.ToSingle(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value);
                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, (int)AxisDef.AxY2, (int)AxisDef.AxZ3, (int)AxisDef.AxR3, (int)AxisDef.AxT2, x, y, RunProcess.LogicData.slaverData.basics.Safe_Z, 0, t, 0)) { };
                    break;
                case 2:
                    x = Convert.ToSingle(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[1].Value);
                    y = Convert.ToSingle(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[2].Value);
                    t = Convert.ToSingle(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[3].Value);
                    while (!RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX1, (int)AxisDef.AxY1, (int)AxisDef.AxZ1, (int)AxisDef.AxR3, (int)AxisDef.AxT1, x, y, RunProcess.LogicData.slaverData.basics.Safe_ZL, 0, t, 0)) { };
                    break;
                case 3:
                    x = Convert.ToSingle(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[1].Value);
                    y = Convert.ToSingle(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[2].Value);
                    t = Convert.ToSingle(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[3].Value);
                    while (!RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, (int)AxisDef.AxY2, (int)AxisDef.AxZ2, (int)AxisDef.AxR3, (int)AxisDef.AxT2, x, y, RunProcess.LogicData.slaverData.basics.Safe_ZR, 0, t, 0)) { };
                    break;
            }
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data = sender as DataGridView;
            if (e.ColumnIndex == 4 && data.Rows.Count > 0)
            {
                switch (Convert.ToInt32(data.Tag.ToString ()))
                {
                    case 0:
                        if(Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) < -1 ||
                            Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) >= VisionProject.Instance.VisionTools.PolishLeft.Count)
                        {
                            data.Rows[e.RowIndex].Cells[4].Value = -1;
                        }
                        else
                        {
                            object value = data.Rows[e.RowIndex].Cells[4].Value;
                            for (int i = 0; i < data.Rows.Count;i++ )
                            {
                                if (data.Rows[i].Cells[4].Selected == true)
                                {
                                    data.Rows[i].Cells[4].Value = value;
                                }
                            }
                        }
                        break;
                    case 1:
                        if (Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) < -1 ||
                            Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) >= VisionProject.Instance.VisionTools.PolishRight.Count)
                        {
                            data.Rows[e.RowIndex].Cells[4].Value = -1;
                        }
                        else
                        {
                            object value = data.Rows[e.RowIndex].Cells[4].Value;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                if (data.Rows[i].Cells[4].Selected == true)
                                {
                                    data.Rows[i].Cells[4].Value = value;
                                }
                            }
                        }
                        break;
                    case 2:
                        if (Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) < -1 ||
                            Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) >= VisionProject.Instance.VisionTools.SolderLeft.Count)
                        {
                            data.Rows[e.RowIndex].Cells[4].Value = -1;
                        }
                        else
                        {
                            object value = data.Rows[e.RowIndex].Cells[4].Value;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                if (data.Rows[i].Cells[4].Selected == true)
                                {
                                    data.Rows[i].Cells[4].Value = value;
                                }
                            }
                        }
                        break;
                    case 3:
                        if (Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) < -1 ||
                         Convert.ToInt32(data.Rows[e.RowIndex].Cells[4].Value) >= VisionProject.Instance.VisionTools.SolderRight.Count)
                        {
                            data.Rows[e.RowIndex].Cells[4].Value = -1;
                        }
                        else
                        {
                            object value = data.Rows[e.RowIndex].Cells[4].Value;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                if (data.Rows[i].Cells[4].Selected == true)
                                {
                                    data.Rows[i].Cells[4].Value = value;
                                }
                            }
                        }
                        break;
                }
            }
        }

        #endregion

        #region 相机标定
        private void button23_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(0);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(1);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(2);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(3);
        }

        #endregion

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tool = (ToolStripMenuItem)sender;
            
            tabControl1.SelectedIndex = 0;
            tabControl2.SelectedIndex = Convert.ToInt32(tool.Tag.ToString());
            
             

        }

        #region 读写
        private void button_read_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch(Convert.ToInt32 (button.Tag.ToString ()))
            {
                case 0:
                    numericUpDown1.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    numericUpDown2.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    numericUpDown4.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ1];
                    numericUpDown3.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR1];
                    numericUpDown5.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1];
                    break;
                case 1:
                    numericUpDown10.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    numericUpDown9.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    numericUpDown8.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ2];
                    numericUpDown7.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR2];
                    numericUpDown6.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2];
                    break;
                case 2:
                    numericUpDown20.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    numericUpDown19.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    numericUpDown18.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ1];
                    numericUpDown17.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR1];
                    numericUpDown16.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1];
                    break;
                case 3:
                    numericUpDown15.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    numericUpDown14.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    numericUpDown13.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ2];
                    numericUpDown12.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR2];
                    numericUpDown11.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2];
                    break;
                case 4:
                    numericUpDown30.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    numericUpDown28.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ3];
                    numericUpDown27.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR3];
                    break;
                case 5:
                    numericUpDown25.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    numericUpDown24.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    numericUpDown23.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ1];
                    numericUpDown22.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR1];
                    //numericUpDown21.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT1];
                    break;
                case 6:
                    numericUpDown33.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    numericUpDown32.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    numericUpDown31.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ2];
                    numericUpDown29.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR2];
                   // numericUpDown26.Value = (decimal)RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxT2];
                    break;
            }
        }

        private void button_Locition_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (Convert.ToInt32(button.Tag.ToString()))
            {
                case 0:
                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX1, (int)AxisDef.AxY1, (int)AxisDef.AxZ1, (int)AxisDef.AxR1, (int)AxisDef.AxT1,(float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value, (float)numericUpDown4.Value, (float)numericUpDown5.Value, 0)) { }
                    break;
                case 1:
                    while (!RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, (int)AxisDef.AxY2, (int)AxisDef.AxZ2, (int)AxisDef.AxR2, (int)AxisDef.AxT2, (float)numericUpDown10.Value, (float)numericUpDown9.Value, (float)numericUpDown8.Value, (float)numericUpDown7.Value, (float)numericUpDown6.Value, 0)) { }
                    break;
                case 2:
                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX1, (int)AxisDef.AxY1, (int)AxisDef.AxZ1, (int)AxisDef.AxR1, (int)AxisDef.AxT1, (float)numericUpDown20.Value, (float)numericUpDown19.Value, (float)numericUpDown18.Value, (float)numericUpDown17.Value, (float)numericUpDown16.Value, 0)) { }
                    break;
                case 3:
                    while (!RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, (int)AxisDef.AxY2, (int)AxisDef.AxZ2, (int)AxisDef.AxR2, (int)AxisDef.AxT2, (float)numericUpDown15.Value, (float)numericUpDown14.Value, (float)numericUpDown13.Value, (float)numericUpDown12.Value, (float)numericUpDown11.Value, 0)) { }
                    break;
                case 4:
                    while(!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, (int)AxisDef.AxY1, (int)AxisDef.AxZ3, (int)AxisDef.AxR3, (int)AxisDef.AxT1, (float)numericUpDown30.Value, LogicMain.StopMove, (float)numericUpDown28.Value, (float)numericUpDown27.Value,2,0)) { }
                    break;
                case 5:
                    while (!RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX1, (int)AxisDef.AxY1, (int)AxisDef.AxZ1, (int)AxisDef.AxR1, (int)AxisDef.AxT1, (float)numericUpDown25.Value, (float)numericUpDown24.Value, (float)numericUpDown23.Value, (float)numericUpDown22.Value, LogicMain.StopMove, 0)) { }
                    break;
                case 6:
                    while (!RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, (int)AxisDef.AxY2, (int)AxisDef.AxZ2, (int)AxisDef.AxR2, (int)AxisDef.AxT2, (float)numericUpDown33.Value, (float)numericUpDown32.Value, (float)numericUpDown31.Value, (float)numericUpDown29.Value, LogicMain.StopMove, 0)) { }
                    break;
            }
        }

        #endregion

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = tabControl2.SelectedIndex;
            switch (index)
            {
                case 1:
                    jog.TagBinding(2);
                    tabControl3.SelectedIndex = 0;
                    break;
                case 2:
                    jog.TagBinding(3);
                    tabControl3.SelectedIndex = 0;
                    break;
                case 3:
                    jog.TagBinding(0);
                    tabControl3.SelectedIndex = 1;
                    break;
                case 4:
                    jog.TagBinding(1);
                    tabControl3.SelectedIndex = 2;
                    break;
                case 5:
                    jog.TagBinding(tabControl5.SelectedIndex);
                    break;
                default:
                    jog.TagBinding(0);
                    break;
            }
            if (form1 != null && !form1.IsDisposed)
            {
                form1.Close();
            }
            if (form_Solder != null && !form_Solder.IsDisposed)
            {
                form_Solder.Close();
            }

        }

        private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
        {
            jog.TagBinding(tabControl5.SelectedIndex);
        }



        #region 关闭窗口

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string a,string b);

        [DllImport("user32.dll")]
        private static extern IntPtr PostMessage(IntPtr hWnd,int msg,IntPtr wp,IntPtr lp);

        public void CloseWindow(string name)
        {
            IntPtr ptrl = FindWindow(null, name);
            if(ptrl != IntPtr.Zero)
            {
                PostMessage(ptrl, 0x1110, IntPtr.Zero, IntPtr.Zero);
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)//打磨头数据清零
        {
            FormMain.RunProcess.LogicData.RunData.polishtimes = 0;

            FormMain.RunProcess.movedriverZm.WriteRegister(new BaseData(4084, new int[]{ 0,0 }));
        }

        #region UPH

        public static ObservableCollection<uphDef> uph_list = new ObservableCollection<uphDef>();

        private void list_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            float uph = 0;
            
            RunPoint(uph_list[uph_list.Count -1].time.ToString("f2"), uph_list[uph_list.Count - 1].ID);

            if (uph_list.Count > 10)
            {
                uph_list.RemoveAt(0);
                return;
            }

            foreach (var vs in uph_list)
            {
                uph += vs.time;
            }

            int count = uph_list.Count;

            int n = RunProcess.LogicData.RunData.wPointFs_Solder[0].Count;
            RunProcess.LogicData.RunData.UPH = (int)(3600 / (uph / count) * n);

            label7.Text = "UPH:" + RunProcess.LogicData.RunData.UPH.ToString();
        }

        public void RunPoint(string str,int index)
        {
            this.Invoke((Action)(() =>
            {
                if(index == 0)
                {
                    label8.Text = "左平台时间：" + str;
                }
                else
                {
                    label9.Text = "右平台时间：" + str;
                }
            }));
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int indexer = Convert.ToInt32(button.Tag.ToString());
            FormMain.RunProcess.LogicAPI.rinse[indexer].exe();
            FormMain.RunProcess.LogicAPI.rinse[indexer].Initialize();
            button2.Enabled = true;//始终让button处于使能状态
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormMain.RunProcess.LogicData.RunData.leftSoldertintimes = 0;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormMain.RunProcess.LogicData.RunData.rightSoldertintimes = 0;
        }
        
    }

    public class uphDef
    {
        public float time { set; get; }
        public int ID { get; set; }

        public uphDef(float t,int i)
        {
            time = t;
            ID = i;
        }
    }
}
