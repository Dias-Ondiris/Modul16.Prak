using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Modul16.Prak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к отслеживаемой директории:");
            string path = Console.ReadLine();

            Console.WriteLine("Введите путь к лог-файлу:");
            string logPath = Console.ReadLine();

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Указанная директория не существует.");
                return;
            }

            using (FileSystemWatcher watcher = new FileSystemWatcher(path))
            {
                watcher.NotifyFilter = NotifyFilters.FileName
                                       | NotifyFilters.DirectoryName
                                       | NotifyFilters.LastWrite;

                watcher.Created += (sender, e) => LogChange(logPath, $"Создан: {e.FullPath}");
                watcher.Deleted += (sender, e) => LogChange(logPath, $"Удалён: {e.FullPath}");
                watcher.Renamed += (sender, e) => LogChange(logPath, $"Переименован: {e.OldFullPath} в {e.FullPath}");
                watcher.Changed += (sender, e) => LogChange(logPath, $"Изменён: {e.FullPath}");

                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Начато отслеживание. Нажмите 'q' для выхода.");
                while (Console.Read() != 'q') ;
            }
        }
        static void LogChange(string logPath, string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logPath, true))
                {
                    sw.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи в лог-файл: {ex.Message}");
            }
        }
    }
}
