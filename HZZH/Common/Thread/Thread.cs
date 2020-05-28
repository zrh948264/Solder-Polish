using System.ComponentModel;
using System.Threading;

namespace Device
{
    public class LogicThreadDef
    {
        public ManualResetEvent manualReset;//控制线程的执行停止
        public Mutex runMutex;//确保只有一个线程访问某个资源或某段代码。可被用于防止一个程序的多个实例同时运行
        public AutoResetEvent runEvent;
        public BackgroundWorker bWorker;

        public LogicThreadDef()
        {
            runMutex = new Mutex();
            manualReset = new ManualResetEvent(true);
            runEvent = new AutoResetEvent(false);
            bWorker = new BackgroundWorker();
            bWorker.WorkerSupportsCancellation = true;
        }

        public void Pause()
        {
            if (bWorker != null)
            {
                if (bWorker.IsBusy == true)
                {
                    manualReset.Reset();
                }
            }
        }

        public void Start()
        {
            if (bWorker != null)
            {
                if (bWorker.IsBusy == false)
                {
                    bWorker.RunWorkerAsync();
                    manualReset.Set();
                }
                else
                {
                    manualReset.Set();
                }
            }
        }

        public void Stop()
        {
            if (bWorker != null)
            {
                if (bWorker.IsBusy == true)
                {
                    manualReset.Set();
                    bWorker.CancelAsync();
                }
            }
        }

        public bool Busy
        {
            get
            {
                return bWorker.IsBusy;
            }
        }
    }
}