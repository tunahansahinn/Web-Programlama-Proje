using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarisKuafor.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        public DateTime RandevuZamani { get; set; }

        [ForeignKey("Berberler")]
        public int BERBERLERID { get; set; }
        public Berberler Berberler { get; set; }
        public string Adiniz { get; set; }
        [Phone(ErrorMessage = "Telefon Numarasız olmaz")]
        public string TelefonNumaraniz { get; set; }


        [ForeignKey("Beceriler")]
        public int BECERILERID { get; set; }
        public Beceriler Beceriler { get; set; }
        public string? Okeyleme { get; set; }
    }
}
