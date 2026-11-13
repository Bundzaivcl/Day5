using System;
using System.Collections.Generic;
using System.Linq;
using Day3.Models;
using Microsoft.AspNetCore.Mvc;
namespace Day3.Controllers
{
    public class ProductController : Controller
    {
        // Danh sách sản phẩm mẫu
        private static List<Product> _products = new List<Product>()
        {
            new Product()
            {
                Id = 1,
                Name = "Product 1",
                Price = 100000,
                Description = "Description 1",
                CategoryId = 1
            },
            new Product()
            {
                Id = 2,
                Name = "Product 2",
                Price = 200000,
                Description = "Description 2",
                CategoryId = 2
            },
            new Product()
            {
                Id = 3,
                Name = "Product 3",
                Price = 300000,
                Description = "Description 3",
                CategoryId = 3
            }
        };

        // GET: Product
        // Hiển thị danh sách và tìm kiếm sản phẩm
        public ActionResult Index([FromQuery] ProductSearchModel search)
        {
            // Kiểm tra số trang
            if (search.Page < 1)
            {
                search.Page = 1;
            }

            if (search.PageSize < 1)
            {
                search.PageSize = 10;
            }

            // Lấy danh sách sản phẩm
            var query = _products.AsQueryable();

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(p =>
                    p.Name != null &&
                    p.Name.Contains(
                        search.Keyword,
                        StringComparison.OrdinalIgnoreCase
                    )
                );
            }

            // Đếm tổng số sản phẩm
            var total = query.Count();

            // Tính tổng số trang
            var totalPages = (int)Math.Ceiling(
                (double)total / search.PageSize
            );

            // Lấy sản phẩm theo trang
            var items = query
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .ToList();

            // Truyền dữ liệu sang View
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = search.Page;
            ViewBag.Search = search;

            return View(items);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            // Tạo ID mới
            product.Id = _products.Count > 0
                ? _products.Max(p => p.Id) + 1
                : 1;

            // Thêm sản phẩm
            _products.Add(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public ActionResult Edit([FromRoute] int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromRoute] int id, [FromForm] Product updateProduct)
        {
            if (!ModelState.IsValid)
            {
                return View(updateProduct);
            }

            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin sản phẩm
            product.Name = updateProduct.Name;
            product.Price = updateProduct.Price;
            product.Description = updateProduct.Description;
            product.CategoryId = updateProduct.CategoryId;

            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Xóa sản phẩm
            _products.Remove(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
