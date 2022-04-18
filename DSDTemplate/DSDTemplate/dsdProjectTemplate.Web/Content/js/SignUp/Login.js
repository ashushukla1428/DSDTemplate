var webSiteUrl = window.location.origin; 
      
$(document).ready(function() {
    $("#txtUserName,#txtPassword").keyup(function(event) {
        if (event.which === 13) {
            $("#btnLogin").click();
        }
    });
 
     $("#txtFogotPasswordEmail").keyup(function(event) {
        if (event.which === 13) {
            $("#btnForgotLink").click();
        }
    });

 $("#txtForgotUserNameEmail").keyup(function(event) {
        if (event.which === 13) {
            $("#btnForgotUserNameLink").click();
        }
    });


});
$('#btnLogin').click(function () {
    if (formValidation('formLogin') === true) {
       
        var loginModel = {
            UserName: $('#txtUserName').val(),
            Password: $('#txtPassword').val()
        };
        $('#btnLogin').html("Wait..");
        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: webSiteUrl +'/Login/Login',
            success: function (data) {
               //alert(data.IsTwoFactorAuthenticationDone);
                
                if (data.Status) {
                    if(data.IsTwoFactorAuthenticationRequested===true && data.IsTwoFactorAuthenticationDone===false)
                    {
                     // window.location.href = "/TwoFactorAuthentication";
                        $('#modelTwoFactorAuthentication').modal('toggle');
                        $('#btnLogin').html("Login");
                    }
                     else{
                     window.location.href = "/dashboard";
                    }  
                  }
                else {
                    swal("Sorry!", data.Message, "warning");
                    $('#btnLogin').html("Login");
                }
              
                //$('#divLoadStep3').html(data);
            },
            failure: function (response) {
                // alert(response.responseText);
                swal("Sorry!", response.responseText, "error");
                $('#btnLogin').html("Login");
            },
            error: function (response) {
                swal("Sorry!", response.responseText, "error");
                $('#btnLogin').html("Login");
            }
        });
    }
});

$('#btnTwoFactorAuthentication').click(function () {
    if (formValidation('formLogin') === true) {
       
        var loginModel = {
            UserName: $('#txtUserName').val(),
            Password: $('#txtPassword').val(),
            Code: $('#txtTwoFactorAuthentication_Code').val()
        };
        $('#btnTwoFactorAuthentication').html("Wait..");
        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: webSiteUrl +'/Login/CheckTwoFactorAuthentication',
            success: function (data) {
               //alert(data.IsTwoFactorAuthenticationDone);
                
                if (data.Status) {
                    window.location.href = "/dashboard"; 
                  }
                else {
                    swal("Sorry!", data.Message, "warning");
                    $('#btnTwoFactorAuthentication').html("Verify Me");
                }
              
                //$('#divLoadStep3').html(data);
            },
            failure: function (response) {
                // alert(response.responseText);
                swal("Sorry!", response.responseText, "error");
                $('#btnTwoFactorAuthentication').html("Verify Me");
            },
            error: function (response) {
                swal("Sorry!", response.responseText, "error");
                $('#btnTwoFactorAuthentication').html("Verify Me");
            }
        });
    }
});

$('#btnForgotLink').click(function () {
    if (formValidation('formForgotPassword') === true) {

        var loginModel = {
            Email: $('#txtFogotPasswordEmail').val()
        };
        $('#btnForgotLink').html("Wait..");
        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: webSiteUrl+'/Home/SendForgotPasswordLink',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.Status) {
                    swal("Thanks", data.Message, "success");
                    $('#txtFogotPasswordEmail').val('');
                    $('#modelForgotPassword').modal('hide');
                }
                else {
                    swal("Sorry!", data.Message, "warning");
                }
                $('#btnForgotLink').html("Send me reset password link");
                //$('#divLoadStep3').html(data);
            },
            failure: function (response) {
                // alert(response.responseText);
                $('#btnForgotLink').html("Send me reset password link");
                swal("Sorry!", response.responseText, "error");
            },
            error: function (response) {
                $('#btnForgotLink').html("Send me reset password link");
                swal("Sorry!", response.responseText, "error");
            }
        });
    }
});
$('#btnForgotUserNameLink').click(function () {
    if (formValidation('formForgotUserName') === true) {

        var loginModel = {
            Email: $('#txtForgotUserNameEmail').val()
        };
        $('#btnForgotUserNameLink').html("Wait..");
        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: 'Home/SendForgotUserNameLink',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.Status) {
                    swal("Thanks", data.Message, "success");
                    $('#txtForgotUserNameEmail').val('');
                    $('#modelForgotUserName').modal('hide');
                }
                else {
                    swal("Sorry!", data.Message, "warning");
                }
                $('#btnForgotUserNameLink').html("Send me reset password link");
                //$('#divLoadStep3').html(data);
            },
            failure: function (response) {
                // alert(response.responseText);
                $('#btnForgotUserNameLink').html("Send me reset password link");
                swal("Sorry!", response.responseText, "error");
            },
            error: function (response) {
                $('#btnForgotUserNameLink').html("Send me reset password link");
                swal("Sorry!", response.responseText, "error");
            }
        });
    }
});                    
$('#btnResetPassword').click(function () {

    if (formValidation('fromRestPasword') === true) {
        if ($('#Password').val() !== $('#ConformPassword').val()) {
            swal("Sorry!", 'Your password and re-confirm password do not match!', "error");
            return;
        }

        var loginModel = {
            Password: $('#Password').val(),
            Token: $('#hdnToken').val()
        };
        $('#btnResetPassword').html("Wait..");
        $.ajax({
            type: 'POST',
            data: { model: loginModel },
            url: '/Home/ChangePassword',
            success: function (data) {
                //alert(JSON.stringify(data));
                if (data.Status) {
                    //swal("Thanks", data.Message, "success");
                    swal("Thanks", data.Message, "success")
                        .then((value) => {
                            window.location.href = "/";
                        });
                   
                }
                else {
                    swal("Sorry!", data.Message, "warning");
                }
                $('#btnResetPassword').html("Rest my password");
                //$('#divLoadStep3').html(data);
            },
            failure: function (response) {
                // alert(response.responseText);
                $('#btnResetPassword').html("Rest my password");
                swal("Sorry!", response.responseText, "error");
            },
            error: function (response) {
                $('#btnResetPassword').html("Rest my password");
                swal("Sorry!", response.responseText, "error");
            }
        });
    }
});