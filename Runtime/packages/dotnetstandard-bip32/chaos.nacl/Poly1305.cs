﻿using System;
using Chaos.NaCl.Internal;

namespace Chaos.NaCl
{
    internal sealed class Poly1305 : OneTimeAuth
    {
        public override int KeySizeInBytes
        {
            get { return 32; }
        }

        public override int SignatureSizeInBytes
        {
            get { return 16; }
        }

        public override byte[] Sign(byte[] message, byte[] key)
        {
            throw new NotSupportedException("Needs more testing");
            /*
            if (message == null)
                throw new ArgumentNullException("message");
            if (key == null)
                throw new ArgumentNullException("key");
            if (key.Length != 32)
                throw new ArgumentException("Invalid key size", "key");

            var result = new byte[16];
            Array8<UInt32> internalKey;
            ByteIntegerConverter.Array8LoadLittleEndian32(out internalKey, key, 0);
            Poly1305Donna.poly1305_auth(result, 0, message, 0, message.Length, ref internalKey);
            return result;
            */
        }

        public override void Sign(ArraySegment<byte> signature, ArraySegment<byte> message, ArraySegment<byte> key)
        {
            throw new NotSupportedException("Needs more testing");
            /*
            if (signature.Array == null)
                throw new ArgumentNullException("signature.Array");
            if (message.Array == null)
                throw new ArgumentNullException("message.Array");
            if (key.Array == null)
                throw new ArgumentNullException("key.Array");
            if (key.Count != 32)
                throw new ArgumentException("Invalid key size", "key");
            if (signature.Count != 16)
                throw new ArgumentException("Invalid signature size", "signature");

            Array8<UInt32> internalKey;
            ByteIntegerConverter.Array8LoadLittleEndian32(out internalKey, key.Array, key.Offset);
            Poly1305Donna.poly1305_auth(signature.Array, signature.Offset, message.Array, message.Offset, message.Count, ref internalKey);
            */
        }

        public override bool Verify(byte[] signature, byte[] message, byte[] key)
        {
            throw new NotSupportedException("Needs more testing");
            /*
            if (signature == null)
                throw new ArgumentNullException("signature");
            if (message == null)
                throw new ArgumentNullException("message");
            if (key == null)
                throw new ArgumentNullException("key");
            if (signature.Length != 16)
                throw new ArgumentException("Invalid signature size", "signature");
            if (key.Length != 32)
                throw new ArgumentException("Invalid key size", "key");

            var tempBytes = new byte[16];//todo: remove allocation
            Array8<UInt32> internalKey;
            ByteIntegerConverter.Array8LoadLittleEndian32(out internalKey, key, 0);
            Poly1305Donna.poly1305_auth(tempBytes, 0, message, 0, message.Length, ref internalKey);
            return CryptoBytes.ConstantTimeEquals(tempBytes, signature);
            */
        }

        public override bool Verify(ArraySegment<byte> signature, ArraySegment<byte> message, ArraySegment<byte> key)
        {
            throw new NotSupportedException("Needs more testing");
            /*
            if (signature.Array == null)
                throw new ArgumentNullException("signature.Array");
            if (message.Array == null)
                throw new ArgumentNullException("message.Array");
            if (key.Array == null)
                throw new ArgumentNullException("key.Array");
            if (key.Count != 32)
                throw new ArgumentException("Invalid key size", "key");
            if (signature.Count != 16)
                throw new ArgumentException("Invalid signature size", "signature");

            var tempBytes = new byte[16];//todo: remove allocation
            Array8<UInt32> internalKey;
            ByteIntegerConverter.Array8LoadLittleEndian32(out internalKey, key.Array, key.Offset);
            Poly1305Donna.poly1305_auth(tempBytes, 0, message.Array, message.Offset, message.Count, ref internalKey);
            return CryptoBytes.ConstantTimeEquals(new ArraySegment<byte>(tempBytes), signature);
            */
        }
    }
}
