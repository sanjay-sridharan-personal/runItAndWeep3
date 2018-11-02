function Add(x, y) {
    return x + y;
}
function Subtract(x, y) {
    return x - y;
}
function Multiply(x, y) {
    return x * y;
}
function TryDivide(x, y) {
    if (x > y)
        return x / y;
    else
        return 0;
}
function getOperands(cleanInputString) {
    let numArray = [];
    for (let i in cleanInputString) {
        numArray[i] = parseFloat(cleanInputString[i]);
    }
    return numArray;
}
function calcValue(numArray, opArray) {
    var total = numArray[0];
    for (var i = 0; i < opArray.length; ++i) {
        switch (opArray[i]) {
            case "+":
                total = Add(total, numArray[i + 1]);
                break;
            case "-":
                total = Subtract(total, numArray[i + 1]);
                break;
            case "*":
                total = Multiply(total, numArray[i + 1]);
                break;
            case "/":
                total = TryDivide(total, numArray[i + 1]);
                break;
            case "":
                break;
        }
    }
    return total;
}
function calcMain(stringExp) {
    var operatorRegex = /[+-/*\/]/g;
    var cleanRegex = /[^\+\-\*\/0-9]/g;
    var modExp = stringExp.replace(cleanRegex, '');
    var expResult = "";
    var splitNum = modExp.split(operatorRegex);
    var numArray = getOperands(splitNum);
    var opArray = modExp.match(operatorRegex);
    if (numArray.length = opArray.length + 1)
        expResult = calcValue(numArray, opArray);
    else
        expResult = "Error in eval of expression";
    return "stringExp :" + stringExp + "<br>" + "  modExp : " + modExp + "<br>" + "    numArray : " + numArray + "<br>" + "   opArray : " + opArray + "<br>" + "expResult = " + expResult;
}
let expString = prompt("Enter expression: ");
document.body.innerHTML = calcMain(expString);
