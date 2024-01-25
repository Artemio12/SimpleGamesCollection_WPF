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
        public static DispatcherTimer timer = new DispatcherTimer();
        private MemoryCardDamager damager;
        private TextBlock outputTextBlock;
        private int startTime;
        private int currentTime;
        public int CurrentTime { get { return currentTime; } set { currentTime = value; } }
        public Timer(IClickable clickable, ref List<TextBlock> HPHeartList, TextBlock textBlock)
        {
            outputTextBlock = textBlock;
        }

        public Timer(MemoryCardDamager damager)
        {
            this.damager = damager;
            outputTextBlock = damager.OutputTextBlock;
            
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
