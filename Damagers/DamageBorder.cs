using System.Windows.Controls;
using System.Windows.Media;

namespace WPFGamesCollection
{
    public class DamageBorder
    {
        private BaseDamager damager;
        public DamageBorder(BaseDamager damager)
        {
            this.damager = damager;
        }
        public void TakeDamage(Border damageBorder)
        {
            damageBorder.BorderBrush = Brushes.Orange;
            damageBorder.Background = Brushes.Red;
            (damageBorder.Child as TextBlock).Foreground = Brushes.Purple;
            damager.TakeDamage();
        }
    }
}
