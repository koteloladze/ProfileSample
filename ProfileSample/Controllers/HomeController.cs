using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ProfileSampleEntities();

            var sources = context.ImgSources.Take(20).AsEnumerable();

            var model = new List<ImageModel>();

            foreach (var imgSource in sources)
            {
                var obj = new ImageModel()
                {
                    Name = imgSource.Name,
                    Data = imgSource.Data
                };

                model.Add(obj);
            }

            return View(model);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");
            var imgList = new List<ImgSource>();

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        stream.ReadAsync(buff, 0, (int)stream.Length);

                        var entity = new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        };

                        imgList.Add(entity);
                    }
                }

                context.ImgSources.AddRange(imgList);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}