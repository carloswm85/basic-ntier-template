// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitectureTemplate.Web.Models.ViewModels.Identity.Manage;

public class RemoveLoginViewModel
{
    public string LoginProvider { get; set; } = default!;
    public string ProviderKey { get; set; } = default!;
}
