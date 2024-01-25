using Backtowork.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RestSharp;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Backtowork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["url"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";



            return View();
        }


        public IActionResult refreshmybox(string data = "")
        {
            using var db = new SavedataContext();

            if(data != "")
            {
                if(db.Students.Where(x=>x.text == data).Count() == 0)
                {
                    db.Add(new mahasiswa { text = data });
                    db.SaveChanges();
                }
            }

            var blog = db.Students.OrderBy(b => b.Id);
            string cc = "";
            foreach(var item in blog)
            {
                cc += "<div>"+ item.text + "<button class='button success' id='delete' data-id='" + item.Id+"'>Delete</button></div><br/>";

            }
              
            return Content(cc);
        }


        public IActionResult delete(int data)
        {
            using var db = new SavedataContext();
            if (db.Students.Where(x => x.Id == data).Count() > 0)
            {
                db.Remove(db.Students.Where(x => x.Id == data).First());
                db.SaveChanges();
            }
        

            var blog = db.Students.OrderBy(b => b.Id);
            string cc = "";
            foreach (var item in blog)
            {
                cc += "<div>" + item.text + "<button class='button success' id='delete' data-id='" + item.Id + "'>Delete</button></div><br/>";

            }

            return Content(cc);
        }

        public IActionResult checkmygraduate(string search = "")
        {
            string cc = "";

            var client = new RestClient("https://api-frontend.kemdikbud.go.id");
            var request = new RestRequest("/hit_mhs/" + search);
            var response = client.ExecuteGet(request);

            cc = response.Content;

            JObject o = JObject.Parse(cc);
            cc = "not found";
            foreach (var x in o.GetValue("mahasiswa"))
            {
                if (x.ToString().ToLower().Contains(search.ToLower()))
                    cc = x.ToString();
            }
            if (cc != "not found")
            {
                JObject ox = JObject.Parse(cc);
                cc = ox.GetValue("text").ToString();
                string[] ccdumpt = cc.Split(",");
                cc = "";
                foreach(var x in ccdumpt)
                {
                    cc += x+"<br/>";
                }
            }

            return Content(cc);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}