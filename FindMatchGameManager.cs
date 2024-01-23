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
    internal class FindMatchGameManager :BaseGameManager
    {
        private IClickable clickable;
        private MemoryCardGameStruct gameStruct;
        private List<Border> clickableBorderList = new List<Border>();
        private Timer myTimer;
        public Timer MyTimer => myTimer;

        private MemoryCardDamager damager;
        public MemoryCardDamager Damager => damager;

        public FindMatchGameManager(IClickable clickable, ref MemoryCardGameStruct gameStruct) : this(clickable)
        {
            this.gameStruct = gameStruct;
            damager = new MemoryCardDamager(clickable, ref gameStruct.HPBar, gameStruct.outputTextBlock);
            myTimer = new Timer(damager);
        }

        private FindMatchGameManager(IClickable clickable)
        {
            this.clickable = clickable;
        }

        public override async void SetUp()
        {
            Random random = new Random();
            List<string> list = new List<string>();

            foreach (string str in gameStruct.emojiList)
            {
                list.Add(str);
            }

            var clickableBorders = from text in gameStruct.bordersOnGrid
                                  where text.Tag.ToString() == nameof(EnumTags.Clickable)
                                  select text;

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
                clickableBorderList.Add(border);
            }
            
            damager.ClickableBorderList = clickableBorderList;
            myTimer.StartTimer(gameStruct.startTime);
        }

        public override void Restart()
        {
            gameStruct.outputTextBlock.Text = "Timer";

            foreach (var border in clickableBorderList)
            {
                border.Child.Opacity = 0;
                border.Background = Brushes.White;
                border.BorderBrush = Brushes.Black;
                (border.Child as TextBlock).Foreground = Brushes.Black;
                border.MouseDown += clickable.Border_MouseDown;
            }
            clickableBorderList.Clear();
        }
    }
}
