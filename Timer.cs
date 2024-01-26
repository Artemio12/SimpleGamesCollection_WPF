using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;

namespace MatchGame1
{
    internal class Timer
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private BaseDamager damager;
        private TextBlock outputTextBlock;
        private int startTime;
        private int currentTime;
        public int CurrentTime { get => currentTime; set => currentTime = value; }
       
        public Timer(BaseDamager damager, TextBlock outputTextBlock)
        {
            this.damager = damager;
            this.outputTextBlock = outputTextBlock;
        }

        public void StartTimer(int startTime)
        {
            this.startTime = startTime;
            currentTime = startTime;
            timer.Interval = TimeSpan.FromSeconds(.1f);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime--;
            outputTextBlock.Text = (currentTime / 10f).ToString("0.0s");

            if (currentTime == 0)
            {
                damager.TakeDamage();
                CurrentTime = startTime;
            }
        }
        public void StopTimer(TextBlock outputTextBlock,string finalText)
        {
            outputTextBlock.Text = finalText;
            timer.Stop();
        }
    }
}
