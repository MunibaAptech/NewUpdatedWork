using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace RegistrationForm.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Registration()
        {

            WebAppEntities4 db = new WebAppEntities4();

            var cityList = db.tbl_City.ToList();
            ViewBag.CityList = new SelectList( cityList, "CityId", "CityName");



            return View();
        }
        [HttpPost]
        public ActionResult Registration(tbl_UserRegistration user, HttpPostedFileBase uploadImg)
        {
            WebAppEntities4 db = new WebAppEntities4();


            string fileName = Path.GetFileName(uploadImg.FileName);
            ViewBag.msg = fileName;
            uploadImg.SaveAs(Server.MapPath("~/Images/" + fileName));
            user.Image = fileName;

            db.tbl_UserRegistration.Add(user);
            db.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("ListView");
        }

        public ActionResult ListView()
        {
            WebAppEntities4 db = new WebAppEntities4();
            var user = db.tbl_UserRegistration.ToList();
            return View(user);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tbl_UserRegistration objUser)
        {
            WebAppEntities4 db = new WebAppEntities4();
            var user = db.tbl_UserRegistration.Where(x => x.UserName == objUser.UserName && x.Password == objUser.Password).Count();

            if(user > 0)
            {
                return RedirectToAction("ListView");
            }
            else
            {
                return View();
            }
                
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            WebAppEntities4 db = new WebAppEntities4();
            var cityList = db.tbl_City.ToList();
            ViewBag.City = new SelectList(cityList, "CityId", "CityName");
            var userId = db.tbl_UserRegistration.Find(id);

            if(userId.Image == null)
            {
                userId.Image = "~/Images/No_Image_Available.jpg";
            }
            return View(userId);
        }

        [HttpPost]
        public ActionResult Edit(tbl_UserRegistration EditReg, HttpPostedFileBase uploadImg)
        {
            if(EditReg.Image != null)
            {
                string fileName = Path.GetFileName(uploadImg.FileName);
                ViewBag.msg = fileName;
                uploadImg.SaveAs(Server.MapPath("~/Images/" + fileName));
                EditReg.Image = fileName;
            }
            

            WebAppEntities4 db = new WebAppEntities4();
            db.Entry(EditReg).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit");
          
        }


    }
}