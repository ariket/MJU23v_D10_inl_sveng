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
                string[] commandLine = Console.ReadLine().Split();
                string command = commandLine[0];
                if (command == "quit" || command == "q")
                { Console.WriteLine("Goodbye!"); return; }
                else if (command == "load")
                {
                    if (commandLine.Length == 1) loadList(defaultFile);
                    else try
                        { loadList("..\\..\\..\\dict\\" + commandLine[1]); }
                        catch (System.IO.FileNotFoundException) { Console.WriteLine($"File ..\\..\\..\\dict\\{commandLine[1]} didn´t exist"); }
                }
                else if (command == "list")
                {
                    try
                    {   foreach (SweEngGloss gloss in dictionary)
                        { Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}"); } }
                    catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "new")
                {
                    wordCheck(commandLine, out string newSweWord, out string newEngWord);
                    if (commandLine.Length == 1 || commandLine.Length == 3)
                        try { dictionary.Add(new SweEngGloss(newSweWord, newEngWord)); Console.WriteLine($"{newSweWord} and {newEngWord} added to list "); }
                        catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "delete")
                {
                    wordCheck(commandLine, out string delSweWord, out string delEngWord);
                    if (commandLine.Length == 1 || commandLine.Length == 3)
                        try { removeWord(delSweWord, delEngWord); }
                        catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "translate")
                {
                    if (commandLine.Length == 2)
                    { TranslateWord(commandLine[1]); }
                    else if (commandLine.Length == 1)
                    {
                        Console.Write("Write word to be translated: ");
                        string wordToTranslate = Console.ReadLine();
                        TranslateWord(wordToTranslate);
                    }
                    else { Console.WriteLine("Wrong input. Write translate and press enter"); }
                }
                else if (command == "help")
                {
                    Console.WriteLine("Allowed commands:");
                    Console.Write("<quit> - end this program\n<load> - load a file\n<load> <filename> - load a specific file\n<list> - list all posts in program\n<new> - " +
                                  "add new post\n<new> <swedish word> <english word> - add new post\n<delete> - delete post\n<delete> <swedish word> <english word> - " +
                                  "delete specific post\n<translate> - translate a word \n<translate> <word> - translate a specific word \n ");
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }
        private static void wordCheck(string[] commandLine, out string sweWord, out string engWord)
        {
            if (commandLine.Length == 3) { sweWord = commandLine[1]; engWord = commandLine[2]; }
            else if (commandLine.Length == 1)
            {
                Console.Write("Write word in Swedish: ");
                sweWord = Console.ReadLine();
                Console.Write("Write word in English: ");
                engWord = Console.ReadLine();
            }
            else { Console.WriteLine("Wrong input. <help> for help"); sweWord = ""; engWord = ""; }
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
        private static void TranslateWord(string word)
        {
            try
            {   int wordexist = 0;
                foreach (SweEngGloss gloss in dictionary)
                {
                    if (gloss.word_swe == word)
                    { Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}"); wordexist++; }
                    if (gloss.word_eng == word)
                    { Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}"); wordexist++; }
                }
                if (wordexist == 0) Console.WriteLine($"The word {word} doesn´t exist in dictionary");
            }
            catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
        }
    }
}