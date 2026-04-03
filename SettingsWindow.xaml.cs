using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Reversi
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public bool IsPlayerBlack { get; private set; }
        public bool IsShowMoves { get; private set; }
        public int Difficulty { get; private set; }

        public SettingsWindow(bool prevPlayerBlack, bool prevShowMoves, int prevDifficulty)
        {
            InitializeComponent();

            if (prevPlayerBlack) Black.IsChecked = true;
            else White.IsChecked = true;

            if (prevShowMoves) MovesYes.IsChecked = true;
            else MovesNo.IsChecked = true;

            if (prevDifficulty == 1) Easy.IsChecked = true;
            else if (prevDifficulty == 2) Medium.IsChecked = true;
            else if (prevDifficulty == 3) Hard.IsChecked = true;

        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            IsPlayerBlack = (Black.IsChecked == true);
            IsShowMoves = (MovesYes.IsChecked == true);
            if (Easy.IsChecked == true) Difficulty = 1;
            else if (Medium.IsChecked == true) Difficulty = 2;
            else if (Hard.IsChecked == true) Difficulty = 3;

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
