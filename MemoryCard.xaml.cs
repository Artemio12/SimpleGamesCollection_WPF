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
        private Stack<Border> selectBorderStack = new Stack<Border>(2);
        private MainWindow mainWindow;
        public MainWindow MainWindow { get { return mainWindow; } set { mainWindow = value; } }
        private MemoryCardGameStruct gameStruct;
        private FindMatchGameManager gameManager;       

        private Button button;
        public Button Button => button;
      
        private int matchesFound;

        public MemoryCard()
        {
            InitializeComponent();

            InitializeStruct();
            gameManager = new FindMatchGameManager(this, ref gameStruct);
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

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;
            button.Visibility = Visibility.Hidden;
            if (button.Content.ToString() == "Restart")
            {
                gameManager.Restart();
            }
            gameManager.SetUp();
        }

        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border currentBorder = sender as Border;
            TextBlock currentTextBlock = currentBorder.Child as TextBlock;

            if (currentBorder.Child.Opacity == 0) FindMatch(currentBorder, currentTextBlock);
        }

        private async void FindMatch(Border currentBorder, TextBlock currentTextBlock)
        {
            if (selectBorderStack.Count < gameStruct.countSelectedMatches)
            {
                currentBorder.Child.Opacity = 1;

                if (currentTextBlock.Text != gameStruct.emojiDamager) selectBorderStack.Push(currentBorder);
                else
                {
                    gameManager.Damager.TakeDemonDamage(currentBorder, currentTextBlock);
                    if (selectBorderStack.Count != 0) selectBorderStack.Pop().Child.Opacity = 0;
                }
            }

            if (selectBorderStack.Count == gameStruct.countSelectedMatches && selectBorderStack.All(p => (p.Child as TextBlock).Text == currentTextBlock.Text))
            {
                MatchCount(ref matchesFound);
                foreach (var element in selectBorderStack)
                {
                    element.BorderBrush = Brushes.Blue;
                    element.Background = Brushes.Green;
                }

                selectBorderStack.Clear();
            }
            else if (selectBorderStack.Count == gameStruct.countSelectedMatches)
            {
                if (Button.Visibility == Visibility.Visible)
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
        private void MatchCount(ref int matchCount)
        {
            gameManager.MyTimer.CurrentTime += gameStruct.bonusTime;
            matchCount++;
            if (matchesFound == 7) gameManager.Damager.GameOver("You win");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Visibility = Visibility.Visible;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.Visibility = Visibility.Visible;

        }
    }
}
