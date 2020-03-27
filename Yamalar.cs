using System.Linq;
using SDG.Unturned;
using Steamworks;
using fr34kyn01535.Uconomy;
using HarmonyLib;

namespace DaeParaGostergesi
{
    [HarmonyPatch(typeof(DatabaseManager))]
    [HarmonyPatch("IncreaseBalance")]
    internal class Yamalar
    {
        [HarmonyPostfix]
        private static void BakiyeGüncellendiktenSonra(decimal __result, string id)
        {
            var steamId = new CSteamID(ulong.Parse(id));

            if (Provider.clients.All(o => o.playerID.steamID != steamId) || ParaGöstergesi.Örnek.EfektiAlmayacakOyuncular.Contains(steamId.m_SteamID))
            {
                return;
            }

            ParaGöstergesi.Örnek.EfektiGönder(steamId, __result);
        }
    }
}