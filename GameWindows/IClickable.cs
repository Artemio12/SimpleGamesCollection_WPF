using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFGamesCollection
{
    public interface IClickable
    {
        Button StartButton { get; }
        void StartButton_Click(object sender, RoutedEventArgs e);
        void MainMenuButton_Click(object sender, RoutedEventArgs e);
        void ClickableBorder_MouseDown(object sender, MouseButtonEventArgs e);
    }
}
