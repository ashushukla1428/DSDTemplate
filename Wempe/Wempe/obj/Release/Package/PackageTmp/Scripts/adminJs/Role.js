///--------paging and listing functions 
var _lock = true;
var _lastCall = true;

$(document).ready(function () {

    getList(1, $('#ddlRoleList').val());
});
$('#ddlRoleList').change(function () {
    //alert('in');
    getList(1, $('#ddlRoleList').val());
});
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
        url: siteUrl + 'Role/Add',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(data.Status);
            if (data.Status) {
                resetControl();
                $('#responsive').modal('toggle');
                showMessage('Success', 'success', data.Message, 'toast-top-right');
                getList(1, $('#ddlRoleList').val());
                
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
    




    getAdminPages(id);
    getCompanyPages(id);

    jQuery.ajax({
        url: siteUrl + 'Role/Edit',
        dataType: 'json',
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {

            $('#hdnid').val(data.RoleID);
            $('#txtName').val(data.Role);
        
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

    //alert($('#roleTree').find('.jstree-checked').length);
    
    //$('#roleTree ul li').find('.sgrRolePopUp').parent().removeClass('jstree-unchecked');
    //alert('in');
    ////alert($('.sgrRolePopUp').length);

    //$('.sgrRolePopUp').each(function (i, obj) {
    //    alert('in2');
    //});
}



function OpenAddBox(id) {

   // alert(id);

    $('#hdnid').val(0);

    $('#txtName').val('');
    getAdminPages(0);
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

function Delete(id)
{
    jQuery.ajax({
        url: siteUrl + 'Role/Delete',
        dataType: 'json',
        type: 'POST',
        data: JSON.stringify({ "id": id }),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            if (data.Status) {
                resetControl();
                showMessage('Success', 'success', data.Message, 'toast-top-right');
                getList(1, $('#ddlRoleList').val());

            }
            else {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
        }
    });
   }
//for get list 
function getList(page, IsActive) {
    jQuery.ajax({
        url: siteUrl + 'Role/PageList',
        data: { "page": page, "IsActive": IsActive },
        success: function (data) {
            //jQuery("#tbodyDetails").html(data);
            jQuery("#tbodyDetails").empty();

            
            

            var html = '';
            for (var i = 0; i < data.Data.length; i++) {
               
                
                if (data.Data[i].Role == 'Company') {
                  //  html += '<tr><td>' + data.Data[i].Role + '</td><td>' + data.Data[i].IsActive + '</td><td>Reserved.</td></tr>';
                }

                else if (data.Data[i].Role == 'Super Admin' || data.Data[i].Role == 'Super User') {
                    html += '<tr><td>' + data.Data[i].Role + '</td><td>' + data.Data[i].IsActive + '</td><td>Reserved.</td></tr>';
                }
                else {
                    html += '<tr><td>' + data.Data[i].Role + '</td><td>' + data.Data[i].IsActive + '</td><td>' + "<a href='#' class='btn default btn-xs blue-stripe' onclick='Edit(" + data.Data[i].RoleID + ");'><i class='fa fa-edit'></i>Set Permissions </a>" + '</td></tr>';
                }
            }
            jQuery("#tbodyDetails").html(html);

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