using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HungApple.Data;
using HungApple.Models;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using PagedList.Core;
using HungApple.Models.ViewModels;

namespace HungApple.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HungAppleContext _context;
        public int PageSize = 9;

        public ProductsController(HungAppleContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int productPage = 1)
        {
			IQueryable<Product> query = _context.Product.Include(p => p.Category);
			decimal minPrice = 1;
			decimal maxPrice = 9999999;
			if (query.Count() > 0)
			{
				maxPrice = query.Max(x => x.Price);
				minPrice = query.Min(x => x.Price);
			}
			ViewBag.MaxPrice = maxPrice;
			ViewBag.MinPrice = minPrice;
			return View
            (
                new ProductListViewModel
                {
                    Product = _context.Product.Skip((productPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = productPage,
                        TotalItems = _context.Product.Count()
                    }
				}
             );
        }
        [HttpPost]
        public async Task<IActionResult> Search(string keywords, int productPage = 1)
        {
            return View
            ("Index",
                new ProductListViewModel
                {
                    Product = _context.Product.Where(p => p.Name.Contains(keywords)).Skip((productPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = productPage,
                        TotalItems = _context.Product.Count()
                    }
                }
             );
        }

        public async Task<IActionResult> Index1()
        {
			IEnumerable<Product> products = _context.Product.Include(p => p.Category); // Lấy danh sách sản phẩm
			return View(products);
		}
		public IActionResult Viewbestsell() //hiển thị danh sách bestseller
		{
			// Giả sử bạn lấy danh sách các sản phẩm bán chạy từ database
			IEnumerable<Product> topSellingProducts = _context.Product.Include(p => p.Category).Include(p => p.IsBestSeller);
			// Lưu danh sách vào ViewBag để sử dụng trong view
			ViewBag.TopSellingProducts = topSellingProducts;
			return View();
		}

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id, int? page)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var lstComment = await _context.Comment.Join(_context.User,
                                        c => c.UserId,
                                        u => u.Id,
                                        (c, u) =>new Comment
                                        {
                                             Id = c.Id,
                                             Rating = c.Rating,
                                             Content = c.Content,
                                             DateCreated = c.DateCreated,
                                             UserId = c.UserId,
                                             ProductId = c.ProductId,
                                             UserName = u.Username
                                        }).Where(x => x.ProductId == id).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var modelRating = TotalRating(lstComment);
            ViewBag.ModelRating = modelRating;         
            double totalRating = 0;
            if (lstComment != null && lstComment.Count > 0)
            {
                totalRating = lstComment.Select(x => (double?)x.Rating).Average() ?? 0;
            }
            ViewBag.total = modelRating.TotalRating;
            int pageSize = 10;
            ViewBag.TotalPages = (int)Math.Ceiling((double)lstComment.Count / pageSize);
            int pageNumber = page ?? 1;
            int startIndex = (pageNumber - 1) * pageSize;
            ViewBag.TotalRating = Math.Round(totalRating, 1);
            lstComment = lstComment.Skip(startIndex).Take(pageSize).ToList();
            ViewBag.ListComment = lstComment;
            ViewBag.CurrentPage = pageNumber;
            return View(product);
        }

		public async Task<IActionResult> Filter(string category, decimal? priceMin, decimal? priceMax, int productPage = 1)
		{
            var stringCate = Request.Query["category"].ToString();
            ViewBag.Cate = stringCate; 
            IQueryable<Product> query = _context.Product.Include(p => p.Category);
            decimal minPrice = 1;
            decimal maxPrice = 9999999;
            if(query.Count() > 0)
            {
                maxPrice = query.Max(x => x.Price);
                minPrice = query.Min(x => x.Price);
            }
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinPrice = minPrice;
            //ViewBag.MaxPriceFilter = (priceMax == null ? maxPrice : priceMax);
            //ViewBag.MinPriceFilter = (priceMin == null ? minPrice : priceMin);
            var model = new ProductListViewModel();
            if (!string.IsNullOrEmpty(stringCate))
			{
				query = query.Where(p => stringCate.Contains(p.Category.Name));
			}
            if (priceMin.HasValue)
            {
                query = query.Where(p => p.Price >= priceMin.Value);
            }

            if (priceMax.HasValue)
            {
                query = query.Where(p => p.Price <= priceMax.Value);
            }
            query = query.Skip((productPage - 1) * PageSize).Take(PageSize);
            var PagingInfo = new PagingInfo
            {
                ItemsPerPage = PageSize,
                CurrentPage = productPage,
                TotalItems = _context.Product.Count()
            };
            model.Product = query;
            model.PagingInfo = PagingInfo;
			return View(model);
		}

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Detail,Price,Image,Color,Quantity,IsNew,IsBestSeller,CategoryId,DiscountId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Detail,Price,Image,Color,Quantity,IsNew,IsBestSeller,CategoryId,DiscountId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'HungAppleContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpPost]
        public ActionResult RatingComment(Comment comment)
        {
            if(comment.UserId == 0 || comment.ProductId == 0)
            {
                return NotFound(); ;
            }
            comment.DateCreated = DateTime.Now;
            _context.Comment.Add(comment);
            _context.SaveChanges();
            return Redirect("/Products/Details/" + comment.ProductId);
        }
        public CountRatingModel TotalRating(List<Comment> cmt)
        {
            var countRating = new CountRatingModel();
            if(cmt != null && cmt.Count > 0)
            {
                countRating.TotalRating = cmt.Count();
                countRating.TotalOne = cmt.Where(x => x.Rating == 1).Count();
                countRating.TotalTwo = cmt.Where(x => x.Rating == 2).Count();
                countRating.TotalThree = cmt.Where(x => x.Rating == 3).Count();
                countRating.TotalFour = cmt.Where(x => x.Rating == 4).Count();
                countRating.TotalFive = cmt.Where(x => x.Rating == 5).Count();
                double pecentOne = ((double)countRating.TotalOne / (double)countRating.TotalRating) * 100;
                double pecentTwo = ((double)countRating.TotalTwo / (double)countRating.TotalRating) * 100;
                double pecentThree = ((double)countRating.TotalThree / (double)countRating.TotalRating) * 100;
                double pecentFour = ((double)countRating.TotalFour / (double)countRating.TotalRating) * 100;
                double pecentFive = ((double)countRating.TotalFive / (double)countRating.TotalRating) * 100;
                countRating.PercentOne = Math.Round(pecentOne, 0);
                countRating.PercentTwo = Math.Round(pecentTwo, 0);
                countRating.PercentThree = Math.Round(pecentThree, 0);
                countRating.PercentFour = Math.Round(pecentFour, 0);
                countRating.PercentFive = Math.Round(pecentFive, 0);
            }
            return countRating;
        }
    }
}
