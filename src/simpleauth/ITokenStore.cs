﻿namespace SimpleAuth
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;
    using Shared.Models;

    public interface ITokenStore
    {
        /// <summary>
        /// Try to get a valid access token.
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="clientId"></param>
        /// <param name="idTokenJwsPayload"></param>
        /// <param name="userInfoJwsPayload"></param>
        /// <returns></returns>
        Task<GrantedToken> GetToken(string scopes, string clientId, JwtPayload idTokenJwsPayload, JwtPayload userInfoJwsPayload);
        Task<GrantedToken> GetRefreshToken(string getRefreshToken);
        Task<GrantedToken> GetAccessToken(string accessToken);
        Task<bool> AddToken(GrantedToken grantedToken);
        Task<bool> RemoveRefreshToken(string refreshToken);
        Task<bool> RemoveAccessToken(string accessToken);
        Task<bool> Clean();
    }
}