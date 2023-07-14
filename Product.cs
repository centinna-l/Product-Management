public class Product
{

    public string product_name { get; set; }
    public decimal product_price { get; set; }
    public string product_picture { get; set; }
    public string product_description { get; set; }
    public string product_currency { get; set; }
    public int product_qty { get; set; }

    public Product(string product_name, decimal product_price, string product_picture, string product_description, string product_currency, int product_qty
)
    {
        product_name = product_name;
        product_price = product_price;
        product_picture = product_picture;
        product_description = product_description;
        product_currency = product_currency;
        product_qty = product_qty
;
    }
}
