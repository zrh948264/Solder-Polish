namespace userListView
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UserCtrlMsgListView : UserControl
    {
        private object olock = new object();
        private IContainer components = null;
        private ListView listView1;

        public UserCtrlMsgListView()
        {
            this.InitializeComponent();
            this.InitializeListView_Log();
        }

        public void AddUserMsg(string msg, string MsgLevel)
        {
            object olock = this.olock;
            lock (olock)
            {
                ListViewItem item = new ListViewItem();
                int count = this.listView1.Items.Count;
                item.Text = count.ToString();
                item.SubItems.Add(DateTime.Now.ToString());
                item.SubItems.Add(MsgLevel);
                item.SubItems.Add(msg);
                if ((count % 2) > 0)
                {
                    item.BackColor = Color.PaleTurquoise;
                }
                this.listView1.Items.Add(item);
                item.EnsureVisible();
            }
        }

        public void ClearMsgItems()
        {
            this.listView1.Items.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listView1 = new ListView();
            base.SuspendLayout();
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new Point(0, 0);
            this.listView1.Margin = new Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x1f1, 290);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.Controls.Add(this.listView1);
            base.Name = "UserCtrlMsgListView";
            base.Size = new Size(0x1f1, 290);
            base.ResumeLayout(false);
        }

        private void InitializeListView_Log()
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("序号", 40, HorizontalAlignment.Center);
            this.listView1.Columns.Add("时间", 130, HorizontalAlignment.Center);
            this.listView1.Columns.Add("类型", 70, HorizontalAlignment.Center);
            this.listView1.Columns.Add("内容", 600, HorizontalAlignment.Left);
        }
    }
}

