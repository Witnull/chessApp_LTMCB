using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Pieces
{
    [Serializable]
    public abstract class Piece
    {
        protected Piece(Position position, bool white)
        {
            Position = position;
            White = white;
        }

        public Position Position { get; set; }
        public bool White { get; set; }

        protected Piece() { }
        public Piece(Piece p) 
        {
            Position = p.Position;
            White = p.White;
        }

        public abstract List<Board> PossibleMoves(Board b);

        public abstract Bitmap GetBitmap(Graphics g);

        public void Draw(Graphics g, Position key)
        {
            Bitmap bitmap = GetBitmap(g);
            g.DrawImage(bitmap, key.X * Board.TILE_SIDE + Board.OFFSET_X, key.Y * Board.TILE_SIDE + Board.OFFSET_Y, Board.TILE_SIDE, Board.TILE_SIDE);
        }

        public bool Equals(Piece piece)
        {
            if (Position.X != piece.Position.X || Position.Y != piece.Position.Y) return false;
            if (GetType() != piece.GetType()) return false;
            if (White !=  piece.White) return false;
            return true;
        }

        public override string ToString()
        {
            return (this.White ? "White" : "Black") + " " + this.GetType().Name + " " + this.Position.ToString();
        }

        abstract public Piece Clone();

    }
}
