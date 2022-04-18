
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
        BrandName: $('#Brand_Name').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder
    };
    $('#tbodyDetails').html('<tr><td colspan="8">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Manufacturer/getList',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            $('#tbodyDetails').html('');
            $('#tbodyTemplate').tmpl(data).appendTo('#tbodyDetails');

            setPagingDetail(data.length, data[0].TotalCount, pageNo);

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

$('#sample_1_previous').click(function () {

    var _index = parseInt($('#hdnPageNo').val()) - 1;
    if (_index > 0) {
        _lock = true;
        getList(_index);
    }
});
//update order detail 
function updateBrand() {
    var SaveData = 0;
    var formData = new FormData();
    var totalFiles = document.getElementById("file_upload").files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("file_upload").files[i];       
        var filename = file.name;
        formData.append("file_upload", file);
    } 
    if (document.getElementById("file_upload").files.length > 0) {
        $('#HFImageName').val(filename);
        $.ajax({
            type: "POST",
            url: siteUrl + 'Manufacturer/Upload',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
              //  alert(JSON.stringify(response));
            },
            error: function (error) {
                alert("Error while uploading file");
            }
        });
    }
    var _imagePath = $('#file_upload').val();
    if (_imagePath == '')
    {
        _imagePath = $('#HFImageName').val();
        //alert(_imagePath);
    }
    if (validate()) {
      
            var model =
               {
                   ManufactureID: $('#hfDataID').val(),
                   ManufactureName: ($('#txtBrandName').val()),
                   BrandCode: ($('#txtBrandCode').val()),
                   DropShipFee: ($('#txtDropShipFee').val()),
                   ShipZip: $('#txtShipZip').val(),
                   URLkey: $('#txtURLkey').val(),
                   SortOrder: $('#txtSortOrder').val(),
                   ManufacturePhoto: _imagePath,
                   BrandType: $('#drpBrandType option:selected').val()
               };

            $.ajax({
                url: siteUrl + 'Manufacturer/Edit',
                type: 'POST',
                data: JSON.stringify(model),
                contentType: 'application/json;charset=utf-8',
                success: function (data) {
                    // alert(JSON.stringify(data.SubTotal));
                    alert(_saveMessage);
                },
                error: function (_data) {
                    alert(_errorMessage);
                }
            });
        }
        //$('#txtBrandName').val('');
        //$('#hfDataID').val('');
        //$('#txtBrandCode').val('');
        //$('#txtDropShipFee').val('');
        ////$('#txtShipState').val('');
        //$('#txtShipZip').val('');
        //$('#txtURLkey').val('');
        //$('#txtSortOrder').val('');
        //$('#file_upload').val('');
    
}

 