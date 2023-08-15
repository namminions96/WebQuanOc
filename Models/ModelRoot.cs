namespace CheckApiWeb.Models
{
    public class ModelRoot
    {
        public class TransDiscountEntry
        {
            public int LineNo { get; set; }
            public int ParentLineNo { get; set; }
            public string OfferNo { get; set; }
            public string OfferType { get; set; }
            public double DiscountAmount { get; set; }
            public double Qty { get; set; }
            public string Note { get; set; }
        }

        public class TransLine
        {
            public int LineNo { get; set; }
            public int ParentLineNo { get; set; }
            public string ItemNo { get; set; }
            public string ItemName { get; set; }
            public string ItemName2 { get; set; }
            public string Barcode { get; set; }
            public string Uom { get; set; }
            public double UnitPrice { get; set; }
            public double Qty { get; set; }
            public double DiscountAmount { get; set; }
            public string VatGroup { get; set; }
            public double VatPercent { get; set; }
            public double VatAmount { get; set; }
            public double LineAmountExcVAT { get; set; }
            public double LineAmountIncVAT { get; set; }
            public bool IsLoyalty { get; set; }
            public string ItemType { get; set; }
            public object Remark { get; set; }
            public List<TransDiscountEntry> TransDiscountEntry { get; set; }
        }

        public class TransPaymentEntry
        {
            public int LineNo { get; set; }
            public string TenderType { get; set; }
            public double PaymentAmount { get; set; }
            public string ReferenceNo { get; set; }
            public string TransactionId { get; set; }
            public string PayForOrderNo { get; set; }
            public string TransactionNo { get; set; }
            public string ApprovalCode { get; set; }
            public string TraceCode { get; set; }
            public object AdditionalData { get; set; }
        }

        public class Data
        {
            public string AppCode { get; set; }
            public string OrderNo { get; set; }
            public string OrderTime { get; set; }
            public int DeliveryType { get; set; }
            public string CustNo { get; set; }
            public string CustName { get; set; }
            public string CustPhone { get; set; }
            public string CustAddress { get; set; }
            public string CustNote { get; set; }
            public double TotalAmount { get; set; }
            public double PaymentAmount { get; set; }
            public List<TransLine> TransLine { get; set; }
            public List<TransPaymentEntry> TransPaymentEntry { get; set; }
        }

        public class Meta
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public class RootObject
        {
            public Meta Meta { get; set; }
            public Data Data { get; set; }
        }
    }
}

