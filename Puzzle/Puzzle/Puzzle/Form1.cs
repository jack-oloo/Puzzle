using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle
{
    public partial class frmPuzzleGame : Form
    {
        int inNullSliceIndex, inmoves = 0;
        List<Bitmap> lstOriginalPictureList = new List<Bitmap>();
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


        public frmPuzzleGame()
        {
            InitializeComponent();
            lstOriginalPictureList.AddRange(new Bitmap[] { Properties.Resources._1, Properties.Resources._2, Properties.Resources._3, Properties.Resources._4, Properties.Resources._5, Properties.Resources._6, Properties.Resources._7, Properties.Resources._8, Properties.Resources._9, Properties.Resources._null });
            lblMovesMade.Text += inmoves;
            label2.Text = "00:00:00";
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            Shuffle();
        }

        void Shuffle()
        {
            do
            {
                int j;
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                Random r = new Random();
                for (int i = 0; i < 9; i++)
                {
                    Indexes.Remove((j = Indexes[r.Next(0, Indexes.Count)]));
                    ((PictureBox)gbPuzzzleBox.Controls[i]).Image = lstOriginalPictureList[j];
                    if (j == 9) inNullSliceIndex = i;
                }

            } while (CheckWin());
        }

        

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            DialogResult YesOrNo = new DialogResult();
            if (label2.Text != "00:00:00")
            {
                YesOrNo = MessageBox.Show("Are You Sure To RESTART?", "Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            }
            if (YesOrNo == DialogResult.Yes || label2.Text == "00:00:00")
            {
                Shuffle();
                timer.Reset();
                label2.Text = "00:00:00";
                inmoves = 0;
                lblMovesMade.Text = "Moves Made : 0";
            }
        }

        

        private void AskPermissionBeforeQuite(object sender, FormClosingEventArgs e)
        {
            DialogResult YesOrNo = MessageBox.Show("Are you sure you want to quit ?", "Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sender as Button != btnQuite && YesOrNo == DialogResult.No) e.Cancel = true;
            if (sender as Button != btnQuite && YesOrNo == DialogResult.Yes) Environment.Exit(0);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            AskPermissionBeforeQuite(sender, e as FormClosingEventArgs);
        }

        private void SwitchPictureBox(object sender, EventArgs e)
        {
            if (label2.Text == "00:00:00")
                timer.Start();
            int InPictureBoxIndex = gbPuzzzleBox.Controls.IndexOf(sender as Control);
            if (inNullSliceIndex != InPictureBoxIndex)
            {
                List<int> FourBrothers = new List<int>(new int[] { InPictureBoxIndex - 1, InPictureBoxIndex - 3, InPictureBoxIndex + 1, InPictureBoxIndex + 3 });
                if (FourBrothers.Contains(inNullSliceIndex))
                {
                    ((PictureBox)gbPuzzzleBox.Controls[inNullSliceIndex]).Image = ((PictureBox)gbPuzzzleBox.Controls[InPictureBoxIndex]).Image;
                    ((PictureBox)gbPuzzzleBox.Controls[InPictureBoxIndex]).Image = lstOriginalPictureList[9];
                    inNullSliceIndex = InPictureBoxIndex;
                    lblMovesMade.Text = "Moves Made : " + (++inmoves);
                    if (CheckWin())
                    {
                        timer.Stop();
                        (gbPuzzzleBox.Controls[8] as PictureBox).Image = lstOriginalPictureList[8];
                        MessageBox.Show("Congratulations, \nYour Fox Is Healthy\nTime Elapsed : " + timer.Elapsed.ToString().Remove(8) +
                            "\nTotal Moves Made : " + inmoves, "Puzzle");
                        inmoves = 0;
                        lblMovesMade.Text = "Moves Made : 0";
                        label2.Text = "00:00:00";
                        timer.Reset();
                        Shuffle();
                    }

                }
            }
        }

        bool CheckWin()
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((gbPuzzzleBox.Controls[i] as PictureBox).Image != lstOriginalPictureList[i]) break;
            }
            if (i == 8) return true;
            else return false;
        }

        private void UpdateTimeElapsed(object sender, EventArgs e)
        {
            if (timer.Elapsed.ToString() != "00:00:00")
                label2.Text = timer.Elapsed.ToString().Remove(8);
            if (timer.Elapsed.ToString() == "00:00:00")
                btnPause.Enabled = false;
            else
                btnPause.Enabled = true;
            if (timer.Elapsed.Minutes.ToString() == "1")
            {
                timer.Reset();
                lblMovesMade.Text = "Moves Made : 0";
                label2.Text = "00:00:00";
                inmoves = 0;
                btnPause.Enabled = false;
                MessageBox.Show("Time is up buddy\nTry again", "Puzzle");
                Shuffle();
            }
        }

        private void PauseOrResume(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                timer.Stop();
                gbPuzzzleBox.Visible = false;
                btnPause.Text = "Resume";
            }
            else
            {
                timer.Start();
                gbPuzzzleBox.Visible = true;
                btnPause.Text = "Pause";
            }
        }
    }
}
