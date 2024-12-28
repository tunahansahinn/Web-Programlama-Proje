using System.ComponentModel.DataAnnotations.Schema;

namespace BarisKuafor.Models
{
    public class Mesailer
    {
        public int Id { get; set; }
        [ForeignKey("Berberler")]
        public int BERBERLERID {  get; set; }
        public Berberler berberler { get; set; }

        public DateTime mesai {  get; set; }

    }
}
