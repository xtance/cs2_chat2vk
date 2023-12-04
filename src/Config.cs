using CounterStrikeSharp.API;
using Newtonsoft.Json;

public class Config
{

	public required string token;
	public required string version;
	public required string peer_id;
	public required int admin_id;
	public required int max_per_round;
	public required string use_text;
	public required string fail_text;
	public required string success_text;
	public required string max_per_round_text;

	public static Config Load()
	{
		string path = GetCfgDirectory() + "/chat2vk";
		Directory.CreateDirectory(path);

		using StreamReader reader = new ($"{path}/settings.json");
		string json = reader.ReadToEnd();

		//Console.WriteLine($"[C2VK] Settings: {json}");
		Config cfg = JsonConvert.DeserializeObject<Config>(json) ?? throw new Exception("No settings found!");
		return cfg;
	}

	private static string GetCfgDirectory()
	{
		return Server.GameDirectory + "/csgo/cfg";
	}
}