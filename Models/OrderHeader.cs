namespace CheckApiWeb.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public int? Madh { get; set; }
        public string? Tenkh { get; set; }
        public string?  Sodienthoai{ get; set; }
        public double Tongtien { get; set; }
        public string? Diachi { get; set; }
        public DateTime Orderdate { get; set; }
        public bool Status { get; set;}
        public string User { get; set; }
    }
}
