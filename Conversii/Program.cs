using System;

namespace NumberConversion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the number to convert: ");
            string number = Console.ReadLine();

            Console.Write("Enter the base of the input number (2-16): ");
            int fromBase = int.Parse(Console.ReadLine());

            Console.Write("Enter the target base (2-16): ");
            int toBase = int.Parse(Console.ReadLine());

            string result = ConvertFromBaseToBase(number, fromBase, toBase);
            Console.WriteLine($"Result of conversion: {result}");
        }

        static double ConvertToBase10(string number, int sourceBase)
        {
            double result = 0;

            int integerPartEndIndex = number.IndexOf('.');
            if (integerPartEndIndex == -1)
                integerPartEndIndex = number.Length;

            for (int i = 0; i < integerPartEndIndex; i++)
            {
                char digit = number[i];
                int digitValue = CharToDigitValue(digit);
                result = result * sourceBase + digitValue;
            }

            if (integerPartEndIndex < number.Length - 1)
            {
                double fraction = 1.0 / sourceBase;

                for (int i = integerPartEndIndex + 1; i < number.Length; i++)
                {
                    char digit = number[i];
                    int digitValue = CharToDigitValue(digit);
                    result += digitValue * fraction;
                    fraction /= sourceBase;
                }
            }

            return result;
        }

        static string ConvertFrom10(double decimalValue, int targetBase)
        {
            string result = "";

            int integerPart = (int)decimalValue;
            double fractionalPart = decimalValue - integerPart;

            result = ConvertIntegerPart(integerPart, targetBase);

            if (fractionalPart > 0)
            {
                result += ".";
                result += ConvertFractionalPart(fractionalPart, targetBase);
            }

            return result == "" ? "0" : result;
        }

        static string ConvertIntegerPart(int integerPart, int targetBase)
        {
            if (integerPart == 0)
                return "0";

            string result = "";
            while (integerPart > 0)
            {
                int remainder = integerPart % targetBase;
                char digit = DigitValueToChar(remainder);
                result = digit + result;
                integerPart /= targetBase;
            }

            return result;
        }

        static string ConvertFractionalPart(double fractionalPart, int targetBase)
        {
            const int maxFractionalDigits = 8;
            string result = "";

            for (int i = 0; i < maxFractionalDigits; i++)
            {
                fractionalPart *= targetBase;
                int digit = (int)fractionalPart;
                fractionalPart -= digit;
                result += DigitValueToChar(digit);
            }

            return result;
        }

        static int CharToDigitValue(char digit)
        {
            if (Char.IsDigit(digit))
                return int.Parse(digit.ToString());
            else
                return char.ToUpper(digit) - 'A' + 10;
        }

        static char DigitValueToChar(int digitValue)
        {
            if (digitValue < 10)
                return digitValue.ToString()[0];
            else
                return (char)('A' + digitValue - 10);
        }

        static string ConvertFromBaseToBase(string number, int sourceBase, int targetBase)
        {
            bool isNegative = false;

            if (number[0] == '-')
            {
                isNegative = true;
                number = number.Substring(1);
            }

            double decimalValue = ConvertToBase10(number, sourceBase);
            string result = ConvertFrom10(decimalValue, targetBase);

            if (isNegative)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}

