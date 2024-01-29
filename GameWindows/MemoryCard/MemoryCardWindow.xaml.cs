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

    public struct MemoryCardStruct
    {
        public IEnumerable<Border> bordersOnGrid;
        public StackPanel HPBar;
        public TextBlock outputTextBlock;
        public List<string> emojiList;
        public string emojiDamager;
        public string emojiHeart;
        public string damagedHeart;
        public int startTime;
        public int countSelectedCards;
        public int matchesFound;
        public int bonusTime;
    }

    public partial class MemoryCard :Window, IClickable
    {
        private MemoryCardStruct gameStruct;
        private MemoryCardFacade gameManager;
        private Stack<Border> selectBorderStack = new Stack<Border>(2);

        private DamageBorder damageBorder;

        private MainWindow mainWindow;
        public MainWindow MainWindow 
        { get => mainWindow;  set => mainWindow = value; }

        private Button startButton;
        public Button StartButton => startButton;

        private Button mainMenuButton;
        public Button MainMenuButton => mainMenuButton;

        private int matchesFound;

        public MemoryCard()
        {
            InitializeComponent();

            InitializeStruct();
            gameManager = new MemoryCardFacade(this, ref gameStruct);
            damageBorder = new DamageBorder(gameManager.Damager);
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
            gameStruct.countSelectedCards = 2;
            gameStruct.bonusTime = 10;
            gameStruct.matchesFound = 7;
        }

        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startButton = sender as Button;
            if (matchesFound != 0) matchesFound = 0;
            gameManager.StartGame(startButton);
        }
        public void ClickableBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border currentBorder = sender as Border;
            TextBlock currentTextBlock = currentBorder.Child as TextBlock;

            if (currentBorder.Child.Opacity == 0)
                AddCard(currentBorder, currentTextBlock);

            if (selectBorderStack.Count == gameStruct.countSelectedCards)
                CheckCardsMatch(currentTextBlock);
        }

        private void AddCard(Border currentBorder, TextBlock currentTextBlock)
        {
            if (selectBorderStack.Count < gameStruct.countSelectedCards)
            {
                currentBorder.Child.Opacity = 1;

                if (currentTextBlock.Text != gameStruct.emojiDamager) selectBorderStack.Push(currentBorder);
                else
                {
                    damageBorder.TakeDamage(currentBorder);
                    if (selectBorderStack.Count != 0) selectBorderStack.Pop().Child.Opacity = 0;
                }
                return;
            }
        }
        private async void CheckCardsMatch(TextBlock currentTextBlock)
        {
            if (selectBorderStack.All(p => (p.Child as TextBlock).Text == currentTextBlock.Text))
            {
                CountMatches(ref matchesFound);
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
        private void CountMatches(ref int matchCount)
        {
            gameManager.MyTimer.CurrentTime += gameStruct.bonusTime;
            matchCount++;
            if (matchCount == gameStruct.matchesFound) gameManager.GameOver("You win!");
        }

        public void MainMenuButton_Click(object sender, RoutedEventArgs e)
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
