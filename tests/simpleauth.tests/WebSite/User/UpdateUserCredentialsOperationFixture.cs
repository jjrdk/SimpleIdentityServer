﻿// Copyright © 2015 Habart Thierry, © 2018 Jacob Reimers
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

namespace SimpleAuth.Tests.WebSite.User
{
    using Moq;
    using Shared.Models;
    using Shared.Repositories;
    using SimpleAuth.WebSite.User.Actions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SimpleAuth.Shared;
    using SimpleAuth.Shared.Errors;
    using Xunit;

    public class UpdateUserCredentialsOperationFixture
    {
        private readonly Mock<IResourceOwnerRepository> _resourceOwnerRepositoryStub;
        private readonly UpdateUserCredentialsOperation _updateUserCredentialsOperation;

        public UpdateUserCredentialsOperationFixture()
        {
            _resourceOwnerRepositoryStub = new Mock<IResourceOwnerRepository>();
            _updateUserCredentialsOperation = new UpdateUserCredentialsOperation(_resourceOwnerRepositoryStub.Object);
        }

        [Fact]
        public async Task When_Passing_Null_Parameters_Then_Exceptions_Are_Thrown()
        {
            await Assert
                .ThrowsAsync<ArgumentNullException>(
                    () => _updateUserCredentialsOperation.Execute(null, null, CancellationToken.None))
                .ConfigureAwait(false);
            await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _updateUserCredentialsOperation.Execute("subject", null, CancellationToken.None))
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task When_ResourceOwner_DoesntExist_Then_Exception_Is_Thrown()
        {
            _resourceOwnerRepositoryStub.Setup(r => r.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ResourceOwner) null);

            var exception = await Assert.ThrowsAsync<SimpleAuthException>(
                    () => _updateUserCredentialsOperation.Execute("subject", "password", CancellationToken.None))
                .ConfigureAwait(false);

            Assert.Equal(ErrorCodes.InternalError, exception.Code);
            Assert.Equal(ErrorDescriptions.TheRoDoesntExist, exception.Message);
        }

        [Fact]
        public async Task When_Passing_Correct_Parameters_Then_ResourceOwnerIs_Updated()
        {
            _resourceOwnerRepositoryStub.Setup(r => r.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResourceOwner());

            await _updateUserCredentialsOperation.Execute("subject", "password", CancellationToken.None)
                .ConfigureAwait(false);

            _resourceOwnerRepositoryStub.Setup(r => r.Update(It.IsAny<ResourceOwner>(), It.IsAny<CancellationToken>()));
        }
    }
}
