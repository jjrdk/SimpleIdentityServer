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

using Moq;
using SimpleIdentityServer.Core.Parameters;
using SimpleIdentityServer.Core.WebSite.Authenticate;
using SimpleIdentityServer.Core.WebSite.Authenticate.Actions;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Core.UnitTests.WebSite.Authenticate
{
    public sealed class AuthenticateActionsFixture
    {
        private Mock<IAuthenticateResourceOwnerOpenIdAction> _authenticateResourceOwnerActionFake;
        private Mock<ILocalOpenIdUserAuthenticationAction> _localOpenIdUserAuthenticationActionFake;
        private Mock<IGenerateAndSendCodeAction> _generateAndSendCodeActionStub;
        private Mock<IValidateConfirmationCodeAction> _validateConfirmationCodeActionStub;
        private Mock<IRemoveConfirmationCodeAction> _removeConfirmationCodeActionStub;
        private IAuthenticateActions _authenticateActions;

        [Fact]
        public async Task When_Passing_Null_AuthorizationParameter_To_The_Action_AuthenticateResourceOwner_Then_Exception_Is_Thrown()
        {            InitializeFakeObjects();
            var authorizationParameter = new AuthorizationParameter();

                        await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticateActions.AuthenticateResourceOwnerOpenId(null, null, null, null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticateActions.AuthenticateResourceOwnerOpenId(authorizationParameter, null, null, null)).ConfigureAwait(false);
        }

        [Fact]
        public async Task When_Passing_Null_LocalAuthenticateParameter_To_The_Action_LocalUserAuthentication_Then_Exception_Is_Thrown()
        {            InitializeFakeObjects();
            var localAuthenticationParameter = new LocalAuthenticationParameter();

                        await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticateActions.LocalOpenIdUserAuthentication(null, null, null, null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticateActions.LocalOpenIdUserAuthentication(localAuthenticationParameter, null, null, null)).ConfigureAwait(false);
        }

        [Fact]
        public async Task When_Passing_Parameters_Needed_To_The_Action_AuthenticateResourceOwner_Then_The_Action_Is_Called()
        {            InitializeFakeObjects();
            var authorizationParameter = new AuthorizationParameter
            {
                ClientId = "clientId"
            };
            var claimsPrincipal = new ClaimsPrincipal();

                        await _authenticateActions.AuthenticateResourceOwnerOpenId(authorizationParameter, claimsPrincipal, null, null).ConfigureAwait(false);

                        _authenticateResourceOwnerActionFake.Verify(a => a.Execute(authorizationParameter, claimsPrincipal, null, null));
        }

        [Fact]
        public async Task When_Passing_Parameters_Needed_To_The_Action_LocalUserAuthentication_Then_The_Action_Is_Called()
        {            InitializeFakeObjects();
            var authorizationParameter = new AuthorizationParameter
            {
                ClientId = "clientId"
            };
            var localUserAuthentication = new LocalAuthenticationParameter
            {
                UserName = "userName"
            };


                        await _authenticateActions.LocalOpenIdUserAuthentication(localUserAuthentication, authorizationParameter, null, null).ConfigureAwait(false);

                        _localOpenIdUserAuthenticationActionFake.Verify(a => a.Execute(localUserAuthentication, 
                authorizationParameter,
                null, null));
        }
        
        private void InitializeFakeObjects()
        {
            _authenticateResourceOwnerActionFake = new Mock<IAuthenticateResourceOwnerOpenIdAction>();
            _localOpenIdUserAuthenticationActionFake = new Mock<ILocalOpenIdUserAuthenticationAction>();
            _generateAndSendCodeActionStub = new Mock<IGenerateAndSendCodeAction>();
            _validateConfirmationCodeActionStub = new Mock<IValidateConfirmationCodeAction>();
            _removeConfirmationCodeActionStub = new Mock<IRemoveConfirmationCodeAction>();
            _authenticateActions = new AuthenticateActions(
                _authenticateResourceOwnerActionFake.Object,
                _localOpenIdUserAuthenticationActionFake.Object,
                _generateAndSendCodeActionStub.Object,
                _validateConfirmationCodeActionStub.Object,
                _removeConfirmationCodeActionStub.Object);
        }
    }
}
