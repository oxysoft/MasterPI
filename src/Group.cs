using Colorful;
using Newtonsoft.Json;

namespace MasterPI
{
	public partial class Program
	{
		public class Group
		{
			public readonly int id;
			public readonly string characters;
			public int success, fails;
			[JsonIgnore] public Vec2[] lastDrawPositions; 

			public Group(string characters, int id)
			{
				this.characters = characters;
				this.id = id;
				lastDrawPositions = new Vec2[characters.Length];
			}

			public bool Fits() => characters.Length + Console.CursorLeft < MAX_ROW_LENGTH;
		}
	}
}

