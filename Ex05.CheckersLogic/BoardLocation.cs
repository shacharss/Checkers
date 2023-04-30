using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Logic
{
    public class BoardLocation
    {
        private int m_row;
        private int m_col;

        public BoardLocation(int i_Row, int i_Col)
        {
            Row = i_Row;
            Col = i_Col;
        }

        public int Row
        {
            get
            {
                return m_row;
            }

            set
            {
                m_row = value;
            }
        }

        public int Col
        {
            get
            {
                return m_col;
            }

            set
            {
                m_col = value;
            }
        }
    }
}
