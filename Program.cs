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
    //          to be an operator. Assumption: User input will NOT include exponential
    //          notation, e.g. "1E2" which evaluates to 100
    static string ReadLeftmostOperandAsString(string expression)
    {
        string operand = string.Empty;
        bool operandContainsDecimalPoint = false;
        for (int expressionIndex = 0; expressionIndex < expression.Length; ++expressionIndex)
        {
            // Parse expression by copying its characters to an operand until, but not
            // including, the operator that follows the leftmost operand. An operand
            // can contain exactly 0 or 1 decimal points
            bool isDecimalPoint = false;
            char currentChar = expression[expressionIndex];
            bool isValid = IsValidCharacter(out isDecimalPoint, currentChar, 0 == expressionIndex);
            operand += currentChar;

            if (!isValid)
                ExitProgramWithReason($"The final character of \"{operand}\" is invalid");
            
            if (isDecimalPoint)
            {
                if (expressionIndex + 1 == expression.Length)
                    ExitProgramWithReason($"\"{operand}\" needs a digit after the decimal point");

                if (operandContainsDecimalPoint)
                    ExitProgramWithReason($"\"{operand}\" contains multiple decimal points");
                
                operandContainsDecimalPoint = true;
            }
        }

        return operand;
    }

    // @returns This function determines whether the character passed in is valid
    //          in any operand. Note: the operand itself is NOT passed in
    // @param isDecimalPoint Returns whether the character is a decimal point
    // @param character The character to test
    // @param isLeftmost Pass in true if the character is the leftmost one of the
    //        operand, otherwise pass in false
    static bool IsValidCharacter(out bool isDecimalPoint, char character, bool isLeftmost)
    {
        const string regularCharacters = "0123456789";
        const char   optionalDecimalPoint = '.';
        const string optionalLeadingSigns = "+-";

        string validCharacterSet = regularCharacters + optionalDecimalPoint;
        if (isLeftmost)
            validCharacterSet += optionalLeadingSigns;

        if (optionalDecimalPoint == character)
            isDecimalPoint = true;
        else
            isDecimalPoint = false;

        return -1 != validCharacterSet.IndexOf(character);
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
            ExitProgramWithReason($"\"{operandString}\" is not a valid number, " +
                                  "therefore, this program has exited");
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

    // This function terminates the program and outputs the reason for it
    // @param reason The reason to terminate the program
    static void ExitProgramWithReason(string reason)
    {
        Console.WriteLine("\n" + reason);
        const int userError = 0;
        Environment.Exit(userError);
    }
}
