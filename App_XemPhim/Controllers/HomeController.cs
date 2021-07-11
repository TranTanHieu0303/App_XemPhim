using App_XemPhim.Models;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Firebase.Auth;
using Firebase.Storage;
using System.Threading.Tasks;

namespace App_XemPhim.Controllers
{
    public class HomeController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "vDgBJNEJxSebDhSkPb7KYppj5kMNzIs0faVdci5Y",
            BasePath = "https://do-an-ltdt-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhMuc");
            dynamic dataDM= JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<DanhMuc> lstDm = new List<DanhMuc>();
            if(dataDM!=null)
            foreach(var item in dataDM)
            {
                DanhMuc dm = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
                lstDm.Add(dm);
            }
            ViewData["DM"] = lstDm;
            FirebaseResponse responseTL = client.Get("TheLoai");
            dynamic dataTL = JsonConvert.DeserializeObject<dynamic>(responseTL.Body);
            List<TheLoai> lstTL = new List<TheLoai>();
            if (dataTL != null)
                foreach (var item in dataTL)
                {
                    TheLoai Tl = JsonConvert.DeserializeObject<TheLoai>(((JProperty)item).Value.ToString());
                    lstTL.Add(Tl);
                }
            ViewData["TL"] = lstTL;
            FirebaseResponse responseQG = client.Get("QuocGia");
            dynamic dataQG = JsonConvert.DeserializeObject<dynamic>(responseQG.Body);
            List<QuocGia> lstQG = new List<QuocGia>();
            if (dataQG != null)
                foreach (var item in dataQG)
                {
                    QuocGia qg = JsonConvert.DeserializeObject<QuocGia>(((JProperty)item).Value.ToString());
                    lstQG.Add(qg);
                }
            ViewData["QG"] = lstQG;
            FirebaseResponse responsePhim = client.Get("Phim");
            dynamic dataPhim = JsonConvert.DeserializeObject<dynamic>(responsePhim.Body);
            List<Phim> lstPhim = new List<Phim>();
            if (dataPhim != null)
                foreach (var item in dataPhim)
                {
                    Phim ph = JsonConvert.DeserializeObject<Phim>(((JProperty)item).Value.ToString());
                    lstPhim.Add(ph);
                }
            ViewData["Phim"] = lstPhim;
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string Tendn = f["Tendn"];
            string Pass = f["Pass"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Admin");
            TaiKhoan tk = JsonConvert.DeserializeObject<TaiKhoan>(response.Body);
            if (tk.TenDN == Tendn && tk.Pass == Pass)
            {
                Session["tk"] = tk;
                return RedirectToAction("Index");
            } 
            else
            {
                ViewBag.tb = "Tài khoản hoặc mật khẩu không chính xác";
                return View();
            }
                
        }
        [HttpPost]
        public JsonResult AddDanhMuc(string TEN)
        {
            if(TEN=="")
                return Json(0, JsonRequestBehavior.AllowGet);
            DanhMuc danhMuc = new DanhMuc();
            danhMuc.TenDM = TEN;
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("DanhMuc/",danhMuc);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string ma = push.Result.name.ToString();
                danhMuc.MaDM = ma;
                SetResponse set = client.Set("DanhMuc/" + ma, danhMuc);
                if (set.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(0, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public JsonResult AddTheLoai(string TEN)
        {
            if (TEN == "")
                return Json(0, JsonRequestBehavior.AllowGet);
            TheLoai theLoai = new TheLoai();
            theLoai.TenTL = TEN;
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("TheLoai/", theLoai);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string ma = push.Result.name.ToString();
                theLoai.MaTL = ma;
                SetResponse set = client.Set("TheLoai/" + ma, theLoai);
                if (set.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(0, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult AddQuocGia(string TEN)
        {
            if (TEN == "")
                return Json(0, JsonRequestBehavior.AllowGet);
            QuocGia quocGia = new QuocGia();
            quocGia.TenQG = TEN;
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("QuocGia/", quocGia);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string ma = push.Result.name.ToString();
                quocGia.MaQG = ma;
                SetResponse set = client.Set("QuocGia/" + ma, quocGia);
                if (set.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(0, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<ActionResult> AddPhimAsync()
        {
            Phim ph = new Phim();
            string[] f = Request.Form["form"].Split('&');
            ph.TenPhim = Request.Form["TenPhim"];
            ph.MoTa = Request.Form["MoTa"];
            ph.Thoiluong = Request.Form["Thoiluong"];
            ph.sotap = Request.Form["SoTap"];
            ph.LinkTrailer = Request.Form["TraiLer"];
            ph.LinkPhim = Request.Form["LinkP"];
            ph.Dienvien = Request.Form["DienVien"];
            foreach (string item in f )
            {

                if (item.Contains("DM"))
                {
                    if (ph.DanhMuc == null)
                        ph.DanhMuc = item.Replace("DM=", string.Empty);
                    else
                        ph.DanhMuc += "," + item.Replace("DM=", string.Empty);
                }
                if (item.Contains("TL"))
                {
                    if (ph.TheLoai == null)
                        ph.TheLoai = item.Replace("TL=", string.Empty);
                    else
                        ph.TheLoai += "," + item.Replace("TL=", string.Empty);
                }
                if (item.Contains("QG"))
                    if (ph.QuocGia == null)
                        ph.QuocGia = item.Replace("QG=", string.Empty);
                }
            HttpFileCollectionBase Hinh = Request.Files;
            //ph.TenPhim = f["TenP"];
            //ph.DanhMuc = f["DM"];
            //ph.TheLoai = f["TL"];
            //ph.QuocGia = f["QG"];
            //ph.MoTa = f["MoTa"];
            //ph.sotap = f["Sotap"];
            //ph.Dienvien = f["DienVien"];
            //ph.Thoiluong = f["ThoiLuong"];
            //ph.LinkTrailer = f["Trailer"];
            //ph.LinkPhim = f["LinkP"];
            if (Hinh != null)
            {
                HttpPostedFileBase file = Hinh.Get(0);
                string ten = file.FileName;
                var stream = file.InputStream;

                //authentication
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyDNngpU-uh01VhXvlNQBaaF3hgovaROFOU"));
                var a = await auth.SignInWithEmailAndPasswordAsync("trantanhieu1804@gmail.com", "18040303");

                // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
                var task = new FirebaseStorage(
                    "do-an-ltdt.appspot.com",


                     new FirebaseStorageOptions
                     {
                         AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                         ThrowOnCancel = true,
                     })
                    .Child("image")
                    .Child(DateTime.Now.Millisecond + ten)
                    .PutAsync(stream);

                // Track progress of the upload
                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                // await the task to wait until upload completes and get the download url
                var downloadUrl = await task;
                ph.HinhAnh = downloadUrl;
            }
            else
            {
                ph.HinhAnh = "";
            }
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("Phim/", ph);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string ma = push.Result.name.ToString();
                ph.MaPhim = ma;
                SetResponse set = client.Set("Phim/" + ma, ph);
                if (set.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
