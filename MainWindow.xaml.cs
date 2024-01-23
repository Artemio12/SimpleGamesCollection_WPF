using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace MatchGame1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        SimpleGameFactory gameFactory;
        private Window window;
        public MainWindow()
        {
            InitializeComponent();
            gameFactory = new SimpleGameFactory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (gameComboBox.SelectedIndex)
            {
                case 0:
                    MemoryCard memoryCard = new MemoryCard();
                    memoryCard.MainWindow = this;
                    memoryCard.Show();
                    this.Visibility = Visibility.Collapsed;
                    break;
                default:
                    MessageBox.Show("Please, select game");
                    break;
            }
        }

    }
}
