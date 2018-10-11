using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Asp_Pro;
using Asp_Pro.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Pro.Controllers
{
    public class studentController : Controller
    {
        StudentRegContext _ORM = null;
        IHostingEnvironment _ENV = null;
        public studentController(StudentRegContext ORM, IHostingEnvironment ENV)
        {
            _ORM = ORM;
            _ENV = ENV;


        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student S, IFormFile Cv)
        {
            string WwwRoot = _ENV.WebRootPath;
            string FTPpath = WwwRoot + "/WebData/Cv";
            string UniqueName = Guid.NewGuid().ToString();
            string FileExtension = Path.GetExtension(Cv.FileName);
            FileStream Fs = new FileStream(FTPpath + UniqueName + FileExtension, FileMode.Create);
            Cv.CopyTo(Fs);
            Fs.Close();
            S.Cv = "/WebData/Cv" + UniqueName + FileExtension;
            _ORM.Add(S);
            _ORM.SaveChanges();
            string APIURL = "http://bulksms.com.pk/api/sms.php?username=923316125207&password=9178&sender=BrandName&mobile=923349931854&message= Welcome to our website.";
            using (var APIClient = new HttpClient())
            {
                Task<HttpResponseMessage> RM = APIClient.GetAsync(APIURL);
                Task<string> FinalRespone = RM.Result.Content.ReadAsStringAsync();
            }
            ModelState.Clear();

            MailMessage Obj = new MailMessage();
            Obj.From = new MailAddress("wa625775@gmail.com");
            Obj.To.Add(new MailAddress(S.Email));
            Obj.Subject = "Welcome to theta Solution:";
            Obj.Body = "Dear" + S.Name + "<br ><br >" +
            "Thanks for registration with Theta Solution";
            Obj.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(S.Cv))
            {
                Obj.Attachments.Add(new Attachment(_ENV.WebRootPath + S.Cv));
            }
            SmtpClient SMTP = new SmtpClient();
            SMTP.Host = "wa625775@gmail.com";
            SMTP.Port = 587;
            SMTP.EnableSsl = true;
            SMTP.Credentials = new System.Net.NetworkCredential("wa625775@gmail.com", "programer");
            try
            {
                SMTP.Send(Obj);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Mail has sent successfully";
            }
            ViewBag.Message = "New student has added to list";
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

        public FileResult DownloadCV(string Path)
        {
            if (string.IsNullOrEmpty(Path))
            {
                ViewBag.Message = "Invalid Path";
                return null;
            }
            return File(Path, new MimeSharp.Mime().Lookup(Path), DateTime.Now.ToString("ddMMyyyyhhmmss") + System.IO.Path.GetExtension(Path));
        }
        public string ShowAd()
        {
            string Ad = "";
            Ad = "<img class='img img-responsive' src='http://lorempixel.com/400/400/sports/Theta-Solutions/'/>";
            return Ad;
        }
        public string GetStudentsNames()
        {
            string Result = "";
            var r = Request;
            IList<Student> All = _ORM.Student.ToList<Student>();
            Result += "<h1 class='alert alert-success'>Total Students: " + All.Count + "</h1>";
            foreach (Student S in All)
            {
                Result += "<a href='/student/StudentDetail?Id=" + S.Id + "'><p><span class='glyphicon glyphicon-user'></span> " + S.Name + "</p></a> <a href='/student/deletestudent1?id=" + S.Id + "'><p><span class='glyphicon glyphicon-remove'></span> Delete </p></a>";
            }
            return Result;
        }




















































































        public IActionResult Index()
        {
            return View();
        }
    }
}