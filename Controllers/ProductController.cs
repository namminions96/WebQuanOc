using CheckApiWeb.Models;
using CheckApiWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Web_Report.Data;
using System.ComponentModel;
using Org.BouncyCastle.Utilities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Smime;
using Microsoft.AspNetCore.Http;

namespace CheckApiWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UseDbcontext _useDbcontext;
        public ProductController(UseDbcontext useDbcontext, IWebHostEnvironment webHostEnvironment )
        {
            _useDbcontext = useDbcontext;
            _environment = webHostEnvironment;
        }
        //public List<CardItem> Carts
        //{
        //    get
        //    {
        //        var data = HttpContext.Session.Get<List<CardItem>>("GioHang");
        //        if (data == null)
        //        {
        //            return new List<CardItem>();
        //        }
        //        return data;
        //    }
        //    set
        //    {
        //        HttpContext.Session.Set("GioHang", value);
        //    }
        //}
        // GET: ProductController
        public ActionResult Index()
        {
            var userName = HttpContext.Session.GetString("Name");
            //if (userName == null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            var lsitem = _useDbcontext.Items.ToList();
            ViewBag.Name = userName;
            int count = _useDbcontext.CardItems.Count(p => p.User== userName);
            ViewBag.Count = count;
            int countdh = _useDbcontext.OrderHeaders.Count(p => p.User == userName);
            ViewBag.donhang = countdh;
            //------------------------------------------------------------------------------------//
            var allItems = _useDbcontext.CardItems.ToList().Where(p => p.User == userName);
            var cartItemsWithDetails = _useDbcontext.Items.ToList()
                           .Join(allItems,
            Items => Items.Id,
            item => item.Iditem,
            (Items, item) => new cart
            {
                Masp = Items.Id,
                Giaban = Items.Price,
                images = Items.images,
                Tensp = Items.Tensp,
                Soluong = item.Quantity,
                TongTien = Items.Price * item.Quantity
            })
        .ToList();

            double totalAmount = 0;
            foreach (var item in cartItemsWithDetails)
            {
                totalAmount += item.Giaban * item.Soluong;
            }
            ViewBag.TotalAmount = totalAmount;



            return View(lsitem);
        }
        //public IActionResult OnPostAddToCart(int id, string name, double price, int quantity)
        //{
        //    var cart = HttpContext.Session.Get<List<CardItem>>("GioHang") ?? new List<CardItem>();
        //    var product = cart.Find(p => p.Iditem == id);
        //    if (product != null)
        //    {
        //        product.Quantity += quantity;

        //    }
        //    else
        //    {
        //        CardItem item = new CardItem
        //        {
        //            Iditem = id,
        //            Name = name,
        //            Price = price,
        //            Quantity = quantity
        //        };
        //        cart.Add(item);
        //    }

        //    HttpContext.Session.Set("GioHang", cart);
        //    return new JsonResult(new { message = "Sản phẩm đã được thêm vào giỏ hàng thành công!" });
        //}

        public IActionResult OnPostAddToCart(int id, string name, double price, int quantity,string User)
        {
            var userName = HttpContext.Session.GetString("Name");
            if (userName == null)
            {
                return new JsonResult(new { message = "vui lòng đăng nhập trược khi mua hàng !" });
            }
            var product = _useDbcontext.CardItems.FirstOrDefault(p => p.Iditem == id && p.User == userName);
            if (product != null)
            {
                product.Quantity += quantity;
                product.User = userName;
                _useDbcontext.SaveChanges();
            }
            else
            {
                var item = new CardItem()
                {
                    Iditem = id,
                    Name = name,
                    Price = price,
                    Quantity = quantity,
                    User = userName
                };
                _useDbcontext.CardItems.Add(item);
                _useDbcontext.SaveChanges();
            }
            return new JsonResult(new { message = "Sản phẩm đã được thêm vào giỏ hàng thành công!" });
        }

        // GET: ProductController/Details/5
        public ActionResult<cart> Details()
        {
            var userName = HttpContext.Session.GetString("Name");
            if (userName == null)
            {
                return RedirectToAction("Index", "Login");
            }
            @ViewBag.Name = userName;
            int count = _useDbcontext.CardItems.Count(p => p.User == userName);
            ViewBag.Count = count;
            int countdh = _useDbcontext.OrderHeaders.Count(p => p.User == userName);
            ViewBag.donhang = countdh;
            //--------------------------------------------------------
            var allItems = _useDbcontext.CardItems.ToList().Where(p=> p.User == userName);
            var cartItemsWithDetails = _useDbcontext.Items.ToList()
                           .Join(allItems,
            Items => Items.Id,
            item => item.Iditem,
            (Items, item) => new cart
            {
                Masp = Items.Id,
                Giaban = Items.Price,
                images = Items.images,
                Tensp = Items.Tensp,
                Soluong = item.Quantity,
                TongTien = Items.Price * item.Quantity
            })
        .ToList();

            double totalAmount = 0;
            foreach (var item in cartItemsWithDetails)
            {
                totalAmount += item.Giaban * item.Soluong;
            }

            // Truyền tổng tiền vào View để hiển thị
            ViewBag.TotalAmount = totalAmount;
            return View(cartItemsWithDetails);
            
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();

        }
        // POST: ProductController/Create
        [HttpPost]
        public ActionResult Create(Item item, IFormFile file)
        {
            try
            {
                //var userName = HttpContext.Session.GetString("Username");
                //if (userName == null)
                //{
                //    return RedirectToAction("login", "login");
                //}
                if (file != null && file.Length > 0 && item.Tensp!= null && item.Tensp!="" && item.Price>0 && item.Dvt!= "-- Chọn DVT --")
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Get the path to the folder where you want to save the image
                    string imagePath = Path.Combine(_environment.WebRootPath, "images", fileName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var sp = new Item()
                    {
                        Id = 0,
                        Dvt = item.Dvt,
                        Title = item.Title,
                        Tensp = item.Tensp,
                        images = fileName,
                        Price=item.Price
                    };
                    _useDbcontext.Items.Add(sp);
                    _useDbcontext.SaveChanges();
                    TempData["succesfully"] = "User Create Successfully";
                    return RedirectToAction("Create");
                }
                else
                {
                    var pro = new Item()
                    {
                        Id = 0,
                        Dvt = item.Dvt,
                        Title = item.Title,
                        Tensp = item.Tensp,
                        images ="",
                        Price = item.Price

                    };
                    _useDbcontext.Items.Add(pro);
                    _useDbcontext.SaveChanges();
                    TempData["succesfully"] = "User Create Successfully";
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                return View();
            }
        }

       // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return RedirectToAction("Detail", "Product");
        }

        // post: productcontroller/edit/5
        [HttpPost]
        public ActionResult Edit(string Ten, string sdt,string diachi)
        {
            try
            {
                if (Ten == null && sdt == null && diachi == null)
                {
                    return new JsonResult(new { message = $"Bạn chưa nhập thông tin giao hàng!" });
                }
                double totalAmount = 0;
                var userName = HttpContext.Session.GetString("Name");
                var sodh = _useDbcontext.ConfigOrders.FirstOrDefault(p=> p.Name=="SaleOrder");
                var getdh = _useDbcontext.CardItems.ToList().Where(p => p.User == userName);
                var savedh = new OrderDetail();
                foreach (var item in getdh)
                {
                    savedh.Id = 0;
                    savedh.userid = item.User;
                    savedh.OrderDate = DateTime.Now;
                    savedh.Status = false;
                    savedh.Tensp = item.Name;
                    savedh.Masp = item.Iditem;
                    savedh.Soluong = item.Quantity;
                    savedh.TongTien = item.Price * item.Quantity;
                    _useDbcontext.Add(savedh);
                    _useDbcontext.SaveChanges();
                    totalAmount += item.Price * item.Quantity;
                }
                var saveheader = new OrderHeader()
                {
                    Id=0,
                    Madh = sodh.MaOrder + 1,
                    Tenkh = Ten,
                    Diachi = diachi,
                    Sodienthoai = sdt,
                    Orderdate = DateTime.Now,
                    Tongtien = totalAmount,
                    Status = false,
                    User = userName
                };
                _useDbcontext.OrderHeaders.Add(saveheader);
                _useDbcontext.SaveChanges();
                foreach (var item in getdh)
                {
                    _useDbcontext.CardItems.Remove(item);
                }
                _useDbcontext.SaveChanges();
                    return new JsonResult(new { message = $"Sản phẩm đã được đặt thành công mã đơn hàng là : {saveheader.Madh}!" });
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }

        }
        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: ProductController/Delete/5
        public ActionResult Order(int id)
        {
            var userName = HttpContext.Session.GetString("Name");
            if (userName == null)
            {
                return RedirectToAction("Index", "Login");
            }
            @ViewBag.Name = userName;
            int count = _useDbcontext.CardItems.Count(p => p.User == userName);
            ViewBag.Count = count;
            int countdh = _useDbcontext.OrderHeaders.Count(p => p.User == userName);
            ViewBag.donhang = countdh;
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult addCart(string id , int Soluong)
        {
                return View();
        }
        public async Task<IActionResult> GetAll()
        {
            var Response = await GetUsersn();
            return Json(Response);
        }


        public async Task<IActionResult> GetAllOrder()
        {
            var Response = await GetAllOrderdetail();
            return Json(Response);
        }
        public async Task<List<Item>> GetUsersn()
        {
            //var _data = await _useDbcontext.Usertblists.FromSqlRaw("Select id,HovaTen,NgaySinh,CCCD,Email,SoDienThoai,SoTienNap,NgayTao,Status,Ngaysapsinhnhat  from (SELECT *,(month(Ngaysinh)-month(curdate())) as Thang,day(Ngaysinh)-Day(curdate()) as NgaySapSinhNhat FROM `db_report`.`usertb`) as A where Thang=0 and  Ngaysapsinhnhat between '0' and '7'").ToListAsync();//where Thang=0 and  Ngaysapsinhnhat between '0' and '7'
            var _data = await _useDbcontext.Items.ToListAsync();
            List<Item> usertb1 = new List<Item>();
            if (_data != null && _data.Count > 0)
            {
                _data.ForEach(item =>
                {

                    usertb1.Add(new Item()
                    {
                        Id = item.Id,
                        Tensp = item.Tensp,
                        Price = item.Price,
                        Title = item.Title,
                        images = item.images,
                        Dvt = item.Dvt,
                    });
                });
            }
            return usertb1;
        }

        public async Task<List<OrderHeader>> GetAllOrderdetail()
        {
            var userName = HttpContext.Session.GetString("Name");
            if (userName == null)
            {
                return new List<OrderHeader>();
            }
            //var _data = await _useDbcontext.Usertblists.FromSqlRaw("Select id,HovaTen,NgaySinh,CCCD,Email,SoDienThoai,SoTienNap,NgayTao,Status,Ngaysapsinhnhat  from (SELECT *,(month(Ngaysinh)-month(curdate())) as Thang,day(Ngaysinh)-Day(curdate()) as NgaySapSinhNhat FROM `db_report`.`usertb`) as A where Thang=0 and  Ngaysapsinhnhat between '0' and '7'").ToListAsync();//where Thang=0 and  Ngaysapsinhnhat between '0' and '7'
            var _data = _useDbcontext.OrderHeaders.Where(p=> p.User == userName).ToList();
            List<OrderHeader> Order = new List<OrderHeader>();
            if (_data != null && _data.Count > 0)
            {
                _data.ForEach(item =>
                {

                    Order.Add(new OrderHeader()
                    {
                        Id = item.Id,
                        Tenkh = item.Tenkh,
                        Madh = item.Madh,
                        Diachi = item.Diachi,
                        Sodienthoai = item.Sodienthoai,
                        Status = item.Status,
                        Orderdate = item.Orderdate,
                        Tongtien = item.Tongtien,
                        User = item.User
                    });
                });
            }
            return Order;
        }

        public async Task<IActionResult> Remove(string code)
        {
            string Response = await RemoveEmployee(Convert.ToInt32(code));
            return Json(Response);
        }

        public async Task<IActionResult> RemoveCart(string masp)
        {
            string Response = await RemoveCart(Convert.ToInt32(masp));
            return Json(Response);
        }
        public async Task<string> RemoveEmployee(int Code)
        {
            var _data = await _useDbcontext.Items.FirstOrDefaultAsync(item => item.Id == Code);
            string Response = string.Empty;
            if (_data != null)
            {
                try
                {
                    _useDbcontext.Items.Remove(_data);
                    await _useDbcontext.SaveChangesAsync();
                    Response = "pass";
                }
                catch (Exception ex)
                {

                }
            }
            return Response;
        }


        public async Task<string> RemoveCart(int Code)
        {
            var _data = await _useDbcontext.CardItems.FirstOrDefaultAsync(item => item.Id == Code);
            string Response = string.Empty;
            if (_data != null)
            {
                try
                {
                    _useDbcontext.CardItems.Remove(_data);
                    await _useDbcontext.SaveChangesAsync();
                    Response = "pass";
                }
                catch (Exception ex)
                {

                }
            }
            return Response;
        }
        [HttpGet]
        public ActionResult gettable(string table)
        {
            if (table !=null)
            {
                HttpContext.Session.SetString("Table", table);
            }
           return RedirectToAction("Index");
        }
        public async Task<string> RemoveFromCart(int masp)
        {
            var itemToRemove = _useDbcontext.CardItems.SingleOrDefault(item => item.Iditem == masp);
            string Response = string.Empty;
            if (itemToRemove != null)
                {
               _useDbcontext.CardItems.Remove(itemToRemove);
               await _useDbcontext.SaveChangesAsync();
                Response = "pass";
            }
            return Response; 
        }
        public ActionResult UpdateCart(int Iditem,int Quantity)
        {
            try
            {
                var userName = HttpContext.Session.GetString("Name");
                var cart = _useDbcontext.CardItems.FirstOrDefault(p => p.Iditem == Iditem && p.User == userName);
                if (cart != null)
                {
                    cart.Quantity = Quantity;
                }
                _useDbcontext.SaveChanges();
                return RedirectToAction("Detail", "Product");
            }
            catch
            {
                return RedirectToAction("Detail", "Product");
            }
        }
    }
}
