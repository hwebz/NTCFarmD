using System;
using JWT;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gro.Business.DataProtection
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly string _secretKey;
        public JwtTokenGenerator(string key)
        {
            _secretKey = key;
        }

        private const string _data = nameof(_data);

        public T Decrypt<T>(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));

            var payload = JsonWebToken.DecodeToObject(token, _secretKey) as IDictionary<string, object>;
            var jsonData = (string)payload?[_data];

            var data = JsonConvert.DeserializeObject<T>(jsonData);
            return data;
        }

        public string Encrypt<T>(T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var payload = new Dictionary<string, string>
            {
                { _data, jsonData }
            };

            var token = JsonWebToken.Encode(payload, _secretKey, JwtHashAlgorithm.HS256);
            return token;
        }
    }
}
