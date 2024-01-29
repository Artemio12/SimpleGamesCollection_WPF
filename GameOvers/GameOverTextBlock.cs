using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace WPFGamesCollection
{
    internal class GameOverTextBlock :IEndable
    {
        private IClickable clickable;
        private IEnumerable<Border> clickableBorders;

        private BaseDamager damager;
        private BaseTimer myTimer;

        public GameOverTextBlock(IClickable clickable, IEnumerable<Border> clickableBorders, BaseDamager damager, BaseTimer myTimer)
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
