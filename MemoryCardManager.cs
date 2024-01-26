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
    internal class MemoryCardManager :BaseGameManager, IEndable
    {
        private IClickable clickable;
        private IEnumerable<Border> clickableBorders;
        private MemoryCardStruct gameStruct;

        private Timer myTimer;
        public Timer MyTimer => myTimer;

        private BaseDamager damager;
        public BaseDamager Damager => damager;

        public MemoryCardManager(IClickable clickable, ref MemoryCardStruct gameStruct) 
        {
            this.clickable = clickable;
            this.gameStruct = gameStruct;

            damager = new MemoryCardDamager(ref gameStruct.HPBar, gameStruct.emojiHeart, gameStruct.damagedHeart)
            { End = this };
            myTimer = new Timer(damager, gameStruct.outputTextBlock);

            SelectClickableBorders();
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
            damager.CurrentHP = 3;
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
