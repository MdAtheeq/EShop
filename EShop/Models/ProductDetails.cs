using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace EShop.Models
{
    public class ProductDetails
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string ModelName { get; set; }

        //[Required]
        //[Range(1, 50000, ErrorMessage = "Price must be between 1 and 50000")]
        public int Price { get; set; }

        [Required(ErrorMessage ="This field is mandatory")]
        public string Size { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string ExpectedDelivery { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string COD { get; set; }

        [Required(ErrorMessage = "Vendor Cost cannot be zero")]
        public int VendorCost { get; set; }

        [Required(ErrorMessage = "Selling Price cannot be zero")]
        public int SellingPrice { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string Seller { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string ImportedFrom { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string WarehouseLocation { get; set; }
        public string ProductImage { get; set; }
        public int CategoryId { get; set; }
        public int OrderID { get; set; }

        //[Required(ErrorMessage = "This field is mandatory")]
        public string KeywordSearch { get; set; }

        
        public string Category { get; set; }

        [Required(ErrorMessage = "Quantity can't be zero")]
        [Range(1, 50, ErrorMessage = "Quantity can't be zero")]
        public int Quantity { get; set; }

        public int ProductDetailsID { get; set; }

        public string CustomerName { get; set; }

        public string ShippingAddress { get; set; }

        public string ContactNumber { get; set; }

        public string PaymentMethod { get; set; }

        public int CartID { get; set; }
        public int ReviewID { get; set; }
        public string Stars { get; set; }
        public string Review { get; set; }

        public int TotalPrice { get; set; }

        public SelectList ToSelectList(DataTable table, string key,string value)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[value].ToString(),
                    Value = (row[key].ToString())
                });
            }
            return new SelectList(list, "Value", "Text");
        }


    }
}