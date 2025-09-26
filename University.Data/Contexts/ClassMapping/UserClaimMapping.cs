using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Data.Entities.Identity;

namespace University.Data.Context.Mapping
{
    public class UserClaimMapping : IEntityTypeConfiguration<UserClaim>
    {

        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");
            builder.HasKey(uc => uc.Id);
            builder.Property(uc => uc.Id)
                .HasColumnName("UserClaimId");
        }
    }
}