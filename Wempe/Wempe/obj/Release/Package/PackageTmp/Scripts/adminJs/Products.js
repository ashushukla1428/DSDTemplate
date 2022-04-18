var _lock = true;
var mainPhoto = "";
var altImage1 = "";
var altImage2 = "";
var altImage3 = "";
var colorSwatch = "";

var mainext = "";
var alt2ext = "";
var alt3ext = "";
var alt4ext = "";

$(document).keypress(function (e) {
    if (e.which == 13) {
        getList(1);
    }
});

function activeAction(tabIndex)
{
    if (tabIndex == 1) {
        $('#headerActionButton').show();
        $('#footerActionButton').show();
    }
    else {
        $('#headerActionButton').hide();
        $('#footerActionButton').hide();
    }
    
    //
}
function getList(pageNo) {
    _lock = true;
    $('#hdnPageNo').val(pageNo);
   // alert(_sortColumn);

    var model = {
        GroupSKU: $('#txtGroupSKU').val(),
        Status: '0',
        InStock: $('#drpStatus').val(),
        CategoryID: parseInt($('#drpDepartment').val()),
        BrandID: parseInt($('#drpBrands').val()),
        pageno: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder,
        Name: $('#txtItemName').val(),
        Visible: $('#txtCheckbox').is(":checked")

    };
    $('#tbodyDetails').html('<tr><td colspan="9">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Product/getList',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            
            
            if (data.length != 0) {
                setPagingDetail(data.length, data[0].TotalCount, pageNo);
                $('#tbodyDetails').html('');
                $('#tbodyTemplate').tmpl(data).appendTo('#tbodyDetails');
            }
            else {
                setPagingDetail(0, 0, pageNo);
                $('#tbodyDetails').html('<tr><td colspan="9">no record found</td></tr>');
            }
            //  alert(data);
        },
        error: function (_data) {

            $('#tbodyDetails').html('<tr><td colspan="9">error in request</td></tr>');

        }
    });
}

function extract(what)
{
    //alert(what);
    if (what.indexOf('/') > -1)
        answer = what.substring(what.lastIndexOf('/') + 1, what.length);
    else
        answer = what.substring(what.lastIndexOf('\\') + 1, what.length);
     return answer;
}
//update product detail 
function SaveProducts() {

    //get image name
    mainPhoto=extract(selectedMainImage.value);
    altImage1 = '';//extract(selectedAltImage1.value);
    altImage2 = '';//extract(selectedAltImage2.value);
    altImage3 = '';//extract(selectedAltImage3.value);
    

    mainext = mainPhoto.substring(mainPhoto.indexOf("."));
    alt2ext = '';// altImage1.substring(altImage1.indexOf("."));
    alt3ext = '';//altImage2.substring(altImage2.indexOf("."));
    alt4ext = '';//altImage3.substring(altImage3.indexOf("."));

    if (mainext != '') {
        mainPhoto = $('#txtgroupSKU').val() + 'alt5' + mainext;
    }

    if (alt2ext != '') {
        altImage1 = $('#txtgroupSKU').val() + 'alt2' + alt2ext;
    }

    if (alt3ext != '') {
        altImage2 = $('#txtgroupSKU').val() + 'alt3' + alt3ext;
    }

    if (alt4ext != '') {
        altImage3 = $('#txtgroupSKU').val() + 'alt4' + alt4ext;
    }

    colorSwatch = extract(selectedColorSwatchImage.value);
    if (mainPhoto == '')
    {
        mainPhoto = $('#BigImage').val();
    }
    if (colorSwatch == '') {
        colorSwatch = $('#ColorSwatch').val();
    }
    //  alert($('#txtBulletPoint11').val());
    //  if (validate()) {
    var model =
       {
           DataID: parseInt($('#hdnid').val()),
           ProductName: $('#txtName').val(),
           ItemNo: $('#txtItemNo').val(),
           AlternateItemSku: $('#txtAlternateItemSKU').val(),
           GroupSKU: $('#txtgroupSKU').val(),
           BrandName: $('#drpBrands option:selected').text(),
           BrandID: $('#drpBrands option:selected').val(),
           MPN: $('#txtMPN').val(),
           ItemSize: $('#txtSize').val(),
           ItemColor: $('#txtColor').val(),
           ItemWeight: $('#txtWeight').val(),
           UPC: $('#txtUPC').val(),
           MainProduct: $('#drpMainProducts').val() - 1,
           Pack: $('#txtPack').val(),
           HiddenPack: $('#txtHiddenPack').val(),
           MetaDescription: $('#txtMetaDescription').val(),
           SortOrder: $('#txtSortBy').val(),
           VideoLink: $('#txtVideoLink').val(),
           URLKey: $('#txtURL').val(),
           RelateItems: $('#txtRelatedItems').val(),
           ItemStatus: $('#drpItemStatus').val(),
           Counts: $('#txtCounts').val(),
           ShipCanada: $('#txtShipCanada').val(),
           FullURL: $('#txtClickURL').val(),
           AmazonURL: $('#txtAmazonURL').val(),
           AmazonID: $('#txtASIN').val(),
           CostPrice: $('#txtCostPrice').val(),
           MSRP: $('#txtMSRP').val(),
           LowPrice: $('#txtLowPrice').val(),
           OurPrice: $('#txtOurPrice').val(),
           FeedPrice: $('#txtFeedPrice').val(),
           ShippingRate: $('#drpShipingRate').val(),
           ShippingPrice: $('#txtShippingPrice').val(),
           StockType: $('#drpStockType option:selected').val(),
           InStock: $('#drpInStock option:selected').val(),
           StockMsg: $('#txtStockMSG').val(),
           BackOrderDate: $('#txtBackOrderDate').val(),
           StockNo: $('#txtInventory').val(),
           InventoryType: $('#drpTrackInventory option:selected').val(),

           Features: $('#txtFeatures').val(),
           BulletPoint1: $('#txtBulletPoint1').val(),
           BulletPoint2: $('#txtBulletPoint2').val(),
           BulletPoint3: $('#txtBulletPoint3').val(),
           BulletPoint4: $('#txtBulletPoint4').val(),
           BulletPoint5: $('#txtBulletPoint5').val(),
           BulletPoint6: $('#txtBulletPoint6').val(),
           BulletPoint7: $('#txtBulletPoint7').val(),
           BulletPoint8: $('#txtBulletPoint8').val(),
           BulletPoint9: $('#txtBulletPoint9').val(),
           BulletPoint10: $('#txtBulletPoint10').val(),
           BulletPoint11: $('#txtBulletPoint11').val(),
           BulletPoint12: $('#txtBulletPoint12').val(),
           BulletPoint13: $('#txtBulletPoint13').val(),
           BulletPoint14: $('#txtBulletPoint14').val(),
           BulletPoint15: $('#txtBulletPoint15').val(),
           BulletPoint16: $('#txtBulletPoint16').val(),
           BulletPoint17: $('#txtBulletPoint17').val(),
           BulletPoint18: $('#txtBulletPoint18').val(),
           BulletPoint19: $('#txtBulletPoint19').val(),
           BulletPoint20: $('#txtBulletPoint20').val(),

           Description: $('#txtDescription').val(),
           Point1: $('#txtPoint1').val(),
           Point2: $('#txtPoint2').val(),
           Point3: $('#txtPoint3').val(),
           Point4: $('#txtPoint4').val(),
           Point5: $('#txtPoint5').val(),
           Point6: $('#txtPoint6').val(),
           Point7: $('#txtPoint7').val(),
           Point8: $('#txtPoint8').val(),
           Point9: $('#txtPoint9').val(),
           Point10: $('#txtPoint10').val(),
           Point11: $('#txtPoint11').val(),
           Point12: $('#txtPoint12').val(),
           Point13: $('#txtPoint13').val(),
           Point14: $('#txtPoint14').val(),
           Point15: $('#txtPoint15').val(),
           Point16: $('#txtPoint16').val(),
           Point17: $('#txtPoint17').val(),
           Point18: $('#txtPoint18').val(),
           Point19: $('#txtPoint19').val(),
           Point20: $('#txtPoint20').val(),
           gTitle: $('#txtgTitle').val(),
           gDescription: $('#txtgDescription').val(),
           gCategory: $('#txtgCategory').val(),
           gGrouping: $('#txtgGrouping').val(),
           gMPN: $('#txtgMPN').val(),
           gType: $('#txtgType').val(),
           gBrand: $('#txtgBrand').val(),
           gUPC: $('#txtgUPC').val(),
           gSize: $('#txtgSize').val(),
           gColor: $('#txtgColor').val(),

           MainPhoto: mainPhoto,
           AltImage1: altImage1,
           AltImage2: altImage2,
           AltImage3: altImage3,
           ColorSwatch: colorSwatch,
           Visible: $('#txtCheckbox').is(":checked")
       };
    

    $.ajax({
        url: siteUrl + 'Product/Edit',
        type: 'POST',
        data: JSON.stringify({ model: model }),
        contentType: 'application/json;charset=utf-8',
        processData: false,
        dataType: "html",
        success: function (data) {
            // alert(JSON.stringify(data));
            if (parseInt($('#hdnid').val()) == 0)
            {
                window.location = siteUrl+"/Product/Edit/"+data;
            }
            else if (parseInt(data) == -1) {
                alert(_errorMessage);
            }
            else { alert(_saveMessage);  }
            
            $('#selectedMainImage').val('');
        },
        error: function (_data) {

            alert(_errorMessage);

        }
    });
    // }
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


$("#btnBrowsedSave").click(function () {
   // alert('ok');
    if (document.getElementById("selectedMainImage").files.length != 0) {
        $('#btnBrowsedSave').attr("disabled", true);
        $('#btnBrowsedSave').val('Uploading...');
        var formData = new FormData();
        var totalFiles = document.getElementById("selectedMainImage").files.length;
        var browsedFile = document.getElementById("selectedMainImage").files[0];
        if (browsedFile.type.match('image.*')) {
            formData.append($('#txtgroupSKU').val() + "FileUpload5", browsedFile);
            $.ajax({
                type: "POST",
                url: siteUrl + 'Product/UploadMainPhoto',
                data: formData,
                dataType: "html",
                contentType: false,
                processData: false,
                success: function (result) {
                    
                    $('#btnBrowsedSave').attr("disabled", false);
                    $('#btnBrowsedSave').val('Upload Main Image');
                    
                    $("#MIContainerDiv").empty().append(result);
                    // $("#BigImg").attr("src", "http://images.holdnstorage.com/images/" + $('#txtgroupSKU').val() + 'alt5' + mainext);
                   
                    //SaveProducts();

                }
            });
        }
        else {


            $('#btnBrowsedSave').attr("disabled", false);
            $('#btnBrowsedSave').val('Upload Main Image');
            alert("Please browse image file only.");
        }
    }
    else {
        alert("Please browse file first.");
    }
});

$("#btnBrowsedALtImage1").click(function () {

    var formData = new FormData();
    var totalFiles = document.getElementById("selectedAltImage1").files.length;
    var browsedFile = document.getElementById("selectedAltImage1").files[0];
    if (browsedFile.type.match('image.*')) {
        formData.append($('#txtgroupSKU').val()+"FileUpload2", browsedFile);
        $.ajax({
            type: "POST",
            url: siteUrl + 'Product/UploadMainPhoto',
            data: formData,
            dataType: "html",
            contentType: false,
            processData: false,
            success: function (result) {
                $("#AI1ContainerDiv").empty().append(result);
            }
        });
    }
    else {
        alert("Please browse image file only.");
    }
});

$("#btnBrowsedALtImage2").click(function () {

    var formData = new FormData();
    var totalFiles = document.getElementById("selectedAltImage2").files.length;
    var browsedFile = document.getElementById("selectedAltImage2").files[0];
    if (browsedFile.type.match('image.*')) {
        formData.append($('#txtgroupSKU').val()+"FileUpload3", browsedFile);
        $.ajax({
            type: "POST",
            url: siteUrl + 'Product/UploadMainPhoto',
            data: formData,
            dataType: "html",
            contentType: false,
            processData: false,
            success: function (result) {
                $("#AI2ContainerDiv").empty().append(result);
            }
        });
    }
    else {
        alert("Please browse image file only.");
    }
});

$("#btnBrowsedALtImage3").click(function () {


    var formData = new FormData();
    var totalFiles = document.getElementById("selectedAltImage3").files.length;
    var browsedFile = document.getElementById("selectedAltImage3").files[0];
    if (browsedFile.type.match('image.*')) {
        formData.append($('#txtgroupSKU').val() + "FileUpload4", browsedFile);
        $.ajax({
            type: "POST",
            url: siteUrl + 'Product/UploadMainPhoto',
            data: formData,
            dataType: "html",
            contentType: false,
            processData: false,
            success: function (result) {
                $("#AI3ContainerDiv").empty().append(result);
            }
        });
    }
    else {
        alert("Please browse image file only.");
    }
});

$("#btnBrowsedColorSwatch").click(function () {

    if (document.getElementById("selectedColorSwatchImage").files.length != 0) {

        $('#btnBrowsedColorSwatch').attr("disabled", true);
        $('#btnBrowsedColorSwatch').val('Uploading...');

        var formData = new FormData();
        var totalFiles = document.getElementById("selectedColorSwatchImage").files.length;
        var browsedFile = document.getElementById("selectedColorSwatchImage").files[0];
        if (browsedFile.type.match('image.*')) {
            formData.append($('#txtgroupSKU').val() + "FileUpload1", browsedFile);
            $.ajax({
                type: "POST",
                url: siteUrl + 'Product/UploadMainPhoto',
                data: formData,
                dataType: "html",
                contentType: false,
                processData: false,
                success: function (result) {
                    $("#CSContainerDiv").empty().append(result);
                    $('#btnBrowsedColorSwatch').attr("disabled", false);
                    $('#btnBrowsedColorSwatch').val('Upload Color Swatch Image');
                }
            });

        }
        else {
            alert("Please browse image file only.");
            $('#btnBrowsedColorSwatch').attr("disabled", false);
            $('#btnBrowsedColorSwatch').val('Upload Color Swatch Image');
        }
    }
    else {
        alert("Please browse file first.");
    }
});


function resetProductCategory() {
    _id = "";
    $('#drpdwnDepartment').val(0);
    $('#txtCategorySortBy').val('');
}

 

 

///Update ProductCategory
var _id = 0;
var _selectedIndex = 0;
function rowClick(row) {

    var tableData = $(row).children("td").map(function () {
        return $(this).text();
    }).get();
    //alert(JSON.stringify(tableData));
    //_id = $.trim(tableData[0]);
    _id = $.trim(tableData[0]).split(',');
        //alert(_id[0]);
        //alert(_id[1]);
        //alert(_id[2]);

    $('#drpdwnDepartment').val(_id[1]);
    $('#txtCategorySortBy').val(_id[2]);

    //alert($(row).index());
    _selectedIndex = $(row).index();
    //alert($.trim(tableData[0]));
}

 

 

function updateProductCategory() {
   
    //if (validateCartValues()) {
   // alert($('#txtCategorySortBy').val());
    var model =
        {
            ProductID: parseInt($('#hdnid').val()),
            Id: parseInt(_id[0]),
            Category: parseInt($('#drpdwnDepartment').val()),
            SortOrder: parseInt($('#txtCategorySortBy').val())
        };
    $.ajax({
        url: siteUrl + 'Product/UpdateProductCategories',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            // alert($('#drpDepartment option:selected').text());

            //alert(isNaN(_id[0]));
            if (isNaN(_id[0]) && parseInt(data) > 0) {
               
                $('#tbCategory').append('<tr  onclick="rowClick(this);"><td style="display:none;">' + data + ',' + $('#drpdwnDepartment').val() + ',' + $('#txtCategorySortBy').val() + '</td><td>' + $('#drpdwnDepartment option:selected').text() + '</td><td>' + $('#txtCategorySortBy').val() + '</td></tr>');
            }
            else {
                $('#tbCategory tr').eq(_selectedIndex + 1).find('td').eq(0).text(data + ',' + $('#drpdwnDepartment').val() + ',' + $('#txtCategorySortBy').val());
                $('#tbCategory tr').eq(_selectedIndex + 1).find('td').eq(1).text($('#drpdwnDepartment option:selected').text());
                $('#tbCategory tr').eq(_selectedIndex + 1).find('td').eq(2).text($('#txtCategorySortBy').val());
            }
            if (parseInt(data) > 0) {
                alert(_saveMessage);
                resetProductCategory();
            }
            else {
                alert(_errorMessage);
            }

        },
        error: function (_data) {
            alert(_errorMessage);
        }
    });
    //}
}
function deleteProductCategory()
{
    
    var model =
         {
             ProductID: parseInt($('#hdnid').val()),
             Id: parseInt(_id[0]),
             Category: parseInt($('#drpdwnDepartment').val()),
             SortOrder: parseInt($('#txtCategorySortBy').val())
         };
    
    $.ajax({
        url: siteUrl + 'Product/deleteProductCategory',
        type: 'POST',
        "dataType": "json",
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Success == 'true') {
                $('#tbCategory tr').eq(_selectedIndex+1).remove();
                resetProductCategory();
            }
            alert(data.Message);
        },
        error: function (_data) {
            alert(_errorMessage);
        }
    });
}

//------------------New Function-------------
var _productImages = 0;
function productImagesRowClick(row) {

    var tableData = $(row).children("td").map(function () {
        return $(this).text();
    }).get();
    //alert(JSON.stringify(tableData));
    //_id = $.trim(tableData[0]);
    _productImages.length = 0;
    _productImages = $.trim(tableData[0]).split(',');
   // alert(_productImages[3]);
    $('#txtImageSortBy').val(_productImages[2]);
    $("#imgProductImage").attr("src", _imagegRealPath + _productImages[3]);
    
    _selectedIndex = $(row).index();
    
}

$("#btnUploadProductImage").click(function () {
    

        $('#btnUploadProductImage').attr("disabled", true);
        $('#btnUploadProductImage').val('Uploading...');
        var formData = new FormData();
        var totalFiles = document.getElementById("uploadProductImag").files.length;
        var browsedFile = document.getElementById("uploadProductImag").files[0];

        formData.append("SortOrder", parseInt($('#txtImageSortBy').val()));
        formData.append("GroupSKU", $('#txtgroupSKU').val());
        formData.append("ProductID", parseInt($('#hdnid').val()));
        formData.append("oldPhoto", _productImages[3]);

        if (parseInt($('#hndid').val()) != 0 && totalFiles == 0) {

            formData.append("FileUpload", null);
            formData.append("Id", parseInt(_productImages[0]));
            uploadProductImages(formData);

        }
        else {
            if (browsedFile.type.match('image.*')) {

                formData.append("FileUpload", browsedFile);
                formData.append("Id", parseInt(_productImages[0]));
              
                uploadProductImages(formData);
            }
            else {

                $('#btnUploadProductImage').attr("disabled", false);
                $('#btnUploadProductImage').val('Save Image');
                alert("Please browse image file only.");
            }
        }
    
});

function uploadProductImages(formData)
{
    $.ajax({
        type: "POST",
        url: siteUrl + 'Product/UploadProductPhoto',
        data: formData,
        dataType: "html",
        contentType: false,
        processData: false,
        success: function (result) {
            //$("#AI3ContainerDiv").empty().append(result);
            // alert(JSON.stringify(result));
            obj = JSON.parse(result);
            //  alert(obj.DataID);
            $('#uploadProductImag').val('');
            $('#btnUploadProductImage').attr("disabled", false);
            $('#btnUploadProductImage').val('Save Image');
            if (isNaN(_productImages[0])) {

                $('#tbProductImages').append('<tr class="itemtable"  onclick="productImagesRowClick(this);"><td style="display:none;">' + obj.DataID + ',' + obj.StyleNo + ',' + $('#txtImageSortBy').val() + ',' + obj.photo + '</td><td> <img style="height:100px;width:100px;" src="' + _imagegRealPath + obj.photo + '">  </td><td>' + $('#txtImageSortBy').val() + '</td></tr>');
            }
            else {
                $('#tbProductImages tr').eq(_selectedIndex + 1).find('td').eq(1).find('img').remove();
                $('#tbProductImages tr').eq(_selectedIndex + 1).find('td').eq(1).append('<img style="height:100px;width:100px;" src="'+_imagegRealPath + obj.photo + '">');
                $('#tbProductImages tr').eq(_selectedIndex + 1).find('td').eq(2).text($('#txtImageSortBy').val());

                $('#tbProductImages tr').eq(_selectedIndex + 1).find('td').eq(0).text(obj.DataID + ',' + obj.StyleNo + ',' + $('#txtImageSortBy').val() + ',' + obj.photo);

            }
            $('#txtImageSortBy').val(0);
            $("#imgProductImage").attr("src", "~/Images/icon_image_not_available_b.gif");
            _productImages.length = 0;
            alert(_saveMessage);
            
            //_productImages = null;
            //_selectedIndex = null;

        }
    });
}
$("#btnResetProductImage").click(function () {
    $('#btnUploadProductImage').val('Upload Image');
    $('#uploadProductImag').val('');
    $('#txtImageSortBy').val(0);
    $('#btnUploadProductImage').attr("disabled", false);
    $("#imgProductImage").attr("src", "~/Images/icon_image_not_available_b.gif");
    _productImages.length = 0;
})




//--------------------

function rowStylesClick(row) {

    $("#BtnStylesSave").text('Update');
    var tableData = $(row).children("td").map(function () {
        return $(this).text();
    }).get();
    //alert(JSON.stringify(tableData));
    //_id = $.trim(tableData[0]);
    _id = $.trim(tableData[0]).split(',');
    $('#hdnStyleID').val($.trim(tableData[2]).split(','))
    $('#drpStyles').val(_id[0]);

    //alert($(this).index());
    _selectedIndex = $(row).index();
    //alert($.trim(tableData[0]));
}
function updateProductStyle() {
    //alert(_selectedIndex);
    //if (validateCartValues()) {

    var model =
        {
            DetailDataID: parseInt($('#hdnid').val()),
            StyleID: parseInt($('#drpStyles').val()),
            OldStyleID: parseInt($('#hdnStyleID').val()),
            StyleName: $("#drpStyles option:selected").text(),
        };
    $.ajax({
        url: siteUrl + 'Product/UpdateProductStyles',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            // alert($('#drpDepartment option:selected').text());

            //alert(isNaN(_id[0]));
            if (isNaN(_id[0]) && parseInt(data) > 0) {

                $('#tbProductStyleDetail').append('<tr onclick="rowStylesClick(this);"><td style="display:none;">' + $('#drpStyles').val() + '</td><td>' + $('#drpStyles option:selected').text() + '</td><td style="display:none;">' + data + '</td></tr>');
            }
            else {
                $('#tbProductStyleDetail tr').eq(_selectedIndex + 1).find('td').eq(0).text($('#drpStyles').val());
                $('#tbProductStyleDetail tr').eq(_selectedIndex + 1).find('td').eq(1).text($('#drpStyles option:selected').text());

            }
            if (parseInt(data) > 0) {
                alert(_saveMessage);

                resetProductStyle();
            }
            else {
                alert(_errorMessage);
            }

        },
        error: function (_data) {
            alert(_errorMessage);
        }
    });
    //}

}
function resetProductStyle() {
    _id = "";
    $('#drpStyles').val(0);
    $("#BtnStylesSave").text('Save');
    $('#hdnStyleID').val('0');

}
function rowMaterialsClick(row) {

    $("#BtnMaterialSave").text('Update');
    var tableData = $(row).children("td").map(function () {
        return $(this).text();
    }).get();

    _id = $.trim(tableData[0]).split(',');
    $('#hdnMaterialID').val($.trim(tableData[2]).split(','))
    $('#drpMaterials').val(_id[0]);

    _selectedIndex = $(row).index();

}

function updateProductMaterial() {
    //alert(_selectedIndex);
    //if (validateCartValues()) {
    var model =
        {
            DetailDataID: parseInt($('#hdnid').val()),
            MaterialID: parseInt($('#drpMaterials').val()),
            OldMaterialID: parseInt($('#hdnMaterialID').val()),
            MaterialName: $("#drpMaterials option:selected").text(),
        };
    $.ajax({
        url: siteUrl + 'Product/UpdateProductMaterials',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            // alert($('#drpDepartment option:selected').text());

            //alert(isNaN(_id[0]));
            if (isNaN(_id[0]) && parseInt(data) > 0) {

                $('#tbProductMaterialDetail').append('<tr  onclick="rowMaterialsClick(this);"><td style="display:none;">' + $('#drpMaterials').val() + '</td><td>' + $('#drpMaterials option:selected').text() + '</td><td style="display:none;">' + data + '</td></tr>');
            }
            else {
                $('#tbProductMaterialDetail tr').eq(_selectedIndex + 1).find('td').eq(0).text($('#drpMaterials').val());
                $('#tbProductMaterialDetail tr').eq(_selectedIndex + 1).find('td').eq(1).text($('#drpMaterials option:selected').text());

            }
            if (parseInt(data) > 0) {
                alert(_saveMessage);
                resetProductMaterial();
            }
            else {
                alert(_errorMessage);
            }

        },
        error: function (_data) {
            alert(_errorMessage);
        }
    });
    //}
}
function resetProductMaterial() {
    _id = "";
    $('#drpMaterials').val(0);
    $("#BtnMaterialSave").text('Save');
    $('#hdnMaterialID').val('0');

}

function rowProductToFiltersClick(row) {
    $("#BtnProductToFiltersSave").text('Update');
    var tableData = $(row).children("td").map(function () {
        return $(this).text();
    }).get();

    _id = $.trim(tableData[0]).split(',');
    $('#hdnProductToFiltersID').val($.trim(tableData[2]).split(','))
    $('#drpProductToFilters').val(_id[0]);

    _selectedIndex = $(row).index();

}

function updateProductToFilters() {
    //alert(_selectedIndex);
    //if (validateCartValues()) {
    var model =
        {
            ProductID: parseInt($('#hdnid').val()),
            FilterID: parseInt($('#drpProductToFilters').val()),
            OldProductToFilterID: parseInt($('#hdnProductToFiltersID').val()),
        };
    $.ajax({
        url: siteUrl + 'Product/UpdateProductToFilters',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            // alert($('#drpDepartment option:selected').text());

            //alert(isNaN(_id[0]));
            if (isNaN(_id[0]) && parseInt(data) > 0) {

                $('#tbProductProductToFiltersDetail').append('<tr  onclick="rowProductToFiltersClick(this);"><td style="display:none;">' + $('#drpProductToFilters').val() + '</td><td>' + $('#drpProductToFilters option:selected').text() + '</td><td style="display:none;">' + data + '</td></tr>');
            }
            else {

                $('#tbProductProductToFiltersDetail tr').eq(_selectedIndex + 1).find('td').eq(0).text($('#drpProductToFilters').val());
                $('#tbProductProductToFiltersDetail tr').eq(_selectedIndex + 1).find('td').eq(1).text($('#drpProductToFilters option:selected').text());

            }
            if (parseInt(data) > 0) {
                alert(_saveMessage);
                resetProductToFilter();
            }
            else {
                alert(_errorMessage);
            }

        },
        error: function (_data) {
            alert(_errorMessage);
        }
    });
    //}
}
function resetProductToFilter() {
    _id = "";
    $('#drpProductToFilters').val(0);
    $("#BtnProductToFiltersSave").text('Save');
    $('#hdnProductToFiltersID').val('0');

}



