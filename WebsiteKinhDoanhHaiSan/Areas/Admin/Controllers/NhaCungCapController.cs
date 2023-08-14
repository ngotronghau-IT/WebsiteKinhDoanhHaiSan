using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKinhDoanhHaiSan.Models;

namespace WebsiteKinhDoanhHaiSan.Areas.Admin.Controllers
{
    public class NhaCungCapController : BaseController
    {
        QLHaiSanEntities db = new QLHaiSanEntities();

        // GET: Admin/NhaCungCap
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadDs(int trang = 1)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var dsNCC = db.NhaCungCaps.Where(m => m.DaXoa != 1).ToList();


                // phan trang
                var pageSize = 6;
                var pageNumber = dsNCC.Count() % pageSize == 0 ? dsNCC.Count() / pageSize : (dsNCC.Count() / pageSize) + 1;

                var dsHienThi = dsNCC.Skip((trang - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { trangthai = 0, data = dsHienThi, soTrang = pageNumber }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { trangthai = 1, msg = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult Create(string TenNCC, string DiaChi, string SDT, string Email)
        {
            try
            {
                NhaCungCap ncc = new NhaCungCap();
                ncc.TenNCC = TenNCC;

                ncc.DiaChi = DiaChi;

                ncc.SDT = SDT;
                ncc.Email = Email;


                db.NhaCungCaps.Add(ncc);
                db.SaveChanges();
                return Json(new { trangthai = true, msg = "Thêm mới thành công !!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { trangthai = true, msg = "Thêm mới Faillll !!" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult Update(int MaNCC, string TenNCC, string DiaChi, string SDT, string Email)
        {
            try
            {
                var ncc = db.NhaCungCaps.FirstOrDefault(x => x.MaNCC == MaNCC);

                ncc.TenNCC = TenNCC;
                ncc.DiaChi = DiaChi;

                ncc.SDT = SDT;
                ncc.Email = Email;

                db.SaveChanges();
                return Json(new { trangthai = true, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { trangthai = false, msg = "Cập nhật Failll! :" + ex }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // cau hinh proxy cho database //fix loi server 500
                var ncc = db.NhaCungCaps.FirstOrDefault(x=>x.MaNCC==id);

                return Json(new { trangthai = 0, data = ncc, msg = "Lấy thông tin thành công !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { trangthai = 1, msg = "Lấy thông tin Failll :  !" + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var nv = db.NhaCungCaps.FirstOrDefault(x => x.MaNCC == id);

                nv.DaXoa = 1;
                db.SaveChanges();
                return Json(new { trangthai = 0, msg = "Xóa Nhà Cung Cấp có tên : " + nv.TenNCC + " thành công!" }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { trangthai = 1, msg = "Xóa thất bại Failll!:" + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult NCCDaXoa()
        {
            List<NhaCungCap> lst = db.NhaCungCaps.Where(x => x.DaXoa == 1).ToList();
            return View(lst);
        }

        public ActionResult HoanTac(int MaNCC)
        {
            NhaCungCap ncc = db.NhaCungCaps.FirstOrDefault(x => x.MaNCC == MaNCC);
            ncc.DaXoa = 0;
            db.SaveChanges();
            return Redirect("NCCDaXoa");
        }
    }
}