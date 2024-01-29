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
    internal class MemoryCardFacade :BaseGameFacade
    {
        private IClickable clickable;
        
        private IEnumerable<Border> clickableBorders;

        private MemoryCardStruct gameStruct;

        private DecreasingTimer myTimer;
        public DecreasingTimer MyTimer => myTimer;

        private BaseDamager damager;
        public BaseDamager Damager => damager;

        public MemoryCardFacade(IClickable clickable, ref MemoryCardStruct gameStruct) 
        {
            this.clickable = clickable;
            this.gameStruct = gameStruct;

            InitializeObject();
        }

        private void InitializeObject()
        {
            clickableBorders = from text in gameStruct.bordersOnGrid
                               where text.Tag.ToString() == nameof(EnumTags.Clickable)
                               select text;

            damager = new TextHeartsDamager(ref gameStruct.HPBar, gameStruct.emojiHeart, gameStruct.damagedHeart);
            myTimer = new DecreasingTimer(damager, gameStruct.outputTextBlock);
            endable = new GameOverTextBlock(clickable, clickableBorders, damager, myTimer);
            damager.End = endable;

        }

        public override void StartGame(Button button)
        {
            button.Visibility = Visibility.Hidden;
            if (button.Content.ToString() == "Restart") Restart();
            else SetUp();
        }
  
        protected override async void SetUp()
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
                border.MouseDown += clickable.ClickableBorder_MouseDown;
            }
            
            myTimer.StartTimer(gameStruct.startTime);
        }
        protected override void Restart()
        {
            gameStruct.outputTextBlock.Text = "Timer";

            foreach (var border in clickableBorders)
            {
                border.Child.Opacity = 0;
                border.Background = Brushes.White;
                border.BorderBrush = Brushes.Black;
                (border.Child as TextBlock).Foreground = Brushes.Black;
            }
            SetUp();
        }

        public override void GameOver(string finalInscription)
        {
            endable.GameOver(finalInscription);
        }
    }
}
