// HOW TO USE 
// <input type="text" class="form-control" id="name" message="full name is a required field!" required>
// <input type="file" id="txtFIle" extension="jpg,png" message="Profile photo required" required/>
// <input type="email" class="form-control" id="name" message="full name is a required field!" required>
// <input type="checkbox" id="IsActive" message="Is Activeis a required field!" required>
var _swterrroMessage = '';
var _swtmessageLabel = '';
var _swterrroMessageBorderColor = 'Red';

function formValidation(_ContainerId) {
    _swterrroMessage = '';

    $("#" + _ContainerId).each(function (e) {
        $(this).find('input,textarea,select').each(function (i, requiredField) {
            if (typeof $(requiredField).attr('required') !== "undefined") {

                if (typeof $(requiredField).attr('message') !== 'undefined') {
                    _swtmessageLabel = $(requiredField).attr('message');
                }
                else {
                    _swtmessageLabel = 'message attribute is missing in ' + requiredField.id + ' control!';
                }

                if (requiredField.type === 'checkbox') {
                    if (!$(requiredField).is(':checked')) {
                        _swterrroMessage = _swterrroMessage + ' ' + _swtmessageLabel + '\n';
                    }
                }
                else {
                    if ($(requiredField).val().trim() === '') {
                        _swterrroMessage = _swterrroMessage + ' ' + _swtmessageLabel + '\n';
                        $('#' + requiredField.id).css('border-color', _swterrroMessageBorderColor);
                    }
                    else {
                        $('#' + requiredField.id).css('border-color', '');
                    }
                    switch (requiredField.type) {
                        case 'email':
                            
                            isEmail(requiredField);
                            break;
                        case 'file':
                            inputFileType(requiredField);
                            break;
                        default:
                            break;
                    }

                }
            }

        });
    });

    if (_swterrroMessage !== '') {
        swal("Sorry!", "Please, enter all the required fields! \n" + _swterrroMessage, "error");
        //alert("Sorry! Please, enter all the required fields! \n" + _swterrroMessage);
        return false;
    }
    else {
        return true;
    }
}
function isEmail(_checkEmail) {
   // alert($(_checkEmail).val().trim());
    var _regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (!_regex.test($(_checkEmail).val().trim())) {
        _swterrroMessage = _swterrroMessage + 'Email address is not in correct format.' + '\n';
        $('#' + _checkEmail.id).css('border-color', _swterrroMessageBorderColor);
    }
    else {
        $('#' + _checkEmail.id).css('border-color', '');
    }
}
function inputFileType(fileControl) {
    var _swtfileMessage = 'Please choose either a ';
    var _passedExtension = $(fileControl).attr('extension').split(',');
    var _swtfilename = $('#' + fileControl.id).val().split('\\').pop();
    var _swtextension = _swtfilename.replace(/^.*\./, '');
    if (jQuery.inArray(_swtextension, _passedExtension) === -1) {
        _swterrroMessage = _swterrroMessage + ' ' + _swtfileMessage + _passedExtension + '\n';
        return false;
    }
    return true;
}
function checkPassword() {
    if ($('Password').val() !== $('ConformPassword').val()) {
        alert("Passwords do not match.");
        return false;
    }
}
//Use this function for reset Control border
function restControl(_ContainerId) {
    $("#" + _ContainerId + " input").each(function () {
        $(this).css('border-color', "#cccccc");
    });
}