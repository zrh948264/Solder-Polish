using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using SqliteDataBase;

namespace Config
{
    /// <summary>
    /// 配置文件统一操作类
    /// </summary>
    public class ConfigHandle : Config
    {
        #region Singleton

        static object _syncObj = new object();
        static ConfigHandle _instance;
        public static ConfigHandle Instance
        {
            get
            {
                lock (_syncObj)
                {
                    if (_instance == null)
                    { _instance = new ConfigHandle(); }
                }

                return _instance;
            }
        }

        #endregion                     

        /// <summary>
        /// 属性:系统配置
        /// </summary>
        public ConfigSystem SystemDefine { set; get; }

        /// <summary>
        /// 属性:用户配置
        /// </summary>
        public ConfigUser UserDefine { set; get; }

        /// <summary>
        /// 属性:报警配置
        /// </summary>
        public ConfigAlarm AlarmDefine { set; get; }

        /// <summary>
        /// 属性:报警配置
        /// </summary>
        public ConfigInput InputDefine { set; get; }

        /// <summary>
        /// 属性:报警配置
        /// </summary>
        public ConfigOutput OutputDefine { set; get; }


        /// <summary>
        /// 属性：机械参数配置
        /// </summary>
        public ConfigAxis AxisDefine { set; get; }

        /// <summary>
        /// 加载所有配置
        /// </summary>
        public void Load()
        {
            try
            {
                #region 加载配置
                this.AlarmDefine = new ConfigAlarm();
                this.InputDefine = new ConfigInput();
                this.OutputDefine = new ConfigOutput();
                
                this.SystemDefine = ConfigSystem.Load();
                this.UserDefine = ConfigUser.Load();
                this.AxisDefine = ConfigAxis.Load();


                // LoadEquipmentConfig();
                #endregion
            }
            catch (Exception ex)
            {
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：加载配置文件失败!\n异常描述:{0}\n时间：{1}", ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }

        public void Save()
        {
            try
            {
                #region 保存配置

                this.SystemDefine.Save();
                this.UserDefine.Save();
                this.AxisDefine.Save();

                //SaveEquipmentConfig();
                #endregion
            }
            catch (Exception ex)
            {
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：加载配置文件失败!\n异常描述:{0}\n时间：{1}", ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }


        #region 数据库存储

        private static string _name = "EquipmentConfig";
        //报警配置文件的相对路径
        private static string _configName = string.Format("{0}", ConfigDirectory + "\\" + _name);
        /// <summary>
        /// 系统数据
        /// </summary>
        string[] sqlSystemData = { "SystemTypes", "Data1", "Data2", "Data3", "Data4" };
        /// <summary>
        /// 系统数据类型
        /// </summary>
        string[] sqlSystemDataTyp = { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" };
        /// <summary>
        /// 系统配置数据库(包含:平台，通讯，相机,高级设置)
        /// </summary>
        public static AppSqliteConfigure SysDataManage;

        private void LoadEquipmentConfig()
        {
            AppSqliteConfigure.mDataSource = _configName;
            AppSqliteConfigure.Connected();
            try
            {
                //*********************   创建系统配置表(包含:平台配置参数，通讯配置参数，高级设置参数)
                SysDataManage = new AppSqliteConfigure("SystemTb");
                SysDataManage.CreatTable_DB(sqlSystemData, sqlSystemDataTyp);
                List<string> allData = new List<string>();
                SysDataManage.ReadallDataFromSqliteTable_1("SystemTypes", ref allData);
                bool _needCreate = false;
                allData.Distinct();

                if ( !allData.Contains("ConfigUser") && !allData.Contains("ConfigAxis") && !allData.Contains("ConfigSystem")) 
                {
                    _needCreate = true;
                }
                else
                {
                    if (!allData.Contains("ConfigUser"))
                    {
                        SysDataManage.InsertOneDataOfSqliteTable_1(new string[] { "ConfigUser", "", "", "", "" });
                    }
                    if (!allData.Contains("ConfigAxis"))
                    {
                        SysDataManage.InsertOneDataOfSqliteTable_1(new string[] { "ConfigAxis", "", "", "", "" });
                    }
                    if (!allData.Contains("ConfigSystem"))
                    {
                        SysDataManage.InsertOneDataOfSqliteTable_1(new string[] { "ConfigSystem", "", "", "", "" });
                    }
                }
                if (_needCreate)
                {
                    //******************   首次默认插入3行数据 ---- 第一位为配置类型名，第二位为详细配置参数
                    SysDataManage.InsertOneDataOfSqliteTable_1(new string[] { "ConfigUser", "", "", "", "" });
                    SysDataManage.InsertOneDataOfSqliteTable_1(new string[] { "ConfigAxis", "", "", "", "" });
                    SysDataManage.InsertOneDataOfSqliteTable_1(new string[] { "ConfigSystem", "", "", "", "" });

                    //******************   对象序列化成JSON
                    UserDefine = new ConfigUser();
                    string UserJson = JsonHelper.SerializeObject(UserDefine);
                    AxisDefine = new ConfigAxis();
                    string AxisJson = JsonHelper.SerializeObject(AxisDefine);
                    SystemDefine = new ConfigSystem();
                    string SystemJson = JsonHelper.SerializeObject(SystemDefine);

                    //******************   更新数据库
                    SysDataManage.UpdateOneDataOfSqliteTable_1("SystemTypes", "ConfigUser", "Data1", UserJson);
                    SysDataManage.UpdateOneDataOfSqliteTable_1("SystemTypes", "ConfigAxis", "Data1", AxisJson);
                    SysDataManage.UpdateOneDataOfSqliteTable_1("SystemTypes", "ConfigSystem", "Data1", SystemJson);

                    _needCreate = false;
                }
                //**********************   检查数据库信息
                List<string> platMsg = new List<string>();
                SysDataManage.ReadallDataFromSqliteTable_1("SystemTypes", ref platMsg);
                foreach (string item in platMsg)
                {
                    string DesCriptionMsg = "";
                    SysDataManage.ReadOneDataFromSqliteTable_1("SystemTypes", item, "Data1", ref DesCriptionMsg);
                    if (DesCriptionMsg != "")
                    {
                        if (item == "ConfigSystem")
                        {
                            SystemDefine = JsonHelper.DeserializeJsonToObject<ConfigSystem>(DesCriptionMsg);
                            StartUpdate.SendStartMsg("系统配置加载成功");
                        }
                        else if (item == "ConfigUser")
                        {
                            UserDefine = JsonHelper.DeserializeJsonToObject<ConfigUser>(DesCriptionMsg);
                            StartUpdate.SendStartMsg("用户配置加载成功");
                        }
                        else if (item == "ConfigAxis")
                        {
                            AxisDefine = JsonHelper.DeserializeJsonToObject<ConfigAxis>(DesCriptionMsg);
                            StartUpdate.SendStartMsg("轴配置加载成功");
                        }
                    }
                    else
                    {
                        StartUpdate.SendStartMsg(item+"配置加载失败");
                        LogWriter.WriteLog(string.Format("错误：加载配置文件失败!\n异常描述:{0}\n时间：{1}", item, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
                    }
                }
            }
            catch (Exception ex)
            {
                StartUpdate.SendStartMsg("设备配置加载错误");
                LogWriter.WriteLog(string.Format("错误：设备配置加载错误!\n异常描述:{0}\n时间：{1}", ex.ToString(), System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }


        public void SaveEquipmentConfig()
        {
            try
            {
                if (SysDataManage != null)
                {
                    //******************   对象序列化成JSON
                    string SystemJson = JsonHelper.SerializeObject(SystemDefine);
                    string UserJson = JsonHelper.SerializeObject(UserDefine);
                    string AxisJson = JsonHelper.SerializeObject(AxisDefine);
                    //******************   更新数据库
                    SysDataManage.UpdateOneDataOfSqliteTable_1("SystemTypes", "ConfigSystem", "Data1", SystemJson);
                    SysDataManage.UpdateOneDataOfSqliteTable_1("SystemTypes", "ConfigUser", "Data1", UserJson);
                    SysDataManage.UpdateOneDataOfSqliteTable_1("SystemTypes", "ConfigAxis", "Data1", AxisJson);
                }
            }
            catch (InvalidOperationException ex)
            {
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：加载配置文件失败!\n异常描述:{0}\n时间：{1}", ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }
        #endregion


        #region 文件储存


        #endregion

    }
}
