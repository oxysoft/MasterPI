using System.Drawing;

namespace MasterPI
{
	public partial class Program
	{
		// MODE
		const bool MODE_SETBACK    = false;
		const bool MODE_RANDOM     = false; // Unfinished/unsupported
		const bool MODE_HAS_FINISH = true;
		
		// GENERAL
		const int TARGET_MAX     = 100;
		const int NEW_GROUP_SIZE = 5;
		
		// VISUALS
		public const int MAX_ROW_LENGTH   = 30;
		static readonly Color[] ID_COLORS = {
			Color.Aquamarine,
			Color.Orange,
			Color.Red,
			Color.Aqua,
			Color.GreenYellow,
			Color.Coral
		};
		static readonly Color PREVIEW_COLOR = Color.DarkOliveGreen;
		
		// ---- PROGRESS
		const int    PBAR_LENGTH     = 60;
		const string PBAR_CHAR_START = "[";
		const char   BAR_CHAR_LEFT   = '<';
		const char   BAR_CHAR_FILL   = '=';
		const char   BAR_CHAR_RIGHT  = '>';
		const char   BAR_CHAR_EMPTY  = '.';
		const string PBAR_CHAR_END   = "]";
	}
}

