using System.ComponentModel;
using System.Reflection;

namespace CleanArchitectureTemplate.ApplicationCore.Extensions;

/// <summary>
/// This class provides custom extension methods for and from the service layer.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Gets the description attribute value for an enum value.
    /// </summary>
    /// <param name="e">The enum value.</param>
    /// <returns>The description attribute value, OR the enum's name if no description attribute is found.</returns>
    public static string GetEnumDescription(this Enum e)
    {
        var attribute =
            e.GetType()
                .GetTypeInfo()
                .GetMember(e.ToString())
                .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

        return attribute?.Description ?? e.ToString();
    }
}
