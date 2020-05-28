using System;
using System.Collections.Generic;
using System.Linq;
using Common;



namespace Config
{
    [Serializable]
    public class ConfigUser : Config
    {

        private static string _name = "User.config";
        //用户配置文件的相对路径
        private static string _configName = string.Format("{0}", ConfigDirectory + "\\" + _name);

        public ConfigUser()
        {
            UserList = new List<User>();
        }

        /// <summary>
        /// 属性：用户列表实体(用于实体删减+查询)
        /// </summary>
        public List<User> UserList { set; get; }


        /// <summary>
        /// 方法：保存用户配置
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
        /// 方法：加载用户配置
        /// </summary>
        /// <returns></returns>
        public static ConfigUser Load()
        {
            try
            {
                ConfigUser rt = (ConfigUser)Serialization.LoadFromFile(typeof(ConfigUser), _configName);
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
