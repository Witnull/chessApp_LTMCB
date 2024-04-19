using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp

{
    public static class PositionColor
    {
        public static Color Green = Color.FromArgb(50, 0, 255, 0);
        public static Color Red = Color.FromArgb(50, 255, 0, 0);
        public static Color Blue = Color.FromArgb(50, 0, 0, 255);
	}

    [Serializable]
    public class ColoredPosition : Position
    {
        Color Color = PositionColor.Green;
        public ColoredPosition(string stringpos) : base(stringpos)
        {
        }

        public ColoredPosition(Position p) : base(p)
        {
        }

        public ColoredPosition(int x, int y) : base(x, y)
        {
        }

        public ColoredPosition(Position p, Color c) : base(p)
        {
            Color = c;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color), X * Board.TILE_SIDE + Board.OFFSET_X, Y * Board.TILE_SIDE + Board.OFFSET_Y, Board.TILE_SIDE, Board.TILE_SIDE);
        }
    }
}
