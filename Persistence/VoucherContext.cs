using Domain.Model;
using Domain.Model.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence
{
    public class VoucherContext : DbContext
    {
        public DbSet<Voucher> Vouchers { get; set; }

        public VoucherContext()
        {
        }

        public VoucherContext(DbContextOptions<VoucherContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetAuditProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new VoucherConfiguration());
            builder.ApplyConfiguration(new RedemptionConfiguration());
        }


        private void SetAuditProperties()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditEntity && x.State == EntityState.Added);
            var now = DateTimeOffset.UtcNow;

            foreach (var entity in entities)
            {
                var auditEntity = (IAuditEntity) entity.Entity;

                if (entity.State == EntityState.Added)
                {
                    auditEntity.CreatedAt = now;
                }
                else
                {
                    Entry(auditEntity).Property(x => x.CreatedAt).IsModified = false;
                }
            }
        }
    }

    public class VoucherContextFactory : IDesignTimeDbContextFactory<VoucherContext>
    {
        public VoucherContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VoucherContext>();
            optionsBuilder.UseSqlServer(args[0]);
            return new VoucherContext(optionsBuilder.Options);
        }
    }
}
