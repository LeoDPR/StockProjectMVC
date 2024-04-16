using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StockProject.Models
{
    public class Asignacion
    {
        [Key]
        public int Id_asignacion { get; set; }
        [Column("nombre_empleado")]
        public string? Nombre_empleado { get; set; }
        [Column("nombre_producto")]
        public string? Nombre_producto { get; set; }
        [Column("codigo_igs")]
        public string? Codigo_igs { get; set; }
        [Column("fecha")]
        public DateTime? Fecha { get; set; }
        [Column("cantidad")]
        public int? Cantidad { get; set; }
        [Column("referencia")]
        public string? Referencia { get; set; }
    }
}