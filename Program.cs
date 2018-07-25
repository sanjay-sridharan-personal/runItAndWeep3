using System;

class RunMain
{
    static void Main()
    {
        Console.WriteLine("This program adds multiple numbers together, however," +
            " if you make a mistake typing you will have to start all over:");
        var expression = ReadExpression();
        double value = EvaluateExpression(expression);
        Console.WriteLine($" {value}");
        Console.ReadKey();
    }

    static string ReadExpression()
    {
        // TODO(SS): Absorb any enter keys pressed after any operand
        var input = string.Empty;
        do
        {
            char keyPressed = Console.ReadKey().KeyChar;
            if ('=' == keyPressed)
                break;
            input += keyPressed;
        }
        while (true);

        // EvaluateExpression() expects an expression without spaces
        var expression = string.Empty;
        foreach (char character in input)
            if (character != ' ')
                expression += character;

        return expression;
    }

    static double EvaluateExpression(string expression)
    {
        // Expression is of the form N0+N1+N2+…+Nm, where there are NO SPACES at all, and you can replace one or more
        // plus operators with minus operator. Assumption:
        // 1. While negative numbers must be preceded by '-', positive numbers must NOT be preceded by '+'
        // 2. Operands cannot be entered in exponential notation, however, decimal points are acceptable
        return EvaluateSubtrahends(expression);
    }

    static double ConvertOperandToNumber(string input)
    {
        double numberInput = 0;
        bool isValidDouble = Double.TryParse(input, out numberInput);
        if (!isValidDouble)
        {
            Console.WriteLine($"\n\"{input}\" cannot be used as an operand, therefore, this program has exited.");
            const int userError = 0;
            Environment.Exit(userError);
        }

        return numberInput;
    }

    static double EvaluateAddends(string expression)
    {
        // Assumption: Plus characters are always operators

        // If expression is "-1+2+-3", split it on the pluses to produce an array like so ["-1", "2", "-3"]
        var stringArray = expression.Split('+');

        // Produce an addend array by converting each string element to a number
        int arrayLength = stringArray.Length;
        var addendArray = new double[arrayLength];
        for (int index = 0; index < arrayLength; ++index)
            addendArray[index] = EvaluateSubtrahends(stringArray[index]);

        // Sum up addends
        double result = 0;
        foreach (double addend in addendArray)
            result += addend;

        return result;
    }

    static double EvaluateSubtrahends(string expression)
    {
        // Assumption: Negative characters are always operators

        // If expression is "1-2", split it on the minuses to produce an array like so ["1", "2"]
        var stringArray = expression.Split('-');

        // Produce a subtrahend array by converting each string element to a number
        int arrayLength = stringArray.Length;
        var subtrahendArray = new double[arrayLength];
        for (int index = 0; index < arrayLength; ++index)
            subtrahendArray[index] = ConvertOperandToNumber(stringArray[index]);

        // Initialize result to the minuend from which to subtract subtrahends
        double result = subtrahendArray[0];
        for (int index = 1; index < arrayLength; ++index)
            result -= subtrahendArray[index];

        return result;
    }
}
