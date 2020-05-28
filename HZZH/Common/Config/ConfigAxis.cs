using System;
using System.Collections.Generic;
using Common;

namespace Config
{
    [Serializable]
    public   class ConfigAxis : Config
    {
        private static string _name = "Axis.config";
        //用户配置文件的相对路径
        private static string _configName = string.Format("{0}", ConfigDirectory + "\\" + _name);

        public ConfigAxis()
        {
            AxisList = new List<Axis>();
        }
        /// <summary>
        /// 属性：轴机械参数列表实体(用于实体删减)
        /// </summary>
        public List<Axis>  AxisList { set; get; }



        /// <summary>
        /// 方法：保存轴机械参数配置
        /// </summary>
        public void Save()
        {
            try
            {
                Serialization.SaveToFile(this, _configName);
            }
            catch (Exception ex)
            {
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：保存配置文件[{0}]失败!\n异常描述:{1}\n时间：{2}", _name, ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
                throw ex;
            }
        }

        /// <summary>
        /// 方法：加载轴机械参数配置
        /// </summary>
        /// <returns></returns>
        public static ConfigAxis Load()
        {
            try
            {
                ConfigAxis rt = (ConfigAxis)Serialization.LoadFromFile(typeof(ConfigAxis), _configName);
                return rt;
            }
            catch (Exception ex)
            {
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：加载配置文件[{0}]失败!\n异常描述:{1}\n时间：{2}", _name, ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
                throw new LoadException(_name, ex.Message);
            }
        }


    }
}
