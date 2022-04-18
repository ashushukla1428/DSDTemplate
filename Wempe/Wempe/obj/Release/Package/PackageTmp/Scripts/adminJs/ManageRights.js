///--------paging and listing functions 
var _lock = true;
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
        RoleID: $('#Roles').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder
    };
    $('#tbodyDetails').html('<tr><td colspan="3">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'ManageRights/getList',
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
            else {
                setPagingDetail(data.length, data[0].TotalCount, pageNo);
            }

        },
        error: function (_data) {

            $('#tbodyDetails').html('<tr><td colspan="3">error in request</td></tr>');

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
//update order detail 
//selected all checkbox
$('#allcb').change(function () {
    if ($(this).prop('checked')) {
        $('tbody tr td input[type="checkbox"]').each(function () {
            $(this).prop('checked', true);
        });
    } else {
        $('tbody tr td input[type="checkbox"]').each(function () {
            $(this).prop('checked', false);
        });
    }
});

function update()
{// Third way for jQuery 1.2
   // alert($('#Roles').val());
    $("input:checkbox[class=chk]").each(function () {
        alert("Id: " + $(this).attr("id") + " Value: " + $(this).val() + " Checked: " + $(this).is(":checked"));
    });

   
}