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

    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private Button button;

        private List<Border> selectBorderList = new List<Border>();
        private List<TextBlock> HPHeartList = new List<TextBlock>();
        private List<Border> clickableBorderList = new List<Border>();

        private int timeBeforeDamage = 100;
        private int countSelectedMatches = 2;
        private int demonCount;
        private int hp = 3;
        private int matchesFound;
        private int bonusTime = 20;
        private int i = 0;
       
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;
            button.Visibility = Visibility.Hidden;
            if (button.Content.ToString() == "Restart")
            {
                RestartGame();
            }
            SetUpGame();
        }
        public async void SetUpGame()
        {
            Random random = new Random();
            List<string> animalEmoji = new List<string>()
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

            var clickableBorder = from text in mainGrid.Children.OfType<Border>()
                                  where text.Tag.ToString() == nameof(EnumTags.Clickable)
                                  select text;

            foreach (Border border in clickableBorder)
            {
                int index = random.Next(0, animalEmoji.Count);
                (border.Child as TextBlock).Text = animalEmoji[index];
                border.Child.Opacity = 1;
                animalEmoji.RemoveAt(index);

                if ((border.Child as TextBlock).Text == "👹")
                {
                    demonCount++;   
                    (border.Child as TextBlock).Foreground = Brushes.Red;
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(3f));

            foreach (Border border in clickableBorder)
            {
                border.Child.Opacity = 0;
                clickableBorderList.Add(border);
            }

            StartTimer();

            foreach (var heart in HPBar.Children.OfType<TextBlock>())
            {
                heart.Text = "❤️";
                heart.Foreground = Brushes.IndianRed;
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
                HPHeartList.Add(heart);
            }   
        }
        private void RestartGame()
        {
            timeTextBlock.Text = "Timer";
            timeBeforeDamage = 100;
            hp = 3;
            foreach (var border in clickableBorderList)
            {
                border.Child.Opacity = 0;
                border.Background = Brushes.White;
                border.BorderBrush = Brushes.Black;
                (border.Child as TextBlock).Foreground = Brushes.Black;
                border.MouseDown += Border_MouseDown;
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border currentBorder = sender as Border;
            TextBlock currentTextBlock = currentBorder.Child as TextBlock;

            if (currentTextBlock.Text == "👹")
            {
                TakeDemonDamage(currentBorder, currentTextBlock);
                if (selectBorderList.Count != 0)
                {
                    selectBorderList[i].Child.Opacity = 0;
                    selectBorderList.Clear();
                }
            }
            else FindMatch(currentBorder, currentTextBlock);
        }
        private async void FindMatch(Border currentBorder, TextBlock currentTextBlock)
        {
            if (selectBorderList.Count < countSelectedMatches && currentBorder.Child.Opacity == 0)
            {
                currentBorder.Child.Opacity = 1;
                selectBorderList.Add(currentBorder);
            }

            if(selectBorderList.Count == countSelectedMatches && (selectBorderList[i].Child as TextBlock).Text == (selectBorderList[i + 1].Child as TextBlock).Text)
            { 
                MatchCount(ref matchesFound, ref timeBeforeDamage);
                selectBorderList[i].BorderBrush = Brushes.Blue;
                selectBorderList[i].Background = Brushes.Green;
                selectBorderList[i + 1].BorderBrush = Brushes.Blue;
                selectBorderList[i + 1].Background = Brushes.Green;
               
                selectBorderList.Clear();
            }
            else if (selectBorderList.Count == countSelectedMatches)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.3f));
                foreach (var border in selectBorderList)
                {
                    border.Child.Opacity = 0;
                }
                selectBorderList.Clear();
            }
        }
        private void TakeDemonDamage(Border border, TextBlock curretTextblock)
        {
            border.BorderBrush = Brushes.Orange;
            border.Background = Brushes.Red;
            curretTextblock.Foreground = Brushes.Purple;
            border.Child.Opacity = 1;
            TakeDamage();
        }
        private void StartTimer()
        {
            timer.Interval = TimeSpan.FromSeconds(.1f);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeBeforeDamage--;
            timeTextBlock.Text = (timeBeforeDamage / 10f).ToString("0.0s");
            if (matchesFound == (clickableBorderList.Count-demonCount)/countSelectedMatches && timeBeforeDamage > 0)
            {
                GameOver("You win!");
            }
            else if (timeBeforeDamage == 0)
            {
                TakeDamage();
                timeBeforeDamage = 50;
            } 
        }
        private void StopTimer(string finalText)
        {
            timeTextBlock.Text = finalText;
            timer.Stop();
        }
        private void TakeDamage()
        {
            foreach (var element in HPHeartList)
            {
                if (element.Text == "❤️")
                {
                    element.Text = "🤍";
                    hp--;
                    break;
                }
            }
            if (hp == 0) GameOver("Good Game!");
        }

        private void GameOver(string finalInscription)
        {
            StopTimer(finalInscription);
            button.Content = "Restart";
            button.Visibility = Visibility.Visible;
            foreach (var border in clickableBorderList)
            {
                border.MouseDown -= Border_MouseDown;
                border.Child.Opacity = 1;
            }
            return;
        }
        private void MatchCount(ref int matchCount, ref int timeBeforeDamage)
        {
            timeBeforeDamage += bonusTime;
            matchCount++;
        }
    }
}
