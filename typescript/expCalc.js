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
function calcMain(stringExp) {
    var modExp = stringExp.replace(/[^0-9/+/-/*//]+/g, '');
    var splitExp = modExp.split(/\D/);
    return "SplitExp : " + splitExp[0] + " length :" + splitExp.length;
}
var expString = prompt("Enter expression: ");
document.body.innerHTML = calcMain(expString);
