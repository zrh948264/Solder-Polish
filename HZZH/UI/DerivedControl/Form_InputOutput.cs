using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common;
using Config;

namespace MyControl
{
    public partial class InputOutput: Form,IDisposable
    {
        public InputOutput()
        {
            InitializeComponent();
            Initializel();
        }
        Color clrSignalOFF = Color.Gray;
        Color clrSignalON = Color.Green;
        void Initializel()
        {
            dataGridViewIN.ColumnCount = 2;
            dataGridViewIN.Columns[0].Name = "输入";
            dataGridViewIN.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Blue;
            dataGridViewIN.Columns[0].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridViewIN.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridViewIN.Columns[1].Name = "状态";
            dataGridViewIN.Columns[1].DefaultCellStyle.ForeColor = clrSignalOFF;
            dataGridViewIN.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewIN.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewIN.Columns[1].DefaultCellStyle.Font = new Font(this.dataGridViewOUT.Font.FontFamily, 10, FontStyle.Bold);
            dataGridViewIN.Columns[1].Width = 50;
            dataGridViewIN.Columns[1].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridViewIN.Columns[1].DefaultCellStyle.SelectionForeColor = Color.Blue;

            for (int i = 0; i < ConfigHandle.Instance.InputDefine.InputNamelist.Count; i++)
            {
                dataGridViewIN.Rows.Add(new string[] { ConfigHandle.Instance.InputDefine.InputNamelist[i], "off" });
            }

            dataGridViewOUT.ColumnCount = 2;
            dataGridViewOUT.Columns[0].Name = "输出";
            dataGridViewOUT.Columns[0].ReadOnly = true;
            dataGridViewOUT.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewOUT.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewOUT.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewOUT.Columns[0].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridViewOUT.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Blue;

            dataGridViewOUT.Columns[1].Name = "状态";
            dataGridViewOUT.Columns[1].DefaultCellStyle.ForeColor = clrSignalOFF;
            dataGridViewOUT.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewOUT.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewOUT.Columns[1].DefaultCellStyle.Font = new Font(this.dataGridViewOUT.Font.FontFamily, 10, FontStyle.Bold);
            dataGridViewOUT.Columns[1].Width = 50;
            dataGridViewOUT.Columns[1].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dataGridViewOUT.Columns[1].DefaultCellStyle.SelectionForeColor = Color.Blue;

            for (int i = 0; i < ConfigHandle.Instance.OutputDefine.OutputNamelist.Count; i++)
            {
                dataGridViewOUT.Rows.Add(new string[] { ConfigHandle.Instance.OutputDefine.OutputNamelist[i], "OFF" });
            }
        }

        private Device.BoardCtrllerManager movedriverZm;
        public void SetMoveController(Device.BoardCtrllerManager movedriverZm)
        {
            this.movedriverZm = movedriverZm;
			DIDOStatus_ValueChanged(null, null);
            DiDoStatus.ValueChanged += DIDOStatus_ValueChanged;
        }



        void DIDOStatus_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int which, index;
            for (int i = 0; i < ConfigHandle.Instance.InputDefine.InputBit.Count; i++)
            {
                if (ConfigHandle.Instance.InputDefine.InputBit[i] >= 32)
                {
                    which = ConfigHandle.Instance.InputDefine.InputBit[i] / 32;
                    index = ConfigHandle.Instance.InputDefine.InputBit[i] % 32;
                }
                else
                {
                    which = 0;
                    index = ConfigHandle.Instance.InputDefine.InputBit[i] % 32;
                }
                if ((DiDoStatus.CurrInputStatus[which] & (1 << index)) == 0)
                {
                    dataGridViewIN.Rows[i].Cells[1].Value = "ON";
                    dataGridViewIN.Rows[i].Cells[1].Style.ForeColor = clrSignalON;
                }
                else
                {
                    dataGridViewIN.Rows[i].Cells[1].Value = "OFF";
                    dataGridViewIN.Rows[i].Cells[1].Style.ForeColor = clrSignalON;
                }
            }
            for (int i = 0; i < ConfigHandle.Instance.OutputDefine.OutputBit.Count; i++)
            {
                if (ConfigHandle.Instance.OutputDefine.OutputBit[i] >= 32)
                {
                    which = ConfigHandle.Instance.OutputDefine.OutputBit[i] / 32;
                    index = ConfigHandle.Instance.OutputDefine.OutputBit[i] % 32;
                }
                else
                {
                    which = 0;
                    index = ConfigHandle.Instance.OutputDefine.OutputBit[i] % 32;
                }

                if ((DiDoStatus.CurrOutputStatus[which] & (1 << index)) == 0)
                {
                    dataGridViewOUT.Rows[i].Cells[1].Value = "ON";
                    dataGridViewOUT.Rows[i].Cells[1].Style.ForeColor = clrSignalON;
                }
              //  else if ()
               // {
                    
               // }
                else
                {
                    dataGridViewOUT.Rows[i].Cells[1].Value = "OFF";
                    dataGridViewOUT.Rows[i].Cells[1].Style.ForeColor = clrSignalOFF;
                }
            }
        }

        private int[] Output = new int[40];

        private void dataGridViewOUT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Output = (int[])DiDoStatus.CurrOutputStatus.Clone();
            int rowindex = e.RowIndex;
            int columnindex = e.ColumnIndex;
            if (rowindex < 0 || columnindex < 0) return;
            int which, index;
            if (ConfigHandle.Instance.OutputDefine.OutputBit[rowindex] >= 32)
            {
                which = ConfigHandle.Instance.OutputDefine.OutputBit[rowindex] / 32;
                index = ConfigHandle.Instance.OutputDefine.OutputBit[rowindex] % 32;

            }
            else
            {
                which = 0;
                index = ConfigHandle.Instance.OutputDefine.OutputBit[rowindex] % 32;
            }
            if ((Output[which] & (int)(0x0001 << (index))) > 0)
            {
                Output[which] &= (((int)~(0x0001 << (index))));
            }
            else
            {
                Output[which] |= (int)(0x0001 << (index));
            }
            movedriverZm.WriteRegister(new BaseData(1020, Output));
        }


        new public void Dispose()
        {
            DiDoStatus.ValueChanged -= DIDOStatus_ValueChanged;
        }
    }
}
