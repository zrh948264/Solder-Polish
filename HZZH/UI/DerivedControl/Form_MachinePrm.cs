using Common;
using Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyControl
{
    public partial class UserMachinePrm : Form
    {
        
        public UserMachinePrm()
        {
            InitializeComponent();
        }
      
        public List<EffectiveMode> listlev = new List<EffectiveMode>();//电平模式
        public List<EffectiveMode> listHome = new List<EffectiveMode>();//回零模式
        public List<EffectiveMode> Limit = new List<EffectiveMode>();//限位模式
        private void InitializeDataSources()
        {
            listlev.Add(new EffectiveMode() { Index = 0, Name = "ON有效" });
            listlev.Add(new EffectiveMode() { Index = 1, Name = "OFF有效" });
            listlev.Add(new EffectiveMode() { Index = 2, Name = "无效" });

            listHome.Add(new EffectiveMode() { Index = 0, Name = "正常回零" });
            listHome.Add(new EffectiveMode() { Index = 1, Name = "先到上限位再回零" });
            listHome.Add(new EffectiveMode() { Index = 2, Name = "先到下限位再回零" });
            listHome.Add(new EffectiveMode() { Index = 3, Name = "Z向找原点正向" });
            listHome.Add(new EffectiveMode() { Index = 4, Name = "Z向找原点负向" });

            Limit.Add(new EffectiveMode() { Index = 0, Name = "无限位" });
            Limit.Add(new EffectiveMode() { Index = 1, Name = "软限位" });
            Limit.Add(new EffectiveMode() { Index = 2, Name = "硬限位" });
            Limit.Add(new EffectiveMode() { Index = 3, Name = "软硬皆限" });

            
        }


        private Device.BoardCtrllerManager movedriverZm;
        public void SetMoveController(Device.BoardCtrllerManager movedriverZm)
        {
            this.movedriverZm = movedriverZm;
			DownMotorPrmToSlave();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show(this, "是否保存对“电机参数”的修改 ？", ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;
            try
            {                
                SavePrmData();
                DownMotorPrmToSlave();
                movedriverZm.WriteRegister(new BaseData(1100, new int[] { 2}));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SavePrmData();
            Axis axisIO = new Axis();
            List<Axis> listAxis = new List<Axis>();
            axisIO.Num = (int)numericUpDown1.Value;
            for (int i = 0; i < ConfigHandle.Instance.AxisDefine.AxisList.Count; i++)
            {
                if (ConfigHandle.Instance.AxisDefine.AxisList[i].Num == (int)numericUpDown1.Value)
                {
                    MessageBox.Show(numericUpDown1.Value.ToString() + "轴已存在");
                    return;
                }
            }
            ConfigHandle.Instance.AxisDefine.AxisList.Add(axisIO);
            listAxis.AddRange(ConfigHandle.Instance.AxisDefine.AxisList.OrderBy(p => p.Num));
            ConfigHandle.Instance.AxisDefine.AxisList.Clear();
            ConfigHandle.Instance.AxisDefine.AxisList.AddRange(listAxis);
            LoadPrmData();           
        }

        private void LoadPrmData()
        {
            try
            {
                if (ConfigHandle.Instance.AxisDefine.AxisList == null) return;
                this.dataGridViewMachinePrm.Rows.Clear();
                for (int i = 0; i < ConfigHandle.Instance.AxisDefine.AxisList.Count; i++)
                {
                    #region 方式2
                    DataGridViewRow row = new DataGridViewRow();
                    if (i % 2 == 0)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlLight;
                    }
                    DataGridViewTextBoxCell[] textboxcell = new DataGridViewTextBoxCell[17];
                    for (int j = 0; j < 17; j++)
                    {
                        textboxcell[j] = new DataGridViewTextBoxCell();
                    }
                    DataGridViewComboBoxCell[] combox = new DataGridViewComboBoxCell[4];
                    for (int j = 0; j < 4; j++)
                    {
                        combox[j] = new DataGridViewComboBoxCell();
                        combox[j].DataSource = null;
                        combox[j].DataSource = listlev;
                        combox[j].ValueMember = "index";
                        combox[j].DisplayMember = "name";
                    }

                    DataGridViewComboBoxCell comboxHome = new DataGridViewComboBoxCell();
                    comboxHome.DataSource = null;
                    comboxHome.DataSource = listHome;
                    comboxHome.ValueMember = "index";
                    comboxHome.DisplayMember = "name";

                    DataGridViewComboBoxCell comboxLimit = new DataGridViewComboBoxCell();
                    comboxLimit.DataSource = null;
                    comboxLimit.DataSource = Limit;
                    comboxLimit.ValueMember = "index";
                    comboxLimit.DisplayMember = "name";


                    textboxcell[0].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].Num;
                    row.Cells.Add(textboxcell[0]);
                    textboxcell[1].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].Name;
                    row.Cells.Add(textboxcell[1]);
                    textboxcell[2].Value = (int)ConfigHandle.Instance.AxisDefine.AxisList[i].StartSpeed;
                    row.Cells.Add(textboxcell[2]);
                    textboxcell[3].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].AccSpeed;
                    row.Cells.Add(textboxcell[3]);
                    textboxcell[4].Value = (int)ConfigHandle.Instance.AxisDefine.AxisList[i].RunSpeed;
                    row.Cells.Add(textboxcell[4]);
                    textboxcell[5].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].DecSpeed;
                    row.Cells.Add(textboxcell[5]);
                    textboxcell[6].Value =  (int)ConfigHandle.Instance.AxisDefine.AxisList[i].EndSpeed;
                    row.Cells.Add(textboxcell[6]);
                    textboxcell[7].Value = (int)ConfigHandle.Instance.AxisDefine.AxisList[i].HomeSpeedFast;
                    row.Cells.Add(textboxcell[7]);
                    textboxcell[8].Value = (int)ConfigHandle.Instance.AxisDefine.AxisList[i].HomeSpeedSlow;
                    row.Cells.Add(textboxcell[8]);
                    /////////////////////////////////////////////////////////////////////////////
                    comboxHome.Value = listHome[ConfigHandle.Instance.AxisDefine.AxisList[i].HomeMode].Index;
                    row.Cells.Add(comboxHome);
                    textboxcell[9].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].OrgNum.ToString();
                    row.Cells.Add(textboxcell[9]);
                    combox[0].Value = listlev[ConfigHandle.Instance.AxisDefine.AxisList[i].Orglev].Index;//
                    row.Cells.Add(combox[0]);

                    textboxcell[10].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].PositiveLimitPostion;
                    row.Cells.Add(textboxcell[10]);
                    textboxcell[11].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].Poslimit.ToString();
                    row.Cells.Add(textboxcell[11]);
                    combox[1].Value = listlev[ConfigHandle.Instance.AxisDefine.AxisList[i].Poslimitlev].Index;
                    row.Cells.Add(combox[1]);

                    textboxcell[12].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].NegativeLimitPostion;
                    row.Cells.Add(textboxcell[12]);
                    textboxcell[13].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].Neglimit.ToString();
                    row.Cells.Add(textboxcell[13]);
                    combox[2].Value = listlev[ConfigHandle.Instance.AxisDefine.AxisList[i].Neglimitlev].Index;
                    row.Cells.Add(combox[2]);

                    comboxLimit.Value = Limit[ConfigHandle.Instance.AxisDefine.AxisList[i].limitMode].Index;
                    row.Cells.Add(comboxLimit);
                    combox[3].Value = listlev[ConfigHandle.Instance.AxisDefine.AxisList[i].alarmmode].Index;
                    row.Cells.Add(combox[3]);

                    textboxcell[14].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].HomeOffset;
                    row.Cells.Add(textboxcell[14]);
                    textboxcell[15].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].LeaderPer;
                    row.Cells.Add(textboxcell[15]);
                    textboxcell[16].Value = ConfigHandle.Instance.AxisDefine.AxisList[i].PulsePer;
                    row.Cells.Add(textboxcell[16]);

                    this.dataGridViewMachinePrm.Rows.Add(row);
                    if (i % 2 == 0)
                    {
                        this.dataGridViewMachinePrm.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SavePrmData()
        {
            if (dataGridViewMachinePrm.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridViewMachinePrm.Rows.Count; i++)
                {
                    DataGridViewCell dataGridViewCell_1 = this.dataGridViewMachinePrm.Rows[i].Cells[0];
                    DataGridViewCell dataGridViewCell_2 = this.dataGridViewMachinePrm.Rows[i].Cells[1];
                    DataGridViewCell dataGridViewCell_3 = this.dataGridViewMachinePrm.Rows[i].Cells[2];
                    DataGridViewCell dataGridViewCell_4 = this.dataGridViewMachinePrm.Rows[i].Cells[3];
                    DataGridViewCell dataGridViewCell_5 = this.dataGridViewMachinePrm.Rows[i].Cells[4];
                    DataGridViewCell dataGridViewCell_6 = this.dataGridViewMachinePrm.Rows[i].Cells[5];
                    DataGridViewCell dataGridViewCell_7 = this.dataGridViewMachinePrm.Rows[i].Cells[6];
                    DataGridViewCell dataGridViewCell_8 = this.dataGridViewMachinePrm.Rows[i].Cells[7];
                    DataGridViewCell dataGridViewCell_9 = this.dataGridViewMachinePrm.Rows[i].Cells[8];
                    DataGridViewCell dataGridViewCell_10 = this.dataGridViewMachinePrm.Rows[i].Cells[9];
                    DataGridViewCell dataGridViewCell_11 = this.dataGridViewMachinePrm.Rows[i].Cells[10];
                    DataGridViewCell dataGridViewCell_12 = this.dataGridViewMachinePrm.Rows[i].Cells[11];
                    DataGridViewCell dataGridViewCell_13 = this.dataGridViewMachinePrm.Rows[i].Cells[12];
                    DataGridViewCell dataGridViewCell_14 = this.dataGridViewMachinePrm.Rows[i].Cells[13];
                    DataGridViewCell dataGridViewCell_15 = this.dataGridViewMachinePrm.Rows[i].Cells[14];
                    DataGridViewCell dataGridViewCell_16 = this.dataGridViewMachinePrm.Rows[i].Cells[15];
                    DataGridViewCell dataGridViewCell_17 = this.dataGridViewMachinePrm.Rows[i].Cells[16];
                    DataGridViewCell dataGridViewCell_18 = this.dataGridViewMachinePrm.Rows[i].Cells[17];
                    DataGridViewCell dataGridViewCell_19 = this.dataGridViewMachinePrm.Rows[i].Cells[18];
                    DataGridViewCell dataGridViewCell_20 = this.dataGridViewMachinePrm.Rows[i].Cells[19];
                    DataGridViewCell dataGridViewCell_21 = this.dataGridViewMachinePrm.Rows[i].Cells[20];
                    DataGridViewCell dataGridViewCell_22 = this.dataGridViewMachinePrm.Rows[i].Cells[21];
                    DataGridViewCell dataGridViewCell_23 = this.dataGridViewMachinePrm.Rows[i].Cells[22];


                    //表格中数据与机械参数数据对应
                    ConfigHandle.Instance.AxisDefine.AxisList[i].Name = dataGridViewCell_2.Value.ToString();
                    ConfigHandle.Instance.AxisDefine.AxisList[i].StartSpeed =  Convert.ToSingle(dataGridViewCell_3.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].AccSpeed = Convert.ToSingle(dataGridViewCell_4.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].RunSpeed =  Convert.ToSingle(dataGridViewCell_5.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].DecSpeed = Convert.ToSingle(dataGridViewCell_6.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].EndSpeed =  Convert.ToSingle(dataGridViewCell_7.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].HomeSpeedFast =  Convert.ToSingle(dataGridViewCell_8.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].HomeSpeedSlow =  Convert.ToSingle(dataGridViewCell_9.Value);

                    ConfigHandle.Instance.AxisDefine.AxisList[i].HomeMode = (short)Convert.ToSingle(dataGridViewCell_10.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].OrgNum = (short)Convert.ToSingle(dataGridViewCell_11.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].Orglev = (short)Convert.ToSingle(dataGridViewCell_12.Value);

                    ConfigHandle.Instance.AxisDefine.AxisList[i].PositiveLimitPostion = Convert.ToSingle(dataGridViewCell_13.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].Poslimit = (short)Convert.ToSingle(dataGridViewCell_14.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].Poslimitlev = (short)Convert.ToSingle(dataGridViewCell_15.Value);

                    ConfigHandle.Instance.AxisDefine.AxisList[i].NegativeLimitPostion = Convert.ToSingle(dataGridViewCell_16.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].Neglimit = (short)Convert.ToSingle(dataGridViewCell_17.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].Neglimitlev = (short)Convert.ToSingle(dataGridViewCell_18.Value);

                    ConfigHandle.Instance.AxisDefine.AxisList[i].limitMode = (short)Convert.ToSingle(dataGridViewCell_19.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].alarmmode = (short)Convert.ToSingle(dataGridViewCell_20.Value);

                    ConfigHandle.Instance.AxisDefine.AxisList[i].HomeOffset = Convert.ToSingle(dataGridViewCell_21.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].LeaderPer = Convert.ToSingle(dataGridViewCell_22.Value);
                    ConfigHandle.Instance.AxisDefine.AxisList[i].PulsePer = Convert.ToInt32(dataGridViewCell_23.Value);

                }
            }
        }

		public void DownMotorPrmToSlave()
		{
			for (int i = 0; i < ConfigHandle.Instance.AxisDefine.AxisList.Count; i++)
			{
				int number = ConfigHandle.Instance.AxisDefine.AxisList[i].Num - 1;

				List<byte> list = new List<byte>();
				list.Clear();
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].StartSpeed));
				list.AddRange(Functions.NetworkBytes((int)ConfigHandle.Instance.AxisDefine.AxisList[i].AccSpeed));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].RunSpeed));
				list.AddRange(Functions.NetworkBytes((int)ConfigHandle.Instance.AxisDefine.AxisList[i].DecSpeed));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].EndSpeed));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].HomeSpeedFast));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].HomeSpeedSlow));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].HomeOffset));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].NegativeLimitPostion));
				list.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].PositiveLimitPostion));

				ushort addr = (ushort)(2000 + number * 20);
				movedriverZm.WriteRegister(new BaseData(addr, list.ToArray()));

				ushort addr_IO = (ushort)(3200 + number * 10);
				movedriverZm.WriteRegister(new BaseData(addr_IO, new ushort[] { 
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].limitMode,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].Poslimit,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].Poslimitlev,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].Neglimit,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].Neglimitlev,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].OrgNum,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].Orglev,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].HomeMode,
                    (ushort)ConfigHandle.Instance.AxisDefine.AxisList[i].alarmmode
                }));

				List<byte> listPulse = new List<byte>();
				listPulse.Clear();
				listPulse.AddRange(Functions.NetworkBytes((int)ConfigHandle.Instance.AxisDefine.AxisList[i].PulsePer));
				listPulse.AddRange(Functions.NetworkBytes(ConfigHandle.Instance.AxisDefine.AxisList[i].LeaderPer));

				ushort addr_PPR = (ushort)(3000 + number * 4);
				movedriverZm.WriteRegister(new BaseData(addr_PPR, listPulse.ToArray()));

			}
		}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ConfigHandle.Instance.AxisDefine.AxisList.Count == 0)
            {
                MessageBox.Show("没有区域", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("确认删除", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    SavePrmData();
                    int num = (int)numericUpDown1.Value;
                    for (int i = 0; i < ConfigHandle.Instance.AxisDefine.AxisList.Count; i++)
                    {
                        if (num == ConfigHandle.Instance.AxisDefine.AxisList[i].Num)
                        {
                            ConfigHandle.Instance.AxisDefine.AxisList.RemoveAt(i);
                        }
                    }
                    LoadPrmData();                  
                }
            }
        }

        private void UserMachinePrm_Load(object sender, EventArgs e)
        {
            InitializeDataSources();
            LoadPrmData();
        }
    }
}
