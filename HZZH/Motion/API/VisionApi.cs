using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HZZH.Vision.Logic;
using Motion;
using UI;

namespace Vision
{
	
    /// <summary>
    /// 与视觉功能交互的信息
    /// </summary>
	public class VisionAPIDef
	{
        public List<VisionResult> SolderLeft = new List<VisionResult>();
        public List<VisionResult> SolderRight = new List<VisionResult>();
        public List<VisionResult> PolishLeft = new List<VisionResult>();
        public List<VisionResult> PolishRight = new List<VisionResult>();
        
        public List<VisionResult> Solders(int index)
        {
            if (index == 0)
            {
                return SolderLeft;
            }
            else
            {
                return SolderRight;
            }
        }
        public List<VisionResult> Polishs(int index)
        {
            if (index == 0)
            {
                return PolishLeft;
            }
            else
            {
                return PolishRight;
            }
        }

        /// <summary>
        /// 识别模板给出模板中心到相机中心偏差
        /// </summary>
        /// <param name="index">模板号</param>
        /// <param name="Cameeraindex">相机号</param>
        /// <returns></returns>
        public VisionResult TriggerDiscern(int index, int Cameeraindex)
        {
            VisionResult result = new VisionResult();


            return result;
        }


        public bool TriggerCamera0(int index)
        {
            SolderLeft.Clear(); 
            System.Threading.Thread.Sleep(FormMain.RunProcess.LogicData.RunData.vDeley);

            VisionProject.Instance.CameraSoft(0);
            if (VisionProject.Instance.WaiteGetImage(0, 1000)==false)
            {
                return false;
            }

            VisionProject.Instance.LocateSolderLeftShape();
            //VisionProject.Instance.LocateSolderLeftShape(index);
            return SolderLeft.Count > 0;//= FormMain.RunProcess.LogicData.RunData.sNumL;
        }
        public bool TriggerCamera1(int index)
        {
            SolderRight.Clear();
            System.Threading.Thread.Sleep(FormMain.RunProcess.LogicData.RunData.vDeley);

            VisionProject.Instance.CameraSoft(1);
            if (VisionProject.Instance.WaiteGetImage(1, 1000) == false)
            {
                return false;
            }

            VisionProject.Instance.LocateSolderRightShape();
            //VisionProject.Instance.LocateSolderRightShape(index);
            return SolderRight.Count > 0;//= FormMain.RunProcess.LogicData.RunData.sNumR;
        }
        public bool TriggerCamera2(int index)
        {
            PolishLeft.Clear();
            System.Threading.Thread.Sleep(FormMain.RunProcess.LogicData.RunData.vDeley);

            VisionProject.Instance.CameraSoft(2);
            if (VisionProject.Instance.WaiteGetImage(2, 1000) == false)
            {
                return false;
            }

            VisionProject.Instance.LocatePolishLeftShape();
            //VisionProject.Instance.LocatePolishLeftShape(index);
            return PolishLeft.Count >= FormMain.RunProcess.LogicData.RunData.pNumL; //0;//
        }
        public bool TriggerCamera3(int index)
        {
            PolishRight.Clear();
            System.Threading.Thread.Sleep(FormMain.RunProcess.LogicData.RunData.vDeley);

            VisionProject.Instance.CameraSoft(2);
            if (VisionProject.Instance.WaiteGetImage(2, 1000) == false)
            {
                return false;
            }

            VisionProject.Instance.LocatePolishRightShape();
            //VisionProject.Instance.LocatePolishRightShape(index);
            return PolishRight.Count >= FormMain.RunProcess.LogicData.RunData.pNumR;//0;//
        }
    }

    /****************************************************************/
    public class VisionResult
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }

        public bool Result { get; set; }
        public int Type { get; set; }
    }


}
