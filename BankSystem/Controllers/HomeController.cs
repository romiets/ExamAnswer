using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankSystem.DAL;
using BankSystem.Models;
using System.Data.Entity.Infrastructure;

namespace BankSystem.Controllers
{
    public class HomeController : Controller
    {
        private BankDBContext db = new BankDBContext();
        // GET: Account
        public async Task<ActionResult> Index()
        {
            var accounts = db.Accounts;
            return View(await accounts.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Commenting out original code to show how to use a raw SQL query.
            //Department department = await db.Departments.FindAsync(id);

            // Create and execute raw SQL query.
            string query = "SELECT * FROM Accounts WHERE Id = @p0";
            Account account = await db.Accounts.SqlQuery(query, id).SingleOrDefaultAsync();

            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Account/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AccountNumber,AccountName,Password,Balance,CreatedDate")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Account/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                return HttpNotFound();
            }
      
            return View(account);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "AccountNumber", "AccountName", "Password", "Balance", "CreatedDate", "RowVersion" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var accountToUpdate = await db.Accounts.FindAsync(id);
            if (accountToUpdate == null)
            {
                Account deletedAccount = new Account();
                TryUpdateModel(deletedAccount, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The account was deleted by another user.");

                return View(deletedAccount);
            }

            if (TryUpdateModel(accountToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(accountToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Account)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The account was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Account)databaseEntry.ToObject();

                        if (databaseValues.AccountNumber != clientValues.AccountNumber)
                            ModelState.AddModelError("AccountNumber", "Current value: "
                                + databaseValues.AccountNumber);
                        if (databaseValues.AccountName != clientValues.AccountName)
                            ModelState.AddModelError("AccountName", "Current value: "
                                + databaseValues.AccountName);
                        if (databaseValues.Password != clientValues.Password)
                            ModelState.AddModelError("Password", "Current value: "
                                + databaseValues.Password);
                        if (databaseValues.Balance != clientValues.Balance)
                            ModelState.AddModelError("Balance", "Current value: "
                                + String.Format("{0:c}", databaseValues.Balance));
                        if (databaseValues.CreatedDate != clientValues.CreatedDate)
                            ModelState.AddModelError("CreatedDate", "Current value: "
                                + String.Format("{0:d}", databaseValues.CreatedDate));
                        
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        accountToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(accountToUpdate);
        }

        // GET: Account/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = await db.Accounts.FindAsync(id);
            if (account == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Account account)
        {
            try
            {
                db.Entry(account).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = account.Id });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(account);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}