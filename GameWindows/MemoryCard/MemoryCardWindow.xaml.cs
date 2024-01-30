using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFGamesCollection
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

        private MainWindow mainWindow;
        public MainWindow MainWindow 
        { get => mainWindow;  set => mainWindow = value; }

        private Button startButton;
        public Button StartButton => startButton;

        public MemoryCard()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeStruct();
            gameManager = new MemoryCardFacade(this, ref gameStruct);
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
            gameManager.StartGame(startButton);
        }
        public void ClickableBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border currentBorder = sender as Border;
            gameManager.CurrentBorder = currentBorder;
            gameManager.PlayingGame();
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
