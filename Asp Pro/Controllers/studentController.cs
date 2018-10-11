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




















































































        public IActionResult Index()
        {
            return View();
        }
    }
}