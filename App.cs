using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
namespace App;

public class App : BasePlugin
{
	public override string ModuleName => "Chat2VK";
	public override string ModuleAuthor => "xtance";
	public override string ModuleDescription => "Server to VK";
	public override string ModuleVersion => "0.1";

	private VK? vk;
	private static readonly Dictionary<CCSPlayerController, int> amountThisRound = new();

	public override void Load(bool hotReload)
	{
		Console.WriteLine($"[C2VK] Loading at {DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt")}");
		Console.WriteLine($"[C2VK] CSS Version: {Api.GetVersion()}");

		Config cfg = Config.Load();
		vk = new VK(cfg);
 
		AddCommand("vk", "SteamID", async (player, info) =>
		{
			if (player == null)
			{
				Console.WriteLine($"[C2VK] Testing: {info.ArgString}");
				var result = await vk.Send($"Test | {info.ArgString}");
				Console.WriteLine($"[C2VK] Result: {result}");
				return;
			}
			else if (info.ArgCount <= 1)
			{
				player.PrintToChat(cfg.use_text);
				return;
			}
			else if (amountThisRound.ContainsKey(player) && amountThisRound[player] >= cfg.max_per_round)
			{
				player.PrintToChat($"{cfg.max_per_round_text} {cfg.max_per_round}");
				return;
			}
			else
			{
				var message = $" >> {player.PlayerName}: \n >> {info.ArgString}";
				var result = await vk.Send(message);
				if (result) player.PrintToChat(cfg.success_text);
				else player.PrintToChat(cfg.fail_text);

				amountThisRound.TryGetValue(player, out var amount);
				amountThisRound[player] = amount + 1;
			}
		});

		RegisterEventHandler<EventRoundStart>((@event, info) =>
		{
			amountThisRound.Clear();
			return HookResult.Continue;
		});
	}

}



