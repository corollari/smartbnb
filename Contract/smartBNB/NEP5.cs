using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.ComponentModel;
using System.Numerics;
using Helper = Neo.SmartContract.Framework.Helper;

namespace NEP5
{
    public class NEP5 : SmartContract
    {
		[Serializable]
        struct Balance
        {
            public BigInteger amount;
            public BigInteger lastTimeTransfered;
        }

        [DisplayName("transfer")]
        public static event Action<byte[], byte[], BigInteger> Transferred;

        private static readonly byte[] Owner = "Ad1HKAATNmFT5buNgSxspbW68f4XVSssSw".ToScriptHash(); //Owner Address

        public static object Main(string method, object[] args)
        {
            if (Runtime.Trigger == TriggerType.Application)
            {
                var callscript = ExecutionEngine.CallingScriptHash;

                if (method == "balanceOf") return BalanceOf((byte[])args[0]);

                if (method == "decimals") return Decimals();

                if (method == "name") return Name();

                if (method == "symbol") return Symbol();

                if (method == "supportedStandards") return SupportedStandards();

                if (method == "totalSupply") return TotalSupply();

                if (method == "transfer") return Transfer((byte[])args[0], (byte[])args[1], (BigInteger)args[2], callscript);
            }
            return false;
        }

		private static Balance deserializeBalance(byte[] balance){
            if (balance.Length != 0){
				return new Balance() { amount = 0, lastTimeTransfered = 0 };
			} else {
				return (Balance)Helper.Deserialize(balance);
			}
		}

		private static BigInteger max(BigInteger a, BigInteger b){
			if(a >= b)
				return a;
			else
				return b;
		}

		private static BigInteger updateAmount(BigInteger amount, BigInteger lastTransfer, BigInteger currentTime){
			BigInteger deltaTime = currentTime - lastTransfer;

			// Code generated with NEP5interest.py, it essentially calculates the interest as in percentage^deltaTime in logarithmic time by using pre-computed constants
			byte[] rateDenominatorBytes = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
			BigInteger rateDenominator = rateDenominatorBytes.AsBigInteger();

			byte[] magnitudeBytes = {0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
			BigInteger magnitude = magnitudeBytes.AsBigInteger();
			while(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xa0, 0x9b, 0x94, 0x83, 0x2d, 0xbc, 0x75, 0x3c, 0x44, 0xd3, 0xb6, 0xd1, 0x26, 0x7c, 0x40, 0xb0, 0x3f, 0xe0, 0x44, 0xdb, 0x8a, 0xb3, 0x52, 0x90, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x1a, 0x59, 0x5e, 0x72, 0xd7, 0xe8, 0xdf, 0xbc, 0xf0, 0xaa, 0xba, 0x6e, 0x7c, 0x0a, 0x4b, 0xaf, 0x06, 0x91, 0xb0, 0x52, 0x79, 0x41, 0x23, 0xc8, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x5c, 0xff, 0x37, 0xaa, 0x6b, 0x1e, 0xcb, 0xc0, 0xf4, 0x1e, 0x3c, 0x12, 0xd2, 0xf3, 0x84, 0xf0, 0xdf, 0x74, 0x94, 0x0d, 0x7f, 0x1a, 0x10, 0xe4, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x50, 0x62, 0xbb, 0x40, 0x66, 0xb1, 0x00, 0xfb, 0x8f, 0xb5, 0xe7, 0x34, 0x16, 0x58, 0xd2, 0x00, 0xfe, 0x31, 0xb6, 0xcc, 0xaa, 0xab, 0x07, 0xf2, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x44, 0xea, 0x2c, 0xca, 0x2c, 0xad, 0xed, 0xe9, 0xc7, 0x37, 0x3c, 0x76, 0x8c, 0x7a, 0xb7, 0x07, 0x46, 0x91, 0x69, 0x8d, 0x6f, 0xbd, 0x03, 0xf9, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x39, 0x53, 0xdb, 0x08, 0x3d, 0x63, 0xb7, 0x80, 0xba, 0x98, 0x6b, 0x97, 0x37, 0x68, 0x86, 0x2c, 0xc9, 0x53, 0x2a, 0x3b, 0x9e, 0xd8, 0x81, 0xfc, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x2d, 0x9a, 0x32, 0x75, 0xee, 0x14, 0xee, 0x94, 0x3d, 0x69, 0x7e, 0x2a, 0x86, 0x7c, 0x07, 0x02, 0x2d, 0xc2, 0x08, 0xb8, 0xc8, 0xea, 0x40, 0xfe, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xc0, 0x21, 0xfa, 0x09, 0x53, 0xf1, 0xd7, 0x73, 0xba, 0x8e, 0x3d, 0x75, 0xac, 0xc6, 0x1e, 0x3c, 0x56, 0x0d, 0x4c, 0xc2, 0x02, 0x75, 0x20, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x2a, 0xb1, 0x14, 0x4a, 0x0b, 0xb9, 0x00, 0x25, 0x4e, 0x1d, 0x91, 0x55, 0x03, 0x27, 0x10, 0x9d, 0x78, 0x4a, 0xad, 0xfa, 0x68, 0x3a, 0x90, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x79, 0x53, 0xff, 0x83, 0xf4, 0x9d, 0x76, 0x27, 0x85, 0xa4, 0x19, 0x73, 0x80, 0x7b, 0x12, 0xde, 0x46, 0x21, 0xb7, 0x63, 0x2e, 0x1d, 0xc8, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xe6, 0xdf, 0x4f, 0xbe, 0x0b, 0xf4, 0x94, 0x37, 0x52, 0xf5, 0xba, 0x11, 0x66, 0x4b, 0xc1, 0xf0, 0x08, 0x85, 0x73, 0xab, 0x95, 0x0e, 0xe4, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xfd, 0x87, 0x91, 0xa2, 0xa9, 0x79, 0x73, 0x6d, 0x07, 0x50, 0x1f, 0x2c, 0x48, 0x1a, 0x07, 0x34, 0x4a, 0xba, 0x1f, 0x74, 0x4a, 0x07, 0xf2, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x4a, 0x51, 0x22, 0xa9, 0xf1, 0xd6, 0xdc, 0x57, 0xd0, 0xa4, 0x76, 0x98, 0x99, 0x14, 0x4c, 0x14, 0x6c, 0x5a, 0xa9, 0x21, 0xa5, 0x03, 0xf9, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xa6, 0xb6, 0xbb, 0x6b, 0x3e, 0xc7, 0x57, 0x77, 0x6e, 0x22, 0xf2, 0xda, 0x36, 0x87, 0x23, 0x7a, 0x72, 0x0c, 0xbb, 0x8a, 0xd2, 0x81, 0xfc, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x1b, 0x3c, 0xae, 0x8a, 0x76, 0x5a, 0x5c, 0x2d, 0x70, 0x65, 0x2e, 0xb1, 0x13, 0x86, 0x3e, 0xaf, 0x05, 0x1e, 0xd7, 0x43, 0xe9, 0x40, 0xfe, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xbf, 0x93, 0xd4, 0xb8, 0x2b, 0xaf, 0x2b, 0xfe, 0x7f, 0x66, 0x28, 0x64, 0x8d, 0x1f, 0xf0, 0x9e, 0xf5, 0xf4, 0x89, 0xa1, 0x74, 0x20, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0x09, 0xe8, 0xac, 0x10, 0x67, 0x3d, 0xaf, 0xe1, 0x0a, 0x20, 0x77, 0x20, 0x5c, 0xfc, 0xa4, 0x76, 0xf7, 0x93, 0xac, 0x50, 0x3a, 0x90, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xda, 0x42, 0x53, 0xdd, 0xa0, 0x79, 0x05, 0x16, 0x1e, 0xc1, 0x06, 0x7b, 0x01, 0xd0, 0xc8, 0xe3, 0x5a, 0x30, 0x50, 0x28, 0x1d, 0xc8, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xe9, 0x72, 0x61, 0x04, 0x61, 0xfc, 0x77, 0xf4, 0xad, 0xa5, 0xf8, 0x51, 0x4b, 0x5f, 0xd7, 0x3b, 0xc5, 0x91, 0x26, 0x94, 0x0e, 0xe4, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}
			magnitude /= 2;
			if(deltaTime >= magnitude){
				byte[] rateNumeratorBytes = {0xbe, 0x5c, 0x22, 0x78, 0x58, 0x23, 0x75, 0x77, 0xdb, 0xb5, 0xcf, 0x04, 0xd3, 0x19, 0x63, 0x90, 0x48, 0xe7, 0x12, 0x4a, 0x07, 0xf2, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				BigInteger rateNumerator = rateNumeratorBytes.AsBigInteger();
				amount = (amount * rateNumerator) / rateDenominator;
				deltaTime -= magnitude;
			}

			amount = max(amount, 1); //Needed?
			return amount;
		}

		private static Balance updateBalance(Balance bal){
			BigInteger currentTime = Runtime.Time;
			bal.amount = updateAmount(bal.amount, bal.lastTimeTransfered, currentTime);
			bal.lastTimeTransfered = currentTime;
			return bal;
		}

        [DisplayName("balanceOf")]
        public static BigInteger BalanceOf(byte[] account)
        {
            if (account.Length != 20)
                throw new InvalidOperationException("The parameter account SHOULD be 20-byte addresses.");
            StorageMap asset = Storage.CurrentContext.CreateMap(nameof(asset));
            Balance bal = deserializeBalance(asset.Get(account));
			return updateAmount(bal.amount, bal.lastTimeTransfered, Runtime.Time);
        }

        [DisplayName("decimals")]
        public static byte Decimals() => 8;

        private static bool IsPayable(byte[] to)
        {
            var c = Blockchain.GetContract(to);
            return c == null || c.IsPayable;
        }

        [DisplayName("name")]
        public static string Name() => "GinoMo"; //name of the token

        [DisplayName("symbol")]
        public static string Symbol() => "GM"; //symbol of the token

        [DisplayName("supportedStandards")]
        public static string[] SupportedStandards() => new string[] { "NEP-5", "NEP-7", "NEP-10" };

        [DisplayName("totalSupply")]
        public static BigInteger TotalSupply()
        {
            StorageMap contract = Storage.CurrentContext.CreateMap(nameof(contract));
            return contract.Get("totalSupply").AsBigInteger();
        }
#if DEBUG
        [DisplayName("transfer")] //Only for ABI file
        public static bool Transfer(byte[] from, byte[] to, BigInteger amount) => true;
#endif
        //Methods of actual execution
        private static bool Transfer(byte[] from, byte[] to, BigInteger amount, byte[] callscript)
        {
            //Check parameters
            if (from.Length != 20 || to.Length != 20)
                throw new InvalidOperationException("The parameters from and to SHOULD be 20-byte addresses.");
            if (amount <= 0)
                throw new InvalidOperationException("The parameter amount MUST be greater than 0.");
            if (!IsPayable(to))
                return false;
            if (!Runtime.CheckWitness(from) && from.AsBigInteger() != callscript.AsBigInteger())
                return false;
            StorageMap asset = Storage.CurrentContext.CreateMap(nameof(asset));
            Balance fromBalance = updateBalance(deserializeBalance(asset.Get(from)));
            if (fromBalance.amount < amount)
                return false;
            if (from == to)
                return true;

            //Reduce payer balances
            if (fromBalance.amount == amount){
                asset.Delete(from);
			} else {
				fromBalance.amount = fromBalance.amount - amount;
                asset.Put(from, Helper.Serialize(fromBalance));
			}

            //Increase the payee balance
            Balance toBalance = updateBalance(deserializeBalance(asset.Get(to)));
			toBalance.amount = toBalance.amount + amount;
            asset.Put(to, Helper.Serialize(toBalance));

            Transferred(from, to, amount);
            return true;
        }
    }
}
