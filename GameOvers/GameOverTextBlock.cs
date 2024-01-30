using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
        public async void GameOver(string finalInscription)
        {
            myTimer.StopTimer( finalInscription);

            foreach (var border in clickableBorders)
            {
                border.MouseDown -= clickable.ClickableBorder_MouseDown;
            }
    
            await Task.Delay(TimeSpan.FromSeconds(0.3f));

            foreach (var border in clickableBorders)
            {
                border.Child.Opacity = 1;
            }

            clickable.StartButton.Content = "Restart";
            clickable.StartButton.Visibility = Visibility.Visible;
            damager.CurrentHP = damager.MaxHP;

            return;
        }
    }
}
