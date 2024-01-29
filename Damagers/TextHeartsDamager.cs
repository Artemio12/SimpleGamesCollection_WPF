using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace WPFGamesCollection
{
    internal class TextHeartsDamager :BaseDamager
    {
        public StackPanel HPBar;
       
        private string fillHeart;
        private string damagedHeart;

        public TextHeartsDamager(ref StackPanel HPBar, string fillHeart, string damagedHeart)
        {
            this.HPBar = HPBar;
            this.fillHeart = fillHeart;
            this.damagedHeart = damagedHeart;

            maxHP = HPBar.Children.OfType<TextBlock>().Count();
            currentHP = maxHP;
        }

        public override void TakeDamage()
        {
            foreach (var element in HPBar.Children.OfType<TextBlock>())
            {
                if (element.Text == fillHeart)
                {
                    element.Text = damagedHeart;
                    element.Foreground = Brushes.Black;

                    currentHP--;
                    break;
                }
            }
            if (currentHP == 0) End.GameOver("Game over!");
        }
    }
}
