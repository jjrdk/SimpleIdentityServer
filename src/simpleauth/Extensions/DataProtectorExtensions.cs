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

namespace SimpleAuth.Extensions
{
    using Microsoft.AspNetCore.DataProtection;
    using Shared;
    using System;
    using System.Text;

    internal static class DataProtectorExtensions
    {
        public static T Unprotect<T>(this IDataProtector dataProtector, string encoded)
        {
            var unprotected = Unprotect(dataProtector, encoded);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(unprotected);
        }

        private static string Unprotect(this IDataProtector dataProtector, string encoded)
        {
            var bytes = encoded.Base64DecodeBytes();
            var unprotectedBytes = dataProtector.Unprotect(bytes);
            var encoding = new ASCIIEncoding();
            return encoding.GetString(unprotectedBytes);
        }

        public static string Protect<T>(this IDataProtector dataProtector, T toEncode)
        {
            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(toEncode);

            var bytes = Encoding.ASCII.GetBytes(serialized);
            var protectedBytes = dataProtector.Protect(bytes);
            return Convert.ToBase64String(protectedBytes);
        }
    }
}