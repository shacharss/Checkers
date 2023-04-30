using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Checkers.Logic;

namespace Checkers.GUI
{
    internal class GameForm : Form
    {
        // Labels
        private Label m_PlayerOneLabel;
        private Label m_PlayerTwoLabel;
        private Label m_CurrentPlayerLabel;

        // Button Matrix
        private BoardButton[,] m_BoardButtons;

        // Variables
        private readonly int r_BoardSize;
        private readonly string r_PlayerOneName;
        private readonly string r_PlayerTwoName;
        private int m_PlayerOnePoints = 0;
        private int m_PlayerTwoPoints = 0;
        private GameManager m_GameManager;
        private BoardButton m_SelectedChecker;
        private List<BoardButton> possibleMovesForSelectedChecker = new List<BoardButton>();
        private bool m_PlayingVsComputer;

        internal GameForm(int i_BoardSize, string i_PlayerOneName)
            : this(i_BoardSize, i_PlayerOneName, "Computer")
        {
            m_PlayingVsComputer = true;
            this.ClientSize = new System.Drawing.Size(m_BoardButtons[0, r_BoardSize - 1].Right + 16, m_BoardButtons[r_BoardSize - 1, 0].Bottom + 16);
        }

        internal GameForm(int i_BoardSize, string i_PlayerOneName, string i_PlayerTwoName)
        {
            r_BoardSize = i_BoardSize;
            r_PlayerOneName = i_PlayerOneName;
            r_PlayerTwoName = i_PlayerTwoName;
            m_PlayingVsComputer = false;
            initializeBoard();
            m_GameManager = new GameManager(i_BoardSize);
            m_GameManager.PositionChangeNotifier += updateBoard;

            // Initialize Form
            this.Text = "Damka";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ClientSize = new System.Drawing.Size(m_BoardButtons[0, r_BoardSize - 1].Right + 16, m_CurrentPlayerLabel.Bottom + 16);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
        }

        private void boardButtonClick(object sender, EventArgs e)
        {
            BoardButton senderButton = (BoardButton)sender;
            if (senderButton == m_SelectedChecker)
            {
                deselectChecker();
            }
            else if (boardButtonSelectable(senderButton))
            {
                selectChecker(senderButton);
            }
            else if (possibleMovesForSelectedChecker.Contains(senderButton))
            {
                playMove(senderButton);
            }
        }

        private bool boardButtonSelectable(BoardButton i_Boardbutton)
        {
            bool selectable = false;

            if (m_GameManager.CurrentPosition.Board[i_Boardbutton.r_Row, i_Boardbutton.r_Col] == 'X' && m_GameManager.CurrentPosition.BlackToMove)
            {
                selectable = true;
            }
            else if (m_GameManager.CurrentPosition.Board[i_Boardbutton.r_Row, i_Boardbutton.r_Col] == 'Z' && m_GameManager.CurrentPosition.BlackToMove)
            {
                selectable = true;
            }
            else if (m_GameManager.CurrentPosition.Board[i_Boardbutton.r_Row, i_Boardbutton.r_Col] == 'O' && !m_GameManager.CurrentPosition.BlackToMove)
            {
                selectable = true;
            }
            else if (m_GameManager.CurrentPosition.Board[i_Boardbutton.r_Row, i_Boardbutton.r_Col] == 'Q' && !m_GameManager.CurrentPosition.BlackToMove)
            {
                selectable = true;
            }

            return selectable;
        }

        private void playMove(BoardButton i_targetPosition)
        {
            string move = getStringMove(i_targetPosition);
            m_GameManager.PlayMove(move);
            if (m_PlayingVsComputer)
            {
                while (m_GameManager.CurrentPosition.BlackToMove == false)
                {
                    m_GameManager.PlayMove(Computer.GetComputerMove(m_GameManager.CurrentPosition));
                }
            }
        }

        private string getStringMove(BoardButton i_targetPosition)
        {
            string[] cols = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            string[] rows = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            string move = cols[m_SelectedChecker.r_Col] + rows[m_SelectedChecker.r_Row] + ">" + cols[i_targetPosition.r_Col] + rows[i_targetPosition.r_Row];
            return move;
        }

        private void updateBoard(Position i_Position)
        {
            for (int row = 0; row < r_BoardSize; row++)
            {
                for (int col = 0; col < r_BoardSize; col++)
                {
                    if (row % 2 != col % 2)
                    {
                        m_BoardButtons[row, col].Text = char.ToString(i_Position.Board[row, col]);
                    }
                }
            }

            if (i_Position.BlackWon)
            {
                if (playAgain(r_PlayerOneName))
                {
                    deselectChecker();
                    m_PlayerOnePoints += i_Position.MaterialAdvantageForBlack(false);
                    m_PlayerOneLabel.Text = string.Format("{0}: {1}", r_PlayerOneName, m_PlayerOnePoints);
                    m_GameManager = new GameManager(r_BoardSize);
                    m_GameManager.PositionChangeNotifier += updateBoard;
                    updateBoard(m_GameManager.CurrentPosition);
                }
                else
                {
                    this.Close();
                }
            }
            else if (i_Position.WhiteWon)
            {
                if (playAgain(r_PlayerTwoName))
                {
                    deselectChecker();
                    m_PlayerTwoPoints -= i_Position.MaterialAdvantageForBlack(false);
                    m_PlayerTwoLabel.Text = string.Format("{0}: {1}", r_PlayerTwoName, m_PlayerTwoPoints);
                    m_GameManager = new GameManager(r_BoardSize);
                    m_GameManager.PositionChangeNotifier += updateBoard;
                    updateBoard(m_GameManager.CurrentPosition);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                deselectChecker();

                // update whos turn it is
                if (i_Position.BlackToMove == true && m_PlayingVsComputer == false)
                {
                    m_CurrentPlayerLabel.Text = string.Format("{0}'s Turn", r_PlayerOneName);
                }
                else if (i_Position.BlackToMove == false && m_PlayingVsComputer == false)
                {
                    m_CurrentPlayerLabel.Text = string.Format("{0}'s Turn", r_PlayerTwoName);
                }
            }
        }

        private bool playAgain(string i_Winner)
        {
            bool playAgain = false;
            string message = string.Format("{0} Won! \nAnother Round?", i_Winner);
            string caption = "Damka";
            MessageBoxButtons playAgainButtons = MessageBoxButtons.YesNo;
            if (MessageBox.Show(message, caption, playAgainButtons) == DialogResult.Yes)
            {
                playAgain = true;
            }

            return playAgain;
        }

        private void deselectChecker()
        {
            if (m_SelectedChecker != null)
            {
                m_SelectedChecker.BackColor = Color.White;
            }

            m_SelectedChecker = null;

            foreach (BoardButton boardButton in possibleMovesForSelectedChecker)
            {
                boardButton.BackColor = Color.White;
            }

            possibleMovesForSelectedChecker.Clear();
        }

        private void selectChecker(BoardButton i_checkerToSelect)
        {
            deselectChecker();
            m_SelectedChecker = i_checkerToSelect;
            i_checkerToSelect.BackColor = Color.LightBlue;

            foreach (Move move in m_GameManager.CurrentPosition.PossibleMovesDictionary.Values)
            {
                if (move.SourceLocation.Row == i_checkerToSelect.r_Row && move.SourceLocation.Col == i_checkerToSelect.r_Col)
                {
                    BoardButton possibleTargetForSelectedChecker = m_BoardButtons[move.TargetLocation.Row, move.TargetLocation.Col];
                    possibleMovesForSelectedChecker.Add(possibleTargetForSelectedChecker);
                    possibleTargetForSelectedChecker.BackColor = Color.LightGreen;
                }
            }
        }

        private void initializeBoard()
        {
            // Initialize player one label
            m_PlayerOneLabel = new Label();
            m_PlayerOneLabel.Text = string.Format("{0}: {1}", r_PlayerOneName, m_PlayerOnePoints);
            m_PlayerOneLabel.Top = 16;
            if (r_BoardSize == 6)
            {
                m_PlayerOneLabel.Left = 56;
            }
            else if (r_BoardSize == 8)
            {
                m_PlayerOneLabel.Left = 96;
            }
            else if (r_BoardSize == 10)
            {
                m_PlayerOneLabel.Left = 136;
            }

            this.Controls.Add(m_PlayerOneLabel);

            // Initialize player two label
            m_PlayerTwoLabel = new Label();
            m_PlayerTwoLabel.Text = string.Format("{0}: {1}", r_PlayerTwoName, m_PlayerTwoPoints);
            m_PlayerTwoLabel.Top = 16;
            m_PlayerTwoLabel.Left = m_PlayerOneLabel.Left + m_PlayerOneLabel.Width;
            this.Controls.Add(m_PlayerTwoLabel);

            // Initialize button matrix
            m_BoardButtons = new BoardButton[r_BoardSize, r_BoardSize];

            // Initialize button matrix's buttons
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    m_BoardButtons[i, j] = new BoardButton(i, j);
                    m_BoardButtons[i, j].BackColor = Color.White;
                    m_BoardButtons[i, j].TabStop = false;
                    m_BoardButtons[i, j].FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
                    m_BoardButtons[i, j].Size = new System.Drawing.Size(40, 40);
                    if (i == 0)
                    {
                        m_BoardButtons[i, j].Top = m_PlayerOneLabel.Top + m_PlayerOneLabel.Height;
                    }
                    else
                    {
                        m_BoardButtons[i, j].Top = m_BoardButtons[i - 1, j].Top + m_BoardButtons[i - 1, j].Height;
                    }

                    if (j == 0)
                    {
                        m_BoardButtons[i, j].Left = 16;
                    }
                    else
                    {
                        m_BoardButtons[i, j].Left = m_BoardButtons[i, j - 1].Left + m_BoardButtons[i, j - 1].Width;
                    }

                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        m_BoardButtons[i, j].Enabled = false;
                        m_BoardButtons[i, j].BackColor = Color.Gray;
                    }
                    else if (i % 2 == 1 && j % 2 == 1)
                    {
                        m_BoardButtons[i, j].Enabled = false;
                        m_BoardButtons[i, j].BackColor = Color.Gray;
                    }
                    else if (i + 1 < r_BoardSize / 2)
                    {
                        m_BoardButtons[i, j].Text = "O";
                    }
                    else if (i > r_BoardSize / 2)
                    {
                        m_BoardButtons[i, j].Text = "X";
                    }

                    m_BoardButtons[i, j].Click += new EventHandler(boardButtonClick);
                    this.Controls.Add(m_BoardButtons[i, j]);
                }
            }

            // Initialize current player label, if playing against player
            if (m_PlayingVsComputer == false)
            {
                m_CurrentPlayerLabel = new Label();
                m_CurrentPlayerLabel.Text = string.Format("{0}'s Turn", r_PlayerOneName);
                m_CurrentPlayerLabel.Height = 20;
                m_CurrentPlayerLabel.Left = m_BoardButtons[0, (r_BoardSize / 2) - 1].Left;
                m_CurrentPlayerLabel.Top = m_BoardButtons[r_BoardSize - 1, 0].Bottom + 16;
                this.Controls.Add(m_CurrentPlayerLabel);
            }
        }
    }
}
