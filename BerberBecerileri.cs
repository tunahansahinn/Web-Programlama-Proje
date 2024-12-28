using System.ComponentModel.DataAnnotations.Schema;

namespace BarisKuafor.Models
{
    public class BerberBecerileri
    {
        public int Id {  get; set; }

        [ForeignKey("Berberler")]
        public int BERBERLERID {  get; set; }
        public Berberler Berberler { get; set; }

        [ForeignKey("Beceriler")]
        public int BECERILERID {  get; set; }
        public Beceriler Beceriler { get; set; }
    }
}
