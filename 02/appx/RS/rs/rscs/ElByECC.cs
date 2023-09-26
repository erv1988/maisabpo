using System;
using System.Runtime.InteropServices;

public static class ElByECC
{

    public struct RAW_SECTOR_MODE1
    {
        byte[] rawdata;

        public const int USER_DATA_OFFSET = 16;

        public byte[] Raw { get => rawdata; }

        // синхрогруппа
        public byte[] SYNC { get { byte[] data = new byte[12]; Array.Copy(rawdata, 0, data, 0, 12); return data; } }
        // абсолютный адрес сектора
        public byte[] ADDR { get { byte[] data = new byte[3]; Array.Copy(rawdata, 12, data, 0, 3); return data; } }

        // тип сектора
        public byte MODE { get => rawdata[15]; }

        // пользовательские данные
        public byte[] USER_DATA { get { byte[] data = new byte[2048]; Array.Copy(rawdata, 16, data, 0, 2048); return data; } }

        // контрольная сумма
        public byte[] EDC { get { byte[] data = new byte[4]; Array.Copy(rawdata, 2064, data, 0, 4); return data; } }

        // нули (не используется)
        public byte[] ZERO { get { byte[] data = new byte[8]; Array.Copy(rawdata, 2068, data, 0, 8); return data; } }
        
        // P-байты четности
        public byte[] P { get { byte[] data = new byte[172]; Array.Copy(rawdata, 2076, data, 0, 172); return data; } }
        
        // Q-байты четности
        public byte[] Q { get { byte[] data = new byte[104]; Array.Copy(rawdata, 2248, data, 0, 104); return data; } }

        public static RAW_SECTOR_MODE1 Init()
        {
            return new RAW_SECTOR_MODE1()
            {
                rawdata = new byte[2352]
            };
        }

    }


    /// <summary>
    /// Функция GenECCAndEDC_Mode1 осуществляет генерацию корректирующих кодов на основе 2048-байтового блока пользовательских данных 
    /// </summary>
    /// <param name="userdata"> указатель на 2048-байтовый блок пользовательских данных, для которых необходимо выполнить расчет корректирующих кодов. 
    /// Сами пользовательские данные в процессе выполнения функции остаются неизменными и автоматически копируются в буфер целевого сектора, где к ним 
    /// добавляется 104 + 172 байт четности и 4 байта контрольной суммы
    /// </param>
    /// <param name="header"> указатель на 4-байтовый блок, содержащий заголовок сектора. Первые три байта занимает абсолютный адрес, записанный в BCD-форме,
    /// а четвертый байт отвечает за тип сектора, которому необходимо присвоить значение 1, соответствующий режиму «корректирующие коды задействованы»
    /// </param>
    /// <param name="sector"> указатель на 2352-байтовый блок, в который будет записан сгенерированный сектор, содержащий 2048-байт пользовательских данных 
    /// и 104+172 байт корректирующих кодов вместе с 4 байтами контрольной суммы и представленный  структурой RAW_SECTOR</param>
    /// <returns>При успешном завершении функция возвращает ненулевое значение и ноль в противном случае</returns>
    [DllImport("ElbyECC.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GenECCAndEDC_Mode1(byte[] userdata, byte[] header, byte[] sector);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sector">указатель на 2352-байтовый блок данных, содержащий подопытный сектор. Лечение сектора осуществляется «вживую», т.е. непосредственно 
    /// по месту возникновения ошибки. Если количество разрушенных байт превышают корректирующие способности кодов Рида-Соломона, исходные данные остаются неизменными</param>
    /// <param name="DO">флаг, нулевое значение которого указывает на запрет модификации сектора. Другими словами, соответствует режиму TEST ONLY. Ненулевое значение разрешает 
    /// восстановление данных, если они действительно подверглись разрушению</param>
    /// <returns>При успешном завершении функция возвращает ненулевое значение и ноль, если сектор содержит ошибку (в режиме TEST ONLY) или если данные восстановить не удалось 
    /// (при вызове функции в режиме лечения). Для предотвращения возможной неоднозначности рекомендуется вызывать данную функцию в два приема. Первый раз – в режиме тестирования 
    /// для проверки целостности данных, и второй раз – в режиме лечения (если это необходимо)</returns>
    [DllImport("ElbyECC.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int CheckSector(byte[] sector, int DO);

}
