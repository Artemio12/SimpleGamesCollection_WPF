using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatchGame1
{
    internal class FindMatchManager :BaseGameManager, IEndable
    {
        private IClickable clickable;
        private IEnumerable<Border> clickableBorders;
        private MemoryCardGameStruct gameStruct;
        private Timer myTimer;
        public Timer MyTimer => myTimer;

        private MemoryCardDamager damager;
        public MemoryCardDamager Damager => damager;

        public FindMatchManager(IClickable clickable, ref MemoryCardGameStruct gameStruct) : this(clickable)
        {
            this.gameStruct = gameStruct;
            damager = new MemoryCardDamager(clickable, ref gameStruct.HPBar, gameStruct.outputTextBlock)
            { End = this };
            myTimer = new Timer(damager);
            SelectClickableBorders();
        }
        private FindMatchManager(IClickable clickable)
        {
            this.clickable = clickable;
        }
        private void SelectClickableBorders()
        {
            clickableBorders = from text in gameStruct.bordersOnGrid
                               where text.Tag.ToString() == nameof(EnumTags.Clickable)
                               select text;
        }

        public override async void SetUp()
        {
            Random random = new Random();
            List<string> list = new List<string>();

            foreach (string str in gameStruct.emojiList)
            {
                list.Add(str);
            }

            foreach (Border border in clickableBorders)
            {
                int index = random.Next(0, list.Count);
                
                (border.Child as TextBlock).Text = list[index];
                border.Child.Opacity = 1;
                list.RemoveAt(index);

                if ((border.Child as TextBlock).Text == gameStruct.emojiDamager)
                {
                    (border.Child as TextBlock).Foreground = Brushes.Red;
                }
            }

            foreach (var heart in gameStruct.HPBar.Children.OfType<TextBlock>())
            {
                heart.Text = gameStruct.emojiHeart;
                heart.Foreground = Brushes.IndianRed;
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
            }

            await Task.Delay(TimeSpan.FromSeconds(1f));

            foreach (Border border in clickableBorders)
            {
                border.Child.Opacity = 0;
                border.MouseDown += clickable.Border_MouseDown;
            }
            
            damager.ClickableBorders = clickableBorders;
            myTimer.StartTimer(gameStruct.startTime);
        }

        public override void Restart()
        {
            gameStruct.outputTextBlock.Text = "Timer";

            foreach (var border in clickableBorders)
            {
                border.Child.Opacity = 0;
                border.Background = Brushes.White;
                border.BorderBrush = Brushes.Black;
                (border.Child as TextBlock).Foreground = Brushes.Black;
            }
        }

        public override void GameOver(string finalInscription)
        {
            myTimer.StopTimer(gameStruct.outputTextBlock, finalInscription);
            clickable.StartButton.Content = "Restart";
            clickable.StartButton.Visibility = Visibility.Visible;
            foreach (var border in clickableBorders)
            {
                border.MouseDown -= clickable.Border_MouseDown;
                border.Child.Opacity = 1;
            }
            return;
        }
    }
}
