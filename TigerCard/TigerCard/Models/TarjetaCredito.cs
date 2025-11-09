using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TigerCard.Models
{
    public class TarjetaCredito
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required, StringLength(16)]
        public string Numero { get; set; }

        [Required, StringLength(4)]
        public string CVV { get; set; }

        [Required]
        public DateTime FechaExpiracion { get; set; }

        [Required]
        public decimal LimiteCredito { get; set; }

        [Required]
        public decimal SaldoActual { get; set; } = 0m;

        public DateTime FechaCorte { get; set; }
        public DateTime FechaPago { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
