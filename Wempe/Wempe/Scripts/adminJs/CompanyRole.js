///--------paging and listing functions 
var _lock = true;
var _lastCall = true;

//-------------
function Add() {
    if (!validate()) {
        return;
    }

    //alert(siteUrl);
    //if (validateCartValues()) {
    //pageData
    var ids = [];
    $('.jstree-checked').each(function () {
        if ($(this).attr('id') != 0) {
            ids.push($(this).attr('id')); // ids.push(this.id) would work as well.
        }
    });
    //setting to hidden field
   // alert(JSON.stringify(ids.join(", ")));

    var model =
        {
            RoleID: parseInt($('#hdnid').val()),
            Role: $('#txtName').val(),
            IsActive: $('#chkIsActive').is(':checked'),
            ActionsID: ids.join(", ")
        };
   // alert(JSON.stringify(model));
    $.ajax({
        url: siteUrl + 'User/AddRole',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(data.Status);
            if (data.Status) {
                resetControl();
                $('#responsive').modal('toggle');
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
   
    getCompanyPages(id);
    jQuery.ajax({
        url: siteUrl + 'User/EditRole',
        dataType: 'json',
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            $('#hdnid').val(data.RoleID);
        
            $('#txtName').val(data.Role);
        

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
    //getAdminPages(0);
    $('#hdnid').val(0);
    $('#txtName').val('');
    getCompanyPages(0);
   

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

function Delete(id) {
    alert(id);

    jQuery.ajax({
        url: siteUrl + 'User/Delete',
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
        url: siteUrl + 'User/CompanyRoles',
        data: { "page": page },
        success: function (data) {
            jQuery("#divList").html(data);
        }
    });
}

function getRightsRoleWise(roleID) {
    _lock = true;
    $('#hdnid').val(roleID);
    var model = {
        RoleID:roleID,
    };
    $('#tbodyDetails').html('<tr><td colspan="3">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Role/getRightsRoleWise',
        headers: { "requestType": "client" },
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            $('#tbodyDetails').html('');
            $('#tbodyTemplate').tmpl(data).appendTo('#tbodyDetails');
            if (data.length == 0) {
                setPagingDetail(data.length, 0, pageNo);
            }
          
        },
        error: function (_data) {

            $('#tbodyDetails').html('<tr><td colspan="3">error in request</td></tr>');

        }
    });
}
//for reset all controls 
function resetControl()
{
    $('#hdnid').val(0);
   
    $('#txtName').val('');
   

    //$('#chkIsActive').prop('checked');
    //$('#chkIsActive').parent('span').removeClass('checked');

    $('#chkIsActive').prop('checked', true);
    $('#chkIsActive').parent('span').addClass('checked');
}