using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MasterPI
{
	public partial class Program
	{
		private static void Serialize()
		{
			string data = JsonConvert.SerializeObject(GameGroups, Formatting.Indented);
			File.WriteAllText(Path.Combine(diExe.FullName, "data.json"), data);
		}

		private static void Deserialize()
		{
			AllGroups = JsonConvert.DeserializeObject<List<Group>>(File.ReadAllText(Path.Combine(diExe.FullName, "data.json")));
		}
	}
}

