using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchGame1
{
    internal class IncreasingTimer :BaseTimer
    {
        protected override void Timer_Tick(object sender, EventArgs e)
        {
            currentTime++;
            outputTextBlock.Text = (currentTime / 10f).ToString("0.0s");
        }
    }
}
