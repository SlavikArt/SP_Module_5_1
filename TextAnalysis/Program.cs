using System;
using System.Threading;

namespace TextAnalysis
{
    public class Program
    {
        static string text;
        static bool userExit = false;
        static TextStatistics stats = new TextStatistics();
        static ManualResetEvent pause = new ManualResetEvent(true);

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Введіть текст:");
            text = Console.ReadLine();

            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("Введений текст пустий або містить тільки пробіли." +
                    " Будь ласка, введіть коректний текст.\n");
                return;
            }

            Thread analyzeThread = new Thread(AnalyzeText);
            analyzeThread.Start();

            while (analyzeThread.IsAlive && !userExit)
            {
                Console.WriteLine("Натисніть 's' щоб зупинити аналіз, 'c' щоб продовжити, 'q' щоб вийти");
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.S:
                        pause.Reset();
                        break;
                    case ConsoleKey.C:
                        pause.Set();
                        break;
                    case ConsoleKey.Q:
                        pause.Reset();
                        userExit = true;
                        break;
                }
            }
            if (userExit)
                Console.WriteLine("Виконання програми завершено.");
            else
                Console.WriteLine("\nАналіз тексу завершено.");
        }

        static void AnalyzeText()
        {
            while (stats.Chars < text.Length)
            {
                pause.WaitOne();

                char c = text[stats.Chars];

                if (char.IsWhiteSpace(c))
                {
                    stats.NewWord = true;
                }
                else if (stats.NewWord)
                {
                    stats.Words++;
                    stats.NewWord = false;
                }

                if (c == '.' || c == '!' || c == '?')
                {
                    stats.Sentences++;
                    if (c == '?') stats.QuestionSentences++;
                    if (c == '!') stats.ExclamationSentences++;
                    stats.NewSentence = true;
                }
                else if (stats.NewSentence)
                {
                    stats.NewSentence = false;
                }

                Console.WriteLine($"Кількість речень: {stats.Sentences}");
                Console.WriteLine($"Кількість символів: {stats.Chars + 1}");
                Console.WriteLine($"Кількість слів: {stats.Words}");
                Console.WriteLine($"Кількість питальних речень: {stats.QuestionSentences}");
                Console.WriteLine($"Кількість окличних речень: {stats.ExclamationSentences}\n");

                stats.Chars++;
            }
        }
    }
}

