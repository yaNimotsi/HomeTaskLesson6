using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HomeTaskLesson6
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] allProcess = Process.GetProcesses();

            Dictionary<int, string> processDictionary = AddProcessToDictionary(allProcess, out int maxLenNameProc, out int maxLenIDProc);

            PrintAllProcess(processDictionary, maxLenNameProc, maxLenIDProc);

            StartMenuKillProcess(ref processDictionary, maxLenNameProc, maxLenIDProc);

            Console.WriteLine(allProcess.Length);
        }

        /// <summary>
        /// Add all process in dictionary by nameProcess ID and nameProcess
        /// </summary>
        /// <param name="allProcess">Array all process</param>
        /// <returns></returns>
        static Dictionary<int, string> AddProcessToDictionary(Process[] allProcess, out int maxLenNameProc, out int maxLenIDProc)
        {
            Dictionary<int, string> procesessDictionary = new Dictionary<int, string>();

            string procName;
            int procID;
            maxLenNameProc = 0;
            maxLenIDProc = 0;

            foreach (Process proc in allProcess)
            {
                procName = proc.ProcessName;
                procID = proc.Id;

                procesessDictionary.Add(procID, procName);

                if (maxLenNameProc < procName.Length) maxLenNameProc = procName.Length;
                if (maxLenIDProc < procID.ToString().Length) maxLenIDProc = procID.ToString().Length;
            }

            return procesessDictionary;
        }

        /// <summary>
        /// Print allProcess to console
        /// </summary>
        /// <param name="procesessDictionary">Dictionary all process</param>
        /// <param name="maxLenNameProc"> param to use PadRight</param>
        /// <param name="maxLenIDProc"> param to use PadRight</param>
        static void PrintAllProcess(Dictionary<int, string> procesessDictionary, int maxLenNameProc, int maxLenIDProc)
        {
            Console.WriteLine($"{"ID".PadRight(maxLenIDProc)} {"Name Process".PadRight(maxLenNameProc)}");

            foreach (KeyValuePair<int, string> keyValue in procesessDictionary)
            {
                Console.WriteLine($"{keyValue.Key.ToString().PadRight(maxLenIDProc)} {keyValue.Value.PadRight(maxLenNameProc)}");
            }
        }

        /// <summary>
        /// Start menu to kill process
        /// </summary>
        /// <param name="procesessDictionary">Dictionary all process</param>
        /// <param name="maxLenNameProc"> param to use PadRight</param>
        /// <param name="maxLenIDProc"> param to use PadRight</param>
        static void StartMenuKillProcess(ref Dictionary<int, string> procesessDictionary, int maxLenNameProc, int maxLenIDProc)
        {
            while (true)
            {
                Console.WriteLine("Введите ID или имя процесса, который хотите завершить. Для завершения введите -1");

                string userVal = Console.ReadLine();

                if (userVal == "-1") break;

                if (int.TryParse(userVal, out int IntUserVal))
                {
                    KillProcessById(ref procesessDictionary, IntUserVal);
                }
                else
                {
                    KillProcByName(ref procesessDictionary, userVal);
                }
            }

            PrintAllProcess(procesessDictionary, maxLenNameProc, maxLenIDProc);
        }

        /// <summary>
        /// Method to kill process by Name
        /// </summary>
        /// <param name="procesessDictionary">Dictionary all process</param>
        /// <param name="maxLenNameProc"> param to use PadRight</param>
        /// <param name="maxLenIDProc"> param to use PadRight</param>
        static void KillProcByName(ref Dictionary<int, string> procesessDictionary, string userVal)
        {
            try
            {
                Process[] procToKill = Process.GetProcessesByName(userVal);
                if (procToKill.Length == 1)
                {
                    procToKill[0].Kill();
                    Console.WriteLine("Процесс успешно остановлен");

                    procesessDictionary.Remove(procToKill[0].Id);
                }
            }
            catch { Console.WriteLine("Что-то пошло не так, повторите попытку"); }
        }
        /// <summary>
        /// Method to kill process by ID
        /// </summary>
        /// <param name="procesessDictionary">Dictionary all process</param>
        /// <param name="maxLenNameProc"> param to use PadRight</param>
        /// <param name="maxLenIDProc"> param to use PadRight</param>
        static void KillProcessById(ref Dictionary<int, string> procesessDictionary, int IntUserVal)
        {
            foreach (int key in procesessDictionary.Keys)
            {
                if (key == IntUserVal)
                {
                    try
                    {
                        Process proccToKill = Process.GetProcessById(key);
                        proccToKill.Kill();
                        Console.WriteLine("Процесс успешно остановлен");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        return;
                    }
                }
            }

            Console.WriteLine("Процесса с указанным ID найти не удалось!");
        }
    }
}
