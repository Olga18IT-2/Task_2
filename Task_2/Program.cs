using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Task_2
{
    class Program
    {
        public static string GetNumber(string text, string condition)
        {
            Regex r = new Regex(condition);
            var temp = r.Matches(text).Cast<Match>().Select(a => a.Value).ToArray();
            if (temp.Length > 0)
            {
                return temp[0];
            }
            else
            {
                return "";
            }
        }

        static string InputNumber()
        {
            while (true)
            {
                string number = Console.ReadLine();
                if (number.ToLower().Replace("\"", "") == "exit")
                {
                    Environment.Exit(0);
                }

                if (number != "" && GetNumber(number, @"[-]?\d+") == number)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine(" Error ! Repeat entering the number :");
                }

            }
        }

        static string InputSignOfOperation()
        {
            while (true)
            {
                string operation = Console.ReadLine();
                if (operation.ToLower().Replace("\"", "") == "exit")
                {
                    Environment.Exit(0);
                }

                if (operation != "" && GetNumber(operation, @"[+-/*//]") == operation)
                {
                    return operation;
                }
                else
                {
                    Console.WriteLine(" Error ! Repeat entering the sign of the arithmetic operation ( +, -, *, /) :");
                }
            }
        }

        static void Calculate(string number_1, string operation, string number_2)
        {
            MyBigInteger a1 = new MyBigInteger(number_1);
            MyBigInteger b1 = new MyBigInteger(number_2);
            MyBigInteger result1;
            MyBigInteger ost1;
            BigInteger a2 = BigInteger.Parse(number_1);
            BigInteger b2 = BigInteger.Parse(number_2);
            BigInteger result2;
            BigInteger ost2;

            switch (operation)
            {
                case "+":
                    {
                        result1 = a1.Add(b1);
                        result2 = BigInteger.Add(a2, b2);
                        Console.WriteLine(" Method 1 (using my own class MyBigInteger)    = {0}", result1.WriteNumber());
                        Console.WriteLine(" Method 2 (using an existing class BigInteger) = {0}", result2);
                        Console.WriteLine("------------------------------\n");
                        break;
                    }
                case "-":
                    {
                        result1 = a1.Subtract(b1);
                        result2 = BigInteger.Subtract(a2, b2);
                        Console.WriteLine(" Method 1 (using my own class MyBigInteger)    = {0}", result1.WriteNumber());
                        Console.WriteLine(" Method 2 (using an existing class BigInteger) = {0}", result2);
                        Console.WriteLine("------------------------------\n");
                        break;
                    }
                case "*":
                    {
                        result1 = a1.Multiply(b1);
                        result2 = BigInteger.Multiply(a2, b2);
                        Console.WriteLine(" Method 1 (using my own class MyBigInteger)    = {0}", result1.WriteNumber());
                        Console.WriteLine(" Method 2 (using an existing class BigInteger) = {0}", result2);
                        Console.WriteLine("------------------------------\n");
                        break;
                    }
                case "/":
                    {
                        a1.DivRem(b1, out result1, out ost1);
                        if (!b1.IsZero())
                        {
                            result2 = BigInteger.DivRem(a2, b2, out ost2);
                            Console.WriteLine(" Method 1 (using my own class MyBigInteger)    = {0} (остаток {1})", result1.WriteNumber(), ost1.WriteNumber());
                            Console.WriteLine(" Method 2 (using an existing class BigInteger) = {0} (остаток {1})", result2, ost2);
                        }
                        
                        Console.WriteLine("------------------------------\n");
                        break;
                    }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter \"exit\" to close the application");

                Console.WriteLine("Enter the first number (use the digits and - (if necessary) for a negative number) :");
                string number_1 = InputNumber();

                Console.WriteLine("Enter the sign of the arithmetic operation ( +, -, *, /) :");
                string operation = InputSignOfOperation();

                Console.WriteLine("Enter the second number (use the digits and - (if necessary) for a negative number) :");
                string number_2 = InputNumber();

                Calculate(number_1, operation, number_2);
                
            }

        }
    }
}
