using System.Net;
using System.Net.Http.Headers;

public class VK
{
	private readonly Config cfg;
	private readonly HttpClient client;
	private readonly Random random;

	public VK(Config _cfg)
	{
		cfg = _cfg;
		random = new Random();
		client = new()
		{
			BaseAddress = new Uri("https://api.vk.com"),
		};
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cfg.token);
	}

	public async Task<bool> Send(string message, bool debug = false)
	{
		try
		{
			var result = await Post(
				"/method/messages.send",
				new Dictionary<string, string?>
				{
					{"random_id", random.Next(int.MaxValue).ToString()},
					{"peer_id", cfg.peer_id},
					{"message", message},
					{"v", cfg.version},
					{"dont_parse_links", "1"},
				},
				debug
			);
			return result;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return false;
		}
	}

	private async Task<bool> Post(string url, Dictionary<string, string?> input, bool debug = false)
	{
		var post = new FormUrlEncodedContent(input);
		using HttpResponseMessage response = await client!.PostAsync(
			url,
			post
		);
		string json = await response.Content.ReadAsStringAsync();
		if (debug) Console.WriteLine($"[C2VK] [Post/JSON] {response.StatusCode} | {json} ");
		if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"[C2VK] [Fail/HTTP] {response.StatusCode} | {json}");
		if (json != "{\"response\":0}") throw new Exception($"[C2VK] [Fail/VK] {json}");
		return true;
	}

}