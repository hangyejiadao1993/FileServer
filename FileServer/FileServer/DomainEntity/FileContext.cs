using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.DomainEntity
{
    public class FileContext:DbContext
    {
        public FileContext(DbContextOptions  options) :base(options)
        {
             
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<FileData> FileDatas { get; set; }
    }
}
