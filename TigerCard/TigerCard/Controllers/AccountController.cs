using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TigerCard.Data;
using TigerCard.Models;
using TigerCard.Models.ViewModels;

namespace TigerCard.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        private const int MAX_INTENTOS = 3;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _db.Usuarios.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("", "El correo ya está registrado.");
                return View(vm);
            }

            var usuario = new Usuario
            {
                Nombre = vm.Nombre,
                Email = vm.Email
            };

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, vm.Password);

            // Añadimos usuario y tarjeta por defecto
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var tarjeta = new TarjetaCredito
            {
                UsuarioId = usuario.Id,
                Numero = GenerarNumeroTarjeta(),
                CVV = GenerarCVV(),
                FechaExpiracion = DateTime.UtcNow.AddYears(4),
                LimiteCredito = 20000m,
                SaldoActual = 0m,
                FechaCorte = DateTime.UtcNow.Date.AddDays(25),
                FechaPago = DateTime.UtcNow.Date.AddDays(55)
            };

            _db.Tarjetas.Add(tarjeta);
            await _db.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(vm);
            }

            if (usuario.Bloqueado)
            {
                ModelState.AddModelError("", "Cuenta bloqueada. Espera o contacta soporte.");
                return View(vm);
            }

            var verify = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, vm.Password);
            if (verify == PasswordVerificationResult.Success)
            {
                usuario.IntentosFallidos = 0;
                await _db.SaveChangesAsync();

                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                usuario.IntentosFallidos++;
                if (usuario.IntentosFallidos >= MAX_INTENTOS)
                {
                    usuario.Bloqueado = true;
                    usuario.IntentosFallidos = 0;
                    ModelState.AddModelError("", "Has excedido 3 intentos. Cuenta bloqueada.");
                }
                else
                {
                    ModelState.AddModelError("", $"Contraseña incorrecta. Intentos: {usuario.IntentosFallidos}/{MAX_INTENTOS}");
                }

                await _db.SaveChangesAsync();
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UsuarioId");
            return RedirectToAction("Login");
        }

        private string GenerarNumeroTarjeta()
        {
            var rnd = new Random();
            return string.Concat(Enumerable.Range(0, 16).Select(_ => rnd.Next(0, 10).ToString()));
        }

        private string GenerarCVV()
        {
            var rnd = new Random();
            return rnd.Next(100, 999).ToString();
        }
    }
}
