﻿using System;
using System.Security.Cryptography;
using EGID.Application.Common.Exceptions;
using EGID.Application.Common.Interfaces;

namespace EGID.Infrastructure.Security.DigitalSignature
{
    public class DigitalSignatureService : IDigitalSignatureService
    {
        public string SignHash(string base64Hash, string privateKeyXml)
        {
            byte[] sha512HashBytes;

            try
            {
                sha512HashBytes = Convert.FromBase64String(base64Hash);
            }
            catch
            {
                throw new BadRequestException(new []{"صيغة كود الملف base64 غير صحيحة."});
            }

            var rsa = new RSACryptoServiceProvider(4096) {PersistKeyInCsp = false};
            // import the private key used for signing the message
            rsa.FromXmlString(privateKeyXml);

            // Sign the hash, using SHA512 as the hashing algorithm
            byte[] signatureBytes = rsa.SignHash(sha512HashBytes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        public bool VerifySignature(string dataHash, string signature, string publicKeyXml)
        {
            byte[] dataHashBytes;
            byte[] signatureBytes;
            try
            {
                dataHashBytes = Convert.FromBase64String(dataHash);
                signatureBytes = Convert.FromBase64String(signature);
            }
            catch
            {
                throw new BadRequestException(new []{"صيغة كود الملف base64 غير صحيحة."});
            }

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