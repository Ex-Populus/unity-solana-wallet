using Merkator.BitCoin;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;

namespace Solnet.Programs
{
    public static class TokenProgram
    {
        /// <summary>
        /// The base58 encoder instance.
        /// </summary>
        //private static readonly Base58Encoder Encoder = new Base58Encoder();

        /// <summary>
        /// The address of the Token Program.
        /// </summary>
        public const string ProgramId = "TokenkegQfeZyiNwAJbNbGKPFXCWuBvf9Ss623VQ5DA";

        /// <summary>
        /// The public key of the sysvar rent.
        /// </summary>
        public static string SysvarRentPublicKey = "SysvarRent111111111111111111111111111111111";

        /// <summary>
        /// Mint account ccount layout size.
        /// </summary>
        public const int MintAccountDataSize = 82;

        /// <summary>
        /// Transfers tokens from one account to another either directly or via a delegate.
        /// If this account is associated with the native mint then equal amounts of SOL and Tokens will be transferred to the destination account.
        /// </summary>
        /// <param name="source">The account to transfer tokens from. (not wallet public address)</param>
        /// <param name="destination">The account to transfer tokens to. (not wallet public address)</param>
        /// <param name="amount">The amount of tokens to transfer.</param>
        /// <param name="owner">The account owner. (wallet public key that holds the token)</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction Transfer(
            string source, string destination, long amount, string owner)
        {
            return Transfer(Base58Encoding.Decode(source), Base58Encoding.Decode(destination), amount, Base58Encoding.Decode(owner));
        }

        public static TransactionInstructionForJS TransferForJS(
           string source, string destination, long amount, string owner)
        {
            return TransferForJS(Base58Encoding.Decode(source), Base58Encoding.Decode(destination), amount, Base58Encoding.Decode(owner));
        }

        /// <summary>
        /// Transfers tokens from one account to another either directly or via a delegate.
        /// If this account is associated with the native mint then equal amounts of SOL and Tokens will be transferred to the destination account.
        /// </summary>
        /// <param name="source">The account to transfer tokens from. (not wallet public address)</param>
        /// <param name="destination">The account to transfer tokens to. (not wallet public address)</param>
        /// <param name="amount">The amount of tokens to transfer.</param>
        /// <param name="owner">The account owner. (wallet public key that holds the token)</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction Transfer(
            byte[] source, byte[] destination, long amount, byte[] owner)
        {
            var keys = new List<AccountMeta>
            {
                new AccountMeta(source, false, true),
                new AccountMeta(destination, false, true),
                new AccountMeta(owner, true, false)
            };

            var transactionData = EncodeTransferData(amount);

            return new TransactionInstruction
            {
                ProgramId = Base58Encoding.Decode(ProgramId),
                Keys = keys,
                Data = transactionData
            };
        }

        public static TransactionInstructionForJS TransferForJS(
           byte[] source, byte[] destination, long amount, byte[] owner)
        {
            var keys = new List<AccountMetaForJS>
            {
                new AccountMetaForJS(source, false, true),
                new AccountMetaForJS(destination, false, true),
                new AccountMetaForJS(owner, true, false)
            };

            var transactionData = EncodeTransferData(amount);

            return new TransactionInstructionForJS
            {
                programId = Base58Encoding.Decode(ProgramId),
                keys = keys,
                data = transactionData
            };
        }

        /// <summary>
        /// <para>
        /// Transfers tokens from one account to another either directly or via a delegate.
        /// If this account is associated with the native mint then equal amounts of SOL and Tokens will be transferred to the destination account.
        /// </para>
        /// <para>
        /// This instruction differs from Transfer in that the token mint and decimals value is checked by the caller.
        /// This may be useful when creating transactions offline or within a hardware wallet.
        /// </para>
        /// </summary>
        /// <param name="source">The account to transfer tokens from.</param>
        /// <param name="destination">The account to transfer tokens to.</param>
        /// <param name="amount">The amount of tokens to transfer.</param>
        /// <param name="decimals">The token decimals.</param>
        /// <param name="owner">The account owner.</param>
        /// <param name="tokenMint">The token mint.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction TransferChecked(
            string source, string destination, long amount, byte decimals, string owner, string tokenMint)
        {
            return TransferChecked(
                Base58Encoding.Decode(source),
                Base58Encoding.Decode(destination),
                amount,
                decimals,
                Base58Encoding.Decode(owner),
                Base58Encoding.Decode(tokenMint));
        }


        /// <summary>
        /// <para>
        /// Transfers tokens from one account to another either directly or via a delegate.
        /// If this account is associated with the native mint then equal amounts of SOL and Tokens will be transferred to the destination account.
        /// </para>
        /// <para>
        /// This instruction differs from Transfer in that the token mint and decimals value is checked by the caller.
        /// This may be useful when creating transactions offline or within a hardware wallet.
        /// </para>
        /// </summary>
        /// <param name="source">The account to transfer tokens from.</param>
        /// <param name="destination">The account to transfer tokens to.</param>
        /// <param name="amount">The amount of tokens to transfer.</param>
        /// <param name="decimals">The token decimals.</param>
        /// <param name="owner">The account owner.</param>
        /// <param name="tokenMint">The token mint.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction TransferChecked(
            byte[] source, byte[] destination, long amount, byte decimals, byte[] owner, byte[] tokenMint)
        {
            var keys = new List<AccountMeta>
            {
                new AccountMeta(source, false, true),
                new AccountMeta(tokenMint, false, false),
                new AccountMeta(destination, false, true),
                new AccountMeta(owner, true, false)
            };

            var transactionData = EncodeTransferCheckedData(amount, decimals);

            return new TransactionInstruction
            {
                ProgramId = Base58Encoding.Decode(ProgramId),
                Keys = keys,
                Data = transactionData
            };
        }

        /// <summary>
        /// <para>Initializes a new account to hold tokens.
        /// If this account is associated with the native mint then the token balance of the initialized account will be equal to the amount of SOL in the account.
        /// If this account is associated with another mint, that mint must be initialized before this command can succeed.
        /// </para>
        /// <para>
        /// The InitializeAccount instruction requires no signers and MUST be included within the same Transaction
        /// as the system program's <see cref="SystemProgram.CreateAccount(string, string, long, long, string)"/> or <see cref="SystemProgram.CreateAccount(byte[], byte[], long, long, byte[])"/>
        /// instruction that creates the account being initialized.
        /// Otherwise another party can acquire ownership of the uninitialized account.
        /// </para>
        /// </summary>
        /// <param name="account">The account to initialize.</param>
        /// <param name="mint">The token mint.</param>
        /// <param name="owner">The account to set as owner of the initialized account.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction InitializeAccount(string account, string mint, string owner)
        {
            return InitializeAccount(
                Base58Encoding.Decode(account),
                Base58Encoding.Decode(mint),
                Base58Encoding.Decode(owner));
        }

        public static TransactionInstructionForJS InitializeAccountForJS(string account, string mint, string owner)
        {
            return InitializeAccountForJS(
                Base58Encoding.Decode(account),
                Base58Encoding.Decode(mint),
                Base58Encoding.Decode(owner));
        }

        /// <summary>
        /// <para>Initializes a new account to hold tokens.
        /// If this account is associated with the native mint then the token balance of the initialized account will be equal to the amount of SOL in the account.
        /// If this account is associated with another mint, that mint must be initialized before this command can succeed.
        /// </para>
        /// <para>
        /// The InitializeAccount instruction requires no signers and MUST be included within the same Transaction
        /// as the system program's <see cref="SystemProgram.CreateAccount(string, string, long, long, string)"/> or <see cref="SystemProgram.CreateAccount(byte[], byte[], long, long, byte[])"/>
        /// instruction that creates the account being initialized.
        /// Otherwise another party can acquire ownership of the uninitialized account.
        /// </para>
        /// </summary>
        /// <param name="account">The account to initialize.</param>
        /// <param name="mint">The token mint.</param>
        /// <param name="owner">The account to set as owner of the initialized account.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction InitializeAccount(byte[] account, byte[] mint, byte[] owner)
        {
            var keys = new List<AccountMeta>
            {
                new AccountMeta(account, false, true),
                new AccountMeta(mint, false, false),
                new AccountMeta(owner, false, false),
                new AccountMeta(Base58Encoding.Decode(SysvarRentPublicKey), false, false)
            };

            return new TransactionInstruction()
            {
                ProgramId = Base58Encoding.Decode(ProgramId),
                Keys = keys,
                Data = EncodeInitializeAccountData()
            };
        }

        public static TransactionInstructionForJS InitializeAccountForJS(byte[] account, byte[] mint, byte[] owner)
        {
            var keys = new List<AccountMetaForJS>
            {
                new AccountMetaForJS(account, false, true),
                new AccountMetaForJS(mint, false, false),
                new AccountMetaForJS(owner, false, false),
                new AccountMetaForJS(Base58Encoding.Decode(SysvarRentPublicKey), false, false)
            };

            return new TransactionInstructionForJS()
            {
                programId = Base58Encoding.Decode(ProgramId),
                keys = keys,
                data = EncodeInitializeAccountData()
            };
        }

        /// <summary>
        /// Initialize a transaction to initialize a token mint account.
        /// </summary>
        /// <param name="mint">The token mint.</param>
        /// <param name="decimals">The token decimals.</param>
        /// <param name="mintAuthority">The token mint authority.</param>
        /// <param name="freezeAuthority">The token freeze authority.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction InitializeMint(string mint, int decimals, string mintAuthority,
            string freezeAuthority = "")
        {
            return InitializeMint(
                Base58Encoding.Decode(mint),
                decimals,
                Base58Encoding.Decode(mintAuthority),
                !string.IsNullOrWhiteSpace(freezeAuthority) ? Base58Encoding.Decode(mintAuthority) : null);
        }

        /// <summary>
        /// Initialize a transaction to initialize a token mint account.
        /// </summary>
        /// <param name="mint">The token mint.</param>
        /// <param name="decimals">The token decimals.</param>
        /// <param name="mintAuthority">The token mint authority.</param>
        /// <param name="freezeAuthority">The token freeze authority.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction InitializeMint(byte[] mint, int decimals, byte[] mintAuthority, byte[] freezeAuthority = null)
        {
            var keys = new List<AccountMeta>
            {
                new AccountMeta(mint, false, true),
                new AccountMeta(Base58Encoding.Decode(SysvarRentPublicKey), false, false)
            };

            var freezeAuthorityOpt = freezeAuthority != null ? 1 : 0;

            if (freezeAuthority == null)
            { 
                freezeAuthority = new Account().PublicKey;
            }

            var txData = EncodeInitializeMintData(
                mintAuthority,
                freezeAuthority,
                decimals,
                freezeAuthorityOpt);

            return new TransactionInstruction()
            {
                ProgramId = Base58Encoding.Decode(ProgramId),
                Keys = keys,
                Data = txData
            };
        }

        /// <summary>
        /// Initializes a transaction to mint tokens to a destination account.
        /// </summary>
        /// <param name="mint">The token mint.</param>
        /// <param name="destination">The account to mint tokens to.</param>
        /// <param name="amount">The amount of tokens.</param>
        /// <param name="mintAuthority">The token mint authority.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction MintTo(string mint, string destination, long amount, string mintAuthority)
        {
            return MintTo(
                Base58Encoding.Decode(mint),
                Base58Encoding.Decode(destination),
                amount,
                Base58Encoding.Decode(mintAuthority));
        }

        /// <summary>
        /// Initializes a transaction to mint tokens to a destination account.
        /// </summary>
        /// <param name="mint">The token mint.</param>
        /// <param name="destination">The account to mint tokens to.</param>
        /// <param name="amount">The amount of tokens.</param>
        /// <param name="mintAuthority">The token mint authority.</param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction MintTo(byte[] mint, byte[] destination, long amount, byte[] mintAuthority)
        {
            var keys = new List<AccountMeta>
            {
                new AccountMeta(mint, false, true),
                new AccountMeta(destination, false, true),
                new AccountMeta(mintAuthority, true, false)
            };

            byte[] data = EncodeMintToData(amount);

            return new TransactionInstruction()
            {
                ProgramId = Base58Encoding.Decode(ProgramId),
                Keys = keys,
                Data = data
            };
        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="TokenProgramInstructions.InitializeAccount"/> method.
        /// </summary>
        /// <returns>The byte array with the encoded data.</returns>
        private static byte[] EncodeInitializeAccountData() => new[] { (byte)TokenProgramInstructions.InitializeAccount };

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="TokenProgramInstructions.InitializeMint"/> method.
        /// </summary>
        /// <param name="mintAuthority">The mint authority for the token.</param>
        /// <param name="freezeAuthority">The freeze authority for the token.</param>
        /// <param name="decimals">The amount of decimals.</param>
        /// <param name="freezeAuthorityOption">The freeze authority option for the token.</param>
        /// <remarks>The <c>freezeAuthorityOption</c> parameter is related to the existence or not of a freeze authority.</remarks>
        /// <returns>The byte array with the encoded data.</returns>
        private static byte[] EncodeInitializeMintData(
            byte[] mintAuthority, byte[] freezeAuthority, int decimals, int freezeAuthorityOption)
        {
            var methodBuffer = new byte[67];

            methodBuffer[0] = (byte)TokenProgramInstructions.InitializeMint;
            methodBuffer[1] = (byte)decimals;
            Array.Copy(mintAuthority, 0, methodBuffer, 2, 32);
            methodBuffer[34] = (byte)freezeAuthorityOption;
            Array.Copy(freezeAuthority, 0, methodBuffer, 35, 32);

            return methodBuffer;
        }


        /// <summary>
        /// Encode the transaction instruction data for the <see cref="TokenProgramInstructions.Transfer"/> method.
        /// </summary>
        /// <param name="amount">The amount of tokens.</param>
        /// <returns>The byte array with the encoded data.</returns>
        private static byte[] EncodeTransferData(long amount)
        {
            var methodBuffer = new byte[9];

            methodBuffer[0] = (byte)TokenProgramInstructions.Transfer;
            Utils.Int64ToByteArrayLe(amount, methodBuffer, 1);

            return methodBuffer;
        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="TokenProgramInstructions.TransferChecked"/> method.
        /// </summary>
        /// <param name="amount">The amount of tokens.</param>
        /// <param name="decimals">The number of decimals of the token.</param>
        /// <returns>The byte array with the encoded data.</returns>
        private static byte[] EncodeTransferCheckedData(long amount, byte decimals)
        {
            var methodBuffer = new byte[10];

            methodBuffer[0] = (byte)TokenProgramInstructions.TransferChecked;
            Utils.Int64ToByteArrayLe(amount, methodBuffer, 1);
            methodBuffer[9] = decimals;

            return methodBuffer;
        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="TokenProgramInstructions.MintTo"/> method.
        /// </summary>
        /// <param name="amount">The amount of tokens.</param>
        /// <returns>The byte array with the encoded data.</returns>
        private static byte[] EncodeMintToData(long amount)
        {
            var methodBuffer = new byte[9];

            methodBuffer[0] = (byte)TokenProgramInstructions.MintTo;
            Utils.Int64ToByteArrayLe(amount, methodBuffer, 1);

            return methodBuffer;
        }
    }
}