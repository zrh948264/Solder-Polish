using System;

namespace Common 
{
    #region 轴参数类
    [Serializable]
    public class Axis
    {
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }
        public Axis(){
            Name = "";
            Num = 0;
            StartSpeed = 10;
            AccSpeed = 10;
            RunSpeed = 50;
            DecSpeed = 10;
            EndSpeed = 10;
            HomeSpeedFast = 50;
            HomeSpeedSlow = 10;
            HomeOffset = 0;
            PositiveLimitPostion = 99999;
            NegativeLimitPostion = -99999;
            LeaderPer = 1;
            PulsePer = 1;

            limitMode = 0;
            Poslimit = 0;
            Poslimitlev = 0;
            Neglimit = 0;
            Neglimitlev = 0;
            OrgNum = 0;
            Orglev = 0;
            HomeMode = 0;
            alarmmode = 2;
        }
        public string Name { get; set; }			    //轴名称
        public int Num { get; set; }			        //轴号
        public float Position { get; set; }             //轴位置

        public float StartSpeed { get; set; }			//初速度
        public float AccSpeed { get; set; }			    //加速度
        public float RunSpeed { get; set; }			    //运行速度
        public float DecSpeed { get; set; }			    //减速度
        public float EndSpeed { get; set; }			    //末速度
        public float HomeSpeedFast { get; set; }		//回原点快速速度
        public float HomeSpeedSlow { get; set; }		//回原点慢速速度
        public float HomeOffset { get; set; }			//回原点偏移坐标
        public float NegativeLimitPostion { get; set; } //负限位
        public float PositiveLimitPostion { get; set; } //正限位
        public float LeaderPer { get; set; }            //导程
        public float PulsePer { get; set; }             //脉冲

        public short limitMode { get; set; }     //限位模式 0：没限位  1：软件限位 2：硬件限位 3：软硬都限
        public short Poslimit { get; set; }      //正限位
        public short Poslimitlev { get; set; }   //正限位电平
        public short Neglimit { get; set; }      //负限位
        public short Neglimitlev { get; set; }   //负限位电平
        public short OrgNum { get; set; }        //原点
        public short Orglev { get; set; }        //原点电平
        public short HomeMode { get; set; }      //回零模式  0:找原点回 1:找上限回 2:找下限回 3:Z向找原点正 4:Z向找原点负
        public short alarmmode { get; set; }     //报警电平  0:低电平 1:高电平 2:不报警


    }

    #endregion
}
