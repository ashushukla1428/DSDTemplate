function Add() {
    //alert(siteUrl);
    //if (validateCartValues()) {
    //pageData
    var model =
        {
            MenuID: parseInt($('#hdnid').val()),
            parentID: parseInt($('#hdnpid').val()),
            MenuName: $('#txtName').val(),
            PageName: $('#txtPageName').val(),
            PageID: $('#pageData').val(),
            MenuIndex: $('#txtMenuIndex').val(),
            IsActive: $('#chkIsActive').is(':checked')
        };
   // alert(JSON.stringify(model));
    $.ajax({
        url: siteUrl + 'Menu/Add',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(data.Status);
            if (data.Status) {
                resetControl();
                showMessage('Success', 'success', data.Message, 'toast-top-right');
                getList(1);
                
            }
            else
            {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
            //alert(data);
            
        },
        error: function (_data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
          //  alert(_errorMessage);
        }
    });
    //}
}
//for edit request data
function Edit(id)
{
    jQuery.ajax({
        url: siteUrl + 'Menu/Edit',
        dataType: 'json',
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            $('#hdnid').val(data.MenuID);
            $('#hdnpid').val(data.parentID);
            $('#txtName').val(data.MenuName);
            $('#txtPageName').val(data.PageName);
            $('#pageData').val(data.PageID);
            $('#txtMenuIndex').val(data.MenuIndex);

            // alert(data.IsActive);
            if (data.IsActive) {
                $('#chkIsActive').prop('checked', 'checked');
                $('#chkIsActive').parent('span').addClass('checked');
            }
            else {
                $('#chkIsActive').prop('checked');
                $('#chkIsActive').parent('span').removeClass('checked');
            }
            //alert($('#chkIsActive').is(':checked'));
            var $modal = $('#responsive');
            $modal.modal();
        }
    });
}
function OpenAddBox(id) {
    $('#hdnid').val(0);
    $('#hdnpid').val(id);
    $('#txtName').val();
    $('#txtPageName').val();
    $('#txtMenuIndex').val(0);

    // alert(data.IsActive);
    $('#chkIsActive').prop('checked', 'checked');
    $('#chkIsActive').parent('span').addClass('checked');
    //alert($('#chkIsActive').is(':checked'));
    var $modal = $('#responsive');
    $modal.modal();
}

function ConfirmClick(id) {
    if (confirm('Are you sure?')) {
        Delete(id);
    } 
}

function Delete(id)
{
    jQuery.ajax({
        url: siteUrl + 'Menu/Delete',
        dataType: 'json',
        type: 'POST',
        data: JSON.stringify({ "id": id }),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            if (data.Status) {
                resetControl();
                showMessage('Success', 'success', data.Message, 'toast-top-right');
                getList(1);

            }
            else {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
        }
    });
   }
//for get list 
function getList(page) {
    jQuery.ajax({
        url: siteUrl + 'Pages/PageList',
        data: { "page": page },
        success: function (data) {
            jQuery("#divList").html(data);
        }
    });
}
//for reset all controls 
function resetControl()
{
    $('#hdnid').val(0);
    $('#hdnpid').val(0);
    $('#txtName').val('');
    $('#txtPageName').val('');

    //$('#chkIsActive').prop('checked');
    //$('#chkIsActive').parent('span').removeClass('checked');

    $('#chkIsActive').prop('checked', true);
    $('#chkIsActive').parent('span').addClass('checked');
}