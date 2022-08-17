using System;
using System.Collections.Generic;
using System.Text;

namespace FixingTheCase
{
    class Program
    {
        public enum StringState
        {
            CorrectInput, FirstLetterWrong, AllLettersWrong // TODO refactor for fixing not just the first letter of the name based on the string state
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter a set of names, separated by a comma");

            string originalInput = Console.ReadLine();

            bool hasEnteredADigit = CheckForNumbersInTheInput(originalInput);

            if (!hasEnteredADigit)
            {
                RemoveTheWhiteSpace(ref originalInput);

                string[] inputNames = SeparateTheInput(ref originalInput);

                EvaluateInputString(inputNames);

                var namesList = FixTheNameCases(inputNames);

                Console.WriteLine("This is a list of names you entered with the correct case:");

                Console.WriteLine(OutputTheNameString(namesList));

            }
            else
            {
                Console.WriteLine("You've entered a number, please repeat your input...\n");
            }

            Console.Read();

            Console.WriteLine("Press enter to terminate the application...");
        }

        static void RemoveTheWhiteSpace(ref string inputString)
        {
            StringBuilder reconstructedString = new StringBuilder(inputString.Length);

            for (int i = 0; i < inputString.Length; i++)
            {
                char whiteSpace = ' ';

                char? currentCharacter = inputString[i];

                if (inputString[i] == whiteSpace)
                    currentCharacter = null;

                if (currentCharacter != null)
                    reconstructedString.Append(currentCharacter);
            }

            inputString = reconstructedString.ToString();
        }

        static string[] SeparateTheInput(ref string inputString)
        {
            char separator = ',';

            string[] separatedStrings = inputString.Split(separator);

            return separatedStrings;
        }

        static List<string> FixTheNameCases(string[] inputNames) // TODO refactor for different string evaluation types
        {
            List<String> listOfNames = new List<String>();

            foreach (string aName in inputNames)
            {
                char firstLetter = aName[0];

                StringBuilder tempStringContainer = new StringBuilder(aName.Length);

                if (Char.IsLower(firstLetter))
                {
                    var a = firstLetter.ToString().ToUpper();
                    var b = aName.Substring(1, aName.Length - 1);

                    tempStringContainer.Append(a + b);

                    listOfNames.Add(tempStringContainer.ToString());
                }
                else
                {
                    tempStringContainer.Append(aName);

                    listOfNames.Add(tempStringContainer.ToString());
                }
            }

            return listOfNames;
        }

        static string OutputTheNameString(List<string> listOfNames)
        {
            StringBuilder outputNames = new StringBuilder();

            foreach (string aName in listOfNames)
            {
                string returnedName = aName + ", ";

                outputNames.Append(returnedName);
            }

            return outputNames.ToString(0, outputNames.Length - 2);
        }

        static bool CheckForNumbersInTheInput(string stringInput)
        {
            foreach(char aCharacter in stringInput)
            {
                if (char.IsDigit(aCharacter))
                    return true;
            }

            return false;
        }

        static List<int> EvaluateInputString(string[] startingInput)
        {
            List<int> evaluatedStringTypes = new List<int>();

            bool isStringCorrect = false;
            bool isFirstLetterCorrect = false;

            int currentIteration = 0;

            foreach (string aString in startingInput)
            {
                Console.WriteLine("The current iteration is " + currentIteration); // TODO delete later

                if (Char.IsLower(aString[0]))
                {
                    evaluatedStringTypes.Add((int)StringState.FirstLetterWrong);
                    Console.WriteLine("first lower letter wrong string added " + evaluatedStringTypes[currentIteration]); // TODO delete later
                }
                else
                    isFirstLetterCorrect = true;

                for (int i = 1; i < aString.Length; i++)
                {
                    if(Char.IsUpper(aString[i]))
                    {
                        if(!isFirstLetterCorrect)
                        {
                            Console.WriteLine("Removed the " + evaluatedStringTypes[currentIteration]); // TODO delete later
                            evaluatedStringTypes.RemoveAt(currentIteration);
                        }

                        evaluatedStringTypes.Add((int)StringState.AllLettersWrong);
                        isStringCorrect = false;
                        Console.WriteLine("Non-first letter string added " + evaluatedStringTypes[currentIteration]); // TODO delete later
                        break;
                    }

                    else
                    {
                        isStringCorrect = true;
                    }

                }

                if(isStringCorrect && isFirstLetterCorrect)
                {
                    evaluatedStringTypes.Add((int)StringState.CorrectInput);
                    Console.WriteLine("A correct string has been added " + evaluatedStringTypes[currentIteration]); // TODO delete later
                }

                isFirstLetterCorrect = false;
                isStringCorrect = false;
                currentIteration++;
            }

            // TODO delete later
            for (int i = 0; i < evaluatedStringTypes.Count; i++)
            {
                Console.WriteLine(evaluatedStringTypes[i]);
            }

            return evaluatedStringTypes;
        }
    }
}