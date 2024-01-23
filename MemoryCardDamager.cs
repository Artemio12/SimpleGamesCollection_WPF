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
    internal class MemoryCardDamager
    {
        private IClickable clickable;
        private List<Border> clickableBorderList;
        public List<Border> ClickableBorderList { get => clickableBorderList; set => clickableBorderList = value;  }
        private TextBlock outputTextBlock;
        public TextBlock OutputTextBlock => outputTextBlock;

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

        public void TakeDamage()
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
            if (currentHP == 0) GameOver("Good Game");
        }
        public void TakeDemonDamage(Border demonBorder, TextBlock currentTextblock)
        {
            demonBorder.BorderBrush = Brushes.Orange;
            demonBorder.Background = Brushes.Red;
            currentTextblock.Foreground = Brushes.Purple;
            demonBorder.Child.Opacity = 1;
            TakeDamage();
        }
        public void GameOver(string finalInscription)
        {
            Timer.StopTimer(outputTextBlock ,finalInscription);
            clickable.Button.Content = "Restart";
            clickable.Button.Visibility = Visibility.Visible;
            currentHP = maxHP;
            foreach (var border in clickableBorderList)
            {
                border.MouseDown -= clickable.Border_MouseDown;
                border.Child.Opacity = 1;
            }
            return;
        }
    }
}
