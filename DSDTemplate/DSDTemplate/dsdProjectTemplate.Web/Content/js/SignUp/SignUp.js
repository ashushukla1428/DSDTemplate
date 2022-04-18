var _latitude;
var _longitude;
var _communityId = 0;
var _interestsId = '';
$('#txtSignUpPassword, #txtSignUpConformPassword').on('keyup', function () {
    if ($('#txtSignUpPassword').val() == $('#txtSignUpConformPassword').val()) {
        $('#message').html('Matching').css('color', 'green');
    } else
        $('#message').html('Password is not matching').css('color', 'red');
});
$('#btnSignUp').click(function () {
   
    if (formValidation('signUpModel') === true) {
      //
     
        var modelData = {
            FullName: $('#txtFullName').val(),
            Email: $('#txtSignUpUserName').val(),
            Password: $('#txtSignUpPassword').val()
        };
        //console.log(JSON.stringify(modelData));
        $.ajax({
            type: 'POST',
            data: { model: modelData },
            url: '/Home/SignUp',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.status) {
                    swal("Thanks", data.Message, "success");
                }
                else {
                    swal("Sorry!", data.Message, "info");
                }
            },
            failure: function (response) {
               // alert(response.responseText);
                swal("Sorry!", response.responseText, "error");
            },
            error: function (response) {
                swal("Sorry!", response.responseText, "error");
            }
        });
    }
});


$('#brnVerificationStep1').click(function () {
    $('#verificationStep1').hide();
    $('#verificationStep2').show();
   
});
$('#brnVerificationStep1Back').click(function () {
    $('#verificationStep1').show();
    $('#verificationStep2').hide();
   
});