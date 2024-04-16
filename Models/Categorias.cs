using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StockProject.Models
{
    public class Categorias
    {
        [Key]
        public int Id_categoria { get; set; }
        [Column("nombre_categoria")]
        public string? Nombre_categoria { get; set; }
        [Column("descripcion_categoria")]
        public string? Descripcion_categoria { get; set; }
        [Column("date_added")]
        public DateTime? Date_added { get; set; }
    }
}