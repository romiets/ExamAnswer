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
    public class TransactionController : Controller
    {
        private BankDBContext db = new BankDBContext();
        // GET: Transaction
        public async Task<ActionResult> Index(string AccountNumber)
        {
            var transaction = db.Transactions.Where(A => A.AccountNumber == AccountNumber || A.TransferTo == AccountNumber);

            return View(await transaction.ToListAsync());
        }

        public ActionResult Create(string AccountNumber)
        {
            ViewBag.TransactionType_Id = new SelectList(db.TransactionTypes, "Id", "Name");
            ViewBag.TransferTo = new SelectList(db.Accounts, "AccountNumber", "AccountName");
            ViewBag.AccountNumber = AccountNumber;
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AccountNumber,TransactionType_Id,Amount,TransferTo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var _account = (from c in db.Accounts
                                    where c.AccountNumber == transaction.AccountNumber
                                    select c
                                  ).First();
                if (transaction.TransactionType_Id == (int)TransactionTypeEnum.Deposit)
                {
                    _account.Balance = _account.Balance + transaction.Amount;
                    transaction.TransferTo = null;
                    transaction.TransactionType = "Deposit";
                }
                else if (transaction.TransactionType_Id == (int)TransactionTypeEnum.Withdraw)
                {
                    _account.Balance = _account.Balance - transaction.Amount;
                    transaction.TransferTo = null;
                    transaction.TransactionType = "Withdraw";
                }
                else if (transaction.TransactionType_Id == (int)TransactionTypeEnum.FundTransfer)
                {
                    _account.Balance = _account.Balance - transaction.Amount;
                    var _receivingAccount = (from c in db.Accounts
                                    where c.AccountNumber == transaction.TransferTo
                                    select c
                                  ).First();
                    _receivingAccount.Balance = _receivingAccount.Balance + transaction.Amount;
                    transaction.TransactionType = "FundTransfer";
                }
                
                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }

            return View(transaction);
        }
    }
}