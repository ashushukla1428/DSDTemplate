var _lock = true;
var _lockCustomerSearch = true;
var TempValues = { PriState: '', PriCity: '', AltState: '', AltCity: '', thirdState: '', thirdCity: '' };
$(document).keypress(function (e) {
    if (e.which == 13) {
        searchCustomer(1);
    }
});
function editExistingCustomer(customerNumber) {
    $('#hdnCustomerNumber').val(customerNumber);
   window.location.href = "../Repair/NewRepair";
}
function searchCustomer(pageNo) {

    

    _lock = true;
    _lockCustomerSearch = true;
    $('#hdnPageNo').val(pageNo);
    var _SearchFields = '';
    var _SearchValues = '';
    $.each($(".searchValue"), function () {
        if ($(this).val() != '')
        {
            if ($(this).val() != '0') {
                _SearchFields += $(this).attr('fieldName') + '|';
                _SearchValues += $(this).val() + '|';
            }
        }
    });
    var model = {
        SearchFields: _SearchFields,
        SearchValues: _SearchValues,
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Customer/searchCustomer',
        headers: { "requestType": "client" },
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            $('#tbodyDetails').html('');
            $('#tbodyTemplate').tmpl(data).appendTo('#tbodyDetails');
            if (data.length == 0) {
                setPagingDetail(data.length, 0, pageNo);
            }
            else {
                setPagingDetail(data.length, data[0].TotalCount, pageNo);
            }
        },
        error: function (_data) {
            $('#tbodyDetails').html('<tr><td colspan="5">error in request</td></tr>');
        }
    });
}
function setPagingDetail(noOfrows, Total, pageNo) {
    _firstRecord = 1;
    if (noOfrows < pazeSize) {
        noOfrows = Total;
        _firstRecord = Total;
        _lock = false;
        _lockCustomerSearch = false;
    }
    else {
        noOfrows = noOfrows * pageNo;
        if (pageNo > 1) {
            _firstRecord = (noOfrows - pazeSize) + 1;
        }
    }
    $('#sample_1_info').html('Showing ' + _firstRecord + ' to ' + noOfrows + ' of ' + Total + ' entries');
}
$('#sample_1_next').click(function () {
    if (_lockCustomerSearch) {
        var _index = parseInt($('#hdnPageNo').val()) + 1;
        searchCustomer(_index);
    }
});
$('#sample_1_previous').click(function () {
    var _index = parseInt($('#hdnPageNo').val()) - 1;
    if (_index > 0) {
        _lock = true;
        _lockCustomerSearch = true;
        searchCustomer(_index);
    }
});
///get  states
//$("#drpPrimaryCountry").change(function () {
//    //  var selectedText = $(this).find("option:selected").text();
//    
//    if ($("#drpPrimaryCountry option:selected").text() == "United States") {
//        $('#divZipPrimary').show();

//        //  $('#txtPhone1NonUs').val('');

//        $('#divUSPhonePrimary').show();
//        $('#divUSCellPrimary').show();
//        $('#divUSFaxPrimary').show();
//        $('#divUSHOMEPhonePrimary').show();
//        $('#divUSASSISTANTPhonePrimary').show();


//        $('#divNonUSPhonePrimary').hide();
//        $('#divNonUSCellPrimary').hide();
//        $('#divNonUSFaxPrimary').hide();

//        $('#divNonUSHomePhonePrimary').hide();
//        $('#divNonUSASSISTANTPhonePrimary').hide();

//    }
//    else {
//        $('#divZipPrimary').hide();
//        // $('#txtPhone1').val('');
//        $('#divUSPhonePrimary').hide();
//        $('#divUSCellPrimary').hide();
//        $('#divUSFaxPrimary').hide();
//        $('#divUSHOMEPhonePrimary').hide();
//        $('#divUSASSISTANTPhonePrimary').hide();

//        $('#divNonUSPhonePrimary').show();
//        $('#divNonUSCellPrimary').show();
//        $('#divNonUSFaxPrimary').show();

//        $('#divNonUSHomePhonePrimary').show();
//        $('#divNonUSASSISTANTPhonePrimary').show();
//    }
//    var id = $(this).val();
//    //
//    if (id == 0) {
//        // alert(id);
//        showMessage('Oops', 'error', 'Please select primary country!', 'toast-top-right');
//        return;
//    }
//    var drpSates = $("[id*=drpPrimaryStates]");
//    drpSates.empty().append('<option selected="selected" value="">Loading....</option>');

//    $('#drpPrimaryCites').empty().append('<option selected="selected" value="">Please select</option>');


//    jQuery.ajax({
//        url: siteUrl + 'Repair/getStates',
//        dataType: 'json',
//        headers: { "requestType": "client" },
//        data: { "Id": id },
//        contentType: 'application/json;charset=utf-8',
//        success: function (data) {
//            
//            if (data.Status == false) {
//                showMessage('Oops', 'error', data.Message, 'toast-top-right');
//                return;
//            }
//            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
//            $.each(data, function () {
//                drpSates.append($("<option></option>").val(this['Id']).html(this['stateFullName']));
//            });
//            if (TempValues.PriState != '') {
//                $("#drpPrimaryStates").val(TempValues.PriState).attr('selected', true);
//                $("#drpPrimaryStates").change();
//            }
//        },
//        error: function (data) {
//            // alert(JSON.stringify(data));
//            

//            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
//            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
//            //  alert(_errorMessage);
//        }
//    });
//});
///Get cities
//$("#drpPrimaryStates").change(function () {

//    //  var selectedText = $(this).find("option:selected").text();

//    if ($("#drpPrimaryStates option:selected").text() != "Please select") {

//       //Repair.FetchTaxByStateId();

//        var id = $(this).val();
//        if (id == null) {
//            return;
//        }
//        if (id == 0) {
//            showMessage('Oops', 'error', 'Please select primary state!', 'toast-top-right');
//            return;
//        }
//        var drpSates = $("[id*=drpPrimaryCites]");
//        drpSates.empty().append('<option selected="selected" value="">Loading....</option>');
//        jQuery.ajax({
//            url: siteUrl + 'Repair/getCities',
//            dataType: 'json',
//            headers: { "requestType": "client" },
//            data: { "Id": id },
//            contentType: 'application/json;charset=utf-8',
//            success: function (data) {
//                if (data.Status == false) {
//                    showMessage('Oops', 'error', data.Message, 'toast-top-right');
//                    return;
//                }
//                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
//                $.each(data, function () {
//                    drpSates.append($("<option></option>").val(this['Id']).html(this['city']));
//                });

//                if (TempValues.PriCity != '') {
//                    $("#drpPrimaryCites option:contains(" + TempValues.PriCity + ")").attr('selected', true);
//                }


//            },
//            error: function (data) {
//                // alert(JSON.stringify(data));
//                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
//                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
//                //  alert(_errorMessage);
//            }
//        });
//    }
//});

function resetControl() {
    $('#hdnid').val(0);
    $('#txtName').val('');
    //$('#chkIsActive').prop('checked', false);
    //$('#chkIsActive').parent('span').removeClass('checked');

    $('#chkIsActive').prop('checked', true);
    $('#chkIsActive').parent('span').addClass('checked');
}


