namespace CleanArchitectureTemplate.ApplicationCore.Constants;

/// <summary>
/// Provides constant values for common operation names and role identifiers used throughout the application.
/// </summary>
/// <remarks>
/// This class defines string constants for standard operations such as create, read, update, delete,
/// approve, and reject, as well as role names for access control. These constants can be used to ensure consistency
/// when referring to operations and roles in authorization checks or logging.
/// </remarks>
public static class OperationConstants
{
    // Operations
    public static readonly string CreateOperationName = "Create";
    public static readonly string ReadOperationName = "Read";
    public static readonly string UpdateOperationName = "Update";
    public static readonly string DeleteOperationName = "Delete";
    public static readonly string ApproveOperationName = "Approve";
    public static readonly string RejectOperationName = "Reject";

    // Roles
    public static readonly string SuperAdministratorsRole = "SuperAdmin";
    public static readonly string AdministratorsRole = "Admin";
    public static readonly string ManagersRole = "Manager";
    public static readonly string UsersRole = "User";
}
