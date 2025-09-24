using Microsoft.AspNetCore.Mvc;
using RestaurantOrderingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventRegistrationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrderingDbContext _context;

        public HomeController(OrderingDbContext context)
        {
            _context = context;
        }

        // Show all records (Table View)
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        // Show Details (optional)
        public IActionResult ShowDetails()
        {
            var dataList = _context.Orders.ToList();
            return View(dataList);
        }

        // Show Create Page (GET)
        public IActionResult Create()
        {
            var newData = new OrderDb
            {
                OrderId = GenerateNextId(),
                OrderNo = GetNextOrderNo()
            };

            ViewBag.PaymentMethods = new List<string>
            {
                "Cash",
                "Credit Card",
                "Mobile Pay",
                "PayPal"
            };

            return View(newData);
        }

        // Handle Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderDb data)
        {
            if (ModelState.IsValid)
            {
                // Generate OrderId if missing
                if (string.IsNullOrEmpty(data.OrderId))
                    data.OrderId = GenerateNextId();

                // Auto-increment OrderNo
                data.OrderNo = GetNextOrderNo();

                _context.Orders.Add(data);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate payment methods on error
            ViewBag.PaymentMethods = new List<string>
            {
                "Cash",
                "Credit Card",
                "Mobile Pay",
                "PayPal"
            };

            return View(data);
        }

        // Edit (GET)
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var data = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (data == null) return NotFound();

            ViewBag.PaymentMethods = new List<string>
            {
                "Cash",
                "Credit Card",
                "Mobile Pay",
                "PayPal"
            };

            return View(data);
        }

        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, OrderDb updatedData)
        {
            if (id != updatedData.OrderId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Orders.Update(updatedData);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PaymentMethods = new List<string>
            {
                "Cash",
                "Credit Card",
                "Mobile Pay",
                "PayPal"
            };

            return View(updatedData);
        }

        // Delete Page (GET)
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var data = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (data == null) return NotFound();

            return View(data);
        }

        // Delete Record (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var data = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (data != null)
            {
                _context.Orders.Remove(data);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // Generate IDs like BF00-1, BF00-2
        private string GenerateNextId()
        {
            var lastRecord = _context.Orders
                .OrderByDescending(x => x.OrderId)
                .FirstOrDefault();

            int nextNumber = 1;

            if (lastRecord != null && !string.IsNullOrEmpty(lastRecord.OrderId))
            {
                var parts = lastRecord.OrderId.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"BF00-{nextNumber}";
        }

        // Get next OrderNo as string
        private string GetNextOrderNo()
        {
            var lastOrder = _context.Orders
                .OrderByDescending(o => o.OrderNo)
                .FirstOrDefault();

            int nextOrderNo = 1;

            if (lastOrder != null && int.TryParse(lastOrder.OrderNo, out int lastNo))
            {
                nextOrderNo = lastNo + 1;
            }

            return nextOrderNo.ToString();
        }
    }
}
