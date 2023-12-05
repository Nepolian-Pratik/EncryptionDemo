using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

public class Program
{
    public class RSAEncryption
    {
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RSAEncryption()
        {
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }

        public string Encrypt(string plainText)
        {
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(_publicKey);
            var data = Encoding.Unicode.GetBytes(plainText);
            var cipher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cipher);
        }

        public string Decrypt(string cipherText)
        {
            var dataBytes = Convert.FromBase64String(cipherText);
            csp.ImportParameters(_privateKey);
            var plainText = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plainText);
        }
    }

    private static void Main(string[] args)
    {
        var rsa = new RSAEncryption();
        var publicKey = rsa.GetPublicKey();
        var plainText = "Hello World";
        var cipherText = rsa.Encrypt(plainText);
        var decryptedCipherText = rsa.Decrypt(cipherText);
        Console.WriteLine("Public Key: {0}", publicKey);
        Console.WriteLine("Plain Text: {0}", plainText);
        Console.WriteLine("Cipher Text: {0}", cipherText);
        Console.WriteLine("Decrypted Cipher Text: {0}", decryptedCipherText);
        Console.ReadKey();

        Console.WriteLine("-----------");
    }
}