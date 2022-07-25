using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using System;
using System.Security.Cryptography;
using System.Text;

namespace JoggingApp.Services
{
    public class MD5HashService : IHashService
    {
        public string Hash(string input)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash);
        }
    }
}
