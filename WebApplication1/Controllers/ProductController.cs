using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Authorization;
using WebApplication1.Data;
using ProjectData.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // VIEW PRODUCTS
        // =========================
        [Permission("View Products")]
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // =========================
        // VIEW PRODUCT DETAILS
        // =========================
        [Permission("View Product Details")]
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // CREATE PRODUCT (GET)
        // =========================
        [Permission("Create Product")]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE PRODUCT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Create Product")]
        public IActionResult Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Invalid product data!";
                    return View(product);
                }

                _context.Products.Add(product);
                _context.SaveChanges();

                TempData["Success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Something went wrong! Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }

        // =========================
        // EDIT PRODUCT (GET)
        // =========================
        [Permission("Update Product")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // EDIT PRODUCT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Update Product")]
        public IActionResult Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Invalid data!";
                    return View(product);
                }

                _context.Products.Update(product);
                _context.SaveChanges();

                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Something went wrong! Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }

        // =========================
        // DELETE PRODUCT (GET)
        // =========================
        [Permission("Delete Product")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // DELETE PRODUCT (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permission("Delete Product")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _context.Products.Find(id);

                if (product == null)
                {
                    TempData["Error"] = "Product not found!";
                    return RedirectToAction(nameof(Index));
                }

                _context.Products.Remove(product);
                _context.SaveChanges();

                TempData["Success"] = "Product deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Something went wrong! Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}