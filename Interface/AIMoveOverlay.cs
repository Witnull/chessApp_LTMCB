using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Interface
{
	internal class AIMoveOverlay
	{
		public void Draw(Graphics g)
		{
			SolidBrush sb = new SolidBrush(Color.Black);
			SolidBrush sbs = new SolidBrush(Color.White);

			int boxOffsetX = Board.OFFSET_X + 3 * Board.TILE_SIDE;
			int boxOffsetY = Board.OFFSET_Y + (int)(3.5 * Board.TILE_SIDE);

			int boxWidth = 2 * Board.TILE_SIDE;
			int boxHeight = Board.TILE_SIDE;

			float fontSize = (float)(boxHeight * 0.3);
			int textOffsetX = boxOffsetX;
			int textOffsetY = (int)(boxOffsetY + (boxHeight - fontSize) / 2);

			Font f = new Font("Arial", fontSize);

			g.FillRectangle(sb, boxOffsetX, boxOffsetY, boxWidth, boxHeight);
			g.DrawString("AI turn...", f, sbs, textOffsetX, textOffsetY - 10);
		}
	}
}
