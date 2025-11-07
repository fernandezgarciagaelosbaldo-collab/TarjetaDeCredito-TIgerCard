using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TigerCard.Controllers
{
    public class TransaccionController : Controller
    {
        // GET: TransaccionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TransaccionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TransaccionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransaccionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TransaccionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TransaccionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TransaccionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TransaccionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
