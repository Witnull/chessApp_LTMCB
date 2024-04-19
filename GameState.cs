using ChessApp.AlphaBeta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
	[Serializable]
	public class GameState
	{
		public Board Board { get; set; }
		public List<Board> successiveBoards { get; set; } = new List<Board>();
		public Opponent o { get; set; }
		public ColoredPosition checkPosition { get; set; } = null;

		public GameState()
		{
			Board = new Board();
			successiveBoards = new List<Board>();
			o = new Opponent(OpponentDifficulty.EASY);
			checkPosition = null;
		}

		public void Draw(Graphics g)
		{
			Board.DrawTiles(g);
			foreach (Board sb in successiveBoards)
			{
				sb.NewPos.Draw(g);
			}
			if (checkPosition is ColoredPosition)
			{
				checkPosition.Draw(g);
			}
		}

		public void SetCheckPosition()
		{
			checkPosition = null;
			Position king = Board.KingCheckPosition(Board.WhiteTurn);
			if (king is null)
				return;
			checkPosition = new ColoredPosition(king, PositionColor.Red);
		}
    }
}
