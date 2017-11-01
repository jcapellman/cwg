using System;
using System.Security.Cryptography;

namespace cwg.web.Generators
{
    public abstract class BaseGenerator
    {
        public abstract (string sha1, string fileName) Generate();

        protected int getRandomInt(int min = 1, int max = 100) => new Random((int)DateTime.Now.Ticks).Next(min, max);

        protected static void FillArray(byte[] bytes)
        {
            new Random((int)DateTime.Now.Ticks).NextBytes(bytes);
        }

        protected string ComputeSha1(byte[] bytes)
        {
            using (var shaManager = new SHA1Managed())
            {
                return BitConverter.ToString(shaManager.ComputeHash(bytes)).Replace("-", "");
            }
        }
    }
}