using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Shared.Util;
/// <summary>
/// 28-07-2023
/// https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-7.0
/// Modified to return base64 string
/// </summary>
public static class Cryptography
{

    private static string KEY = "~!@#$%^&*()_+{}:>?<`1234567890-=[]\'.,/|";
    public static string EncryptAes(string pCadena)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(pCadena);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                pCadena = Convert.ToBase64String(ms.ToArray());
            }
        }
        return pCadena;
    }

    public static string DecryptAes(string pCadena)
    {
        byte[] cipherBytes = Convert.FromBase64String(pCadena);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                pCadena = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return pCadena;
    }
}

