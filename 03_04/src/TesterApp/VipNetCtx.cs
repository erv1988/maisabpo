using GostCryptography.Config;
using GostCryptography.Gost_R3410;
using GostCryptography.Gost_R3411;
using GostCryptography.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GostCryptography.Base;
using System.Security.Cryptography.Xml;
using System.Runtime.InteropServices;
using GostCryptography.Asn1.Gost.Gost_R3410_2012_512;
using GostCryptography.Asn1.Gost.Gost_R3410_2012_256;

namespace TesterApp
{
    public static class VipNetCtx
    {
        public static void Tests()
        {
            Test_3410();
            //TestHash_3411();
        }

        public static void TestHash_3411()
        {
            byte[] message, result;
            // Примеры из приложения к ГОСТ Р 34.11.2012(2018)
            // https://ru.wikisource.org/wiki/ГОСТ_Р_34.11—2012#А.1.1_Для_функции_хэширования_с_длиной_хэш-кода_512_бит
            // Инициализация алгоритмов
            HashAlgorithm hashAlgorithm_256 = new Gost_R3411_2012_256_HashAlgorithm(GostCryptoConfig.ProviderType);
            HashAlgorithm hashAlgorithm_512 = new Gost_R3411_2012_512_HashAlgorithm(GostCryptoConfig.ProviderType);

            // Входные данные
            byte[] m_A1 = new byte[] {
                0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35,
                0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31,
                0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
                0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32};
            byte[] m_A2 = new byte[] {
                0xD1, 0xE5, 0x20, 0xE2, 0xE5, 0xF2, 0xF0, 0xE8, 0x2C, 0x20, 0xD1, 0xF2, 0xF0, 0xE8, 0xE1, 0xEE,
                0xE6, 0xE8, 0x20, 0xE2, 0xED, 0xF3, 0xF6, 0xE8, 0x2C, 0x20, 0xE2, 0xE5, 0xFE, 0xF2, 0xFA, 0x20,
                0xF1, 0x20, 0xEC, 0xEE, 0xF0, 0xFF, 0x20, 0xF1, 0xF2, 0xF0, 0xE5, 0xEB, 0xE0, 0xEC, 0xE8, 0x20,
                0xED, 0xE0, 0x20, 0xF5, 0xF0, 0xE0, 0xE1, 0xF0, 0xFB, 0xFF, 0x20, 0xEF, 0xEB, 0xFA, 0xEA, 0xFB,
                0x20, 0xC8, 0xE3, 0xEE, 0xF0, 0xE5, 0xE2, 0xFB};


            // Контрольные значения
            // -- А.1.1
            byte[] hash_1_1 = new byte[] {
                0x1B, 0x54, 0xD0, 0x1A, 0x4A, 0xF5, 0xB9, 0xD5, 0xCC, 0x3D, 0x86, 0xD6, 0x8D, 0x28, 0x54, 0x62,
                0xB1, 0x9A, 0xBC, 0x24, 0x75, 0x22, 0x2F, 0x35, 0xC0, 0x85, 0x12, 0x2B, 0xE4, 0xBA, 0x1F, 0xFA,
                0x00, 0xAD, 0x30, 0xF8, 0x76, 0x7B, 0x3A, 0x82, 0x38, 0x4C, 0x65, 0x74, 0xF0, 0x24, 0xC3, 0x11,
                0xE2, 0xA4, 0x81, 0x33, 0x2B, 0x08, 0xEF, 0x7F, 0x41, 0x79, 0x78, 0x91, 0xC1, 0x64, 0x6F, 0x48};
            byte[] hash_1_2 = new byte[] {
                0x9D, 0x15, 0x1E, 0xEF, 0xD8, 0x59, 0x0B, 0x89, 0xDA, 0xA6, 0xBA, 0x6C, 0xB7, 0x4A, 0xF9, 0x27,
                0x5D, 0xD0, 0x51, 0x02, 0x6B, 0xB1, 0x49, 0xA4, 0x52, 0xFD, 0x84, 0xE5, 0xE5, 0x7B, 0x55, 0x00};
            byte[] hash_2_1 = new byte[] {
                0x1E, 0x88, 0xE6, 0x22, 0x26, 0xBF, 0xCA, 0x6F, 0x99, 0x94, 0xF1, 0xF2, 0xD5, 0x15, 0x69, 0xE0,
                0xDA, 0xF8, 0x47, 0x5A, 0x3B, 0x0F, 0xE6, 0x1A, 0x53, 0x00, 0xEE, 0xE4, 0x6D, 0x96, 0x13, 0x76,
                0x03, 0x5F, 0xE8, 0x35, 0x49, 0xAD, 0xA2, 0xB8, 0x62, 0x0F, 0xCD, 0x7C, 0x49, 0x6C, 0xE5, 0xB3,
                0x3F, 0x0C, 0xB9, 0xDD, 0xDC, 0x2B, 0x64, 0x60, 0x14, 0x3B, 0x03, 0xDA, 0xBA, 0xC9, 0xFB, 0x28};
            byte[] hash_2_2 = new byte[] {
                0x9D, 0xD2, 0xFE, 0x4E, 0x90, 0x40, 0x9E, 0x5D, 0xA8, 0x7F, 0x53, 0x97, 0x6D, 0x74, 0x05, 0xB0,
                0xC0, 0xCA, 0xC6, 0x28, 0xFC, 0x66, 0x9A, 0x74, 0x1D, 0x50, 0x06, 0x3C, 0x55, 0x7E, 0x8F, 0x50};


            // 512 бит
            // А.1.1 Для функции хэширования с длиной хэш-кода 512 бит
            message = m_A1;
            result = hashAlgorithm_512.ComputeHash(message, 0, message.Length);
            Console.WriteLine($"А.1.1: {result.SequenceEqual(hash_1_1)}");

            // А.1.2 Для функции хэширования с длиной хэш-кода 256 бит
            message = m_A1;
            result = hashAlgorithm_256.ComputeHash(message, 0, message.Length);
            Console.WriteLine($"А.1.2: {result.SequenceEqual(hash_1_2)}");

            // А.2.1 Для функции хэширования с длиной хэш-кода 512 бит
            message = m_A2;
            result = hashAlgorithm_512.ComputeHash(message, 0, message.Length);
            Console.WriteLine($"А.2.1: {result.SequenceEqual(hash_2_1)}");

            // А.2.2 Для функции хэширования с длиной хэш-кода 256 бит
            message = m_A2;
            result = hashAlgorithm_256.ComputeHash(message, 0, message.Length);
            Console.WriteLine($"А.2.2: {result.SequenceEqual(hash_2_2)}");


            var alg256 = new CngAlgorithm("GR 34.11-2012 256");
        }
    
        public static void Test_3410()
        {
            // Данные для подпись
            byte[] data = File.ReadAllBytes(@"encoded.bmp");
            byte[] signature;


            // Для тестирования берется первый найденный сертификат ГОСТ с закрытым ключем.
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 cert = null;
            foreach (var certificate in store.Certificates)
            {
                if (certificate.HasPrivateKey && certificate.SignatureAlgorithm.Value == Gost_R3410_2012_256_Constants.SignatureAlgorithm.Value/* "1.2.643.2.2.3"*/)
                {
                    cert = certificate;
                }
            }
            store.Close();

            // Формирование ЭЦП
            {
                //// Получение секретного клчюа
                //SecureString securePassword = CreateSecureString(@"87654321-"); ;
                var privateKeyInfo = cert.GetPrivateKeyInfo();
                var privateKey = new Gost_R3410_2012_256_AsymmetricAlgorithm(privateKeyInfo);
                //privateKey.SetContainerPassword(securePassword);
               
                // Подпись данных
                // -- 1. Вычисление хэш значения
                byte[] hash;
                using (var hashAlg = privateKey.CreateHashAlgorithm())
                {
                    hash = hashAlg.ComputeHash(data);
                }
                //--2.Получение ЭЦП
               signature = privateKey.CreateSignature(hash);

            }
            // Проверка подписи
            {
                var privateKeyInfo = cert.GetPrivateKeyInfo();
                var publicKey = new Gost_R3410_2012_256_AsymmetricAlgorithm(privateKeyInfo);

                byte[] hash;
                using (var hashAlg = publicKey.CreateHashAlgorithm())
                {
                    hash = hashAlg.ComputeHash(data);
                }
                bool result = publicKey.VerifySignature(hash, signature);
            }
        }

        public static void Test_3413()
        {

        }



        private static SecureString CreateSecureString(string value)
        {
            var result = new SecureString();

            foreach (var c in value)
            {
                result.AppendChar(c);
            }

            result.MakeReadOnly();

            return result;
        }
    }
}
