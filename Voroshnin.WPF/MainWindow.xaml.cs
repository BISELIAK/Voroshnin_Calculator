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
using Voroshnin.Lib;

namespace Voroshnin.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tbCommand.Clear();

            Grid mainGrid = (Grid)this.Content;
            foreach (UIElement element in mainGrid.Children)
            {
                if (element is Grid buttonsGrid)
                {
                    foreach (UIElement child in buttonsGrid.Children)
                    {
                        if (child is Button button)
                        {
                            button.Click += Button_Click;
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string buttonText = ((Button)e.OriginalSource).Content.ToString();

            if (buttonText == "C")
            {
                tbCommandHistory.Clear();
                tbCommand.Clear();
            }
            else if (buttonText == "=")
            {
                try
                {
                    string expression = tbCommand.Text.Trim();

                    tbCommandHistory.Text = expression;

                    double result = Calculator.Run(expression);

                    tbCommand.Text = result.ToString("G15").Replace(",", ".");
                    tbCommandHistory.Text += $" = {result.ToString().Replace(".", ",")}";
                }
                catch (Exception ex)
                {
                    tbCommandHistory.Text = tbCommand.Text;
                    tbCommand.Text = $"Ошибка: {ex.Message}";
                }
            }
            else
            {
                tbCommand.Text += buttonText;
            }
        }
    }
}
