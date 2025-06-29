using System;

namespace WPFGamesCollection
{
    public class IncreasingTimer :BaseTimer
    {
        protected override void Timer_Tick(object sender, EventArgs e)
        {
            currentTime++;
            outputTextBlock.Text = (currentTime / 10f).ToString("0.0s");
        }
    }
}
