var _lock = true;

$(document).keypress(function (e) {
    if (e.which == 13) {
        getList(1);
    }
});
$('#ddlActiveAllList').change(function () {

    getList(1);
});

$('#drpBrandSearch').change(function () {
    getList(1);
});

function getList(pageNo) {
    _lock = true;
    $('#hdnPageNo').val(pageNo);
    var model = {
        Name: $('#txtSearchName').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder,
        BrandId: $('#drpBrandSearch').val(),
        ActiveOrAllCheck: $('#ddlActiveAllList').val()
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'BoxIncluded/getList',
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
        BoxIncludedID: parseInt($('#hdnid').val()),
        BoxIncluded: $('#txtName').val(),
        brandId: parseInt($('#drpBrands').val()),
        IsActive: $('#chkIsActive').is(':checked')
    };


    $.ajax({
        url: siteUrl + 'BoxIncluded/Add',
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
        url: siteUrl + 'BoxIncluded/Edit',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status == false) {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
                return;
            }
            $('#hdnid').val(data.BoxIncludedID);
            $('#txtName').val(data.BoxIncluded);
            $('#drpBrands').val(data.brandId);
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