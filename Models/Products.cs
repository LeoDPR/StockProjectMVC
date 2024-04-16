using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StockProject.Models
{
    public class Products
    {
        [Key]
        public int Id_producto { get; set; }
        [Column("codigo_producto")]
        public string? Codigo_producto { get; set; }
        [Column("nombre_producto")]
        public string? Nombre_producto { get; set; }
        [Column("date_added")]
        public DateTime Date_added { get; set; }
        [Column("precio_producto")]
        public decimal? Precio_producto { get; set; }
        [Column("stock")]
        public int? Stock { get; set; }
        [Column("id_categoria")]
        public int? Id_categoria { get; set; }

    }
}