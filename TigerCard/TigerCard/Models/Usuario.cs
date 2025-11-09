using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TigerCard.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public int IntentosFallidos { get; set; } = 0;
        public bool Bloqueado { get; set; } = false;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public ICollection<TarjetaCredito> Tarjetas { get; set; }
    }
}
