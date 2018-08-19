using System;
using System.Collections.Generic;

class RunMain
{
    // This program evaluates an arithmetic expression entered by the user
    static void Main()
    {
        Console.WriteLine("This program evaluates an expression consisting of multiple + and - ending with =");
        var expression = ReadExpression();
        double value = EvaluateExpression(expression);
        Console.WriteLine($" {value}");
    }

    // @returns This function returns the expression entered by the user
    static string ReadExpression()
    {
        var expression = string.Empty;
        var keyPressed = Console.ReadKey();
        char character = keyPressed.KeyChar;
        while(character != '=')
        {
            switch(character)
            {
                // Handle both backspace and delete by removing the previous character
                // from expression and outputting the entire expression on a new line
                case '\x8':
                case '\x7F':
                    expression = expression.Remove(expression.Length - 1);
                    Console.Write("\n" + expression);
                    break;

                default:
                    bool dummy;
                    if (   IsValidOperator(character)
                        || IsValidOperandCharacter(out dummy, character, false)
                        || (' ' == character)
                        || ('\t' == character))
                        expression += character;
                    break;
            }

            character = Console.ReadKey().KeyChar;
        }

        return expression;
    }

    // @returns This function returns the result of the user's arithmetic expression
    // @param expression The arithmetic expression entered by the user
    static double EvaluateExpression(string expression)
    {
        // Filter out all whitespace from expression
        expression = expression.Replace(" ", string.Empty);
        expression = expression.Replace("\t", string.Empty);

        List<double> operandArray = ParseOperands(expression);
        return AddOperands(operandArray);
    }

    // @returns The function takes the arithmetic expression and returns a list
    //          of its operands (as opposed to operators which use operands as input
    //          for operations such as addition). List order matches the operands
    //          encountered when reading the expression from left to right
    // @param expression The arithmetic expression. Assumption: expression contains
    //        NO whitespace
    static List<double> ParseOperands(string expression)
    {
        var operandList = new List<double>();
        string operandString;
        bool doNegateOperand = false;
        while (string.Empty != (operandString = ReadLeftmostOperandAsString(expression)))
        {
            // Remove from expression its leftmost operand. Please note that this
            // statement does NOT modify the calling function's argument 1, rather
            // a new value is created to which expression is re-pointed. This value
            // equals the modified result
            expression = expression.Remove(0, operandString.Length);
            double operandDouble = ConvertOperandToDouble(operandString);
            operandDouble *= (doNegateOperand ? -1 : 1);
            operandList.Add(operandDouble);
            if (0 == expression.Length)
                break;

            // Subtraction is handled by negating the operand that follows the
            // minus operator
            char theOperator = expression[0];
            if (theOperator.CompareTo('+') == 0)
                doNegateOperand = false;
            else if (theOperator.CompareTo('-') == 0)
                doNegateOperand = true;
            else
                ExitProgramWithReason($"This program does not handle operator {theOperator}");

            // Remove from expression the operator that succeeds the removed operand
            expression = expression.Remove(0, 1);

        }

        return operandList;
    }

    // @returns This function parses the passed-in expression and returns the leftmost
    //          operand. Here are the termination conditions for the operand:
    //          1. End of expression terminates the operand. Therefore, it is acceptable
    //             for the expression to consist of a single operand without any operators
    //          2. As expression does not contain whitespace, we expect the character immediately
    //             following the leftmost operand to be an operator. Assumption: User input will
    //             NOT include exponential notation, e.g. 1E2 which evaluates to 100
    // @param expression The arithmetic expression. Assumption: expression contains
    //        NO whitespace
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
            bool isValidCharacter = IsValidOperandCharacter(out isDecimalPoint, currentChar, 0 == expressionIndex);
            if (!isValidCharacter)
            {
                // Determine whether the character is a valid operator which signifies
                // that we have reached the end of the operand. If the character is
                // NOT an operator it is an error
                bool isValidOperator = IsValidOperator(currentChar);
                if (isValidOperator)
                    return operand;

                operand += currentChar;
                ExitProgramWithReason($"The final character of {operand} is invalid");
            }

            operand += currentChar;
            if (isDecimalPoint)
            {
                if (expressionIndex + 1 == expression.Length)
                    ExitProgramWithReason($"{operand} must not end with a decimal point");

                if (operandContainsDecimalPoint)
                    ExitProgramWithReason($"{operand} contains multiple decimal points");
                
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
    static bool IsValidOperandCharacter(out bool isDecimalPoint, char character, bool isLeftmost)
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

    static bool IsValidOperator(char character)
    {
        switch (character)
        {
            case '+': return true;
            case '-': return true;
            default:  return false;
        }
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
            ExitProgramWithReason($"{operandString} is not a valid number, " +
                                  "therefore, this program has exited");
        }

        return operandDouble;
    }

    // @returns This function returns the sum of the operands in the array
    // @param operandArray The list of operands to be added together
    static double AddOperands(List<double> operandArray)
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
