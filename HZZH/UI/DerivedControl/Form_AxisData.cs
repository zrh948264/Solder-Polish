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
using Device;

namespace HZZH.UI.DerivedControl
{
    public partial class Form_AxisData : Form
    {
        public Form_AxisData()
        {
            InitializeComponent();
        }

        public BoardCtrllerManager _movedriverZm = new BoardCtrllerManager();//板卡
        public List<Axis> _AxisList;//轴参数

        public void SetAxisList(BoardCtrllerManager movedriverZm,List<Axis> AxisList)
        {
            this._movedriverZm = movedriverZm;
            this._AxisList = AxisList;


            LoadtreeView1();
        }

        #region 轴参数显示

        int RowCount = -1;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RowCount = this.treeView1.SelectedNode.Index;

            propertyGrid1.SelectedObject = _AxisList[RowCount];
        }

        #endregion

        #region 显示轴
        private void LoadtreeView1()
        {
            int count = 0;
            this.treeView1.Nodes.Clear();
            foreach (Axis p in _AxisList)
            {
                count++;
                this.treeView1.Nodes.Add(new TreeNode("轴" + count.ToString()+":"+ p.Name.ToString ()));
            }



        }

        #endregion


    }
}
