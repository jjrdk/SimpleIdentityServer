﻿using SimpleIdentityServer.Uma.Common.DTOs;

namespace SimpleIdentityServer.Uma.Client.Results
{
    using SimpleAuth.Shared;

    public class GetPolicyResult : BaseResponse
    {
        public PolicyResponse Content { get; set; }
    }
}
