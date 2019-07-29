using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NHG.Core.Functions
{
    public class EncDec
    {

        // Encrypt a byte array into a byte array using a key and an IV
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {

            // Create a MemoryStream that is going to accept the encrypted bytes

            MemoryStream ms = new MemoryStream();
            // Create a symmetric algorithm.

            // We are going to use Rijndael because it is strong and available on all platforms.

            // You can use other algorithms, to do so substitute the next line with something like

            //                      TripleDES alg = TripleDES.Create();

            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV.

            // We need the IV (Initialization Vector) because the algorithm is operating in its default

            // mode called CBC (Cipher Block Chaining). The IV is XORed with the first block (8 byte)

            // of the data before it is encrypted, and then each encrypted block is XORed with the

            // following block of plaintext. This is done to make encryption more secure.

            // There is also a mode called ECB which does not need an IV, but it is much less secure.

            alg.Key = Key;

            alg.IV = IV;



            // Create a CryptoStream through which we are going to be pumping our data.

            // CryptoStreamMode.Write means that we are going to be writing data to the stream

            // and the output will be written in the MemoryStream we have provided.

            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);



            // Write the data and make it do the encryption

            cs.Write(clearData, 0, clearData.Length);



            // Close the crypto stream (or do FlushFinalBlock).

            // This will tell it that we have done our encryption and there is no more data coming in,

            // and it is now a good time to apply the padding and finalize the encryption process.

            cs.Close();



            // Now get the encrypted data from the MemoryStream.

            // Some people make a mistake of using GetBuffer() here, which is not the right way.

            byte[] encryptedData = ms.ToArray();



            return encryptedData;

        }
        // Encrypt a string into a string using a password

        //    Uses Encrypt(byte[], byte[], byte[])
        public static string Encrypt(string clearText, string Password)
        {

            // First we need to turn the input string into a byte array.

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);



            // Then, we need to turn the password into Key and IV

            // We are using salt to make it harder to guess our key using a dictionary attack -

            // trying to guess a password by enumerating all possible words.

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });



            // Now get the key/IV and do the encryption using the function that accepts byte arrays.

            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key

            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.

            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael.

            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size.

            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes.

            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));



            // Now we need to turn the resulting byte array into a string.

            // A common mistake would be to use an Encoding class for that. It does not work

            // because not all byte values can be represented by characters.

            // We are going to be using Base64 encoding that is designed exactly for what we are

            // trying to do.

            return Convert.ToBase64String(encryptedData);



        }
        /// <summary>
        /// ma hoa kieu co dien
        /// </summary>
        /// <param name="_strTemp">Chuoi can truyen vao</param>
        /// <returns></returns>
        public static string EncodeOld(string _strTemp)
        {
            char[] _strArr = _strTemp.ToCharArray();
            _strTemp = "";
            for (int i = 0; i < _strArr.Length; i++)
                if (i % 2 == 0)
                    _strTemp += _strArr[i];
            for (int i = 0; i < _strArr.Length; i++)
                if (i % 2 == 1)
                    _strTemp += _strArr[i];
            return _strTemp;
        }
        // Encrypt bytes into bytes using a password

        //    Uses Encrypt(byte[], byte[], byte[])
        public static byte[] Encrypt(byte[] clearData, string Password)
        {

            // We need to turn the password into Key and IV.

            // We are using salt to make it harder to guess our key using a dictionary attack -

            // trying to guess a password by enumerating all possible words.

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });



            // Now get the key/IV and do the encryption using the function that accepts byte arrays.

            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key

            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.

            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael.

            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size.

            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes.

            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));



        }
        // Encrypt a file into another file using a password
        public static void Encrypt(string fileIn, string fileOut, string Password)
        {

            // First we are going to open the file streams

            FileStream fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read);

            FileStream fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);



            // Then we are going to derive a Key and an IV from the Password and create an algorithm

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });



            Rijndael alg = Rijndael.Create();



            alg.Key = pdb.GetBytes(32);

            alg.IV = pdb.GetBytes(16);



            // Now create a crypto stream through which we are going to be pumping data.

            // Our fileOut is going to be receiving the encrypted bytes.

            CryptoStream cs = new CryptoStream(fsOut, alg.CreateEncryptor(), CryptoStreamMode.Write);



            // Now will will initialize a buffer and will be processing the input file in chunks.

            // This is done to avoid reading the whole file (which can be huge) into memory.

            int bufferLen = 4096;

            byte[] buffer = new byte[bufferLen];

            int bytesRead;



            do
            {

                // read a chunk of data from the input file

                bytesRead = fsIn.Read(buffer, 0, bufferLen);



                // encrypt it

                cs.Write(buffer, 0, bytesRead);



            } while (bytesRead != 0);



            // close everything

            cs.Close(); // this will also close the unrelying fsOut stream

            fsIn.Close();

        }
        // Decrypt a byte array into a byte array using a key and an IV
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {

            // Create a MemoryStream that is going to accept the decrypted bytes

            MemoryStream ms = new MemoryStream();



            // Create a symmetric algorithm.

            // We are going to use Rijndael because it is strong and available on all platforms.

            // You can use other algorithms, to do so substitute the next line with something like

            //                      TripleDES alg = TripleDES.Create();

            Rijndael alg = Rijndael.Create();



            // Now set the key and the IV.

            // We need the IV (Initialization Vector) because the algorithm is operating in its default

            // mode called CBC (Cipher Block Chaining). The IV is XORed with the first block (8 byte)

            // of the data after it is decrypted, and then each decrypted block is XORed with the previous

            // cipher block. This is done to make encryption more secure.

            // There is also a mode called ECB which does not need an IV, but it is much less secure.

            alg.Key = Key;

            alg.IV = IV;



            // Create a CryptoStream through which we are going to be pumping our data.

            // CryptoStreamMode.Write means that we are going to be writing data to the stream

            // and the output will be written in the MemoryStream we have provided.

            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);



            // Write the data and make it do the decryption

            cs.Write(cipherData, 0, cipherData.Length);



            // Close the crypto stream (or do FlushFinalBlock).

            // This will tell it that we have done our decryption and there is no more data coming in,

            // and it is now a good time to remove the padding and finalize the decryption process.

            cs.Close();



            // Now get the decrypted data from the MemoryStream.

            // Some people make a mistake of using GetBuffer() here, which is not the right way.

            byte[] decryptedData = ms.ToArray();



            return decryptedData;

        }
        // Decrypt a string into a string using a password

        //    Uses Decrypt(byte[], byte[], byte[])
        public static string Decrypt(string cipherText, string Password)
        {

            // First we need to turn the input string into a byte array.

            // We presume that Base64 encoding was used

            byte[] cipherBytes = Convert.FromBase64String(cipherText);



            // Then, we need to turn the password into Key and IV

            // We are using salt to make it harder to guess our key using a dictionary attack -

            // trying to guess a password by enumerating all possible words.

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });



            // Now get the key/IV and do the decryption using the function that accepts byte arrays.

            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key

            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.

            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael.

            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size.

            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes.

            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));



            // Now we need to turn the resulting byte array into a string.

            // A common mistake would be to use an Encoding class for that. It does not work

            // because not all byte values can be represented by characters.

            // We are going to be using Base64 encoding that is designed exactly for what we are

            // trying to do.

            return Encoding.Unicode.GetString(decryptedData);



        }
        // Decrypt bytes into bytes using a password

        //    Uses Decrypt(byte[], byte[], byte[])
        public static byte[] Decrypt(byte[] cipherData, string Password)
        {

            // We need to turn the password into Key and IV.

            // We are using salt to make it harder to guess our key using a dictionary attack -

            // trying to guess a password by enumerating all possible words.

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });



            // Now get the key/IV and do the Decryption using the function that accepts byte arrays.

            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key

            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.

            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael.

            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size.

            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes.

            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));

        }
        // Decrypt a file into another file using a password
        public static void Decrypt(string fileIn, string fileOut, string Password)
        {

            // First we are going to open the file streams

            FileStream fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read);

            FileStream fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);



            // Then we are going to derive a Key and an IV from the Password and create an algorithm

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });



            Rijndael alg = Rijndael.Create();



            alg.Key = pdb.GetBytes(32);

            alg.IV = pdb.GetBytes(16);



            // Now create a crypto stream through which we are going to be pumping data.

            // Our fileOut is going to be receiving the Decrypted bytes.

            CryptoStream cs = new CryptoStream(fsOut, alg.CreateDecryptor(), CryptoStreamMode.Write);



            // Now will will initialize a buffer and will be processing the input file in chunks.

            // This is done to avoid reading the whole file (which can be huge) into memory.

            int bufferLen = 4096;

            byte[] buffer = new byte[bufferLen];

            int bytesRead;



            do
            {

                // read a chunk of data from the input file

                bytesRead = fsIn.Read(buffer, 0, bufferLen);



                // Decrypt it

                cs.Write(buffer, 0, bytesRead);



            } while (bytesRead != 0);



            // close everything

            cs.Close(); // this will also close the unrelying fsOut stream

            fsIn.Close();

        }
        /// <summary>
        /// Encrypt with key = hrms
        /// </summary>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public static string Encrypt(string cipherText)
        {
            return Encrypt(cipherText, "hrms");
        }
        /// <summary>
        /// Encrypt with key = hrms
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, "hrms");
        }
        /// <summary>
        /// Encode width  MD5
        /// </summary>
        /// <param name="originalPassword"></param>
        /// <returns></returns>
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes).Replace("-", "");
        }

        #region | - Encode crypto - |

        private static string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
        private static string[] Keys = new string[4]{
         "98tvajsdhg!8034y0h0~3uhf0cu3H40TUH30H2FLKJ+GEU502*9029+8H20495*",
         "0984tpeo^rhg09WHIN2043HNTX098~H98X07H98M942-9x8h9mg-8g598xm9521",
         "NVLIDSJnv0w45uv045vlskj~vo8uh0837013509*yrehfaiu!oiudfsjp098sdh",
         "*+pninutcpiu##piucpoieupcoun409*(13485c0439856c084n5609834750c!"
         };
        private static char[] arrCharSet;
        private static string[] arrKeys;

        public static string EncodeCrypto(string str)
        {
            string EncodedStr = ""; int CharPos = 0;
            for (int i = 0; i < str.Length; i++)
            {
                CharPos = CharSet.IndexOf(str.Substring(i, 1));
                for (int j = 0; j < Keys.Length; j++) { EncodedStr += Keys[j].Substring(CharPos, 1); }
            }
            return EncodedStr;
        }
        public static string DecodeCrypto(string str)
        {
            string DecodedStr = ""; string CodeStr = "";
            arrCharSet = CharSet.ToCharArray();
            arrKeys = LoadKeyArray();
            for (int i = 0; i < str.Length / Keys.Length; i++)
            {
                CodeStr = str.Substring((Keys.Length * i), Keys.Length);
                for (int h = 0; h < arrKeys.Length; h++) { if (CodeStr == arrKeys[h]) { DecodedStr += arrCharSet[h]; break; } }
            }
            return DecodedStr;
        }
        private static string[] LoadKeyArray()
        {
            string[] tmpArray = new string[Keys[0].Length];
            int KeyStrLength = Keys[0].Length;
            for (int i = 0; i < KeyStrLength; i++) { for (int h = 0; h < Keys.Length; h++) { tmpArray[i] += Keys[h].Substring(i, 1); } }
            return tmpArray;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings 
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform. 
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption. 
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream 
                                // and place them in a string. 
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform. 
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream. 
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("3928255200621136");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        #endregion
    }
}
