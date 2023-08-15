namespace CheckApiWeb.Models
{
    public class CartSave
    {
        public int Id { get; set; }
        public int? Masp { get; set; }
        public string? Tensp { get; set; }
        public double Giaban { get; set; }
        public int Soluong { get; set; }
        public double? TongTien { get; set;}
        public string? userid { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set;}
        public string? Name { get; set;}
    }
}
