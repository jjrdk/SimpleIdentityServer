﻿namespace SimpleIdentityServer.Core.WebSite.User.Actions
{
    using System.Threading.Tasks;

    public interface IRemoveConsentOperation
    {
        Task<bool> Execute(string consentId);
    }
}