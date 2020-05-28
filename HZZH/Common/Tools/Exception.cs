using System;

namespace Common
{
    /// <summary>
    ///  初始化异常类
    /// </summary>
    public class InitException : System.Exception
    {
        /// <summary>
        /// 字段：设备
        /// </summary>
        private string _device;

        /// <summary>
        /// 字段：原因
        /// </summary>
        private string _reason;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>
        public InitException(string device, string reason)
            : base(reason)
        {
            this._device = device;
            this._reason = reason;
        }
        /// <summary>
        /// 覆写：ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("设备[{0}]初始化时发生错误:{1}", _device, _reason);
        }

    }

    /// <summary>
    /// 启动异常类
    /// </summary>
    public class StartException : System.Exception
    {
        /// <summary>
        /// 字段：设备
        /// </summary>
        private string _device;

        /// <summary>
        /// 字段：原因
        /// </summary>
        private string _reason;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>
        public StartException(string device, string reason)
            : base(reason)
        {
            this._device = device;
            this._reason = reason;
        }
        /// <summary>
        /// 覆写：ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("设备[{0}]启动时发生错误:{1}", _device, _reason);
        }

    }

    /// <summary>
    /// 停止异常类
    /// </summary>
    public class StopException : System.Exception
    {
        /// <summary>
        /// 字段：设备
        /// </summary>
        private string _device;

        /// <summary>
        /// 字段：原因
        /// </summary>
        private string _reason;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>
        public StopException(string device, string reason)
            : base(reason)
        {
            this._device = device;
            this._reason = reason;
        }
        /// <summary>
        /// 覆写：ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("设备[{0}]停止时发生错误:{1}", _device, _reason);
        }

    }

    /// <summary>
    /// 释放异常类
    /// </summary>
    public class ReleaseException : System.Exception
    {
        /// <summary>
        /// 字段：设备
        /// </summary>
        private string _device;

        /// <summary>
        /// 字段：原因
        /// </summary>
        private string _reason;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>
        public ReleaseException(string device, string reason)
            : base(reason)
        {
            this._device = device;
            this._reason = reason;
        }
        /// <summary>
        /// 覆写：ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("设备[{0}]释放时发生错误:{1}", _device, _reason);
        }

    }

    /// <summary>
    /// 加载异常类
    /// </summary>
    public class LoadException : Exception
    {
        private string _configName;
        private string _reason;

        public LoadException(string configName, string reason)
            : base(reason)
        {
            this._configName = configName;
            this._reason = reason;
        }

        /// <summary>
        /// 覆写方法：Exception的ToString()方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("配置[{0}]加载时发生错误:{1}", this._configName, this._reason);
        }
    }
}
