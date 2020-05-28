using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{


    #region 用户
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class User : System.ComponentModel.INotifyPropertyChanged
    {
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public User() { }

        /// <summary>
        /// 属性:用户名称(作为唯一标识符)
        /// </summary>
        public string Name { set; get; }
        public string Type { get; set; }
        public string PassWord { set; get; }
        public string CreatedOn { get; set; }

        public User(string Name_, string Pwd_, string Type_)
        {
            Name = Name_;
            PassWord = Pwd_;
            Type = Type_;
            CreatedOn = DateTime.UtcNow.ToString();
        }
    }

    public class UserMode
    {
        public int Index { get; set; }//有效值
        public string Name { get; set; }

        public UserMode()
        {
            Index = 0;
            Name = "";
        }
    }

    #endregion 

}
