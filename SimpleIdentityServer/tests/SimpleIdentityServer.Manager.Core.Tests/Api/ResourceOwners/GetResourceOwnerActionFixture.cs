﻿#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Moq;
using SimpleIdentityServer.Core.Models;
using SimpleIdentityServer.Core.Repositories;
using SimpleIdentityServer.Manager.Core.Api.ResourceOwners.Actions;
using SimpleIdentityServer.Manager.Core.Errors;
using SimpleIdentityServer.Manager.Core.Exceptions;
using System;
using Xunit;

namespace SimpleIdentityServer.Manager.Core.Tests.Api.ResourceOwners
{
    public class GetResourceOwnerActionFixture
    {
        private Mock<IResourceOwnerRepository> _resourceOwnerRepositoryStub;

        private IGetResourceOwnerAction _getResourceOwnerAction;

        #region Exceptions

        [Fact]
        public void When_Passing_Null_Parameter_Then_Exception_Is_Thrown()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => _getResourceOwnerAction.Execute(null));
        }

        [Fact]
        public void When_ResourceOwner_Doesnt_Exist_Then_Exception_Is_Thrown()
        {
            // ARRANGE
            const string subject = "invalid_subject";
            InitializeFakeObjects();
            _resourceOwnerRepositoryStub.Setup(r => r.GetByUniqueClaim(It.IsAny<string>()))
                .Returns((ResourceOwner)null);

            // ACT
            var exception = Assert.Throws<IdentityServerManagerException>(() => _getResourceOwnerAction.Execute(subject));

            // ASSERT
            Assert.NotNull(exception);
            Assert.True(exception.Code == ErrorCodes.InvalidRequestCode);
            Assert.True(exception.Message == string.Format(ErrorDescriptions.TheResourceOwnerDoesntExist, subject));
        }

        #endregion

        #region Happy path

        [Fact]
        public void When_Getting_Resource_Owner_Then_ResourceOwner_Is_Returned()
        {
            // ARRANGE
            const string subject = "subject";
            InitializeFakeObjects();
            _resourceOwnerRepositoryStub.Setup(r => r.GetByUniqueClaim(It.IsAny<string>()))
                .Returns(new ResourceOwner());

            // ACT
            var result = _getResourceOwnerAction.Execute(subject);

            // ASSERT
            Assert.NotNull(result);
        }

        #endregion

        #region Private methods

        private void InitializeFakeObjects()
        {
            _resourceOwnerRepositoryStub = new Mock<IResourceOwnerRepository>();
            _getResourceOwnerAction = new GetResourceOwnerAction(
                _resourceOwnerRepositoryStub.Object);
        }

        #endregion
    }
}
