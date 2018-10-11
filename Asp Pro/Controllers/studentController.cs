using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp_Pro;
using Asp_Pro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Pro.Controllers
{
    public class studentController : Controller
    {
        StudentRegContext _ORM = null;
        public studentController(StudentRegContext ORM)
        {
            _ORM = ORM;


        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student S)
        {
            _ORM.Add(S);
            _ORM.SaveChanges();
            ModelState.Clear();
            ViewBag.message = "Record Added Successfully";
            return View();
        }
        [HttpGet]
        public IActionResult ViewList()
        {
            IList<Student> list = _ORM.Student.ToList<Student>();
            return View(list);
        }
        [HttpPost]
        public IActionResult ViewList(string name, string sclass, string department)
        {
            IList<Student> list = _ORM.Student.Where(m => m.Name.Contains(name) || m.Class.Contains(sclass) || m.Department.Contains(department)).ToList<Student>();
            return View(list);
        }
        public IActionResult StudentDetail(int Id)
        {
            Student S = _ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return View(S);
        }
        [HttpGet]
        public IActionResult EditStudent(int Id)
        {

            Student S = _ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return View(S);
        }
        [HttpPost]
        public IActionResult EditStudent(Student S)
        {
            _ORM.Student.Update(S);
            _ORM.SaveChanges();
            //Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return RedirectToAction("viewlist");
        }
        public string deleteStudent(Student S)
        {
            string result = "";
            try
            {
                _ORM.Student.Remove(S);
                _ORM.SaveChanges();
                result = "Yes";
            }
            catch (Exception e)
            {
                result = "No";
            }
            return result;
        }




















































































        public IActionResult Index()
        {
            return View();
        }
    }
}