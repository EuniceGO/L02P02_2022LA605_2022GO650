using Microsoft.EntityFrameworkCore;

namespace L02P02_2022LA605_2022GO650.Models
{
    public class libreriaDbContext : DbContext
    {
        public libreriaDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
