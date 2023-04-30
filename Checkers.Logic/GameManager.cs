using System;

namespace Checkers.Logic
{
    public class GameManager
    {
        private Position m_CurrentPosition;
        private bool m_BlackToMove = true;
        public event Action<Position> PositionChangeNotifier;

        public Position CurrentPosition
        {
            get
            {
                return m_CurrentPosition;
            }
        }

        public GameManager(int i_BoardSize)
        {
            char[,] initialBoard = new char[i_BoardSize, i_BoardSize];
            InitializeBoard(initialBoard, i_BoardSize);
            Position startingPosition = new Position(initialBoard, m_BlackToMove, false, null);
            startingPosition.EvaluatePossibleMoves();
            m_CurrentPosition = startingPosition;
        }

        private void InitializeBoard(char[,] o_Board, int i_BoardSize)
        {
            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    if (i % 2 == j % 2)
                    {
                        o_Board[i, j] = ' ';
                    }
                    else if (i < (i_BoardSize / 2) - 1)
                    {
                        o_Board[i, j] = 'O';
                    }
                    else if (i > i_BoardSize / 2)
                    {
                        o_Board[i, j] = 'X';
                    }
                    else
                    {
                        o_Board[i, j] = ' ';
                    }
                }
            }
        }

        public bool PlayMove(string i_Move)
        {
            bool moveIsValid;
            if (!m_CurrentPosition.PossibleMovesDictionary.ContainsKey(i_Move))
            {
                Console.WriteLine("Illegal move! Try again.");
                moveIsValid = false;
            }
            else
            {
                m_CurrentPosition = Position.ApplyMove(m_CurrentPosition, i_Move);
                PositionChangeNotifier.Invoke(m_CurrentPosition);
                moveIsValid = true;
            }

            return moveIsValid;
        }
    }
}
