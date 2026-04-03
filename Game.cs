using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace Reversi
{
    internal class Game
    {
        // board values are as follows:
        // 0 = no piece
        // 1 = computer piece
        // 2 = player piece
        private int[,] board = new int[8, 8];
        private Random rand = new Random();

        public Game(bool IsPlayerBlack)
        {
            NewGame(IsPlayerBlack);
        }

        public void NewGame(bool IsPlayerBlack)
        {
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    // initially set all square to 0 = no piece
                    board[row, col] = 0;
                }

            // set four center piece
            if (IsPlayerBlack)
            {
                board[3, 3] = 1;
                board[4, 4] = 1;
                board[3, 4] = 2;
                board[4, 3] = 2;
            }
            else
            {
                board[3, 3] = 2;
                board[4, 4] = 2;
                board[3, 4] = 1;
                board[4, 3] = 1;
            }
        }

        public int GetPiece(int row, int col)
        {
            if (row < 0 || row > 7 || col < 0 || col > 7)
                return -1;
            else
                return board[row, col];
        }

        public void CountScore(out int you, out int me)
        {
            you = 0; me = 0;
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    if (board[row, col] == 1) me++;
                    if (board[row, col] == 2) you++;
                }
        }

        public void AddPiece(int row, int col, int piece)
        {
            board[row, col] = piece;
        }

        public void FlipPiece(int row, int col)
        {
            if (board[row, col] == 1) board[row, col] = 2;
            else if (board[row, col] == 2) board[row, col] = 1;
        }

        public List<Move> GetMoves(int player)
        {
            int r = 0;
            int c = 0;
            Move move;
            RLine line;
            bool foundMove = false;
            int opponent = 0;

            if (player == 1) opponent = 2;
            if (player == 2) opponent = 1;

            List<Move> m = new List<Move>();
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    foundMove = false;
                    move = new Move();
                    move.square.row = row;
                    move.square.col = col;
                    if (GetPiece(row, col) == 0) // found an empty square
                    {
                        // check North direction
                        r = row - 1;
                        c = col;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                r = r - 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check Northeast direction
                        r = row - 1;
                        c = col + 1;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                r = r - 1;
                                c = c + 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check East direction
                        r = row;
                        c = col + 1;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                c = c + 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check Southeast direction
                        r = row + 1;
                        c = col + 1;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                r = r + 1;
                                c = c + 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check South direction
                        r = row + 1;
                        c = col;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                r = r + 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check Southwest direction
                        r = row + 1;
                        c = col - 1;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                r = r + 1;
                                c = c - 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check West direction
                        r = row;
                        c = col - 1;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                c = c - 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                        // check Northwest direction
                        r = row - 1;
                        c = col - 1;
                        if (GetPiece(r, c) == opponent) // found an opponent piece
                        {
                            line = new RLine();
                            while (GetPiece(r, c) == opponent) // found more opponent pieces
                            {
                                line.flipped.Add(new Square(r, c));
                                r = r - 1;
                                c = c - 1;
                            }
                            if (GetPiece(r, c) == player) // found player piece at end
                            {
                                foundMove = true;
                                line.end.row = r;
                                line.end.col = c;
                                move.lines.Add(line);
                            }
                        }

                    }
                    if (foundMove)
                        m.Add(move);
                }
            return m;
        }

        public bool ComputerPlay(int difficulty)
        {
            List<Move> m = new List<Move>();
            m = GetMoves(1);
            Debug.WriteLine($"computer has {m.Count} moves");
            if (m.Count == 0)
            {
                Debug.WriteLine("computer has no moves");
                return true;
            }

            //Debug.WriteLine("start computer move list");
            for (int i=0; i<m.Count; i++)
            {
                //Debug.WriteLine($"computer move = {m[i].square.row}, {m[i].square.col}");
            }

            if (difficulty == 1) // easy difficulty
                PlayEasy(m);
            else if (difficulty == 2) // medium difficulty
                PlayMedium(m);
            else if (difficulty == 3) // hard difficulty
                PlayHard(m);

            return false;
        }

        public void PlayMove(int player, Move move)
        {
            // add player's piece
            AddPiece(move.square.row, move.square.col, player);
            // flip opponent's pieces
            for (int i = 0; i < move.lines.Count; i++)
            {
                for (int j = 0; j < move.lines[i].flipped.Count; j++)
                {
                    int r = move.lines[i].flipped[j].row;
                    int c = move.lines[i].flipped[j].col;
                    FlipPiece(r, c);
                }
            }
        }

        private void PlayEasy(List<Move> m)
        {
            int choice = rand.Next(m.Count);
            PlayMove(1, m[choice]);
        }

        private void PlayMedium(List<Move> m)
        {
            ComputeScores(m);
            int highest = 0;
            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].score > m[highest].score)
                    highest = i;
            }
            Debug.WriteLine($"med highest score = {m[highest].score}");
            PlayMove(1, m[highest]);
        }

        private void PlayHard(List<Move> m)
        {
            ComputeWeightedScores(m);
            int highest = 0;
            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].score > m[highest].score)
                    highest = i;
            }
            Debug.WriteLine($"hi highest score = {m[highest].score}");
            PlayMove(1, m[highest]);
        }

        private void ComputeScores(List<Move> m)
        {
            int score = 0;
            for (int i = 0; i < m.Count; i++)
            {
                score = 0;
                for (int j = 0; j < m[i].lines.Count; j++)
                {
                    score = score + m[i].lines[j].flipped.Count;
                }
                m[i].score = score;
            }
        }

        private void ComputeWeightedScores(List<Move> m)
        {
            int r = 0;
            int c = 0;

            ComputeScores(m);
            for (int i = 0; i < m.Count; i++)
            {
                r = m[i].square.row;
                c = m[i].square.col;

                // check outside edges
                if (r == 0 || r == 7 || c == 0 || c == 7)
                    m[i].score += 50;

                // check inside edges (inner 4x4)
                if ((r == 2 && c >= 2 && c <= 5) ||
                    (r == 5 && c >= 2 && c <= 5) ||
                    (c == 2 && r >= 2 && r <= 5) ||
                    (c == 5 && r >= 2 && r <= 5))
                    m[i].score += 25;

                // check outside corners
                if ((r == 0 && c == 0) ||
                    (r == 0 && c == 7) ||
                    (r == 7 && c == 0) ||
                    (r == 7 && c == 7))
                    m[i].score += 100;

                // check inside corners (inner 4x4)
                if ((r == 2 && c == 2) ||
                    (r == 2 && c == 5) ||
                    (r == 5 && c == 2) ||
                    (r == 5 && c == 5))
                    m[i].score += 50;

                // check squares next to outside corners
                // upper left
                if ((r == 1 && c == 0) ||
                    (r == 0 && c == 1) ||
                    (r == 1 && c == 1))
                    m[i].score = 0;
                // upper right
                if ((r == 1 && c == 7) ||
                    (r == 0 && c == 6) ||
                    (r == 1 && c == 6))
                    m[i].score = 0;
                // lower left
                if ((r == 6 && c == 0) ||
                    (r == 7 && c == 1) ||
                    (r == 6 && c == 1))
                    m[i].score = 0;
                // lower right
                if ((r == 6 && c == 7) ||
                    (r == 7 && c == 6) ||
                    (r == 6 && c == 6))
                    m[i].score = 0;
            }
        }
    }
}
