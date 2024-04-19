using System;
using System.Collections.Generic;
using System.Drawing;

namespace ChessApp.Pieces
{
    [Serializable]
    internal class Knight : Piece
    {
        public Knight(Position position, bool white) : base(position, white)
        {
        }

        public override Piece Clone()
        {
            Knight p = new Knight(this.Position, this.White);
            return p;
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return White ? Properties.Resources.w_knight_png_shadow_1024px
                : @Properties.Resources.b_knight_png_shadow_1024px;
        }

        public override List<Board> PossibleMoves(Board b)
        {
            List<Board> boards = new List<Board>();
            void move(Position tempPos)
            {
                if (tempPos.X < 0 || tempPos.X > 7 || tempPos.Y < 0 || tempPos.Y > 7) return;
                if (b.IsOccupied(tempPos) && b.PieceByPosition[tempPos].White == White) return;
                Board newBoard = new Board(b, Position, tempPos, this);
                boards.Add(newBoard);
            }
            move(new Position(Position.X + 1, Position.Y + 2));
            move(new Position(Position.X - 1, Position.Y + 2));
            move(new Position(Position.X + 2, Position.Y + 1));
            move(new Position(Position.X + 2, Position.Y - 1));
            move(new Position(Position.X + 1, Position.Y - 2));
            move(new Position(Position.X - 1, Position.Y - 2));
            move(new Position(Position.X - 2, Position.Y + 1));
            move(new Position(Position.X - 2, Position.Y - 1));

            return boards;
        }
    }
}
