using System.ComponentModel.DataAnnotations;

namespace L02P02_2022LA605_2022GO650.Models
{
    public class clientes
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string apellido { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo inválido.")]
        public string email {  get; set; }
        public string direccion {  get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
