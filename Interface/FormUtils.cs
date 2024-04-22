using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessApp.Interface
{
	public class FormUtils
	{
		public delegate void Delegate();
		public static void ShowVictoryDialog()//Delegate callback)
		{
			if (MessageBox.Show("AI is in checkmate.", "Victory") == DialogResult.OK)
			{
				//callback();
			}

		}
		public static void ShowAIStalemateDialog()//Delegate callback)
		{
			if (MessageBox.Show("The AI is in stalemate.", "Stalemate") == DialogResult.OK)
			{
				//callback();
			}
		}

		public static void ShowPlayerStalemateDialog()//Delegate callback)
		{
			if (MessageBox.Show("You are in stalemate.", "Stalemate") == DialogResult.OK)
			{
				//callback();
			}
		}

		public static void ShowDefeatDialog()//Delegate callback)
		{
			if (MessageBox.Show("You are in checkmate.", "Defeat") == DialogResult.OK)
			{
				//callback();
			}
		}
	}
}
