using CsvHelper;
using System.Globalization;

namespace GameTrackerEx01
{
    public static class Format
    {
        public static string CheckForCommas(string input, string inputPurpose)
        {
            if (input.Length < 1)
            {
                Console.WriteLine($"Your input did not contain enough characters.\nPlease re-enter the {inputPurpose} below");

                return CheckForCommas(Console.ReadLine(), inputPurpose);
            }
            if (input.Split(",").Length > 1 && input.Replace(",", "").Length > 0)
            {
                Console.WriteLine($"Your input {input} contains a comma.\nWould you rather:\n1. Have us remove the comma for you\n2. Re-enter the {inputPurpose} yourself");

                int userInput = ConvertStringToInt(Console.ReadLine(), $"Would you rather:\n1. Have us remove the comma for you\n2. Re-enter the {inputPurpose} yourself");

                switch (userInput) {
                    case 1 :
                        return input.Replace(",", "");
                    case 2 :
                        return CheckForCommas(Console.ReadLine(), inputPurpose);
                }

            } else if (input.Replace(",", "").Length < 1) 
            {
                Console.WriteLine($"Input contained entirely commas.\nPlease provide valid input for {inputPurpose} below");
                return CheckForCommas(Console.ReadLine(), inputPurpose);
            }
            return input;
        }


        public static int ConvertStringToInt(string input, string question)
        {
            if (int.TryParse(input, out int num))
            {
                return num;
            }
            //Returning an error message to say the input was not covertible into an Integer type
            Console.WriteLine($"Type conversion failed, {input} is not a type: integer");
            Console.WriteLine("Please provide valid integer input.");
            ReAskQuestion(question);
            return ConvertStringToInt(Console.ReadLine(), question);
        }


        public static float ConvertStringToFloat(string input, string question)
        {
            if (float.TryParse(input, out float num))
            {
                return num;
            }
            //Returning an error message to say the input was not covertible into an Integer type
            Console.WriteLine($"Type conversion failed, {input} is not a type: float");
            Console.WriteLine("Please provide valid float input.");
            ReAskQuestion(question);
            return ConvertStringToFloat(Console.ReadLine(), question);
        }

        public static float ConvertStringToCost(string input, string question)
        {
            if (input.Contains(".") && input.Split(".")[1].Length > 2)
            {
                Console.WriteLine($"Input error, {input} has too many decimal places");
                Console.WriteLine("Please provide an cost convertable input");
                ReAskQuestion(question);
                return ConvertStringToCost(Console.ReadLine(), question);
            }
            if (float.TryParse(input, out float num))
            {
                if (!input.Contains("."))
                {
                    input = $"{input}.";
                }
                while (input.Contains(".") && input.Split(".")[1].Length < 2)
                {
                    input = $"{input}0";
                }
                return float.Parse(input);
            }
            Console.WriteLine($"Type conversion failed, {input} is not a type: float");
            Console.WriteLine("Please provide valid cost input.");
            ReAskQuestion(question);
            return ConvertStringToCost(Console.ReadLine(), question);
        }


        public static void ReAskQuestion(string question)
        {
            Console.WriteLine(question);
        }



        public static bool GetClosedAnswer(string question)
        {
            Console.Write($"{question}\n");
            string response = Console.ReadLine().Trim().ToLower();
            //If answer is yes or no, return value
            if (response.ToLower() == "yes" || response.ToLower() == "y")
            {
                return true;
            }
            if (response.ToLower() == "no" || response.ToLower() == "n")
            {
                return false;
            }
            //If answer is not yes or no, notify user and request new input
            Console.WriteLine($"{response} is not a valid input.");
            return GetClosedAnswer(question);
        }

        public static int[] GetManyMenuResponses(int optionQuant)
        {
            string rawInput = Console.ReadLine();
            string[] indexStrings = SeperateCSVLine(rawInput);
            while (indexStrings.Length > optionQuant)
            {
                Console.WriteLine($"{rawInput} contains more than {optionQuant} indexes. Please input fewer options:");
                rawInput = Console.ReadLine();
                indexStrings = SeperateCSVLine(rawInput);
            }
            int[] inputInts = [];
            int[] receivedInputs = [];
            for (int i = 0; i < indexStrings.Length; i++)
            {
                var input = ConvertStringToIntWithErr(indexStrings[i]);
                while(input.err != "" || input.num < 1 || optionQuant < input.num || receivedInputs.Contains(input.num))
                {
                    Console.WriteLine(input.err);
                    if (input.err != "")
                    {
                        input = ConvertStringToIntWithErr(ReAskInput(input.err, "Please select your desired action's number:"));
                    } else if (receivedInputs.Contains(input.num))
                    {
                        input = ConvertStringToIntWithErr(ReAskInput($"{input.num} has already been selected.", "Please select your desired action's number:"));
                    }
                    else
                    {
                        input = ConvertStringToIntWithErr(ReAskInput($"{input.num} is not within the selectable number range.", "Please select your desired action's number:"));
                    }
                }
                inputInts = inputInts.Append(input.num).ToArray();
                receivedInputs = receivedInputs.Append(input.num).ToArray();
            }
            return inputInts;
        }

        public static int GetSingleResponse(int optionCount, string question)
        {
            Console.WriteLine($"There are {optionCount} options available");
            int input = ConvertStringToInt(Console.ReadLine(), question);
            while (input > optionCount || input < 0)
            {
                input = ConvertStringToInt(ReAskInput($"{input} is not within the selectable number range.", "Please select your desired action's number:"), question);
            }
            return input;
        }

        public static string[] SeperateCSVLine(string line)
        {
            //The string is split into an array via the coma values.
            /*NOTE: if one of the values has a coma in it this will 
            bug out due to too many columns/values. */
            string[] values = line.Split(",");
            return values;
        }

        public static string ReAskInput(string q1, string q2)
        {
            Console.WriteLine(q1);
            Console.WriteLine(q2);
            string answer = Console.ReadLine();
            return answer;
        } 

        public static (int num, string err) ConvertStringToIntWithErr(string input)
        {
            if (int.TryParse(input, out int num))
            {
                return (num, "");
            }
            //Returning an error message to say the input was not covertible into an Integer type
            return (-1, $"Type conversion failed, {input} is not a type: integer");
        }



        public static DateTime GetDateValue(string header)
        {
            var dateParseResult = ConvertStringToTime(AskForInput(header), "dd/MM/yyyy");
            while(dateParseResult.err != "")
            {
                Console.WriteLine(dateParseResult.err);
                dateParseResult = ConvertStringToTime(AskForInput(header), "dd/MM/yyyy");
            }
            return dateParseResult.time;
        }

        public static (DateTime time, string err) ConvertStringToTime(string input, string format)
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
            {
                return (time, "");
            }
            //Returning an error message to say the input was not covertible into DateTime type
            return (DateTime.Now, $"Type conversion failed, {input} is not a type: DateTime in format {format}");
        }

        public static string AskForInput(string header)
        {
            Console.WriteLine($"Please enter the {header}:");
            string detail = Console.ReadLine();
            if (detail.Contains(","))
            {
                detail = CheckForCommas(detail, header);
            }
            return detail;
        }

        public static string ReplaceCommasInString(string commaFilledInput)
        {
            return commaFilledInput.Replace(",", "<^&^>");
        }

        public static string ReturnCommasToString(string commaReplacedInput)
        {
            return commaReplacedInput.Replace("<^&^>", ",");
        }

        
    }
}