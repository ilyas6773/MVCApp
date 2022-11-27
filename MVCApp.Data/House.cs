using System.ComponentModel.DataAnnotations;

namespace MVCAppData
{
    public class House
    {
        public int Id { get; set; } = 0;
        [Required]
        public String Title { get; set; } = "Default Name";
        public int Price { get; set; } = 0;
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public double Geo_Lat { get; set; } = 1;
        public double Geo_Lon { get; set; } = 1;
        public int status { get; set; } = 1;
        public string file { get; set; } = string.Empty;
    }
}
