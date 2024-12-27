using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Track.Models;
using Track.PreData;
using Track.Repository;
using Track.Repository.Irepository;
using Track.ViewModel;

namespace Track.Controllers
{
    [Authorize(Roles = Roll.Admin)]
    public class BillController : Controller
    {
        private readonly IunitOfwork _db;
        private readonly UserManager<IdentityUser> _userManager;
        public BillController(IunitOfwork db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult CreateBill(int? id)
        {
            ViewBag.ActivePage = "Bill";
            IEnumerable<SelectListItem> CustomerName = _db.customer.getAll(null).Select(u=> new SelectListItem
            {
                Text = u.Name,
                Value= u.Id.ToString(),
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
            ViewBag.CustomerName=CustomerName;
            ViewBag.CustomerNumber=CustomerNumber;
            ViewBag.ProductName=ProductName;
            BillClass myBill = _db.Bill.GetOne(u => u.Id == id, null);
            if(myBill==null)
            {
                return View(new BillClass());
            }
            return View(myBill);
        }

        public JsonResult getCombill(int? id)
        {
            List<BillhasProductClass> data= _db.Billhasproduct.getSpecifics(u=>u.Bill_id==id, prop:"Product").ToList();
            return Json(data);
        }
        public JsonResult getCom(int? id)
        {
            if(id==null)
            {
                string? user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<BillhasProductClass> data = _db.Billhasproduct.getSpecifics(u => u.Bill_id == null && u.User_id == user_id, prop: "Product").ToList();
                return new JsonResult(data);
            }
            else
            {
                List<BillhasProductClass> data = _db.Billhasproduct.getSpecifics(u => u.Bill_id == id, prop: "Product").ToList();
                return Json(data);
            }
        }

        public JsonResult stockid(int? id)
        {
            List<StockClass> data = _db.Stock.getSpecifics(u=>u.billhasProduct_id==id,prop:null).ToList();
            return Json(data);
        }

        public IActionResult Deletebill(int id)
        {
            BillClass one = _db.Bill.GetOne(u => u.Id == id, null);
            List<BillhasProductClass> two= _db.Billhasproduct.getSpecifics(u=>u.Bill_id== id, prop:null).ToList();
            if (one != null)
            {
                try
                {
                    _db.Billhasproduct.DeleteMost(two);
                    _db.Bill.Delete(one);
                    _db.Save();
                    return Json(new
                    {
                        success = true,
                        message = "Value Deleted Successfully"
                    });
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Chalani Record is Still present"
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
                    message = "No value found"
                });
            }
        }
        public IActionResult DeleteCom(int id)
        {
            BillhasProductClass one = _db.Billhasproduct.GetOne(u => u.Id == id, null);
            List<StockClass> To_remove = _db.Stock.getSpecifics(u => u.billhasProduct_id == one.Id, null).ToList();
            if(one != null) 
            {
                foreach(var obj in  To_remove)
                {
                    obj.InStock = "Y";
                    _db.Stock.Update(obj);
                }
                _db.Billhasproduct.Delete(one);
                _db.Save();
                return Json(new
                {
                    success=true,
                    message="Value Deleted Successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success=false,
                    message="Product not found"
                });
            }
        }

        public IActionResult Check(int? id)
        {
            ViewBag.ActivePage = "Bill";
            IEnumerable<SelectListItem> userList = _userManager.Users.Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.Id.ToString()
            });
            BillClass one = _db.Bill.GetOne(u=>u.Id == id, prop: "Customer");
            ViewBag.userList = userList;
            return View(one);
        }
        public JsonResult AllBill()
        {
            List<BillClass> data = _db.Bill.getAll(prop: "Customer").ToList();
            return new JsonResult(data);
        }

        public JsonResult lokPay(int? id)
        {
            var list = _db.Payment.getSpecifics(u => u.Bill_id == id, null).Select(u=> new {
                id=u.Id,
                date= u.PDate,
                method=u.Method,
                amount=u.Amount,
                commission=Convert.ToString(u.commission),
                commissionper=Convert.ToString(u.commissionper),
                commissino_to = Convert.ToString(u.Commissino_to)
            }).ToList();
            return Json(list);
        }

        public IActionResult DeletePayment(int? id)
        {
            PaymentClass one = _db.Payment.GetOne(u => u.Id == id, null);
            if( one!=null)
            {
                BillClass billone = _db.Bill.GetOne(u => u.Id == one.Bill_id, null);
                billone.payment -= one.Amount;
                _db.Payment.Delete(one);
                _db.Bill.Update(billone);               
                _db.Save();
                return Json(new
                {
                    success = true,
                    message = "Payment record deleted",
                    pay = billone.payment
                });
            }
            else
            {
                return Json(new {
                    success=false,
                    message="No payment found"
                });
            }
        }
        public IActionResult Addbill()
        {
            ViewBag.ActivePage = "Bill";
            return View();
        }

        [HttpPost]
        public IActionResult Addbill(BillClass bill)
        {
            ViewBag.ActivePage = "Bill";
            try
            {
                if (ModelState.IsValid)
                {
                    BillClass billone = _db.Bill.GetOne(u => u.Id==bill.Id, null);
                    List<BillhasProductClass> one;
                    if (billone==null) 
                    {
                        _db.Bill.Add(bill);
                         one= _db.Billhasproduct.getSpecifics(u => u.Bill_id == null, prop: null).ToList();
                    }
                    else
                    {
                        _db.Bill.Update(bill);
                        one=_db.Billhasproduct.getSpecifics(u=>u.Bill_id==bill.Id, prop: null).ToList();
                    }
                    _db.Save();

                   
                    foreach (var data in one)
                    {
                        data.Bill_id = bill.Id;
                        _db.Billhasproduct.Update(data);
                        List<StockClass> To_update = _db.Stock.getSpecifics(u => u.billhasProduct_id == data.Id, null).ToList();
                        foreach (var lita in To_update)
                        {
                            lita.Customer_id = bill.Customer_id;
                            lita.InStock = "N";
                            _db.Stock.Update(lita);
                        }
                    }
                    _db.Save();
                    return View();
                }
                else
                {
                    TempData["error"] = "The bill Description was not valid";
                }
            } catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("CreateBill");
        }


        [HttpPost]
        public IActionResult Paid(PaymentClass obj, string hiDate) 
        {
            obj.PDate = DateTime.Parse(hiDate);
            ModelState.Remove("obj.PDate");
            if(ModelState.IsValid)
            {
                bool Sucess;
                string mesage;
                int paymentamt=0;
                try
                {
                    BillClass bill = _db.Bill.GetOne(u => u.Id == obj.Bill_id, prop: null);
                    
                    if (obj.Id == 0)
                    {
                        if (bill == null)
                        {
                            throw new Exception("No bill found");
                        }
                        bill.payment = Convert.ToInt32(bill.payment) + obj.Amount;
                        obj.commissionper = (obj.commission/obj.Amount)*100;
                        _db.Bill.Update(bill);
                        _db.Payment.Add(obj);
                        _db.Save();
                        Sucess=true;
                        mesage = "Payment Successfully Added";
                        paymentamt = Convert.ToInt32(bill.payment);
                    }
                    else
                    {
                        PaymentClass wow= _db.Payment.GetOne(u=>u.Id== obj.Id, prop: null);
                        if( wow == null) 
                        {
                            throw new Exception("NO payment of the given Id found");
                        }
                        if (bill == null)
                        {
                            throw new Exception("No bill found");
                        }
                        bill.payment=bill.payment-wow.Amount+obj.Amount;
                        _db.Bill.Update(bill);
                        _db.Payment.Update(obj);
                        _db.Save();
                        Sucess= true;
                        mesage = "Payment Succesfully Updated";
                        paymentamt = Convert.ToInt32(bill.payment);
                    }

                } 
                catch (Exception ex)
                {
                    Sucess = false;
                    mesage=ex.Message;
                    
                }
                return Json(new 
                    {
                        success= Sucess,
                        message= mesage,
                        pay=paymentamt,
                    });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Modal State not valid"
                });
            }
        }


        //to add serial number in the form
        public JsonResult SerialId(int id, int Qua, int pro, int? f)
        {
            
            List<Text_value> ids= _db.Stock.getSpecifics(u => u.InStock == "Y" && u.Product_id==pro && u.isDamaged==null, null).Select(u => new Text_value
            {
                Id = u.Id,
                Value = Convert.ToString(u.serial_number)
            }).ToList();
            int litCount= ids.Count;
            if (id==0)
            {
                if(Qua>litCount)
                {
                    return Json(new
                    {
                        success=false,
                        message="Not Enough Stock Available"
                    });
                }
                else
                {
                    return Json(new
                    {
                        value=ids.Take(Qua),
                        success=true
                    });
                }
            }
            else 
            {
                List<Text_value> wEdit;

                if (f == null)
                {
                    wEdit = _db.Stock.getSpecifics(u => u.billhasProduct_id == id, null).Select(u => new Text_value
                    {
                        Id = u.Id,
                        Value = u.serial_number.ToString()
                    }).ToList();
                }
                else
                {
                    wEdit = _db.Stock.getSpecifics(u => u.chalanihasProduct_id == id, null).Select(u => new Text_value
                    {
                        Id = u.Id,
                        Value = u.serial_number.ToString()
                    }).ToList();
                }

                if (wEdit.Count>=Qua) 
                {
                    return Json(new
                    {
                        value = wEdit.Take(Qua),
                        success = true
                    });
                } else if((Qua-wEdit.Count)<=litCount) 
                { 
                    wEdit.AddRange(ids.Take(Qua - wEdit.Count));
                    return Json(new
                    {
                        value = wEdit,
                        success = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        success=false,
                        message="Not Enough item in stock"
                    });
                }
            }
        }


        [HttpPost]
        public IActionResult addCom(BillhasProductVM obj)
        {
            obj.Class.total= obj.Class.Rate * obj.Class.Quantity;
            obj.Class.User_id=User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success1;
            string message1;
            if(ModelState.IsValid)
            {
                try
                {
                    List<string?> serial_no = _db.Stock.getSpecifics(u => u.serial_number != null, null).Select(u => u.serial_number).ToList();
                    if(obj.Serial_no.Select(u=>u.Value).Contains(null))
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
                        u.billhasProduct_id ==null, null).Count();
                        
                        if (Instock_count < obj.Class.Quantity)
                        {
                            throw new Exception("Not Enough Item avaibale in Stock");
                        }
                        foreach (var check in obj.Serial_no)
                        {
                            string check_serial =check.Value;
                            if(check_serial!=_db.Stock.GetOne(u=>u.Id==check.Id, null).serial_number)
                            {
                                if (serial_no.Contains(check_serial))
                                {
                                    throw new Exception("Serial_no error not Unique");
                                }
                            }
                            
                        }
                        //List<StockClass> man = _db.Stock.getSpecifics(u => u.billhasProduct_id == null
                        //&& u.Product_id == obj.Class.product_id && u.InStock=="Y", null).Take(obj.Class.Quantity).ToList();
                        if(obj.Class.Bill_id==0)
                        {
                            obj.Class.Bill_id = null;
                        }
                        _db.Billhasproduct.Add(obj.Class);
                        _db.Save();
                        foreach (var item in obj.Serial_no)
                        {
                            StockClass man = _db.Stock.GetOne(u => u.Id == item.Id, null);
                            if(man.InStock=="N")
                            {
                                _db.Billhasproduct.Delete(obj.Class);
                                _db.Save();
                                throw new Exception("Given Item of product no " + item.Id + " has alread been out of stock enter again");
                            }
                            man.billhasProduct_id = obj.Class.Id;
                            man.serial_number = obj.Serial_no.FirstOrDefault(u=>u.Id==item.Id).Value;
                            man.InStock= "N";
                            _db.Stock.Update(man);
                        }
                        _db.Save();
                        success1 = true;
                        message1 = "Value Added";
                    }
                    else
                    {
                        List<StockClass> man = _db.Stock.getSpecifics(u=>u.billhasProduct_id ==obj.Class.Id, null).ToList();
                        int B_update = man.Count;

                        if(B_update>=obj.Class.Quantity)
                        {
                            int check = 0;
                            foreach(var  item in man) 
                            {
                                if(check<obj.Class.Quantity && obj.Serial_no.Select(u=>u.Id).Contains(item.Id))
                                {

                                    item.serial_number = obj.Serial_no[check].Value.ToString();
                                    item.InStock = "N";
                                    check++;
                                }
                                else
                                {
                                    item.serial_number = null;
                                    item.billhasProduct_id = null;
                                    item.InStock = "Y";
                                }
                                _db.Stock.Update(item);
                            }
                        }
                        else 
                        { 
                            int additional_required= obj.Class.Quantity-B_update;
                            int Instock_count = _db.Stock.getSpecifics(u => u.InStock == "Y" && u.Product_id == obj.Class.product_id && u.billhasProduct_id == null, null).Count();
                            if (Instock_count < additional_required)
                            {
                                throw new Exception("Not Enough Item avaibale in Stock");
                            }
                            //List<StockClass> newStock = _db.Stock.getSpecifics(u => u.billhasProduct_id == null
                            //&& u.Product_id == obj.Class.product_id && u.InStock == "Y", null).Take(additional_required).ToList();
                            //man.AddRange(newStock);
                            //var count = 0;
                            //foreach(var item in man)
                            //{
                            //    item.billhasProduct_id = obj.Class.Id;
                            //    item.serial_number = obj.Serial_no[count].Value;
                            //    _db.Stock.Update(item);
                            //    count++;
                            //}
                            foreach (var item in obj.Serial_no)
                            {
                                StockClass man1 = _db.Stock.GetOne(u => u.Id == item.Id, null);
                                if (!(man1.InStock == "Y" || man1.billhasProduct_id==obj.Class.Id))
                                {
                                    throw new Exception("Given Item of product no " + item.Id + " has alread been out of stock enter again");
                                }
                                man1.billhasProduct_id = obj.Class.Id;
                                man1.serial_number = obj.Serial_no.FirstOrDefault(u => u.Id == item.Id).Value;
                                man1.InStock= "N";
                                _db.Stock.Update(man1);
                            }

                        }
                        _db.Billhasproduct.Update(obj.Class);
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
                        success =false,
                        message="Modal state not valid"
                    });
            }
        }
    }
}
