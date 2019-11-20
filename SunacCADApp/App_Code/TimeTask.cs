using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunacCADApp
{
    public class TimeTask
    {
        public event System.Timers.ElapsedEventHandler ExecuteTask;
        private static readonly TimeTask _task = null;
        private System.Timers.Timer _timer = null;

        //定义时间
        private int _interval = 60000;

        public int Interval { set; get; }


        static TimeTask()
        {
            _task = new TimeTask();
        }

        public static TimeTask Instance()
        {
            return _task;
        }

        //开始
        public void Start()
        {
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(_interval);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
                _timer.Enabled = true;
                _timer.Start();
            }
        }

        protected void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (null != ExecuteTask)
            {
                ExecuteTask(sender, e);
            }
        }

        //停止
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    
       
    }
}