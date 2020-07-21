using ProVision.InteractiveROI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using HZZH.Vision.Logic;
using Motion;
using UI;
using Common;
using System.Threading;
using Vision.Tool.Calibrate;

namespace UI
{

    public partial class Form_SolderSet : Form
    {
        public FormMain ftemp;
        public int _id = 0;

        public const int LEFT_SOLDER = 0;
        public const int RIGHT_SOLSER = 1;
        public const int LEFT_POLISH = 2;
        public const int RIGHT_POLISH = 3;

        public Form_SolderSet()
        {
            InitializeComponent();
        }
        public Form_SolderSet(int id)
        {
            InitializeComponent();
            _id = id;
        }

        private void Log(string log)
        {
#if DEBUG
            Tools.WriteLog.AddLog(string.Format("[{0}]  \t{1}", this.GetType().Name, log));
#endif
        }


        private void Form_SolderSet_Load(object sender, EventArgs e)
        {
            ftemp = (FormMain)this.Owner;
            hWndCtrller = new HWndCtrllerEx(this.hWindowControl0) { UseThreadEnable = true };
            hWindowControl0.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };
            hWindowControl0.HMouseDown += HWindowControl0_HMouseDown;
            hWindowControl0.HMouseMove += HWindowControl0_HMouseMove;
            hWindowControl0.HMouseUp += HWindowControl0_HMouseUp;
            HOperatorSet.SetFont(hWindowControl0.HalconWindow, "-Arial-40-*-1-*-*-1-ANSI_CHARSET-");

            this.treeView1.HideSelection = false;
            this.treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.treeView1.DrawNode += new DrawTreeNodeEventHandler(treeView1_DrawNode);

            ((HWndCtrllerEx)hWndCtrller).Paint += Form_SolderSet_Paint;

            switch (_id)
            {
                case LEFT_SOLDER:
                    //textBox1.Text = FormMain.RunProcess.LogicData.RunData.sNumL.ToString();
                    numericUpDown1.Visible = label1.Visible = false;
                    Functions.SetBinding(numericUpDown1, "Value", FormMain.RunProcess.LogicData.RunData, "sNumL");

                    locationShapes = VisionProject.Instance.VisionTools.SolderLeft;
                    _SolderPos = FormMain.RunProcess.LogicData.vData.vSolderDatasL;
                    break;
                case RIGHT_SOLSER:
                    //textBox1.Text = FormMain.RunProcess.LogicData.RunData.sNumR.ToString();
                    numericUpDown1.Visible = label1.Visible = false;
                    Functions.SetBinding(numericUpDown1, "Value", FormMain.RunProcess.LogicData.RunData, "sNumR");

                    locationShapes = VisionProject.Instance.VisionTools.SolderRight;
                    _SolderPos = FormMain.RunProcess.LogicData.vData.vSolderDatasR;
                    break;
                case LEFT_POLISH:
                    //textBox1.Text = FormMain.RunProcess.LogicData.RunData.pNumL.ToString();
                    numericUpDown1.Visible = label1.Visible = true;
                    Functions.SetBinding(numericUpDown1, "Value", FormMain.RunProcess.LogicData.RunData, "pNumL");

                    locationShapes = VisionProject.Instance.VisionTools.PolishLeft;
                    _polishPos = FormMain.RunProcess.LogicData.vData.vPolishDatasL;
                    break;
                case RIGHT_POLISH:
                    //textBox1.Text = FormMain.RunProcess.LogicData.RunData.pNumR.ToString();
                    numericUpDown1.Visible = label1.Visible = true;
                    Functions.SetBinding(numericUpDown1, "Value", FormMain.RunProcess.LogicData.RunData, "pNumR");

                    locationShapes = VisionProject.Instance.VisionTools.PolishRight;
                    _polishPos = FormMain.RunProcess.LogicData.vData.vPolishDatasR;
                    break;
            }


            DispalyModelCombox();


            button5.Enabled = button9.Enabled = button8.Enabled = true;// false;
            if (comboBox1.Items.Count == 0)
            {
                button2_Click(null, EventArgs.Empty);
            }


        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
            return;
        }


        private void propertyGridShow(int index)
        {
            if (_id == LEFT_POLISH || _id == RIGHT_POLISH)
            {
                LoadtreeView1(_polishPos[index].pos);
            }
            else
            {
                LoadtreeView_S(_SolderPos[index].pos);
            }
        }

        private void DispalyModelCombox()
        {
            if (locationShapes != null)
            {
                comboBox1.Items.Clear();
                comboBox1.Text = "";
                for (int i = 0; i < locationShapes.Count; i++)
                {
                    comboBox1.Items.Add("模板" + i);
                }

                if (comboBox1.Items.Count > 0)
                {

                    comboBox1.SelectedIndex = 0;
                    // propertyGridShow(0);

                }
            }
        }




        private HWndCtrller hWndCtrller = null;
        // 挂载用于在图像中显示十字叉的
        private LocationCrossCollection crossCollection = new LocationCrossCollection();
        private Cross operaCross = null;
        List<LocationShapePoint> locationShapes = null;
        private HImage hImage = null;


        List<PolishPos> _polishPos = null;
        List<SolderPos> _SolderPos = null;


        private void Form_SolderSet_Paint(object sender, EventArgs e)
        {
            crossCollection.Draw(hWindowControl0.HalconWindow);
            if (locationShapes != null)
            {
                int index = comboBox1.SelectedIndex;
                if (index >= 0)
                {
                    locationShapes[index].TransLocationCross.Draw(hWindowControl0.HalconWindow);
                }
            }

            if (operaCross != null)
            {
                hWindowControl0.HalconWindow.SetColor("green");
                hWindowControl0.HalconWindow.SetLineWidth(2);
                hWindowControl0.HalconWindow.DispCross((double)operaCross.Pixel.Y, (double)operaCross.Pixel.X, 90, 0);
            }
        }

        private void HWindowControl0_HMouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (operaCross != null)
            {
                operaCross.Pixel = new PointF((float)e.X, (float)e.Y);
                crossCollection.Add(operaCross);
                operaCross = null;
                crossCollection.SetCrossActive(e.X, e.Y);

                Repaint();
            }
            _mouseDown = false;

        }

        private void HWindowControl0_HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (operaCross != null)
            {
                operaCross.Pixel = new PointF((float)e.X, (float)e.Y);
                Repaint();
            }

            if (_mouseDown == true && e.Button == MouseButtons.Left && crossCollection.ActiveCross != null)
            {
                crossCollection.MoveActiveCross(e.X, e.Y);
                Repaint();
            }
        }

        bool _mouseDown = false;
        private void HWindowControl0_HMouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && crossCollection != null)
            {
                crossCollection.SetCrossActive(e.X, e.Y);
                Repaint();
                _mouseDown = true;
            }

        }

        private void Repaint()
        {
            hWndCtrller.Repaint();
        }

        Point point = new Point();

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;

            if (index >= 0)
            {
                if (_id == LEFT_POLISH || _id == RIGHT_POLISH)
                {
                    PolishPosdata f4 = new PolishPosdata();
                    if (_polishPos[index].Vpos.Y == 0 || _polishPos[index].Vpos.X == 0)
                    {
                        MessageBox.Show("先设置模板基准点");
                        return;
                    }
                    if (_id == LEFT_POLISH)
                    {
                        f4.pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1] - _polishPos[index].Vpos.Y;
                    }
                    else
                    {
                        f4.pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2] - _polishPos[index].Vpos.Y;
                    }
                    f4.pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3] - _polishPos[index].Vpos.X;
                    f4.pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ3];
                    f4.pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR3];

                    _polishPos[index].pos.Add(f4);
                }
                else
                {
                    SolderPosdata f4 = new SolderPosdata();
                    if (_SolderPos[index].Vpos.Y == 0 || _SolderPos[index].Vpos.X == 0)
                    {
                        MessageBox.Show("先设置模板基准点");
                        return;
                    }
                    if (_id == LEFT_SOLDER)
                    {
                        f4.pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1] - _SolderPos[index].Vpos.X;
                        f4.pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1] - _SolderPos[index].Vpos.Y;
                        f4.pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ1];
                        f4.pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR1];
                    }
                    else
                    {
                        f4.pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2] - _SolderPos[index].Vpos.X;
                        f4.pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2] - _SolderPos[index].Vpos.Y;
                        f4.pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ2];
                        f4.pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR2];
                    }

                    _SolderPos[index].pos.Add(f4);
                }


                propertyGridShow(index);

                if (this.treeView1.Nodes.Count > 0)
                {
                    int i = this.treeView1.Nodes.Count;
                    this.treeView1.SelectedNode = treeView1.Nodes[i - 1];
                }
            }

            ///add

            //    operaCross = new Cross();
            //crossCollection.SetCrossActive(99999, 99999);
            //Log("在模板中添加定位点");
        }


        /// <summary>
        /// 当前操作的模板编号
        /// </summary>
        public int OperShapeIndex
        {
            get
            {
                return comboBox1.SelectedIndex;
            }
        }

        private void AddPixelCross(float x, float y)
        {
            if (locationShapes != null && Calib != null)
            {
                PointF pixel;
                PointF imageCenter = new PointF();
                imageCenter.X = locationShapes[OperShapeIndex].ImageSize.Width / 2f;
                imageCenter.Y = locationShapes[OperShapeIndex].ImageSize.Height / 2f;

                Calib.WorldPointToPixelPoint(new PointF(x, y), out pixel, imageCenter, new PointF());

                crossCollection.Add(operaCross);
                crossCollection.SetCrossActive(pixel.X, pixel.Y);

                Repaint();
            }
        }




        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
            {
                int index = comboBox1.SelectedIndex;
                if (index >= 0 && RowCount > -1 && this.treeView1.Nodes.Count > 0)
                {
                    if (_id == LEFT_POLISH || _id == RIGHT_POLISH)
                    {
                        _polishPos[index].pos.RemoveAt(RowCount);
                    }
                    else
                    {

                        _SolderPos[index].pos.RemoveAt(RowCount);
                    }
                    propertyGridShow(index);

                    //if (this.treeView1.Nodes.Count > 0)
                    //{
                    //    int i = this.treeView1.Nodes.Count;
                    //    this.treeView1.SelectedNode = treeView1.Nodes[0];
                    //}
                }
            }
            else
            {
                MessageBox.Show("请先选择端点");
            }


            ///remove


            //operaCross = null;
            //crossCollection.RemoveActive();
            //Repaint();
            //Log("在模板中移除选中点位点");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            button5.Enabled = button9.Enabled = false;
            locationShapes.Add(new LocationShapePoint());

            if (_id == LEFT_POLISH || _id == RIGHT_POLISH)
            {
                _polishPos.Add(new PolishPos());
                //加模板中心
            }
            else
            {
                _SolderPos.Add(new SolderPos());
                //加模板中心
            }

            DispalyModelCombox();


            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            Log("添加模板");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index >= 0)
            {
                locationShapes.RemoveAt(index);
                switch (_id)
                {
                    case LEFT_POLISH:
                        _polishPos.RemoveAt(index);
                        for (int j = 0; j < FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[0].Count; j++)
                        {
                            if (FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[0][j].templateIndex == index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[0][j].templateIndex = -1;
                            }
                            else if (FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[0][j].templateIndex > index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[0][j].templateIndex--;
                            }
                        }
                        break;
                    case RIGHT_POLISH:
                        _polishPos.RemoveAt(index);
                        for (int j = 0; j < FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[1].Count; j++)
                        {
                            if (FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[1][j].templateIndex == index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[1][j].templateIndex = -1;
                            }
                            else if (FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[1][j].templateIndex > index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Polish[1][j].templateIndex--;
                            }
                        }
                        break;
                    case LEFT_SOLDER:
                        _SolderPos.RemoveAt(index);
                        for (int j = 0; j < FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[0].Count; j++)
                        {
                            if (FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[0][j].templateIndex == index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[0][j].templateIndex = -1;
                            }
                            else if (FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[0][j].templateIndex > index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[0][j].templateIndex--;
                            }
                        }
                        break;
                    case RIGHT_SOLSER:
                        _SolderPos.RemoveAt(index);
                        for (int j = 0; j < FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[1].Count; j++)
                        {
                            if (FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[1][j].templateIndex == index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[1][j].templateIndex = -1;
                            }
                            else if (FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[1][j].templateIndex > index)
                            {
                                FormMain.RunProcess.LogicData.RunData.wPointFs_Solder[1][j].templateIndex--;
                            }
                        }
                        break;
                }

                DispalyModelCombox();
                Log("删除模板");
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (locationShapes != null)
            {
                int index = comboBox1.SelectedIndex;
                if (index >= 0)
                {
                    crossCollection = locationShapes[index].locationCross;

                    if (locationShapes[index].Shape.ModelImg != null && locationShapes[index].Shape.ModelImg.IsInitialized())
                    {
                        hImage = locationShapes[index].Shape.ModelImg.Clone();
                        hWndCtrller.AddIconicVar(hImage);
                    }
                    else
                    {
                        hWndCtrller.ClearEntries();
                    }


                    propertyGridShow(index);
                    if (this.treeView1.Nodes.Count > 0)
                    {
                        int i = this.treeView1.Nodes.Count;
                        this.treeView1.SelectedNode = treeView1.Nodes[0];
                    }

                    button5.Enabled = button9.Enabled = button8.Enabled = false;
                }
                Repaint();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            int index = comboBox1.SelectedIndex;
            if (index >= 0)
            {

                switch (_id)
                {
                    case LEFT_SOLDER:
                        VisionProject.Instance.SolderLeftShape(index, hImage.Clone());
                        break;
                    case RIGHT_SOLSER:
                        VisionProject.Instance.SolderRightShape(index, hImage.Clone());
                        break;
                    case LEFT_POLISH:
                        VisionProject.Instance.PolishLeftShape(index, hImage.Clone());
                        break;
                    case RIGHT_POLISH:
                        VisionProject.Instance.PolishRightShape(index, hImage.Clone());
                        break;
                }
            }
        }

        float ang = 0;

        private void button7_Click(object sender, EventArgs e)
        {
            button2_Click(null, null);
            Thread.Sleep(200);
            PointF4 f4 = new PointF4();
            PointF? point = null;
            switch (_id)
            {
                case LEFT_SOLDER:
                    //VisionProject.Instance.LocateSolderLeftShape();
                    f4.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    f4.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];

                    point = VisionProject.Instance.LocateSolderLeftShape(hImage.Clone(), OperShapeIndex, this.hWndCtrller, out this.ang);

                    Thread.Sleep(100);
                    if (point != null)
                    {
                        _SolderPos[OperShapeIndex].Vpos.X = f4.X + point.Value.X;
                        _SolderPos[OperShapeIndex].Vpos.Y = f4.Y + point.Value.Y;

                        Tools.WriteLog.AddLog(DateTime.Now.ToString() + "拍照识别偏差 X;" + point.Value.X.ToString() + "Y;" + point.Value.Y.ToString()
                            + "基点坐标X:" + _SolderPos[OperShapeIndex].Vpos.X.ToString() + "Y:" + _SolderPos[OperShapeIndex].Vpos.Y.ToString());


                        string info = "X偏移" + point.Value.X.ToString("f2") + "，Y偏移" + point.Value.Y.ToString("f2") + ",是否移动到中心点？";
                        if (MessageBox.Show(info, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            while (!FormMain.RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX1, ((int)AxisDef.AxY1), (int)AxisDef.AxZ1, (int)AxisDef.AxR1, ((int)AxisDef.AxT1), _SolderPos[OperShapeIndex].Vpos.X, _SolderPos[OperShapeIndex].Vpos.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL, 0, 2, 0))
                            {
                                Thread.Sleep(1);
                            }
                            Thread.Sleep(100);
                            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[0].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[0].start != 1))
                            {
                                Thread.Sleep(1);
                            }
                        }
                        button5.Enabled = button9.Enabled = button8.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("基准点设置失败");
                        button5.Enabled = button9.Enabled = button8.Enabled = false;
                    }
                    break;
                case RIGHT_SOLSER:
                    //VisionProject.Instance.LocateSolderRightShape();
                    f4.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    f4.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    point = VisionProject.Instance.LocateSolderRightShape(hImage.Clone(), OperShapeIndex, this.hWndCtrller, out this.ang);
                    Thread.Sleep(100);
                    if (point != null)
                    {

                        _SolderPos[OperShapeIndex].Vpos.X = f4.X + point.Value.X;
                        _SolderPos[OperShapeIndex].Vpos.Y = f4.Y + point.Value.Y;
                        Tools.WriteLog.AddLog(DateTime.Now.ToString() + "拍照识别偏差 X;" + point.Value.X.ToString() + "Y;" + point.Value.Y.ToString()
                            + "基点坐标X:" + _SolderPos[OperShapeIndex].Vpos.X.ToString() + "Y:" + _SolderPos[OperShapeIndex].Vpos.Y.ToString());

                        string info = "X偏移" + point.Value.X.ToString("f2") + "，Y偏移" + point.Value.Y.ToString("f2") + ",是否移动到中心点？";
                        if (MessageBox.Show(info, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            while (!FormMain.RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, ((int)AxisDef.AxY2), (int)AxisDef.AxZ2, (int)AxisDef.AxR2, ((int)AxisDef.AxT2), _SolderPos[OperShapeIndex].Vpos.X, _SolderPos[OperShapeIndex].Vpos.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL, 0, 2, 0))
                            {
                                Thread.Sleep(1);
                            }
                            Thread.Sleep(100);
                            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[1].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[1].start != 1))
                            {
                                Thread.Sleep(1);
                            }
                            Thread.Sleep(100);
                            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[0].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[0].start != 1))
                            {
                                Thread.Sleep(1);
                            }
                        }
                        button5.Enabled = button9.Enabled = button8.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("基准点设置失败");
                        button5.Enabled = button9.Enabled = button8.Enabled = false;
                    }
                    break;
                case LEFT_POLISH:
                    //VisionProject.Instance.LocatePolishLeftShape();
                    f4.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    f4.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    point = VisionProject.Instance.LocatePolishLeftShape(hImage.Clone(), OperShapeIndex, this.hWndCtrller);
                    Thread.Sleep(100);
                    if (point != null)
                    {
                        _polishPos[OperShapeIndex].Vpos.X = f4.X + point.Value.X;
                        _polishPos[OperShapeIndex].Vpos.Y = f4.Y + point.Value.Y;

                        Tools.WriteLog.AddLog(DateTime.Now.ToString() + "拍照识别偏差 X;" + point.Value.X.ToString() + "Y;" + point.Value.Y.ToString()
                            + "基点坐标X:" + _polishPos[OperShapeIndex].Vpos.X.ToString() + "Y:" + _polishPos[OperShapeIndex].Vpos.Y.ToString());

                        string info = "X偏移" + point.Value.X.ToString("f2") + "，Y偏移" + point.Value.Y.ToString("f2") + ",是否移动到中心点？";
                        if (MessageBox.Show(info, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            while (!FormMain.RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, ((int)AxisDef.AxY1), (int)AxisDef.AxZ3, (int)AxisDef.AxR3, ((int)AxisDef.AxT1), _polishPos[OperShapeIndex].Vpos.X, _polishPos[OperShapeIndex].Vpos.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL, 0, 2, 0))
                            {
                                Thread.Sleep(1);
                            }
                            Thread.Sleep(100);
                            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[0].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[0].start != 1))
                            {
                                Thread.Sleep(1);
                            }
                        }
                        button5.Enabled = button9.Enabled = button8.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("基准点设置失败");
                        button5.Enabled = button9.Enabled = button8.Enabled = false;
                    }
                    break;
                case RIGHT_POLISH:
                    //VisionProject.Instance.LocatePolishRightShape();
                    f4.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    f4.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    point = VisionProject.Instance.LocatePolishRightShape(hImage.Clone(), OperShapeIndex, this.hWndCtrller);
                    Thread.Sleep(100);
                    if (point != null)
                    {
                        _polishPos[OperShapeIndex].Vpos.X = f4.X + point.Value.X;
                        _polishPos[OperShapeIndex].Vpos.Y = f4.Y + point.Value.Y;
                        Tools.WriteLog.AddLog(DateTime.Now.ToString() + "拍照识别偏差 X;" + point.Value.X.ToString() + "Y;" + point.Value.Y.ToString()
                            + "基点坐标X:" + _polishPos[OperShapeIndex].Vpos.X.ToString() + "Y:" + _polishPos[OperShapeIndex].Vpos.Y.ToString());

                        string info = "X偏移" + point.Value.X.ToString("f2") + "，Y偏移" + point.Value.Y.ToString("f2") + ",是否移动到中心点？";
                        if (MessageBox.Show(info, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            while (!FormMain.RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX3, ((int)AxisDef.AxY2), (int)AxisDef.AxZ3, (int)AxisDef.AxR3, (int)AxisDef.AxT2, _polishPos[OperShapeIndex].Vpos.X, _polishPos[OperShapeIndex].Vpos.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL, 0, 2, 0))
                            {
                                Thread.Sleep(1);
                            }
                            Thread.Sleep(100);
                            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[1].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[1].start != 1))
                            {
                                Thread.Sleep(1);
                            }
                        }
                        button5.Enabled = button9.Enabled = button8.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("基准点设置失败");
                        button5.Enabled = button9.Enabled = button8.Enabled = false;
                    }
                    break;
            }
            Repaint();
        }


        int RowCount = -1;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RowCount = this.treeView1.SelectedNode.Index;
            if (_id == LEFT_POLISH || _id == RIGHT_POLISH)
                propertyGrid1.SelectedObject = _polishPos[OperShapeIndex].pos[RowCount].polishDef;
            else
                propertyGrid1.SelectedObject = _SolderPos[OperShapeIndex].pos[RowCount].solderDef;

            //this.treeView1.SelectedNode.BackColor = Color.Blue;
        }

        #region 显示点位
        private void LoadtreeView1(List<PolishPosdata> f4s)
        {
            int count = 0;
            this.treeView1.Nodes.Clear();
            foreach (PolishPosdata p in f4s)
            {
                count++;
                this.treeView1.Nodes.Add(new TreeNode("端点" + count.ToString() + 
                    ":X:" + p.pos.X.ToString("f2") + 
                    ";\r\n" + "Y:" + p.pos.Y.ToString("f2") + 
                    ";\r\n" + "Z:" + p.pos.Z.ToString("f2") + 
                    ";\r\n" + "R:" + p.pos.R.ToString("f2") + ";"));
            }
        }

        private void LoadtreeView_S(List<SolderPosdata> f4s)
        {
            int count = 0;
            this.treeView1.Nodes.Clear();
            foreach (SolderPosdata p in f4s)
            {
                count++;
                this.treeView1.Nodes.Add(new TreeNode("端点" + count.ToString() +
                    ":X:" + p.pos.X.ToString("f2") +
                    ";\r\n" + "Y:" + p.pos.Y.ToString("f2") +
                    ";\r\n" + "Z:" + p.pos.Z.ToString("f2") +
                    ";\r\n" + "R:" + p.pos.R.ToString("f2") + ";"));
            }
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            button5.Enabled = button9.Enabled = false;
            button7.Enabled = button3.Enabled = true;

            hImage = VisionProject.Instance.GetCurrentImage(CameraIndex);
            if (hImage != null)
            {
                hWndCtrller.AddIconicVar(hImage.Clone());
                Repaint();
            }

        }


        private int CameraIndex
        {
            get
            {
                int index = 0;
                switch (_id)
                {
                    case LEFT_SOLDER:
                        index = 0;
                        break;
                    case RIGHT_SOLSER:
                        index = 1;
                        break;
                    case LEFT_POLISH:
                        index = 2;
                        break;
                    case RIGHT_POLISH:
                        index = 2;
                        break;
                }
                return index;
            }

        }

        private CalibPointToPoint Calib
        {
            get
            {
                CalibPointToPoint calib = null;
                switch (_id)
                {
                    case LEFT_SOLDER:
                        calib = VisionProject.Instance.Calib[0];
                        break;
                    case RIGHT_SOLSER:
                        calib = VisionProject.Instance.Calib[1];
                        break;
                    case LEFT_POLISH:
                        calib = VisionProject.Instance.Calib[2];
                        break;
                    case RIGHT_POLISH:
                        calib = VisionProject.Instance.Calib[3];
                        break;
                }
                return calib;
            }

        }


        private void button10_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("是否将当前位置设置为基准点？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                return;

            int index = comboBox1.SelectedIndex;
            PointF2 f2 = new PointF2();
            PointF point = new PointF();

            switch (_id)
            {
                case LEFT_SOLDER:
                    //point = VisionProject.Instance.GetSolderLeftShapeDeviation(index);
                    _SolderPos[index].Vpos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1];
                    _SolderPos[index].Vpos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];
                    break;
                case RIGHT_SOLSER:
                    //point = VisionProject.Instance.GetSolderRightShapeDeviation(index);
                    _SolderPos[index].Vpos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2];
                    _SolderPos[index].Vpos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    break;
                case LEFT_POLISH:
                    //point = VisionProject.Instance.GetPolishLeftShapeDeviation(index);
                    _polishPos[index].Vpos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    _polishPos[index].Vpos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1];

                    break;
                case RIGHT_POLISH:
                    //point = VisionProject.Instance.PolishRightShapeDeviation(index);
                    _polishPos[index].Vpos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3];
                    _polishPos[index].Vpos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2];
                    break;
            }
        }

        private void button9_Click(object sender, EventArgs e)//修改点
        {
            int index = comboBox1.SelectedIndex;
            if (index < 0 || RowCount < 0 || this.treeView1.SelectedNode == null)
            {
                MessageBox.Show("请先选择端点");
            }
            else
            {
                switch (_id)
                {
                    case LEFT_SOLDER:
                        _SolderPos[index].pos[RowCount].pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX1] - _SolderPos[index].Vpos.X;
                        _SolderPos[index].pos[RowCount].pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1] - _SolderPos[index].Vpos.Y;
                        _SolderPos[index].pos[RowCount].pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ1];
                        _SolderPos[index].pos[RowCount].pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR1];
                        break;
                    case RIGHT_SOLSER:
                        _SolderPos[index].pos[RowCount].pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX2] - _SolderPos[index].Vpos.X;
                        _SolderPos[index].pos[RowCount].pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2] - _SolderPos[index].Vpos.Y;
                        _SolderPos[index].pos[RowCount].pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ2];
                        _SolderPos[index].pos[RowCount].pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR2];
                        break;
                    case LEFT_POLISH:
                        _polishPos[index].pos[RowCount].pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3] - _polishPos[index].Vpos.X;
                        _polishPos[index].pos[RowCount].pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY1] - _polishPos[index].Vpos.Y;
                        _polishPos[index].pos[RowCount].pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ3];
                        _polishPos[index].pos[RowCount].pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR3];
                        break;
                    case RIGHT_POLISH:
                        _polishPos[index].pos[RowCount].pos.X = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX3] - _polishPos[index].Vpos.X;
                        _polishPos[index].pos[RowCount].pos.Y = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY2] - _polishPos[index].Vpos.Y;
                        _polishPos[index].pos[RowCount].pos.Z = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ3];
                        _polishPos[index].pos[RowCount].pos.R = FormMain.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxR3];
                        break;
                }
            }
            propertyGridShow(index);

        }

        private void button8_Click(object sender, EventArgs e)//定位
        {
            int index = comboBox1.SelectedIndex;
            DialogResult data = DialogResult.None;
            if (RowCount < 0) return;
            PointF4 f4 = new PointF4();

            float x = 0;
            float y = 0;
            float aa =0;

            switch (_id)
            {
                case LEFT_SOLDER:
                    f4.X = _SolderPos[index].pos[RowCount].pos.X + _SolderPos[index].Vpos.X;
                    f4.Y = _SolderPos[index].pos[RowCount].pos.Y + _SolderPos[index].Vpos.Y;
                    f4.Z = _SolderPos[index].pos[RowCount].pos.Z;
                    f4.R = _SolderPos[index].pos[RowCount].pos.R;

                    Tools.WriteLog.AddLog(DateTime.Now.ToString() + "焊锡左X;" + f4.X.ToString() + "Y：" + f4.Y.ToString() + "Z：" + f4.Z.ToString() + "R：" + f4.R.ToString() + "基点X：" + _SolderPos[index].Vpos.X.ToString() + "Y:" + _SolderPos[index].Vpos.Y.ToString());

                    if (FormMain.RunProcess.LogicData.RunData.Rotate)
                    {
                        FormMain.RunProcess.Transorm(Logic.UsingPlatformSelect.Left, _SolderPos[index].Vpos.X, _SolderPos[index].Vpos.Y,
                            f4.X, f4.Y, f4.R, this.ang, out x, out y);
                        aa = (float)(this.ang * 180 / Math.PI);

                        while (!FormMain.RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX1, ((int)AxisDef.AxY1),
                            (int)AxisDef.AxZ1, (int)AxisDef.AxR1, ((int)AxisDef.AxT1), x, y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL, f4.R + aa, 2, 0))
                        {
                            Thread.Sleep(1);
                        }
                    }
                    else
                    {
                        while (!FormMain.RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX1, ((int)AxisDef.AxY1),
                            (int)AxisDef.AxZ1, (int)AxisDef.AxR1, ((int)AxisDef.AxT1), f4.X, f4.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL, f4.R, 2, 0))
                        {
                            Thread.Sleep(1);
                        }
                    }

                    
                    Thread.Sleep(100);
                    while (!(FormMain.RunProcess.LogicAPI.PlatformMove[0].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[0].start != 1))
                    {
                        Thread.Sleep(1);
                    }

                    if (MessageBox.Show("是否Z轴下降到焊点", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        FormMain.RunProcess.movedriverZm.MoveAbs((int)AxisDef.AxZ1, FormMain.RunProcess.LogicData.slaverData.basics.TeachSpeedL, f4.Z);
                    }

                    break;
                case RIGHT_SOLSER:
                    f4.X = _SolderPos[index].pos[RowCount].pos.X + _SolderPos[index].Vpos.X;
                    f4.Y = _SolderPos[index].pos[RowCount].pos.Y + _SolderPos[index].Vpos.Y;
                    f4.Z = _SolderPos[index].pos[RowCount].pos.Z;
                    f4.R = _SolderPos[index].pos[RowCount].pos.R;

                    Tools.WriteLog.AddLog(DateTime.Now.ToString() + "焊锡右X;" + f4.X.ToString() + "Y：" + f4.Y.ToString() + "Z：" + f4.Z.ToString() + "R：" + f4.R.ToString() + "基点X：" + _SolderPos[index].Vpos.X.ToString() + "Y:" + _SolderPos[index].Vpos.Y.ToString());

                    if (FormMain.RunProcess.LogicData.RunData.Rotate_r)
                    {
                        FormMain.RunProcess.Transorm(Logic.UsingPlatformSelect.Right, _SolderPos[index].Vpos.X, _SolderPos[index].Vpos.Y,
                            f4.X, f4.Y, f4.R, this.ang, out x, out y);
                        aa = (float)(this.ang * 180 / Math.PI);

                        while (!FormMain.RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, ((int)AxisDef.AxY2),
                            (int)AxisDef.AxZ2, (int)AxisDef.AxR2, ((int)AxisDef.AxT2), x, y,/*f4.X, f4.Y,*/ FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZR, f4.R + aa, 2, 0))
                        {
                            Thread.Sleep(1);
                        }
                    }
                    else
                    {
                        while (!FormMain.RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX2, ((int)AxisDef.AxY2),
                            (int)AxisDef.AxZ2, (int)AxisDef.AxR2, ((int)AxisDef.AxT2), f4.X, f4.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZR, f4.R, 2, 0))
                        {
                            Thread.Sleep(1);
                        }
                    }
                    Thread.Sleep(100);
                    while (!(FormMain.RunProcess.LogicAPI.PlatformMove[1].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[1].start != 1))
                    {
                        Thread.Sleep(1);
                    }

                    if (MessageBox.Show("是否Z轴下降到焊点", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        FormMain.RunProcess.movedriverZm.MoveAbs((int)AxisDef.AxZ2, FormMain.RunProcess.LogicData.slaverData.basics.TeachSpeedR, f4.Z);
                    }

                    break;
                case LEFT_POLISH:
                    f4.X = _polishPos[index].pos[RowCount].pos.X + _polishPos[index].Vpos.X;
                    f4.Y = _polishPos[index].pos[RowCount].pos.Y + _polishPos[index].Vpos.Y;
                    f4.Z = _polishPos[index].pos[RowCount].pos.Z;
                    f4.R = _polishPos[index].pos[RowCount].pos.R;

                    Tools.WriteLog.AddLog(DateTime.Now.ToString() + "打磨左X;" + f4.X.ToString() + "Y：" + f4.Y.ToString() + "Z：" + f4.Z.ToString() + "R：" + f4.R.ToString() + "基点X：" + _polishPos[index].Vpos.X.ToString() + "Y:" + _polishPos[index].Vpos.Y.ToString());

                    while (!FormMain.RunProcess.LogicAPI.PlatformMove[0].exe((int)AxisDef.AxX3, ((int)AxisDef.AxY1),
                        (int)AxisDef.AxZ3, (int)AxisDef.AxR3, ((int)AxisDef.AxT1), f4.X, f4.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_Z, f4.R, 2, 0))
                    {
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(100);
                    while (!(FormMain.RunProcess.LogicAPI.PlatformMove[0].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[0].start != 1))
                    {
                        Thread.Sleep(1);
                    }

                    if (MessageBox.Show("是否Z轴下降到磨点", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        FormMain.RunProcess.movedriverZm.MoveAbs((int)AxisDef.AxZ3, FormMain.RunProcess.LogicData.slaverData.basics.TeachSpeed, f4.Z);
                    }

                    break;
                case RIGHT_POLISH:
                    f4.X = _polishPos[index].pos[RowCount].pos.X + _polishPos[index].Vpos.X;
                    f4.Y = _polishPos[index].pos[RowCount].pos.Y + _polishPos[index].Vpos.Y;
                    f4.Z = _polishPos[index].pos[RowCount].pos.Z;
                    f4.R = _polishPos[index].pos[RowCount].pos.R;

                    Tools.WriteLog.AddLog(DateTime.Now.ToString() + "打磨右X;" + f4.X.ToString() + "Y：" + f4.Y.ToString() + "Z：" + f4.Z.ToString() + "R：" + f4.R.ToString() + "基点X：" + _polishPos[index].Vpos.X.ToString() + "Y:" + _polishPos[index].Vpos.Y.ToString());

                    while (!FormMain.RunProcess.LogicAPI.PlatformMove[1].exe((int)AxisDef.AxX3, ((int)AxisDef.AxY2),
                        (int)AxisDef.AxZ3, (int)AxisDef.AxR3, ((int)AxisDef.AxT2), f4.X, f4.Y, FormMain.RunProcess.LogicData.slaverData.basics.Safe_Z, f4.R, 2, 0))
                    {
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(100);
                    while (!(FormMain.RunProcess.LogicAPI.PlatformMove[1].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[1].start != 1))
                    {
                        Thread.Sleep(1);
                    }

                    if (MessageBox.Show("是否Z轴下降到磨点", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        FormMain.RunProcess.movedriverZm.MoveAbs((int)AxisDef.AxZ3, FormMain.RunProcess.LogicData.slaverData.basics.TeachSpeed, f4.Z);
                    }

                    break;
            }
        }

        private void Form_SolderSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            ftemp.List_Change();
        }
      




        private void 一键同步参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否将来当前模板内所有端点参数设置为当前参数", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            if (_id == LEFT_POLISH || _id == RIGHT_POLISH)
            {
                foreach (PolishPosdata data in _polishPos[OperShapeIndex].pos)
                {
                    data.polishDef = _polishPos[OperShapeIndex].pos[RowCount].polishDef.Clone();
                }

            }
            else
            {
                foreach(SolderPosdata data in _SolderPos[OperShapeIndex].pos)
                {
                    data.solderDef = _SolderPos[OperShapeIndex].pos[RowCount].solderDef.Clone();
                }

            }
        }
    }





}
