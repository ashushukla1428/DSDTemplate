jQuery(document).ready(function () {
    GetCompletedRepairsByYear();
    jQuery('#Years').change(function () {
        GetCompletedRepairsByYear();
    });
    $(".btnPrint").printPage({
        //alert('s');
        url: '../Letters/IndexAdmin?callType=#CallType&repairNumber=' +0 + "&priAddress=#priAddress" + "&random=" + Math.random(),
        attr: "href",
        message: "Your document is being created"
    });
});

function GetCompletedRepairsByYear()
{
    $('.clsCompletedRepairsByYear').html($('#Years option:selected').text());
    jQuery.ajax({
        url: siteUrl + 'Admin/GetCompleteRepairsByYear',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "Year": $('#Years option:selected').text() },
        contentType: 'application/json;charset=utf-8',
        success: function (_data) {
            $('#tbodyDetailsCompletedRepairs').html('');
            var HTML = ''
            var count = 0;
            //alert(_data.count);
            if (_data != '') {
                $.each(_data, function (i) {
                    count = count + 1;
                    var TempDate = new Date(parseInt(_data[i].entryDate.replace("/Date(", "").replace(")/", ""), 10));
                    HTML += '<tr><td>' + TempDate.toUTCString() + '</td><td><a href=../Repair/NewRepair?id=2&Rid=' + _data[i].repairNumber + '>' + _data[i].repairNumberComplete + '</a></td><td>' + _data[i].Status + '</td></tr>';
                });
            }
            else {
                HTML = '<tr><td colspan=3 align=center>No Record Found!</td></tr>';
            }
            $('#tbodyDetailsCompletedRepairs').html(HTML);
            $('#CompletedRepairByYearCount').html(count);
        }
    });
}

function openPrintBoxAdmin(_callType, _priAddress, _showPopup, _newReport) {

    if (_newReport == 0) {
        $('#hdnCallType').val(_callType);
    }
    else {
        _callType = $('#hdnCallType').val();
    }

    //if (_priAddress == 0) {
    //    if ($('#hdnPriAddress').val() == "0") {
    //        $('#hdnPriAddress').val(1);
    //        $('#divAlternate').show();
    //        $('#divPrimary').hide();
    //        _priAddress = 1;
    //    }
    //    else {
    //        $('#hdnPriAddress').val(0);
    //        $('#divAlternate').hide();
    //        $('#divPrimary').show();
    //        _priAddress = 0;
    //    }


    //}
    //else {
    //    $('#hdnPriAddress').val(_priAddress);
    //}

    $('#divPrintAdmin').empty();
    $('#divPrintAdmin').html('Loading....');
    $('#divPrintAdmin').load('../Letters/IndexAdmin?callType=' + _callType + '&repairNumber=' + 0 + "&priAddress=" + _priAddress + "&random=" + Math.random());
    //if (_showPopup == 1) {
        var $modal = $('#responsivePrintAdmin');
        $modal.modal();
    //}
}