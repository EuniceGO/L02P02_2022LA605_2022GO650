using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L02P02_2022LA605_2022GO650.Models
{
    public class Libros
    {
        [Key]
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Nombre del Libro")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public string? nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
        public string? descripcion { get; set; }

        [Display(Name = "URL de la Imagen")]
        [StringLength(255)]
        public string? url_imagen { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Debe seleccionar un autor.")]
        public int id_autor { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public int id_categoria { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 999999.99, ErrorMessage = "El precio debe estar entre 0 y 999,999.99.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal precio { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(1, ErrorMessage = "Solo puede contener un carácter.")]
        public string? estado { get; set; }
    }
}
