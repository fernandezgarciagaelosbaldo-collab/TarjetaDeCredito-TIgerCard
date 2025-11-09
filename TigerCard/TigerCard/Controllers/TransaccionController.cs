using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TigerCard.Data;
using TigerCard.Models;

namespace TigerCard.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TransaccionController(ApplicationDbContext db) { _db = db; }

        [HttpGet]
        public IActionResult Create()
        {
            var idStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idStr)) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(decimal monto, string descripcion, string categoria)
        {
            var idStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idStr)) return RedirectToAction("Login", "Account");
            int usuarioId = int.Parse(idStr);

            // buscamos la primera tarjeta del usuario
            var tarjeta = await _db.Tarjetas
                .Where(t => t.UsuarioId == usuarioId)
                .Include(t => t.Transacciones)
                .FirstOrDefaultAsync();

            if (tarjeta == null)
            {
                ModelState.AddModelError("", "No hay tarjeta asociada.");
                return View();
            }

            if (monto <= 0)
            {
                ModelState.AddModelError("", "El monto debe ser mayor a 0.");
                return View();
            }

            var disponible = tarjeta.LimiteCredito - tarjeta.SaldoActual;
            if (monto > disponible)
            {
                ModelState.AddModelError("", $"Límite insuficiente. Disponible: {disponible:C}");
                return View();
            }

            var trans = new Transaccion
            {
                TarjetaId = tarjeta.Id,
                Tipo = TipoTransaccion.Compra,
                Monto = monto,
                Descripcion = descripcion ?? "",
                Categoria = categoria ?? ""
            };

            tarjeta.SaldoActual += monto;
            _db.Transacciones.Add(trans);
            _db.Tarjetas.Update(tarjeta);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Compra registrada: {monto:C}";
            return RedirectToAction("Index", "Usuario");
        }
    }
}
