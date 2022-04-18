$('#forgot-btn').click(function () {
    $(this).text('Sending..');
    var model = { EmailAddress: $('#forgotEmailAddress').val() };

    $.ajax({
        url: siteUrl + 'Home/ForgotPassword',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#forgot-btn').text('Submit');
            alert(data);
            
        },
        error: function () {
            $('#forgot-btn').text('Submit');
            alert("error");
            
        }
    });
 
});