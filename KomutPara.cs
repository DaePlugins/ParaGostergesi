using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using fr34kyn01535.Uconomy;

namespace DaeParaGostergesi
{
    internal class KomutPara : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "para";
        public string Help => "Para göstergesini açıp kapatır.";
        public string Syntax => "/para";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>{ "dae.paragostergesi.para" };

        public void Execute(IRocketPlayer komutuÇalıştıran, string[] parametreler)
        {
            var oyuncu = (UnturnedPlayer)komutuÇalıştıran;

            if (!ParaGöstergesi.Örnek.EfektiAlmayacakOyuncular.Contains(oyuncu.CSteamID.m_SteamID))
            {
                ParaGöstergesi.Örnek.EfektiAlmayacakOyuncular.Add(oyuncu.CSteamID.m_SteamID);
                EffectManager.askEffectClearByID(ParaGöstergesi.Örnek.Configuration.Instance.EfektIdsi, oyuncu.CSteamID);
            }
            else
            {
                ParaGöstergesi.Örnek.EfektiAlmayacakOyuncular.Remove(oyuncu.CSteamID.m_SteamID);
                ParaGöstergesi.Örnek.EfektiGönder(oyuncu.CSteamID, ParaGöstergesi.Örnek.Configuration.Instance.XpKullanılsın ? oyuncu.Experience : Uconomy.Instance.Database.GetBalance(oyuncu.CSteamID.ToString()));
            }
        }
    }
}