using System.ComponentModel.DataAnnotations;

namespace BasicNLayerTemplate.MVC.Validation;

/// <summary>
/// Custom validation attribute to restrict uploaded file extensions.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class AllowedFileExtensionsAttribute : ValidationAttribute
{
    /// <summary>
    /// List of allowed file extensions (e.g., ".jpg", ".png").
    /// </summary>
    private readonly string[] _extensions;

    public AllowedFileExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    /// <summary>
    /// Validates the file extension of the uploaded file.
    /// </summary>
    /// <param name="value">The value of the field (expected to be IFormFile).</param>
    /// <param name="validationContext">Context of the validation.</param>
    /// <returns>ValidationResult.Success if valid; otherwise, an error message.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Cast the incoming value to IFormFile
        var file = value as IFormFile;

        // Validate only if a file is provided
        if (file != null)
        {
            // Extract file extension in lowercase (e.g., ".jpg")
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Check if the extension is NOT in the allowed list
            if (!_extensions.Contains(extension))
            {
                // Return validation error with allowed formats
                return new ValidationResult($"Allowed formats: {string.Join(", ", _extensions)}");
            }
        }

        // If no file or valid extension, return success
        return ValidationResult.Success;
    }
}
