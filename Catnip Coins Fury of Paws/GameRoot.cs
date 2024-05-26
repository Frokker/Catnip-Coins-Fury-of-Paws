using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace CatnipCoinsFuryOfPaws
{
    public partial class GameRoot : Form
    {
        
        
        public GameRoot()
        {
            InitializeComponent();
        }
        
        private void play_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game newGame = new Game();
            newGame.Show();
        }

       
        private void GameRoot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        }
}