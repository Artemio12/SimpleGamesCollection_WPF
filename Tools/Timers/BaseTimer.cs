using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MatchGame1
{
    public abstract class BaseTimer
    {
        protected DispatcherTimer timer = new DispatcherTimer();
        protected BaseDamager damager;
        protected TextBlock outputTextBlock;
        protected int startTime;
        protected int currentTime;

        public int CurrentTime { get => currentTime; set => currentTime = value; }
        public void StartTimer(int startTime)
        {
            this.startTime = startTime;
            currentTime = startTime;
            timer.Interval = TimeSpan.FromSeconds(.1f);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void StopTimer(string finalText)
        {
            outputTextBlock.Text = finalText;
            StopTimer();
        }

        public void StopTimer()
        {
            timer.Tick -= Timer_Tick;
            timer.Stop();
        }

        protected abstract void Timer_Tick(object sender, EventArgs e);
    }
}
