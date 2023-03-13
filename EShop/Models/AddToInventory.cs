using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;


namespace EShop.Models
{
    public class AddToInventory
    {
        [Required(ErrorMessage = "This field is mandatory")]
        public string ProductName { get; set; }

        [Required]
        [Range(1, 50000, ErrorMessage = "Price must be between 1 and 50000")]
        public int Price { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "This field is mandatory")]
        public string KeywordSearch { get; set; }
        [Required(ErrorMessage = "This field is mandatory")]
        public string Category { get; set; }

        public SelectList ToSelectList(DataTable table, string key, string value)
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