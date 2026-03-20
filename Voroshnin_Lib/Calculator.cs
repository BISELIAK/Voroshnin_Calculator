using System;
using System.Collections.Generic;
using System.Globalization;

namespace Voroshnin.Lib
{
    public static class Calculator
    {
        public static double Execute(double first, char op, double second)
        {
            switch (op)
            {
                case '+': return first + second;
                case '-': return first - second;
                case '*': return first * second;
                case '/':
                    if (second == 0)
                        throw new DivideByZeroException("Деление на ноль");
                    return first / second;
                case '^': return Math.Pow(first, second);
                default: throw new ArgumentException("Неизвестный оператор");
            }
        }

        public static double Run(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Пустое выражение");

            expression = expression.Replace(",", ".").Replace(" ", "");

            var numbers = new Stack<double>();
            var operators = new Stack<char>();

            int i = 0;
            while (i < expression.Length)
            {
                char c = expression[i];

                if (char.IsDigit(c) || c == '.' || (c == '-' && IsStartOfNegative(expression, i)))
                {
                    string numStr = "";
                    if (c == '-') { numStr += c; i++; }

                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        numStr += expression[i++];
                    }

                    if (double.TryParse(numStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                        numbers.Push(num);
                    else
                        throw new FormatException("Неверное число: " + numStr);
                    continue;
                }
                else if (c == '(')
                {
                    operators.Push(c);
                    i++;
                }
                else if (c == ')')
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                        ProcessOperator(numbers, operators);

                    if (operators.Count == 0)
                        throw new FormatException("Нет открывающей скобки");

                    operators.Pop();
                    i++;
                }
                else if (IsOperator(c))
                {
                    while (operators.Count > 0 &&
                           operators.Peek() != '(' &&
                           GetPriority(operators.Peek()) >= GetPriority(c))
                    {
                        ProcessOperator(numbers, operators);
                    }
                    operators.Push(c);
                    i++;
                }
                else
                {
                    throw new FormatException("Неверный символ: " + c);
                }
            }

            while (operators.Count > 0)
            {
                if (operators.Peek() == '(')
                    throw new FormatException("Нет закрывающей скобки");
                ProcessOperator(numbers, operators);
            }

            if (numbers.Count != 1)
                throw new FormatException("Ошибка вычисления");

            return numbers.Pop();
        }

        private static bool IsStartOfNegative(string expr, int index)
        {
            if (expr[index] != '-') return false;
            if (index == 0) return true;
            char prev = expr[index - 1];
            return prev == '(' || IsOperator(prev);
        }

        private static bool IsOperator(char c) => c == '+' || c == '-' || c == '*' || c == '/' || c == '^';

        private static int GetPriority(char op)
        {
            switch (op)
            {
                case '^': return 3;
                case '*':
                case '/': return 2;
                case '+':
                case '-': return 1;
                default: return 0;
            }
        }

        private static void ProcessOperator(Stack<double> numbers, Stack<char> operators)
        {
            if (numbers.Count < 2)
                throw new FormatException("Недостаточно чисел");

            char op = operators.Pop();
            double b = numbers.Pop();
            double a = numbers.Pop();
            numbers.Push(Execute(a, op, b));
        }
    }
}