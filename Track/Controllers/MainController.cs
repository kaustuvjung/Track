using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Track.Data;
using Track.Models;
using Track.PreData;
using Track.Repository.Irepository;
using Track.ViewModel;

namespace Track.Controllers
{
    //[Authorize(Roles = Roll.Admin)]


    public class MainController : Controller
    {
        private readonly IunitOfwork _db;
        private readonly Applicationdbcontext _context;
        public MainController(IunitOfwork db, Applicationdbcontext context)
        {
            _db = db;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetVendor()
        {
            List<VendorClass> data =_db.Vendor.getAll(prop:null).ToList();
            return new JsonResult(data);
        }

        public IActionResult Addvendor() 
        {
            ViewBag.ActivePage = "Vendor";
            return View();
        }


        [HttpPost]
        public IActionResult Addvendor(VendorClass obj)
        {
            if(ModelState.IsValid) 
            {
                if(obj.Id==0)
                {
                    _db.Vendor.Add(obj);
                    _db.Save();
                    return Json(new
                    {
                        success = true,
                        message = "New Vendor Added"
                    });
                }
                else

                {
                    VendorClass vendor = _db.Vendor.GetOne(u => u.Id == obj.Id, null);
                    vendor.Id = obj.Id;
                    vendor.PhoneNumber = obj.PhoneNumber;
                    vendor.Description = obj.Description;
                    _db.Vendor.Update(vendor);
                    _db.Save();
                    return Json(new
                    {
                        success = true,
                        message = "Vendor Updated"
                    });
                }
                
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong"
                });
            }
        }

        public JsonResult getProvince()
        {
            List<ProvinceClass> data= _context.Province.ToList();
            return new JsonResult(data);
        }

        public JsonResult getLocalBody(int? id)
        {
            List<LocalBodyClass> data = _context.LocalBody.Where(u => u.DistrictId == id).ToList();
            return new JsonResult(data);
        }
        public JsonResult getDistrict(int? id)
        {
            List<DistrictClass> data= _context.District.Where(u=>u.ProvinceId==id).ToList();
            return new JsonResult(data);
        }

        public JsonResult getAllcutomer()
        {
            List<CustomerClass> list = _db.customer.getAll(prop: "Province,District,LocalBody").ToList();
            return new JsonResult(list);
        }
        public IActionResult CustomerInfo()
        {
            ViewBag.ActivePage = "Customer";
            CustomerClass customer = new CustomerClass();
            return View(customer);
        }
        
        [HttpPost]
        public IActionResult CustomerInfo(CustomerClass obj)
        {
            if(ModelState.IsValid)
            {
                try
                {


                    if (obj.Id == 0)
                    {
                        _db.customer.Add(obj);
                        _db.Save();
                        return Json(new
                        {
                            success = true,
                            message = "Data Added Successfully",
                            type = "Value Added"
                        });
                    }
                    else
                    {
                        _db.customer.Update(obj);
                        _db.Save();
                        return Json(new
                        {
                            success = true,
                            message = "Data Edited Successfully",
                            type = "Value Edited"
                        });
                    }
                }
                catch (Exception ex) 
                {
                    return Json(new
                    {
                        success=false,
                        message=ex.Message,
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success=false,
                    message="some error in model state"
                });
            }
        }

        public IActionResult DeleteVendor(int?id)
        {
            VendorClass vendorClass = _db.Vendor.GetOne(u => u.Id == id, null);
            if(vendorClass != null)
            {
                try
                {
                    _db.Vendor.Delete(vendorClass);
                    _db.Save();
                    return Json(new
                    {
                        success=true,
                        message="Value successfully Deleted"
                    });
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
            else
            {
                return Json(new
                {
                    success = false,
                    message = "No value found"
                });
            }
        }
        public IActionResult Delete(int? id) 
        {
            CustomerClass man = _db.customer.GetOne(u => u.Id == id,prop:null);
            try
            {            
                if(man!=null) 
                {
                    List<StockClass> Sman = _db.Stock.getSpecifics(u => u.Customer_id == id, prop: null).ToList();
                    foreach(var a in Sman)
                    {
                        a.Customer_id = null;
                        _db.Stock.Update(a);
                    }
                    _db.Save();
                    _db.customer.Delete(man);
                    _db.Save();
                    return Json(new
                    {
                        Success = true,
                        message = "Customer of id " + man.Id + " is Deleted"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        message = "Customer of given id not found"
                    });
                }
            }
            catch (Exception ex) 
            {

                return Json(new
                {
                    Success = false,
                    message = ex.Message,
                });
            }
        }
    }
}
