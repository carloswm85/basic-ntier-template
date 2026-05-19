using Microsoft.EntityFrameworkCore;

namespace ComplexNLayerTemplate.Data.Model;

/// <summary>
/// Partial class for ComplexNLayerTemplateContext.
///
/// - This file content is intentionally left minimal.
/// - All content here is deleted and replaced when scafolding from the database.
/// - DO NOT use this file for customizations. Instead use additional partial class files.
/// - This is the convention.
/// </summary>
public partial class ComplexNLayerTemplateDbContext : DbContext
{
    public ComplexNLayerTemplateDbContext() { }

    public ComplexNLayerTemplateDbContext(DbContextOptions<ComplexNLayerTemplateDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    /// <summary>
    /// Use this method for extending the DbContext’s model configuration without modifying the generated file.
    /// This is the convention.
    /// </summary>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
