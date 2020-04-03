using System.Xml.Serialization;

namespace DaeParaGostergesi.Modeller
{
    public class ParaBirimi
    {
        [XmlAttribute]
        public string Kısaltma { get; set; }
        [XmlAttribute]
        public string Birim { get; set; }

        public ParaBirimi()
        {
        }

        public ParaBirimi(string kısaltma, string birim)
        {
            Kısaltma = kısaltma;
            Birim = birim;
        }
    }
}