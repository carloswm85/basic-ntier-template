// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanArchitectureTemplate.Web.Models.ViewModels.Identity.Manage;

public class ConfigureTwoFactorViewModel
{
    public string SelectedProvider { get; set; } = default!;

    public ICollection<SelectListItem> Providers { get; set; } = [];
}
