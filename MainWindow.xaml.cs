using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reversi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double SquareSize = 50;
        private Game _game = new Game(true);
        private bool IsPlayerBlack = true;
        private bool IsShowMoves = true;
        private int Difficulty = 1;
        private List<Move> _moves = new List<Move>();
        private bool playerPasses = false;
        private bool computerPasses = false;
        private bool IsPlayersTurn = false;
        private bool EndOfGameCalled = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void gameCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsPlayersTurn) return;
            Point p = e.GetPosition(gameCanvas);
            int row = (int)(p.Y / SquareSize);
            int col = (int)(p.X / SquareSize);
            if (!IsLegalMove(row, col, out int move)) return;

            _game.PlayMove(2, _moves[move]);

            IsPlayersTurn = false;
            computerPasses =  ComputerPlay();
            UpdateScore();
            ReDraw();
            playerPasses = StartPlayerPlay();
            if (computerPasses && playerPasses)
                EndOfGame();
        }

        private void EndOfGame()
        {
            if (!EndOfGameCalled)
            {
                _game.CountScore(out int you, out int me);
                if (me > you)
                    MessageBox.Show("Sorry, you lost", "Game Over",
                    MessageBoxButton.OK, MessageBoxImage.None);
                else if (me < you)
                    MessageBox.Show("Congratulations, you won", "Game Over",
                    MessageBoxButton.OK, MessageBoxImage.None);
                else
                    MessageBox.Show("It's a draw", "Game Over",
                    MessageBoxButton.OK, MessageBoxImage.None);
                EndOfGameCalled = true;
            }
        }

        private void UpdateScore()
        {
            _game.CountScore(out int you, out int me);
            YourScore.Text = you.ToString("D2");
            MyScore.Text = me.ToString("D2");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // The window's total size (including title bar and borders)
            Debug.WriteLine($"Window: {this.Width} x {this.Height}");

            double x = gameCanvas.ActualWidth;
            double y = gameCanvas.ActualHeight;
            SquareSize = Math.Min(x, y) / 8.0;

            ReDraw();
        }

        private void ReDraw()
        {
            gameCanvas.Children.Clear();

            // draw the board's vertical lines
            for (int i = 0; i <= 8; i++)
            {
                double x = i * SquareSize;
                Line line = new Line();
                line.X1 = x;
                line.Y1 = 0;
                line.X2 = x;
                line.Y2 = SquareSize * 8;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 2;
                gameCanvas.Children.Add(line);
            }

            // draw the board's horizontal lines
            for (int i = 0; i <= 8; i++)
            {
                double y = i * SquareSize;
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = y;
                line.X2 = SquareSize * 8;
                line.Y2 = y;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 2;
                gameCanvas.Children.Add(line);
            }

            // draw the pieces
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    // board values are as follows:
                    // 0 = no piece
                    // 1 = computer piece
                    // 2 = player piece
                    Ellipse piece = new Ellipse();
                    piece.Width = SquareSize * 0.86;
                    piece.Height = SquareSize * 0.86;
                    Canvas.SetLeft(piece, (col + 0.07) * SquareSize);
                    Canvas.SetTop(piece, (row + 0.07) * SquareSize);
                    if ((IsPlayerBlack && _game.GetPiece(row, col) == 2) ||
                        (!IsPlayerBlack && _game.GetPiece(row, col) == 1))
                    {
                        // draw a black piece
                        piece.Fill = Brushes.Black;
                        //Debug.WriteLine($"black piece at {row}, {col}");
                    }
                    else if ((IsPlayerBlack && _game.GetPiece(row, col) == 1) ||
                        (!IsPlayerBlack && _game.GetPiece(row, col) == 2))
                    {
                        // draw a white piece
                        piece.Fill = Brushes.White;
                        //Debug.WriteLine($"white piece at {row}, {col}");
                    }
                    gameCanvas.Children.Add(piece);
                }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(IsPlayerBlack, IsShowMoves, Difficulty);
            settingsWindow.Owner = this;
            bool? result = settingsWindow.ShowDialog(); // Modal dialog

            if (result == true)  // User clicked OK
            {
                if (IsPlayerBlack != settingsWindow.IsPlayerBlack)
                {
                    IsPlayerBlack = settingsWindow.IsPlayerBlack;
                    NewGame();
                }
                IsShowMoves = settingsWindow.IsShowMoves;
                Difficulty = settingsWindow.Difficulty;

                // change the colors in the header
                UpdateHeader();

                // save the properties to disk
                Properties.Settings.Default.IsPlayerBlack = IsPlayerBlack;
                Properties.Settings.Default.ShowMoves = IsShowMoves;
                Properties.Settings.Default.Difficulty = Difficulty;
                Properties.Settings.Default.Save();
            }

        }

        private void UpdateHeader()
        {
            // change the colors in the header
            if (IsPlayerBlack)
            {
                YourColor.Fill = Brushes.Black;
                MyColor.Fill = Brushes.White;
            }
            else
            {
                YourColor.Fill = Brushes.White;
                MyColor.Fill = Brushes.Black;
            }
        }

        private void NewGame()
        {
            // set the game to its initial state
            _game.NewGame(IsPlayerBlack);
            // redraw the board and pieces
            ReDraw();
            // change the colors in the header
            UpdateHeader();

            computerPasses = false;
            playerPasses = false;
            EndOfGameCalled = false;

            // if computer is black, it plays first
            if (!IsPlayerBlack)
                computerPasses = ComputerPlay();

            // update the score
            UpdateScore();
            ReDraw();

            // player's turn
            playerPasses = StartPlayerPlay();
            if (computerPasses && playerPasses)
                EndOfGame();
        }

        private bool ComputerPlay()
        {
            Debug.WriteLine("starting computer play");
            bool pass = _game.ComputerPlay(Difficulty);
            //if (pass) Debug.WriteLine("computer has no moves");
            return pass;
            //return true;
        }

        private bool StartPlayerPlay()
        {
            int row = 0;
            int col = 0;

            Debug.WriteLine("starting player play");
            _moves = _game.GetMoves(2);
            Debug.WriteLine($"player has {_moves.Count} moves");
            while (_moves.Count == 0)
            {
                Debug.WriteLine("player has no moves");
                computerPasses = ComputerPlay();
                if (computerPasses)
                {
                    EndOfGame();
                    return true;
                }
                _moves = _game.GetMoves(2);
            }

            //Debug.WriteLine("start player move list");
            if (IsShowMoves)
            {
                for (int i = 0; i < _moves.Count; i++)
                {
                    row = _moves[i].square.row;
                    col = _moves[i].square.col;
                    //Debug.WriteLine($"player move = {row + 1}, {col + 1}");

                    Ellipse piece = new Ellipse();
                    piece.Width = SquareSize * 0.86;
                    piece.Height = SquareSize * 0.86;
                    Canvas.SetLeft(piece, (col + 0.07) * SquareSize);
                    Canvas.SetTop(piece, (row + 0.07) * SquareSize);
                    if (IsPlayerBlack)
                    {
                        // draw a black piece
                        piece.Fill = Brushes.Black;
                        //Debug.WriteLine($"black piece at {row}, {col}");
                    }
                    else
                    {
                        // draw a white piece
                        piece.Fill = Brushes.White;
                        //Debug.WriteLine($"white piece at {row}, {col}");
                    }
                    piece.Opacity = 0.125;
                    gameCanvas.Children.Add(piece);
                }
            }

            IsPlayersTurn = true;
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // load the properties from disk
            Properties.Settings.Default.Reload();
            IsPlayerBlack = Properties.Settings.Default.IsPlayerBlack;
            IsShowMoves = Properties.Settings.Default.ShowMoves;
            Difficulty = Properties.Settings.Default.Difficulty;

            NewGame();
        }

        private bool IsLegalMove(int row, int col, out int move)
        {
            for (int i = 0; i < _moves.Count; i++)
            {
                if ((row == _moves[i].square.row) &&
                    (col == _moves[i].square.col))
                {
                    move = i;
                    return true;
                }
            }
            move = -1;
            return false;
        }
    }
}