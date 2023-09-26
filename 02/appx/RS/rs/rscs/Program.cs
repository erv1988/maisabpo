using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElByECC;

namespace rscs
{
    class Program
    {
        static int Main(string[] args)
        {
            int a, d;
            int USER_DATA_SIZE = 2048;
            int N_BYTES_DAMAGE = 6;

            RAW_SECTOR_MODE1 sectorForDamage = RAW_SECTOR_MODE1.Init();
            RAW_SECTOR_MODE1 sectorForCompare = RAW_SECTOR_MODE1.Init();
            byte[] stub_head = new byte[4];

            byte []user_data = new byte[USER_DATA_SIZE];

            // инициализация пользовательских данных
            Console.SetWindowSize(160, 80);
            Console.WriteLine("инициализация пользовательских данных");
            for (a = 0; a < USER_DATA_SIZE; a++) 
                user_data[a] = (byte)a;

            stub_head[3] = 1;

            printData(user_data);
            // генерация кодов Рида-Соломона на основе пользовательских данных
            Console.WriteLine("генерация кодов Рида-Соломона на основе пользовательских данных");
            a = GenECCAndEDC_Mode1(user_data, stub_head, sectorForDamage.Raw);
            if (a == 0)
            {
                Console.WriteLine("ошибка при генерации кода Рида-Соломона");
                return -1;
            }
            Array.Copy(sectorForDamage.Raw, sectorForCompare.Raw, sectorForDamage.Raw.Length);
            Console.WriteLine("SYNC=");
            printData(sectorForCompare.SYNC);
            Console.WriteLine("ADDR=");
            printData(sectorForCompare.ADDR);
            Console.WriteLine("EDC=");
            printData(sectorForCompare.EDC);
            Console.WriteLine("P=");
            printData(sectorForCompare.P);
            Console.WriteLine("Q=");
            printData(sectorForCompare.Q);
            Console.WriteLine("USER_DATA=");
            printData(sectorForCompare.USER_DATA);

            // умышленное искажение пользовательских данных
            Console.WriteLine("умышленное искажение пользовательских данных");
            for (a = 0; a < N_BYTES_DAMAGE; a++) 
                sectorForDamage.Raw[a+RAW_SECTOR_MODE1.USER_DATA_OFFSET] ^= 0xFF;

            // проверка наличия искажений
            Console.Write("проверка наличия искажений ");
            d = 0;
            for (a = 0; a < N_BYTES_DAMAGE; a++)
                d |= sectorForDamage.Raw[a + RAW_SECTOR_MODE1.USER_DATA_OFFSET] ^ sectorForCompare.Raw[a + RAW_SECTOR_MODE1.USER_DATA_OFFSET];
            if (d == 0)
            {
                Console.WriteLine("искажения не случилось");
                return -1;
            }
            Console.WriteLine(" искажения присутсвуют");
            printData(sectorForDamage.USER_DATA);

            // проверка целостности пользовательских данных
            Console.WriteLine("проверка целостности пользовательских данных");
            a = CheckSector(sectorForDamage.Raw, 0);
            if (a != 0)
            {
                Console.WriteLine("проверка целостности не выявила ошибок");
                return -1;
            }

            // восстановление пользовательских данных
            a = CheckSector(sectorForDamage.Raw, 1);
            if (a == 0)
            {
                Console.WriteLine("данные не восстановились");
                return -1;
            }
            Console.WriteLine("данные восстановлены!");
            printData(sectorForDamage.USER_DATA);

            return 0;
        }

        static void printData(byte[] data)
        {
            int c = 0;
            for (int n = 0; n < data.Length; ++n, ++c)
            {
                if (c == 32)
                {
                    Console.WriteLine();
                    c = 0;
                }
                Console.Write($"{data[n]:X02} ");
            }
            Console.WriteLine();
        }
    }
}
