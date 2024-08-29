using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace hw_1_2;

public partial class TrainsContext : DbContext
{
    public TrainsContext()
    {
    }

    public TrainsContext(DbContextOptions<TrainsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Train> Trains { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Trains;User=sa;Password=admin@Admin87457;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
