﻿namespace SimpleAuth.Api.Profile.Actions
{
    using System;
    using System.Threading.Tasks;
    using Exceptions;
    using Shared.Repositories;

    internal sealed class UnlinkProfileAction : IUnlinkProfileAction
    {
        private readonly IResourceOwnerRepository _resourceOwnerRepository;
        private readonly IProfileRepository _profileRepository;

        public UnlinkProfileAction(IResourceOwnerRepository resourceOwnerRepository, IProfileRepository profileRepository)
        {
            _resourceOwnerRepository = resourceOwnerRepository;
            _profileRepository = profileRepository;
        }

        public async Task<bool> Execute(string localSubject, string externalSubject)
        {
            if (string.IsNullOrWhiteSpace(localSubject))
            {
                throw new ArgumentNullException(nameof(localSubject));
            }

            if (string.IsNullOrWhiteSpace(externalSubject))
            {
                throw new ArgumentNullException(nameof(externalSubject));
            }

            var resourceOwner = await _resourceOwnerRepository.Get(localSubject).ConfigureAwait(false);
            if (resourceOwner == null)
            {
                throw new IdentityServerException(
                    Errors.ErrorCodes.InternalError,
                    string.Format(Errors.ErrorDescriptions.TheResourceOwnerDoesntExist, localSubject));
            }

            var profile = await _profileRepository.Get(externalSubject).ConfigureAwait(false);
            if (profile == null || profile.ResourceOwnerId != localSubject)
            {
                throw new IdentityServerException(Errors.ErrorCodes.InternalError, Errors.ErrorDescriptions.NotAuthorizedToRemoveTheProfile);
            }

            if (profile.Subject == localSubject)
            {
                throw new IdentityServerException(Errors.ErrorCodes.InternalError, Errors.ErrorDescriptions.TheExternalAccountAccountCannotBeUnlinked);
            }

            return await _profileRepository.Remove(new[] { externalSubject }).ConfigureAwait(false);
        }
    }
}
