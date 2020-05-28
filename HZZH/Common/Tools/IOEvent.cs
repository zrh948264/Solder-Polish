using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    #region IO刷新

    /// <summary>
    /// 数据变更触发事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="e">事件</param>
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    /// <summary>
    /// 静态类绑定IO值
    /// </summary>
    public static class DiDoStatus
    {
        public static event ValueChangedEventHandler ValueChanged;
        static int[] lb_InputStatus = new int[40];
        static int[] lb_OutputStatus = new int[40];
        /// <summary>
        /// Input参数
        /// </summary>
        public static int[] CurrInputStatus
        {
            get
            {
                return lb_InputStatus;
            }

            set
            {
                if (compareArr(lb_InputStatus, value))
                {
                    Buffer.BlockCopy(value, 0, lb_InputStatus, 0, value.Length * 4);
                    if (ValueChanged != null)
                        ValueChanged.Invoke(null, new ValueChangedEventArgs("IntputStatus", lb_InputStatus));
                }
            }
        }
        /// <summary>
        /// Output参数
        /// </summary>
        public static int[] CurrOutputStatus
        {
            get
            { return lb_OutputStatus; }

            set
            {
                if (compareArr(lb_OutputStatus, value))
                {
                    Buffer.BlockCopy(value, 0, lb_OutputStatus, 0, value.Length * 4);
                    if (ValueChanged != null)
                        ValueChanged.Invoke(null, new ValueChangedEventArgs("OutputStatus", lb_OutputStatus));
                }

            }
        }

        private static bool compareArr(int[] arr1, int[] arr2)
        {
            if (arr1 == null)
            {
                arr1 = arr2;
                return true;
            }

            for (int i = 0; i < arr2.Length; i++)
            {
                int f = arr1[i] - arr2[i];
                if ((arr1[i] - arr2[i]) != 0)
                {
                    int gg = arr1[i];
                    int gg1 = arr2[i];
                    return true;
                }
            }

            return false;

        }
    }

    /// <summary>
    /// 事件执行
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        public string propertyName;

        public object newValue;

        /// <summary>
        /// This is a constructor.
        /// Add parameter and property as needed for more values in event args.
        /// </summary>
        public ValueChangedEventArgs(string propertyName, object newValue)
        {
            this.propertyName = propertyName;

            this.newValue = newValue;
        }
    }

    #endregion

}
