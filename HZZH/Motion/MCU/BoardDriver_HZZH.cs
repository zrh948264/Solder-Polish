using System;
using Common;
using HZZH.Card;



namespace Driver
{
    public class BoardDriver_HZZH
    {
        Controller hzaux = new Controller();


        #region 实现抽象函数
        public  bool DoConnectCtrller(string ip,int port)
        {
            bool rt = false;
            try
            {
                hzaux.HZ_InitializeUdp(ip, port);
                rt = true;
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡链接失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }

            return rt;
        }

        public  bool DoIsConnected()
        {
            return hzaux.Succeed;
        }

        public  bool DoDisconnectCtrller()
        {
            bool rt = false;
            try
            {
                if (hzaux != null)
                {
                    hzaux.Dispose();
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡关闭失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss"))); }

            return rt;
        }

        /// <summary>
        /// 设定轴限位模式
        /// </summary>
        /// <param name="limitMode">限位模式参数：0无限位，1软限位，2硬限位，3软硬都限位</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoLimitMode(ushort limitMode, ushort naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetLimitMode(limitMode, naxis) == 0) {
                    rt = true; 
                }
               
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设定轴限位模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
 
        }

        /// <summary>
        /// 设置轴正限位信号和电平
        /// </summary>
        /// <param name="poslimit">正限位信号：配置限位端口，0、1、2..........等</param>
        /// <param name="poslimitlev">正限位信号电平：0低电平，1高电平</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoPosLimitlevel(ushort poslimit, ushort poslimitlev, ushort naxis){
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetPosLimitlevel(poslimit, poslimitlev,naxis) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴正限位信号和电平失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置轴负限位信号和电平
        /// </summary>
        /// <param name="neglimit">负限位信号：配置限位端口，0、1、2..........等</param>
        /// <param name="neglimitlev">负限位信号电平：0低电平，1高电平</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoNegsLimitlevel(ushort neglimit, ushort neglimitlev, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetNegsLimitlevel(neglimit, neglimitlev,naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴负限位信号和电平失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置轴原点信号和电平
        /// </summary>
        /// <param name="orgNum">原点信号：配置限位端口，0、1、2..........等</param>
        /// <param name="orglev">原点信号电平：0低电平，1高电平</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoOrgLimitlevel(ushort orgNum, ushort orglev, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetOrgLimitlevel(orgNum, orglev, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴原点信号和电平失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置轴回零模式
        /// </summary>
        /// <param name="homeMode">回零模式：0：反向找原点；1：先正向找上限位，再反向找原点；2：先反向找下限位，再正向找原点</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoHomMode(ushort homeMode, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetHomMod(homeMode, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴回零模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置轴报警模式
        /// </summary>
        /// <param name="alarmmode">轴报警电平：</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAlarmlevel(ushort alarmmode, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetAlarmlevel(alarmmode, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴报警模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置轴回零
        /// </summary>
        /// <param name="speed">回零速度，脉冲量</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoHome(int speed, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxHome(speed, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴回零失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 设置指定轴的起始速度
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="lspeed">起始速度：脉冲量</param>
        /// <returns></returns>
        public bool DoAxisInitspeed(int lspeed, int naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetStartSpeed(lspeed, (ushort)naxis)==0)
                {
                    rt = true;
                }       
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴初始速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 设置指定轴的运动速度
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="speed">运动速度：脉冲量</param>
        /// <returns></returns>
        public bool DoAxisSteadySpeed(int speed, int naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetSpeed(speed, (ushort)naxis)==0)
                {
                    rt = true;
                }  
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴运行速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置指定轴的加速度
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="accel">加速度</param>
        /// <returns></returns>
        public bool DoAxisAccel(int naxis, float accel)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetAcecl((int)accel, (ushort)naxis)==0)
                {
                    rt = true;  
                }  
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴加速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置指定轴的减速度
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="decel">减速度</param>
        /// <returns></returns>
        public bool DoAxisDecel(int naxis, float decel)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetDecel((int)decel, (ushort)naxis)==0)
                {
                    rt = true;  
                } 
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴减速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 设置轴末速度
        /// </summary>
        /// <param name="End">末速度,脉冲量</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAxisEndSpeed(int End, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetEndSpeed(End, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴末速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 设置轴回原点快速度
        /// </summary>
        /// <param name="HomFast">回零快速,脉冲量</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAxisHomeFastSpeed(int HomFast, ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetHomeFastSpeed(HomFast, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴回原点快速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置轴回原点慢速度
        /// </summary>
        /// <param name="HomSlow">回零慢速度,脉冲量</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAxisHomeSlowSpeed(int HomSlow, ushort naxis)
        {

            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetHomeSlowSpeed(HomSlow, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴回原点慢速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 设置轴原点偏移量
        /// </summary>
        /// <param name="HomOffset">回原点偏移坐标,脉冲量</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAxHomeOffset(int HomOffset, ushort naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetHomeOffset(HomOffset, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置轴原点偏移量失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        
        }


        /// <summary>
        /// 设置指定轴的负向软限位值
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="fvalue">负限位值：脉冲量</param>
        /// <returns></returns>
        public bool DoAxisSRevValue(int fvalue, int naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetSoftMinLimit(fvalue, (ushort)naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置软负限位值失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 设置指定轴的正向软限位值
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="fvalue">正限位值：脉冲量</param>
        /// <returns></returns>
        public bool DoAaxisSFwdValue(int fvalue, int naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxSetSoftMaxLimit(fvalue, (ushort)naxis) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置软正限位值失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }



        /// <summary>
        /// 指定轴运动绝对位置
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="speed">速度，脉冲量</param>
        /// <param name="pos">绝对位置，脉冲量</param>
        /// <returns></returns>
        public bool DoSingleAbsMove(int naxis, int speed, int pos)
        {
            bool rt = false;
            try
            {
                if (  hzaux.HZ_AxMoveAbs(pos, speed, (ushort)naxis)==0)
                {
                    rt = true;   
                }     
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡指定轴绝对运动指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 指定轴运动绝对位置
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="speed">速度，毫米量</param>
        /// <param name="pos">绝对位置，毫米量</param>
        /// <returns></returns>
        public bool DoSingleAbsMove(int naxis, float speed, float pos)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxMoveAbs(pos, speed, (ushort)naxis) == 0)
                {
                    rt = true;
                }  
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡指定轴绝对运动失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 指定轴运动相对位置
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="speed">速度，脉冲量</param>
        /// <param name="pos">相对位置，脉冲量</param>
        /// <returns></returns>
        public bool DoSingleRelMove(int naxis, float speed, float pos)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxMoveRel(pos, speed, (ushort)naxis) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡指定轴相对运动指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 指定轴运动相对位置
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="speed">速度，毫米量</param>
        /// <param name="pos">相对位置，毫米量</param>
        /// <returns></returns>
        public bool DoSingleRelMove(int naxis, int speed, int pos)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxMoveRel(pos, speed, (ushort)naxis) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡指定轴相对运动指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }



        /// <summary>
        /// 指定轴口恒定速度运行
        /// </summary>
        /// <param name="distance">位移,脉冲量</param>
        /// <param name="speed">速度,脉冲量</param>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public bool DoMoveVelocity(int distance, int speed, ushort naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxMoveVelocity(distance, speed, naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设定轴口恒定速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 指定轴立即停止运动
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAxisStop(ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxStop(naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡指定轴立即停止运动失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 指定轴减速停止运动
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoAxisStopDec(ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxStopDec(naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡指定轴减速停止运动失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取指定轴的轴状态
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="naxisStatus">轴状态： 0就绪，1停止，2减速停，3普通运动，4连续运动，5正在回原点，6未激活状态，7错误停，8轴同步状态</param>
        /// <returns></returns>
        public  bool DoAxisStatus(int naxis,ref short naxisStatus)
        {
            bool rt = false;
            try
            {
                if ( hzaux.HZ_AxGetStatus(ref naxisStatus, (ushort)naxis)==0)
                {
                    rt = true; 
                }    
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取轴状态指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取硬件版本
        /// </summary>
        /// <param name="HardWareVer">硬件版本：长度10</param>
        /// <returns></returns>
        public bool DoHardWareVer(ref int[] HardWareVer) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetHardWareVer(ref HardWareVer) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取硬件版本指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取软件版本
        /// </summary>
        /// <param name="softVer">软件版本：长度10</param>
        /// <returns></returns>
        public bool DoSoftWareVer(ref int[] softVer) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetSoftWareVer(ref softVer) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取软件版本指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取指定轴的位置
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="curpos">当前位置，脉冲量</param>
        /// <returns></returns>
        public bool DoAxisCurPos(int naxis, ref int curpos)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxGetCurPos(ref curpos, (ushort)naxis) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取轴当前位置指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取指定轴的位置
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <param name="curpos">当前位置，毫米量</param>
        /// <returns></returns>
        public bool DoAxisCurPos(int naxis, ref float curpos)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxGetCurPos(ref curpos, (ushort)naxis) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取轴当前位置指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取编码器状态
        /// </summary>
        /// <param name="code">编码器值，长度5</param>
        /// <param name="num">编码器编号</param>
        /// <returns></returns>
        public bool DoEncoder(ref int[] code, ushort num)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_AxGetEncoder(ref code, num) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设定轴限位模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取指定输入口的当前值
        /// </summary>
        /// <param name="nport">输入口号</param>
        /// <param name="value">当前值</param>
        /// <returns></returns>
        public bool DoInBitValue(int nport, ref int value)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetInputStata(ref value, (ushort)nport) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取输入位指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取指定输出口的当前值
        /// </summary>
        /// <param name="nport">输出口号</param>
        /// <param name="value">当前值</param>
        /// <returns></returns>
        public bool DoOutBitValue(int nport, ref int value)
        {
            bool rt = false;
            value = 0;
            try
            {
                if (hzaux.HZ_GetOutputStata(ref value, (ushort)nport) == 0)
                {
                    rt = true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取输出位指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置输出状态
        /// </summary>
        /// <param name="nport">输出口位置，从0开始</param>
        /// <returns></returns>
        public  bool DoOutputStata(ushort nport)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetOutputStata(nport) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置输出状态失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取错误代码，获取的是数字，错误内容参考说明书
        /// </summary>
        /// <param name="errorCode">报警码，长度20</param>
        /// <returns></returns>
        public  bool DoErrorCode(ref int[] errorCode){
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetErrorCode(ref errorCode) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取错误代码失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        
        /// <summary>
        /// 获取设备错误等级
        /// </summary>
        /// <param name="errorLevel">报警等级</param>
        /// <returns></returns>
        public  bool DoErrorLevel(ref int errorLevel) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetErrorLevel(ref errorLevel) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取设备错误等级失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

       


        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="time">时间，长度4</param>
        /// <returns></returns>
        public  bool DoMachineTime(ref byte[] time) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetMachineTime(ref time) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取时间失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="data">日期，长度4</param>
        /// <returns></returns>
        public  bool DoMachineData(ref byte[] data) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetMachineData(ref data) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设定轴限位模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 获取客户随机码
        /// </summary>
        /// <param name="id">客户随机码,长度2</param>
        /// <returns></returns>
        public  bool DoMachineCID(ref int[] id) 
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetMachineCID(ref id) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设定轴限位模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }
        /// <summary>
        /// 获取设备运行状态
        /// </summary>
        /// <param name="state">设备状态：0初始，1停止，2运行，3复位，4急停，5暂停</param>
        /// <returns></returns>
        public bool DoRunStatus(ref int state) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_GetRunStatus(ref state) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡获取设备运行状态失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 操作控制卡写Flash
        /// </summary>
        /// <returns></returns>
        public  bool DoWriteFlash() 
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteFlash() == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡操作控制卡写Flash失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置点动模式  0：数量级 1：指定脉冲数 2：连续模式
        /// </summary>
        /// <param name="mode">0,1,2</param>
        /// <returns></returns>
        public  bool DoJogMode(int mode) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogMode(mode) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置点动模式失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置点动等级，在0点动模式下有效
        /// </summary>
        /// <param name="level">0：一个脉冲，1：:10个脉冲，2:100个脉冲，3:1000个脉冲，4：10000个脉冲，5:100000个脉冲</param>
        /// <returns></returns>
        public  bool DoJogLevel(int level) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogLevel(level) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置点动等级失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置固定距离(脉冲数)，在1点动模式下有效
        /// </summary>
        /// <param name="pluse">脉冲量</param>
        /// <returns></returns>
        public  bool DoJogPosition(int pluse) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogPosition(pluse) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置固定距离失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 正向点动
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoJogForward(ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogForward(naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡正向点动指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 反向点动
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoJogBackward(ushort naxis) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogBackward(naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡反向点动指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }
        /// <summary>
        /// 点动清零
        /// </summary>
        /// <returns></returns>
        public  bool DoJogClear() 
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogClear() == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡点动清零指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 回零
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoJogGohome(ushort naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogGohome(naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡回零指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 点动立即停止
        /// </summary>
        /// <param name="naxis">轴号</param>
        /// <returns></returns>
        public  bool DoJogStop(ushort naxis)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogStop(naxis) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡点动立即停止指令失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 设置点动速度
        /// </summary>
        /// <param name="speed">速度</param>
        /// <returns></returns>
        public  bool DoJogSpeedPercent(int speed) 
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetJogSpeedPercent(speed) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡设置点动速度失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }



        /// <summary>
        /// 清除报警
        /// </summary>
        /// <returns></returns>
        public  bool DoClearAlarm() 
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_SetClearAlarm() == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡清除报警失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 读取一位寄存器，返回两个字节
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="number">短整型个数，返回字节</param>
        /// <param name="pValue">字节数组</param>
        /// <returns></returns>
        public  bool DoReadRegister(ushort startAddr, ushort number, out byte[] pValue) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, number,out pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取一位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
                pValue = new byte[] { 0 };
            }

            return rt;
        }

        /// <summary>
        /// 读取一位寄存器，返回一个无符号短整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="number">无符号短整型个数</param>
        /// <param name="pValue">无符号短整型数组</param>
        /// <returns></returns>
        public  bool DoReadRegister(ushort startAddr, ushort number, out ushort[] pValue) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, number, out pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取一位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
                pValue = new ushort[] { 0 };
            }

            return rt;
        }

        /// <summary>
        /// 读取一位寄存器，返回一个短整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="number">短整型个数</param>
        /// <param name="pValue">短整型数组</param>
        /// <returns></returns>
        public  bool DoReadRegister(ushort startAddr, ushort number, out short[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, number, out pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取一位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
                pValue = new short[] { 0 };
            }

            return rt;
        }

        /// <summary>
        /// 读取两位寄存器，返回一个无符号整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="number">无符号整型个数</param>
        /// <param name="pValue">无符号整型数组</param>
        /// <returns></returns>
        public  bool DoReadRegister(ushort startAddr, ushort number, out uint[] pValue) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, number, out pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取两位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
                pValue = new uint[] { 0 };
            }

            return rt;
        }

        /// <summary>
        /// 读取两位寄存器，返回一个整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="number">整型个数</param>
        /// <param name="pValue">整型数组</param>
        /// <returns></returns>
        public  bool DoReadRegister(ushort startAddr, ushort number, out int[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, number, out pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取两位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
                pValue = new int[] { 0 };
            }

            return rt;
        }

        /// <summary>
        /// 读取两位寄存器，返回一个浮点型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="number">浮点型个数</param>
        /// <param name="pValue">浮点型数组</param>
        /// <returns></returns>
        public  bool DoReadRegister(ushort startAddr, ushort number, out float[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, number, out pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取两位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
                pValue = new float[] { 0 };
            }

            return rt;
        }


        /// <summary>
        /// 读取对象
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="structure">对象数据</param>
        /// <returns></returns>
        public bool DoReadRegister(ushort startAddr, object structure)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_ReadRegister(startAddr, structure) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡读取两位寄存器失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }
        /// <summary>
        /// 写字节
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public  bool DoWriteRegister(ushort startAddr, byte[] pValue) {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写字节失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写无符号短整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public  bool DoWriteRegister(ushort startAddr, ushort[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写无符号短整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写短整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public  bool DoWriteRegister(ushort startAddr, short[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写短整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写无符号整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public  bool DoWriteRegister(ushort startAddr, uint[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写无符号整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public  bool DoWriteRegister(ushort startAddr, int[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写浮点型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public  bool DoWriteRegister(ushort startAddr, float[] pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写浮点型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }


        /// <summary>
        /// 写无符号短整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public bool DoWriteRegister(ushort startAddr, ushort pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写无符号短整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写短整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public bool DoWriteRegister(ushort startAddr, short pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写短整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写无符号整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public bool DoWriteRegister(ushort startAddr, uint pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写无符号整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写整型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public bool DoWriteRegister(ushort startAddr, int pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写整型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写浮点型
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="pValue">写入数据</param>
        /// <returns></returns>
        public bool DoWriteRegister(ushort startAddr, float pValue)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, pValue) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写浮点型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        /// <summary>
        /// 写对象数据
        /// </summary>
        /// <param name="startAddr">地址</param>
        /// <param name="structure">写入对象数据</param>
        /// <returns></returns>
        public bool DoWriteRegister(ushort startAddr, object structure)
        {
            bool rt = false;
            try
            {
                if (hzaux.HZ_WriteRegister(startAddr, structure) == 0)
                {
                    rt = true;
                }

            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(string.Format("错误：板卡写浮点型失败!\n异常描述:{0}\n时间：{1}", ex.Message, DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
            return rt;
        }

        #endregion

  
    }
}
