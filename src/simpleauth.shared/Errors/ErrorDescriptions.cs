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

namespace SimpleAuth.Shared.Errors
{
    internal static class ErrorDescriptions
    {
        //public const string TheClientIsNotAuthorized = "the client is not authorized";
        //public const string TheAuthorizationProcessNeedsMoreInformation = "the authorization process needs more information";
        //public const string TheResourceOwnerDidntGiveHisConsent = "require intervention of the resource owner";
        public const string TheParameterNeedsToBeSpecified = "the parameter {0} needs to be specified";
        public const string TheUrlIsNotWellFormed = "the url {0} is not well formed";
        public const string TheResourceSetCannotBeInserted = "an error occured while trying to insert the resource set";
        public const string TheResourceSetDoesntExist = "resource set {0} doesn't exist";
        public const string SomeResourcesDontExist = "some resources don't exist";
        //public const string AtLeastOneResourceDoesntExist = "at least one resource doesn't exist";
        public const string ThePolicyDoesntContainResource = "the authorization policy doesn't contain the resource";
        public const string TheResourceSetCannotBeUpdated = "resource set {0} cannot be udpated";
        public const string TheResourceSetCannotBeRemoved = "resource set {0} cannot be removed";
        public const string TheResourceSetCannotBeRetrieved = "resource set {0} cannot be retrieved";
        public const string TheResourceSetsCannotBeRetrieved = "resource sets cannot be retrieved";
        //public const string TheScopeCannotBeRetrieved = "scope cannot be retrieved";
        //public const string TheScopeCannotBeInserted = "scope cannot be inserted";
        //public const string TheScopeCannotBeUpdated = "scope cannot be updated";
        //public const string TheScopeCannotBeRemoved = "scope cannot be removed";
        //public const string TheScopesCannotBeRetrieved = "scopes cannot be retrieved";
        public const string TheScopeAreNotValid = "one or more scopes are not valid";
        //public const string TheSchemeIsNotCorrect = "authorization scheme is not correct";
        //public const string AtLeastOneTicketCannotBeInserted = "at least one ticket cannot be inserted";
        //public const string AtLeastOneTicketDoesntExist = "at least one ticket doesn't exist";
        //public const string TheTicketIssuerIsDifferentFromTheClient = "the ticket issuer is different from the client";
        public const string TheTicketIsExpired = "the ticket is expired";
        public const string TheTicketDoesntExist = "the ticket {0} doesn't exist";
        public const string TheTicketCannotBeInserted = "the ticket cannot be inserted";
        //public const string TheRptCannotBeInserted = "the rpt cannot be inserted";
        public const string ThePolicyCannotBeInserted = "the authorization policy cannot be inserted";
        public const string ThePolicyCannotBeUpdated = "the authorization policy cannot be updated";
        public const string OneOrMoreScopesDontBelongToAResourceSet = "one or more scopes don't belong to a resource set";
        public const string TheAuthorizationPolicyCannotBeRetrieved = "the authorization policy {0} cannot be retrieved";
        public const string TheAuthorizationPolicyCannotBeUpdated = "the authorization policy {0} cannot be updated";
        //public const string TheAutorizationPoliciesCannotBeRetrieved = "the authorization policies cannot be retrieved";
        //public const string TheRptsDontExist = "the rpts {0} don't exist";
        //public const string TheRptIsExpired = "the rpt is expired";
        //public const string TheAuthorizationPolicyDoesntExist = "the authorization policy {0} doesn't exist";
        //public const string TheClaimTokenIsNotValid = "the claim token parameter is not valid";
        public const string TheAuthorizationPolicyIsNotSatisfied = "the authorization policy is not satisfied";
        //public const string JwksCannotBeDeserialied = "the json web key set cannot be deserialized";
        public const string JwkIsInvalid = "the json web key set is invalid";
        //public const string CannotExtractParametersFromJsonWebKey = "cannot extract the parameters from the json web key";
        //public const string OneOfTheParameterIsNotBase64Encoded = "one of the parameter is not base64";
        //public const string TheTokenIsNotAValidJws = "the token is not a valid JWS";
        //public const string TheTokenIsNotAValidJwe = "the token is not a valid JWE";
        //public const string TheJsonWebKeyCannotBeFound = "the json web key {0} cannot be found {1}";
        //public const string TheSignatureCannotBeChecked = "the signature cannot be checked if the URI is not specified";
        //public const string TheJwsCannotBeGeneratedBecauseMissingParameters = "the jws cannot be generated because either the Url or Kid is not specified";
        //public const string TheKtyIsNotSupported = "the kty '{0}' is not supported";
        //public const string TheContentCannotBeExtractedFromJweToken = "the content cannot be extracted from the jwe token";
        public const string TheScopeDoesntExist = "the scope '{0}' doesn't exist";
        //public const string TheScopesDontExist = "the scopes '{0}' don't exist";
        //public const string TheResourceOwnerMustBeConfirmed = "the account must be confirmed";
        //public const string TheScopeAlreadyExists = "The scope {0} already exists";
        //public const string TheFileExtensionIsNotCorrect = "the file extension is not correct";
        //public const string TheFileIsNotWellFormed = "the file is not well formed";
        //public const string ClaimExists = "a claim already exists with the same name";
        //public const string ClaimDoesntExist = "the claim doesn't exist";
        //public const string CannotInsertClaimIdentifier = "cannot insert claim identifier";
        //public const string CannotRemoveClaimIdentifier = "cannot remove claim identifier";
        public const string ThePasswordCannotBeUpdated = "the password cannot be updated";
        public const string TheClaimsCannotBeUpdated = "the claims cannot be updated";
        public const string TheResourceOwnerCannotBeRemoved = "the resource owner cannot be removed";
        //public const string TheClientCannotBeUpdated = "an error occured while trying to update the client";
        //public const string TheClientCannotBeRemoved = "an error occured while trying to remove the client";
        public const string MissingParameter = "the parameter {0} is missing";
        public const string RequestIsNotValid =  "The request is not valid";
        public const string ClientIsNotValid = "the client id parameter {0} doesn't exist or is not valid";
        public const string RedirectUrlIsNotValid = "the redirect url {0} doesn't exist or is not valid";
        public const string ResourceOwnerCredentialsAreNotValid = "resource owner credentials are not valid";
        public const string ParameterIsNotCorrect = "the parameter {0} is not correct";
        public const string ScopesAreNotAllowedOrInvalid = "the scopes {0} are not allowed or invalid";
        public const string DuplicateScopeValues = "duplicate scopes {0} have been passed in parameter";
        public const string TheScopesNeedToBeSpecified = "the scope(s) {0} need(s) to be specified";
        public const string TheUserNeedsToBeAuthenticated = "the user needs to be authenticated";
        public const string TheAuthorizationRequestCannotBeProcessedBecauseThereIsNotValidPrompt =
            "the authorization request cannot be processed because there is not valid prompt";
        //public const string TheClaimCannotBeFetch = "the claims cannot be fetch";
        public const string TheUserNeedsToGiveHisConsent = "the user needs to give his consent";
        public const string ThePromptParameterIsNotSupported = "the prompt parameter {0} is not supported";
        //public const string TheUserCannotBeReauthenticated = "The user cannot be reauthenticated";
        public const string TheRedirectionUriIsNotWellFormed = "Based on the RFC-3986 the redirection-uri is not well formed";
        public const string AtLeastOneResponseTypeIsNotSupported =
            "at least one response_type parameter is not supported";
        public const string AtLeastOnePromptIsNotSupported =
            "at least one prompt parameter is not supported";
        public const string PromptParameterShouldHaveOnlyNoneValue = "prompt parameter should have only none value";
        public const string TheAuthorizationFlowIsNotSupported = "the authorization flow is not supported";
        public const string TheClientDoesntSupportTheResponseType = "the client '{0}' doesn't support the response type: '{1}'";
        public const string TheClientDoesntSupportTheGrantType = "the client {0} doesn't support the grant type {1}";
        //public const string TheIdTokenCannotBeSigned = "the id token cannot be signed";
        public const string TheClientDoesntExist = "the client doesn't exist";
        //public const string TheClientCannotBeAuthenticated = "the client cannot be authenticated";
        public const string TheClientCannotBeAuthenticatedWithSecretBasic = "the client cannot be authenticated with secret basic";
        public const string TheClientCannotBeAuthenticatedWithSecretPost = "the client cannot be authenticated with secret post";
        public const string TheClientCannotBeAuthenticatedWithTls = "the client cannot be authenticated with TLS";
        public const string TheAuthorizationCodeIsNotCorrect = "the authorization code is not correct";
        public const string TheAuthorizationCodeHasNotBeenIssuedForTheGivenClientId =
            "the authorization code has not been issued for the given client id {0}";
        public const string TheTokenHasNotBeenIssuedForTheGivenClientId = "the token has not been issued for the given client id '{0}'";
        public const string TheAuthorizationCodeIsObsolete = "the authorization code is obsolete";
        public const string TheRedirectionUrlIsNotTheSame =
            "the redirection url is not the same than the one passed in the authorization request";
        //public const string TheIdServerIsNotPresentInTheAudience = "the identity server is not present in the audience";
        //public const string TheJwtTokenHasAlreadyBeenUsed = "the jwt token has already been used";
        //public const string TheJwtTokenIsExpired = "the jwt token is expired";
        public const string TheTokenIsExpired = "the token is expired";
        public const string TheTokenIsNotValid = "the token is not valid";
        //public const string TheIssuerFromJwtIsNotCorrect = "the issuer from JWT is not correct";
        //public const string TheClientIdPassedInJwtIsNotCorrect = "the client id passed in JWT is not correct";
        //public const string TheAudiencePassedInJwtIsNotCorrect = "the audience passed in JWT is not correct";
        //public const string TheReceivedJwtHasExpired = "the received JWT has expired";
        public const string TheSignatureIsNotCorrect = "the signature is not correct";
        //public const string TheHeaderCannotBeExtractedFromJwsToken = "the header cannot be extracted from JWS token";
        public const string TheJwsPayloadCannotBeExtracted = "the jws payload cannot be extracted";
        //public const string TheHeaderCannotBeExtractedFromJweToken = "the header cannot be extracted from JWE token";
        //public const string TheJweTokenCannotBeDecrypted = "the jwe token cannot be decrypted";
        public const string TheClientAssertionIsNotAJwsToken = "the client assertion is not a JWS token";
        public const string TheClientAssertionIsNotAJweToken = "the client assertion is not a JWE token";
        //public const string TheJwsPayLoadCannotBeExtractedFromTheClientAssertion =
        //    "the jws payload cannot be extracted from the client assertion";
        //public const string TheClientAssertionCannotBeDecrypted =
        //    "the client assertion cannot be decrypted";
        public const string TheClaimIsNotValid = "the claim {0} is not valid";
        public const string TheRequestUriParameterIsNotWellFormed = "the request_uri parameter is not well formed";
        public const string TheRequestDownloadedFromRequestUriIsNotValid =
            "the request downloaded from request URI is not valid";
        public const string TheRequestParameterIsNotCorrect = "the request parameter is not correct";
        //public const string TheIdTokenHintParameterCannotBeDecrypted = "the id_token_hint parameter cannot be decrypted";
        public const string TheIdTokenHintParameterIsNotAValidToken = "the id_token_hint parameter is not a valid token";
        //public const string TheSignatureOfIdTokenHintParameterCannotBeChecked = "the signature of id token hint parameter cannot be checked";
        public const string TheIdentityTokenDoesntContainSimpleAuthAsAudience = "The identity token does not contain SimpleAuth in the audience";
        public const string TheCurrentAuthenticatedUserDoesntMatchWithTheIdentityToken = "the current authenticated user doesn't match with the identity token";
        public const string TheResponseCannotBeGeneratedBecauseResourceOwnerNeedsToBeAuthenticated =
            "the response cannot be generated because the resource owner needs to be authenticated";
        public const string TheRedirectUrlIsNotValid = "the redirect_uri {0} is not well formed";
        public const string TheRedirectUrlCannotContainsFragment = "The redirect_uri {0} cannot contain fragment";
        //public const string TheRedirectUriParameterIsNotValid = "one or more redirect_uri values are invalid";
        //public const string TheRedirectUriContainsAFragment = "one or more redirect_uri contains a fragment";
        //public const string TheJwksParameterCannotBeSetBecauseJwksUrlIsUsed =
        //    "the jwks parameter cannot be set because the Jwks Url has already been set";
        public const string TheParameterIsTokenEncryptedResponseAlgMustBeSpecified =
            "the parameter id_token_encrypted_response_alg must be specified";
        public const string TheParameterUserInfoEncryptedResponseAlgMustBeSpecified =
            "the parameter userinfo_encrypted_response_alg must be specified";
        public const string TheParameterRequestObjectEncryptionAlgMustBeSpecified =
            "the parameter request_object_encryption_alg must be specified";
        public const string OneOfTheRequestUriIsNotValid = "one of the request_uri is not valid";
        public const string TheSectorIdentifierUrisCannotBeRetrieved = "the sector identifier uris cannot be retrieved";
        public const string OneOrMoreSectorIdentifierUriIsNotARedirectUri =
            "one or more sector uri is not a redirect_uri";
        public const string TheRefreshTokenIsNotValid = "the refresh token is not valid";
        public const string TheRequestCannotBeExtractedFromTheCookie =
            "the request cannot be extracted from the cookie";
        public const string AnErrorHasBeenRaisedWhenTryingToAuthenticate =
            "an error {0} has been raised when trying to authenticate";
        //public const string TheLoginInformationCannotBeExtracted = "the login information cannot be extracted";
        //public const string TheResourceOwnerCredentialsAreNotCorrect = "the resource owner credentials are not correct";
        //public const string TheExternalProviderIsNotSupported = "the external provider {0} is not supported";
        //public const string NoSubjectCanBeExtracted = "no subject can be extracted";
        public const string TheTokenDoesntExist = "the token doesn't exist";
        //public const string TheRoCannotBeCreated = "the resource owner cannot be created because subject is missing";
        public const string TheSubjectCannotBeRetrieved = "the subject cannot be retrieved";
        public const string TheRoDoesntExist = "the resource owner doesn't exist";
        //public const string TheRoWithCredentialsAlreadyExists = "a resource owner with same credentials already exists";
        //public const string TheAccountHasAlreadyBeenActivated = "the accout has already been activated";
        public const string TwoFactorAuthenticationCannotBePerformed = "two factor authentication cannot be performed";
        public const string TwoFactorAuthenticationIsNotEnabled = "two factor authentication is not enabled";
        public const string TheConfirmationCodeCannotBeSaved = "the confirmation code cannot be saved";
        //public const string TheExternalResourceOwnerIsNotAuthenticated = "the 'external' resource owner is not authenticated";
        //public const string TheResourceOwnerIsNotALocalAccount = "the resource owner is not a local account";
        public const string TheClientIdDoesntExist = "the client id {0} doesn't exist";
        public const string TheClientDoesntContainASharedSecret = "the client {0} doesn't have a shared secret";
        public const string TheClientRequiresPkce = "the client {0} requires PKCE";
        public const string TheCodeVerifierIsNotCorrect = "the code verifier is not correct";
        //public const string TheBulkPublicClaimsIsNotWorking = "the bulk operation is not working ...";
        public const string TheResourceOwnerDoesntExist = "The resource owner {0} doesn't exist";
        public const string TheProfileAlreadyLinked = "the profile is already linked to your account";
        public const string NotAuthorizedToRemoveTheProfile = "not authorized to remove the profile";
        //public const string TheScimConfigurationMustBeSpecified = "the scim configuration is not correct";
        public const string TheExternalAccountAccountCannotBeUnlinked = "the external account cannot be unlinked";
        public const string TheAmrDoesntExist = "the amr {0} doesn't exist";
        public const string NoActiveAmr = "no active AMR";
        //public const string TheUserIsNotAuthorized = "the user is not authorized";
        //public const string TheResourceOwnerCannotBeAdded = "An error occured while trying to insert the resource owner";
        public const string TheRefreshTokenCanBeUsedOnlyByTheSameIssuer = "the refresh token can be used only by the same issuer";
        //public const string TheTokenIsNotAValidResourceOwnerToken = "not a valid resource owner token";
    }
}