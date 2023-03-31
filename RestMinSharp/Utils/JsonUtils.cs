using Newtonsoft.Json;

namespace RestMinSharp.Utils
{
	public class JsonUtils
	{
		public static string IdentJsonString(string json)
		{
			try
			{
				return JsonConvert.SerializeObject(
				JsonConvert.DeserializeObject(json), Formatting.Indented);
			}
			catch
			{
				return json;
			}
		}
	}
}