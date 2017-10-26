using System.Linq;
using Colorful;

namespace MasterPI
{
	public partial class Program
	{
		private static void MainLogic()
		{
			while (true)
			{
				Console.Clear();

				DigitIndex = 0;
				if (MODE_RANDOM)
				{
					BootTraining();
				} else
				{
					Start();
				}
				
				while (LogicDigit()) { }
			}
		}
		
		private static bool AdvanceGroup()
		{
			int icGroup = CurrentGroupIndex;

			if (icGroup >= GameGroups.Count - 1)
			{
				GenGroup();

				if (MODE_HAS_FINISH)
				{
					return true;
				}
			}
			
			PreviousGroup = CurrentGroup;
			CurrentGroup = GameGroups[icGroup + 1];
			DigitIndex = 0;
			return false;
		}
		
		private static Group GetGroup(int index)
		{
			int d = GameGroups.Count - index;
			while (GameGroups.Count <= index)
			{
				GenGroup();
			}
			
			return GameGroups[index];
		}

		private static void GenGroup()
		{
			int lenCurrent = GameGroups.Sum(g => g.characters.Length);
			string pi = MakePi(lenCurrent + NEW_GROUP_SIZE);
			
			string newDigits = pi.Substring(lenCurrent + 1, NEW_GROUP_SIZE);
			
			Group lastGrp = GameGroups.Last();
			GameGroups.Add(new Group(newDigits, lastGrp.id));
		}
	}
}

