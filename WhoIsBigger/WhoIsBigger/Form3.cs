using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhoIsBigger
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= numericUpDown1.Value; i++) 
            {
                Round round = new Round(i);
                foreach(Player player in Game.Instance.players)
                {
                    round.moves.Add(new Move(player));
                }
                Game.Instance.AddRound(round);
            }

            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }
    }
}
