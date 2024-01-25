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
using System.Reflection;

namespace MatchGame1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public enum EnumTags
    {
        NotActive,
        Clickable,
    }

    public struct MemoryCardGameStruct
    {
        public IEnumerable<Border> bordersOnGrid;
        public StackPanel HPBar;
        public TextBlock outputTextBlock;
        public List<string> emojiList;
        public string emojiDamager;
        public string emojiHeart;
        public string damagedHeart;
        public int startTime;
        public int countSelectedMatches;
        public int matchesFound;
        public int bonusTime;
    }

    public partial class MemoryCard :Window, IClickable
    {
        private MemoryCardGameStruct gameStruct;
        private FindMatchManager gameManager;
        private Stack<Border> selectBorderStack = new Stack<Border>(2);
        private MainWindow mainWindow;
        public MainWindow MainWindow { get { return mainWindow; } set { mainWindow = value; } }
        private Button startButton;
        public Button StartButton => startButton;

        private int matchesFound;

        public MemoryCard()
        {
            InitializeComponent();

            InitializeStruct();
            gameManager = new FindMatchManager(this, ref gameStruct);
        }

        private void InitializeStruct()
        {
            gameStruct.bordersOnGrid = mainGrid.Children.OfType<Border>();
            gameStruct.HPBar = HPBar;
            gameStruct.emojiList = new List<string>()
            {
                "🐶", "🐶",
                "🐱", "🐱",
                "🐨", "🐨",
                "🐻", "🐻",
                "🐸", "🐸",
                "🦊", "🦊",
                "🦝", "🦝",
                "👹", "👹",
            };
            gameStruct.outputTextBlock = timeTextBlock;
            gameStruct.emojiDamager = "👹";
            gameStruct.emojiHeart = "❤️";
            gameStruct.damagedHeart = "🤍";
            gameStruct.startTime = 50;
            gameStruct.countSelectedMatches = 2;
            gameStruct.bonusTime = 10;
        }

        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startButton = sender as Button;
            startButton.Visibility = Visibility.Hidden;
            if (startButton.Content.ToString() == "Restart")
            {
                gameManager.Restart();
            }
            gameManager.SetUp();
        }

        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border currentBorder = sender as Border;
            TextBlock currentTextBlock = currentBorder.Child as TextBlock;

            if (selectBorderStack.Count < gameStruct.countSelectedMatches)
                FindCards(currentBorder, currentTextBlock);

            if (selectBorderStack.Count == gameStruct.countSelectedMatches)
                CheckMatch(currentTextBlock);
        }

        private async void CheckMatch(TextBlock currentTextBlock)
        {
            if (selectBorderStack.All(p => (p.Child as TextBlock).Text == currentTextBlock.Text))
            {
                MatchCount(ref matchesFound);
                foreach (var element in selectBorderStack)
                {
                    element.BorderBrush = Brushes.Blue;
                    element.Background = Brushes.Green;
                }

                selectBorderStack.Clear();
            }
            else
            {
                if (StartButton.Visibility == Visibility.Visible)
                {
                    return;
                }
                
                await Task.Delay(TimeSpan.FromSeconds(0.3f));

                foreach (var border in selectBorderStack)
                {
                    border.Child.Opacity = 0;
                }
                selectBorderStack.Clear();
            }
        }

        private void FindCards(Border currentBorder, TextBlock currentTextBlock)
        {
            if (currentBorder.Child.Opacity == 0)
            {
                currentBorder.Child.Opacity = 1;

                if (currentTextBlock.Text != gameStruct.emojiDamager) selectBorderStack.Push(currentBorder);
                else
                {
                    TakeDemonDamage(currentBorder, currentTextBlock);
                    if (selectBorderStack.Count != 0) selectBorderStack.Pop().Child.Opacity = 0;
                }
                return;
            }
        }

        public void TakeDemonDamage(Border demonBorder, TextBlock currentTextblock)
        {
            demonBorder.BorderBrush = Brushes.Orange;
            demonBorder.Background = Brushes.Red;
            currentTextblock.Foreground = Brushes.Purple;
            demonBorder.Child.Opacity = 1;
            gameManager.Damager.TakeDamage();
        }

        private void MatchCount(ref int matchCount)
        {
            gameManager.MyTimer.CurrentTime += gameStruct.bonusTime;
            matchCount++;
            if (matchCount == 7) gameManager.GameOver("You win");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.Visibility = Visibility.Visible;
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Visibility = Visibility.Visible;
        }
    }
}
