using ChessApp.AlphaBeta;
using ChessApp.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ChessApp
{
    [Serializable]
    public class Board
    {
        public Dictionary<Position, Piece> PieceByPosition { get; set; }
        public bool WhiteTurn { get; set; } = true;
        public static int TILE_SIDE { get; set; }
        public static int OFFSET_X { get; set; }
        public static int OFFSET_Y { get; set; } = 25;
        public Piece CurrentClickedPiece { get; set; }

        public Piece PreviousClickPiece { get; set; }
        public ColoredPosition NewPos { get; set; }

        public int TurnNumber { get; set; } = 0;

        public Board(Board board)
        {
            PieceByPosition = new Dictionary<Position, Piece>();
            WhiteTurn = !board.WhiteTurn;
            foreach (Position key in board.PieceByPosition.Keys)
            {
                if (board.PieceByPosition[key] != null)
                    PieceByPosition[key] = board.PieceByPosition[key].Clone();
                else
                    PieceByPosition[key] = null;
            }
            TurnNumber = board.TurnNumber + 1;
        }

        public Board(Board b, Position posOld, Position posNew, Piece p) : this(b)
        {
            p = p.Clone();
            NewPos = new ColoredPosition(posNew);
            PieceByPosition[posNew] = p;
            PieceByPosition[posOld] = null;
            PieceByPosition[posNew].Position = posNew;
        }

		public Board(Board b, Position posOld, ColoredPosition posNew, Piece p) : this(b, posOld, posNew as Position, p)
		{
            NewPos = posNew;
		}

		public static bool IsInBoard(Position p)
        {
            return p.X >= 0 && p.Y >= 0 && p.X <= 7 && p.Y <= 7;
        }

        //Generates starting board setup.
        public Board()
        {
            PieceByPosition = new Dictionary<Position, Piece>();
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceByPosition.Add(new Position(i, j), null);
                }
            }
            {
                Position pos = new Position(4, 0);
                PieceByPosition[pos] = new King(pos, false);
                pos = new Position(4, 7);
                PieceByPosition[pos] = new King(pos, true);
                pos = new Position(3, 0);
                PieceByPosition[pos] = new Queen(pos, false);
                pos = new Position(3, 7);
                PieceByPosition[pos] = new Queen(pos, true);
            }
            for (int i = 0; i < 8; ++i)
            {
                Position pos = new Position(i, 1);
                PieceByPosition[pos] = new Pawn(pos, false);
                pos = new Position(i, 6);
                PieceByPosition[pos] = new Pawn(pos, true);
            }
            for (int i = 0; i < 8; i += 7)
            {
                Position pos = new Position(i, 0);
                PieceByPosition[pos] = new Rook(pos, false);
                pos = new Position(i, 7);
                PieceByPosition[pos] = new Rook(pos, true);

            }
            for (int i = 1; i < 8; i += 5)
            {
                Position pos = new Position(i, 0);
                PieceByPosition[pos] = new Knight(pos, false);
                pos = new Position(i, 7);
                PieceByPosition[pos] = new Knight(pos, true);

            }
            for (int i = 2; i < 8; i += 3)
            {
                Position pos = new Position(i, 0);
                PieceByPosition[pos] = new Bishop(pos, false);
                pos = new Position(i, 7);
                PieceByPosition[pos] = new Bishop(pos, true);

            }

        }

        public List<Board> Successor()
        {
            List<Board> res = new List<Board>();

            List<Piece> pieces = PieceByPosition.Values.ToList();
            foreach (Piece piece in pieces)
            {

                if (piece == null) continue;
                if (piece.White != WhiteTurn)
                    continue;
                List<Board> moves = piece.PossibleMoves(this);
                foreach (Board move in moves)
                {
                    res.Add(move);
                }
            }
            return res.Where(b => !b.KingIsInCheck(!b.WhiteTurn)).ToList();
        }

        public bool IsOccupied(Position position)
        {
            return PieceByPosition[position] != null;
        }

        public void DrawTiles(Graphics g)
        {
            for (int i = 0; i < 64; ++i)
            {
                new Position(i % 8, i / 8).Draw(g);
            }
            foreach (Position position in PieceByPosition.Keys)
            {
                if (PieceByPosition[position] == null) continue;
                PieceByPosition[position].Draw(g, position);
            }
        }

        public Board Click(Position p, List<Board> successiveStates)
        {
            Position clickedPosition = new Position((p.X - OFFSET_X) / TILE_SIDE, (p.Y - OFFSET_Y) / TILE_SIDE);
            //if (!WhiteTurn || !IsInBoard(clickedPosition))
            if (!IsInBoard(clickedPosition))
                return this;

            Piece clickedPiece = PieceByPosition[clickedPosition];


            if (clickedPiece == null || !clickedPiece.White)
            {
                //if (CurrentClickedPiece == null) return this;

                //impossible move, i.e. unclicked the currently selected piece
                if (clickedPiece != null && !clickedPiece.White && this.WhiteTurn==false)
                {
                    successiveStates.Clear();
                    CurrentClickedPiece = clickedPiece;
                    CurrentClickedPiece.PossibleMoves(this).Where(board => !board.KingIsInCheck(true)).ToList().ForEach(board => { successiveStates.Add(board); });
                    return this;
                }
                if (!successiveStates.Any(ss => ss.NewPos.Equals(clickedPosition)))
                {
                    //CurrentClickedPiece = null;
                    successiveStates.Clear();
                    return this;
                }

                Board res = successiveStates.Find(ss => ss.NewPos.Equals(clickedPosition));
                res.PreviousClickPiece = this.CurrentClickedPiece;
                successiveStates.Clear();
                return res;
            }
            else if (clickedPiece.White && this.WhiteTurn==true)
            {
                successiveStates.Clear();
                CurrentClickedPiece = clickedPiece;
                CurrentClickedPiece.PossibleMoves(this)
                    .Where(board => !board.KingIsInCheck(true)).ToList()
                    .ForEach(board => { successiveStates.Add(board); });
            }
            else if (clickedPiece.White && !this.WhiteTurn)
            {
                Board res = successiveStates.Find(ss => ss.NewPos.Equals(clickedPosition));
                if (res == null) return this;
                res.PreviousClickPiece = this.CurrentClickedPiece;
                successiveStates.Clear();
                return res;


            }

            return this;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Turn: " + (WhiteTurn ? "White" : "Black"));
            foreach (Position position in PieceByPosition.Keys)
            {
                if (PieceByPosition[position] == null)
                    continue;
                sb.AppendLine($"{position}: {PieceByPosition[position]}");
            }
            sb.AppendLine("New pos: " + NewPos);
            sb.AppendLine("Evaluation: " + EvaluationUtils.EvaluateBoard(this));

            return sb.ToString();
        }

        public bool KingIsInCheck(bool white)
        {
            return !(KingCheckPosition(white) is null);
        }

		public Position KingCheckPosition(bool white)
		{
			King king = null;
			foreach (Piece piece in PieceByPosition.Values)
			{
				if (piece is null) continue;
				if (piece is King kx && piece.White == white)
				{
					king = kx;
					break;
				}
			}

			// if no king found then there is no check.
			if (king is null)
				return null;

			foreach (Piece piece in PieceByPosition.Values.ToArray())
			{
				//skip friendly pieces
				if (piece is null || piece.White == king.White)
					continue;
				//if piece can attack king, king is in check
				if (piece.PossibleMoves(this).Any(b => b.NewPos.Equals(king.Position)))
					return king.Position;
			}
			return null;
		}

		public bool NoPossibleMoves()
        {
            return Successor().Count == 0;
        }

    }
}
