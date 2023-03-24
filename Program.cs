using System.Linq.Expressions;
using static System.Reflection.Metadata.BlobBuilder;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss>? dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
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
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                { Console.WriteLine("Goodbye!"); break; }
                else if (command == "load")
                {
                    if (argument.Length == 1) loadList(defaultFile);
                    else try
                        { loadList("..\\..\\..\\dict\\" + argument[1]); }
                        catch (System.IO.FileNotFoundException) { Console.WriteLine($"File ..\\..\\..\\dict\\{argument[1]} didn´t exist"); }
                }
                else if (command == "list")
                {
                    try
                    {
                        foreach (SweEngGloss gloss in dictionary)
                        { Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}"); }
                    }
                    catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "new")
                {
                    wordCheck(argument, out string newSweWord, out string newEngWord);
                    if (argument.Length == 1 || argument.Length == 3)
                        try { dictionary.Add(new SweEngGloss(newSweWord, newEngWord)); Console.WriteLine($"{newSweWord} and {newEngWord} added to list "); }
                        catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "delete")
                {
                    wordCheck(argument, out string delSweWord, out string delEngWord);
                    if (argument.Length == 1 || argument.Length == 3)
                        try { removeWord(delSweWord, delEngWord); }
                        catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)
                    {
                        TranslateWord(argument[1]);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.Write("Write word to be translated: ");
                        string wordToTranslate = Console.ReadLine();
                        TranslateWord(wordToTranslate);
                    }
                    else { Console.WriteLine("Wrong input. Use translate and push enter"); }
                }
                else if (command == "help")
                {
                    Console.WriteLine("Allowed commands:");
                    Console.Write("quit - for end this program\nload - load a file\nlist - list all posts in program\nnew - " +
                                  "add new post\ndelete - delete post\ntranslate - translate av word \n ");
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }
        private static void wordCheck(string[] argument, out string sweWord, out string engWord)
        {
            if (argument.Length == 3) { sweWord = argument[1]; engWord = argument[2]; }
            else if (argument.Length == 1)
            {
                Console.Write("Write word in Swedish: ");
                sweWord = Console.ReadLine();
                Console.Write("Write word in English: ");
                engWord = Console.ReadLine();
            }
            else { Console.WriteLine("Wrong input. Use new or delete and push enter"); sweWord = ""; engWord = ""; }
        }

        private static void loadList(string defaultFile)
        {
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

        private static void removeWord(string sweWord, string engWord)
        {
            int index = -1;
            for (int i = 0; i < dictionary.Count; i++)
            {
                SweEngGloss gloss = dictionary[i];
                if (gloss.word_swe == sweWord && gloss.word_eng == engWord)
                    index = i;
            }
            try
            {
                dictionary.RemoveAt(index); Console.WriteLine($"{sweWord} and {engWord} deleted from list ");
            }
            catch (System.ArgumentOutOfRangeException) { Console.WriteLine($"{sweWord} and/or {engWord} doesn´t excist in dictionary"); }
        }
        private static void TranslateWord(string argument)
        {
            try
            {
                int wordexist = 0;
                foreach (SweEngGloss gloss in dictionary)
                {
                    if (gloss.word_swe == argument)
                    { Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}"); wordexist++; }
                    if (gloss.word_eng == argument)
                    { Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}"); wordexist++; }
                }
                if (wordexist == 0) Console.WriteLine($"The word {argument} doesn´t exist in dictionary");
            }
            catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }

        }
    }
}