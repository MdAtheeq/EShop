using EShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList.Mvc;
using PagedList;

namespace EShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        string connectionString = @"Data Source = IND-L599\SQLEXPRESS; Initial Catalog = aqshop; Integrated Security = True";
        [HttpGet]
        public ActionResult Index()
        {
           
            DataTable dtblProduct = new DataTable();

            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spIndex", sqlCon);
                    sqlDa.Fill(dtblProduct);
                }
                return View(dtblProduct);
            }
        }

        public ActionResult ViewItem(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spViewItem @ProductID", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productdetails.ProductImage = dtblProduct.Rows[0][1].ToString();
                productdetails.Brand = dtblProduct.Rows[0][2].ToString();
                productdetails.ModelName = dtblProduct.Rows[0][3].ToString();
                productdetails.Price = Convert.ToInt32(dtblProduct.Rows[0][4].ToString());
                productdetails.Size = dtblProduct.Rows[0][5].ToString();
                productdetails.ExpectedDelivery = dtblProduct.Rows[0][6].ToString();
                productdetails.COD = dtblProduct.Rows[0][7].ToString();
                return View(productdetails);

            }
            else
                return View();


        }

        public ActionResult ShowReviews(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spShowReviews @ProductID", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            //if (dtblProduct.Rows.Count >0)
            //{
            //    productdetails.ReviewID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
            //    productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
            //    productdetails.CustomerName = dtblProduct.Rows[0][2].ToString();
            //    productdetails.Stars = dtblProduct.Rows[0][3].ToString();
            //    productdetails.Review = dtblProduct.Rows[0][4].ToString();
            //}

            //else
            //{
            //    return View();
            //}
            if (dtblProduct.Rows.Count>0)
            {
                return View(dtblProduct);
            }
            else
            {
                return RedirectToAction("AddReview", "Product");
            }
           


        }


        public ActionResult AddReview(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Review Where ProductID=@ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count >0 )
            {
                productdetails.ReviewID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                return View(productdetails);
            }
            else
            {
                return View();
            }
                
        }

        [HttpPost]
        public ActionResult AddReview(ProductDetails productdetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Review VALUES (@ProductID, @CustomerName, @Stars, @Review)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                sqlCmd.Parameters.AddWithValue("@CustomerName", productdetails.CustomerName);
                sqlCmd.Parameters.AddWithValue("@Stars", productdetails.Stars);
                sqlCmd.Parameters.AddWithValue("@Review", productdetails.Review);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index","Product");
        }

        public ActionResult AddtoCart(int id)
        {
            ProductDetails productModel = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Product Where ProductID=@ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productModel.ProductID = Convert.ToInt32(dtblProduct.Rows[0][0]);
                productModel.ProductName = dtblProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToInt32(dtblProduct.Rows[0][2]);
                return View(productModel);

            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult AddtoCart(ProductDetails productdetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "Insert Into Cart Values(@ProductId, @Quantity)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                //sqlCmd.Parameters.AddWithValue("@ProductName", productdetails.ProductName);
                //sqlCmd.Parameters.AddWithValue("@Price", productdetails.Price);
                sqlCmd.Parameters.AddWithValue("@Quantity", productdetails.Quantity);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ShowCart");
        }

        public ActionResult ShowCart()
        {
            DataTable dtblProduct = new DataTable();

            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spShowCart", sqlCon);
                    sqlDa.Fill(dtblProduct);
                }

                return View(dtblProduct);
            }

        }

        public ActionResult RemoveFromCart(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Cart WHERE CartID = @CartID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@CartID", id);

                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ShowCart");
        }

        public ActionResult SearchProducts()
        {
            //ProductDetails productdetails = new ProductDetails;
            DataTable dtblproduct = new DataTable();
            //ViewBag.table = dtblproduct;
            return View(dtblproduct);
            
            

        }

        
        

        [HttpPost]
        public ActionResult SearchProducts(string search)
            
        {
            DataTable dtblproduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product WHERE KeywordSearch like '%' + @KeywordSearch + '%'", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("KeywordSearch", search);
                sqlDa.Fill(dtblproduct);
            }
            return View(dtblproduct);
        }

        public ActionResult Filter()
        {
             DataTable dtblproduct = new DataTable();
                return View(dtblproduct);
        }

        [HttpPost]
        public ActionResult Filter(string rate)
        {
            DataTable dtblproduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product WHERE Price" + rate, sqlCon);
                
                sqlDa.Fill(dtblproduct);
            }
            return View(dtblproduct);

        }

      

        public ActionResult ShowInventory()
        {
            DataTable dtblProduct = new DataTable();

            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product", sqlCon);
                    sqlDa.Fill(dtblProduct);
                }
                return View(dtblProduct);
            }
        }

        public ActionResult ViewInventoryItem(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM ProductDetails Where ProductID=@ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductId", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productdetails.ProductDetailsID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                productdetails.Brand = dtblProduct.Rows[0][2].ToString();
                productdetails.ModelName = dtblProduct.Rows[0][3].ToString();
                productdetails.Price = Convert.ToInt32(dtblProduct.Rows[0][4].ToString());
                productdetails.Size = dtblProduct.Rows[0][5].ToString();
                productdetails.ExpectedDelivery = dtblProduct.Rows[0][6].ToString();
                productdetails.COD = dtblProduct.Rows[0][7].ToString();
                productdetails.VendorCost = Convert.ToInt32(dtblProduct.Rows[0][8].ToString());
                productdetails.SellingPrice = Convert.ToInt32(dtblProduct.Rows[0][9].ToString());
                productdetails.Seller = dtblProduct.Rows[0][10].ToString();
                productdetails.ImportedFrom = dtblProduct.Rows[0][11].ToString();
                productdetails.WarehouseLocation = dtblProduct.Rows[0][12].ToString();
                return View(productdetails);

            }
            else
                return View();
        }


        public ActionResult AddtoInventory()
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select Category from Category", conn);

                sqlDataAdapter.Fill(dtblProduct);
                ViewBag.ProductList = productdetails.ToSelectList(dtblProduct, "Category", "Category");

            }
            return View();
        }
        

        [HttpPost]
        public ActionResult AddtoInventory(AddToInventory productdetails)

        {
            if (ModelState.IsValid == true)
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO Product VALUES (@ProductName,@Price,@ProductImage,@KeywordSearch,@Category)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    //sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                    sqlCmd.Parameters.AddWithValue("@ProductName", productdetails.ProductName);
                    sqlCmd.Parameters.AddWithValue("Price", productdetails.Price);
                    sqlCmd.Parameters.AddWithValue("@ProductImage", productdetails.ProductImage);
                    sqlCmd.Parameters.AddWithValue("@KeywordSearch", productdetails.KeywordSearch);
                    sqlCmd.Parameters.AddWithValue("@Category", productdetails.Category);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction("AddItemDescription", "Product");

            }
            else
            {
                DataTable dtblProduct = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select Category from Product", conn);

                    sqlDataAdapter.Fill(dtblProduct);
                    ViewBag.ProductList = productdetails.ToSelectList(dtblProduct, "Category", "Category");

                }
                return View(productdetails);
            }
        }

        public ActionResult AddItemDescription()
        {
            ProductDetails productModel = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT TOP 1 * FROM Product ORDER BY ProductID DESC";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                //sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productModel.ProductID = Convert.ToInt32(dtblProduct.Rows[0][0]);
                productModel.ProductName = dtblProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToInt32(dtblProduct.Rows[0][2]);
                return View(productModel);

            }
            else
                return View();

            //return View();
        }

        [HttpPost]
        public ActionResult AddItemDescription(ProductDetails productdetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO ProductDetails VALUES (@ProductID,@Brand,@ModelName,@Price,@Size,@ExpectedDelivery,@COD, @VendorCost, @SellingPrice, @Seller, @ImportedFrom, @WarehouseLocation)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                sqlCmd.Parameters.AddWithValue("@Brand", productdetails.Brand);
                sqlCmd.Parameters.AddWithValue("ModelName", productdetails.ModelName);
                sqlCmd.Parameters.AddWithValue("@Price", productdetails.Price);
                sqlCmd.Parameters.AddWithValue("@Size", productdetails.Size);
                sqlCmd.Parameters.AddWithValue("@ExpectedDelivery", productdetails.ExpectedDelivery);
                sqlCmd.Parameters.AddWithValue("@COD", productdetails.COD);
                sqlCmd.Parameters.AddWithValue("@VendorCost", productdetails.VendorCost);
                sqlCmd.Parameters.AddWithValue("@SellingPrice", productdetails.SellingPrice);
                sqlCmd.Parameters.AddWithValue("@Seller", productdetails.Seller);
                sqlCmd.Parameters.AddWithValue("@ImportedFrom", productdetails.ImportedFrom);
                sqlCmd.Parameters.AddWithValue("@WarehouseLocation", productdetails.WarehouseLocation);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ShowInventory", "Product");

        }

        public ActionResult EditInventory(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Product Where ProductID=@ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][0]);
                productdetails.ProductName = dtblProduct.Rows[0][1].ToString();
                productdetails.Price = Convert.ToInt32(dtblProduct.Rows[0][2]);
                productdetails.ProductImage = dtblProduct.Rows[0][3].ToString();
                productdetails.KeywordSearch = dtblProduct.Rows[0][4].ToString();
                productdetails.Category = dtblProduct.Rows[0][5].ToString();
                return View(productdetails);

            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult EditInventory(ProductDetails productdetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Product SET ProductName = @ProductName, Price=@Price, ProductImage = @ProductImage, KeywordSearch = @KeywordSearch, Category = @Category WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                sqlCmd.Parameters.AddWithValue("@ProductName", productdetails.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productdetails.Price);
                sqlCmd.Parameters.AddWithValue("@ProductImage", productdetails.ProductImage);
                sqlCmd.Parameters.AddWithValue("@KeywordSearch", productdetails.KeywordSearch);
                sqlCmd.Parameters.AddWithValue("@Category", productdetails.Category);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ShowInventory", "Product");
        }

        public ActionResult EditInventoryDetails(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM ProductDetails Where ProductID=@ProductID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("ProductID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productdetails.ProductDetailsID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                productdetails.Brand = dtblProduct.Rows[0][2].ToString();
                productdetails.ModelName = dtblProduct.Rows[0][3].ToString();
                productdetails.Price = Convert.ToInt32(dtblProduct.Rows[0][4].ToString());
                productdetails.Size = dtblProduct.Rows[0][5].ToString();
                productdetails.ExpectedDelivery = dtblProduct.Rows[0][6].ToString();
                productdetails.COD = dtblProduct.Rows[0][7].ToString();
                productdetails.VendorCost = Convert.ToInt32(dtblProduct.Rows[0][8].ToString());
                productdetails.SellingPrice = Convert.ToInt32(dtblProduct.Rows[0][9].ToString());
                productdetails.Seller = dtblProduct.Rows[0][10].ToString();
                productdetails.ImportedFrom = dtblProduct.Rows[0][11].ToString();
                productdetails.WarehouseLocation = dtblProduct.Rows[0][12].ToString();
                return View(productdetails);

            }
            else
                return View();
        }

        [HttpPost]

        public ActionResult EditInventoryDetails(ProductDetails productdetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE ProductDetails SET ProductID = @ProductID, Brand = @Brand, ModelName=@ModelName, Price = @Price, Size = @Size, ExpectedDelivery = @ExpectedDelivery, COD = @COD, VendorCost = @VendorCost, SellingPrice = @SellingPrice, Seller = @Seller, ImportedFrom = @ImportedFrom, WarehouseLocation = @WarehouseLocation WHERE ProductID = @ProductID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                sqlCmd.Parameters.AddWithValue("@Brand", productdetails.Brand);
                sqlCmd.Parameters.AddWithValue("@ModelName", productdetails.ModelName);
                sqlCmd.Parameters.AddWithValue("@Price", productdetails.Price);
                sqlCmd.Parameters.AddWithValue("@Size", productdetails.Size);
                sqlCmd.Parameters.AddWithValue("@ExpectedDelivery", productdetails.ExpectedDelivery);
                sqlCmd.Parameters.AddWithValue("@COD", productdetails.COD);
                sqlCmd.Parameters.AddWithValue("@VendorCost", productdetails.VendorCost);
                sqlCmd.Parameters.AddWithValue("@SellingPrice", productdetails.SellingPrice);
                sqlCmd.Parameters.AddWithValue("@Seller", productdetails.Seller);
                sqlCmd.Parameters.AddWithValue("@ImportedFrom", productdetails.ImportedFrom);
                sqlCmd.Parameters.AddWithValue("@WarehouseLocation", productdetails.WarehouseLocation);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ShowInventory", "Product");
        }

        public ActionResult DeleteInventoryItem(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spDeleteInventoryItem";
                cmd.Connection = sqlCon;
                cmd.Parameters.AddWithValue("@ProductID", id);

                cmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            return RedirectToAction("PartialTable");
        }

        public ActionResult PartialTable()
        {
            DataTable dtblProduct = new DataTable();

            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product", sqlCon);
                    sqlDa.Fill(dtblProduct);
                }
                return PartialView(dtblProduct);
            }
        }


        public ActionResult PlaceOrder(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Cart Where CartID=@CartID";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("CartId", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productdetails.CartID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                //productModel.ProductName = dtblProduct.Rows[0][2].ToString();
                //productModel.Price = Convert.ToInt32(dtblProduct.Rows[0][3]);

                productdetails.Quantity = Convert.ToInt32(dtblProduct.Rows[0][2].ToString());
               
                return View(productdetails);

            }
            else
                return View();
        }

        [HttpPost]

        public ActionResult PlaceOrder(ProductDetails productdetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "Insert Into OrderDetails Values(@ProductId,@Quantity, @CustomerName, @ContactNumber, @PaymentMethod, @ShippingAddress)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productdetails.ProductID);
                sqlCmd.Parameters.AddWithValue("@Quantity", productdetails.Quantity);
                sqlCmd.Parameters.AddWithValue("@CustomerName", productdetails.CustomerName);
                sqlCmd.Parameters.AddWithValue("@ContactNumber", productdetails.ContactNumber);
                sqlCmd.Parameters.AddWithValue("@PaymentMethod", productdetails.PaymentMethod);
                sqlCmd.Parameters.AddWithValue("@ShippingAddress", productdetails.ShippingAddress);

                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("ShowOrders");

        }


        public ActionResult ShowOrders()
        {
            DataTable dtblProduct = new DataTable();

            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spShowOrders", sqlCon);
                    sqlDa.Fill(dtblProduct);
                }
                return View(dtblProduct);
            }
        }

        public ActionResult OrderSummary(int id)
        {
            ProductDetails productdetails = new ProductDetails();
            DataTable dtblProduct = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spOrderSummary @OrderID", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("OrderID", id);
                sqlDa.Fill(dtblProduct);
            }
            if (dtblProduct.Rows.Count == 1)
            {
                productdetails.OrderID = Convert.ToInt32(dtblProduct.Rows[0][0].ToString());
                productdetails.ProductID = Convert.ToInt32(dtblProduct.Rows[0][1].ToString());
                productdetails.Brand = dtblProduct.Rows[0][2].ToString();
                productdetails.Price = Convert.ToInt32(dtblProduct.Rows[0][3].ToString());
                productdetails.Quantity = Convert.ToInt32(dtblProduct.Rows[0][4].ToString());
                productdetails.ModelName = dtblProduct.Rows[0][5].ToString();
                productdetails.Size = dtblProduct.Rows[0][6].ToString();
                productdetails.CustomerName = dtblProduct.Rows[0][7].ToString();
                productdetails.ContactNumber = dtblProduct.Rows[0][8].ToString();
                productdetails.PaymentMethod = dtblProduct.Rows[0][9].ToString();
                productdetails.ShippingAddress = dtblProduct.Rows[0][10].ToString();
                productdetails.TotalPrice = Convert.ToInt32((Convert.ToInt32(dtblProduct.Rows[0][3].ToString())) * (Convert.ToInt32(dtblProduct.Rows[0][4].ToString())));
                return View(productdetails);

            }
            else
                return View();
        }

        public ActionResult HomeView()
        {

            DataTable dtblProduct = new DataTable();

            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("Execute spIndex", sqlCon);
                    sqlDa.Fill(dtblProduct);
                }
                return View(dtblProduct);
            }
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult AddAdmin()
        {
            return View(new AdminDetails());
        }

        public JsonResult CheckEmailAvailability(string adminemail)
       {
            int TotalCount;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spCheckEmailAvailability";
                cmd.Connection = sqlCon;
                cmd.Parameters.AddWithValue("@AdminEmail", adminemail);
                cmd.Parameters.Add("@TotalCount", SqlDbType.Int);
                cmd.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                TotalCount = Convert.ToInt32(cmd.Parameters["@TotalCount"].Value);
                sqlCon.Close();

            }

            
                return Json( Convert.ToBoolean(TotalCount), JsonRequestBehavior.AllowGet );
            
          
        }

        [HttpPost]
        public ActionResult AddAdmin(AdminDetails admindetails)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO AdminDetails VALUES (@AdminEmail,@AdminPassword)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@AdminEmail", admindetails.AdminEmail);
                sqlCmd.Parameters.AddWithValue("@AdminPassword", admindetails.AdminPassword);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("AdminLogin");
        }
    }
        

}