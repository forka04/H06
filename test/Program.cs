using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = @"C:\Documents\C_Sharp\N.txt";

            Console.WriteLine($"положите файл с числом по указаному пути -{str}");
            int n = ReadNumber(str);
            Console.WriteLine($"Получено число из файла {n}\n Выберите дальнейший вариант работы" +
                $"\n Нажать 1 для создания файла с группами " +
                $"\n Нажать 2 для вывода в консоль количества групп");

            bool readOnlee = Console.ReadKey().KeyChar == '2';

            if (readOnlee)
                CreateStringFile(n, readOnlee);
            else
                CreateStringFile(n);




        }





        /// <summary>
        /// создать Текст с группами неделимых на сбея чисел
        /// </summary>
        /// <param name="n"></param>
        /// <param name="onlySee">только в косноль вывести число групп</param>
        /// <returns></returns>
        public static void CreateStringFile(int n, bool onlySee)
        {

            int count = 1;
            if (n > 0)
            {
                int first = 1;
                Console.Write($"\nГруппа {count}");
                for (int i = 2; i <= n; i++)
                {
                    if (onlySee)
                    {
                        if (i % first == 0)
                        {
                            count++;
                            first = i;
                            Console.Write($"\nГруппа {count}");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// создать Текст с группами неделимых на себя чисел
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static void CreateStringFile(int n)
        {
            int count = 1;
            if (n > 0)
            {
                string path = @"C:\Documents\C_Sharp\file.txt";
                using (StreamWriter streamWriter = new(path))
                {
                    int first = 1;
                    streamWriter.Write($"Группа {count}: {first}");
                    //StringBuilder stringBuilder = new StringBuilder();

                    for (int i = 2; i <= n; i++)
                    {

                        if (i % first == 0)
                        {
                            count++;
                            first = i;
                            // streamWriter.WriteLine(stringBuilder);
                            // stringBuilder.Clear();
                            // stringBuilder.Append($"Группа {count}: {i}");
                            streamWriter.Write($"\nГруппа {count}: {i}");

                        }
                        else
                        {
                            //stringBuilder.Append($" {i}");
                            streamWriter.Write($" {i}");
                        }
                    }
                }

                Console.WriteLine($"\nФайл с группами записан в -{path}");
                Console.WriteLine("Желаете сделать ахрив? \n y / n ? ");

                bool answer = Char.ToLower(Console.ReadKey().KeyChar) == 'y';
                if (answer)
                    CompressZip(path);
            }
        }


        public static void CompressZip(string path)
        {
            string compressed = @"C:\Documents\C_Sharp\file.zip";


            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (FileStream zipfileStream = File.Create(compressed))
                {
                    using (GZipStream gZipStream = new GZipStream(zipfileStream, CompressionMode.Compress))
                    {
                        fileStream.CopyTo(gZipStream);
                        Console.WriteLine($"\nСжатие файла {path} завершено." +
                            $"\n Размер файла иходного : {fileStream.Length} " +
                            $"\n Размер файла сжатого :{zipfileStream.Length} ");
                    }
                }
            }
        }


        /// <summary>
        /// Извлекаем число из файла
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ReadNumber(string str)
        {
            try
            {
                string line;
                using (StreamReader streamReader = new(str))
                {
                    line = streamReader.ReadLine();
                }
                return int.Parse(line); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
    }
}