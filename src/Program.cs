using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Console = Colorful.Console;

namespace MasterPI
{
	public partial class Program
	{
		private static FileInfo fiExe;
		private static DirectoryInfo diExe;

		static Group PreviousGroup { get; set; }
		static List<Group> AllGroups { get; set; }
		static List<Group> GameGroups { get; set; }

		static Group CurrentGroup { get; set; }
		static int CurrentGroupIndex => GameGroups.IndexOf(CurrentGroup);

		static int DigitIndex { get; set; }
		static int GlobalDigitIndex => GameGroups.Take(CurrentGroupIndex).Sum(g => g.characters.Length) + DigitIndex;
		static bool isRestart;
		
		public static void Main(string[] args)
		{
			fiExe = new FileInfo(Assembly.GetEntryAssembly().Location);
			diExe = fiExe.Directory;
			
			Deserialize();
			
			MainLogic();
		}

		private static void Start()
		{
			GameGroups = AllGroups.ToList();
			PreviousGroup = CurrentGroup = GameGroups[0];
			
			DrawPiHeader();
			DrawPreviewRow();
		}
		
		private static void BootTraining()
		{
			float[] buckets = new float[AllGroups.Count];

			for (int i = 0; i < AllGroups.Count; i++)
				buckets[i] = AllGroups[i].success / (float) AllGroups[i].fails; 
			
			List<Group> peaks = new List<Group>();
			for (int i = 0; i < buckets.Length; i++)
			{
				if (i <= 0)                  continue;
				if (i >= buckets.Length - 1) continue;
				
				float wl = buckets[i - 1];
				float wr = buckets[i + 1];
				float w = buckets[i];


				if (wl > w && wl < wr)
				{
					peaks.Add(AllGroups[i]);
				}
			}

			peaks = peaks.OrderBy(g => buckets[AllGroups.IndexOf(g)]).ToList();
			
			CurrentGroup = AllGroups[AllGroups.IndexOf(peaks[0]) - 2];
			GameGroups = AllGroups.GetRange(AllGroups.IndexOf(CurrentGroup) - 2, AllGroups.IndexOf(CurrentGroup) + 1);
			DrawPreviewRow();
		}
		
		private static bool LogicDigit()
		{
			// Wait for good key (number OR escape).
			ConsoleKeyInfo cki;
			do
			{
				cki = Console.ReadKey(true);
			} while(
				!char.IsNumber(cki.KeyChar)
				&& cki.Key != ConsoleKey.Escape
				&& cki.Key != ConsoleKey.Enter
				);

			// Restart if escape.
			if (cki.Key == ConsoleKey.Escape)
			{
				isRestart = true;
				return false;
			} else if (cki.Key == ConsoleKey.Enter)
			{
				DrawFail(Color.GreenYellow);

				do
				{
				} while (Console.ReadKey(true).Key != ConsoleKey.Enter);
				
				return false;
			}
			
			// Gets the current pi digit and compares it to the entered digit.
			char piChar = CurrentGroup.characters[DigitIndex++];
			if (piChar == cki.KeyChar)
			{
				// Draw the digit.
				
				CurrentGroup.lastDrawPositions[DigitIndex - 1].x = Console.CursorLeft; 
				CurrentGroup.lastDrawPositions[DigitIndex - 1].y = Console.CursorTop; 
				Console.Write(piChar.ToString(), ID_COLORS[CurrentGroup.id]);

				// Checks if we've completed the group.
				if (DigitIndex >= CurrentGroup.characters.Length)
				{
					CurrentGroup.success++;

					AdvanceGroup();
					if (TryBreakLine(CurrentGroup))
					{
						DrawPreviewRow();
					}
					
					Console.Write(" ");
					DrawGroupMasked(ID_COLORS[CurrentGroup.id]);
					Serialize();
				}

				// Increment the global counter.
				return true;
			} else
			{
				CurrentGroup.fails++;

				if (MODE_SETBACK)
				{
					// Move to start of previous 2 groups.
					CurrentGroup = GameGroups[Math.Max(CurrentGroupIndex - 1, 0)];
					DigitIndex = 0;

					// Hide all shown groups after the current.
					for (int i = CurrentGroupIndex; i < GameGroups.Count; i++)
					{
						Group g = GameGroups[i];
						foreach (Vec2 pos in g.lastDrawPositions)
						{
							Console.CursorLeft = pos.x;
							Console.CursorTop  = pos.y;
							Console.Write(".", ID_COLORS[g.id]);
						}
					}
					
					Console.CursorLeft = CurrentGroup.lastDrawPositions[0].x;
					Console.CursorTop  = CurrentGroup.lastDrawPositions[0].y;
					DrawGroupMasked(ID_COLORS[CurrentGroup.id]);
					
					return true;
				} else
				{
					DrawFail(Color.OrangeRed);

					do
					{
					} while (Console.ReadKey(true).Key != ConsoleKey.Enter);
					return false;
				}
			}
		}
	}
}