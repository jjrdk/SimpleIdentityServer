﻿namespace SimpleIdentityServer.Uma.Core.Api.PolicyController.Actions
{
    using System.Threading.Tasks;
    using Parameters;

    public interface IAddResourceSetToPolicyAction
    {
        Task<bool> Execute(AddResourceSetParameter addResourceSetParameter);
    }
}