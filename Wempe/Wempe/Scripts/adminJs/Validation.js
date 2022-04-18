//function validateFormOnSubmit(theForm) {
//    var reason = "";

//    reason += validateUsername(theForm.username);
//    reason += validatePassword(theForm.pwd);
//    reason += validateEmail(theForm.email);
//    reason += validatePhone(theForm.phone);
//    reason += validateEmpty(theForm.from);

//    if (reason != "") {
//        alert("Some fields need correction:\n" + reason);
//        return false;
//    }

//    return true;
//}

function validateEmpty(fld,message) {
    var error = "";

    if (fld.value.length == 0) {
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
        if (message == 'NA') {
            error = "The required field has not been filled in.\n"
        }
        else {
            error = message + '\n';
        }
    } else {
        fld.style.border = '1px solid #E5E5E5';
       
    }
    return error;
}

function validateDropdown(fld, message) {
 
    var error = "";
    //var e = document.getElementById("ddlView");
    var strDrp = fld.options[fld.selectedIndex].value;
   // alert(strDrp);
 //   var strDrp = fld.options[fld.selectedIndex].text;
    if (strDrp == '') {
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
        if (message == 'NA') {
            error = "The required field has not been filled in.\n"
        }
        else {
            error = message + '\n';
        }
    }
    else {
        fld.style.border = '1px solid #E5E5E5';

    }
    return error;
}

function validateUsername(fld) {
    var error = "";
    var illegalChars = /\W/; // allow letters, numbers, and underscores

    if (fld.value == "") {
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
        error = "You didn't enter a username.\n";
    } else if ((fld.value.length < 4) || (fld.value.length > 10)) {
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
        error = "The user name must be between 4 and 10 characters long..\n";
    } else if (illegalChars.test(fld.value)) {
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
        error = "The user name must consist of letters and numbers only (no special characters).\n";
    } else {
        fld.style.border = '1px solid #E5E5E5';
    }
    return error;
}

function validatePassword(fld) {
    var error = "";
    var illegalChars = /[\W_]/; // allow only letters and numbers 

    if (fld.value == "") {
        fld.style.background = '#e1ad73';
        error = "You didn't enter a password.\n";
    } else if ((fld.value.length < 8) || (fld.value.length > 20)) {
        error = "The password must be between 8 and 20 characters long. \n";
        fld.style.background = '#e1ad73';
    } else if (illegalChars.test(fld.value)) {
        error = "The password must consist of letters and numbers only.\n";
        fld.style.background = '#e1ad73';
    } else if (!((fld.value.search(/(a-z)+/)) && (fld.value.search(/(0-9)+/)))) {
        error = "The password must contain at least one numeral.\n";
        fld.style.background = '#e1ad73';
    } else {
        fld.style.border = '1px solid White';
    }
    return error;
}

function trim(s) {
    return s.replace(/^\s+|\s+$/, '');
}

function validateEmail(fld, message) {
    var error = "";
    var tfld = trim(fld.value);                        // value of field with whitespace trimmed off
    var emailFilter = /^[^@]+@[^@.]+\.[^@]*\w\w$/;
    var illegalChars = /[\(\)\<\>\,\;\:\\\"\[\]]/;

    if (fld.value == "") {
        fld.style.border = '1px solid #E5E5E5';
       // fld.style.border = '1px solid #E40B05';
       // error = "You didn't enter an email address.\n";
    } else if (!emailFilter.test(tfld)) {              //test email for illegal characters
        fld.style.border = '1px solid #E40B05';
        error = "Please enter a valid " + message + " email address.\n";
    } else if (fld.value.match(illegalChars)) {
        fld.style.border = '1px solid #E40B05';
        error = "The " + message + " email address contains illegal characters.\n";
    } else {
        fld.style.border = '1px solid #E5E5E5';
    }
    return error;
}

function validatePhone(fld) {
    var error = "";
    var stripped = fld.value.replace(/[\(\)\.\-\ ]/g, '');

    if (fld.value == "") {
        error = "You didn't enter a phone number.\n";
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
    } else if (isNaN(parseInt(stripped))) {
        error = "The phone number contains illegal characters.\n";
        //fld.style.background = '#e1ad73';
        fld.style.border = '1px solid #E40B05';
    } else if (!(stripped.length == 10)) {
        error = "The phone number is the wrong length. Make sure you included an area code.\n";
        // fld.style.border = '1px solid White';
        fld.style.border = '1px solid #E5E5E5';
    }
    return error;
}



function IsNumeric(fld, message) {
    var error = "";
    var numStr = /^-?(\d+\.?\d*)$|(\d*\.?\d+)$/;
    var temp = true;
    if (fld.value.length != 0) {
        temp = numStr.test(fld.value);
    }
    if (temp == false) {
        fld.style.border = '1px solid #E40B05';
        error = message + '\n';
    }
    else {
        fld.style.border = '1px solid #E5E5E5';
    }
    return error;
}