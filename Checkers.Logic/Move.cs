using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Logic
{
    public class Move
    {
        private BoardLocation m_sourceLocation;
        private BoardLocation m_targetLocation;

        public BoardLocation SourceLocation
        {
            get
            {
                return m_sourceLocation;
            }

            set
            {
                m_sourceLocation = value;
            }
        }

        public BoardLocation TargetLocation
        {
            get
            {
                return m_targetLocation;
            }

            set
            {
                m_targetLocation = value;
            }
        }

        public Move(BoardLocation i_sourceLocation, BoardLocation i_targetLocation)
        {
            m_sourceLocation = i_sourceLocation;
            m_targetLocation = i_targetLocation;
        }

        public bool IsCapture()
        {
            return Math.Abs(m_sourceLocation.Row - m_targetLocation.Row) == 2;
        }

        public override
        string ToString()
        {
            char[] lowerLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
            char[] upperLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

            StringBuilder toString = new StringBuilder();

            toString.Append(upperLetters[SourceLocation.Col]);
            toString.Append(lowerLetters[SourceLocation.Row]);
            toString.Append(">");
            toString.Append(upperLetters[TargetLocation.Col]);
            toString.Append(lowerLetters[TargetLocation.Row]);

            return toString.ToString();
        }
    }
}
