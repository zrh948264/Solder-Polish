
using HZZH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections;
using Device;
using Common;


namespace Vision.Logic
{
    [Serializable]
    public class MotionPlatform : IPlatformMove
    {
        public BoardCtrllerManager movedriverZm = null;

        public int AxisX { get; set; }
        public int AxisY { get; set; }

        public MotionPlatform(BoardCtrllerManager board, int axX, int axY)
        {
            movedriverZm = board;
            AxisX = axX;
            AxisY = axY;
        }


        public float[] AxisPosition
        {
            get
            {
                float[] XYROrigin = new float[3];
                XYROrigin[0] = movedriverZm.CurrentPos.FloatValue[AxisX];
                XYROrigin[1] = movedriverZm.CurrentPos.FloatValue[AxisY];
                XYROrigin[2] = 0;

                return XYROrigin;
            }
        }



        void IPlatformMove.AbsMoving(float x, float y, float r)
        {
            this.AbsMoving(x, y);
        }



        public void AbsMoving(float x, float y)
        {
            movedriverZm.MoveAbs(AxisX, 20, x);
            movedriverZm.MoveAbs(AxisY, 20, y);


            System.Threading.Thread.Sleep(3);

            if (CompleteMovedEvent != null)
            {
                CheckCompleteMove();
            }

        }

        public bool WaitOnCompleteMoving(int outTime = -1)
        {
            DateTime time = DateTime.Now;
            float spendTime = outTime < 0 ? float.PositiveInfinity : outTime;
            while (Math.Abs((time - DateTime.Now).TotalMilliseconds) < spendTime)
            {
                int[] state = GetMotionState();

                bool flag = false;
                for (int i = 0; i < state.Length; i++)
                {
                    if (state[i] != 0)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == true)
                {
                    System.Threading.Thread.Sleep(10);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        private int[] GetMotionState()
        {
            BaseData AxisStatus = new BaseData(50, 5);
            movedriverZm.ReadRegister(AxisStatus);

            DateTime time = DateTime.Now;
            while (true)
            {
                if (AxisStatus.Succeed)
                {
                    return AxisStatus.IntValue;
                }

                if ((DateTime.Now - time).TotalMilliseconds > 1000)
                {
                    return new int[] { 0, 0, 0, 0, 0 };
                }
            }
        }


        private void CheckCompleteMove()
        {

            Action action = (() =>
            {
                Thread.Sleep(50);
                ((IPlatformMove)this).WaitOnCompleteMoving();
            });
            AsyncCallback callback = ((ar) =>
            {
                OnCompleteMoved(this, EventArgs.Empty);
            });
            IAsyncResult result = action.BeginInvoke(callback, null);


        }



        public event EventHandler CompleteMovedEvent;

        protected void OnCompleteMoved(object sender, EventArgs e)
        {
            EventHandler temp = CompleteMovedEvent;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }


        private MotionPlatform()
        {

        }






    }







}
