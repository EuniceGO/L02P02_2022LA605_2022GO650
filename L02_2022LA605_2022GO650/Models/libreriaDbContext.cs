﻿using Microsoft.EntityFrameworkCore;

namespace L02P02_2022LA605_2022GO650.Models
{
    public class libreriaDbContext : DbContext
    {
        public libreriaDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Libros> Libros { get; set; }

        public DbSet<clientes> clientes { get; set; }
        public DbSet<pedido_encabezado> pedido_encabezado { get; set; }

        public DbSet<pedido_detalle> pedido_detalle { get; set; }
    }
}
