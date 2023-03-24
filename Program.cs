using System.Linq.Expressions;

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
            Console.WriteLine("Welcome to the dictionary app! \n<help> for allowed commands ");
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split(); //FIXME kontroll felaktig inmatning från användare
                string command = argument[0];
                if (command == "quit")
                { Console.WriteLine("Goodbye!"); break; }
                else if (command == "load")
                {
                    if (argument.Length == 1) loadList(defaultFile);
                    else try {
                            { defaultFile = "..\\..\\..\\dict\\" + argument[1];
                                loadList(defaultFile);
                            }
                        } catch (System.IO.FileNotFoundException) { defaultFile = "..\\..\\..\\dict\\sweeng.lis"; Console.WriteLine($"File {defaultFile} didn´t exist"); }
                }
                else if (command == "list")
                {   try {
                        foreach (SweEngGloss gloss in dictionary)
                        { Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}"); }
                    } catch (System.NullReferenceException) { Console.WriteLine($"Empty list, load a list before using this command"); }
                }
                else if (command == "new")
                {
                    if (argument.Length == 3) //TODO kontrollera att inmatning av användare är korrekt
                    {dictionary.Add(new SweEngGloss(argument[1], argument[2]));}
                    else if (argument.Length == 1)
                    {
                        string sweWord, engWord;
                        wordInput(out sweWord, out engWord);
                        dictionary.Add(new SweEngGloss(sweWord, engWord));
                    }
                    else Console.WriteLine("Felaktig inmatning. Använd new och därefter enter");
                }
                else if (command == "delete")  //TODO kontrollera att inmatning av användare är korrekt
                {
                    if (argument.Length == 3)
                    {
                        string sweWord = argument[1];
                        string engWord = argument[2];
                        removeWord(sweWord, engWord);
                    }
                    else if (argument.Length == 1)
                    {
                        string sweWord, engWord;
                        wordInput(out sweWord, out engWord);
                        removeWord(sweWord, engWord);
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)  //FIXME kontrollera att ordet finns annars ge felmedelande 
                    {
                        TranslateWord(argument[1]);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string wordToTranslate = Console.ReadLine();
                        TranslateWord(wordToTranslate);
                    }
                }
                else if (command == "help")
                {
                    Console.WriteLine("Allowed commands:");
                    Console.Write("quit - for end this program\nload - load a file\nlist - list all posts in program\nnew - add new post\ndelete - delete post\ntranslate - translate av word  ");
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }   //NYI Help funktion
            while (true);
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
            dictionary.RemoveAt(index);
        }

        private static void wordInput(out string sweWord, out string engWord)
        {
            Console.Write("Write word in Swedish: ");
            sweWord = Console.ReadLine();
            Console.Write("Write word in English: ");
            engWord = Console.ReadLine();
        }

        private static void TranslateWord(string argument)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == argument)
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                if (gloss.word_eng == argument)
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
            }
        }
    }
}