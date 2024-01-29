using System.Windows;
using System.Windows.Controls;

namespace MatchGame1
{
    /// <summary>
    /// Логика взаимодействия для GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window, IEndable
    {
        private IClickable clickable;
        private BaseDamager damager;
        private BaseTimer timer;
        private Button button;

        public GameOverWindow(IClickable clickable, BaseDamager damager, BaseTimer timer)
        {
            InitializeComponent();

            this.clickable = clickable;
            this.damager = damager;
            this.timer = timer;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            button = clickable.StartButton;
            clickable.StartButton_Click(button, e);
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            clickable.MainMenuButton_Click(this, e);
            this.Close();
        }
        
        public void GameOver(string finalInscription)
        {
            timer.StopTimer();
            finalTextBlock.Text = finalInscription;
            clickable.StartButton.Content = "Restart";
            clickable.StartButton.Visibility = Visibility.Visible;
            damager.CurrentHP = damager.MaxHP;
            this.ShowDialog();
        }
    }
}
