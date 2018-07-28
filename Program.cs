using System;

class RunMain
{
    // This program evaluates an arithmetic expression entered by the user
    static void Main()
    {
        Console.WriteLine("This program adds multiple numbers together, however, " +
                          "if you make a mistake typing you cannot backspace:");
        var expression = ReadExpression();
        double value = EvaluateExpression(expression);
        Console.WriteLine($" {value}");
    }

    // @returns This function returns the expression entered by the user as a string
    //          filtering out all whitespace as it is irrelevant
    static string ReadExpression()
    {
        var expression = string.Empty;
        var keyPressed = Console.ReadKey();
        char character = keyPressed.KeyChar;
        while(character != '=')
        {
            switch(character)
            {
                case ' ': // Filter out all whitespace from expression
                case '\t':
                case '\n':
                case '\r':
                    break;
                default:
                    expression += character;
                    break;
            }

            character = Console.ReadKey().KeyChar;
        }

        return expression;
    }

    // @returns This function returns the result of the user's arithmetic expression
    // @param expression The arithmetic expression. Assumption: expression contains
    //        NO whitespace or parenthesis like ( or )
    static double EvaluateExpression(string expression)
    {
        double[] operandArray = ParseOperands(expression);

        // Assumption: To easily test this program assume the user wants only to
        // add together numbers
        return AddOperands(operandArray);
    }

    // @returns The function takes the arithmetic expression and returns an array
    //          of its operands (numbers, as opposed to operators which use the
    //          numbers as input for, say, addition). Array order matches the
    //          operands encountered when reading expression from left to right
    // @param expression The arithmetic expression. Assumption: expression contains
    //        NO whitespace or parenthesis like ( or )
    static double[] ParseOperands(string expression)
    {
        string operand = ReadLeftmostOperandAsString(expression);
        var operandArray = new double[1];
        operandArray[0] = ConvertOperandToDouble(operand);
        return operandArray;
    }

    // @returns This function parses the passed-in expression and returns a truncated
    //          version of it that contains just the leftmost operand. We expect
    //          the first character of the expression to be the first character
    //          of the leftmost operand also. Also, as expression does not contain
    //          whitespace, we expect the character that follows the leftmost operand
    //          to be an operator
    static string ReadLeftmostOperandAsString(string expression)
    {
        return null;
    }

    // @returns This function takes a string and attempts to convert it to a double.
    //          If successful, this double is returned, otherwise the program exits
    // @param operandString The prospective operand to be converted to double
    static double ConvertOperandToDouble(string operandString)
    {
        double operandDouble = 0;
        bool isValidDouble = Double.TryParse(operandString, out operandDouble);
        if (!isValidDouble)
        {
            Console.WriteLine($"\n\"{operandString}\" is not a valid number, therefore," +
                              " this program has exited.");
            const int userError = 0;
            Environment.Exit(userError);
        }

        return operandDouble;
    }

    // @returns This function returns the sum of the operands in the array
    // @param operandArray The list of operands to be added together
    static double AddOperands(double[] operandArray)
    {
        double sum = 0;
        foreach (double operand in operandArray)
            sum += operand;

        return sum;
    }
}
