namespace FreeCourse.Web.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }
        public string GatewayBaseUri { get; set; }
        public string PhotoStockUri { get; set; }
        public SericeApi Catalog { get; set; }
        public SericeApi PhotoStock { get; set; }
        public SericeApi Basket { get; set; }
        public SericeApi Discount { get; set; }
        public SericeApi Payment { get; set; }
        public SericeApi Order { get; set; }
    }

    public class SericeApi
    {
        public string Path{ get; set; }
    }
}
