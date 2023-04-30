using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers.GUI
{
    public partial class MenuForm : Form
    {
        // Labels
        private Label m_BoardSizeLabel;
        private Label m_PlayersLabel;
        private Label m_PlayerOneLabel;

        // Radiobuttons
        private RadioButton m_BoardSizeSixRadioButton;
        private RadioButton m_BoardSizeEightRadioButton;
        private RadioButton m_BoardSizeTenRadioButton;

        // TextBoxes
        private TextBox m_PlayerOneTextBox;
        private TextBox m_PlayerTwoTextBox;

        // CheckBoxes
        private CheckBox m_PlayerTwoCheckBox;

        // Buttons
        private Button m_DoneButton;

        // Variables
        private int m_BoardSize;

        public MenuForm()
        {
            // Initialize Components
            initializeComponents();

            // Initialize Form
            this.Text = "Game Settings";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ClientSize = new System.Drawing.Size(m_DoneButton.Right + 16, m_DoneButton.Bottom + 16);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
        }

        private void initializeComponents()
        {
            // Initialize board size label
            m_BoardSizeLabel = new Label();
            m_BoardSizeLabel.Text = "Board Size:";
            m_BoardSizeLabel.Top = 16;
            m_BoardSizeLabel.Left = 16;
            this.Controls.Add(m_BoardSizeLabel);

            // Initialize board size six button
            m_BoardSizeSixRadioButton = new RadioButton();
            m_BoardSizeSixRadioButton.Text = "6x6";
            m_BoardSizeSixRadioButton.Top = m_BoardSizeLabel.Top + m_BoardSizeLabel.Height;
            m_BoardSizeSixRadioButton.Left = 32;
            m_BoardSizeSixRadioButton.Width = 50;
            m_BoardSizeSixRadioButton.Click += m_BoardSizeSixRadioButton_Click;
            this.Controls.Add(m_BoardSizeSixRadioButton);

            // Initialize board size eight button
            m_BoardSizeEightRadioButton = new RadioButton();
            m_BoardSizeEightRadioButton.Text = "8x8";
            m_BoardSizeEightRadioButton.Top = m_BoardSizeLabel.Top + m_BoardSizeLabel.Height;
            m_BoardSizeEightRadioButton.Left = m_BoardSizeSixRadioButton.Left + m_BoardSizeSixRadioButton.Width;
            m_BoardSizeEightRadioButton.Width = 50;
            m_BoardSizeEightRadioButton.Click += m_BoardSizeEightRadioButton_Click;
            this.Controls.Add(m_BoardSizeEightRadioButton);

            // Initialize board size ten button
            m_BoardSizeTenRadioButton = new RadioButton();
            m_BoardSizeTenRadioButton.Text = "10x10";
            m_BoardSizeTenRadioButton.Top = m_BoardSizeLabel.Top + m_BoardSizeLabel.Height;
            m_BoardSizeTenRadioButton.Left = m_BoardSizeEightRadioButton.Left + m_BoardSizeEightRadioButton.Width;
            m_BoardSizeTenRadioButton.Width = 60;
            m_BoardSizeTenRadioButton.Click += m_BoardSizeTenRadioButton_Click;
            this.Controls.Add(m_BoardSizeTenRadioButton);

            // Initialize players label
            m_PlayersLabel = new Label();
            m_PlayersLabel.Text = "Players:";
            m_PlayersLabel.Top = m_BoardSizeSixRadioButton.Top + m_BoardSizeSixRadioButton.Height + 8;
            m_PlayersLabel.Left = 16;
            this.Controls.Add(m_PlayersLabel);

            // Initialize player one label
            m_PlayerOneLabel = new Label();
            m_PlayerOneLabel.Text = "Player 1:";
            m_PlayerOneLabel.Top = m_PlayersLabel.Top + m_PlayersLabel.Height + 3;
            m_PlayerOneLabel.Left = 32;
            m_PlayerOneLabel.Width = 60;
            this.Controls.Add(m_PlayerOneLabel);

            // Initialize player two checkbox (with label)
            m_PlayerTwoCheckBox = new CheckBox();
            m_PlayerTwoCheckBox.Text = "Player 2:";
            m_PlayerTwoCheckBox.Top = m_PlayerOneLabel.Top + m_PlayerOneLabel.Height + 7;
            m_PlayerTwoCheckBox.Left = m_PlayerOneLabel.Left + 3;
            m_PlayerTwoCheckBox.Width = 70;
            m_PlayerTwoCheckBox.Checked = false;
            m_PlayerTwoCheckBox.Click += m_PlayerTwoCheckBox_Click;
            this.Controls.Add(m_PlayerTwoCheckBox);

            // Initialize player one text box
            m_PlayerOneTextBox = new TextBox();
            m_PlayerOneTextBox.Top = m_PlayersLabel.Top + m_PlayersLabel.Height;
            m_PlayerOneTextBox.Left = m_PlayersLabel.Left + m_PlayersLabel.Width;
            this.Controls.Add(m_PlayerOneTextBox);

            // Initialize player two text box
            m_PlayerTwoTextBox = new TextBox();
            m_PlayerTwoTextBox.Top = m_PlayerOneLabel.Top + m_PlayerOneLabel.Height + 8;
            m_PlayerTwoTextBox.Left = m_PlayerOneTextBox.Left;
            m_PlayerTwoTextBox.Enabled = false;
            this.Controls.Add(m_PlayerTwoTextBox);

            // Initialize done button
            m_DoneButton = new Button();
            m_DoneButton.Text = "Done";
            m_DoneButton.Left = m_PlayerTwoTextBox.Right - m_DoneButton.Width;
            m_DoneButton.Top = m_PlayerTwoTextBox.Top + m_PlayerTwoTextBox.Height + 16;
            m_DoneButton.Click += m_DoneButton_Click;
            this.Controls.Add(m_DoneButton);
        }

        private void m_BoardSizeSixRadioButton_Click(object sender, EventArgs e)
        {
            setBoardSize(6);
        }

        private void m_BoardSizeEightRadioButton_Click(object sender, EventArgs e)
        {
            setBoardSize(8);
        }

        private void m_BoardSizeTenRadioButton_Click(object sender, EventArgs e)
        {
            setBoardSize(10);
        }

        private void m_DoneButton_Click(object sender, EventArgs e)
        {
            finalizeAndOpenGameForm();
        }

        private void m_PlayerTwoCheckBox_Click(object sender, EventArgs e)
        {
            handleCheckBox();
        }

        private void setBoardSize(int i_size)
        {
            m_BoardSize = i_size;
        }

        private void finalizeAndOpenGameForm()
        {
            if (m_BoardSizeSixRadioButton.Checked == false && m_BoardSizeEightRadioButton.Checked == false && m_BoardSizeTenRadioButton.Checked == false)
            {
                MessageBox.Show("Please choose a board size!");
            }
            else if (m_PlayerOneTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please choose a name for player one!");
            }
            else if (m_PlayerTwoCheckBox.Checked == true && m_PlayerTwoTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please choose a name for player two!");
            }
            else
            {
                this.Hide();
                GameForm game;

                // open two player game
                if (m_PlayerTwoCheckBox.Checked == true)
                {
                    game = new GameForm(m_BoardSize, m_PlayerOneTextBox.Text, m_PlayerTwoTextBox.Text);
                    game.ShowDialog();
                }

                // open computer game
                else if (m_PlayerTwoCheckBox.Checked == false)
                {
                    game = new GameForm(m_BoardSize, m_PlayerOneTextBox.Text);
                    game.ShowDialog();
                }

                this.Close();
            }
        }

        private void handleCheckBox()
        {
            if (m_PlayerTwoCheckBox.Checked == true)
            {
                m_PlayerTwoTextBox.Enabled = true;
            }
            else if (m_PlayerTwoCheckBox.Checked == false)
            {
                m_PlayerTwoTextBox.Enabled = false;
            }
        }
    }
}
