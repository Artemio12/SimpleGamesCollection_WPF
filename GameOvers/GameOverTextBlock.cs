using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MatchGame1
{
    internal class GameOverTextBlock :IEndable
    {
        private IClickable clickable;
        private IEnumerable<Border> clickableBorders;

        private BaseDamager damager;
        private DecreasingTimer myTimer;

        public GameOverTextBlock(IClickable clickable, IEnumerable<Border> clickableBorders, BaseDamager damager, DecreasingTimer myTimer)
        { 
            this.clickable = clickable;
            this.clickableBorders = clickableBorders;
            this.damager = damager;
            this.myTimer = myTimer;
        }
        public void GameOver(string finalInscription)
        {
            myTimer.StopTimer( finalInscription);
    
            clickable.StartButton.Content = "Restart";
            clickable.StartButton.Visibility = Visibility.Visible;
            damager.CurrentHP = damager.MaxHP;
            foreach (var border in clickableBorders)
            {
                border.MouseDown -= clickable.ClickableBorder_MouseDown;
                border.Child.Opacity = 1;
            }
            return;
        }
    }
}
