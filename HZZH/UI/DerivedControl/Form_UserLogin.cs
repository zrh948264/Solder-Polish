using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using Common;
using Config;

namespace MyControl
{
    public partial class UserLogin: Form
    {
        public UserLogin()
        {            
            InitializeComponent();
        }

        User currentuser;

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Password.Text.Trim() == "")
                {
                    MessageBox.Show("请输入密码...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_Password.Focus();
                    return;
                }
                for (int i = 0; i < ConfigHandle.Instance.UserDefine.UserList.Count; i++)
                {
                    if (ConfigHandle.Instance.UserDefine.UserList[i].Name.Equals(comboBox1.Text) && 
                        ConfigHandle.Instance.UserDefine.UserList[i].PassWord.Equals(GetSHA1HashData(txt_Password.Text)))
                    {
                        currentuser = ConfigHandle.Instance.UserDefine.UserList[i];
                    }
                }

                if (currentuser != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("无效登录信息,用户名或密码错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("用户名或密码错误，请输入正确的用户名和密码");
            }
        }

        private string GetSHA1HashData(string data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            //return "641890219989519581101501581612559294203219190239";
            return returnValue.ToString();
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            currentuser = null;
            this.Close();
        }

        public User GetCurrentUser()
        {
            return currentuser;
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < ConfigHandle.Instance.UserDefine.UserList.Count; i++)
            {
                if (ConfigHandle.Instance.UserDefine.UserList[i].Type != "2" && ConfigHandle.Instance.UserDefine.UserList[i].Type != "3")
                {
                    comboBox1.Items.Add(ConfigHandle.Instance.UserDefine.UserList[i].Name);
                }
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

    }
}
