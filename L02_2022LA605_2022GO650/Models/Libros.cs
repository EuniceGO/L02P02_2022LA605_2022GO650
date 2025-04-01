using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L02P02_2022LA605_2022GO650.Models
{

    [Table("libros")] 
    public class Libros
    {

        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Nombre del Libro")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public string? Nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
        public string? Descripcion { get; set; }

        [Display(Name = "URL de la Imagen")]
        [StringLength(255)]
        public string? UrlImagen { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Debe seleccionar un autor.")]
        public int IdAutor { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public int IdCategoria { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 999999.99, ErrorMessage = "El precio debe estar entre 0 y 999,999.99.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(1, ErrorMessage = "Solo puede contener un carácter.")]
        public string? Estado { get; set; }
    }
}
