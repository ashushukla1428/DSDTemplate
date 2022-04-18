var pazeSize = 55;

var siteUrl = "http://urtestsite.com:8055/";
var weburl = window.location.toString();
var n = weburl.indexOf("localhost");
if (n > 0) {
    siteUrl = "http://localhost:52871/";
}
var _imagegRealPath = "http://images.abca.com/images/";

//error message 
var _saveMessage = 'record saved successfully.';
var _errorMessage = 'there was an error processing your request. please try again later.';

var _sortColumn = '';
var _sortOrder = '';
function setSorting(_th,sortColumn,_items)
{
  
    for (var i = 0; i < _items.length; i++) {
     //   alert(i);
        if ($(_th).index() != i) {
            $('#sample_1 tr').eq(0).find('th').eq(_items[i]).addClass("sort");
        }
    }
   // $('#sample_1 tr').eq(0).find('th').eq(3).addClass("sort");
    if (_sortColumn == sortColumn) {
        if (_sortOrder == 'DESC') {
            _sortOrder = 'ASC';

            $(_th).removeAttr("class").addClass("sorting_asc");
        }
        else {
            _sortOrder = 'DESC';
            $(_th).removeAttr("class").addClass("sorting_desc");
        }
    }
    else {
        _sortColumn = sortColumn;
        _sortOrder = 'DESC';

        $(_th).removeAttr("class").addClass("sorting_desc");
    }
    getList(1);
}
function activemenu(menuname)
{
    $('#' + menuname + '').addClass('start active open');


}

function activeClass(liname) {
    $('#' + liname + '').addClass('classActive');
}

function getClientSideImage(input, txtImg) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + txtImg).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

// Cookies
function createCookie(cookieName, cookieValue, nDays) {
   //alert(cookieValue);
    var today = new Date();
    var expire = new Date();
    if (nDays == null || nDays == 0) nDays = 1;
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + escape(cookieValue)
                    + ";expires=" + expire.toGMTString();

}

function readCookie(cookieName) {
    var theCookie = " " + document.cookie;
    //alert(theCookie);
    var ind = theCookie.indexOf(" " + cookieName + "=");
    if (ind == -1) ind = theCookie.indexOf(";" + cookieName + "=");
    if (ind == -1 || cookieName == "") return "";
    var ind1 = theCookie.indexOf(";", ind + 1);
    if (ind1 == -1) ind1 = theCookie.length;
    return unescape(theCookie.substring(ind + cookieName.length + 2, ind1));
}

function eraseCookie(name) {
    createCookie(name, "0", -1);
}
function saveTheme()
{
    var model =
      {
          themeName: $('#drpThemes').val()
      };
    $.ajax({
        url: siteUrl + 'Company/SetTheme',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(data.Status);
            location.reload();
            //alert(data);

        },
        error: function (data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
            //  alert(_errorMessage);
        }
    });
}

function ValidateFile(_this) {
    var fileExtension = ["bmp", "gif", "png", "jpg", "jpeg", "doc", "xls", "pdf", "xlsm","xlsx","zip","rar","csv","ppt","odt","txt"];
   
    var file = _this.id;
    if ($.inArray($(_this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
        alert("Only formats are allowed : " + fileExtension.join(', '));
        $(_this).val('');
        return false;
    }

    var f = _this.files[0];
    if (f.size > 8388608 || f.fileSize > 8388608) {
        alert("Allowed file size exceeded. (Max. 8 MB)")
        $(_this).val('');
        return false;
    }

    
}
//Valodation 
function validateUS4BoxPhone(_txtbox1, _txtbox2, _txtbox3, _txtbox4, _errormessage) {
    var _status = 0;
    var _checkOtherBox = 0;
    var _box1 = document.getElementById(_txtbox1);
    var _box2 = document.getElementById(_txtbox2);
    var _box3 = document.getElementById(_txtbox3);
    var _box4 = document.getElementById(_txtbox4);
    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';
    _box3.style.border = '1px solid #E5E5E5';
    _box4.style.border = '1px solid #E5E5E5';

    var error = "";
    var regex = new RegExp("^[0-9?=.*!@#$%^&*]+$");
    var _re = /^[ A-Za-z0-9_@./#&+-]*$/;
    var stripped1 = _box1.value.replace(regex, '');
    var stripped2 = _box2.value.replace(regex, '');
    var stripped3 = _box3.value.replace(regex, '');
    var stripped4 = _box4.value.replace(regex, '');

    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';
    _box3.style.border = '1px solid #E5E5E5';
    _box4.style.border = '1px solid #E5E5E5';

    if (_box1.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box2.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box3.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box4.value != '') {

        _checkOtherBox = 1;
    }
    else {
        _checkOtherBox = 0;
        _status = 1;
    }

    if (_checkOtherBox == 1) {

        if (_box1.value == "") {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box1.value)) {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else {
            _status = 1;
            _checkOtherBox = 1;
            _box1.style.border = '1px solid #E5E5E5';
        }

        //_box2
        if (_status == 1) {
            if (_box2.value == "") {
                _status = 0;
                _box2.style.border = '1px solid #E40B05';
            }
            else if (!regex.test(_box2.value)) {
                _status = 0;
                _box2.style.border = '1px solid #E40B05';
            }
            else {
                status = 1;
                _box2.style.border = '1px solid #E5E5E5';
            }
        }
        //_box3
        if (_status == 1) {
            if (_box3.value == "") {
                //fld.style.background = '#e1ad73';
                _status = 0;
                _box3.style.border = '1px solid #E40B05';
            }
            else if (!regex.test(_box3.value)) {
                // error = "The " + message + " phone number contains illegal characters.\n";
                //fld.style.background = '#e1ad73';
                _status = 0;
                _box3.style.border = '1px solid #E40B05';
            }
            else {
                _status = 1;
                _box3.style.border = '1px solid #E5E5E5';
            }
        }
        //_box4
        if (_status == 1) {
            if (_box4.value == "") {
                //fld.style.background = '#e1ad73';
                _status = 0;
                _box4.style.border = '1px solid #E40B05';
            }
            else if (!regex.test(_box4.value)) {
                // error = "The " + message + " phone number contains illegal characters.\n";
                //fld.style.background = '#e1ad73';
                _status = 0;
                _box4.style.border = '1px solid #E40B05';
            }
            else if (!regex.test(_box1.value)) {
                _status = 0;
                _checkOtherBox = 1;
                _box1.style.border = '1px solid #E40B05';
            }
            else {
                _status = 1;
                _box4.style.border = '1px solid #E5E5E5';
            }
        }
    }
    if (_status == 0) {
        return _errormessage + ".\n";
    }
    return error;
}
function validateUS3BoxPhone(_txtbox1, _txtbox2, _txtbox3, _errormessage) {
    var _status = 0;
    var _checkOtherBox = 0;
    var _box1 = document.getElementById(_txtbox1);
    var _box2 = document.getElementById(_txtbox2);
    var _box3 = document.getElementById(_txtbox3);

    var error = "";
    var regex = new RegExp("^[0-9?=.*!@#$%^&*]+$");
    var _re = /^[ A-Za-z0-9_@./#&+-]*$/;
    var stripped1 = _box1.value.replace(regex, '');
    var stripped2 = _box2.value.replace(regex, '');
    var stripped3 = _box3.value.replace(regex, '');

    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';
    _box3.style.border = '1px solid #E5E5E5';

    if (_box1.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box2.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box3.value != '') {
        _checkOtherBox = 1;
    }

    else {
        _checkOtherBox = 0;
        _status = 1;
    }

    if (_checkOtherBox == 1) {

        if (_box1.value == "") {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box1.value)) {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else {
            _status = 1;
            _checkOtherBox = 1;
            _box1.style.border = '1px solid #E5E5E5';
        }

        //_box2
        if (_status == 1) {
            if (_box2.value == "") {
                _status = 0;
                _box2.style.border = '1px solid #E40B05';
            }
            else if (!regex.test(_box2.value)) {
                _status = 0;
                _box2.style.border = '1px solid #E40B05';
            }
            else {
                status = 1;
                _box2.style.border = '1px solid #E5E5E5';
            }
        }

        //_box3
        if (_status == 1) {
            if (_box3.value == "") {
                //fld.style.background = '#e1ad73';
                _status = 0;
                _box3.style.border = '1px solid #E40B05';
            }
            else if (!regex.test(_box3.value)) {
                // error = "The " + message + " phone number contains illegal characters.\n";
                //fld.style.background = '#e1ad73';
                _status = 0;
                _box3.style.border = '1px solid #E40B05';
            }
            else {
                _status = 1;
                _box3.style.border = '1px solid #E5E5E5';
            }
        }

    }
    if (_status == 0) {
        return _errormessage + ".\n";
    }
    return error;
}
//validatePrimaryCreditCardEXP
function validateCreditCardEXP(_cardExpDay, _cardExpYear, _errormessage) {
    var _status = 0;
    var _checkOtherBox = 0;
    var _box1 = document.getElementById(_cardExpDay);
    var _box2 = document.getElementById(_cardExpYear);

    var error = "";
    var regex = new RegExp("^[0-9?=.*!@#$%^&*]+$");
    var _re = /^[ A-Za-z0-9_@./#&+-]*$/;
    var stripped1 = _box1.value.replace(regex, '');
    var stripped2 = _box2.value.replace(regex, '');

    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';

    if (_box1.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box2.value != '') {
        _checkOtherBox = 1;
    }

    else {
        _checkOtherBox = 0;
        _status = 1;
    }

    var dt = new Date();
    var m = dt.getMonth();
    //Note: 0=January, 1=February etc.
    var mnth = m + 1;
    //check year
    var dt = new Date();
    var yr = dt.getFullYear()

    if (_checkOtherBox == 1) {

        if (_box1.value == "") {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box1.value)) {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else if (_box1.value < mnth || _box1.value > 12) {
            if (_box2.value <= yr) {
                _status = 0;
                _box1.style.border = '1px solid #E40B05';
            }
            else {
                _status = 1;
                _checkOtherBox = 1;
                _box1.style.border = '1px solid #E5E5E5';
            }
        }

        else {
            _status = 1;
            _checkOtherBox = 1;
            _box1.style.border = '1px solid #E5E5E5';
        }

        //_box2

        if (_box2.value == "") {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box2.value)) {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        } else if (_box2.value < yr) {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        }
        else {
            status = 1;
            _box2.style.border = '1px solid #E5E5E5';
        }



    }
    if (_status == 0) {
        return _errormessage + "\n";
    }
    return error;
}
function validateCreditCardlength(_txtbox1, _txtbox2, _txtbox3, _txtbox4, _errormessage) {
    var _status = 0;
    var error = "";
    var _box1 = document.getElementById(_txtbox1);
    var _box2 = document.getElementById(_txtbox2);
    var _box3 = document.getElementById(_txtbox3);
    var _box4 = document.getElementById(_txtbox4);

    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';
    _box3.style.border = '1px solid #E5E5E5';
    _box4.style.border = '1px solid #E5E5E5';

    var _card1 = (_box1.value.length + _box2.value.length + _box3.value.length + _box4.value.length);
    //alert((_card1 < 16));
    //alert((_box1.value.length + _box2.value.length + _box3.value.length + _box4.value.length));
    if (_card1 != 0) {
        if (_card1 < 16) {
            _status = 1;
            _box1.style.border = '1px solid #E40B05';
            _box2.style.border = '1px solid #E40B05';
            _box3.style.border = '1px solid #E40B05';
            _box4.style.border = '1px solid #E40B05';
            //  error= "Please enter a valid primary credit card number.\n";
        }
    }
    if (_status == 1) {

        error = _errormessage + "\n";

    }
    return error;
}

//validatePrimaryStateZip
function validatePrimaryStateZip() {
    var _status = 0;
    var _checkOtherBox = 0;
    var _box1 = document.getElementById('txtStateZip');
    var _box2 = document.getElementById('txtStateZip1');

    var error = "";
    var regex = new RegExp("^[0-9?=.*!@#$%^&*]+$");
    var _re = /^[ A-Za-z0-9_@./#&+-]*$/;
    var stripped1 = _box1.value.replace(regex, '');
    var stripped2 = _box2.value.replace(regex, '');

    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';

    if (_box1.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box2.value != '') {
        _checkOtherBox = 1;
    }

    else {
        _checkOtherBox = 0;
        _status = 1;
    }


    if (_checkOtherBox == 1) {

        if (_box1.value == "") {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box1.value)) {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }

        else {
            _status = 1;
            _checkOtherBox = 1;
            _box1.style.border = '1px solid #E5E5E5';
        }

        //_box2

        if (_box2.value == "") {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box2.value)) {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        }
        else {
            status = 1;
            _box2.style.border = '1px solid #E5E5E5';
        }



    }
    if (_status == 0) {
        return "Please enter a valid Primary Credit Card expiry date.\n";
    }
    return error;
}
//validateAlternateCreditStateZip
function validateAlternateCreditStateZip() {
    var _status = 0;
    var _checkOtherBox = 0;
    var _box1 = document.getElementById('txtAltStateZip');
    var _box2 = document.getElementById('txtAltStateZip1');
    var error = "";
    var regex = new RegExp("^[0-9?=.*!@#$%^&*]+$");
    var _re = /^[ A-Za-z0-9_@./#&+-]*$/;
    var stripped1 = _box1.value.replace(regex, '');
    var stripped2 = _box2.value.replace(regex, '');

    _box1.style.border = '1px solid #E5E5E5';
    _box2.style.border = '1px solid #E5E5E5';


    if (_box1.value != '') {
        _checkOtherBox = 1;
    }
    else if (_box2.value != '') {
        _checkOtherBox = 1;
    }

    else {
        _checkOtherBox = 0;
        _status = 1;
    }

    if (_checkOtherBox == 1) {

        if (_box1.value == "") {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box1.value)) {
            _status = 0;
            _box1.style.border = '1px solid #E40B05';
        }
        else {
            _status = 1;
            _checkOtherBox = 1;
            _box1.style.border = '1px solid #E5E5E5';
        }

        //_box2

        if (_box2.value == "") {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        }
        else if (!regex.test(_box2.value)) {
            _status = 0;
            _box2.style.border = '1px solid #E40B05';
        }
        else {
            status = 1;
            _box2.style.border = '1px solid #E5E5E5';
        }




    }
    if (_status == 0) {
        return "Please enter a valid Alternate Credit Card expiry date..\n";
    }
    return error;
}

function validateNumericValue(fld, message) {
    var error = "";

    var numStr = /^-?(\d+\.?\d*)$|(\d*\.?\d+)$/;
    var temp = numStr.test(fld.value.toString());
    if (temp == false && fld.value != '') {
        fld.style.border = '1px solid #E40B05';
        if (message == 'NA') {
            error = "Only Numeric value allowed.\n"
        }
        else {
            error = message + '\n';
        }
    } else {
        fld.style.border = '1px solid #E5E5E5';

    }
    return error;
}