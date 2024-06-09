using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhoIsBigger
{
    public partial class Form4 : Form
    {
        private System.Windows.Forms.Timer timer1;
        private List<Player> winnerForExtra;
        private Player firstPlayerWinnerForExtra;

        public Form4()
        {
            InitializeComponent();
            winnerForExtra = new List<Player>();
            Game.Instance.FirstRoundIdentify();
            Game.Instance.FirstPlayerIdentify();
            PrintRound();
            PrintPlayer();
        }

        private void OnFrameChanged(object o, EventArgs e)
        {
            pictureBox1.Invalidate();
        }


        public void PrintRound()
        {
            label1.Text = "Раунд: " + Game.Instance.rounds[0].number.ToString();
        }
        public void PrintPlayer()
        {
            label3.Text = "Ход игрока: " + Game.Instance.players[0].name;
        }

        public void PrintThrow(int throw_value)
        {
            label4.Text = "Бросок: " + throw_value;
        }

        public void PrintMaxVal(int max_throw_value)
        {
            label2.Text = "Наибольшее число: " + max_throw_value;
        }

        private void LoadDiceImage(PictureBox pictureBox, int dice_number)
        {
            string path = Path.Combine(Application.StartupPath, @"..\..\..\Resources", $"dice{dice_number}.png");
            pictureBox.Image = Image.FromFile(path);
        }

        private void PrintTableResult()
        {
            dataGridView1.Visible = true;
            dataGridView1.Rows.Clear(); // очищаем строки dataGridView1

            foreach (var kvp in Game.Instance.resultsPerRound)
            {
                int rowIndex = dataGridView1.Rows.Add(); // добавляем новую строку
                dataGridView1.Rows[rowIndex].Cells[0].Value = kvp.Key.name; // заполняем столбец "Name"
                dataGridView1.Rows[rowIndex].Cells[1].Value = kvp.Value; // заполняем столбец "Points"
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible == true)
            {
                dataGridView1.Visible = false;
            }
            label2.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += timer1_Tick;
            pictureBox1.Visible = true;
            ImageAnimator.Animate(pictureBox1.Image, OnFrameChanged);
            timer1.Interval = 1000; // 2 секунды
            timer1.Start();// Запуск гифки


            foreach (Move move in Game.Instance.rounds[0].moves)
            {
                int max = 0;
                if (winnerForExtra.Contains(move.player))
                {
                    Random random = new Random();
                    max = 0;
                    int dice1 = random.Next(1, 7);
                    LoadDiceImage(pictureBox2, dice1);
                    max = dice1;
                    move.throws.Add(new Throw(max));
                    PrintThrow(move.throws.Count());
                    PrintMaxVal(max);
                    Game.Instance.players[0].AddRoundPoints(max);
                    button2.Visible = true;
                    button1.Visible = false;
                    return;
                }
                

                if (move.player == Game.Instance.players[0])
                {
                    if (move.throws.Count() == 0)
                    {
                        Random random = new Random();
                        max = 0;
                        int dice1 = random.Next(1, 7);
                        int dice2 = random.Next(1, 7);
                        int dice3 = random.Next(1, 7);
                        LoadDiceImage(pictureBox2, dice1);
                        LoadDiceImage(pictureBox3, dice2);
                        LoadDiceImage(pictureBox4, dice3);
                        max = Math.Max(Math.Max(dice1, dice2), dice3);
                        move.throws.Add(new Throw(max));
                        PrintThrow(move.throws.Count());
                        PrintMaxVal(max);
                    }
                    else if (move.throws.Count() == 1)
                    {
                        Random random = new Random();
                        max = 0;
                        int dice1 = random.Next(1, 7);
                        int dice2 = random.Next(1, 7);
                        LoadDiceImage(pictureBox2, dice1);
                        LoadDiceImage(pictureBox3, dice2);
                        pictureBox4.Image = null;
                        max = Math.Max(dice1, dice2);
                        move.throws.Add(new Throw(max));
                        PrintThrow(move.throws.Count());
                        PrintMaxVal(max);
                    }
                    else if (move.throws.Count() == 2)
                    {
                        Random random = new Random();
                        max = 0;
                        int dice1 = random.Next(1, 7);
                        LoadDiceImage(pictureBox2, dice1);
                        pictureBox3.Image = null;
                        pictureBox4.Image = null;
                        max = dice1;
                        move.throws.Add(new Throw(max));
                        PrintThrow(move.throws.Count());
                        PrintMaxVal(max);
                    }
                    Game.Instance.players[0].AddRoundPoints(max);
                    if (move.throws.Count() == 3)
                    {
                        button2.Visible = true;
                        button1.Visible = false;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
            pictureBox2.Visible = false;
            bool flag = true;
            Game.Instance.PlayerChange(); //смена основных игроков

            if (winnerForExtra.Count() > 1) //если это доп замена
            {
                winnerForExtra = Game.Instance.ExtraPlayerChange(winnerForExtra);
                if (firstPlayerWinnerForExtra == winnerForExtra[0]) //конец круга
                {
                    winnerForExtra.Clear();
                    label5.Visible = false;
                    flag = fun();
                }
                else
                {
                    while (Game.Instance.players[0] != winnerForExtra[0])
                    {
                        Game.Instance.PlayerChange();
                    }
                    label5.Visible = true;
                }
            }
            else //если это просто замена
            {
                if (Game.Instance.firstPlayer == Game.Instance.players[0]) //конец круга
                {
                    flag = fun();
                }
            }
            PrintPlayer();
            label4.Text = "Бросок:";
            if (flag)
            {
                button1.Visible = true;
                button2.Visible = false;
            }
        }


        private bool fun()
        {
            Game.Instance.UpdateResultsPerRound();
            List<Player> winnersByRound = Game.Instance.DetermineWinnerForRound(); //игроки-победители раунда
            if (winnersByRound.Count() > 1) //игроков-победителей раунда больше одного
            {
                //доп броски
                winnerForExtra = winnersByRound;
                firstPlayerWinnerForExtra = winnerForExtra[0];

                foreach (Player player in Game.Instance.players)
                {
                    player.ClearRoundPoints();
                }

                while (Game.Instance.players[0] != winnerForExtra[0])
                {
                    Game.Instance.PlayerChange();
                }
                foreach (Move move in Game.Instance.rounds[0].moves)
                {
                    if (winnerForExtra.Contains(move.player))
                    {
                        move.throws.Clear();
                    }
                }
                label5.Visible = true;
            }
            else //один игрок-победитель раунда
            {
                Game.Instance.UpdateResultsPerRound();
                winnersByRound[0].AddGeneralPoints();//прибавить победителю очко

                foreach (Player player in Game.Instance.players)
                {
                    player.ClearRoundPoints();
                }

                Game.Instance.RoundChange(); //смена раунда
                PrintTableResult();
                Game.Instance.ClearResultsPerRound();
                if (Game.Instance.firstRound == Game.Instance.rounds[0]) //раунд был последний
                {
                    List<Player> winnersByGame = Game.Instance.DetermineWinnerForGame(); //игроки-победители игры
                    if (winnersByGame.Count() > 1) //игроков-победителей игры больше одного
                    {
                        int winChips = Game.Instance.bank / winnersByGame.Count();
                        label2.Text = $"Победители игры: {string.Join(", ", winnersByGame.Select(x => x.name))} получают по {winChips}";
                    }
                    else //один игрок-победитель игры
                    {
                        label2.Text = $"Победитель игры: {winnersByGame[0].name} получает {Game.Instance.bank}";
                    }
                    label1.Visible = false;
                    label2.Visible = true;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    button1.Visible = false;
                    button2.Visible = false;
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    pictureBox4.Visible = false;
                    return false;
                }
                else //раунд был не последний
                {
                    PrintRound();
                }
            }
            return true;
        }





        

        private void timer1_Tick(object sender, EventArgs e)
        {
            ImageAnimator.StopAnimate(pictureBox1.Image, OnFrameChanged);
            pictureBox1.Visible = false;
            timer1.Stop();
            label2.Visible = true;
            if (pictureBox2.Image != null)
            {
                pictureBox2.Visible = true;
            }
            else
            {
                pictureBox2.Visible = false;
            }
            if (pictureBox3.Image != null)
            {
                pictureBox3.Visible = true;
            }
            else
            {
                pictureBox3.Visible = false;
            }
            if (pictureBox4.Image != null)
            {
                pictureBox4.Visible = true;
            }
            else
            {
                pictureBox4.Visible = false;
            }
        }
    }
}
