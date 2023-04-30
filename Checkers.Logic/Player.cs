using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Logic
{
    internal class Player
    {
        private readonly string r_PlayerName;
        private readonly char r_PlayerSymbol;
        private string m_PreviousMove;

        public int Points { get; set; } = 0;

        public string PreviousMove
        {
            get
            {
                return m_PreviousMove;
            }

            set
            {
                m_PreviousMove = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public char PlayerSymbol
        {
            get
            {
                return r_PlayerSymbol;
            }
        }

        public Player(string i_PlayerName, char i_PlayerSymbol)
        {
            r_PlayerName = i_PlayerName;
            r_PlayerSymbol = i_PlayerSymbol;
        }
    }
}
