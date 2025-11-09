using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TigerCard.Models
{
    public enum TipoTransaccion
    {
        Compra = 0,
        Pago = 1
    }

    public class Transaccion
    {
        public int Id { get; set; }

        [Required]
        public int TarjetaId { get; set; }

        [ForeignKey("TarjetaId")]
        public TarjetaCredito Tarjeta { get; set; }

        [Required]
        public TipoTransaccion Tipo { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public string Categoria { get; set; } = "";

        public string Descripcion { get; set; } = "";
    }
}
