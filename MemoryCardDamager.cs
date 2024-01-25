using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatchGame1
{
    internal class MemoryCardDamager :BaseDamager
    {
        private IClickable clickable;
        private IEnumerable<Border> clickableBorders;
        public IEnumerable<Border> ClickableBorders 
        { get => clickableBorders; set => clickableBorders = value; }
      
        private TextBlock outputTextBlock;
        public TextBlock OutputTextBlock => outputTextBlock;

        public IEndable End { private get; set; }

        public StackPanel HPBar;
        private int currentHP;
        public int maxHP;

        public MemoryCardDamager(IClickable clickable, ref StackPanel HPBar, TextBlock outputTextBlock)
        {
            maxHP = HPBar.Children.OfType<TextBlock>().Count();
            this.clickable = clickable;
            this.outputTextBlock = outputTextBlock;
            this.HPBar = HPBar;
            currentHP = maxHP;
        }

        public override void TakeDamage()
        {
            foreach (var element in HPBar.Children.OfType<TextBlock>())
            {
                if (element.Text == "❤️")
                {
                    element.Text = "🤍";
                    //element.Text = "💔";
                    element.Foreground = Brushes.Black;

                    currentHP--;
                    
                    break;
                }
            }
            if (currentHP == 0) 
            {
                End.GameOver("Good Game");
                currentHP = maxHP;
            } 
        }
       
        //public void GameOver(string finalInscription)
        //{
        //    Timer.StopTimer(outputTextBlock ,finalInscription);
        //    clickable.StartButton.Content = "Restart";
        //    clickable.StartButton.Visibility = Visibility.Visible;
        //    currentHP = maxHP;
        //    foreach (var border in clickableBorders)
        //    {
        //        border.MouseDown -= clickable.Border_MouseDown;
        //        border.Child.Opacity = 1;
        //    }
        //    return;
        //}
    }
}
