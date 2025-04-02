namespace L02P02_2022LA605_2022GO650.Models
{
    public class pedido_detalle
    {
        public int id { get; set; }
        public int id_pedido {  get; set; }
        public int id_libro {  get; set; }

        public DateTime created_at { get; set; }
    }
}
