using System;
using System.Collections.Generic;
using System.Drawing;

namespace ChessApp.Pieces
{
    // Top
    [Serializable]
    public class Rook : ContinuousPathPiece
    {
        public bool MovedSinceStart { get; set; }

        public Rook(Position position, bool white) : base(position, white)
        {
            MovedSinceStart = false;
        }

        public Rook(ContinuousPathPiece cpp) : base(cpp)
        {
        }



        public override Bitmap GetBitmap(Graphics g)
        {
            return White ? Properties.Resources.w_rook_png_shadow_1024px
                : @Properties.Resources.b_rook_png_shadow_1024px;
        }

        public override List<Board> PossibleMoves(Board b)
        {
            List<Board> boards = new List<Board>();

            // right
            findValidPositions(new Position(Position.X + 1, Position.Y), b, boards, p => p.X <= 7, p => new Position(p.X + 1, p.Y));
            // left
            findValidPositions(new Position(Position.X - 1, Position.Y), b, boards, p => p.X >= 0, p => new Position(p.X - 1, p.Y));
            // top
            findValidPositions(new Position(Position.X, Position.Y + 1), b, boards, p => p.Y <= 7, p => new Position(p.X, p.Y + 1));
            // bottom
            findValidPositions(new Position(Position.X, Position.Y - 1), b, boards, p => p.Y >= 0, p => new Position(p.X, p.Y - 1));

            boards.ForEach(board =>
            {
                Position p = board.NewPos;
                Rook r = (Rook)board.PieceByPosition[p];
                r.MovedSinceStart = true;
            });

            return boards;
        }

        public override Piece Clone()
        {
            Rook r = new Rook(this.Position, this.White);
            // For castling
            r.MovedSinceStart = this.MovedSinceStart;
            return r;
        }
    }
}
