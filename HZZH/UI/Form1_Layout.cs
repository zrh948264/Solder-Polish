using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Common.PointLayout;
using HZZH.Vision.Logic;
using Motion;

namespace UI
{
    public partial class Form1_Layout : Form
    {
        public FormMain ftemp;
        SiteRegion _index = 0;//
        DisplayLayoutPoint displayLayoutPoint = null;
        List<LocationShapePoint> locationShapes = null;

        public Form1_Layout()
        {
            InitializeComponent();
        }

        public Form1_Layout(int index)
        {
            InitializeComponent();
            this._index = (SiteRegion)index;
            FormMain.RunProcess.LogicData.RunData.GetLayoutPointsShow((int)_index, 0);
            toolStripButton1.ForeColor = Color.Black;
            toolStripButton2.ForeColor = Color.Green;
        }

        #region 显示画点
        
        public void LoadData()
        {
            switch (_index)
            {
                case SiteRegion.SOLDER_LIFT:
                    locationShapes = VisionProject.Instance.VisionTools.SolderLeft;
                    break;
                case SiteRegion.SOLDER_RIGHT:
                    locationShapes = VisionProject.Instance.VisionTools.SolderRight;
                    break;
                case SiteRegion.POLISH_LIFT:
                    locationShapes = VisionProject.Instance.VisionTools.PolishLeft;
                    break;
                case SiteRegion.POLISH_RIGHT:
                    locationShapes = VisionProject.Instance.VisionTools.PolishRight;
                    break;
            }

            if (displayLayoutPoint != null)
            {
                displayLayoutPoint.Dispose();
            }
            displayLayoutPoint = new DisplayLayoutPoint(this.pictureBox1, FormMain.RunProcess.LogicData.RunData.GenLayoutPointEnumerable());

            System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();

            if (_index == SiteRegion.SOLDER_LIFT)
            {
                RectangleF rectangleF = new RectangleF();
                rectangleF.Location = new PointF(0, 0);
                rectangleF.Width = pictureBox1.ClientSize.Width;
                rectangleF.Height = pictureBox1.ClientSize.Height;
                matrix = new System.Drawing.Drawing2D.Matrix(rectangleF, new PointF[] { new PointF(0, rectangleF.Height), new PointF(rectangleF.Width, rectangleF.Height), new PointF(0, 0) });
            }
            else
            {
                matrix.RotateAt(180, new PointF(pictureBox1.ClientSize.Width / 2f, pictureBox1.ClientSize.Height / 2f));
            }

            displayLayoutPoint.DisplayMatrix = matrix;

            displayLayoutPoint.Repaint();


            contextMenuStrip1.Items.Clear();
            for (int i = 0;i < locationShapes.Count;i++)
            {
                string neme = "模板"+ i.ToString();
                contextMenuStrip1.Items.Add(neme);
                contextMenuStrip1.Items[i].Click += ToolStripMenuItem_Click;
                contextMenuStrip1.Items[i].Tag = i.ToString();
            }
            
        }

        #endregion
        
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tool = sender as ToolStripMenuItem;
            int index = Convert.ToInt32(tool.Tag.ToString());

            wPointF[] sites = displayLayoutPoint.GetSelectPoint<wPointF>();
            foreach (var v in sites)
            {
                v.templateIndex = index;
            }


            ftemp.List_Change();
        }

        private void Form1_Layout_Load(object sender, EventArgs e)
        {
            ftemp = (FormMain)this.Owner;
            LoadData();
        }
        
        private void toolStripButton_T_Click(object sender, EventArgs e)
        {
            toolStripButton1.ForeColor = Color.Black;
            toolStripButton2.ForeColor = Color.Green;

            FormMain.RunProcess.LogicData.RunData.GetLayoutPointsShow((int)_index, 0);
            LoadData();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton1.ForeColor = Color.Green;
            toolStripButton2.ForeColor = Color.Black;

            FormMain.RunProcess.LogicData.RunData.GetLayoutPointsShow((int)_index, 1);
            LoadData();
        }
    }
}
