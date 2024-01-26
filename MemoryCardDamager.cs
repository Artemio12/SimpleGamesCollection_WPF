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
        public IEndable End { private get; set; }
        public StackPanel HPBar;
        private int currentHP;
        public int maxHP;

        public MemoryCardDamager(ref StackPanel HPBar)
        {
            maxHP = HPBar.Children.OfType<TextBlock>().Count();
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
                    element.Foreground = Brushes.Black;

                    currentHP--;
                    Console.WriteLine(currentHP.ToString() + "fd");
                    break;
                }
            }
            if (currentHP == 0) 
            {
                End.GameOver("Good Game");
                currentHP = maxHP;
            } 
        }
    }
}
