using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
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
    [Authorize(Roles = Roll.Admin)]

    public class ChalaniController : Controller
    {
        public readonly IunitOfwork _db;
        public readonly UserManager<IdentityUser> _userManager;
        public ChalaniController(IunitOfwork db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Chalani";
            return View();
        }

        //TO get the data from Chalani table
        public JsonResult getAllChalani()
        {
            List<ChalaniClass> data  = _db.Chalani.getAll(prop: "Customer").ToList();
            return Json(data);
        }

        //To delete data from Chalani table
        public JsonResult DeleteAllChalan(int id)
        {
            try
            {
                ChalaniClass one = _db.Chalani.GetOne(u => u.Id == id, null);
                if (one != null)
                {
                    List<ChalanihasProductClass> ChasP = _db.Chalanihasproduct.getSpecifics(u => u.Chalani_id == one.Id, null).ToList();
                    //ChalaniController hi = new ChalaniController(_db, _userManager);
                    foreach (var data in ChasP)
                    {
                        JsonResult result = DeleteChalani(data.Id);//this is a IAction function which is below will return json data after deleting all its corresponding chalanihasproduct data
                        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        JObject jsonObject = JObject.Parse(jsonData);
                        JObject Value = JObject.Parse(jsonObject.GetValue("Value").ToString());
                        bool Tsuccess= (bool)Value.GetValue("success");
                        if(!Tsuccess)//checking if the DeleteChalani function worked well
                        {
                            throw new Exception("Something went wrong");
                        }
                    }
                    _db.Chalani.Delete(one);
                    _db.Save();
                    return Json(new
                    {
                        success=true,
                        message="The value was deleted"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "No data available"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }

        [HttpPost]//this is to addd chalanihasproduct data
        public IActionResult AddChalani(ChalaniClass obj)
        {
            ViewBag.ActivePage = "Chalani";
            if (ModelState.IsValid) 
            {
                List<ChalanihasProductClass> data;
                if (obj.Id == 0) //If new data is being changed
                {
                    string User_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    data = _db.Chalanihasproduct.getSpecifics(u => u.User_id == User_id && u.Chalani_id == null, null).ToList();
                    _db.Chalani.Add(obj);
                }
                else//if old data to update is being changed
                {
                    data= _db.Chalanihasproduct.getSpecifics(u=>u.Chalani_id==obj.Id, null).ToList();
                    _db.Chalani.Update(obj);
                }
                _db.Save();
                
                foreach(var item in data) 
                {
                    item.Chalani_id = obj.Id;
                    List<StockClass> Stock= _db.Stock.getSpecifics(u=>u.chalanihasProduct_id==item.Id, null).ToList();
                    foreach(var lili in Stock)
                    {
                        lili.Customer_id= obj.Customer_id;
                        _db.Stock.Update(lili);//updating all the stock present in corresponding chalani has product 
                    }
                    _db.Chalanihasproduct.Update(item);
                }
                _db.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Modal state is not Valid";
                return RedirectToAction("CreateChalani");
            }
        }
        //get method to get correspoding chalani has product data present in current chalani
        public JsonResult getChalani(int? id)
        {
            if(id==0)//will return those chalani has product data in which no chalani_id foreign key is provided
            {
                string? user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<ChalanihasProductClass> data = _db.Chalanihasproduct.getSpecifics(u => u.User_id == user_id && u.Chalani_id == null, prop: "Product").ToList();
                return Json(data);
            }
            else//when an edit button is clicked then all those chalani has product is 
            {
                List<ChalanihasProductClass> data = _db.Chalanihasproduct.getSpecifics(u => u.Chalani_id == id, prop: "Product").ToList();
                return Json(data);
            }
            
        }
        public IActionResult CreateChalani(int? id)
        {
            ViewBag.ActivePage = "Chalani";
            IEnumerable<SelectListItem> CustomerName = _db.customer.getAll(null).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            IEnumerable<SelectListItem> CustomerNumber = _db.customer.getAll(null).Select(u => new SelectListItem
            {
                Text = u.PhoneNumber.ToString(),
                Value = u.Id.ToString(),
            });
            IEnumerable<SelectListItem> ProductName = _db.Product.getAll(null).Select(u => new SelectListItem
            {
                Text = u.Name+" "+u.Modal,
                Value = u.Id.ToString(),
            });
            ViewBag.CustomerName = CustomerName;
            ViewBag.CustomerNumber = CustomerNumber;
            ViewBag.ProductName = ProductName;
            ChalaniClass value = _db.Chalani.GetOne(u => u.Id == id, null);

            if(value == null) 
            {
                ChalaniClass lili = new ChalaniClass();
                lili.Id = 0;
                return View(lili);
            }
            //else if(value.BillCreated=="Y")
            //{
            //    return RedirectToAction("Index");
            //}
            else 
            { 
                return View(value);

            }
        }



        public JsonResult DeleteChalani(int id)
        {
            ChalanihasProductClass to_delete = _db.Chalanihasproduct.GetOne(u => u.Id == id, null);
            if (to_delete != null) 
            {
                List<StockClass> man= _db.Stock.getSpecifics(u=>u.chalanihasProduct_id==id, null).ToList();
                foreach(var data in man)
                {
                    data.InStock="Y";
                    data.Customer_id = null;
                    data.chalanihasProduct_id = null;
                    _db.Stock.Update(data);
                }
                _db.Chalanihasproduct.Delete(to_delete);
                _db.Save();
                return Json(new 
                {
                    success = true,
                    message = "Value Deleted"
                });
            }
            else
            {
                return Json(new 
                {
                    success=false,
                    message="Given value not found"
                });
            }
        }


        [HttpPost]
        public IActionResult AddChalaniOne(ChalanihasProductVM obj)
        {
            obj.Class.User_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success1;
            string message1;
            if (ModelState.IsValid)
            {
                try
                {
                    List<string?> serial_no = _db.Stock.getSpecifics(u => u.serial_number != null, null).Select(u => u.serial_number).ToList();
                    if (obj.Serial_no.Select(u => u.Value).Contains(null))
                    {
                        throw new Exception("No Serial number is provided");
                    }
                    if (obj.Serial_no.Select(u => u.Value).Distinct().Count() != obj.Serial_no.Count)
                    {
                        throw new Exception("Serial no not unique");
                    }


                    if (obj.Class.Id == 0)
                    {
                        int Instock_count = _db.Stock.getSpecifics(u => u.InStock == "Y" && u.Product_id == obj.Class.product_id &&
                        u.chalanihasProduct_id == null, null).Count();

                        if (Instock_count < obj.Class.Quantity)
                        {
                            throw new Exception("Not Enough Item avaibale in Stock");
                        }
                        foreach (var check in obj.Serial_no)
                        {
                            string check_serial = check.Value;
                            if (check_serial != _db.Stock.GetOne(u => u.Id == check.Id, null).serial_number)
                            {
                                if (serial_no.Contains(check_serial))
                                {
                                    throw new Exception("Serial_no error not Unique");
                                }
                            }

                        }
                        if(obj.chalani_no!=0)
                        {
                            obj.Class.Chalani_id = obj.chalani_no;
                        }
                        _db.Chalanihasproduct.Add(obj.Class);
                        _db.Save();
                        foreach (var item in obj.Serial_no)
                        {
                            StockClass man = _db.Stock.GetOne(u => u.Id == item.Id, null);
                            if (man.InStock == "N")
                            {
                                _db.Chalanihasproduct.Delete(obj.Class);
                                _db.Save();
                                throw new Exception("Given Item of product no " + item.Id + " has alread been out of stock enter again");
                            }
                            man.chalanihasProduct_id = obj.Class.Id;
                            man.serial_number = obj.Serial_no.FirstOrDefault(u => u.Id == item.Id).Value;
                            man.InStock = "N";
                            _db.Stock.Update(man);
                        }
                        _db.Save();
                        success1 = true;
                        message1 = "Value Added";
                    }
                    else
                    {
                        List<StockClass> man = _db.Stock.getSpecifics(u => u.chalanihasProduct_id == obj.Class.Id, null).ToList();
                        foreach(var item in man) 
                        {
                            item.chalanihasProduct_id = null;
                            item.InStock="Y";
                            _db.Stock.Update(item);                       
                        }
                        _db.Save();
                        foreach(var item in obj.Serial_no)
                        {
                            StockClass stock = _db.Stock.GetOne(u => u.Id == item.Id, null);
                            stock.serial_number = item.Value;
                            stock.chalanihasProduct_id = obj.Class.Id;
                            stock.InStock = "N";
                            _db.Stock.Update(stock);
                        }
                        _db.Save();
                        if(obj.chalani_no!=0)
                        {
                            obj.Class.Chalani_id= obj.chalani_no;
                        }
                        _db.Chalanihasproduct.Update(obj.Class);
                        success1 = true;
                        message1 = "Value Updated";
                    }
                    _db.Save();
                    return Json(new
                    {
                        success = success1,
                        message = message1
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
                return Json(
                    new
                    {
                        success = false,
                        message = "Modal state not valid"
                    });
            }
        }
    }
}
