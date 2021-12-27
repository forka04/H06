using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H06_1
{
    /// <summary>
    /// Создает группы чисел, не делящихся друг на друга, и записывает их в файл. 
    /// </summary>
    class Program
    {

        /// <summary>
        /// Метод, высчитывающий количество групп для вывода на консоль.
        /// </summary>
        /// <param name="n">Конечное число диапазона.</param>
        /// <returns>Количество групп.</returns>
        static int GrpsNum(int n)
        {
            return (int)Math.Log(n, 2) + 1;  // Формула для вычисления количества групп
        }

        /// <summary>
        /// Метод, заполняющий группы числами по делимости из введённого диапазона
        /// </summary>
        /// <param name="n">Конечное число диапазона.</param>
        /// <returns>Затраченное время.</returns>
        static int[][] GrpMake(int n)
        {
            #region
            bool Grp;
            bool GrpAdd;
            int[][] grp_arr = Array.Empty<int[]>(); // Инициализируем массив

            for (int i = 1; i <= n; i++)
            {
                Grp = false;
                if (i == 1 && grp_arr.Length < i)
                {
                    Array.Resize(ref grp_arr, grp_arr.Length + 1);
                    grp_arr[^1] = Array.Empty<int>();                              
                    Array.Resize(ref grp_arr[^1], grp_arr[^1].Length + 1);         // Расширяем массив на 1 единицу
                    grp_arr[^1][^1] = i;                                           // Записываем число в массив  
                }
                else
                {
                    for (int j = 0; j < grp_arr.Length; j++)                
                    {
                        GrpAdd = true;
                        for (int k = 0; k < grp_arr[j].Length; k++)
                        {
                            if (i % grp_arr[j][k] == 0)
                            {
                                GrpAdd = false;
                                break;
                            }
                        }
                        if (GrpAdd)
                        {
                            Array.Resize(ref grp_arr[j], grp_arr[j].Length + 1);
                            grp_arr[j][^1] = i;                                     // Записываем число в массив
                            break;
                        }
                        else if (!GrpAdd && j == grp_arr.Length - 1)
                        {
                            Grp = true;
                        }
                    }
                    if (Grp)
                    {
                        Array.Resize(ref grp_arr, grp_arr.Length + 1);
                        grp_arr[^1] = Array.Empty<int>();
                        Array.Resize(ref grp_arr[^1], grp_arr[^1].Length + 1);
                        grp_arr[^1][^1] = i;                                          // Записываем число в массив
                    }
                }
            }
            return grp_arr;
            #endregion
        }

        /// <summary>
        /// Метод возвращает текущее время
        /// </summary>
        /// <returns></returns>
        static DateTime TimerStart()
        {
            DateTime start_tm = DateTime.Now;
            return start_tm;
        }

        /// <summary>
        /// Метод, возвращающий время выполнения
        /// </summary>
        /// <returns>Затраченное время</returns>
        static TimeSpan TimerStop(DateTime starttime)
        {
            TimeSpan stop_tm = DateTime.Now.Subtract(starttime);
            return stop_tm;
        }

        /// <summary>
        /// Метод чтения из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Заданное число N</returns>
        static int Read_File(string path)
        {
            var n = File.ReadAllText(path);                                                               // Считываем из файла число N
            return int.Parse(n);
        }

        /// <summary>
        /// Метод архивации файла в zip формат
        /// </summary>
        static void Compress(string sourceFile)
        {
            string compressFile = sourceFile + ".zip" ;

            using (FileStream read_file = new(sourceFile, FileMode.OpenOrCreate))                          // Поток для чтения файла
            {
                using FileStream wr_file = File.Create(compressFile);                                      // Поток для записи сжатого файла
                using GZipStream zip_file = new(wr_file, CompressionMode.Compress);                        // Поток архивации
                read_file.CopyTo(zip_file);                                                                // копируем байты из одного потока в другой
                Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                    sourceFile, read_file.Length.ToString(), wr_file.Length.ToString());
            }
            File.Delete(sourceFile);                                                                        //Удаление исходного файла
            Console.WriteLine($"Файл заархивирован");
        }

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        /// <param name="groups">Массив с группами</param>
        /// <returns>Сформированный файл</returns>
        static string WriteFile(int[][] groups)
        {
            string file = @"C:\Documents\C_Sharp\group.txt";

            // Вывод массива чисел в файл используя класс StreamWriter
            using (StreamWriter st_write = new(file))                                                        // Создание потока для записи в файл
            {
                for (int m = 0; m < groups.Length; m++)
                {
                    st_write.WriteLine($"Группа {m + 1}:");
                    for (int j = 0; j < groups[m].Length; j++)
                    {
                        st_write.Write($"{Convert.ToString(groups[m][j])} ");
                    }
                    st_write.WriteLine("\n");
                }
            }
            return file;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Делимость чисел от 1 до N");
            Console.WriteLine("\nУкажите путь к файлу, содержащему число N:");
            string path = Console.ReadLine();
            int n = Read_File(path);
            Console.WriteLine($"\nЗагруженное число N = {n}");
            if (n > 1000000000)                                                                              //Условие, которое проверяет число N
            {
                Console.WriteLine("Программа работает только с числами N не превосходящими 1 000 000 000");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"\nВыберите режим работы:");
                Console.WriteLine($"1 - Показать в консоли количество групп M для заданного значения N");
                Console.WriteLine($"2 - Заполнить группы и записать в файл");
                int mode = int.Parse(Console.ReadLine());

                switch (mode)
                {
                    case 1:
                        var tm_start = TimerStart();                       // Запускаем счётчик

                        // Вызываем метод расчёта количества групп и выводим результат
                        Console.WriteLine($"\nКоличество групп: {GrpsNum(n)}");
                        var stop = TimerStop(tm_start);               // Останавливаем счётчик
                        Console.WriteLine($"\nЗатраченное время на выполение {stop.TotalSeconds} сек");
                        Console.Read();

                        break;

                    case 2:

                        tm_start = TimerStart();                      // start счётчик

                        int[][] groups = GrpMake(n);                   // Распределение чисел по группам
                        string file = WriteFile(groups);
                        stop = TimerStop(tm_start);                   // stop счётчик
                        Console.WriteLine($"\n\nЗатраченное время на выполение {stop.TotalSeconds} сек");
                        Console.WriteLine($"\nЗаархивировать данные? (1 - да, 0 - нет)");
                        if (int.Parse(Console.ReadLine()) == 1)
                        {
                            Compress(file);
                        }
                        else
                        {
                            break;
                        }

                        Console.Read();
                        break;
                    default:
                        break;
                }
            }            
        }
    }
}