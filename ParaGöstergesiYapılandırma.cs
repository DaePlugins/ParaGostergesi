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

        public void LoadDefaults()
        {
            EfektIdsi = 15962;

            BakiyedekiKüsuratıGizle = false;
            GizlemekİçinMinimumBakiye = 100000.00m;
            
            BakiyeRengi = "00FF00";
            ParaBirimi = "TRY";
            ParaBirimiRengi = "00FF00";

            XpKullanılsın = true;
            XpKullanılırkenParaBiriminiGizle = true;
        }
    }
}