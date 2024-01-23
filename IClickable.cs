using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MatchGame1
{
    public interface IClickable
    {
        Button Button { get; }
        void Button_Click(object sender, RoutedEventArgs e);
        void Border_MouseDown(object sender, MouseButtonEventArgs e);
    }
}
