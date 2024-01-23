using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatchGame1
{
    public enum EnumWindows 
    {
        MainWnidow,
        MemoryCard,

    }
    internal class SimpleGameFactory
    {
        public Window CreateGame(int index)
        {
            Window game = null;
            switch (index)
            {
                case -1:
                    game = new MainWindow();
                    break;
                case 0:
                    game = new MemoryCard();
                    break;
                default:
                    MessageBox.Show("Please, select the game");
                    break;
            }
            return game;
        }
    }
}
