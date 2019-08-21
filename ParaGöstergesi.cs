using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using fr34kyn01535.Uconomy;
using Harmony;

namespace DaeParaGostergesi
{
	public class ParaGöstergesi : RocketPlugin<ParaGöstergesiYapılandırma>
    {
        public static ParaGöstergesi Örnek { get; private set; }
        private HarmonyInstance _harmony;

		private string _paraBirimi = "";

        private readonly Dictionary<string, string> _paraBirimleri = new Dictionary<string, string>(9)
        {
            { "try", "₺" },
            { "usd", "$" },
            { "eur", "€" },
            { "jpy", "¥" },
            { "gbp", "£" },
            { "chf", "₣" },
            { "cny", "元" },
            { "won", "₩" },
            { "rub", "₽" }
        };

        public List<ulong> EfektiAlmayacakOyuncular { get; } = new List<ulong>();

        protected override void Load()
        {
			Örnek = this;

            if (!Configuration.Instance.XpKullanılsın || !Configuration.Instance.XpKullanılırkenParaBiriminiGizle)
            {
                _paraBirimi = Translate("Format", _paraBirimleri.TryGetValue(Configuration.Instance.ParaBirimi.ToLower(), out var paraBirimi) ? paraBirimi : "₺", Configuration.Instance.ParaBirimiRengi);
            }

            foreach (var steamOyuncu in Provider.clients)
			{
                if (!EfektiAlmayacakOyuncular.Contains(steamOyuncu.playerID.steamID.m_SteamID))
                {
                    EfektiGönder(steamOyuncu.playerID.steamID, Configuration.Instance.XpKullanılsın ? steamOyuncu.player.skills.experience : Uconomy.Instance.Database.GetBalance(steamOyuncu.playerID.ToString()));
                }
			}

            if (Configuration.Instance.XpKullanılsın)
            {
                UnturnedPlayerEvents.OnPlayerUpdateExperience += TecrübeGüncellendiğinde;
            }
            else
            {
                _harmony = HarmonyInstance.Create("dae.paragostergesi");
                _harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
			
            U.Events.OnPlayerConnected += OyuncuBağlandığında;
        }
		
        protected override void Unload()
        {
            Örnek = null;

            foreach (var steamOyuncu in Provider.clients)
            {
                EffectManager.askEffectClearByID(Configuration.Instance.EfektIdsi, steamOyuncu.playerID.steamID);
            }
            
            if (Configuration.Instance.XpKullanılsın)
            {
                UnturnedPlayerEvents.OnPlayerUpdateExperience -= TecrübeGüncellendiğinde;
            }
            else
            {
                _harmony.UnpatchAll("dae.paragostergesi");
                _harmony = null;
            }

            U.Events.OnPlayerConnected -= OyuncuBağlandığında;
        }
        
        private void TecrübeGüncellendiğinde(UnturnedPlayer oyuncu, uint tecrübe)
        {
            if (!EfektiAlmayacakOyuncular.Contains(oyuncu.CSteamID.m_SteamID))
            {
                EfektiGönder(oyuncu.CSteamID, tecrübe);
            }
        }

        private void OyuncuBağlandığında(UnturnedPlayer oyuncu)
        {
            if (!EfektiAlmayacakOyuncular.Contains(oyuncu.CSteamID.m_SteamID))
            {
                EfektiGönder(oyuncu.CSteamID, Configuration.Instance.XpKullanılsın ? oyuncu.Experience : Uconomy.Instance.Database.GetBalance(oyuncu.Id));
            }
        }

        public void EfektiGönder(CSteamID steamId, object bakiye)
        {
            string format;
            if (Configuration.Instance.XpKullanılsın || Configuration.Instance.BakiyedekiKüsuratıGizle && Convert.ToDecimal(bakiye) > Configuration.Instance.GizlemekİçinMinimumBakiye)
            {
                format = "{0:#,##0}";
            }
            else
            {
                format = "{0:#,##0.00}";
            }

            bakiye = string.Format(CultureInfo.GetCultureInfo("tr-TR"), format, bakiye);

            EffectManager.sendUIEffect(Configuration.Instance.EfektIdsi, 15962, steamId, true, Translate("Format", bakiye, Configuration.Instance.BakiyeRengi), _paraBirimi);
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "Format", "<color=#{1}>{0}</color>" }
        };
    }
}