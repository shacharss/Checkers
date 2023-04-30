using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Logic
{
    public class Computer
    {
        private const int k_MaxDepth = 6;

        public static string GetComputerMove(Position i_Position)
        {
            string bestMove = null;
            int bestMoveValue = 0;
            foreach (string move in i_Position.PossibleMovesDictionary.Keys)
            {
                int currentMoveValue = RecursiveGetPositionValue(Position.ApplyMove(i_Position, move), 1);
                if (bestMove == null || bestMoveValue > currentMoveValue)
                {
                    bestMove = move;
                    bestMoveValue = currentMoveValue;
                }
            }

            return bestMove;
        }

        private static int RecursiveGetPositionValue(Position i_Position, int i_Depth)
        {
            int posValue = 0;

            if (i_Position.PossibleMovesDictionary.Count == 0)
            {
                if (i_Position.BlackWon)
                {
                    posValue = int.MaxValue;
                }
                else
                {
                    posValue = int.MinValue;
                }
            }
            else if (i_Depth == k_MaxDepth)
            {
                posValue = i_Position.MaterialAdvantageForBlack(true);
            }
            else
            {
                bool firstIteration = true;
                foreach (string move in i_Position.PossibleMovesDictionary.Keys)
                {
                    int newPositionValue = RecursiveGetPositionValue(Position.ApplyMove(i_Position, move), i_Depth + 1);
                    if (firstIteration)
                    {
                        posValue = newPositionValue;
                    }
                    else if ((i_Position.BlackToMove && newPositionValue > posValue) || (!i_Position.BlackToMove && newPositionValue < posValue))
                    {
                        posValue = newPositionValue;
                    }

                    firstIteration = false;
                }
            }

            return posValue;
        }
    }
}
