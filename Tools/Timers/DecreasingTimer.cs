using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;

namespace MatchGame1
{
    internal class DecreasingTimer :BaseTimer
    {
        public DecreasingTimer(BaseDamager damager, TextBlock outputTextBlock)
        {
            this.damager = damager;
            this.outputTextBlock = outputTextBlock;
        }

        protected override void Timer_Tick(object sender, EventArgs e)
        {
            currentTime--;
            outputTextBlock.Text = (currentTime / 10f).ToString("0.0s");

            if (currentTime == 0)
            {
                damager.TakeDamage();
                CurrentTime = startTime/2;
            }
        }
    }
}
