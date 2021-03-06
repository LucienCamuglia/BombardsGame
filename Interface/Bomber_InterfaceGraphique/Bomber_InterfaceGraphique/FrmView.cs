﻿/* *
 * Project     : BombardsGame
 * Description :
 * Authors     : De Biasi Loris, Devaud Alan
 * Date        : 17.03.2017
 * Version     : 1.0
 * */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Bomber_InterfaceGraphique
{
    public partial class FrmView : Form
    {
        #region Properties
        List<Player> playerlist = new List<Player>();
        public BG_PowerBar PowerBar { get; set; }
        public System.Windows.Forms.Timer Watch { get; set; }
        public bool SpacePressed { get; set; }
        const int SRV_PORT = 8000;
        public bool IsConnected { get; set; }
        BG_Client client;
        #endregion

        public FrmView()
        {
            this.KeyPreview = true; // Add key event on the WinForm
            this.DoubleBuffered = true;
            InitializeComponent();
        }

        private void FrmView_Load(object sender, EventArgs e)
        {
            rtb_score.WriteScore(playerlist);
            // Place on the game panel center (Winform without right informations)
            this.PowerBar = new BG_PowerBar((this.Width - this.pictureBox1.Width) / 2 - (BG_PowerBar.DEFAULT_WIDTH / 2), this.Bounds.Height - 100); // 100 to show, else the bar is outside the winform

            /*playerlist.Add(new Player("paul"));
            playerlist.Add(new Player("wdwdw"));
            playerlist.Add(new Player("ghhhhh"));*/

            this.Watch = new System.Windows.Forms.Timer();
            this.Watch.Interval = 1;
            this.Watch.Tick += Watch_Tick;
            //this.Watch.Start();

            this.SpacePressed = false;
            this.IsConnected = false;
        }

        /// <summary>
        /// Watch game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Watch_Tick(object sender, EventArgs e)
        {
            this.Refresh();

            if (!this.Focused)
                this.Focus();

            if (this.SpacePressed)
                this.PowerBar.StartProgress();
            else if (!this.SpacePressed)
                this.PowerBar.StopProgress();



        }

        // Show the debug form
        private void TSMIDebug_Click(object sender, EventArgs e)
        {
            Debug debug = new Debug();
            debug.ShowDialog(this);
        }

        // Quit the application
        private void TSMIQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TSMIJoinGame_Click(object sender, EventArgs e)
        {
            JoinGame join = new JoinGame();
            if (join.ShowDialog(this) == DialogResult.OK)
            {
                this.client = new BG_Client(join.IPServer, SRV_PORT, join.Pseudo, this);
                client.Connect();

                // Start the thread for checking messages
                Thread thread = new Thread(new ThreadStart(client.CheckForNewMessages));
                thread.Start();

                this.IsConnected = true;
                this.Watch.Start();
            }
        }

        private void FrmView_Paint(object sender, PaintEventArgs e)
        {
            // Draw the status
            //DrawConnectionStatus(e.Graphics, false);
            this.DrawConnexionState(e.Graphics);

            this.PowerBar.Draw(e); // Draw the power bar

            if (this.IsConnected)
            {
                // Draw the field
                this.client.field.Draw(e);

                // Draw the players
                foreach (var player in this.client.listPlayers)
                {
                    player.Cannon.Draw(e);
                }
            }
        }

        /// <summary>
        /// Draw the connection status
        ///     - Red => disconnected
        ///     - Green => connected
        /// </summary>
        /// <param name="g"></param>
        /// <param name="isConnected"></param>
        private void DrawConnectionStatus(Graphics g, bool isConnected)
        {
            Rectangle rect = new Rectangle(10, 30, 20, 20);

            this.IsConnected = isConnected;

            /*if (isConnected == true)
            {
                g.FillRectangle(Brushes.Green, rect);
            }
            else
            {
                g.FillRectangle(Brushes.Red, rect);
            }*/
        }

        private void DrawConnexionState(Graphics g)
        {
            Rectangle rect = new Rectangle(10, 30, 20, 20);
            g.FillRectangle((this.IsConnected) ? Brushes.Green : Brushes.Red, rect);
        }

        // Disable or enable the menu option
        private void TSMIMenu_Click(object sender, EventArgs e)
        {
            if (true) // Need to check if player already in game or not
            {
                TSMIJoinGame.Enabled = true;
                TSMIQuitGame.Enabled = false;
            }
            else
            {
                TSMIJoinGame.Enabled = false;
                TSMIQuitGame.Enabled = true;
            }
        }

        private void FrmView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                this.SpacePressed = true;
            }

        }

        private void FrmView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                this.SpacePressed = false;
        }


    }
}
