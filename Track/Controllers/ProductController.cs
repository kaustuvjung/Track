using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Http;
using Track.Data;
using Track.Models;
using Track.PreData;
using Track.Repository.Irepository;

namespace Track.Controllers
{
   // [Authorize(Roles = Roll.Admin)]

    public class ProductController : Controller
    {
        private readonly IunitOfwork _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IunitOfwork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
      

        public JsonResult GetAll()
        {
            List<ProductClass> Products = _db.Product.getAll(prop: null).ToList();
            foreach(var data in Products)
            {
                data.In_stock = _db.Stock.getSpecifics(u => u.Product_id == data.Id && u.InStock=="Y", null).Count();
                _db.Product.Update(data);
            }
            _db.Save();
            return new JsonResult(Products);
        }

        public IActionResult OnRepair()
        {
            ViewBag.ActivePage = "Onrepair";
            return View();
        }
        public JsonResult Damaged()
        {
            List<StockClass> myList = _db.Stock.getSpecifics(u => u.isDamaged != null, null).ToList();
            return Json(myList);
        }
        public JsonResult UpDamage(int? id, string? message,int d)
        {
            try
            {
                StockClass man = _db.Stock.GetOne(u => u.Id == id, null);
                if(d==1)
                {
                    man.isDamaged = null;
                }
                man.Damaged_why = message;
                _db.Stock.Update(man);
                _db.Save();
                return Json(new {
                    success = true,
                });
            }
            catch(Exception ex) 
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }
        public IActionResult Index()
        {
            ViewBag.ActivePage = "Product";
            IEnumerable<SelectListItem> CompanyName = Company.names.Select(u => new SelectListItem
            {
                Text = u,
                Value = u
            });

            IEnumerable<SelectListItem> CatagoryType = Company.type.Select(u => new SelectListItem { Text = u, Value = u });
            ViewBag.CompanyName = CompanyName;
            ViewBag.CatagoryType = CatagoryType;
            return View();
        }
        public JsonResult upStock([FromUri] int[] ID, string? message)
        {
            try
            {
                foreach (var id in ID)
                {
                    StockClass one = _db.Stock.GetOne(u => u.Id == id, null);
                    one.isDamaged = "N";
                    one.Damaged_why = message;
                    if (one.chalanihasProduct_id != null)
                    {
                        int iId = Convert.ToInt32(one.chalanihasProduct_id);
                        one.chalanihasProduct_id = null;
                        StockClass two = _db.Stock.GetOne(u => u.InStock == "Y" && u.Product_id == one.Product_id && u.isDamaged == null && u.chalanihasProduct_id == null, null);
                        two.chalanihasProduct_id = iId;
                        two.InStock= "N";
                        _db.Stock.Update(two);
                    }
                    _db.Stock.Update(one);
                }
                _db.Save();
                return Json(new
                {
                    success = true,
                });
            }
            catch (Exception ex) 
            {
                return Json(new { 
                    success=false,
                    message=ex.Message,
                });
            }
            
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Index(ProductClass obj, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;//for www root folder path

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string Productpath = Path.Combine(wwwRootPath, @"Images\Product");
                if (!string.IsNullOrEmpty(obj.ImgUrl))
                {
                    //delete old image 
                    var oldimage = Path.Combine(wwwRootPath, obj.ImgUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldimage))
                    {
                        System.IO.File.Delete(oldimage);
                    }
                }
                using (var filestream = new FileStream(Path.Combine(Productpath, fileName), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }

                obj.ImgUrl= @"\Images\Product\" + fileName;
            }

            if (ModelState.IsValid)
            {


                _db.Product.Add(obj);
                _db.Save();
                return RedirectToAction("Index");
                
            }
            else
            {
                return View(obj);
            }
        }

        public IActionResult Delete(int? id)
        {
            ProductClass To_delete = _db.Product.GetOne(u => u.Id == id, prop:null);
            try
            {
                if (To_delete != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;//for www root folder path
                    string Productpath = Path.Combine(wwwRootPath, @"Images\Product");
                    if (!string.IsNullOrEmpty(To_delete.ImgUrl))
                    {
                        //delete old image 
                        var oldimage = Path.Combine(wwwRootPath, To_delete.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimage))
                        {
                            System.IO.File.Delete(oldimage);
                        }
                    }



                    _db.Product.Delete(To_delete);
                    _db.Save();
                    return Json(new
                    {
                        success = true,
                        message = To_delete.Id + " id number product Deleted"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "There was an error deleting"
                    });
                }
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

    }
}
