﻿using System;
using System.Security.Cryptography;
using EGID.Application;

namespace EGID.Infrastructure.Security.DigitalSignature
{
    public class DigitalSignatureService : IDigitalSignatureService
    {
        public string SignHash(string base64Hash, string privateKeyXml)
        {
            var sha512HashBytes = Convert.FromBase64String(base64Hash);

            var rsa = new RSACryptoServiceProvider(4096) {PersistKeyInCsp = false};
            // import the private key used for signing the message
            rsa.FromXmlString(privateKeyXml);

            // Sign the hash, using SHA512 as the hashing algorithm
            var signatureBytes = rsa.SignHash(sha512HashBytes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        public bool VerifySignature(string dataHash, string signature, string publicKeyXml)
        {
            var dataHashBytes = Convert.FromBase64String(dataHash);
            var signatureBytes = Convert.FromBase64String(signature);

            // instantiate RSA to verify
            using var rsa = RSA.Create();

            rsa.FromXmlString(publicKeyXml);

            // Verify signature, using SHA512 as the hashing algorithm
            return rsa.VerifyHash(
                dataHashBytes,
                signatureBytes,
                HashAlgorithmName.SHA512,
                RSASignaturePadding.Pkcs1
            );
        }
    }
}