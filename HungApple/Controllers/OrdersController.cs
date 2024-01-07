using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HungApple.Data;
using HungApple.Models;
using HungApple.Infrastructure;
using HungApple.Models.ViewModels;

namespace HungApple.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HungAppleContext _context;

        public OrdersController(HungAppleContext context)
        {
            _context = context;
        }

        //GET: Orders
        //public async Task<IActionResult> Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index(PaymentModel? cart)
        {
            ViewDefault();
            var userDetail = new User();
            if (User != null && User.Claims != null && User.Claims.Count() > 0)
            {
                var users = User.Claims.FirstOrDefault();
                int userId = (users != null ? int.Parse(users.ValueType) : 0);
                userDetail = _context.User.Find(userId);
            }
            var model = new CartLine();
            if(cart != null)
            {
                var product = _context.Product.Find(cart.ProductId);
                if(product != null)
                {
                    model.Product = product;
                    model.Quantity = cart.Quantity;
                }
            }
            ViewBag.UserDetail = userDetail;
            ViewBag.Payment = _context.Delivery.ToList();
            ///var hungAppleContext = _context.Order.Include(o => o.Delivery).Include(o => o.Districts).Include(o => o.Provinces).Include(o => o.User).Include(o => o.Wards);
            return View(model);
        }
        public void ViewDefault()
        {
            ViewBag.ListProvinces = _context.Provinces.ToList();
            ViewBag.ListDistricts = _context.Districts.ToList();
            ViewBag.ListWards = _context.Wards.ToList();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Delivery)
                .Include(o => o.Districts)
                .Include(o => o.Provinces)
                .Include(o => o.User)
                .Include(o => o.Wards)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["DeliveryId"] = new SelectList(_context.Delivery, "Id", "Description");
            ViewData["DistrictId"] = new SelectList(_context.Districts, "district_id", "district_id");
            ViewData["ProvinceId"] = new SelectList(_context.Provinces, "province_id", "province_id");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Password");
            ViewData["WardId"] = new SelectList(_context.Wards, "ward_id", "ward_id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,UserId,DeliveryId,ProvinceId,DistrictId,WardId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryId"] = new SelectList(_context.Delivery, "Id", "Description", order.DeliveryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "district_id", "district_id", order.DistrictId);
            ViewData["ProvinceId"] = new SelectList(_context.Provinces, "province_id", "province_id", order.ProvinceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Password", order.UserId);
            ViewData["WardId"] = new SelectList(_context.Wards, "ward_id", "ward_id", order.WardId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["DeliveryId"] = new SelectList(_context.Delivery, "Id", "Description", order.DeliveryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "district_id", "district_id", order.DistrictId);
            ViewData["ProvinceId"] = new SelectList(_context.Provinces, "province_id", "province_id", order.ProvinceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Password", order.UserId);
            ViewData["WardId"] = new SelectList(_context.Wards, "ward_id", "ward_id", order.WardId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,UserId,DeliveryId,ProvinceId,DistrictId,WardId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["DeliveryId"] = new SelectList(_context.Delivery, "Id", "Description", order.DeliveryId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "district_id", "district_id", order.DistrictId);
            ViewData["ProvinceId"] = new SelectList(_context.Provinces, "province_id", "province_id", order.ProvinceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Password", order.UserId);
            ViewData["WardId"] = new SelectList(_context.Wards, "ward_id", "ward_id", order.WardId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Delivery)
                .Include(o => o.Districts)
                .Include(o => o.Provinces)
                .Include(o => o.User)
                .Include(o => o.Wards)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'HungAppleContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
