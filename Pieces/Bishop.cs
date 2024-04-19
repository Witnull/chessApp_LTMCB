using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ChessApp.Pieces
{
    [Serializable]
    public class Bishop : ContinuousPathPiece
    {
        public Bishop(ContinuousPathPiece cpp) : base(cpp)
        {
        }

        public Bishop(Position position, bool white) : base(position, white)
        {
        }

        public override Piece Clone()
        {
            Bishop p = new Bishop(this.Position, this.White);
            return p;
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return White ? Properties.Resources.w_bishop_png_shadow_1024px
                : @Properties.Resources.b_bishop_png_shadow_1024px;
        }

        public override List<Board> PossibleMoves(Board b)
        {
            List<Board> boards = new List<Board>();

            // top right
            findValidPositions(
                new Position(Position.X + 1, Position.Y + 1),
                b,
                boards,
                p => p.X <= 7 && p.Y <= 7,
                p => new Position(p.X + 1, p.Y + 1)
            );

            // top left
            findValidPositions(
                new Position(Position.X - 1, Position.Y + 1),
                b,
                boards,
                p => p.X >= 0 && p.Y <= 7,
                p => new Position(p.X - 1, p.Y + 1)
            );

            // bottom left
            findValidPositions(
                new Position(Position.X - 1, Position.Y - 1),
                b,
                boards,
                p => p.X >= 0 && p.Y >= 0,
                p => new Position(p.X - 1, p.Y - 1)
            );

            // bottom right
            findValidPositions(
                new Position(Position.X + 1, Position.Y - 1),
                b,
                boards,
                p => p.X <= 7 && p.Y >= 0,
                p => new Position(p.X + 1, p.Y - 1)
            );

            return boards;
        }
    }
}
