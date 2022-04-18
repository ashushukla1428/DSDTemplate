var _lock = true;
var TempValues = { State: '', City: '' };
$(document).keypress(function (e) {
    if (e.which == 13) {
        getList(1);
    }
});


function getList(pageNo) {
    _lock = true;
    $('#hdnPageNo').val(pageNo);
    var model = {
        Name: $('#txtSearchName').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder,
        CountryId: $('#drpTaxCountrySearch').val(),
        StateId: $('#drpTaxStateSearch').val(),
        CityId: $('#drpTaxCitySearch').val(),
        TaxType: $('#ddlTaxList').val()
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'ManageTax/getList',
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
            $('#tbodyDetails').html('<tr><td colspan="9">error in request</td></tr>');
        }
    });
}

function setPagingDetail(noOfrows, Total, pageNo) {

    _firstRecord = 1;

    if (noOfrows < pazeSize) {
        noOfrows = Total;
        _firstRecord = Total;
        _lock = false;
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
    if (_lock) {
        var _index = parseInt($('#hdnPageNo').val()) + 1;
        getList(_index);
    }
});

$('#ddlTaxList').change(function () {

    getList(1);

});

$('#sample_1_previous').click(function () {
    var _index = parseInt($('#hdnPageNo').val()) - 1;
    if (_index > 0) {
        _lock = true;
        getList(_index);
    }
});

$("#drpTaxCountrySearch").change(function () {
    var id = $(this).val();
    if (id == 0) {
       // showMessage('Oops', 'error', 'Please select country!', 'toast-top-right');
        return;
    }
    var drpSates = $("[id*=drpTaxStateSearch]");
    drpSates.empty().append('<option selected="selected" value="">Loading....</option>');
    jQuery.ajax({
        url: siteUrl + 'Repair/getStates',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "Id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status == false) {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
                return;
            }
            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
            $.each(data, function () {
                drpSates.append($("<option></option>").val(this['Id']).html(this['stateFullName']));
            });
        },
        error: function (data) {
            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
        }
    });
});



$("#drpTaxStateSearch").change(function () {
    var id = $(this).val();
    if (id == 0) {
       // showMessage('Oops', 'error', 'Please select country!', 'toast-top-right');
        return;
    }
    var drpCity = $("[id*=drpTaxCitySearch]");
    drpCity.empty().append('<option selected="selected" value="">Loading....</option>');
    jQuery.ajax({
        url: siteUrl + 'Repair/getCities',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "Id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status == false) {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
                return;
            }
            drpCity.empty().append('<option selected="selected" value="">Please select</option>');
            $.each(data, function () {
                drpCity.append($("<option></option>").val(this['Id']).html(this['city']));
            });
        },
        error: function (data) {
            // alert(JSON.stringify(data));
            drpCity.empty().append('<option selected="selected" value="">Please select</option>');
            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
            //  alert(_errorMessage);
        }
    });
});




$("#drpTaxCountrySearch,#drpTaxStateSearch,#drpTaxCitySearch").change(function () {
    getList(1);

});


$("#drpTaxCountry").change(function () {
    var id = $(this).val();
    if (id == 0) {
        showMessage('Oops', 'error', 'Please select country!', 'toast-top-right');
        return;
    }
    var drpSates = $("[id*=drpTaxState]");
    drpSates.empty().append('<option selected="selected" value="">Loading....</option>');
    jQuery.ajax({
        url: siteUrl + 'Repair/getStates',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "Id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status == false) {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
                return;
            }
            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
            $.each(data, function () {
                drpSates.append($("<option></option>").val(this['Id']).html(this['stateFullName']));
            });

            if (TempValues.State != '') {
                $("#drpTaxState").val(TempValues.State).attr('selected', true);
                $("#drpTaxState").change();
            }
        },
        error: function (data) {
            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
        }
    });
});

$("#drpTaxState").change(function () {

    //  var selectedText = $(this).find("option:selected").text();

    if ($("#drpTaxState option:selected").text() != "Please select") {

        var id = $(this).val();
        if (id == null) {
            return;
        }
        if (id == 0) {
            showMessage('Oops', 'error', 'Please select state!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpTaxCity]");
        drpSates.empty().append('<option selected="selected" value="">Loading....</option>');
        jQuery.ajax({
            url: siteUrl + 'Repair/getCities',
            dataType: 'json',
            headers: { "requestType": "client" },
            data: { "Id": id },
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                if (data.Status == false) {
                    showMessage('Oops', 'error', data.Message, 'toast-top-right');
                    return;
                }
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                $.each(data, function () {
                    drpSates.append($("<option></option>").val(this['Id']).html(this['city']));
                });

                if (TempValues.City != '') {
                    $("#drpTaxCity").val(TempValues.City).attr('selected', true);
                    $("#drpTaxCity").change();
                }
            },
            error: function (data) {
                // alert(JSON.stringify(data));
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                //  alert(_errorMessage);
            }
        });
    }
});


$('#drpTaxCountrySearch').change(function () {

    if ($("#drpTaxCountrySearch option:selected").text() != "Select Country") {



    }
});

function Add() {
    if (!validate()) {
        return;
    }
    var model =
    {
        Id: parseInt($('#hdnid').val()),
        taxRate: $('#txtName').val(),
        IsActive: $('#chkIsActive').is(':checked'),
        StateId: $('#drpTaxCity').val()
    };
    $.ajax({
        url: siteUrl + 'ManageTax/Add',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status) {
                resetControl();
                $('#responsive').modal('toggle');
                showMessage('Success', 'success', data.Message, 'toast-top-right');
                getList(1);
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

function Edit(id) {
    resetControl();
    if (id == 0) {
        $('#hdnid').val(id);
        var $modal = $('#responsive');
        $modal.modal();
        return;
    }

    jQuery.ajax({
        url: siteUrl + 'ManageTax/Edit',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //if (data.Status == false) {
            //    showMessage('Oops', 'error', data.Message, 'toast-top-right');
            //    return;
            //}
            $('#hdnid').val(data.Id);
            $('#txtName').val(data.taxRate);
            $('#drpTaxCountry').val(data.country);
            $('#drpTaxCountry').change();
          //  $("#drpTaxState").val(data.state).attr('selected', true);
           // $("#drpTaxState").change();
            TempValues.State = data.state;
            TempValues.City = data.StateId;
            //$('#drpTaxState').val(tempState);
            //$('#drpTaxState').change();

            //$('#drpTaxCity').val(data.StateId);


            if (data.IsActive) {
                $('#chkIsActive').prop('checked', true);
                $('#chkIsActive').parent('span').addClass('checked');
            }
            else {
                $('#chkIsActive').prop('checked', false);
                $('#chkIsActive').parent('span').removeClass('checked');
            }
            var $modal = $('#responsive');
            $modal.modal();
        },
        error: function (data) {
            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
        }
    });
}

function resetControl() {
    $('#hdnid').val(0);
    $('#txtName').val('');
    //$('#chkIsActive').prop('checked', false);
    //$('#chkIsActive').parent('span').removeClass('checked');
    $('#chkIsActive').prop('checked', true);
    $('#chkIsActive').parent('span').addClass('checked');
    $('#drpTaxCountry').val('');
    $('#drpTaxState').empty().append('<option selected="selected" value="">Please select</option>');
    $('#drpTaxCity').empty().append('<option selected="selected" value="">Please select</option>');
    TempValues.City = '';
    TempValues.State = '';
}