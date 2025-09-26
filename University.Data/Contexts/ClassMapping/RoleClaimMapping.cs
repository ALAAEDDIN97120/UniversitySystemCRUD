using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Data.Entities.Identity;

namespace University.Data.Context.Mapping
{
    public class RoleClaimMapping : IEntityTypeConfiguration<RoleClaim>

    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable
                ("RoleClaims");
            builder.HasKey(rc => rc.Id);
            builder.Property(rc => rc.Id)
                .HasColumnName("RoleClaimId");
        }
    }
}