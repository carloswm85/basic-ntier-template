using Microsoft.EntityFrameworkCore;

/// <summary>
/// Partial class for BasicNtierTemplateContext.
/// 
/// - This file content is intentionally left minimal.
/// - All content here is deleted and replaced when scafolding from the database.
/// - DO NOT use this file for customizations. Instead use additional partial class files.
/// - This is the convention.
/// </summary>
public partial class BasicNtierTemplateContext : DbContext
{
    public BasicNtierTemplateContext()
    {
    }

    public BasicNtierTemplateContext(DbContextOptions<BasicNtierTemplateContext> options)
        : base(options)
    {
    }

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
