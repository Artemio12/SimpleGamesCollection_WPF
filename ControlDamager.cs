using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatchGame1
{
    internal class ControlDamager
    {
        private BaseDamager damager;
        public ControlDamager(BaseDamager damager)
        {
            this.damager = damager;
        }
        public void TakeBorderDamage(Border damageBorder, TextBlock currentTextBlock)
        {
            damageBorder.BorderBrush = Brushes.Orange;
            damageBorder.Background = Brushes.Red;
            currentTextBlock.Foreground = Brushes.Purple;
            damager.TakeDamage();
        }
    }
}
