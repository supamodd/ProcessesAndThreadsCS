using System;
using System.Threading;

namespace ProcessesAndThreadsCSharp;

internal static class Program
{
    private static volatile bool finish = false;

    private static readonly Mutex consoleMutex = new();

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Нажмите Enter для остановки.\n");

        Thread plusThread = new(Plus)
        {
            Name = "PlusThread"
        };

        Thread minusThread = new(Minus)
        {
            Name = "MinusThread"
        };

        plusThread.Start();
        minusThread.Start();

        Console.ReadLine();
        finish = true;

        plusThread.Join();
        minusThread.Join();

        consoleMutex.Dispose();

        Console.WriteLine("\nПотоки завершены.");
    }

    private static void Plus()
    {
        while (!finish)
        {
            consoleMutex.WaitOne();
            try
            {
                Console.Write("+ ");
                Thread.Sleep(100);
            }
            finally
            {
                consoleMutex.ReleaseMutex();
            }

            Thread.Sleep(100);
        }
    }

    private static void Minus()
    {
        while (!finish)
        {
            consoleMutex.WaitOne();
            try
            {
                Console.Write("- ");
                Thread.Sleep(100);
            }
            finally
            {
                consoleMutex.ReleaseMutex();
            }

            Thread.Sleep(100);
        }
    }
}
