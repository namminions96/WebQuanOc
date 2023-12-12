namespace CheckApiWeb.Models
{
    public class ConfigOrder
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? MaOrder { get; set; }
        public int? MaxOrder { get; set; }
        public DateTime? Fromdate { get; set;}
        public DateTime? Todate { get; set; }
        public bool? Status { get; set;}

    }
}
