using System.ComponentModel.DataAnnotations;

namespace BarisKuafor.Models
{
    public class Berberler
    {
        [Key]
        public int Id {  get; set; }
        public string BerberAdi {  get; set; }
        
        public string Duzeyi {  get; set; }
        
        public string TelefonNumarasi {  get; set; }

        

    }
}
