using ChessApp.AlphaBeta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessApp
{
    public partial class gamePlayervsPlayer : Form
    {
        public GameState playerGameState = new GameState();
        public bool InGame { get; set; } = false;

        public bool Dirty { get; set; } = false;


        public gamePlayervsPlayer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            GenerateGame();
            Dirty = false;
            Invalidate();
            
        }

        public void GenerateGame()
        {

            playerGameState.Board = new Board();
            Dirty = false;
            Invalidate();
        }

        private void gamePlayervsPlayer_Paint(object sender, PaintEventArgs e)
        {
            this.Size = new Size(700, 700);
            Board.TILE_SIDE = (ClientSize.Height - Board.OFFSET_Y) / 8;
            Board.OFFSET_X = (ClientSize.Width - 8 * Board.TILE_SIDE) / 2;
            playerGameState.Draw(e.Graphics);
        }

        private void gamePlayervsPlayer_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void gamePlayervsPlayer_ResizeEnd(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void gamePlayervsPlayer_MouseClick(object sender, MouseEventArgs e)
        {
            //White Play
            if (playerGameState.Board.WhiteTurn == true)
            {
                Board WhiteBoard = playerGameState.Board.Click(new Position(e.X, e.Y), playerGameState.successiveBoards);

                if (!ReferenceEquals(playerGameState.Board, WhiteBoard))
                {
                    Dirty = true;
                }

                playerGameState.Board = WhiteBoard;

                Invalidate();

            }

            //Black Play
            else if (playerGameState.Board.WhiteTurn == false)
            {
                
                Board BlackBoard = playerGameState.Board.Click(new Position(e.X,e.Y), playerGameState.successiveBoards);

                if (!ReferenceEquals(playerGameState.Board, BlackBoard))
                {
                    Dirty = true;
                }

                playerGameState.Board = BlackBoard;

                Invalidate();
            }
            playerGameState.SetCheckPosition();

            Refresh();

        }
    }
}
