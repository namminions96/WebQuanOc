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
        public List<CardItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CardItem>>("GioHang");
                if (data == null)
                {
                    return new List<CardItem>();
                }
                return data;
            }
            set
            {
                HttpContext.Session.Set("GioHang", value);
            }
        }
        // GET: ProductController
        public ActionResult Index()
        {
            var userName = HttpContext.Session.GetString("Name");
            if (userName == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var lsitem = _useDbcontext.Items.ToList();
            ViewBag.Name = userName;
     
            return View(lsitem);
        }
        public IActionResult OnPostAddToCart(int id, string name, double price, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CardItem>>("GioHang") ?? new List<CardItem>();
            var product = cart.Find(p => p.Iditem == id);
            if (product != null)
            {
                product.Quantity += quantity;
              
            }
            else
            {
                CardItem item = new CardItem
                {
                    Iditem = id,
                    Name = name,
                    Price = price,
                    Quantity = quantity
                };
                cart.Add(item);
            }

            HttpContext.Session.Set("GioHang", cart);
            return new JsonResult(new { message = "Sản phẩm đã được thêm vào giỏ hàng thành công!" });
        }
    // GET: ProductController/Details/5
    public ActionResult<cart> Details(int id)
        {
            var allItems = Carts.ToList();
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
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        public ActionResult Edit()
        {
            try
            {
                var name = HttpContext.Session.GetString("Name");
                var userid = HttpContext.Session.GetString("Table");
                var table = userid != null ? userid : name;
                var allItems = Carts.ToList();
                //var card1 = _useDbcontext.cartSaves.ToList()
                //    .Join(allItems, a => a.Masp, b => b.Iditem, (a, b)
                //    => new
                //    {
                //        b.Price,
                //        b.Iditem,b.Quantity,b.Name
                //    }).ToList();
                CartSave cartSaves = new CartSave();
                foreach (var item in allItems)
                {
                 var check = _useDbcontext.cartSaves.FirstOrDefault(p=> p.Masp == item.Iditem && p.userid == userid);

                    if (check ==null)
                    {
                    cartSaves.Id = 0;
                    cartSaves.userid = userid;
                    cartSaves.OrderDate = DateTime.Now.Date;
                    cartSaves.Status = false;
                    cartSaves.Tensp = item.Name;
                    cartSaves.Giaban = item.Price;
                    cartSaves.Masp = item.Iditem;
                    cartSaves.TongTien = item.Price * item.Quantity;
                    cartSaves.Soluong = item.Quantity;
                    cartSaves.Name = name;
                    _useDbcontext.cartSaves.Add(cartSaves);
                    _useDbcontext.SaveChanges();
                    }
                    else
                    {
                        check.Soluong= item.Quantity;
                        _useDbcontext.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            } catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
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
        public async Task<IActionResult> Remove(string code)
        {
            string Response = await RemoveEmployee(Convert.ToInt32(code));
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
        [HttpGet]
        public ActionResult gettable(string table)
        {
            if (table !=null)
            {
                HttpContext.Session.SetString("Table", table);
            }
           return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(int productId)
        {
            // Kiểm tra xem sản phẩm có trong giỏ hàng không
          
                // Tìm sản phẩm trong giỏ hàng và xóa nó
                var itemToRemove = Carts.SingleOrDefault(item => item.Iditem == productId);
                if (itemToRemove != null)
                {
                    Carts.Remove(itemToRemove);
                }
                HttpContext.Session.Set("GioHang", System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(Carts));

            return RedirectToAction("Detail", "Product"); 
        }
    }
}
