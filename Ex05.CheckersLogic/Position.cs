using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Logic
{
    public class Position
    {
        private char[,] m_Board;
        private bool m_BlackToMove = true;

        public char[,] Board
        {
            get
            {
                return m_Board;
            }
        }

        public bool BlackToMove
        {
            get
            {
                return m_BlackToMove;
            }

            set
            {
                m_BlackToMove = value;
            }
        }

        private Dictionary<string, Move> m_possibleMovesDictionary;

        public Dictionary<string, Move> PossibleMovesDictionary
        {
            get
            {
                return m_possibleMovesDictionary;
            }

            private set
            {
                m_possibleMovesDictionary = value;
            }
        }

        private bool mustCapture = false;

        public bool MustCapture
        {
            get
            {
                return mustCapture;
            }

            set
            {
                mustCapture = value;
            }
        }

        private bool m_BlackWon = false;
        private bool m_WhiteWon = false;

        private BoardLocation onlyMoveableChecker;

        public bool BlackWon
        {
            get
            {
                return m_BlackWon;
            }
        }

        public bool WhiteWon
        {
            get
            {
                return m_WhiteWon;
            }
        }

        public Position(char[,] i_Board, bool i_BlackToMove, bool i_MustCapture, BoardLocation i_OnlyMoveableChecker)
        {
            m_Board = i_Board;
            m_BlackToMove = i_BlackToMove;
            mustCapture = i_MustCapture;
            onlyMoveableChecker = i_OnlyMoveableChecker;
        }

        public int MaterialAdvantageForBlack(bool i_CalledFromComputer)
        {
            int advantage = 0;
            if (m_BlackWon && i_CalledFromComputer)
            {
                advantage = int.MaxValue;
            }
            else if (m_WhiteWon && i_CalledFromComputer)
            {
                advantage = int.MinValue;
            }
            else
            {
                int dimension = (int)Math.Sqrt(m_Board.Length);
                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (m_Board[i, j] == ' ')
                        {
                            continue;
                        }
                        else if (m_Board[i, j] == 'X')
                        {
                            advantage++;
                        }
                        else if (m_Board[i, j] == 'O')
                        {
                            advantage--;
                        }
                        else if (m_Board[i, j] == 'Z')
                        {
                            advantage += 4;
                        }
                        else if (m_Board[i, j] == 'Q')
                        {
                            advantage -= 4;
                        }
                    }
                }
            }

            return advantage;
        }

        public void EvaluatePossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();
            for (int row = 0; row < Math.Sqrt(m_Board.Length); row++)
            {
                for (int col = 0; col < Math.Sqrt(m_Board.Length); col++)
                {
                    if (onlyMoveableChecker != null && (row != onlyMoveableChecker.Row || col != onlyMoveableChecker.Col))
                    {
                        continue;
                    }

                    if (m_Board[row, col] == ' ' || (m_BlackToMove && (m_Board[row, col] == 'O' || m_Board[row, col] == 'Q')) || (!m_BlackToMove && (m_Board[row, col] == 'X' || m_Board[row, col] == 'Z')))
                    {
                        continue;
                    }

                    List<BoardLocation> exploreableLocations = new List<BoardLocation>();
                    if (m_Board[row, col] != 'O' && row != 0)
                    {
                        if (col > 0)
                        {
                            exploreableLocations.Add(new BoardLocation(row - 1, col - 1));
                        }

                        if (col < Math.Sqrt(m_Board.Length) - 1)
                        {
                            exploreableLocations.Add(new BoardLocation(row - 1, col + 1));
                        }
                    }

                    if (m_Board[row, col] != 'X' && row != Math.Sqrt(m_Board.Length) - 1)
                    {
                        if (col > 0)
                        {
                            exploreableLocations.Add(new BoardLocation(row + 1, col - 1));
                        }

                        if (col < Math.Sqrt(m_Board.Length) - 1)
                        {
                            exploreableLocations.Add(new BoardLocation(row + 1, col + 1));
                        }
                    }

                    BoardLocation currentLocation = new BoardLocation(row, col);

                    foreach (BoardLocation exploreableLocation in exploreableLocations)
                    {
                        if (m_Board[exploreableLocation.Row, exploreableLocation.Col] == ' ')
                        {
                            if (!mustCapture)
                            {
                                possibleMoves.Add(new Move(currentLocation, exploreableLocation));
                            }
                        }
                        else if ((m_BlackToMove && (m_Board[exploreableLocation.Row, exploreableLocation.Col] == 'Q' || m_Board[exploreableLocation.Row, exploreableLocation.Col] == 'O')) || (!m_BlackToMove && (m_Board[exploreableLocation.Row, exploreableLocation.Col] == 'Z' || m_Board[exploreableLocation.Row, exploreableLocation.Col] == 'X')))
                        {
                            int landingRow = (2 * exploreableLocation.Row) - currentLocation.Row;
                            int landingCol = (2 * exploreableLocation.Col) - currentLocation.Col;
                            if (landingRow >= 0 && landingCol >= 0 && landingRow <= Math.Sqrt(m_Board.Length) - 1 && landingCol <= Math.Sqrt(m_Board.Length) - 1 && m_Board[landingRow, landingCol] == ' ')
                            {
                                if (!mustCapture)
                                {
                                    mustCapture = true;
                                    possibleMoves.Clear();
                                }

                                possibleMoves.Add(new Move(currentLocation, new BoardLocation(landingRow, landingCol)));
                            }
                        }
                    }
                }
            }

            PossibleMovesDictionary = new Dictionary<string, Move>();
            foreach (Move move in possibleMoves)
            {
                PossibleMovesDictionary.Add(move.ToString(), move);
            }
        }

        public static Position ApplyMove(Position i_Pos, string i_Move)
        {
            if (i_Pos.m_possibleMovesDictionary.ContainsKey(i_Move))
            {
                Move move = i_Pos.m_possibleMovesDictionary[i_Move];
                char[,] newBoard = (char[,])i_Pos.m_Board.Clone();
                char checker = newBoard[move.SourceLocation.Row, move.SourceLocation.Col];
                newBoard[move.SourceLocation.Row, move.SourceLocation.Col] = ' ';
                if (move.TargetLocation.Row == 0 && checker == 'X')
                {
                    newBoard[move.TargetLocation.Row, move.TargetLocation.Col] = 'Z';
                }
                else if (move.TargetLocation.Row == Math.Sqrt(i_Pos.m_Board.Length) - 1 && checker == 'O')
                {
                    newBoard[move.TargetLocation.Row, move.TargetLocation.Col] = 'Q';
                }
                else
                {
                    newBoard[move.TargetLocation.Row, move.TargetLocation.Col] = checker;
                }

                bool newBlackToMove = !i_Pos.m_BlackToMove;
                bool newMustCapture = false;
                BoardLocation newOnlyMoveableChecker = null;
                if (move.IsCapture())
                {
                    newBlackToMove = !newBlackToMove;
                    newMustCapture = true;
                    int captureRow = move.TargetLocation.Row - Math.Sign(move.TargetLocation.Row - move.SourceLocation.Row);
                    int captureCol = move.TargetLocation.Col - Math.Sign(move.TargetLocation.Col - move.SourceLocation.Col);
                    newBoard[captureRow, captureCol] = ' ';
                    newOnlyMoveableChecker = move.TargetLocation;
                }

                Position newPosition = new Position(newBoard, newBlackToMove, newMustCapture, newOnlyMoveableChecker);
                newPosition.EvaluatePossibleMoves();
                if (newPosition.PossibleMovesDictionary.Count == 0)
                {
                    if (newPosition.MustCapture)
                    {
                        newPosition.BlackToMove = !newPosition.BlackToMove;
                        newPosition.MustCapture = false;
                        newPosition.onlyMoveableChecker = null;
                        newPosition.EvaluatePossibleMoves();
                        if (newPosition.PossibleMovesDictionary.Count == 0)
                        {
                            if (newPosition.m_BlackToMove)
                            {
                                newPosition.m_WhiteWon = true;
                            }
                            else
                            {
                                newPosition.m_BlackWon = true;
                            }
                        }
                    }
                    else
                    {
                        if (newPosition.m_BlackToMove)
                        {
                            newPosition.m_WhiteWon = true;
                        }
                        else
                        {
                            newPosition.m_BlackWon = true;
                        }
                    }
                }

                return newPosition;
            }
            else
            {
                return null;
            }
        }
    }
}
