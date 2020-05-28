using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using UI;
using System.Drawing;

namespace MyControl
{
    public partial class Jog :Form, IDisposable
    {
        public int _id = 0;

        public const int LEFT_SOLDER = 0;
        public const int RIGHT_SOLSER = 1;
        public const int LEFT_POLISH = 2;
        public const int RIGHT_POLISH = 3;

        public Jog()
        {
            InitializeComponent();
            //InitializeAxisName();
            ContorlBinding();
            TagBinding(0);

        }
        public void SetMoveController(Device.BoardCtrllerManager movedriverZm)
        {
            this.movedriverZm = movedriverZm;
        }

        private void InitializeAxisName()
        {
            IList<ComboBoxIndex> list = new List<ComboBoxIndex>();

            list.Add(new ComboBoxIndex() { index = 0, name = "X1轴" });
            list.Add(new ComboBoxIndex() { index = 1, name = "Y1轴" });
            list.Add(new ComboBoxIndex() { index = 2, name = "Z1轴" });
            list.Add(new ComboBoxIndex() { index = 3, name = "R1轴" });
            list.Add(new ComboBoxIndex() { index = 4, name = "T1轴" });
            list.Add(new ComboBoxIndex() { index = 5, name = "S1轴" });

            list.Add(new ComboBoxIndex() { index = 6, name = "X2轴" });
            list.Add(new ComboBoxIndex() { index = 7, name = "Y2轴" });
            list.Add(new ComboBoxIndex() { index = 8, name = "Z2轴" });
            list.Add(new ComboBoxIndex() { index = 9, name = "R2轴" });
            list.Add(new ComboBoxIndex() { index = 10, name = "T2轴" });
            list.Add(new ComboBoxIndex() { index = 11, name = "S2轴" });

            list.Add(new ComboBoxIndex() { index = 12, name = "X3轴" });
            list.Add(new ComboBoxIndex() { index = 13, name = "Z3轴" });
            list.Add(new ComboBoxIndex() { index = 14, name = "R3轴" });

            comboBox4.DataSource = list;
            comboBox4.ValueMember = "index";
            comboBox4.DisplayMember = "name";
            comboBox4.SelectedIndex = 0;
        }
        public void ContorlBinding()
        {
            ///焊锡头1
            ConfigJog(Direction.Neg, button16);//x
            ConfigJog(Direction.Pos, button2);

            ConfigJog(Direction.Neg, button3);//y
            ConfigJog(Direction.Pos, button1);

            ConfigJog(Direction.Neg, button18);//z
            ConfigJog(Direction.Pos, button17);

            ConfigJog(Direction.Pos, button12);//r
            ConfigJog(Direction.Neg, button10);

            ConfigJog(Direction.Pos, button13);//t
            ConfigJog(Direction.Neg, button19);
            /////焊锡头2
            //ConfigJog(Direction.Pos, button7);//x
            //ConfigJog(Direction.Neg, button6);

            //ConfigJog(Direction.Pos, button4);//y
            //ConfigJog(Direction.Neg, button5);

            //ConfigJog(Direction.Neg, button9);//z
            //ConfigJog(Direction.Pos, button8);

            //ConfigJog(Direction.Pos, button14);//r
            //ConfigJog(Direction.Neg, button11);

            //ConfigJog(Direction.Pos, button26);//t
            //ConfigJog(Direction.Neg, button27);
            /////焊锡头2
            //ConfigJog(Direction.Neg, button15);//x
            //ConfigJog(Direction.Pos, button20);

            //ConfigJog(Direction.Pos, button21);//z
            //ConfigJog(Direction.Neg, button23);

            //ConfigJog(Direction.Pos, button25);//r
            //ConfigJog(Direction.Neg, button24);


            ConfigJog(Direction.Pos, button28);//S1
            ConfigJog(Direction.Neg, button29);

            //ConfigJog(Direction.Pos, button31);//S2
            //ConfigJog(Direction.Neg, button30);

            ConfigJog(Direction.Hom, button22);
        }


        public void TagBinding(int index)
        {
            IList<ComboBoxIndex> list = new List<ComboBoxIndex>();
            _id = index;
            switch (index)
            {
                case LEFT_SOLDER:
                    ConfigJog(Direction.Neg, button16);//x
                    ConfigJog(Direction.Pos, button2);
                    button2.Tag = button16.Tag = "0";
                    button3.Tag = button1.Tag = "1";
                    button18.Tag = button17.Tag = "2";
                    button12.Tag = button10.Tag = "3";
                    button13.Tag = button19.Tag = "4";
                    button28.Tag = button29.Tag = "5";
                    

                    list.Add(new ComboBoxIndex() { index = 0, name = "X1轴" });
                    list.Add(new ComboBoxIndex() { index = 1, name = "Y1轴" });
                    list.Add(new ComboBoxIndex() { index = 2, name = "Z1轴" });
                    list.Add(new ComboBoxIndex() { index = 3, name = "R1轴" });

                    break;
                case RIGHT_SOLSER:
                    ConfigJog(Direction.Pos, button16);//x
                    ConfigJog(Direction.Neg, button2);
                    button2.Tag = button16.Tag = "6";
                    button3.Tag = button1.Tag = "7";
                    button18.Tag = button17.Tag = "8";
                    button12.Tag = button10.Tag = "9";
                    button13.Tag = button19.Tag = "10";
                    button28.Tag = button29.Tag = "11";

                    list.Add(new ComboBoxIndex() { index = 0, name = "X2轴" });
                    list.Add(new ComboBoxIndex() { index = 1, name = "Y2轴" });
                    list.Add(new ComboBoxIndex() { index = 2, name = "Z2轴" });
                    list.Add(new ComboBoxIndex() { index = 3, name = "R2轴" });
                    break;
                case LEFT_POLISH:
                    ConfigJog(Direction.Pos, button16);//x
                    ConfigJog(Direction.Neg, button2);
                    button2.Tag = button16.Tag = "12";
                    button3.Tag = button1.Tag = "1";
                    button18.Tag = button17.Tag = "13";
                    button12.Tag = button10.Tag = "14";
                    button13.Tag = button19.Tag = "4";
                    button28.Tag = button29.Tag = "5";

                    list.Add(new ComboBoxIndex() { index = 0, name = "X3轴" });
                    list.Add(new ComboBoxIndex() { index = 1, name = "Y1轴" });
                    list.Add(new ComboBoxIndex() { index = 2, name = "Z3轴" });
                    list.Add(new ComboBoxIndex() { index = 3, name = "R3轴" });
                    break;
                case RIGHT_POLISH:
                    ConfigJog(Direction.Pos, button16);//x
                    ConfigJog(Direction.Neg, button2);
                    button2.Tag = button16.Tag = "12";
                    button3.Tag = button1.Tag = "7";
                    button18.Tag = button17.Tag = "13";
                    button12.Tag = button10.Tag = "14";
                    button13.Tag = button19.Tag = "10";
                    button28.Tag = button29.Tag = "11";

                    list.Add(new ComboBoxIndex() { index = 0, name = "X3轴" });
                    list.Add(new ComboBoxIndex() { index = 1, name = "Y2轴" });
                    list.Add(new ComboBoxIndex() { index = 2, name = "Z3轴" });
                    list.Add(new ComboBoxIndex() { index = 3, name = "R3轴" });
                    break;
            }

            comboBox4.DataSource = list;
            comboBox4.ValueMember = "index";
            comboBox4.DisplayMember = "name";
            comboBox4.SelectedIndex = 0;

        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            _targetPos = (float)numericUpDown31.Value;
        }


        #region 按键事件
        /// <summary>
        /// 绑定的板卡
        /// </summary>
        private Device.BoardCtrllerManager movedriverZm;
        /// <summary>
        /// 移动距离
        /// </summary>
        public float _targetPos = 1;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float _speed = 5;

        public int _mode = 1;

        private void ConfigJog(Direction type, Button _b)
        {
            _b.MouseDown -= btn_JogAxisPos_MouseDown;
            _b.MouseDown -= btn_JogAxisNeg_MouseDown;
            _b.MouseUp -= btn_JogAxis_MouseUp;
            _b.Click -= btn_Home_Click;

            switch (type)
            {
                case Direction.Pos:
                    _b.MouseDown += btn_JogAxisPos_MouseDown;
                    _b.MouseUp += btn_JogAxis_MouseUp;
                    break;
                case Direction.Neg:
                    _b.MouseDown += btn_JogAxisNeg_MouseDown;
                    _b.MouseUp += btn_JogAxis_MouseUp;
                    break;
                case Direction.Hom:
                    _b.Click += btn_Home_Click;
                    break;
            }
        }

        private void btn_JogAxisPos_MouseDown(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);
            if(axis== (ushort)Motion.AxisDef.AxT1)
            {
                FormMain.RunProcess.LogicAPI.reversals[0].exe(0);
                FormMain.RunProcess.LogicAPI.reversals[0].Initialize();
                return;
            }
            if (axis == (ushort)Motion.AxisDef.AxT2)
            {
                FormMain.RunProcess.LogicAPI.reversals[1].exe(0);
                FormMain.RunProcess.LogicAPI.reversals[1].Initialize();
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                _speed = 40;
                JogAxisPos(axis, _mode, _speed, _targetPos);
            }
            if (e.Button == MouseButtons.Right)
            {
                _speed = 10;
                JogAxisPos(axis, _mode, _speed, _targetPos);
            }
        }

        private void btn_JogAxis_MouseUp(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);
            if (axis != (ushort)Motion.AxisDef.AxT1 && axis != (ushort)Motion.AxisDef.AxT2)
            {
                JogAxisStop(axis, _mode);
            }
        }

        private void btn_JogAxisNeg_MouseDown(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);
            if (axis == (ushort)Motion.AxisDef.AxT1)
            {
                FormMain.RunProcess.LogicAPI.reversals[0].exe(1);
                FormMain.RunProcess.LogicAPI.reversals[0].Initialize();
                return;
            }
            if (axis == (ushort)Motion.AxisDef.AxT2)
            {
                //while (!FormMain.RunProcess.LogicAPI.reversals[1].exe(1)) { }

                FormMain.RunProcess.LogicAPI.reversals[1].exe(1);
                FormMain.RunProcess.LogicAPI.reversals[1].Initialize();
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                _speed = 40;
                JogAxisNeg(axis, _mode, _speed, _targetPos);
            }
            if (e.Button == MouseButtons.Right)
            {
                _speed = 10;
                JogAxisNeg(axis, _mode, _speed, _targetPos);
            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
			ushort index = (ushort)comboBox4.SelectedIndex;
            ushort axis = 0;
            float speed = 100;

            switch (_id)
            {
                case LEFT_SOLDER:
                    switch (index)
                    {
                        case 0:
                            axis = 0;
                            break;
                        case 1:
                            axis = 1;
                            break;
                        case 2:
                            axis = 2;
                            break;
                        case 3:
                            axis = 3;
                            break;
                    }
                    break;
                case RIGHT_SOLSER:
                    switch (index)
                    {
                        case 0:
                            axis = 6;
                            break;
                        case 1:
                            axis = 7;
                            break;
                        case 2:
                            axis = 8;
                            break;
                        case 3:
                            axis = 9;
                            break;
                    }
                    break;
                case LEFT_POLISH:
                    switch (index)
                    {
                        case 0:
                            axis = 12;
                            break;
                        case 1:
                            axis = 1;
                            break;
                        case 2:
                            axis = 13;
                            break;
                        case 3:
                            axis = 14;
                            break;
                    }
                    break;
                case RIGHT_POLISH:
                    switch (index)
                    {
                        case 0:
                            axis = 12;
                            break;
                        case 1:
                            axis = 7;
                            break;
                        case 2:
                            axis = 13;
                            break;
                        case 3:
                            axis = 14;
                            break;
                    }
                    break;
                    break;
            }
            AxisHomeAction(axis, speed, comboBox4.Text);
        }

        public void JogAxisNeg(ushort axisNum, int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
				movedriverZm.MoveSpd(axisNum, jogSpeed, -targetPos);
            }
            else
            {
				movedriverZm.MoveRel(axisNum, jogSpeed, -targetPos);
            }
        }

        public void JogAxisPos(ushort axisNum, int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
				movedriverZm.MoveSpd(axisNum, jogSpeed, targetPos);
            }

            else
            {
				movedriverZm.MoveRel(axisNum, jogSpeed, targetPos);
            }
        }

        public void JogAxisStop(ushort axisNum,int mode)
        {
            if (mode == 0)
            {
				movedriverZm.MoveStop(axisNum);
            }
        }

        public void AxisHomeAction(ushort axisNum, float jogSpeed, string Axis_Name)
        {
            if (MessageBox.Show("确定要执行此轴回零...", Axis_Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
				movedriverZm.MoveHome(axisNum, jogSpeed);
            }
        }


        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_X.Text = "X:" + movedriverZm.CurrentPos.FloatValue[Convert.ToInt32(button2.Tag.ToString())].ToString("f2");
            label_Y.Text = "Y:" + movedriverZm.CurrentPos.FloatValue[Convert.ToInt32(button1.Tag.ToString())].ToString("f2");
            label_Z.Text = "Z:" + movedriverZm.CurrentPos.FloatValue[Convert.ToInt32(button18.Tag.ToString())].ToString("f2");
            label_R.Text = "R:" + movedriverZm.CurrentPos.FloatValue[Convert.ToInt32(button12.Tag.ToString())].ToString("f2");
        }

        private void Jog_Load(object sender, EventArgs e)
        {
            this.timer1.Interval = 200;
            this.timer1.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                _mode = 1;
            }
            else
            {
                _mode = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] Output = (int[])DiDoStatus.CurrOutputStatus.Clone();
            if ((Output[0] & (int)(0x0001 << (3))) > 0)
            {
                //button4.BackColor = SystemColors.Control;
                button4.BackColor = Color.Green;
                Output[0] &= (((int)~(0x0001 << (3))));
            }
            else
            {
                //button4.BackColor = Color.Green;
                button4.BackColor = SystemColors.Control;
                Output[0] |= (int)(0x0001 << (3));
            }
            movedriverZm.WriteRegister(new BaseData(1020, Output));
        }
    }

    public enum Direction : int
    {
        Pos,
        Neg,
        Hom
    }

    public class ComboBoxIndex
    {
        public int index { get; set; }
        public string name { get; set; }
    }

}
