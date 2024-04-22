using ChessApp.AlphaBeta;
using ChessApp.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ChessApp
{
    public partial class gameScreen : Form
    {
        public GameState GameState { get; set; } = new GameState();
        public bool InGame { get; set; } = false;

        readonly AIMoveOverlay aimo = new AIMoveOverlay();
        public string SavedGamePath { get; set; } = null;
        public bool Dirty { get; set; } = false;


        public gameScreen()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            GenerateGame();
            Dirty = false;
            UpdateTitle();
        }

        public void GenerateGame()
        {
            GameState.o = new Opponent(OpponentDifficulty.EASY);

            GameState.Board = new Board();

            SavedGamePath = null;

            Dirty = false;
            UpdateTitle();
         
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.Size = new System.Drawing.Size(700, 700);
            Board.TILE_SIDE = (ClientSize.Height - Board.OFFSET_Y) / 8;
            Board.OFFSET_X = (ClientSize.Width - 8 * Board.TILE_SIDE) / 2;
            GameState.Draw(e.Graphics);
            if (!GameState.Board.WhiteTurn)
            {
                aimo.Draw(e.Graphics);
            }
        }

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Invalidate();
        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Board newBoard = GameState.Board.Click(new Position(e.X, e.Y), GameState.successiveBoards);

            if (!ReferenceEquals(GameState.Board, newBoard))
            {
                Dirty = true;
                UpdateTitle();
            }

            GameState.Board = newBoard;

            Invalidate();
            GameState.SetCheckPosition();
            Refresh();

            //AI MOVE
            if (GameState.Board.WhiteTurn == false)
            {
                Board aiMove = GameState.o.Move(GameState.Board);

                if (aiMove != null)
                {
                    GameState.Board = aiMove;
                    if (GameState.Board.NoPossibleMoves())
                    {
                        if (GameState.Board.KingIsInCheck(true))
                            FormUtils.ShowDefeatDialog(); //() => newToolStripMenuItem_Click(null, EventArgs.Empty));
                        else
                            FormUtils.ShowPlayerStalemateDialog(); //() => newToolStripMenuItem_Click(null, EventArgs.Empty));
                    }
                }
                else //ai didn't generate move
                {
                    if (GameState.Board.KingIsInCheck(false))
                        FormUtils.ShowVictoryDialog(); // () => newToolStripMenuItem_Click(null, EventArgs.Empty));
                    else
                        FormUtils.ShowAIStalemateDialog(); // () => newToolStripMenuItem_Click(null, EventArgs.Empty));
                }
            }

            GameState.SetCheckPosition();

            Refresh();
        }
        /*
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UnsavedChangesAbort())
                return;
            GenerateGame();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SavedGamePath is null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }
            FileStream stream = File.OpenWrite(SavedGamePath);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, GameState);
            stream.Dispose();
            Dirty = false;
            UpdateTitle();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveAs())
                saveToolStripMenuItem_Click(sender, e);
        }

        private bool SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "sav",
                Filter = "Saved Games (*.sav)|*.sav"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SavedGamePath = sfd.FileName;
                this.Text = $"ChessApp: {Path.GetFileNameWithoutExtension(SavedGamePath)}";
                return true;
            }
            return false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UnsavedChangesAbort())
                return;
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "sav",
                Filter = "Saved Games (*.sav)|*.sav"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    GameState = bf.Deserialize(ofd.OpenFile()) as GameState;
                    SavedGamePath = ofd.FileName;
                    Dirty = false;
                    UpdateTitle();
                    Invalidate();
                }
                catch (Exception)
                {
                    MessageBox.Show("The file is either corrupted or not a ChessApp savegame.", "Loading failed");
                }
            }

        } */

        private void UpdateTitle()
        {
            Text = $"PvE";
            if (SavedGamePath != null)
                Text += $" - {Path.GetFileNameWithoutExtension(SavedGamePath)}";
            if (Dirty)
                Text += "*";
        }
        /*
        private bool UnsavedChangesAbort()
        {
            if (!Dirty)
                return false;
            if (MessageBox.Show("Do you want to save your game?", "Unsaved progress", MessageBoxButtons.YesNo)
                == DialogResult.No)
                return false;
            saveToolStripMenuItem_Click(null, EventArgs.Empty);
            if (!Dirty)
                return false;
            return true;
        }*/
    }
}
