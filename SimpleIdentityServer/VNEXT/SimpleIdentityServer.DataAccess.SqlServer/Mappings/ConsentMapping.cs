﻿using System.Data.Entity.ModelConfiguration;
using SimpleIdentityServer.DataAccess.SqlServer.Models;

namespace SimpleIdentityServer.DataAccess.SqlServer.Mappings
{
    public sealed class ConsentMapping : EntityTypeConfiguration<Consent>
    {
        public ConsentMapping()
        {
            ToTable("consents");
            HasKey(c => c.Id);
            HasRequired(c => c.Client)
                .WithMany(c => c.Consents);
            HasRequired(c => c.ResourceOwner)
                .WithMany(r => r.Consents);
            HasMany(c => c.GrantedScopes)
                .WithMany(s => s.Consents)
                .Map(c =>
                {
                    c.MapLeftKey("ConsentId");
                    c.MapRightKey("ScopeName");
                    c.ToTable("consentScopes");
                });
            HasMany(c => c.Claims)
                .WithMany(c => c.Consents)
                .Map(c =>
                {
                    c.MapLeftKey("ConsentId");
                    c.MapRightKey("ClaimCode");
                    c.ToTable("consentClaims");
                });
        }
    }
}
