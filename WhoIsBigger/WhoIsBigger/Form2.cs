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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count_players = dataGridView1.RowCount;
            if (count_players < 2) 
            {
                MessageBox.Show("Количество игроков должно быть не менее двух");
            }
            else
            {
                List<(string Name, int Bet )> playerData = new List<(string, int)>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if (row.Cells["NamePlayer"].Value != null && row.Cells["Bet"].Value != null)
                        {
                            string name = row.Cells["NamePlayer"].Value.ToString();
                            if (int.TryParse(row.Cells["Bet"].Value.ToString(), out int bet) && bet > 0)
                            {
                                playerData.Add((name, bet));
                            }
                            else
                            {
                                MessageBox.Show("Ставка должна быть положительной числовой величиной");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Заполните поля с данными игрока");
                        }
                    }
                }
                foreach ((string name, int bet) in playerData)
                {
                    Player player = new Player(name,bet);
                    Game.Instance.AddPlayer(player);
                }
                Game.Instance.AddBank();

                Form3 form3 = new Form3();
                form3.Show();
                this.Hide();
            }
        }
    }
}
