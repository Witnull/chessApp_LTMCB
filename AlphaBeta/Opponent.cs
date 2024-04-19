using ChessApp.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessApp.AlphaBeta
{
    public enum OpponentDifficulty
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2
    }

    [Serializable]
    public class Opponent
    {
        static readonly Random r = new Random();

        public Opponent(OpponentDifficulty difficulty)
        {
            Difficulty = difficulty;
        }

        public OpponentDifficulty Difficulty { get; set; }

        private struct Node
        {
            public Board board;
            public int value;

            public Node(Board board, int value)
            {
                this.board = board;
                this.value = value;
            }
        }
        public Board Move(Board board)
        {
            List<Node> nodes = new List<Node>();
            int pivot_value = board.WhiteTurn ? -EvaluationUtils.INFTY : EvaluationUtils.INFTY;
            foreach (Board move in board.Successor()) {
                int value = EvaluationUtils.AlphabetaInit(move, (int)Difficulty, board.WhiteTurn);
                pivot_value = board.WhiteTurn ? Math.Max(pivot_value, value) : Math.Min(pivot_value,value);
                nodes.Add(new Node(move, value));
            }
            List<Node> eligibleMoves = nodes.FindAll(n => n.value == pivot_value);
            if (eligibleMoves.Count > 0)
            {
                Board next = eligibleMoves[r.Next(eligibleMoves.Count)].board;
                Position newPos = next.NewPos;
                return next;
            }
            return null; //returns null if there are no possible moves.
        }
    }
}
