using Common;
using System;
using System.Windows.Forms;
using Motion;
namespace UI
{
    public partial class Frm_TwoPArray :Form
    {
        public FormMain ftemp;

        public Frm_TwoPArray()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent; 
        }

        private void Frm_PArray_Load(object sender, EventArgs e)
        {
            ftemp = (FormMain)this.Owner;
        }


        #region 两点阵列

        private void btn_TwoArrayreadp1_Click(object sender, EventArgs e)
        {
            //numericUpDown1.Value = (decimal)ftemp.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX];
            //numericUpDown2.Value = (decimal)ftemp.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY];
            //numericUpDown3.Value = (decimal)ftemp.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ];
        }

        private void btn_TwoArrayreadp2_Click(object sender, EventArgs e)
        {
            //numericUpDown4.Value = (decimal)ftemp.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxX];
            //numericUpDown5.Value = (decimal)ftemp.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxY];
            //numericUpDown6.Value = (decimal)ftemp.RunProcess.movedriverZm.CurrentPos.FloatValue[(int)AxisDef.AxZ];
        }
        #endregion

        PointF3 pLeftDwon = new PointF3();
        PointF3 pRightUp = new PointF3();

        private void btnArr_Click(object sender, EventArgs e)
        {

            if ((!radioButton1.Checked) && (!radioButton2.Checked))
            {
                MessageBox.Show("请选择方向并确认");
                return;
            }
            int RowNum = Convert.ToInt32(numericUpDown7.Value);
            int ColumnNum = Convert.ToInt32(numericUpDown8.Value);

            pLeftDwon.X = Convert.ToSingle(numericUpDown1.Value);
            pLeftDwon.Y = Convert.ToSingle(numericUpDown2.Value);
            pLeftDwon.Z = Convert.ToSingle(numericUpDown3.Value);


            pRightUp.X = Convert.ToSingle(numericUpDown4.Value);
            pRightUp.Y = Convert.ToSingle(numericUpDown5.Value);
            pRightUp.Z = Convert.ToSingle(numericUpDown6.Value);

            int dripDirection = radioButton1.Checked ? 0 : 1;//点胶方式
            //ftemp.LoadStickData(PointArray.MatrixArrayList(pLeftDwon, pRightUp, RowNum, ColumnNum, dripDirection));
            //ftemp.pictureBoxStitchRefresh();
            MessageBox.Show("阵列成功");
        }


        private int GetPasteDirction()
        {
            // 测试方式   0 横向  1 纵向
            return radioButton1.Checked == true ? 0 : 1;
        }

        private void SetPasteDirction(int mode)
        {
            if (mode < 2)
            {
                if (mode == 0)
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;

            }
        }

    }
}
