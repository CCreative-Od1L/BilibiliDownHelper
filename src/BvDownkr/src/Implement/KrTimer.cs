using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Implement
{
    public class KrTimer {
        private long timeOut = 0;
        private int IntervalSecond { get; set; } = 0;
        private bool isLoop;
        private event Action TimeOutAction;
        private readonly AutoResetEvent stop = new(false);
        public KrTimer(Action action, int interval = 10, bool loop = false) {
            TimeOutAction += action;
            isLoop = loop;
            IntervalSecond = interval;
            timeOut = long.Parse(DateTimeUtils.GetCurrentTimestampSecond()) + interval;

            StartTask();
        }
        private void StartTask() {
            Task task = new(() => {
                AutoResetEvent pause = new(false);
                while(true) {
                    if (long.Parse(DateTimeUtils.GetCurrentTimestampSecond()) >= timeOut) {
                        TimeOutAction?.Invoke();
                        if(isLoop) { 
                            Reset();
                        }
                        stop.WaitOne();
                    }
                    // * 0.5秒检测一次
                    pause.WaitOne(500, true);
                }
            });
            task.Start();
        }
        public void AddAction(Action action) {
            TimeOutAction += action;
        }
        public void RemoveAction(Action action) {
            TimeOutAction -= action;
        }
        public void ChangeIsLoop(bool enableLoop) {
            isLoop = enableLoop;
        }
        public void Reset() {
            timeOut = long.Parse(DateTimeUtils.GetCurrentTimestampSecond()) + IntervalSecond;
            stop.Set();
        }
    }
}
