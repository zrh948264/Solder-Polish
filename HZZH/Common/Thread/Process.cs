
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
namespace Device
{
    public class MainProcess
    {
		LogicThreadDef RunThread = new LogicThreadDef();
        public void InitializationRunThread()
        {
            RunThread.bWorker.DoWork += bWorker_Main_DoWork;
            RunThread.Start();
        }
        void bWorker_Main_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                while(worker.CancellationPending == false)
                {
                    RunThread.manualReset.WaitOne();
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("流程控制错误：" + ex.Message);
            }
        }

        public void  AppStart()
        {
            RunThread.Start();
        }
     
        public void  AppStop()
        {
            RunThread.Stop();
        }


        public void AppPause()
        {
            RunThread.Pause();

        }

        public void AppReset()
        {
            RunThread.Stop();
        }
    }
}
