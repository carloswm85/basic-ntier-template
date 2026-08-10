using System.ComponentModel.DataAnnotations;
using CleanArchitectureTemplate.Web.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CleanArchitectureTemplate.Web.Annotations;

public sealed class GenericCompareAttribute : ValidationAttribute, IClientModelValidator
{
    private GenericCompareOperator operatorname = GenericCompareOperator.GreaterThanOrEqual;

    public string CompareToPropertyName { get; set; } = string.Empty;

    public GenericCompareOperator OperatorName
    {
        get => operatorname;
        set => operatorname = value;
    }

    public GenericCompareAttribute()
        : base() { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var basePropertyInfo = validationContext.ObjectType.GetProperty(CompareToPropertyName);

        if (basePropertyInfo == null)
        {
            return new ValidationResult($"Property '{CompareToPropertyName}' was not found.");
        }

        var valOther = basePropertyInfo.GetValue(validationContext.ObjectInstance) as IComparable;

        var valThis = value as IComparable;

        if (valThis == null)
        {
            return ValidationResult.Success;
        }

        if (valOther == null)
        {
            return ValidationResult.Success;
        }

        bool isInvalid =
            (operatorname == GenericCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0)
            || (
                operatorname == GenericCompareOperator.GreaterThanOrEqual
                && valThis.CompareTo(valOther) < 0
            )
            || (operatorname == GenericCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0)
            || (
                operatorname == GenericCompareOperator.LessThanOrEqual
                && valThis.CompareTo(valOther) > 0
            );

        return isInvalid ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        MergeAttribute(context.Attributes, "data-val", "true");

        MergeAttribute(
            context.Attributes,
            "data-val-genericcompare",
            FormatErrorMessage(context.ModelMetadata.GetDisplayName())
        );

        MergeAttribute(
            context.Attributes,
            "data-val-genericcompare-comparetopropertyname",
            CompareToPropertyName
        );

        MergeAttribute(
            context.Attributes,
            "data-val-genericcompare-operatorname",
            OperatorName.ToString()
        );
    }

    private static bool MergeAttribute(
        IDictionary<string, string> attributes,
        string key,
        string value
    )
    {
        if (attributes.ContainsKey(key))
        {
            return false;
        }

        attributes.Add(key, value);
        return true;
    }
}
