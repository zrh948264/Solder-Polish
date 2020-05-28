using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    #region 将加载数据发送到启动界面里面
    public static class StartUpdate
    {
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void SendStartMsgEventHandler(SendCmdArgs e);
        /// <summary>
        /// 定义一个发送消息的事件
        /// </summary>
        public static event SendStartMsgEventHandler StartMsg;
        /// <summary>
        /// 触发消息的函数
        /// </summary>
        /// <param name="e"></param>
        public static bool SendStartMsg(string strRecieve)
        {
            if (strRecieve != "")
            {
                if (StartMsg != null)
                {
                    SendCmdArgs e = new SendCmdArgs(strRecieve);
                    StartMsg(e);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public class SendCmdArgs : EventArgs
    {
        private string m_StrRecieve;
        public SendCmdArgs(string strRecieve)
        {
            this.m_StrRecieve = strRecieve;
        }
        /// <summary>
        /// 服务器接收到的数据
        /// </summary>
        public string StrReciseve { set { m_StrRecieve = value; } get { return m_StrRecieve; } }

    }

    public static class ShowMessge
    {
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void SendStartMsgEventHandler(SendCmdArgs e);
        /// <summary>
        /// 定义一个发送消息的事件
        /// </summary>
        public static event SendStartMsgEventHandler StartMsg;
        /// <summary>
        /// 触发消息的函数
        /// </summary>
        /// <param name="e"></param>
        public static bool SendStartMsg(string strRecieve)
        {
            if (strRecieve != "")
            {
                if (StartMsg != null)
                {
                    SendCmdArgs e = new SendCmdArgs(strRecieve);
                    StartMsg(e);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    #endregion
}
