using System.Windows;


namespace WPFGamesCollection
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (gameComboBox.SelectedIndex)
            {
                case 0:
                    MemoryCard memoryCard = new MemoryCard { MainWindow = this };
                    memoryCard.Show();
                    this.Visibility = Visibility.Collapsed;
                    break;
                default:
                    MessageBox.Show("Please, select a game");
                    break;
            }
            gameComboBox.SelectedIndex = -1;
        }
    }
}
