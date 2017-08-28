using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;

namespace WPFTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            label2.Cursor = Cursors.Hand;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonBig_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                buttonBig.Content = "1";
            }
            else if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                buttonBig.Content = "2";
            }
        }

        private void frame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonBig_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                GameList tempest = SearchStore.gmze[listBox.SelectedIndex];
                BitmapImage test = new BitmapImage(new Uri(tempest.JpgPath));

                imageBord.Source = test;

                label.Content = tempest.GameName;
                label2.Content = "Открыть страничку игры";
                if (tempest.rub)
                    label1.Content = tempest.vaCost.ToString() + "руб";
                else
                    label1.Content = tempest.vaCost.ToString() + '$';
            }
        }
        
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "Search")
                textBox.Text = "";
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                label.Content = "";
                label1.Content = "";
                label2.Content = "";
                imageBord.Source = null;
                listBox.Items.Clear();
                SearchStore.TotalSearh(textBox.Text);
                foreach(GameList tmp in SearchStore.gmze)
                {
                    listBox.Items.Add(tmp.GameName + " (" + tmp.storeCho + ")");
                }
            }
        }

        private void label2_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void label2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
                System.Diagnostics.Process.Start(SearchStore.gmze[listBox.SelectedIndex].refToStore);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
