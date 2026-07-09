using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sci_cal
{
    public static class ExpressionEvaluator
    {
        public static double Evaluate(string expression)
        {
            Match match = Regex.Match(expression,
                @"^(\d+(\.\d+)?)([\+\-\*/])(\d+(\.\d+)?)%$");

            if (match.Success)
            {
                double left = double.Parse(match.Groups[1].Value);
                string op = match.Groups[3].Value;
                double percent = double.Parse(match.Groups[4].Value);

                switch (op)
                {
                    case "+":
                        return left + (left * percent / 100);

                    case "-":
                        return left - (left * percent / 100);

                    case "*":
                        return left * (percent / 100);

                    case "/":
                        return left / (percent / 100);
                }
            }

            List<string> tokens = Tokenize(expression);

            Queue<string> postfix = ToPostfix(tokens);

            return EvaluatePostfix(postfix);
        }

        private static int Precedence(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;

                case "*":
                case "/":
                    return 2;

                case "^":
                    return 3;

                case "%":
                    return 4;

                default:
                    return 0;
            }
        }

        private static List<string> Tokenize(string expression)
        {
            List<string> tokens = new List<string>();

            string number = "";
            string word = "";

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                // Detect negative numbers
                if (c == '-' &&
                    (i == 0 || "+-*/^(".Contains(expression[i - 1])) &&
                    i + 1 < expression.Length &&
                    (char.IsDigit(expression[i + 1]) || expression[i + 1] == '.'))
                {
                    number = "-";
                    continue;
                }

                if (char.IsDigit(c) || c == '.')
                {
                    if (word != "")
                    {
                        tokens.Add(word);
                        word = "";
                    }

                    number += c;
                }
                else if (char.IsLetter(c))
                {
                    if (number != "")
                    {
                        tokens.Add(number);
                        number = "";
                    }

                    word += c;
                }
                else
                {
                    if (number != "")
                    {
                        tokens.Add(number);
                        number = "";
                    }

                    if (word != "")
                    {
                        tokens.Add(word);
                        word = "";
                    }

                    if (!char.IsWhiteSpace(c))
                    {
                        tokens.Add(c.ToString());
                    }
                }
            }

            if (number != "")
                tokens.Add(number);

            if (word != "")
                tokens.Add(word);

            return tokens;
        }

        private static Queue<string> ToPostfix(List<string> tokens)
        {
            Queue<string> output = new Queue<string>();
            Stack<string> operators = new Stack<string>();

            foreach (string token in tokens)
            {
                double number;

                if (double.TryParse(token, out number))
                {
                    output.Enqueue(token);
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "^" || token == "%")
                {
                    while (operators.Count > 0 &&
                           operators.Peek() != "(" &&
                           Precedence(operators.Peek()) >= Precedence(token))
         
                    {
                        output.Enqueue(operators.Pop());
                    }

                    operators.Push(token);
                }
                else if (token == "sin" ||
                         token == "cos" ||
                            token == "tan" ||
                             token == "sqrt" ||
                            token == "log" ||
                         token == "ln")
                {
                    operators.Push(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Enqueue(operators.Pop());
                    }

                    if (operators.Count > 0)
                        operators.Pop(); // Remove "("

                    if (operators.Count > 0 &&
                       (operators.Peek() == "sin" ||
     operators.Peek() == "cos" ||
     operators.Peek() == "tan" ||
     operators.Peek() == "sqrt" ||
     operators.Peek() == "log" ||
     operators.Peek() == "ln"))
                    {
                        output.Enqueue(operators.Pop());
                    }
                }
            }

            while (operators.Count > 0)
            {
                output.Enqueue(operators.Pop());
            }

            return output;

        }

        private static double EvaluatePostfix(Queue<string> postfix)
        {
            Stack<double> values = new Stack<double>();

            while (postfix.Count > 0)
            {
                string token = postfix.Dequeue();

                double number;

                if (double.TryParse(token, out number))
                {
                    values.Push(number);
                }
                else if (token == "sin")
                {
                    double value = values.Pop();
                    values.Push(CalculatorFunctions.Sin(value));
                }
                else if (token == "cos")
                {
                    double value = values.Pop();
                    values.Push(CalculatorFunctions.Cos(value));
                }
                else if (token == "tan")
                {
                    double value = values.Pop();
                    values.Push(CalculatorFunctions.Tan(value));
                }
                else if (token == "sqrt")
                {
                    double value = values.Pop();
                    values.Push(CalculatorFunctions.Sqrt(value));
                }
                else if (token == "log")
                {
                    double value = values.Pop();
                    values.Push(CalculatorFunctions.Log(value));
                }
                else if (token == "ln")
                {
                    double value = values.Pop();
                    values.Push(CalculatorFunctions.Ln(value));
                }
                else if (token == "%")
                {
                    double value = values.Pop();
                    values.Push(value / 100.0);
                }
                else
                {
                    double b = values.Pop();
                    double a = values.Pop();

                    switch (token)
                    {
                        case "+":
                            values.Push(a + b);
                            break;

                        case "-":
                            values.Push(a - b);
                            break;

                        case "*":
                            values.Push(a * b);
                            break;

                        case "/":
                            if (b == 0)
                                throw new Exception("Cannot divide by zero.");

                            values.Push(a / b);
                            break;

                        case "^":
                            values.Push(CalculatorFunctions.Power(a, b));
                            break;
                    }
                }
            }

            return values.Pop();
        }
    }
}
