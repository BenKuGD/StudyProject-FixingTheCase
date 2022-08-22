using System;
using System.Collections.Generic;
using System.Text;

namespace FixingTheCase
{
    class Program
    {
        public enum StringState
        {
            CorrectInput, FirstLetterWrong, AllLettersWrong 
        }

        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Enter a set of names, separated by a comma");

                string originalInput = Console.ReadLine();

                bool hasEnteredADigit = CheckForNumbersInTheInput(originalInput);

                bool isValidInput = OriginalInputValidation(originalInput);

                if (!hasEnteredADigit && isValidInput)
                {
                    RemoveTheWhiteSpace(ref originalInput);

                    RemoveUnnecessaryCharacters(ref originalInput);

                    string[] inputNames = SeparateTheInput(ref originalInput, out bool containsAnEmptyString);

                    if (containsAnEmptyString)
                    {
                        Console.WriteLine("There was an issue with your input, terminating");
                        break;
                    }

                    List<int> evaluatedStringTypes = EvaluateInputString(inputNames);

                    var namesList = FixTheNameCases(inputNames, evaluatedStringTypes);

                    Console.WriteLine("This is a list of names you entered with the correct case:");

                    Console.WriteLine(OutputTheNameString(namesList));

                    break;

                }
                else
                {
                    Console.WriteLine("Incorrect input (no names provided or numbers in input were given). \nPlease try again...");
                }
            }

            Console.WriteLine("\nPress enter to terminate the application...");

            Console.Read();
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

        static void RemoveUnnecessaryCharacters(ref string inputString)
        {
            StringBuilder stringContainer = new StringBuilder(inputString.Length);

            char separator = ',';

            foreach(char aCharacter in inputString)
            {
                if (Char.IsLetter(aCharacter) || aCharacter == separator)
                    stringContainer.Append(aCharacter);
            }

            inputString = stringContainer.ToString();
        }

        static string FixTheFirstLetter(string stringToFix)
        {
            char firstLetter = stringToFix[0];

            firstLetter = char.ToUpper(firstLetter);

            string fixedString = string.Concat(firstLetter, stringToFix.Substring(1, stringToFix.Length - 1));

            return fixedString;
        }

        static string MakeAllTheLettersLower(string stringToChange)
        {
            StringBuilder stringContainer = new StringBuilder();

            foreach(char letter in stringToChange)
            {
                char loweredLetter = Char.ToLower(letter);
                stringContainer.Append(loweredLetter);
            }

            return stringContainer.ToString();
        }

        static string[] SeparateTheInput(ref string inputString, out bool hasAnEmptyString)
        {
            char separator = ',';

            string[] separatedStrings = inputString.Split(separator);

            bool containsAnEmptyString = false;

            foreach(string aString in separatedStrings)
            {
                if (String.IsNullOrEmpty(aString))
                    containsAnEmptyString = true;
            }

            hasAnEmptyString = containsAnEmptyString;

            return separatedStrings;
        }

        static List<string> FixTheNameCases(string[] inputNames, List<int> stringTypes) 
        {
            List<string> listOfNames = new List<string>();

            int currentIteration = 0;

            foreach (string aName in inputNames)
            {
                StringBuilder tempStringContainer = new StringBuilder(aName.Length);

                StringState aNameState = (StringState)stringTypes[currentIteration];

                if (currentIteration > stringTypes.Count)
                    break;

                switch(aNameState)
                {
                    case StringState.CorrectInput:
                        tempStringContainer.Append(aName);
                        listOfNames.Add(tempStringContainer.ToString());
                        currentIteration++;
                        continue;

                    case StringState.FirstLetterWrong:
                        tempStringContainer.Append(FixTheFirstLetter(aName));
                        listOfNames.Add(tempStringContainer.ToString());
                        currentIteration++;
                        continue;

                    case StringState.AllLettersWrong:
                        var correctedName = MakeAllTheLettersLower(aName);
                        tempStringContainer.Append(FixTheFirstLetter(correctedName));
                        listOfNames.Add(tempStringContainer.ToString());
                        currentIteration++;
                        continue;

                    default:
                        throw new Exception("Input was ambiguous");
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

        static bool OriginalInputValidation(string originalInput)
        {
            int numberOfLettersEntered = 0;

            foreach(char aCharacter in originalInput)
            {
                if (Char.IsLetter(aCharacter))
                    numberOfLettersEntered++;
            }

            if (numberOfLettersEntered > 0)
                return true;

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
                if (Char.IsLower(aString[0]))
                {
                    evaluatedStringTypes.Add((int)StringState.FirstLetterWrong);
                }
                else
                    isFirstLetterCorrect = true;

                for (int i = 1; i < aString.Length; i++)
                {
                    if(Char.IsUpper(aString[i]))
                    {
                        if(!isFirstLetterCorrect)
                        {
                            evaluatedStringTypes.RemoveAt(currentIteration);
                        }

                        evaluatedStringTypes.Add((int)StringState.AllLettersWrong);
                        isStringCorrect = false;
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
                }

                isFirstLetterCorrect = false;
                isStringCorrect = false;
                currentIteration++;
            }

            return evaluatedStringTypes;
        }
    }
}