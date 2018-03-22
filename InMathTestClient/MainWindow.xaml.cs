using InMath.LexicalAnalysis;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InMathTestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            this.tbLoading.Visibility = Visibility.Visible;
            var input = this.tbInput.Text;
            var tokens = await Task.Run<List<LexicalToken>>(() =>
             {
                 var sfm = new LexicalStackMachine();
                 var results = sfm.Parse(input);
                 return results.Tokens;
             });

            // Stop timing.
            stopwatch.Stop();

            lbTokens.ItemsSource = tokens;
            this.tbLoading.Text = "Time elapsed: " + stopwatch.Elapsed;
        }
    }
}
