using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockProject.Models
{
    public class Empleados
    {
        [Key]
        public int Empleados_id { get; set; }
        [Column("no_empleado")]
        public int No_empleado { get; set; }
        [Column("nombre_empleado")]
        public string? Nombre_empleado { get; set; }
        [Column("area_trabajo")]
        public string? Area_trabajo { get; set; }
        [Column("date_added")]
        public DateTime? Date_added { get; set; }
    }
}
