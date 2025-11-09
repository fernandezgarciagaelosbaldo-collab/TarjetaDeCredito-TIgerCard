using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TigerCard.Data;

namespace TigerCard.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UsuarioController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var idStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idStr)) return RedirectToAction("Login", "Account");

            int id = int.Parse(idStr);
            var usuario = await _db.Usuarios
                .Include(u => u.Tarjetas)
                    .ThenInclude(t => t.Transacciones)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return RedirectToAction("Login", "Account");

            return View(usuario);
        }
    }
}
