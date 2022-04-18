function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    else {
            document.getElementById('txtAmount').value = format(document.getElementById('txtAmount').value);
            return true;
    }
}
function saveAmount()
{
    if (checkAmountValue())
    {
        fade(document.getElementById('divMain'));
    }
}
function checkAmountValue()
{
    var _amount = document.getElementById("txtAmount").value;
    _amount = _amount.toString().replace(",", "")
    if (_amount > 100 || _amount < 10) {
        alert("Please insertAmount between 10 and 100");
        document.getElementById("txtZoom").focus();
        return false;
    }
}
function fade(element) {
    var op = 1;  // initial opacity
    var timer = setInterval(function () {
        if (op <= 0.1) {
            clearInterval(timer);
            element.style.display = 'none';
        }
        element.style.opacity = op;
        element.style.filter = 'alpha(opacity=' + op * 100 + ")";
        op -= op * 0.1;
    }, 50);
}

var format = function (num) {
    var str = num.toString().replace("$", ""), parts = false, output = [], i = 1, formatted = null;
    if (str.indexOf(".") > 0) {
        parts = str.split(".");
        str = parts[0];
    }
    str = str.split("").reverse();
    for (var j = 0, len = str.length; j < len; j++) {
        if (str[j] != ",") {
            output.push(str[j]);
            if (i % 3 == 0 && j < (len - 1)) {
                output.push(",");
            }
            i++;
        }
    }
    formatted = output.reverse().join("");
    //return ("$" + formatted + ((parts) ? "." + parts[1].substr(0, 2) : ""));
    return ("" + formatted + ((parts) ? "." + parts[1].substr(0, 1) : ""));
};