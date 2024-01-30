using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFGamesCollection
{
    internal class MemoryCardFacade :BaseGameFacade
    {
        private MemoryCardStruct gameStruct;
        private IClickable clickable;
        private IEnumerable<Border> clickableBorders;

        private Stack<Border> selectBorderStack = new Stack<Border>(2);

        private BaseTimer myTimer;
        private BaseDamager damager;
        private DamageBorder damageBorder;

        public Border CurrentBorder {  private get; set; }


        private int matchesFound;

        public MemoryCardFacade(IClickable clickable, ref MemoryCardStruct gameStruct) 
        {
            this.clickable = clickable;
            this.gameStruct = gameStruct;

            CreateComponent();
        }

        private void CreateComponent()
        {
            clickableBorders = from text in gameStruct.bordersOnGrid
                               where text.Tag.ToString() == nameof(EnumTags.Clickable)
                               select text;

            damager = new TextHeartsDamager(ref gameStruct.HPBar, gameStruct.emojiHeart, gameStruct.damagedHeart);
            damageBorder = new DamageBorder(damager);
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
        public override void PlayingGame()
        {
            FindMatch(CurrentBorder);
        }
        public override void GameOver(string finalInscription)
        {
            endable.GameOver(finalInscription);
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

        private void FindMatch(Border currentBorder)
        {
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
            myTimer.CurrentTime += gameStruct.bonusTime;
            matchCount++;
            if (matchCount == gameStruct.matchesFound) GameOver("You win!");
        }

        protected override void Restart()
        {
            gameStruct.outputTextBlock.Text = "Timer";
            matchesFound = 0;
            foreach (var border in clickableBorders)
            {
                border.Child.Opacity = 0;
                border.Background = Brushes.White;
                border.BorderBrush = Brushes.Black;
                (border.Child as TextBlock).Foreground = Brushes.Black;
            }

            SetUp();
        }
    }
}
