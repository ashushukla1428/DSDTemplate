var _lock = true;
var TempValues = { State1: '', State2: '', State3: '', State4: '', City1: '', City2: '', City3: '', City4: '' };
//$(document).keypress(function (e) {
//    if (e.which == 13) {
//        getList(1);
//    }
//});

$('#ddlActiveAllList').change(function () {

    getList(1);
});


function getList(pageNo) {
    _lock = true;
    $('#hdnPageNo').val(pageNo);
    var model = {
        Name: $('#txtSearchName').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder, ActiveOrAllCheck: $('#ddlActiveAllList').val()
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'ItemLocation/getList',
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

var _txtMainCity, _txtMainTelephoneSegment1NonUS, _txtmainContactTelephoneSegment1NonUS, _txtmainFaxSegment1NonUs, _mainContactFaxSegment1NonUs;
var _txtSpareCity, _txtSpareTelephoneSegment1NonUS, _txtSpareContactTelephoneSegment1NonUS, _txtSpareFaxSegment1NonUs, _SpareContactFaxSegment1NonUs;
var _txtStrapCity, _txtStrapTelephoneSegment1NonUS, _txtStrapContactTelephoneSegment1NonUS, _txtStrapFaxSegment1NonUs, _StrapContactFaxSegment1NonUs;
var _txtBillingCity, _txtBillingTelephoneSegment1NonUS, _txtBillingContactTelephoneSegment1NonUS, _txtBillingFaxSegment1NonUs, _BillingContactFaxSegment1NonUs;

function Add() {
    if (!validateItem()) {
        return;
    }
   //Main
   
        _txtMainTelephoneSegment1NonUS = $('#txtMainTelephoneSegment1NonUS').val();
        _txtmainContactTelephoneSegment1NonUS = $('#txtmainContactTelephoneSegment1NonUS').val();
        _txtmainFaxSegment1NonUs = $('#txtmainFaxSegment1NonUs').val();
        _mainContactFaxSegment1NonUs = $('#mainContactFaxSegment1NonUs').val();

        if ($("#txtMainCity").val() != '') {
            _txtMainCity = $("#txtMainCity").val();
        }
        else {
            _txtMainCity = $("#drpMainCites").val();
        }
   
    //Spare
   
        _txtSpareTelephoneSegment1NonUS = $('#txtsparePartsTelephoneSegment1NonUS').val();
        _txtSpareContactTelephoneSegment1NonUS = $('#txtsparePartsContactTelephoneSegment1NonUS').val();
        _txtSpareFaxSegment1NonUs = $('#txtsparePartsFaxSegment1NonUs').val();
        _SpareContactFaxSegment1NonUs = $('#sparePartsContactFaxSegment1NonUs').val();

        if ($("#txtsparePartsCity").val() != '') {
            _txtSpareCity = $("#txtsparePartsCity").val();
        }
        else {
            _txtSpareCity = $("#drpsparePartsPrimaryCites").val();
        }

    //Strap
  
        _txtStrapTelephoneSegment1NonUS = $('#txtStrapTelephoneSegment1NonUS').val();
        _txtStrapContactTelephoneSegment1NonUS = $('#txtStrapContactTelephoneSegment1NonUS').val();
        _txtStrapFaxSegment1NonUs = $('#txtStrapFaxSegment1NonUs').val();
        _StrapContactFaxSegment1NonUs = $('#StrapContactFaxSegment1NonUs').val();

        if ($("#txtStrapCity").val() != '') {
            _txtStrapCity = $("#txtStrapCity").val();
        }
        else {
            _txtStrapCity = $("#drpStrapPrimaryCites").val();
        }
   

    //Billing City
  
        _txtBillingTelephoneSegment1NonUS = $('#txtbillingTelephoneSegment1NonUS').val();
        _txtBillingContactTelephoneSegment1NonUS = $('#txtbillingContactTelephoneSegment1NonUS').val();
        _txtBillingFaxSegment1NonUs = $('#txtbillingFaxSegment1NonUs').val();
        _BillingContactFaxSegment1NonUs = $('#billingContactFaxSegment1NonUs').val();

        if ($("#txtNoNUSbillingCity").val() != '') {
            _txtBillingCity = $("#txtNoNUSbillingCity").val();
        }
        else {
            _txtBillingCity = $("#drpbillingPrimaryCites").val();
        }


    var model =
    {
        Id: parseInt($('#hdnid').val()),
        name: $('#txtItemName').val(),
        mainCompany: $('#txtMainCompany').val(),
        mainAddressLine1: $('#txtMainAddress1').val(),
        mainAddressLine2: $('#txtMainAddress2').val(),
        mainCity: _txtMainCity,//txtMainCity
        mainState: $('#drpMainState').val(),
        mainZipCode: $('#txtMainZipCode').val(),
        mainZipCodePlusFour: $('#txtMainZipCodePlusFour').val(),

        mainCountry: $('#drpMainCountry').val(),
        mainTelephoneSegment1: _txtMainTelephoneSegment1NonUS,//txtMainTelephoneSegment1NonUS
        mainTelephoneSegment2: $('#txtMainTelephoneSegment2').val(),
        mainTelephoneSegment3: $('#txtMainTelephoneSegment3').val(),


        mainTelephoneExtension: $('#txtMainTelephoneSegment1NonUSExtension').val(),


        mainFaxSegment1: _txtmainFaxSegment1NonUs,
        mainFaxSegment2: $('#txtmainFaxSegment1NonUsExtension').val(),
        mainFaxSegment3: $('#txtmainFaxSegment3').val(),


        mainEMailAddress: $('#txtmainEMailAddress').val(),
        mainStickerCode: $('#txtmainStickerCode').val(),
        mainWempeAccount: $('#txtmainWempeAccount').val(),
        mainContact: $('#txtmainContact').val(),

        mainContactTelephoneSegment1: _txtmainContactTelephoneSegment1NonUS,//txtmainContactTelephoneSegment1NonUS
        mainContactTelephoneSegment2: $('#txtmainContactTelephoneSegment2').val(),
        mainContactTelephoneSegment3: $('#txtmainContactTelephoneSegment3').val(),
        mainContactTelephoneExtension: $('#txtmainContactTelephoneSegment1NonUSExtension').val(),

        mainContactFaxSegment1: _mainContactFaxSegment1NonUs,
        mainContactFaxSegment2: $('#mainContactFaxSegment1NonUsExtension').val(),
        mainContactFaxSegment3: $('#txtmainContactFaxSegment3').val(),

        mainContactEMailAddress: $('#txtmainContactEMailAddress').val(),
        //Spare Parts -----------------------------------------------
        sparePartsCompany: $('#txtsparePartsCompany').val(),
        sparePartsAddressLine1: $('#txtsparePartsAddress1').val(),
        sparePartsAddressLine2: $('#txtsparePartsAddress2').val(),
        sparePartsCity: _txtSpareCity,//drpsparePartsPrimaryCites
        sparePartsState: $('#drpsparePartsState').val(),
        sparePartsZipCode: $('#txtsparePartsZipCode').val(),
        sparePartsZipCodePlusFour: $('#txtsparePartsZipCodePlusFour').val(),

        sparePartsCountry: $('#drpSPARECountry').val(),
        sparePartsTelephoneSegment1: _txtSpareTelephoneSegment1NonUS,//txtsparePartsTelephoneSegment1NonUS
        sparePartsTelephoneSegment2: $('#txtsparePartsTelephoneSegment2').val(),
        sparePartsTelephoneSegment3: $('#txtsparePartsTelephoneSegment3').val(),
        sparePartsTelephoneExtension: $('#txtsparePartsTelephoneSegment1NonUSExtension').val(),
        sparePartsFaxSegment1: _txtSpareFaxSegment1NonUs,//txtsparePartsFaxSegment1NonUs
        sparePartsFaxSegment2: $('#txtsparePartsFaxSegment1NonUs').val(),
        sparePartsFaxSegment3: $('#txtsparePartsFaxSegment3').val(),

        sparePartsEMailAddress: $('#txtsparePartsEMailAddress').val(),
        sparePartsStickerCode: $('#txtsparePartsStickerCode').val(),
        sparePartsWempeAccount: $('#txtsparePartsWempeAccount').val(),
        sparePartsContact: $('#txtsparePartsContact').val(),

        sparePartsContactTelephoneSegment1: _txtSpareContactTelephoneSegment1NonUS,//txtsparePartsContactTelephoneSegment1NonUS
        sparePartsContactTelephoneSegment2: $('#txtsparePartsContactTelephoneSegment2').val(),
        sparePartsContactTelephoneSegment3: $('#txtsparePartsContactTelephoneSegment3').val(),
        sparePartsContactTelephoneExtension: $('#txtsparePartsContactTelephoneSegment1NonUSExtension').val(),

        sparePartsContactFaxSegment1: _SpareContactFaxSegment1NonUs,//sparePartsContactFaxSegment1NonUs
        sparePartsContactFaxSegment2: $('#sparePartsContactFaxSegment1NonUsExtension').val(),
        sparePartsContactFaxSegment3: $('#txtsparePartsContactFaxSegment3').val(),
        sparePartsContactEMailAddress: $('#txtsparePartsContactEMailAddress').val(),
        //strap detail---------------------------------------------------------------
        strapCompany: $('#txtStrapCompany').val(),
        strapAddressLine1: $('#txtStrapAddress1').val(),
        strapAddressLine2: $('#txtStrapAddress2').val(),
        strapCity: _txtStrapCity,//txtStrapCity
        strapState: $('#drpStrapState').val(),
        strapZipCode: $('#txtStrapZipCode').val(),
        strapZipCodePlusFour: $('#txtStrapZipCodePlusFour').val(),

        strapCountry: $('#drpStrapCountry').val(),
        strapTelephoneSegment1: _txtStrapTelephoneSegment1NonUS,//txtStrapTelephoneSegment1NonUS
        strapTelephoneSegment2: $('#txtStrapTelephoneSegment2').val(),
        strapTelephoneSegment3: $('#txtStrapTelephoneSegment3').val(),
        strapTelephoneExtension: $('#txtStrapTelephoneSegment1NonUSExtension').val(),
        strapFaxSegment1: _txtStrapFaxSegment1NonUs,//txtStrapFaxSegment1NonUs
        strapFaxSegment2: $('#txtStrapFaxSegment1NonUsExtension').val(),
        strapFaxSegment3: $('#txtStrapFaxSegment3').val(),
        strapEMailAddress: $('#txtStrapEMailAddress').val(),

        strapStickerCode: $('#txtStrapStickerCode').val(),
        strapWempeAccount: $('#txtStrapWempeAccount').val(),
        sparePartsContact: $('#txtstrapContact').val(),

        strapContactTelephoneSegment1: _txtStrapContactTelephoneSegment1NonUS,//txtStrapContactTelephoneSegment1NonUS
        strapContactTelephoneSegment2: $('#txtStrapContactTelephoneSegment2').val(),
        strapContactTelephoneSegment3: $('#txtStrapContactTelephoneSegment3').val(),
        strapContactTelephoneExtension: $('#txtStrapContactTelephoneSegment1NonUSExtension').val(),

        strapContactFaxSegment1: _StrapContactFaxSegment1NonUs,//StrapContactFaxSegment1NonUs
        strapContactFaxSegment2: $('#StrapContactFaxSegment1NonUsExtension').val(),
        strapContactFaxSegment3: $('#txtStrapContactFaxSegment3').val(),

        strapContactEMailAddress: $('#txtstrapContactEMailAddress').val(),//txtStrapContactEMailAddress
        
        //billing Parts 
        billingCompany: $('#txtBillingCompany').val(),
        billingAddressLine1: $('#txtbillingAddress1').val(),
        billingAddressLine2: $('#txtbillingAddress2').val(),
        billingCity: _txtBillingCity,//txtNoNUSbillingCity
        billingState: $('#drpbillingState').val(),
        billingZipCode: $('#txtbillingZipCode').val(),
        billingZipCodePlusFour: $('#txtbillingZipCodePlusFour').val(),

        billingCountry: $('#drpBILLINGCountry').val(),
        billingTelephoneSegment1: _txtBillingTelephoneSegment1NonUS,//txtbillingTelephoneSegment1NonUS
        billingTelephoneSegment2: $('#txtbillingTelephoneSegment2').val(),
        billingTelephoneSegment3: $('#txtbillingTelephoneSegment3').val(),
        billingTelephoneExtension: $('#txtbillingTelephoneSegment1NonUSExtension').val(),

        billingFaxSegment1: _txtBillingFaxSegment1NonUs,//txtbillingFaxSegment1NonUs
        billingFaxSegment2: $('#txtbillingFaxSegment1NonUsExtension').val(),
        billingFaxSegment3: $('#txtbillingFaxSegment3').val(),

        billingEMailAddress: $('#txtbillingEMailAddress').val(),
        billingStickerCode: $('#txtbillingStickerCode').val(),
        billingWempeAccount: $('#txtbillingWempeAccount').val(),
        billingContact: $('#txtbillingContact').val(),

        billingContactTelephoneSegment1: _txtBillingContactTelephoneSegment1NonUS,//txtbillingContactTelephoneSegment1NonUS
        billingContactTelephoneSegment2: $('#txtbillingContactTelephoneSegment2').val(),
        billingContactTelephoneSegment3: $('#txtbillingContactTelephoneSegment3').val(),
        billingContactTelephoneExtension: $('#txtbillingContactTelephoneSegment1NonUSExtension').val(),

        billingContactFaxSegment1: _BillingContactFaxSegment1NonUs,
        billingContactFaxSegment2: $('#billingContactFaxSegment1NonUsExtension').val(),
        billingContactFaxSegment3: $('#txtbillingContactFaxSegment3').val(),
        billingContactEMailAddress: $('#txtbillingContactEMailAddress').val(),

        notes: $('#txtnotes').val(),


        IsActive: $('#chkIsActive').is(':checked')
    };


    $.ajax({
        url: siteUrl + 'ItemLocation/Add',
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
    //  resetControl();
    $('#hdnid').val(id);
    if (id == 0) {
       
        TempValues.State1 = '';
        TempValues.City1 = '';

        TempValues.State2 = '';
        TempValues.City2 = '';

        TempValues.State3 = '';
        TempValues.City3 = '';

        TempValues.State4 = '';
        TempValues.City4 = '';



        $('#txtMainTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtmainContactTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtmainFaxSegment1NonUs').mask("(999) 999-9999");
        $('#mainContactFaxSegment1NonUs').mask("(999) 999-9999");

        $('#txtsparePartsTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtsparePartsContactTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtsparePartsFaxSegment1NonUs').mask("(999) 999-9999");
        $('#sparePartsContactFaxSegment1NonUs').mask("(999) 999-9999");


        $('#txtbillingTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtbillingContactTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtbillingFaxSegment1NonUs').mask("(999) 999-9999");
        $('#billingContactFaxSegment1NonUs').mask("(999) 999-9999");


        $('#txtStrapTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtStrapContactTelephoneSegment1NonUS').mask("(999) 999-9999");
        $('#txtStrapFaxSegment1NonUs').mask("(999) 999-9999");
        $('#StrapContactFaxSegment1NonUs').mask("(999) 999-9999");



        var $modal = $('#responsive');
        $modal.modal();
        return;
    }

    jQuery.ajax({
        url: siteUrl + 'ItemLocation/Edit',
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
            $('#txtItemName').val(data.name);
           
            TempValues.State1 = data.mainState;
            TempValues.City1 = data.mainCity;

            TempValues.State2 = data.sparePartsState;
            TempValues.City2 = data.sparePartsCity;

            TempValues.State3 = data.billingState;
            TempValues.City3 = data.billingCity;

            TempValues.State4 = data.strapState;
            TempValues.City4 = data.strapCity;
          
            $("#drpMainCountry option:contains(" + data.mainCountry + ")").attr('selected', 'selected');
            $("#drpMainCountry").trigger('change');

            $("#drpSPARECountry option:contains(" + data.sparePartsCountry + ")").attr('selected', 'selected');
            $("#drpSPARECountry").trigger('change');

            $("#drpBILLINGCountry option:contains(" + data.billingCountry + ")").attr('selected', 'selected');
            $("#drpBILLINGCountry").trigger('change');
         
            $("#drpStrapCountry option:contains(" + data.strapCountry + ")").attr('selected', 'selected');
            $("#drpStrapCountry").trigger('change');

            
            $('#txtItemName').val(data.name);
            $('#txtMainCompany').val(data.mainCompany);
            $('#txtMainAddress1').val(data.mainAddressLine1);
            $('#txtMainAddress2').val(data.mainAddressLine2);
            $('#drpMainCites').val(data.mainCity),//txtMainCity
            $('#drpMainState').val(data.mainState);
            $('#txtMainZipCode').val(data.mainZipCode);
            $('#txtMainZipCodePlusFour').val(data.mainZipCodePlusFour);

            $('#drpMainCountry').val(data.mainCountry);



            $('#txtMainTelephoneSegment1NonUS').val(data.mainTelephoneSegment1);//txtMainTelephoneSegment1NonUS


            $('#txtMainTelephoneSegment1NonUSExtension').val(data.mainTelephoneExtension);


        
            $('#txtmainFaxSegment1NonUs').val(data.mainFaxSegment1);


            $('#txtmainFaxSegment1NonUsExtension').val(data.mainFaxSegment2);
         




            $('#txtmainEMailAddress').val(data.mainEMailAddress);
            $('#txtmainStickerCode').val(data.mainStickerCode);
            $('#txtmainWempeAccount').val(data.mainWempeAccount);
            $('#txtmainContact').val(data.mainContact);

            $('#txtmainContactTelephoneSegment1NonUS').val(data.mainContactTelephoneSegment1);//txtmainContactTelephoneSegment1NonUS
            $('#txtmainContactTelephoneSegment1NonUSExtension').val(data.mainContactTelephoneExtension);

            $('#mainContactFaxSegment1NonUs').val(data.mainContactFaxSegment1);

            $('#mainContactFaxSegment1NonUsExtension').val(data.mainContactFaxSegment2);
            


            $('#txtmainContactEMailAddress').val(data.mainContactEMailAddress);
            //Spare Parts -----------------------------------------------
            $('#txtsparePartsCompany').val(data.sparePartsCompany);
            $('#txtsparePartsAddress1').val(data.sparePartsAddressLine1);
            $('#txtsparePartsAddress2').val(data.sparePartsAddressLine2);
            $('#txtsparePartsCity').val(data.sparePartsCity);//drpsparePartsPrimaryCites
            $('#drpsparePartsState').val(data.sparePartsState);
            $('#txtsparePartsZipCode').val(data.sparePartsZipCode);
            $('#txtsparePartsZipCodePlusFour').val(data.sparePartsZipCodePlusFour);

            $('#drpSPARECountry').val(data.sparePartsCountry);

            $('#txtsparePartsTelephoneSegment1NonUS').val(data.sparePartsTelephoneSegment1);//txtsparePartsTelephoneSegment1NonUS
          
            $('#txtsparePartsTelephoneSegment1NonUSExtension').val(data.sparePartsTelephoneExtension);

            $('#txtsparePartsFaxSegment1NonUs').val(data.sparePartsFaxSegment1);//txtsparePartsFaxSegment1NonUs
            $('#txtsparePartsFaxSegment1NonUsExtension').val(data.sparePartsFaxSegment2);


            $('#txtsparePartsEMailAddress').val(data.sparePartsEMailAddress);
            $('#txtsparePartsStickerCode').val(data.sparePartsStickerCode);
            $('#txtsparePartsWempeAccount').val(data.sparePartsWempeAccount);
            $('#txtsparePartsContact').val(data.sparePartsContact);

            $('#txtsparePartsContactTelephoneSegment1NonUS').val(data.sparePartsContactTelephoneSegment1);//txtsparePartsContactTelephoneSegment1NonUS
          
            $('#txtsparePartsContactTelephoneSegment1NonUSExtension').val(data.sparePartsContactTelephoneExtension);

            $('#sparePartsContactFaxSegment1NonUs').val(data.sparePartsContactFaxSegment1);//sparePartsContactFaxSegment1NonUs
            $('#sparePartsContactFaxSegment1NonUsExtension').val(data.sparePartsContactFaxSegment2);


            $('#txtsparePartsContactEMailAddress').val(data.sparePartsContactEMailAddress);
            //strap detail---------------------------------------------------------------
            $('#txtStrapCompany').val(data.strapCompany);
            $('#txtStrapAddress1').val(data.strapAddressLine1);
            $('#txtStrapAddress2').val(data.strapAddressLine2);
            $('#drpStrapPrimaryCites').val(data.strapCity);//txtStrapCity
            $('#drpStrapState').val(data.strapState);
            $('#txtStrapZipCode').val(data.strapZipCode);
            $('#txtStrapZipCodePlusFour').val(data.strapZipCodePlusFour);

            $('#drpStrapCountry').val(data.strapCountry);
            $('#txtStrapTelephoneSegment1NonUS').val(data.strapTelephoneSegment1);//txtStrapTelephoneSegment1NonUS

            $('#txtStrapTelephoneSegment1NonUSExtension').val(data.strapTelephoneExtension);

            //$('#txtStrapTelephoneSegment2').val(data.strapTelephoneSegment2);
            //$('#txtStrapTelephoneSegment3').val(data.strapTelephoneSegment3);
            //$('#txtStrapTelephoneExtension').val(data.strapTelephoneExtension);


            $('#txtStrapFaxSegment1NonUs').val(data.strapFaxSegment1);//txtStrapFaxSegment1NonUs
        
            $('#txtStrapFaxSegment1NonUsExtension').val(data.strapFaxSegment2);

            $('#txtStrapEMailAddress').val(data.strapEMailAddress);

            $('#txtStrapStickerCode').val(data.strapStickerCode);
            $('#txtStrapWempeAccount').val(data.strapWempeAccount);
            $('#txtstrapContact').val(data.sparePartsContact);

            $('#txtStrapContactTelephoneSegment1NonUS').val(data.strapContactTelephoneSegment1);//txtStrapContactTelephoneSegment1NonUS
       
            $('#txtStrapContactTelephoneSegment1NonUSExtension').val(data.strapContactTelephoneExtension);


            $('#StrapContactFaxSegment1NonUs').val(data.strapContactFaxSegment1);//StrapContactFaxSegment1NonUs
          
            $('#StrapContactFaxSegment1NonUsExtension').val(data.strapContactFaxSegment2);

            $('#txtstrapContactEMailAddress').val(data.strapContactEMailAddress);//txtStrapContactEMailAddress
            
            //billing Parts 
            $('#txtBillingCompany').val(data.billingCompany);
            $('#txtbillingAddress1').val(data.billingAddressLine1);
            $('#txtbillingAddress2').val(data.billingAddressLine2);
            $('#drpbillingPrimaryCites').val(data.billingCity);//txtNoNUSbillingCity
            $('#drpbillingState').val(data.billingState);
            $('#txtbillingZipCode').val(data.billingZipCode);
            $('#txtbillingZipCodePlusFour').val(data.billingZipCodePlusFour);

            $('#drpBILLINGCountry').val(data.billingCountry);

            $('#txtbillingTelephoneSegment1NonUS').val(data.billingTelephoneSegment1);//txtbillingTelephoneSegment1NonUS
          
            $('#txtbillingTelephoneSegment1NonUSExtension').val(data.billingTelephoneExtension);

            $('#txtbillingFaxSegment1NonUs').val(data.billingFaxSegment1);//txtbillingFaxSegment1NonUs
           

            $('#txtbillingFaxSegment1NonUsExtension').val(data.billingFaxSegment2);

            $('#txtbillingEMailAddress').val(data.billingEMailAddress);
            $('#txtbillingStickerCode').val(data.billingStickerCode);
            $('#txtbillingWempeAccount').val(data.billingWempeAccount);
            $('#txtbillingContact').val(data.billingContact);

            $('#txtbillingContactTelephoneSegment1NonUS').val(data.billingContactTelephoneSegment1);//txtbillingContactTelephoneSegment1NonUS
        
            $('#txtbillingContactTelephoneSegment1NonUSExtension').val(data.billingContactTelephoneExtension);


            $('#billingContactFaxSegment1NonUs').val(data.billingContactFaxSegment1);

            $('#billingContactFaxSegment1NonUsExtension').val(data.billingContactFaxSegment2);



          
            $('#txtbillingContactEMailAddress').val(data.billingContactEMailAddress);

            $('#txtnotes').val(data.notes);


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

$(document).ready(function () {

    $('#drpMainCountry').change(function () {
        
        if ($("#drpMainCountry option:selected").text() == "United States") {


            $('#txtMainTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtmainContactTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtmainFaxSegment1NonUs').mask("(999) 999-9999");
            $('#mainContactFaxSegment1NonUs').mask("(999) 999-9999");

            
          
            $('#txtMainCity').hide();
         
        }
        else {
            $('#txtMainTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtmainContactTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtmainFaxSegment1NonUs').unmask("(999) 999-9999");
            $('#mainContactFaxSegment1NonUs').unmask("(999) 999-9999");

            $('#txtMainCity').show();
        }
      

        bindStates($("#drpMainState"), $('#drpMainCountry').val());

    });
    $('#drpSPARECountry').change(function () {

        if ($("#drpSPARECountry option:selected").text() == "United States") {

            $('#txtsparePartsTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtsparePartsContactTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtsparePartsFaxSegment1NonUs').mask("(999) 999-9999");
            $('#sparePartsContactFaxSegment1NonUs').mask("(999) 999-9999");

            $('#txtsparePartsCity').hide();
            
        }
        else {
          
            $('#txtsparePartsTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtsparePartsContactTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtsparePartsFaxSegment1NonUs').unmask("(999) 999-9999");
            $('#sparePartsContactFaxSegment1NonUs').unmask("(999) 999-9999");


            $('#txtsparePartsCity').show();
        }

        bindStates($("#drpsparePartsState"), $('#drpSPARECountry').val());
    });
    $('#drpBILLINGCountry').change(function () {

        if ($("#drpBILLINGCountry option:selected").text() == "United States") {

         

            $('#txtbillingTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtbillingContactTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtbillingFaxSegment1NonUs').mask("(999) 999-9999");
            $('#billingContactFaxSegment1NonUs').mask("(999) 999-9999");



            $('#txtbillingCity').hide();
        }
        else {
          
            $('#txtbillingTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtbillingContactTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtbillingFaxSegment1NonUs').unmask("(999) 999-9999");
            $('#billingContactFaxSegment1NonUs').unmask("(999) 999-9999");


            $('#txtbillingCity').show();
        }

        bindStates($("#drpbillingState"), $('#drpBILLINGCountry').val());
    });

    $('#drpStrapCountry').change(function () {

        if ($("#drpStrapCountry option:selected").text() == "United States") {

         

            $('#txtStrapTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtStrapContactTelephoneSegment1NonUS').mask("(999) 999-9999");
            $('#txtStrapFaxSegment1NonUs').mask("(999) 999-9999");
            $('#StrapContactFaxSegment1NonUs').mask("(999) 999-9999");


            $('#txtStrapCity').hide();
        }
        else {
          

            $('#txtStrapTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtStrapContactTelephoneSegment1NonUS').unmask("(999) 999-9999");
            $('#txtStrapFaxSegment1NonUs').unmask("(999) 999-9999");
            $('#StrapContactFaxSegment1NonUs').unmask("(999) 999-9999");


            $('#txtStrapCity').show();
        }

        bindStates($("#drpStrapState"), $('#drpStrapCountry').val());
    });

    //states drp events
    $('#drpMainState').change(function () {
       
        bindCities($("#drpMainCites"), $('#drpMainState').val());
    });
    $('#drpsparePartsState').change(function () {
        bindCities($("#drpsparePartsPrimaryCites"), $('#drpsparePartsState').val());
    });
    $('#drpbillingState').change(function () {
        bindCities($("#drpbillingPrimaryCites"), $('#drpbillingState').val());
    });

    $('#drpStrapState').change(function () {
        bindCities($("#drpStrapPrimaryCites"), $('#drpStrapState').val());
    });
    function bindStates(drpSates, id)
    {
        if (id == '0') {
            return;
        }
        drpSates.empty().append('<option selected="selected" value="">Loading....</option>');
        jQuery.ajax({
            url: siteUrl + 'ItemLocation/getStates',
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
                    drpSates.append($("<option></option>").val(this['state']).html(this['stateFullName']));
                });
                
                if (TempValues.State1 != '' && drpSates.selector == '#drpMainState') {
                    drpSates.val(TempValues.State1);
                    drpSates.change();
                }
                if (TempValues.State2 != '' && drpSates.selector == '#drpsparePartsState') {
                    drpSates.val(TempValues.State2);
                    drpSates.change();
                }
                if (TempValues.State3 != '' && drpSates.selector == '#drpbillingState') {
                    drpSates.val(TempValues.State3);
                    drpSates.change();
                }
                if (TempValues.State4 != '' && drpSates.selector == '#drpStrapState') {
                    drpSates.val(TempValues.State4);
                    drpSates.change();
                }

            },
            error: function (data) {
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
            }
        });
    }

    function bindCities(drpCities, id) {
        if (id == '')
        {
            return;
        }
        drpCities.empty().append('<option selected="selected" value="">Loading....</option>');
        jQuery.ajax({
            url: siteUrl + 'ItemLocation/getCities',
            dataType: 'json',
            headers: { "requestType": "client" },
            data: { "Id": id },
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                if (data.Status == false) {
                    showMessage('Oops', 'error', data.Message, 'toast-top-right');
                    return;
                }
                drpCities.empty().append('<option selected="selected" value="">Please select</option>');
                $.each(data, function () {
                    //drpCities.append($("<option></option>").val(this['Id']).html(this['city']));
                    drpCities.append($("<option></option>").val(this['city']).html(this['city']));
                });

                if (TempValues.City1 != '' && drpCities.selector == '#drpMainCites') {
                    drpCities.val(TempValues.City1);
                }
                if (TempValues.City2 != '' && drpCities.selector == '#drpsparePartsPrimaryCites') {
                    drpCities.val(TempValues.City2);
                }
                if (TempValues.City3 != '' && drpCities.selector == '#drpbillingPrimaryCites') {
                    drpCities.val(TempValues.City3);
                }
                if (TempValues.City4 != '' && drpCities.selector == '#drpStrapPrimaryCites') {
                    drpCities.val(TempValues.City4);
                }

            },
            error: function (data) {
                drpCities.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
            }
        });
    }
});