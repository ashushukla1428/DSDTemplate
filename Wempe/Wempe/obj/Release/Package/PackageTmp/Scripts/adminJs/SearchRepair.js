

function searchRepair(pageNo, CustomerNumber) {
    _lock = true;
    $('#hdnPageNoRepair').val(pageNo);
    var _SearchFields = '';
    var _SearchValues = '';
    $.each($(".SearchValueRepair"), function () {
        //alert($(this).attr('fieldName'));
        //alert($(this).val());
        if ($(this).val() != '') {
            if ($(this).val() != '0' && $(this).val() != undefined && $(this).attr('fieldNameRepair') != undefined) {
                _SearchFields += $(this).attr('fieldNameRepair') + '|';
                _SearchValues += $(this).val() + '|';
            }
        }
        // Do something
    });
    var model = {
        SearchFields: _SearchFields,
        SearchValues: _SearchValues,
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder,
        CustomerNumber: CustomerNumber,


        // column for dates


        dueDateCustomerStart: $('#txtTab2DueDateCUSTOMERDays').val(),
        dueDateCustomerEnd: $('#txtTab2DueDateCUSTOMERDays2').val(),
        dueDateSupplierStart: $('#txtTab2DueDateSupplierDays').val(),
        dueDateSupplierEnd: $('#txtTab2DueDateSupplierDays2').val(),
        EntryDateStart: $('#txtRepairSearchEntryDateStart').val(),
        EntryDateEnd: $('#txtRepairSearchEntryDateEnd').val()

    };
    $('#tbodyDetailsRepair').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Repair/searchRepair',
        headers: { "requestType": "client" },
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            $('#tbodyDetailsRepair').html('');
            $('#tbodyTemplateRepair').tmpl(data).appendTo('#tbodyDetailsRepair');
            if (data.length == 0) {
                setPagingDetailRepair(data.length, 0, pageNo);
            }
            else {
                setPagingDetailRepair(data.length, data[0].TotalCount, pageNo);
            }
        },
        error: function (_data) {
            $('#tbodyDetailsRepair').html('<tr><td colspan="6">error in request</td></tr>');
        }
    });
}
function setPagingDetailRepair(noOfrows, Total, pageNo) {
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
    $('#sample_1_infoRepair').html('Showing ' + _firstRecord + ' to ' + noOfrows + ' of ' + Total + ' entries');
}
$(document).ready(function () {

    $('#txtTab2DueDateCUSTOMERDays').datepicker();
    $('#txtTab2DueDateCUSTOMERDays2').datepicker();
    $('#btnCalenderShowSearchRepairDueDateCustomerStart').click(function () {
        $('#txtTab2DueDateCUSTOMERDays').datepicker('show');
    });
    $('#btnCalenderShowSearchRepairDueDateCustomerEnd').click(function () {
        $('#txtTab2DueDateCUSTOMERDays2').datepicker('show');
    });



    $('#txtTab2DueDateSupplierDays').datepicker();
    $('#txtTab2DueDateSupplierDays2').datepicker();

    $('#btnCalenderShowSearchRepairDueDateSupplierStart').click(function () {
        $('#txtTab2DueDateSupplierDays').datepicker('show');
    });
    $('#btnCalenderShowSearchRepairDueDateSupplierEnd').click(function () {
        $('#txtTab2DueDateSupplierDays2').datepicker('show');
    });

    $('#txtRepairSearchEntryDateStart').datepicker();
    $('#txtRepairSearchEntryDateEnd').datepicker();

    $('#btnCalenderShowSearchRepairEntryDateStart').click(function () {
        $('#txtRepairSearchEntryDateStart').datepicker('show');
    });
    $('#btnCalenderShowSearchRepairEntryDateEnd').click(function () {
        $('#txtRepairSearchEntryDateEnd').datepicker('show');
    });




    $('#sample_1_nextRepair').click(function () {
        if (_lock) {
            var _index = parseInt($('#hdnPageNoRepair').val()) + 1;
            searchRepair(_index, 0);
        }
    });
    $('#sample_1_previousRepair').click(function () {
        var _index = parseInt($('#hdnPageNoRepair').val()) - 1;
        if (_index > 0) {
            _lock = true;
            searchRepair(_index, 0);
        }
    });
});

function search_Customer_Repair(pageNo) {

    searchCustomer(pageNo);
    //searchRepair(pageNo);//s

    searchRepair(pageNo, 0);
};


function searchCustomer(pageNo) {

   


    _lock = true;
    _lockCustomerSearch = true;
    $('#hdnPageNo').val(pageNo);
    var _SearchFields = '';
    var _SearchValues = '';
    $.each($(".searchValue"), function () {
        if ($(this).val() != '') {
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
        url: siteUrl + 'Repair/searchCustomer',
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
        //if($('#divSearchbyExistingCustomer').css('display'))
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

function ShowExistingRepairSearch()
{
    searchRepair(1, 0);
    $('#divRepairListByCustomerNumberPopUp').removeAttr('style');
    $('#divRepairListByCustomerNumberPopUp').removeClass('modal');
    $('#divRepairListByCustomerNumberPopUp').removeClass('fade');
    $('#divRepairListPopupHeader').hide();
}
function ShowRepairListByCustomerNoPopUp(CustomerNumber) {
    searchRepair(1, CustomerNumber);
    $('#divRepairListPopupHeader').show();
    $('#divRepairListByCustomerNumberPopUp').addClass('modal');
    $('#divRepairListByCustomerNumberPopUp').addClass('fade');
    var $modal = $('#divRepairListByCustomerNumberPopUp');
    $modal.modal();
    return;
}