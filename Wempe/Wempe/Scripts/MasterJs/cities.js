var TempValues = { StateID: '', CountyId:''};
var _lock = true;

$(document).keypress(function (e) {
    if (e.which == 13) {
        getList(1);
    }
});

$('#ddlActiveAllList').change(function () {

    getList(1);
});

function getList(pageNo) {
    _lock = true;
    $('#hdnPageNo').val(pageNo);
    var model = {
        Name: $('#txtSearchName').val(),
        StateId: $('#drpSearchCityState').val(),

        CountyID: $('#drpSearchCityCounty').val(),

        CountryId: $('#drpSearchCountry').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder, ActiveOrAllCheck: $('#ddlActiveAllList').val()
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Master/getCityList',
        headers: { "requestType": "client" },
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            $('#tbodyDetails').html('');
            $('#tbodyTemplate').tmpl(data).appendTo('#tbodyDetails');
            if (data.length == 0) {
                setPagingDetail(data.length, 0, pageNo);

                $('#tbodyDetails').html('<tr><td colspan=5 align=centre>No Record Found</td></tr>');

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

$('#sample_1_previous').click(function () {
    var _index = parseInt($('#hdnPageNo').val()) - 1;
    if (_index > 0) {
        _lock = true;
        getList(_index);
    }
});

function Add() {
    if (!validate()) {
        return;
    }

    var model =
    {
        Id: parseInt($('#hdnid').val()),
        city: $('#txtName').val(),
        StateId: $('#drpcityState').val(),
       // ZipCode: $('#txtZipcode').val(),
        CountyID: $('#drpcityCounty').val(),
        IsActive: $('#chkIsActive').is(':checked')
    };


    $.ajax({
        url: siteUrl + 'Master/AddCity',
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

function Edit(id, stateId, countryId) {
    resetControl();
    
    if (id == 0) {
        $('#hdnid').val(id);
        var $modal = $('#responsive');
        $modal.modal();
        return;
    }
    TempValues.StateID = stateId;

    
    jQuery.ajax({
        url: siteUrl + 'Master/EditCity',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status == false) {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
                return;
            }

            $('#hdnid').val(data.Id);
            $('#txtName').val(data.city);

           // $('#txtZipcode').val(data.ZipCode);


            TempValues.CountyId = data.CountyID;



            $('#drpCityCountry').val(countryId);
            $("#drpCityCountry").trigger('change');
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
}

function bindStates(drpSates, id) {
    if (id == '0' || id == '') {
        return;
    }
    drpSates.empty().append('<option selected="selected" value="">Loading....</option>');
    jQuery.ajax({
        url: siteUrl + 'Master/getStates',
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
            
            if (TempValues.StateID != '') {
                drpSates.val(TempValues.StateID);
                TempValues.StateID = '';
            }


            bindCounty($("#drpcityCounty"), $('#drpcityState').val());
        },
        error: function (data) {
            drpSates.empty().append('<option selected="selected" value="">Please select</option>');
            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
        }
    });
}




function bindCounty(drpCounty, id) {
   
    if (id == '0' || id=='') {
        return;
    }
    drpCounty.empty().append('<option selected="selected" value="">Loading....</option>');
    jQuery.ajax({
        url: siteUrl + 'Master/getCounty',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "Id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status == false) {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
                return;
            }
            drpCounty.empty().append('<option selected="selected" value="">Please select</option>');
            $.each(data, function () {
                drpCounty.append($("<option></option>").val(this['Id']).html(this['County']));
            });

            if (TempValues.CountyId != '') {
                drpCounty.val(TempValues.CountyId);
                TempValues.CountyId = '';
            }
        },
        error: function (data) {
            drpCounty.empty().append('<option selected="selected" value="">Please select</option>');
            showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
        }
    });
}

$(document).ready(function () {
    $('#drpSearchCountry').change(function () {
        $('#drpSearchCityState').html('<option>Select State</option>');
        if ($('#drpSearchCountry').val() != '') {
            bindStates($("#drpSearchCityState"), $('#drpSearchCountry').val());
        }
        getList(1);
    });

    $('#drpSearchCityState').change(function () {
       
        if ($('#drpSearchCityState').val() != '') {
            bindCounty($("#drpSearchCityCounty"), $('#drpSearchCityState').val());
        }
        getList(1);
    });


    $('#drpSearchCityCounty').change(function () {

        //if ($('#drpSearchCityState').val() != '') {
        //    bindStates($("#drpSearchCityState"), $('#drpSearchCountry').val());
        //}
        getList(1);
    });



    bindStates($("#drpSearchCityState"), $('#drpSearchCountry').val());
    bindStates($("#drpcityState"), $('#drpCityCountry').val());

    $('#drpCityCountry').change(function () {
        bindStates($("#drpcityState"), $('#drpCityCountry').val());
        //getList(1);
    });


    $('#drpcityState').change(function () {
        bindCounty($("#drpcityCounty"), $('#drpcityState').val());
        //getList(1);
    });
});