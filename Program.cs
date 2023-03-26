using System;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Windows.Input;
using static System.Reflection.Metadata.BlobBuilder;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            { this.word_swe = word_swe; this.word_eng = word_eng; }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app! \nwrite <help> for allowed commands ");
            do
            {
                Console.Write("> ");
                string[] commandLine = Console.ReadLine().Split();
                if (commandLine[0] == "quit" || commandLine[0] == "q" || commandLine[0] == "exit" || commandLine[0] == "Quit")
                { Console.WriteLine("Goodbye!"); return; }
                else if (commandLine[0] == "load")
                {
                    loadList(defaultFile, commandLine);
                }
                else if (commandLine[0] == "list")
                {
                    listList();
                }
                else if (commandLine[0] == "new" || commandLine[0] == "delete")
                {
                    removeAddWord(commandLine);
                }
                else if (commandLine[0] == "translate")
                {
                    TranslateWord(commandLine);
                }
                else if (commandLine[0] == "help")
                {
                    Console.Write("Allowed commands:\n<quit> - end this program\n<load> - load a file\n<load> <filename> - load a specific file\n<list> - list all " +
                                  "posts in program\n<new> - add new post\n<new> <swedish word> <english word> - add new post\n<delete> - delete post\n<delete> " +
                                  "<swedish word> <english word> - delete specific post\n<translate> - translate a word \n<translate> <word> - translate a specific word \n ");
                }
                else
                {   Console.WriteLine($"Unknown command: '{commandLine[0]}'");     }
            }
            while (true);
        }
        private static void listList()
        {
            try
            {
                foreach (SweEngGloss gloss in dictionary)
                { Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}"); }
            }
            catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
        }
        private static void loadList(string defaultFile, string[] CommandLine)
        {
            try
            {
                if (CommandLine.Length != 1) defaultFile = "..\\..\\..\\dict\\" + CommandLine[1];
                using (StreamReader sr = new StreamReader(defaultFile))
                {
                    dictionary = new List<SweEngGloss>(); // Empty it!
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        SweEngGloss gloss = new SweEngGloss(line);
                        dictionary.Add(gloss);
                        line = sr.ReadLine();
                    }
                }
            }
            catch (System.IO.FileNotFoundException) { Console.WriteLine($"File {defaultFile} didn´t exist"); }
        }
        private static void removeAddWord(string[] CommandLine)
        {
            string sweWord = "", engWord = "";
            if (CommandLine.Length == 3) { sweWord = CommandLine[1]; engWord = CommandLine[2]; }
            else if (CommandLine.Length == 1)
            {
                Console.Write("Write word in Swedish: ");
                sweWord = Console.ReadLine();
                Console.Write("Write word in English: ");
                engWord = Console.ReadLine();
            }
            else { Console.WriteLine("Wrong input. <help> for help"); return; }

            if (CommandLine[0] == "new")
            {
                try { dictionary.Add(new SweEngGloss(sweWord, engWord)); Console.WriteLine($"{sweWord} and {engWord} added to list "); }
                catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
            }
            else
            {
                try
                {
                    for (int i = 0; i < dictionary.Count; i++)
                    {
                        SweEngGloss gloss = dictionary[i];
                        if (gloss.word_swe == sweWord && gloss.word_eng == engWord)
                        { dictionary.RemoveAt(i); Console.WriteLine($"{sweWord} and {engWord} deleted from list "); }
                    }
                }
                catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); return; }
                catch (System.ArgumentOutOfRangeException) { Console.WriteLine($"{sweWord} and/or {engWord} doesn´t excist in dictionary"); }
            }
        }
        private static void TranslateWord(string[] CommandLine)
        {
            string wordToTranslate = "";
            if (CommandLine.Length == 1)
            {
                Console.Write("Write word to be translated: ");
                wordToTranslate = Console.ReadLine();
            }
            else if (CommandLine.Length == 2) wordToTranslate = CommandLine[1];
            else { Console.WriteLine("Wrong input. Write translate and press enter"); }
            try
            {
                int wordexist = 0;
                foreach (SweEngGloss gloss in dictionary)
                {
                    if (gloss.word_swe == wordToTranslate)
                    { Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}"); wordexist++; }
                    if (gloss.word_eng == wordToTranslate)
                    { Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}"); wordexist++; }
                }
                if (wordexist == 0) Console.WriteLine($"The word {wordToTranslate} doesn´t exist in dictionary");
            }
            catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
        }
    }
}