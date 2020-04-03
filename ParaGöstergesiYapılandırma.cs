using System.Collections.Generic;
using DaeParaGostergesi.Modeller;
using Rocket.API;

namespace DaeParaGostergesi
{
    public class ParaGöstergesiYapılandırma : IRocketPluginConfiguration
    {
        public ushort EfektIdsi { get; set; }

        public bool BakiyedekiKüsuratıGizle { get; set; }
        public decimal GizlemekİçinMinimumBakiye { get; set; }

        public string BakiyeRengi { get; set; }
        public string ParaBirimi { get; set; }
        public string ParaBirimiRengi { get; set; }

        public bool XpKullanılsın { get; set; }
        public bool XpKullanılırkenParaBiriminiGizle { get; set; }

        public string MevcutParaBirimi { get; set; }
        public List<ParaBirimi> ParaBirimleri { get; set; } = new List<ParaBirimi>();

        public void LoadDefaults()
        {
            EfektIdsi = 15962;

            BakiyedekiKüsuratıGizle = false;
            GizlemekİçinMinimumBakiye = 100000.00m;
            
            BakiyeRengi = "FFFFFF";
            ParaBirimi = "TRY";
            ParaBirimiRengi = "00FF00";

            XpKullanılsın = true;
            XpKullanılırkenParaBiriminiGizle = true;

            MevcutParaBirimi = "TRY";
            ParaBirimleri = new List<ParaBirimi>
            {
                new ParaBirimi("TRY", "₺"),
                new ParaBirimi("USD", "$"),
                new ParaBirimi("EUR", "€"),
                new ParaBirimi("JPY", "¥"),
                new ParaBirimi("GBP", "£"),
                new ParaBirimi("CHF", "₣"),
                new ParaBirimi("CNY", "元"),
                new ParaBirimi("WON", "₩"),
                new ParaBirimi("RUB", "₽")
            };
        }
    }
}