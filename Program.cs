﻿namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
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
                {
                    Console.WriteLine("Goodbye!"); break;
                }
                else if (command == "load")
                {
                    if(argument.Length == 2) //FIXME kontrollera att filen existerar
                    {
                        using (StreamReader sr = new StreamReader(argument[1]))
                        {
                            dictionary = new List<SweEngGloss>(); // Empty it!
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                SweEngGloss gloss = new SweEngGloss(line);          //TODO göra static function
                                dictionary.Add(gloss);
                                line = sr.ReadLine();
                            }
                        }
                    }
                    else if(argument.Length == 1)
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
                }
                else if (command == "list")
                {
                    foreach(SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                    }
                }
                else if (command == "new")
                {
                    if (argument.Length == 3) //TODO kontrollera att inmatning av användare är korrekt
                    {
                        dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                    }
                    else if(argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string sweWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string engWord = Console.ReadLine();
                        dictionary.Add(new SweEngGloss(sweWord, engWord));
                    }
                }
                else if (command == "delete")  //TODO kontrollera att inmatning av användare är korrekt
                {
                    if (argument.Length == 3)
                    {
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++) {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == argument[1] && gloss.word_eng == argument[2])
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string sweWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string engWord = Console.ReadLine();
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.word_swe == sweWord && gloss.word_eng == engWord)
                                index = i;
                        }
                        dictionary.RemoveAt(index);
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)  //FIXME kontrollera att ordet finns annars ge felmedelande 
                    {
                        TranslateWord(argument);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string wordToTranslate = Console.ReadLine();
                        TranslateWord(argument);
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

        private static void TranslateWord(string[] argument)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == argument[1])
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                if (gloss.word_eng == argument[1])
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
            }
        }
    }
}