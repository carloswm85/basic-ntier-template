// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitectureTemplate.ApplicationCore.Interfaces.Identity;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}
