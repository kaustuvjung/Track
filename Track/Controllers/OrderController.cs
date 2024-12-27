using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Track.Models;
using Track.PreData;
using Track.Repository.Irepository;
using Track.ViewModel;

namespace Track.Controllers
{
    [Authorize(Roles = Roll.Admin+","+Roll.client)]

    public class OrderController : Controller
    {
        private readonly IunitOfwork _db;
        private readonly UserManager<IdentityUser> _userManager;
        public OrderController(IunitOfwork db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Order";
            return View();
        }

        public JsonResult GetAll()
        {

            List<OrderClass> order = _db.Order.getAll(prop: "vendor").ToList();
            _db.Save();
            return new JsonResult(order);
        }
        
        public JsonResult Get(int? id) 
        {
            List<StockClass> list = _db.Stock.getSpecifics(u => u.OrderhasProducts.Order_id == id, prop: "Product,Customer,OrderhasProducts").ToList();
            return new JsonResult(list);
        }


        public JsonResult GetMost()
        {
            List<StockClass> list = _db.Stock.getAll(prop: "Product,Customer,OrderhasProducts").ToList();
            return new JsonResult(list);
        }


        public IActionResult newOrder()
        {
            ViewBag.ActivePage = "Order";
            IEnumerable<SelectListItem> Products= _db.Product.getAll(prop:null).Select(u=> new SelectListItem
            {
                Text=u.Id + " " + u.Name,
                Value=u.Id.ToString(),
            });

            IEnumerable<SelectListItem> Vendor = _db.Vendor.getAll(prop: null).Select(u => new SelectListItem
            {
                Text = u.Id + " " + u.Name,
                Value = u.Id.ToString(),
            });
            ViewBag.Products = Products;  
            ViewBag.Vendor = Vendor;
            return View();
        }

        public JsonResult GetBucket()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<OrderhasProducts> allBuckets= _db.Orderhasproduct.getSpecifics(u=>u.User_id == UserId && u.Order_id==null,prop: "Product").ToList();
            return new JsonResult(allBuckets);
        }

        public IActionResult Orderhasproductadd(OrderHasProduct_string obj)
        {
            obj.Order_product.User_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                if (obj.Order_product.Id == 0)
                {
                    _db.Orderhasproduct.Add(obj.Order_product);
                    _db.Save();
                    try
                    {
                        for (int i = 0; i < obj.Order_product.Quantity; i++)
                        {
                            StockClass stockClass = new StockClass();
                            stockClass.Order_Products_id = obj.Order_product.Id;
                            stockClass.serial_number = obj.serial_no[i];
                            stockClass.Product_id = obj.Order_product.Product_id;
                            stockClass.InStock = "T";
                            _db.Stock.Add(stockClass);
                        }
                        _db.Save();
                    }
                    catch (Exception ex)
                    {
                        _db.Orderhasproduct.Delete(obj.Order_product);
                        _db.Save();
                        return Json(new
                        {
                            success = false,
                            message = "Invalid Serial number" + ex.Message
                        });
                    }
                    return Json(new
                    {
                        success = true,
                        message = "Product Added"
                    });
                }
                else
                {
                    OrderhasProducts buck = _db.Orderhasproduct.GetOne(u => u.Id == obj.Order_product.Id, null);
                    _db.Save();
                    if (buck != null)
                    {
                        _db.Orderhasproduct.Update(obj.Order_product);
                        List<StockClass> man = _db.Stock.getSpecifics(u => u.Order_Products_id == buck.Id, null).ToList();

                        int B_update = man.Count;

                        if (B_update >= obj.Order_product.Quantity)
                        {
                            try
                            {
                                int check = 0;
                                foreach (var item in man)
                                {
                                    if (check < obj.Order_product.Quantity)
                                    {

                                        item.serial_number = obj.serial_no[check];
                                        _db.Stock.Update(item);
                                        check++;
                                    }
                                    else
                                    {
                                        _db.Stock.Delete(item);
                                    }

                                }
                                _db.Save();

                            }
                            catch (Exception ex)
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = "Invalid Serial no" + ex.Message,
                                });
                            }

                            return Json(new
                            {
                                success = true,
                                message = "Product Updated"
                            });
                        }
                        else
                        {
                            int additional_required = obj.Order_product.Quantity - B_update;
                            int check = 0;
                            //try
                            {
                                foreach (var item in man)
                                {
                                    if (check < obj.Order_product.Quantity)
                                    {

                                        item.serial_number = obj.serial_no[check];
                                        _db.Stock.Update(item);
                                        check++;
                                    }
                                }
                                for (int i = check; i < obj.Order_product.Quantity; i++)
                                {
                                    StockClass addStock = new StockClass();         
                                    addStock.Order_Products_id = obj.Order_product.Id;
                                    addStock.serial_number = obj.serial_no[i];  
                                    addStock.Product_id = obj.Order_product.Product_id;
                                    addStock.InStock = "T";
                                    _db.Stock.Add(addStock);
                                }
                                _db.Save();
                            }
                            //catch(Exception ex)
                            //{
                            //    return Json(new
                            //    {
                            //        success = false,
                            //        message = "Invalid Sql" + ex.Message
                            //    });
                            //}

                            return Json(new
                            {
                                success = true,
                                message = "Product Updated"
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {   
                            success = false,
                            message = "Product Not found"
                        });
                    }
                }
            }
            else
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Modal State was not valid"
                });
            }
        }

        // delete product indiv
        public IActionResult DeleteOrderhasproduct(int? id)
        {
            OrderhasProducts To_Delete = _db.Orderhasproduct.GetOne(u => u.Id == id, prop: null);
            if (To_Delete != null)
            {
                _db.Orderhasproduct.Delete(To_Delete);
                _db.Save();
                return Json(new
                {
                    success = true,
                    message = To_Delete.Id + " id's record is Deleted"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "There was error while  deleting"
                });
            }
        }
        
        public IActionResult AddSerial(int? id)
        {
            ViewBag.ActivePage = "Stock";
            IEnumerable<SelectListItem> Customer_id = _db.customer.getAll(prop: null).Select(u => new SelectListItem
            {
                Text = u.Id + " " + u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.Customer_id = Customer_id;
            if (id!=null)
            {
                return View(id);
            }
            else
            {
                return View(0);
            }
        }

        [HttpPost]
        public IActionResult AddSerial(IDandSerial data) 
        {
            ViewBag.ActivePage = "Stock";
            if (ModelState.IsValid)
            {
                try
                {
                    if(data.id!=0)
                    {
                        StockClass one = _db.Stock.GetOne(u => u.Id == data.id, prop: null);
                        one.serial_number = data.serial_no;
                        if (data.customer_id != null)
                        {
                            one.Customer_id = data.customer_id;
                            one.InStock = "N";
                        }
                        else
                        {
                            one.InStock = "Y";
                        }
                        one.isDamaged = data.isDamaged;
                        one.Damaged_why = data.Damaged_why;
                        _db.Stock.Update(one);
                        _db.Save();

                        return Json(new
                        {
                            success = true,
                            message = "Value Updated"
                        });
                    }
                    else
                    {
                        StockClass one = new StockClass();
                        one.Id = data.id;
                        one.serial_number= data.serial_no;
                        one.Customer_id= data.customer_id;
                        one.InStock="Y";
                        _db.Stock.Add(one);
                        _db.Save();

                        return Json(new
                        {
                            success = true,
                            message = "Value Added"
                        });
                    }
                   
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success=false,
                        message=ex.Message
                    });
                }

            }
            else
            {
                return Json(new
                {
                    success=false,
                    message="Modal state is Not validate"
                });
            }
        }


        [HttpPost]
        public IActionResult Addstock(Add_stock data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StockClass one = new StockClass();
                    one.Id = data.id;
                  
                    if(data.customer_id==0)
                    {
                        one.Customer_id =null;
                        one.InStock = "Y";

                    }
                    else
                    {
                        one.Customer_id=data.customer_id;
                        one.InStock = "Y";
                    }
                    
                    one.serial_number = data.serial_no;
                    //one.Product_id = _db.Order.GetOne(u => u.Id == data.order_id,prop:null).Product_id;
                    _db.Stock.Add(one);
                    _db.Save();
                    
                    return Json(new
                    {
                        success = true,
                        message = "Serial Number Updated"
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = ex.Message
                    });
                }

            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Modal state is Not validate"
                });
            }
        }


        public IActionResult ViewOrder(int? id)
        {
            ViewBag.ActivePage = "Order";
            OrderClass one = _db.Order.GetOne(u => u.Id == id, prop: "vendor");
            return View(one);
        }

        public JsonResult getVewOrder(int? id) 
        {
            List<OrderhasProducts> list = _db.Orderhasproduct.getSpecifics(u => u.Order_id == id, prop: "Product").ToList();
            return new JsonResult(list);
        }

        public JsonResult SerialId(int? id)
        {
            List<string> myStock = _db.Stock.getSpecifics(u => u.Order_Products_id == id, prop: null).Select(u=>u.serial_number).ToList();
            return Json(new
            {
                success=true,
                value=myStock
            });

        }

        // delete order product
        public IActionResult Deletestock(int? id)
        {
            StockClass To_Delete = _db.Stock.GetOne(u => u.Id == id, prop: null);
            if(To_Delete != null)
            {
                _db.Stock.Delete(To_Delete);
                _db.Save();
                return Json(new
                {
                    success = true,
                    message = To_Delete.Id + " id's recorde is Deleted"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "There was error while deleting"
                });
            }
        }

        public IActionResult Delete(int id)
        {
            OrderClass To_delete = _db.Order.GetOne(u => u.Id == id, prop: null);
            if(To_delete != null) 
            {
                List<OrderhasProducts> myOrder = _db.Orderhasproduct.getSpecifics(u => u.Order_id == id, null).ToList();
                foreach(var data in myOrder)
                {
                    _db.Orderhasproduct.Delete(data);
                }
                _db.Order.Delete(To_delete);
                _db.Save();
                return Json(new
                {
                    success = true,
                    message= To_delete.Id+" id's recorde is Deleted"
                });
            }
            else
            {
                return Json(new
                {
                    success= false,
                    message="There was error while deleting"
                });
            }
        }

        public IActionResult AddnewOrder(OrderClass man)
        {

            if(ModelState.IsValid) 
            {
                try
                {
                    _db.Order.Add(man);
                    _db.Save();
                    string User_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    List<OrderhasProducts> buckets = _db.Orderhasproduct.getSpecifics(u=>u.User_id==User_id && u.Order_id==null,null).ToList();
                 
                    foreach ( var a in buckets)
                    {
                        a.Order_id = man.Id;
                        List<StockClass> man2= _db.Stock.getSpecifics(u=>u.Order_Products_id==a.Id, null).ToList();
                        foreach ( var b in man2)
                        {
                            b.InStock = "Y";
                            _db.Stock.Update(b);
                        }
                        _db.Orderhasproduct.Update(a);
                    }
                    _db.Save();
                    return Json(new
                    {
                        success=true,
                        message="Order Added"
                    });
                }
                catch (Exception ex) 
                {

                    return Json(new
                    {
                        success= false,
                        message=ex.Message
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success=false,
                    message="Invalid Modal State"
                });
            }
        }
    }
}
