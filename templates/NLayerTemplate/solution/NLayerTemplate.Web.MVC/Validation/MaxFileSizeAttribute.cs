using System.ComponentModel.DataAnnotations;

namespace ComplexNLayerTemplate.MVC.Validation;

/// <summary>
/// Custom validation attribute to restrict the maximum file size of uploaded files.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class MaxFileSizeAttribute : ValidationAttribute
{
    /// <summary>
    /// Maximum allowed file size in bytes.
    /// </summary>
    private readonly int _maxSize;

    public MaxFileSizeAttribute(int maxSize)
    {
        _maxSize = maxSize;
    }

    /// <summary>
    /// Validates that the uploaded file does not exceed the maximum size.
    /// </summary>
    /// <param name="value">The value of the field (expected to be IFormFile).</param>
    /// <param name="validationContext">Context of the validation.</param>
    /// <returns>ValidationResult.Success if valid; otherwise, an error message.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Cast the incoming value to IFormFile
        var file = value as IFormFile;

        // Validate only if a file is provided and exceeds max size
        if (file != null && file.Length > _maxSize)
        {
            // Return validation error with max size in MB
            // Convert bytes to megabytes for user-friendly message
            // _maxSize / 1024 into kylobytes / 1024 into megabytes
            return new ValidationResult($"Max file size is {_maxSize / 1024 / 1024} MB");
        }

        // If no file or size is within limits, return success
        return ValidationResult.Success;
    }
}
