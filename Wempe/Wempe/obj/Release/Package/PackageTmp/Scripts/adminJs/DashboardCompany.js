jQuery(document).ready(function () {
    $('#divCompanyDashboardRepairsBySearch').show();
    GetTodayRepairs('1', 'Search', '', '');
});


function setSorting2(_th, sortColumn, _items) {
    
    for (var i = 0; i < _items.length; i++) {
        //   alert(i);
        if ($(_th).index() != i) {
            $('#sample_1 tr').eq(0).find('th').eq(_items[i]).addClass("sort");
        }
    }
    // $('#sample_1 tr').eq(0).find('th').eq(3).addClass("sort");
    if (_sortColumn == sortColumn) {
        if (_sortOrder == 'DESC') {
            _sortOrder = 'ASC';

            $(_th).removeAttr("class").addClass("sorting_asc");
        }
        else {
            _sortOrder = 'DESC';
            $(_th).removeAttr("class").addClass("sorting_desc");
        }
    }
    else {
        _sortColumn = sortColumn;
        _sortOrder = 'DESC';

        $(_th).removeAttr("class").addClass("sorting_desc");
    }
    if ($('#Li3').hasClass('active')) {
        // $('#btnCompanyDashboardSearchRepairsByDate').click();

        GetTodayRepairs('1', 'Search', $('#txtCompanyDashboardStartDate').val(), $('#txtCompanyDashboardEndDate').val());
    }
    if ($('#Li1').hasClass('active')) {
        GetTodayRepairs('1', 'Today', '', '');
    }

    if ($('#Li2').hasClass('active')) {
        GetTodayRepairs('1', 'Week', '', '');
    }
   
}

function GetTodayRepairs(pageNo, Type, StartDate, EndDate) {
    _lock = true;
    var d = { PageNo: pageNo, Type: Type, StartDate: $('#txtCompanyDashboardStartDate').val(), EndDate: $('#txtCompanyDashboardEndDate').val(), sortColumn: _sortColumn,
        sortOrder: _sortOrder
    };

    $('#hdnPageNoRepair').val(pageNo);
    $('#tbodyDetailsRepair').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Company/searchRepair',
        headers: { "requestType": "client" },
        type: 'POST',
        data:  JSON.stringify(d),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            $('#tbodyDetailsRepair').html('');
            $('#tbodyTemplateRepair').tmpl(data).appendTo('#tbodyDetailsRepair');
            if (data.length == 0) {
                setPagingDetailRepair(data.length, 0, pageNo);

                $('#tbodyDetailsRepair').html('<tr><td colspan=7 align=centre>No Record Found</td></tr>');
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

    $('#txtCompanyDashboardStartDate,#txtCompanyDashboardEndDate').datepicker();
    $('#btnCompanyDashboardStartDateShow').click(function () {
        $('#txtCompanyDashboardStartDate').datepicker('show');
    });
    $('#btnCompanyDashboardEndDateShow').click(function () {
        $('#txtCompanyDashboardEndDate').datepicker('show');
    });
    $('#btnCompanyDashboardSearchRepairsByDate').click(function () {
        if ($('#txtCompanyDashboardStartDate').val() == '') {
            showMessage('Oops', 'error', 'Please fill start date!', 'toast-top-right');
            return;
        }
        else if ($('#txtCompanyDashboardEndDate').val() == '') {
            showMessage('Oops', 'error', 'Please fill end date!', 'toast-top-right');
            return;
        }
        else {
            var StartDate = $('#txtCompanyDashboardStartDate').val().split("/");
            var EndDate = $('#txtCompanyDashboardEndDate').val().split("/");
            var Start = new Date();
            var End = new Date();
            //Start = new Date(StartDate[2], StartDate[1] - 1, StartDate[0]);
            Start = new Date(StartDate[2], StartDate[0] - 1, StartDate[1]);
            End = new Date(EndDate[2], EndDate[0] - 1, EndDate[1]);
      
            if (Start > End) {
                showMessage('Oops', 'error', 'Start date can not greater then end date!', 'toast-top-right');
            } else {
                GetTodayRepairs('1', 'Search', '', '');
            }
        }
    });

    $('#sample_1_nextRepair').click(function () {
        if (_lock) {
            var _index = parseInt($('#hdnPageNoRepair').val()) + 1;
            if ($('#Li1').hasClass('active')) {
                GetTodayRepairs(_index, 'Today', '', '');
            }
            if ($('#Li2').hasClass('active')) {
                GetTodayRepairs(_index, 'Week', '', '');
            }
            if ($('#Li3').hasClass('active')) {
                GetTodayRepairs(_index, 'Search', '', '');
            }
        }
    });
    $('#sample_1_previousRepair').click(function () {
        var _index = parseInt($('#hdnPageNoRepair').val()) - 1;
        if (_index > 0) {
            _lock = true;
            if ($('#Li1').hasClass('active')) {
                GetTodayRepairs(_index, 'Today', '', '');
            }
            if ($('#Li2').hasClass('active')) {
                GetTodayRepairs(_index, 'Week', '', '');
            }
            if ($('#Li3').hasClass('active')) {
                GetTodayRepairs(_index, 'Search', '', '');
            }
        }
    });
    $("#ulListCompanyDashboard li a").click(function () {
        $('#divCompanyDashboardRepairsBySearch').hide();
        if ($(this).parent().attr("id") == 'Li1') {
            GetTodayRepairs('1', 'Today', '', '');
        }
        if ($(this).parent().attr("id") == 'Li2') {
            GetTodayRepairs('1', 'Week', '', '');
        }
        if ($(this).parent().attr("id") == 'Li3') {
            $('#sample_1_infoRepair').html('Showing 0 of 0 entries');
            $('#tbodyDetailsRepair').html('');
            $('#divCompanyDashboardRepairsBySearch').show();
            if ($('#txtCompanyDashboardStartDate').val() != '' && $('#txtCompanyDashboardEndDate').val() != '') {
                GetTodayRepairs('1', 'Search', '', '');
            }
        }
    });
});