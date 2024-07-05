using financial_application.Models;
using Microsoft.AspNetCore.Mvc;

namespace financial_application.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var list = _context.Accounts.ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                // Set success message in TempData
                TempData["SuccessMessage"] = "Account created successfully!";
                return RedirectToAction("Index");
            }
            return View(account);
        }

    }
}
