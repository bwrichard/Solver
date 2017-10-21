using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Solver
{
    class Program
    {
        const string FileName = "WordsByFrequency1K.txt";
        public static List<string> dictionary = new List<string>();
        public static List<string> dictionaryLength1 = new List<string>();
        public static List<string> dictionaryLength2 = new List<string>();        
        public static List<string> dictionaryLength3 = new List<string>();        
        public static List<string> dictionaryLength4 = new List<string>();
        public static List<string> dictionaryLength5 = new List<string>();
        public static List<string> dictionaryLength6 = new List<string>();
        public static List<string> dictionaryLength7 = new List<string>();
        public static List<string> dictionaryLength8 = new List<string>();
        public static List<string> dictionaryLength9 = new List<string>();
        public static List<string> dictionaryLength10 = new List<string>();

        //const string CypherText = "j kjlm nmo lmoot mpq rosl";
        //const string CypherText = "c ovjff ycn hnmb baelw pjbun uek c ovjff un hnmb baelw";
        //        const string CypherText = "j kjlm nmo lmoot mpq rosl ptq lxprr mopql";
        //const string CypherText = "iocpc jpc xk ajte gckgmc st ioc zkpmf zsio xk msiimc zok ujpcx zoe eky fcusfc ik ocmg";//4.5 Seconds: there are so many people in the world with -- found "there are go many people in the would with"
        //const string CypherText = "iocpc jpc xk ajte gckgmc st ioc zkbmf";
        //const string CypherText = "vpkw swpsdw oftrm t dppm dtmw";  // 2:42, then 38, then :28   "some people think I look like" 
        //const string CypherText = "swpsdw oftrm t dppm dtmw";  // can't find this.  Should be "some people think I look like" 
        //const string CypherText = "vpkw swpsdw oftrm t dppm dtmw n vbwwo sponop"; // this one takes 1:10:42 unoptimized, now 15:37 -- "some people think I look like a sweet potato"
        //const string CypherText = "vpkw swpsdw oftrm t dppm dtmw";

        const string CypherText = "ywf ilemxfz ael uxx cezfo rd mn wec cf xeet";

        //const string CypherText = "dyj me mc l ab rjc me ea hawfv";

        //const string CypherText = "iocpc jpc xk ajte gckgmc st ioc zkpmf zsio xk msiimc zok ujpcx zoe eky fcusfc ik ocmg";
        //const string CypherText = "as zjykyzrdk ge lykfc yep kdzkdyruge uc rjd zgauz kdhudm ue y zgadps cgkr gm edt mgk ad yep kdyhhs mne";
        // const string CypherText = "iocpc jpc ajte";

        List<string> Original = new List<string> { };
        static List<CypherKeyList> CrossRef = new List<CypherKeyList>();

        public static char piorLetter { get; private set; }

        static char currentCypherLetter;

        static List<string> cypherWords = new List<string>();
        static List<string> solutionWords = new List<string>();
        //private static char priorLetter;

        public char CurrentLetter { get => currentCypherLetter; set => currentCypherLetter = value; }

        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Init();
            int i = 0;
            do
            {
                AddLetter();
                if (!FoundInDictionary())
                {
                    SubtractLetter();
                }
                i++;
                if (i>9999)
                {
                    Console.Write(".");
                    //PrintActivity();
                    //PrintDebug(false);
                    i = 0;
                }
                //PrintActivity();
                //PrintDebug(false);

            } while (!(FoundInDictionary() && cypherComplete()));


            //TODO: currently, if no solution is found this will run forever.  There are no reasonable-ness checks, and no way to skip a word which may just not be in our dictionary.
            // Also, don't support special characters.
            // And, won't solve with a big dictionary.  Getting sidetracked somewhere.

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            PrintDebug(true);
            PrintActivity();
            Console.WriteLine();

        }

        private static void PrintActivity()
        {
            foreach (var c in CrossRef)
            {
                Console.Write("Cypher:");
                Console.Write(c.cypherLetter.ToString() + " "); 
                Console.Write("Clear:");
                Console.Write(c.clearLetter.ToString() + " ");
                Console.Write("Tried:");
                foreach (var tried in c.triedLetters)
                {
                    Console.Write(tried + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            //Console.ReadLine();
        }

        static void Init()
        {
            // load up the cypherkeylist
            //todo: put this in the constructor
            for (char c = 'a'; c <= 'z'; c++)
            {
                if (CypherText.Contains(c.ToString()))
                {
                    CypherKeyList temp = new CypherKeyList();
                    temp.cypherLetter = c;
                    CrossRef.Add(temp);
                }
            }
            // Create the Cypher Word List
            foreach (string word in CypherText.Split(' '))
            {
                cypherWords.Add(word);
            }
            // Create the Solution Word List
            foreach (string word in cypherWords)
            {
                string newWord = "";
                foreach (char c in word)
                {
                    newWord += '-';
                }
                solutionWords.Add(newWord);
            }

            dictionary = File.ReadAllLines(FileName).ToList();

            // this speed up the performance by 80%
            for(int i=0; i<dictionary.Count;i++)
            {
                string temp = "";
                temp = dictionary[i].ToLower();
                dictionary[i] = temp;

                switch (dictionary[i].Length)
                {
                    case 1:
                        dictionaryLength1.Add(dictionary[i]);
                        break;
                    case 2:
                        dictionaryLength2.Add(dictionary[i]);
                        break;
                    case 3:
                        dictionaryLength3.Add(dictionary[i]);
                        break;
                    case 4:
                        dictionaryLength4.Add(dictionary[i]);
                        break;
                    case 5:
                        dictionaryLength5.Add(dictionary[i]);
                        break;
                    case 6:
                        dictionaryLength6.Add(dictionary[i]);
                        break;
                    case 7:
                        dictionaryLength7.Add(dictionary[i]);
                        break;
                    case 8:
                        dictionaryLength8.Add(dictionary[i]);
                        break;
                    case 9:
                        dictionaryLength9.Add(dictionary[i]);
                        break;
                    case 10:
                        dictionaryLength10.Add(dictionary[i]);
                        break;
                }
            }

            // try loading smaller dictionaries with just the words of specified lenths
            // re-processing through this added about one minute to a 37 second process. Ouch!
            //foreach(var word in dictionary)
            //{
            //    switch (word.Length)
            //    {
            //        case 1:
            //            dictionaryLength1.Add(word);
            //            break;
            //        case 2:
            //            dictionaryLength2.Add(word);
            //            break;
            //        case 3:
            //            dictionaryLength3.Add(word);
            //            break;
            //        case 4:
            //            dictionaryLength4.Add(word);
            //            break;
            //        case 5:
            //            dictionaryLength5.Add(word);
            //            break;
            //    }
                    
            //}

        }

        private static void AddLetter()
        {
            // find the letter with a blank clearLetter
            //  use crossref
            char letterToAdd = 'a';
            foreach (var cypher in CrossRef)
            {
                // if this one doesn't have a letter, it must be our next target.  We've already cleared it in the miss,
                // so we still need to check the tried letters list
                if (cypher.clearLetter == '\0')
                {
                    // Now, add the next letter we haven't already tried and get out

                    // TODO: check to see if we already have this letter in our Cypher List.  Can't have two A's for example.
                    // so, what we really need to do is get the first letter that is not in triedLetters and is not in cypher.clearLetter

                    //TODO: Test and Refactor this

                    // check for the tried letters list
                    if (cypher.triedLetters.Count == 0)
                    {
                        // haven't missed any yet for this one
                        // get the first letter that is not in cypher.clearLetter
                        for (char c = 'a'; c <= 'z'; c++)
                        {
                            //var item = cypherWords.Find( x => x.clearLetter == c);
                            // TODO: get this extension method working. For now will loop.
                            bool isFound = false;
                            foreach (var tempCypher in CrossRef)
                            {
                                if (c == tempCypher.clearLetter)
                                { //already using this one, so skip
                                    isFound = true;
                                }
                            }
                            if (!isFound)
                            {
                                letterToAdd = c;
                                break;
                            }
                        }
                    }
                    else
                    {
                        //TODO: Test and Refactor this
                        // bug: we're going past z through the full character set

                        // Add the next letter one greater than the largest in the triedLetter list
                        letterToAdd = cypher.triedLetters[cypher.triedLetters.Count - 1];
                        // See if this letter is already used in our clear letters
                        letterToAdd++;
                        for (char l = letterToAdd; l <= 'z'; l++)
                        {
                            // iterate through the rest of the alphabet
                            // this actually leaves a potential logic flaw where we may miss gaps.
                            bool isFound = false;
                            foreach (var tempCypher in CrossRef)
                            {
                                if (l == tempCypher.clearLetter)
                                { //already using this one, so skip
                                    isFound = true;
                                }
                            }
                            if (!isFound)
                            {
                                letterToAdd = l;
                                break;
                            }
                        }
                    }
                    cypher.clearLetter = letterToAdd;
                    cypher.triedLetters.Add(letterToAdd);
                    piorLetter = currentCypherLetter;
                    currentCypherLetter = cypher.cypherLetter;
                    // Now add this letter to our proposed solution
                    int i = 0;
                    foreach (var word in cypherWords)
                    {
                        int j = 0;
                        foreach (char c in word)
                        {
                            if (c == currentCypherLetter)
                            { string tempWord = "";
                                // Replace this one in the solution word list
                                tempWord = solutionWords[i];
                                solutionWords[i] = tempWord.ReplaceAt(j, cypher.clearLetter);
                            }
                            j++;
                        }
                        i++;

                    }
                    break;
                }
            }
        }


        private static bool FoundInDictionary()
        {
            // Compare every word against our dictionary

            // If there are any letters that don't line up with at least one word, 
            // we fail and need to roll back the last letter tried
            foreach (var solutionWord in solutionWords)
            {
                // if there are no letters in this word, skip it
                if (!(solutionWord.ContainsOnly('-')))
                {
                    if (!FindThisWordinDictionary(solutionWord))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool FindThisWordinDictionary(string solutionWord)
        {
            //// iterate through every word in dictionary
            //foreach (string dictionaryWord in dictionary)
            //{
            //    if (solutionWord.Length == dictionaryWord.Length)
            //    {
            //        if (IsWordMatched(solutionWord, dictionaryWord))
            //        {
            //            return true;
            //        }
            //    }
            //}

            // This saved about 30%, so it's a good optimization
            List<string> lengthList = dictionary;
            switch (solutionWord.Length)
            {
                case 1:
                    foreach (string dictionaryWord in dictionaryLength1)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 2:
                    foreach (string dictionaryWord in dictionaryLength2)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 3:
                    foreach (string dictionaryWord in dictionaryLength3)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 4:
                    foreach (string dictionaryWord in dictionaryLength4)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 5:
                    foreach (string dictionaryWord in dictionaryLength5)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 6:
                    foreach (string dictionaryWord in dictionaryLength6)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 7:
                    foreach (string dictionaryWord in dictionaryLength7)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 8:
                    foreach (string dictionaryWord in dictionaryLength8)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 9:
                    foreach (string dictionaryWord in dictionaryLength9)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;
                case 10:
                    foreach (string dictionaryWord in dictionaryLength10)
                    {
                        if (IsWordMatched(solutionWord, dictionaryWord))
                        {
                            return true;
                        }
                    }
                    break;                
            }
            // iterate through every word in correct length dictionary
            //foreach (string dictionaryWord in dictionaryLength1)
            //{
            //    if (IsWordMatched(solutionWord, dictionaryWord))
            //    {
            //        return true;
            //    }
            //}



            return false;
        }

        private static bool IsWordMatched(string sWord, string dWord)
        {
            // check any chars not "-" for a match
            int j = 0;
            // check every character for a match. Don't worry about '-'s because they just won't match, which is fine
            foreach (char c in sWord)
            {
                // if the letters don't match, and it's not blank, flag as nomatch
                if (c.ToString() != dWord.Substring(j, 1) && c != '-')
                {
                    return false;  // failed for this word
                }
                j++;
            }
            return true;
        }

        private static void SubtractLetter()
        {
            // subtract one letter from the last cypher - didn't have any matches in the dictionary.
            // we've already added the letter to triedLetters for this cypher, so just need to clear it
            // Subtract two letters. So, clear the clearLetter and TriedLetters from the last cypher. Then do SubtractLetter for prior cypher.
            int i = 0;
            foreach (var cypher in CrossRef)
            {
                if (cypher.cypherLetter == currentCypherLetter)
                {
                    if (cypher.clearLetter == 'z' && i == 0)
                    {
                        // We've failed.  Message and quit.
                        Console.WriteLine("No matching patterns found in dictionary.  We give up.");
                        PrintDebug(false);
                        PrintActivity();
                        Console.ReadLine();
                        break;
                    }
                    if (cypher.clearLetter >= 'z')
                    { // also need to back up one
                        cypher.triedLetters = new List<char>();
                        CrossRef[i-1].clearLetter = '\0';
                    }
                    cypher.clearLetter = '\0';
                    // Now remove this letter from our proposed solution
                    int p = 0;
                    foreach (var word in cypherWords)
                    {
                        int j = 0;
                        foreach (char c in word)
                        {
                            if (c == cypher.cypherLetter)
                            {
                                string tempWord = "";
                                // Replace this one in the solution word list
                                tempWord = solutionWords[p];
                                solutionWords[p] = tempWord.ReplaceAt(j, '-');
                            }
                            j++;
                        }
                        p++;
                    }
                }
                i++;
            }

        }

        private static bool cypherComplete()
        {
            // check to see if the cypher is complete - all clearLetters are filled
            if ((CrossRef[CrossRef.Count - 1].clearLetter != '\0')) return true;
            return false;
        }

        private static void PrintDebug(bool stopForInput)
        {
            Console.WriteLine(CypherText);
            Console.WriteLine("CypherKeyList:");
            foreach (var c in CrossRef)
            {
                Console.Write(c.cypherLetter.ToString() + " ");
            }
            Console.WriteLine();
            foreach (var c in CrossRef)
            {
                Console.Write(c.clearLetter.ToString() + " ");
            }
            //Console.WriteLine();
            //Console.WriteLine("Dictionary:");
            //foreach(var word in dictionary)
            //{
            //    Console.Write(word + " ");

            //}
            Console.WriteLine();
            Console.WriteLine("SolutionWords:");
            Console.WriteLine();
            foreach (var word in solutionWords)
            {
                Console.Write(word + " ");

            }
            Console.WriteLine();
            if (stopForInput)
            {
                Console.Read();
            }
        }
    }
}
