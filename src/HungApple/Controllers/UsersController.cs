using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HungApple.Data;
using HungApple.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HungApple.Controllers
{
    public class UsersController : Controller
    {
        private readonly HungAppleContext _context;

        public UsersController(HungAppleContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'HungAppleContext.User'  is null.");
        }

        // GET: Users/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewDefault();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Details(User user, IFormFile fileImage)
        {
            if(user.Id == 0)
            {
                return NotFound();
            }
            var dataUser = await _context.User
                .FirstOrDefaultAsync(m => m.Id == user.Id);
            if(dataUser == null)
            {
                ViewBag.Error = "Cập nhật không thành công!";
                return View(user);
            }
            var pathImage = string.Empty;
            if (fileImage != null)
            {
                string filename = "";
                filename = fileImage.FileName;
                string folderName = DateTime.Now.ToString("yyMMdd");
                string newPath = Path.Combine(folderName);
                string SavePath = Path.Combine("wwwroot/Upload/Avatar", newPath, filename);
                var fi = new FileInfo(SavePath);
                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    fileImage.CopyTo(stream);
                }
                pathImage = folderName + "\\" + filename;
            }        
            dataUser.Email = user.Email;
            dataUser.Phone = user.Phone;
            dataUser.ProvinceId = user.ProvinceId;
            dataUser.DistrictId = user.DistrictId;
            dataUser.WardId = user.WardId;
            dataUser.ImagePath = (!string.IsNullOrEmpty(pathImage) ? pathImage : dataUser.ImagePath);
            _context.Update(dataUser);
            _context.SaveChanges();
            ViewBag.Success = "Cập nhật thành công";
            return Redirect("/Users/Details?Id="+user.Id);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Username == model.Username || u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User already exists with this username or email.");
                    return View(model);
                }
                //Role mặc định
                model.Role = "User";
                model.DistrictId = 602;
                model.WardId = 9564;
                model.ProvinceId = 52;

                // Save the new user to the database
                var newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = Helper.ToMD5(model.Password),
                    Role = model.Role, // Set the role explicitly
                    DistrictId = model.DistrictId, // Set DistrictId explicitly
                    WardId = model.WardId, // Set WardId explicitly
                    ProvinceId = model.ProvinceId // Set ProvinceId explicitly
                };
                _context.Add(newUser);
                await _context.SaveChangesAsync();

                // Redirect to login or another appropriate action
                return RedirectToAction("Login");
            }

            return View(model);
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                var user =  _context.User.FirstOrDefault(u => u.Email == Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No account found with this email.");
                    return View();
                }
                user.ResetPasswordToken = Guid.NewGuid().ToString();
                user.ResetPasswordExpiration = DateTime.Now.AddHours(1);  // Token expires in 1 hour
                _context.Update(user);
                 _context.SaveChangesAsync();
                return Redirect("/Users/ResetPass?Id="+user.Id); 
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ResetPass(int Id)
        {
            var model = new ResertPassword();
            model.Id = Id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPass(ResertPassword resert)
        {
            if(resert.Id == 0)
            {
                ViewBag.Error = "Đặt lại mật khẩu không thành công";
                return View(resert);
            }
            if (!resert.PasswordNew.Equals(resert.ConfirmPassword))
            {
                ViewBag.Error = "Mật khẩu xác nhận không đúng";
                return View(resert);
            }
            var user = _context.User.Find(resert.Id);
            if(user == null)
            {
                ViewBag.Error = "Đặt lại mật khẩu không thành công";
                return View(resert);
            }
            var passHash = Helper.ToMD5(resert.PasswordNew);
            user.Password = passHash;
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Logout", "Users");
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Users");
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var passHash = Helper.ToMD5(password);
            var user = _context.User.Where(u => u.Username == username && u.Password == passHash).FirstOrDefault<User>();
            if (user == null || _context.User == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username, user.Id.ToString()),
            };
            // Check if user's Role is not null before creating a Claim for it
            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }
            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,Phone,Role,ResetPasswordToken,ResetPasswordExpiration")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'HungAppleContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {  
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordModel pass)
        {
            var user = User.Claims;
            int userId = 0;
            if (user != null && user.Count() > 0)
            {
                var dtUser = user.FirstOrDefault();
                if (dtUser != null)
                {
                    userId = int.Parse(dtUser.ValueType);
                }
            }
            else
            {
                return RedirectToAction("Logout", "Users");
            }
            pass.Id = userId;
            if (pass.Id == 0)
            {
                ViewBag.Error = "Cập nhật không thàng công";
                return View(user);
            }
            var dataUser = _context.User.Find(pass.Id);
            if(dataUser == null)
            {
                ViewBag.Error = "Cập nhật không thàng công";
                return View(user);
            }
            var passOld = Helper.ToMD5(pass.PasswordOld);
            if (!dataUser.Password.Equals(passOld))
            {
                ViewBag.Error = "Mật khẩu cũ không chính xác";
                return View(user);
            }
            if (!pass.PasswordNew.Equals(pass.ConfirmPassword))
            {
                ViewBag.Error = "Mật khẩu xác nhận không đúng";
                return View(user);
            }
            dataUser.Password = passOld;
            _context.Update(dataUser);
            _context.SaveChanges();
            return RedirectToAction("Logout", "Users");
        }
        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public void ViewDefault()
        {
            ViewBag.ListProvinces = _context.Provinces.ToList();
            ViewBag.ListDistricts = _context.Districts.ToList();
            ViewBag.ListWards =  _context.Wards.ToList();
        }
        public JsonResult GetListDistrictsByProvincesId(int provincesId)
        {
            var data = _context.Districts.Where(x => x.province_id == provincesId).ToList();
            return Json(new {status = true, data = data});
        }
        public JsonResult GetListWardByDistrictId(int districtId)
        {
            var data = _context.Wards.Where(x => x.district_id == districtId).ToList();
            return Json(new { status = true, data = data });
        }
        public async Task<IActionResult> Subscribe(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var model = _context.NewsLetterSubscription.Where(x => x.Email.ToLower().Equals(email.ToLower().Trim())).FirstOrDefault();
                if(model == null)
                {
                    var subscribe = new NewsLetterSubscription
                    {
                        Email = email.Trim(),
                        CreatedDate = DateTime.Now
                    };
                    _context.NewsLetterSubscription.Add(subscribe);
                    _context.SaveChanges();
                }
            }
            await Task.Delay(1500);
            return RedirectToAction("Index", "Home");
        }
    }
}
