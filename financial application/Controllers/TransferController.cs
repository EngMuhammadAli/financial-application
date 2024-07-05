using financial_application.Models;
using Microsoft.AspNetCore.Mvc;

namespace financial_application.Controllers
{
    public class TransferController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display transfer form
        public IActionResult Transfer()
        {
            ViewBag.Accounts = _context.Accounts.ToList();
            return View();
        }

        // Process transfer
        [HttpPost]
        public async Task<IActionResult> Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts.FindAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
            {
                TempData["SuccessMessage"] = "Invalid account details.!";

                ViewBag.Accounts = _context.Accounts.ToList();
                return View();
            }

            if (fromAccount.Balance < amount)
            {
                TempData["SuccessMessage"] = "Insufficient balance.!";
                ViewBag.Accounts = _context.Accounts.ToList();
                return View();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    fromAccount.Balance -= amount;
                    toAccount.Balance += amount;

                    _context.Transactions.Add(new Transaction
                    {
                        FromAccountId = fromAccountId,
                        ToAccountId = toAccountId,
                        Amount = amount,
                        TransactionDate = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    TempData["SuccessMessage"] = "Transaction completed successfully!";

                }
                catch
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Transaction failed. Please try again.");
                    ViewBag.Accounts = _context.Accounts.ToList();
                    return View();
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }

}
