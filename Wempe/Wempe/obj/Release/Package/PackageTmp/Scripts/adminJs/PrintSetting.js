var _lock = true;

function Add() {
    
    var model =
    {
        SettingID: parseInt($('#hdnid').val()),
        PaperTopMargin: $('#PaperTopMargin').val(),
        PaperRightMargin: $('#PaperRightMargin').val(),
        PaperBottomMargin: $('#PaperBottomMargin').val(),
        PaperLeftMargin: $('#PaperLeftMargin').val()
    };

    $.ajax({
        url: siteUrl + 'PrintSetting/Add',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status) {
                $('#responsive').modal('toggle');
                showMessage('Success', 'success', data.Message, 'toast-top-right');
            }
            else {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
        },
        error: function (_data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
        }
    });
}
