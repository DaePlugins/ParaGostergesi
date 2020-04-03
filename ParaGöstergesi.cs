using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DaeParaGostergesi.Modeller;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using fr34kyn01535.Uconomy;
using HarmonyLib;

namespace DaeParaGostergesi
{
	public class ParaGöstergesi : RocketPlugin<ParaGöstergesiYapılandırma>
    {
        public static ParaGöstergesi Örnek { get; private set; }
        private Harmony _harmony;

        private string _mevcutParaBirimi;

        public List<ulong> EfektiAlmayacakOyuncular { get; } = new List<ulong>();

        protected override void Load()
        {
			Örnek = this;

            if (!Configuration.Instance.XpKullanılsın || !Configuration.Instance.XpKullanılırkenParaBiriminiGizle)
            {
                _mevcutParaBirimi = (Configuration.Instance.ParaBirimleri.FirstOrDefault(p => p.Kısaltma == Configuration.Instance.MevcutParaBirimi) ?? new ParaBirimi("TRY", "₺")).Birim;
            }

            foreach (var steamOyuncu in Provider.clients.Where(s => !EfektiAlmayacakOyuncular.Contains(s.playerID.steamID.m_SteamID)))
            {
                EfektiGönder(steamOyuncu.playerID.steamID, Configuration.Instance.XpKullanılsın ? steamOyuncu.player.skills.experience : Uconomy.Instance.Database.GetBalance(steamOyuncu.playerID.ToString()));
            }

            if (Configuration.Instance.XpKullanılsın)
            {
                UnturnedPlayerEvents.OnPlayerUpdateExperience += TecrübeGüncellendiğinde;
            }
            else
            {
                _harmony = new Harmony("dae.paragostergesi");
                _harmony.PatchAll();
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

            EffectManager.sendUIEffect(Configuration.Instance.EfektIdsi, 15962, steamId, true, Translate("Format", bakiye, Configuration.Instance.BakiyeRengi),
                $"<color=#{Configuration.Instance.ParaBirimiRengi}>{_mevcutParaBirimi}</color>");
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "Format", "<color=#{1}>{0}</color>" }
        };
    }
}