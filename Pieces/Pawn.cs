using ChessApp.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ChessApp
{
    [Serializable]
    internal class Pawn : Piece
    {
        public int TwoSquareAdvanceTimestamp { get; set; } = -1;

		public Pawn(Position position, bool white) : base(position, white)
		{
		}

		public Pawn(Position position, bool white, int twoSquareAdvanceTimestamp) : base(position, white)
        {
            TwoSquareAdvanceTimestamp = twoSquareAdvanceTimestamp;
        }

        public override Bitmap GetBitmap(Graphics g)
        {
            return White ? Properties.Resources.w_pawn_png_shadow_1024px
                : @Properties.Resources.b_pawn_png_shadow_1024px;
        }

        public override List<Board> PossibleMoves(Board b)
        {
            List<Board> boards = new List<Board>();

            Position forwardOne = White ? new Position(Position.X, Position.Y-1) : new Position(Position.X, Position.Y + 1);
            Position forwardTwo = White ? new Position(Position.X, Position.Y - 2) : new Position(Position.X, Position.Y + 2);
            Position captureLeft = White ? new Position(Position.X - 1, Position.Y - 1) : new Position(Position.X - 1, Position.Y + 1);
            Position captureRight = White ? new Position(Position.X + 1, Position.Y - 1) : new Position(Position.X + 1, Position.Y + 1);
            Position left = new Position(Position.X-1, Position.Y);
            Position right = new Position(Position.X+1, Position.Y);

            int startY = White ? 6 : 1;
            int endY = White ? 0 : 7;

            void processedAdd(Board rawBoard)
            {
                if (rawBoard.NewPos.Y == endY)
                {
                    rawBoard.NewPos = new ColoredPosition(rawBoard.NewPos, PositionColor.Blue);
                    rawBoard.PieceByPosition[rawBoard.NewPos] = new Queen(rawBoard.NewPos, White);
                }
                boards.Add(rawBoard);
            }

			//Forward
            if (Board.IsInBoard(forwardOne) && !b.IsOccupied(forwardOne)) {
                processedAdd(new Board(b, Position, forwardOne, new Pawn(forwardOne, White)));
                if (Position.Y == startY && !b.IsOccupied(forwardTwo))
                    processedAdd(new Board(b, Position, forwardTwo, new Pawn(forwardTwo, White, b.TurnNumber)));
            }

            //Capture
            void captureForward(Position capture)
            {
				if (Board.IsInBoard(capture) && b.IsOccupied(capture) && b.PieceByPosition[capture].White != White)
					processedAdd(new Board(b, Position, capture, new Pawn(capture, White)));
			}
            captureForward(captureLeft); captureForward(captureRight);

            //En passant
            void enpassant(Position adjacent, Position capture)
            {
                if (Board.IsInBoard(adjacent) && b.IsOccupied(adjacent) && b.PieceByPosition[adjacent].White != White &&
                    b.PieceByPosition[adjacent] is Pawn p && p.TwoSquareAdvanceTimestamp == b.TurnNumber-1)
                {
                    ColoredPosition cp = new ColoredPosition(capture, PositionColor.Blue);
                    Board epb = new Board(b, Position, cp, new Pawn(capture, White));
                    epb.PieceByPosition[adjacent] = null;
                    processedAdd(epb);
                }
            }
            enpassant(left, captureLeft); enpassant(right, captureRight);

            return boards;
        }

        public override Piece Clone()
        {
            Pawn p = new Pawn(this.Position, this.White, this.TwoSquareAdvanceTimestamp);
            return p;
        }

    }
}
