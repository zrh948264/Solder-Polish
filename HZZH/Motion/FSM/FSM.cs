using Common;
using System.Threading;

namespace Motion
{
	public enum FsmStaDef : int
	{
		INIT = 0,		//初始态
		STOP = 1,		//停止态
		RUN = 2,		//运行态
		RESET = 3,		//复位态
		SCRAM = 4,      //急停态
		PAUSE = 5,      //暂停态
	}

	public enum RunModeDef : int
	{ 
		NORMAL = 0,		//正常模式
		AGING = 1,		//老化模式
	}

	public class FsmDef
	{
		public Device.BoardCtrllerManager movedriverZm { get; set; }
		public FsmStaDef Status { get; set; }
		public RunModeDef RunMode { get; set; }

		BaseData FsmSta_Reg = new BaseData(500, 1);
		public FsmDef(Device.BoardCtrllerManager movedriverZm)
		{
			this.movedriverZm = movedriverZm;
			Status = FsmStaDef.INIT;
			RunMode = RunModeDef.NORMAL;
		}

		public void Run(RunModeDef RunMode)
		{
			if (!movedriverZm.Succeed)
			{
				return;
			}
			this.RunMode = RunMode;
			BaseData RunMode_Reg = new BaseData(1504, new int[] { (int)RunMode });
			movedriverZm.WriteRegister(RunMode_Reg);
			while (RunMode_Reg.Succeed == false) { Thread.Sleep(1); }
			BaseData FsmCmd_Reg = new BaseData(1500, new int[] { (int)FsmStaDef.RUN });
			movedriverZm.WriteRegister(FsmCmd_Reg);
			while (FsmCmd_Reg.Succeed == false) { Thread.Sleep(1); }
			this.Status = FsmStaDef.RUN;
		}
		public void Stop()
		{
			if (!movedriverZm.Succeed)
			{
				return;
			}
			BaseData FsmCmd_Reg = new BaseData(1500, new int[] { (int)FsmStaDef.STOP });
			movedriverZm.WriteRegister(FsmCmd_Reg);
			while (FsmCmd_Reg.Succeed == false) { Thread.Sleep(1); }
			this.Status = FsmStaDef.STOP;
		}
		public void Pause()
		{
			if (!movedriverZm.Succeed)
			{
				return;
			}
			BaseData FsmCmd_Reg = new BaseData(1500, new int[] { (int)FsmStaDef.PAUSE });
			movedriverZm.WriteRegister(FsmCmd_Reg);
			while (FsmCmd_Reg.Succeed == false) { Thread.Sleep(1); }
			this.Status = FsmStaDef.PAUSE;
		}
		public void Reset()
		{
			if (!movedriverZm.Succeed)
			{
				return;
			}
			BaseData FsmCmd_Reg = new BaseData(1500, new int[] { (int)FsmStaDef.RESET });
			movedriverZm.WriteRegister(FsmCmd_Reg);
			while (FsmCmd_Reg.Succeed == false) { Thread.Sleep(1); }
			this.Status = FsmStaDef.RESET;
		}
		public void Scram()
		{
			if (!movedriverZm.Succeed)
			{
				return;
			}
			BaseData FsmCmd_Reg = new BaseData(1500, new int[] { (int)FsmStaDef.SCRAM });
			movedriverZm.WriteRegister(FsmCmd_Reg);
			while (FsmCmd_Reg.Succeed == false) { Thread.Sleep(1); }
			this.Status = FsmStaDef.SCRAM;
		}
		public FsmStaDef GetStatus()
		{
			if (!movedriverZm.Succeed)
			{
				return Status;
            }
            FsmSta_Reg.Succeed = false;
            movedriverZm.ReadRegister(FsmSta_Reg);
            while (FsmSta_Reg.Succeed == false) ;
            this.Status = (FsmStaDef)FsmSta_Reg.IntValue[0];
			return this.Status;
		}
	}
}
