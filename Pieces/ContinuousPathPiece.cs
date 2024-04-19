using System;
using System.Collections.Generic;

namespace ChessApp.Pieces
{
    [Serializable]
    public abstract class ContinuousPathPiece : Piece
    {
        protected ContinuousPathPiece(Position position, bool white) : base(position, white)
        {
        }

        protected ContinuousPathPiece(ContinuousPathPiece cpp) : base(cpp)
        {
        }

        protected delegate Position Operation(Position p);

        protected void findValidPositions(Position p, Board b, List<Board> boards, Predicate<Position> condition, Operation operation)
        {
            Position position = p;
            while (condition(position))
            {
                if (b.IsOccupied(position) && b.PieceByPosition[position].White == White)
                    break;
                boards.Add(new Board(b, Position, position, this));
                if (b.IsOccupied(position))
                    break;
                position = operation(position);
            }
        }
    }
}
