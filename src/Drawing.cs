using System;
using System.Drawing;
using System.Text;
using Console = Colorful.Console;

namespace MasterPI
{
	public partial class Program
	{
		public static bool RequiresLF(Group g, Group previousGroup = null)
		{
			return !(g.Fits() && g.id == (previousGroup ?? GameGroups[Math.Max(GameGroups.IndexOf(g) - 1, 0)]).id);
		}

		private static void DrawPiHeader()
		{
			Console.WriteLine();
			Console.Write(" 3.", Color.Gray);
			Console.WriteLine();
			
			Console.Write(" ");
		}

		private static void DrawPreviewRow()
		{
			int initialLeft = Console.CursorLeft;
			int i = CurrentGroupIndex;

			Console.Write(" ");
			
			Group g = GetGroup(i);
			do
			{
				DrawGroupMasked(g, ID_COLORS[g.id], false);
				Console.Write(" ");
			} while (!RequiresLF(g = GetGroup(i++), GetGroup(i)));
			
			Console.SetCursorPosition(initialLeft, Console.CursorTop);
		}

		private static bool TryBreakLine(Group g)
		{
			bool result = !g.Fits() || GameGroups[Math.Max(GameGroups.IndexOf(g) - 1, 0)].id != g.id;
			if (result)
				Console.WriteLine();
			return result;
		}
		
		private static void DrawGroupMasked(Color c) => DrawGroupMasked(CurrentGroup, c);
		private static void DrawGroupMasked(Group g, Color c, bool withBackspace = true)
		{
			for (int i = 0; i < g.characters.Length; i++)
				Console.Write(".", g, c);

			if (withBackspace)
				for (int i = 0; i < CurrentGroup.characters.Length; i++)
					Console.Write("\b");			
		}
		
		private static void DrawFail(Color c)
		{
			// Draws the rest of the characters in gray.
			for (int i = DigitIndex; i < CurrentGroup.characters.Length; i++)
			{
				Console.Write(CurrentGroup.characters[i], PREVIEW_COLOR);
			}
			
			// Next line.
			Console.WriteLine();
			Console.WriteLine();
			Console.Write(" ");
			
			StringBuilder barChars = new StringBuilder("".PadLeft(PBAR_LENGTH, BAR_CHAR_EMPTY));
			float percent = (GlobalDigitIndex + 1) / (float) TARGET_MAX;
			int barLength = (int) Math.Floor(percent * PBAR_LENGTH);

			if (barLength >= 2)
			{
				for (int i = 0; i < barLength; i++)
				{
					if (i == 0)
						barChars[i] = BAR_CHAR_LEFT;
					else if (i == barLength - 1)
						barChars[i] = BAR_CHAR_RIGHT;
					else
						barChars[i] = BAR_CHAR_FILL;
				}
			}
			
			Console.WriteLine($"{GlobalDigitIndex}/{TARGET_MAX}    {(percent * 100f):0.00}%    {PBAR_CHAR_START}{barChars}{PBAR_CHAR_END}", c);
		}
	}
}

