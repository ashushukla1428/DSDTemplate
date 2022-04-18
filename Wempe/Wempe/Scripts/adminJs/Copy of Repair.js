var _lock = true;

var TempValues = { PriState: '', PriCity: '', AltState: '', AltCity: '' };



//------------------------ Customer Tab-----------------------------------//
function addCustomer() {
    //alert(siteUrl);
   // alert($('#txtPhone1').val() != '' ? $('#txtPhone1').val() : $('#txtPhone1NonUs').val());
    if (!validateCustomer()) {
        return;
    }
  
    var cookieCustomerNumber = readCookie('cookieCustomerNumber');
    //alert(cookieCustomerNumber);
    var model =
       {
           customerNumber: cookieCustomerNumber,
           title: $("#drpTitels1").val(),
           firstName: $('#txtFirstName').val(),
           middleInitial: $('#txtMiddle').val(),
           lastName: $('#txtLast').val(),

           asstTitle: $("#drpTitels2").val(),
           asstFirstName: $('#txtFirstName1').val(),
           asstMiddleInitial: $('#txtMiddle1').val(),
           asstLastName: $('#txtLast1').val(),
           asstRelation: $('#ddlAssistantRelation').val(),


           SpouseTitle: $("#drpTitelsSpouse").val(),
           SpousefirstName: $('#txtFirstNameSpouse').val(),
           SpousemiddleInitial: $('#txtMiddleSpouse').val(),
           SpouselastName: $('#txtLastSpouse').val(),
           SpouseRelation: $('#ddlSpouseRelation').val(),
          
           priCompany: $('#txtCompany').val(),
           priAddressLine1: $('#txtLine1').val(),
           priAddressLine2: $('#txtLine2').val(),
           priCity: $("#drpPrimaryCites option:selected").text(),
           priState: $("#drpPrimaryStates").val(),
           priZipCode: $('#txtStateZip').val(),
           priZipCodePlusFour: $('#txtStateZip1').val(),
           priCountry: $("#drpPrimaryCountry").val(),

           //priTelephoneSegment1: $('.txtPhone1:visible').val(),
           priTelephoneSegment1: $('#txtPhone1').val() != '' ? $('#txtPhone1').val() : $('#txtPhone1NonUs').val(),
           priTelephoneSegment2: $('#txtPhone2').val(),
           priTelephoneSegment3: $('#txtPhone3').val(),
           priTelephoneExtension: $('#txtPhone4').val(),

           priHomephoneSegment1: $('#txtHomePhone1').val(),
           //priTelephoneSegment1: $('#txtHomePhone1').val() != '' ? $('#txtHomePhone1').val() : $('#txtPhone1NonUs').val(),
           priHomephoneSegment2: $('#txtHOMEPhone2').val(),
           priHomephoneSegment3: $('#txtHomePhone3').val(),

           priAssistanatphoneSegment1: $('#txtASSISTANTPhone1').val(),
           //priAssistanatphoneSegment1: $('#txtASSISTANTPhone1').val() != '' ? $('#txtPhone1').val() : $('#txtPhone1NonUs').val(),
           priAssistanatSegment2: $('#txtPhone2').val(),
           priAssistanatSegment3: $('#txtPhone3').val(),
           priAssistanatExtension: $('#txtPhone4').val(),

           priCellphoneSegment1: $('.txtCall1:visible').val(),
           priCellphoneSegment2: $('#txtCall2').val(),
           priCellphoneSegment3: $('#txtCall3').val(),

           priFaxSegment1: $('.txtFAX1:visible').val(),
           priFaxSegment2: $('#txtFAX2').val(),
           priFaxSegment3: $('#txtFAX3').val(),

           priEMailAddress: $('#txtEmail').val(),
           priEmailAddress2: $('#txtEmail2').val(),
           //ALTERNATE ADDRESS
           secCompany: $('#txtAltCompany').val(),
           secAddressLine1: $('#txtAltLine1').val(),
           secAddressLine2: $('#txtAltLine2').val(),
           secCity: $("#drpAltCites option:selected").text(),
           secState: $("#drpAltStates").val(),
           secZipCode: $('#txtAltStateZip').val(),
           secZipCodePlusFour: $('#txtAltStateZip1').val(),
           secCountry: $("#drpAltCountry").val(),

           secTelephoneSegment1: $('.txtAltPhone1:visible').val(),
           secTelephoneSegment2: $('#txtAltPhone2').val(),
           secTelephoneSegment3: $('#txtAltPhone3').val(),
           secTelephoneExtension: $('#txtAltPhone4').val(),

           secHomephoneSegment1: $('.txtPhone1:visible').val(),
           secHomephoneSegment2: $('#txtPhone2').val(),
           secHomephoneSegment3: $('#txtPhone3').val(),

           secAssistanatSegment1: $('.txtPhone1:visible').val(),
           secAssistanatSegment2: $('#txtPhone2').val(),
           secAssistanatSegment3: $('#txtPhone3').val(),
           secAssistanatExtension: $('#txtPhone4').val(),


           secCellphoneSegment1: $('.txtAltCall1:visible').val(),
           secCellphoneSegment2: $('#txtAltCall2').val(),
           secCellphoneSegment3: $('#txtAltCall3').val(),

           secFaxSegment1: $('.txtAltFAX1:visible').val(),
           secFaxSegment2: $('#txtAltFAX2').val(),
           secFaxSegment3: $('#txtAltFAX3').val(),
           secEMailAddress: $('#txtAltEmail').val(),
           secEMailAddress2: $('#txtAltEmail').val(),
           // Third Address

           thirdCompany: $('#txtthirdCompany').val(),
           thirdAddressLine1: $('#txtthirdLine1').val(),
           thirdAddressLine2: $('#txtthirdLine2').val(),
           thirdCity: $("#drpthirdCites option:selected").text(),
           thirdState: $("#drpthirdStates").val(),
           thirdZipCode: $('#txtthirdStateZip').val(),
           thirdZipCodePlusFour: $('#txtthirdStateZip1').val(),
           thirdCountry: $("#drpthirdCountry").val(),

           thirdTelephoneSegment1: $('.txtthirdPhone1:visible').val(),
          // thirdTelephoneSegment1: $('#txtthirdPhone1').val() != '' ? $('#txtthirdPhone1').val() : $('#txtPhone1NonUs').val(),
           thirdTelephoneSegment2: $('#txtthirdPhone2').val(),
           thirdTelephoneSegment3: $('#txtthirdPhone3').val(),
           thirdTelephoneExtension: $('#txtthirdPhone4').val(),

           thirdHomephoneSegment1: $('.txtthirdPhone1:visible').val(),
           thirdHomephoneSegment2: $('#txtthirdPhone2').val(),
           thirdHomephoneSegment3: $('#txtthirdPhone3').val(),

           thirdAssistanatphoneSegment1: $('.txtthirdPhone1:visible').val(),
           thirdAssistanatSegment2: $('#txtthirdPhone2').val(),
           thirdAssistanatSegment3: $('#txtthirdPhone3').val(),
           thirdAssistanatExtension: $('#txtthirdPhone4').val(),

           thirdCellphoneSegment1: $('.txtthirdCall1:visible').val(),
           thirdCellphoneSegment2: $('#txtthirdCall2').val(),
           thirdCellphoneSegment3: $('#txtthirdCall3').val(),

           thirdFaxSegment1: $('.txtthirdFAX1:visible').val(),
           thirdFaxSegment2: $('#txtthirdFAX2').val(),
           thirdFaxSegment3: $('#txtthirdFAX3').val(),

           thirdEMailAddress: $('#txtthirdEmail').val(),
           thirdEmailAddress2: $('#txtthirdEmail2').val(),

           //PRIMARY CREDIT CARD
           priCreditCardSegment1: $('#txtCreditCard1').val(),
           priCreditCardSegment2: $('#txtCreditCard2').val(),
           priCreditCardSegment3: $('#txtCreditCard3').val(),
           priCreditCardSegment4: $('#txtCreditCard4').val(),
           priCreditCardExpMonth: $('#txtCardExpDay').val(),
           priCreditCardExpYear: $('#txtCardExpYear').val(),
           //ALTERNATE ADDRESS
           secCreditCardSegment1: $('#txtAltCreditCard1').val(),
           secCreditCardSegment2: $('#txtAltCreditCard2').val(),
           secCreditCardSegment3: $('#txtAltCreditCard3').val(),
           secCreditCardSegment4: $('#txtAltCreditCard4').val(),
           secCreditCardExpMonth: $('#txtAltCardExpDay').val(),
           secCreditCardExpYear: $('#txtAltCardExpYear').val(),
           // third

           //ALTERNATE ADDRESS
           thirdCreditCardSegment1: $('#txtthirdCreditCard1').val(),
           thirdCreditCardSegment2: $('#txtthirdCreditCard2').val(),
           thirdCreditCardSegment3: $('#txtthirdCreditCard3').val(),
           thirdCreditCardSegment4: $('#txtthirdCreditCard4').val(),
           thirdCreditCardExpMonth: $('#txtthirdCardExpDay').val(),
           thirdCreditCardExpYear: $('#txtthirdCardExpYear').val(),

           //NOTE
           contactPreference: $("#drpContactPrefrences").val(),
           customerType: $("#drpCustomerType").val(),
           notes: $('#txtNote').val()
       };

    // alert(JSON.stringify(model));
    $.ajax({
        url: siteUrl + 'Repair/AddCustomer',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            //alert(JSON.stringify(data));
            if (data.Status) {
             //   resetControl();
                //  $('#responsive').modal('toggle');
                createCookie('cookieCustomerNumber', data.Message, 2);
                showMessage('Success', 'success', "customer added successfully!", 'toast-top-right');
              //  getList(1);
            }
            else {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
            //alert(data);

        },
        error: function (_data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
            //  alert(_errorMessage);
        }
    });
}

function getCustomer(id) {
    //  alert(id);
    //resetControl();
  
    if (id == 0)
    {
        return;
    }
    jQuery.ajax({
        url: siteUrl + 'Repair/getCustomer',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        async: false,
        success: function (_data) {
            //
           // alert(JSON.stringify(data));
            if (_data.Status == false) {
                showMessage('Oops', 'error', _data.Message, 'toast-top-right');
                return;
            }
       
            // primary country, state, city
         
            //$("#drpPrimaryCountry option:contains(" + _data.priCountry + ")").attr('selected', true).trigger('change');
            //$("#drpAltCountry option:contains(" + _data.secCountry + ")").attr('selected', true).trigger('change');
            $("#drpPrimaryCountry").val(_data.priCountry).trigger('change');
            $("#drpAltCountry").val(_data.secCountry).trigger('change');
            TempValues.PriState = _data.priState;
            TempValues.PriCity = _data.priCity;
            TempValues.AltState = _data.secState;
            TempValues.AltCity = _data.secCity;
        

            $('#drpTitelsSpouse').val(_data.SpouseTitle);
            $('#txtFirstNameSpouse').val(_data.SpousefirstName);
            $('#txtMiddleSpouse').val(_data.SpousemiddleInitial);
            $('#txtLastSpouse').val(_data.SpouselastName);
            $('#ddlSpouseRelation').val(_data.SpouseRelation);

            $('#txtFirstName').val(_data.firstName);
            $('#txtMiddle').val(_data.middleInitial);
            $('#txtLast').val(_data.lastName);
            
             //$("#drpTitels2 option:selected").text(),
            $('#txtFirstName1').val(_data.asstFirstName);
            $('#txtMiddle1').val(_data.asstMiddleInitial);
            $('#txtLast1').val(_data.asstLastName);

            $('#ddlAssistantRelation').val(_data.asstRelation);

          
          
            $('#txtCompany').val(_data.priCompany);
            $('#txtLine1').val(_data.priAddressLine1);
            $('#txtLine2').val(_data.priAddressLine2);
            //priCity: $("#drpPrimaryCites option:selected").text(),
            //priState: $("#drpPrimaryStates option:selected").text(),
            $('#txtStateZip').val(_data.priZipCode);
            $('#txtStateZip1').val(_data.priZipCodePlusFour);
            //priCountry: $("#drpPrimaryCountry option:selected").text(),
            if ($("#drpPrimaryCountry option:selected").text() == "United States") {
                $('#txtPhone1').val(_data.priTelephoneSegment1);
                $('#txtPhone2').val(_data.priTelephoneSegment2);
                $('#txtPhone3').val(_data.priTelephoneSegment3);
                $('#txtPhone4').val(_data.priTelephoneExtension);
            }
            else {
                $('#txtPhone1NonUs').val(_data.priTelephoneSegment1);
            }


            $('.txtHomePhone1:visible').val(_data.priHomephoneSegment1);
            $('#txtHomePhone1').val(_data.priHomephoneSegment1);
            $('#txtHomePhone2').val(_data.priHomephoneSegment2);
            $('#txtHomePhone3').val(_data.priHomephoneSegment3);

            $('#txtASSISTANTPhone1').val(_data.priAssistanatSegment1);
            $('#txtASSISTANTPhone2').val(_data.priAssistanatSegment2);
            $('#txtASSISTANTPhone3').val(_data.priAssistanatSegment3);
            $('#txtASSISTANTPhone4').val(_data.priAssistanatExtension);

            $('.txtCall1:visible').val(_data.priCellphoneSegment1);
            $('#txtCall2').val(_data.priCellphoneSegment2);
            $('#txtCall3').val(_data.priCellphoneSegment3);

            $('.txtFAX1:visible').val(_data.priFaxSegment1);
            $('#txtFAX2').val(_data.priFaxSegment2);
            $('#txtFAX3').val(_data.priFaxSegment3);

            $('#txtEmail').val(_data.priEMailAddress);
            $('#txtEmail2').val(_data.priEMailAddress2);
            //ALTERNATE ADDRESS
            $('#txtAltCompany').val(_data.secCompany);
            $('#txtAltLine1').val(_data.secAddressLine1);
            $('#txtAltLine2').val(_data.secAddressLine2);
            //secCity: $("#drpAltCites option:selected").text(),
            //secState: $("#drpAltStates option:selected").text(),
            $('#txtAltStateZip').val(_data.secZipCode);
            $('#txtAltStateZip1').val(_data.secZipCodePlusFour);
            //secCountry: $("#drpAltCountry option:selected").text(),

            $('.txtAltPhone1:visible').val(_data.secTelephoneSegment1);
            $('#txtAltPhone2').val(_data.secTelephoneSegment2);
            $('#txtAltPhone3').val(_data.secTelephoneSegment3);
            $('#txtAltPhone4').val(_data.secTelephoneExtension);


            $('.txtAltHomePhone1:visible').val(_data.secHomephoneSegment1);
            $('#txtAltHomePhone1').val(_data.secHomephoneSegment1);
            $('#txtAltHomePhone2').val(_data.secHomephoneSegment2);
            $('#txtAltHomePhone3').val(_data.secHomephoneSegment3);

            $('#txtAltASSISTANTPhone1').val(_data.secAssistanatSegment1);
            $('#txtAltASSISTANTPhone2').val(_data.secAssistanatSegment2);
            $('#txtAltASSISTANTPhone3').val(_data.secAssistanatSegment3);
            $('#txtAltASSISTANTPhone4').val(_data.secAssistanatExtension);



            $('.txtAltCall1:visible').val(_data.secCellphoneSegment1);
            $('#txtAltCall2').val(_data.secCellphoneSegment2);
            $('#txtAltCall3').val(_data.secCellphoneSegment3);

            $('.txtAltFAX1:visible').val(_data.secFaxSegment1);
            $('#txtAltFAX2').val(_data.secFaxSegment2);
            $('#txtAltFAX3').val(_data.secFaxSegment3);
            $('#txtAltEmail').val(_data.secEMailAddress);
            $('#txtAltEmail2').val(_data.secEMailAddress2);

            // third address

            $('#txtthirdCompany').val(_data.thirdCompany);
            $('#txtthirdLine1').val(_data.thirdAddressLine1);
            $('#txtthirdLine2').val(_data.thirdAddressLine2);
            //secCity: $("#drpAltCites option:selected").text(),
            //secState: $("#drpAltStates option:selected").text(),
            $('#txtthirdStateZip').val(_data.thirdZipCode);
            $('#txtthirdStateZip1').val(_data.thirdZipCodePlusFour);
            //secCountry: $("#drpAltCountry option:selected").text(),

            $('.txtthirdPhone1:visible').val(_data.thirdTelephoneSegment1);
            $('#txtthirdPhone2').val(_data.thirdTelephoneSegment2);
            $('#txtthirdPhone3').val(_data.thirdTelephoneSegment3);
            $('#txtthirdPhone4').val(_data.thirdTelephoneExtension);


            $('.txtthirdHomePhone1:visible').val(_data.thirdHomephoneSegment1);
            $('#txtthirdHomePhone1').val(_data.thirdHomephoneSegment1);
            $('#txtthirdHomePhone2').val(_data.thirdHomephoneSegment2);
            $('#txtthirdHomePhone3').val(_data.thirdHomephoneSegment3);

            $('#txtthirdASSISTANTPhone1').val(_data.thirdAssistanatSegment1);
            $('#txtthirdASSISTANTPhone2').val(_data.thirdAssistanatSegment2);
            $('#txtthirdASSISTANTPhone3').val(_data.thirdAssistanatSegment3);
            $('#txtthirdASSISTANTPhone4').val(_data.thirdAssistanatExtension);



            $('.txtthirdCall1:visible').val(_data.thirdCellphoneSegment1);
            $('#txtthirdCall2').val(_data.thirdCellphoneSegment2);
            $('#txtthirdCall3').val(_data.thirdCellphoneSegment3);

            $('.txtthirdFAX1:visible').val(_data.thirdFaxSegment1);
            $('#txtthirdFAX2').val(_data.thirdFaxSegment2);
            $('#txtthirdFAX3').val(_data.thirdFaxSegment3);
            $('#txtthirdEmail').val(_data.thirdEMailAddress);
            $('#txtthirdEmail2').val(_data.thirdEMailAddress2);

            //PRIMARY CREDIT CARD
            $('#txtCreditCard1').val(_data.priCreditCardSegment1);
            $('#txtCreditCard2').val(_data.priCreditCardSegment2);
            $('#txtCreditCard3').val(_data.priCreditCardSegment3);
            $('#txtCreditCard4').val(_data.priCreditCardSegment4);
            $('#txtCardExpDay').val(_data.priCreditCardExpMonth);
            $('#txtCardExpYear').val(_data.priCreditCardExpYear);
            //ALTERNATE ADDRESS
            $('#txtAltCreditCard1').val(_data.secCreditCardSegment1);
            $('#txtAltCreditCard2').val(_data.secCreditCardSegment2);
            $('#txtAltCreditCard3').val(_data.secCreditCardSegment3);
            $('#txtAltCreditCard4').val(_data.secCreditCardSegment4);
            $('#txtAltCardExpDay').val(_data.secCreditCardExpMonth);
            $('#txtAltCardExpYear').val(_data.secCreditCardExpYear);
            //NOTE
            //contactPreference: $("#drpContactPrefrences option:selected").text(),
            $("#drpContactPrefrences").val(_data.contactPreference);
            $("#drpCustomerType").val(_data.customerType);
            //customerType: $("#drpCustomerType option:s;elected").text(),
            $('#txtNote').val(_data.notes);

           // $('#hdnid').val(data.AppraiserID);
           // $('#txtName').val(data.AppraiserTitle);
            // alert(data.IsActive);
           
        },
       
        error: function (_data) {
            // alert(JSON.stringify(data));
            showMessage('Oops', 'error', _data, 'toast-top-right');
            //  alert(_errorMessage);
        }
    });
}
//$(document).ready(function () {
 
//});
//getCustomer
////////
$(function () {

    //$("#drpPrimaryCountry").trigger('change');
    //$("#drpAltCountry").trigger('change');
    //
    $('#txtTab2PurchaseDate').datepicker();
    $('#btnCalenderShow').click(function () {
        $('#txtTab2PurchaseDate').datepicker('show');
    });
    $('#btnCalenderShow2').click(function () {
        $('#txtTab2WarrantyforthisRepair').datepicker('show');
    });
    //**********************PRIMARY ADDRESS*****************************
    ///get  states
    $("#drpPrimaryCountry").change(function () {
        //  var selectedText = $(this).find("option:selected").text();

        if ($("#drpPrimaryCountry option:selected").text() == "United States") {
            $('#divZipPrimary').show();

            $('#divUSPhonePrimary').show();
            $('#divUSCellPrimary').show();
            $('#divUSFaxPrimary').show();

            $('#divNonUSPhonePrimary').hide();
            $('#divNonUSCellPrimary').hide();
            $('#divNonUSFaxPrimary').hide();

        }
        else {
            $('#divZipPrimary').hide();

            $('#divUSPhonePrimary').hide();
            $('#divUSCellPrimary').hide();
            $('#divUSFaxPrimary').hide();

            $('#divNonUSPhonePrimary').show();
            $('#divNonUSCellPrimary').show();
            $('#divNonUSFaxPrimary').show();
        }



        var id = $(this).val();
      
       //
        if (id == 0) {
           // alert(id);
            showMessage('Oops', 'error', 'Please select primary country!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpPrimaryStates]");
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
                if (TempValues.PriState != '') {
                    $("#drpPrimaryStates").val(TempValues.PriState).attr('selected', true);
                    $("#drpPrimaryStates").change();
                }
            },
            error: function (data) {
                // alert(JSON.stringify(data));
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                //  alert(_errorMessage);
            }
        });
    });
    ///Get cities
    $("#drpPrimaryStates").change(function () {
        //  var selectedText = $(this).find("option:selected").text();
        var id = $(this).val();

        if (id == 0) {
            showMessage('Oops', 'error', 'Please select primary state!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpPrimaryCites]");
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

                if (TempValues.PriCity != '') {
                    $("#drpPrimaryCites option:contains(" + TempValues.PriCity + ")").attr('selected', true);
                }

            },
            error: function (data) {
                // alert(JSON.stringify(data));
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                //  alert(_errorMessage);
            }
        });
    });

    //**********************ALTERNATE ADDRESS*****************************
    ///get  states
    $("#drpAltCountry").change(function () {
        //  var selectedText = $(this).find("option:selected").text();

        if ($("#drpAltCountry option:selected").text() == "United States") {
            $('#divZipAlt').show();

            $('#divUSPhoneAlt').show();
            $('#divUSCellAlt').show();
            $('#divUSFaxAlt').show();

            $('#divNonUSPhoneAlt').hide();
            $('#divNonUSCellAlt').hide();
            $('#divNonUSFaxAlt').hide();


           



        }
        else {
            $('#divZipAlt').hide();

            $('#divUSPhoneAlt').hide();
            $('#divUSCellAlt').hide();
            $('#divUSFaxAlt').hide();

            $('#divNonUSPhoneAlt').show();
            $('#divNonUSCellAlt').show();
            $('#divNonUSFaxAlt').show();
        }


        var id = $(this).val();

        if (id == 0) {
            showMessage('Oops', 'error', 'Please select alternate country!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpAltStates]");
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

                if (TempValues.AltState != '') {
                    $("#drpAltStates").val(TempValues.AltState).attr('selected', true);
                    $("#drpAltStates").change();
                }
            },
            error: function (data) {
                // alert(JSON.stringify(data));
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                //  alert(_errorMessage);
            }
        });
    });
    ///Get cities
    $("#drpAltStates").change(function () {
        //  var selectedText = $(this).find("option:selected").text();
        var id = $(this).val();

        if (id == 0) {
            showMessage('Oops', 'error', 'Please select alternate sate!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpAltCites]");
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

                if (TempValues.AltCity != '') {
                    $("#drpAltCites option:contains(" + TempValues.AltCity + ")").attr('selected', true);
                }
            },
            error: function (data) {
                // alert(JSON.stringify(data));
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                //  alert(_errorMessage);
            }
        });
    });

    var format = function (num) {
        var str = num.toString().replace("$", ""), parts = false, output = [], i = 1, formatted = null;
        if (str.indexOf(".") > 0) {
            parts = str.split(".");
            str = parts[0];
        }
        str = str.split("").reverse();
        for (var j = 0, len = str.length; j < len; j++) {
            if (str[j] != ",") {
                output.push(str[j]);
                if (i % 3 == 0 && j < (len - 1)) {
                    output.push(",");
                }
                i++;
            }
        }
        formatted = output.reverse().join("");
        //return ("$" + formatted + ((parts) ? "." + parts[1].substr(0, 2) : ""));
        return ("" + formatted + ((parts) ? "." + parts[1].substr(0, 2) : ""));
    };

    $(function () {
       

        $("#txtTab3RepairPrice1").keyup(function (e) {

            var charCode = e.keyCode;
            //console.log(charCode);
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
           
        });

        $("#txtTab3RepairPrice2").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 & charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
        });

        $("#txtTab3RepairPrice3").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
        });

        $("#txtTab3RepairPrice4").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
        });

        $("#txtTab3RepairPrice5").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
        });

        $("#txtTab3RepairPrice6").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
        });

        $("#txtTab3RepairPrice7").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                //alert('no char');
                $(this).val(format($(this).val()));
                return true;
            }
        });

    });


 //   var cookieCustomerNumber = readCookie('cookieCustomerNumber');
 //   getCustomer(cookieCustomerNumber);

   // var cookieRepairNumber = readCookie('cookieRepairNumber');
    //getItem(cookieRepairNumber);
    //createCookie('cookieCustomerNumber', 2, 2);
    //createCookie('cookieRepairNumber', 1, 2);
//
   // getCustomer(2);
    //getItem(1);
});

//------------------------ Item Tab-----------------------------------//
function addItem() {
    if (!validateItem()) {
        return;
    }
    var cookieCustomerNumber = readCookie('cookieCustomerNumber');
    var cookieRepairNumber = readCookie('cookieRepairNumber');
    var dt = new Date();
    // Sagar :model to be bind here
     var model =
       {
           customerNumber: cookieCustomerNumber,
           repairNumber: cookieRepairNumber,
           statusId: $('#drpTab2Status').val(),
           ticketNumber: $('#txtTicketNumber').val(),
           employeeId: $("#drptab2Employee").val(),
           brandId: $("#drptab2Brand").val(),
           itemId: $("#drptab2Items").val(),
           styleId: $("#drptab2Style").val(),
           movementTypeId: $("#drptab2Movement").val(),
           caseTypeId: $("#drptab2Case").val(),
           caseTypeId: $("#drptab2Case").val(),
           bandTypeId: $('#drptab2Band').val(),
           buckleTypeId: $('#drptab2Buckle').val(),
           dialTypeId: $('#drptab2Dial').val(),
           crystalConditionId: $('#drptab2CrystalCondition').val(),
           bezelConditionId: $('#drptab2BezelCondition').val(),
           backConditionId: $('#drptab2BackCondition').val(),
           lugsConditionId: $('#drptab2LugsCondition').val(),
           caseConditionId: $('#drptab2CaseCondition').val(),
           bandConditionId: $('#drptab2BandCondition').val(),
           buckleConditionId: $('#drptab2BuckleCondition').val(),
           dialConditionId: $('#drptab2DialCondition').val(),
           serialNumber: $('#txtTab2Serial').val(),
           referenceNumber: $('#txtTab2Reference').val(),
           movementSerialNumber: $('#txtTab2MovementSerial').val(),
           caliber: $('#txtTab2Caliber').val(),
           numberOfJewels: $('#txtTab2NumberofJewels').val(),
           purchaseLocationId: $('#drpTab2PurchaseLocation').val(),
           purchaseDate: $('#txtTab2PurchaseDate').val(),
           wempeInventoryNumber: $('#txtTab2Inventory').val(),
           warrantyDocumentId: $('#drpTab2WarrantyDoc').val(),
           boxIncludedId: $('#drpTab2BoxIncluded').val(),
           giftCertATNumber: $('#txtTab2GiftCert').val(),
           supplierId: $('#drpTab2Location').val(),
           supplierRepairNumber: $('#txtTab2SupplierRepair').val(),
           supplierPickupNumber: $('#txtTab2SupplierPickup').val(),
           repairTypeId: $('#drpTab2RepairType').val(),

           dueDateStartDate: dt,//dueDateStartDate
           dueDateTime: $('#txtTab2DueDateCUSTOMERDays').val(),
           dueDateType: $('#drpTab2DueDateCUSTOMER').val(),
           dueDate: $('#txtTab2DueDateCUSTOMERDays2').val(),
           dueDateFactoryStartDate: dt,//dueDateFactoryStartDate
           dueDateFactoryTime: $('#txtTab2DueDateFactoryDays').val(),
           dueDateFactoryType: $('#drpTab2DueDateFactory').val(),
           dueDateFactory: $('#txtTab2DueDateFactoryDays2').val(),
           //-------------------------APPRAISAL
           supplierPreviousRepairNumber: $('#txtTab2SupplierPrevRepair').val(),
           wempePreviousRepairNumber: $('#txtTab2WepmePrevRepair').val(),
           idSecurityId: $('#drpTab2Security').val(),
           authorizedPickup: $('#txtTab2AuthorizedPickup').val(),
           factoryDelayTime: $('#txtTab2Delay').val(),
           factoryDelayType: $('#drpTab2Delay').val(),
           factoryDelayReason: $('#txtTab2Reason').val(),
           appraisalValue: $('#txtTab2AppraisalValue').val(),
           appraisalComments: $('#txtTab2Comments').val(),
           appraiserId: $('#drptab2APPRAISER').val(),
           appraiserTitleId: $('#drptab2AppraiserTitle').val(),
           //--------------------------ENGRAVING
           engravingFontId: $('#drptab2EngravingFont').val(),
           engravingCapitalizationId: $('#drptab2EngravingCapitalization').val(),
           engravingMessage: $('#txtTab2Message').val(),
           notesToEngraver: $('#txtTab2NoteToEngraver').val(),

           //Sagar -- new columns

           ClientComments: $('#txtClientComments').val(),
           WarrantyRepair: $('#txtTab2WarrantyforthisRepair').val(),
           SupplierDueDate: $('#txtTab2DueDateSupplierDays2').val(),
           SupplierdueDateStartDate: dt,//dueDateFactoryStartDate
           SupplierdueDateTime: $('#txtTab2DueDateSupplierDays').val(),
           dueDateFactoryType: $('#drpTab2DueDateSupplier').val(),
           PickupPersonFirstName: $('#txtTab2PickUpFirstName').val(),
           PickupPersonLastName: $('#txtTab2PickUpLastName').val(),
           OrderNumber: $('#txtOrderNumber').val()

 
       };
    $.ajax({
        url: siteUrl + 'Repair/AddItem',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status) {
                createCookie('cookieRepairNumber', data.Message, 2);
                showMessage('Success', 'success', "Item added successfully!", 'toast-top-right');
            }
            else {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
        },
        error: function (data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
        }
    });
}
function getItem(id) {
    //  alert(id);
   // resetControl();

    if (id == 0) {
        return;
    }
    jQuery.ajax({
        url: siteUrl + 'Repair/getRepair',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { "id": id },
        contentType: 'application/json;charset=utf-8',
        async: false,
        success: function (_data) {
            //
            //alert(JSON.stringify(_data));
            if (_data.Status == false) {
                showMessage('Oops', 'error', _data.Message, 'toast-top-right');
                return;
            }

          //  customerNumber: cookieCustomerNumber,
            $('#txtRepairNumber').val(_data.repairNumber)
            $('#drpTab2Status').val(_data.statusId);
            $('#txtTicketNumber').val(_data.ticketNumber);
            $("#drptab2Employee").val(_data.employeeId);
            $("#drptab2Brand").val(_data.brandId);
            $("#drptab2Items").val(_data.itemId);
            $("#drptab2Style").val(_data.styleId);
            $("#drptab2Movement").val(_data.movementTypeId);
            $("#drptab2Case").val(_data.caseTypeId);
            $("#drptab2Case").val(_data.caseTypeId);
            $('#drptab2Band').val(_data.bandTypeId);
            $('#drptab2Buckle').val(_data.buckleTypeId);
            $('#drptab2Dial').val(_data.dialTypeId);
            $('#drptab2CrystalCondition').val(_data.crystalConditionId);
            $('#drptab2BezelCondition').val(_data.bezelConditionId);
            $('#drptab2BackCondition').val(_data.backConditionId);
            $('#drptab2LugsCondition').val(_data.lugsConditionId);
            $('#drptab2CaseCondition').val(_data.caseConditionId);
            $('#drptab2BandCondition').val(_data.bandConditionId);
            $('#drptab2BuckleCondition').val(_data.buckleConditionId);
            $('#drptab2DialCondition').val(_data.dialConditionId);
            $('#txtTab2Serial').val(_data.serialNumber);
            $('#txtTab2Reference').val(_data.referenceNumber);
            $('#txtTab2MovementSerial').val(_data.movementSerialNumber);
            $('#txtTab2Caliber').val(_data.caliber);
            $('#txtTab2NumberofJewels').val(_data.numberOfJewels);
            $('#drpTab2PurchaseLocation').val(_data.purchaseLocationId);
            $('#txtTab2PurchaseDate').val(parseJsonDate(_data.purchaseDate));
            $('#txtTab2Inventory').val(_data.wempeInventoryNumber);
            $('#drpTab2WarrantyDoc').val(_data.warrantyDocumentId);
            $('#drpTab2BoxIncluded').val(_data.boxIncludedId);
            $('#txtTab2GiftCert').val(_data.giftCertATNumber);
            $('#drpTab2Location').val(_data.supplierId);
            $('#txtTab2SupplierRepair').val(_data.supplierRepairNumber);
            $('#txtTab2SupplierPickup').val(_data.supplierPickupNumber);
            $('#drpTab2RepairType').val(_data.repairTypeId);

            $('#idCUSTOMERStartdate').val(parseJsonDate(_data.dueDateStartDate));//date type
            $('#txtTab2DueDateCUSTOMERDays').val(_data.dueDateTime);
            $('#drpTab2DueDateCUSTOMER').val(_data.dueDateType);
            $('#txtTab2DueDateCUSTOMERDays2').val(parseJsonDate(_data.dueDate));//date type

            $('#idFACTORYStartdate').val(_data.dueDateFactoryStartDate);//date type
            $('#txtTab2DueDateFactoryDays').val(_data.dueDateFactoryTime);
            $('#drpTab2DueDateFactory').val(_data.dueDateFactoryType);
            $('#txtTab2DueDateFactoryDays2').val(parseJsonDate(_data.dueDateFactory));//date type
            ////-------------------------APPRAISAL
            $('#txtTab2SupplierPrevRepair').val(_data.supplierPreviousRepairNumber);
            $('#txtTab2WepmePrevRepair').val(_data.wempePreviousRepairNumber);
            $('#drpTab2Security').val(_data.idSecurityId);
            $('#txtTab2AuthorizedPickup').val(_data.authorizedPickup);
            $('#txtTab2Delay').val(_data.factoryDelayTime);
            $('#drpTab2Delay').val(_data.factoryDelayType);
            $('#txtTab2Reason').val(_data.factoryDelayReason);
            $('#txtTab2AppraisalValue').val(_data.appraisalValue);
            $('#txtTab2Comments').val(_data.appraisalComments);
            $('#drptab2APPRAISER').val(_data.appraiserId);
            $('#drptab2AppraiserTitle').val(_data.appraiserTitleId);
            ////--------------------------ENGRAVING
            $('#drptab2EngravingFont').val(_data.engravingFontId);
            $('#drptab2EngravingCapitalization').val(_data.engravingCapitalizationId);
            $('#txtTab2Message').val(_data.engravingMessage);
            $('#txtTab2NoteToEngraver').val(_data.notesToEngraver)

            // Sagar -- new columns

            $('#txtClientComments').val(_data.ClientComments);
            $('#txtTab2WarrantyforthisRepair').val(_data.WarrantyRepair);
            $('#txtTab2DueDateSupplierDays2').val(_data.SupplierDueDate);
            //SupplierdueDateStartDate: dt,//dueDateFactoryStartDate
            $('#txtTab2DueDateSupplierDays').val(_data.SupplierdueDateTime);
            $('#drpTab2DueDateSupplier').val(_data.dueDateFactoryType);
            $('#txtTab2PickUpFirstName').val(_data.PickupPersonFirstName);
            $('#txtTab2PickUpLastName').val(_data.PickupPersonLastName);

            $('#txtOrderNumber').val(_data.OrderNumber)
        },

        error: function (_data) {
            // alert(JSON.stringify(data));
            showMessage('Oops', 'error', _data, 'toast-top-right');
            //  alert(_errorMessage);
        }
    });
}
function resetItemvalues()
{
    $('#txtRepairNumber').val('NEW')
    $('#drpTab2Status').val(0);
    $('#txtTicketNumber').val('');
    $("#drptab2Employee").val(0);
    $("#drptab2Brand").val(0);
    $("#drptab2Items").val(0);
    $("#drptab2Style").val(0);
    $("#drptab2Movement").val(0);
    $("#drptab2Case").val(0);
    $("#drptab2Case").val(0);
    $('#drptab2Band').val(0);
    $('#drptab2Buckle').val(0);
    $('#drptab2Dial').val(0);
    $('#drptab2CrystalCondition').val(0);
    $('#drptab2BezelCondition').val(0);
    $('#drptab2BackCondition').val(0);
    $('#drptab2LugsCondition').val(0);
    $('#drptab2CaseCondition').val(0);
    $('#drptab2BandCondition').val(0);
    $('#drptab2BuckleCondition').val(0);
    $('#drptab2DialCondition').val(0);
    $('#txtTab2Serial').val('');
    $('#txtTab2Reference').val('');
    $('#txtTab2MovementSerial').val('');
    $('#txtTab2Caliber').val('');
    $('#txtTab2NumberofJewels').val('');
    $('#drpTab2PurchaseLocation').val(0);
    $('#txtTab2PurchaseDate').val('');
    $('#txtTab2Inventory').val('');
    $('#drpTab2WarrantyDoc').val(0);
    $('#drpTab2BoxIncluded').val(0);
    $('#txtTab2GiftCert').val('');
    $('#drpTab2Location').val(0);
    $('#txtTab2SupplierRepair').val('');
    $('#txtTab2SupplierPickup').val('');
    $('#drpTab2RepairType').val(0);

    $('#idCUSTOMERStartdate').val('');//date type
    $('#txtTab2DueDateCUSTOMERDays').val('');
    $('#drpTab2DueDateCUSTOMER').val('Week');
    $('#txtTab2DueDateCUSTOMERDays2').val();//date type

    $('#idFACTORYStartdate').val('');//date type
    $('#txtTab2DueDateFactoryDays').val('');
    $('#drpTab2DueDateFactory').val('Week');
    $('#txtTab2DueDateFactoryDays2').val('');//date type
    ////-------------------------APPRAISAL
    $('#txtTab2SupplierPrevRepair').val('');
    $('#txtTab2WepmePrevRepair').val('');
    $('#drpTab2Security').val('');
    $('#txtTab2AuthorizedPickup').val('');
    $('#txtTab2Delay').val('');
    $('#drpTab2Delay').val('');
    $('#txtTab2Reason').val('');
    $('#txtTab2AppraisalValue').val('');
    $('#txtTab2Comments').val('');
    $('#drptab2APPRAISER').val('');
    $('#drptab2AppraiserTitle').val(0);
    ////--------------------------ENGRAVING
    $('#drptab2EngravingFont').val(0);
    $('#drptab2EngravingCapitalization').val(0);
    $('#txtTab2Message').val('');
    $('#txtTab2NoteToEngraver').val('')
}
function parseJsonDate(jsonDateString) {
    var currentTime = new Date(parseInt(jsonDateString.replace('/Date(', '')));
    var month = currentTime.getMonth()+1 ;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "/" + day + "/" + year;

    return date;
}
function resetRepair()
{
    eraseCookie('cookieCustomerNumber');
    window.location.href = "NewRepair";
}

function ShowPopupSearchByExistingCustomer() {
    //resetControl();
    $('#txtSearchbyExistingCustomer').val('');
    $('#divExistingCustomerList').hide();
    $('#tbodyTemplateExsitingCustomers').html('');
    var $modal = $('#divSearchbyExistingCustomer');
    $modal.modal();
    return;
}

function getListExistingCustomer(pageNo) {

    _lock = true;
    //$('#hdnPageNoExsitingCustomer').val(pageNo);

    //var ColValue = '';
    //if ($('#drpSearchbyExistingCustomer').val() == 'priTelephoneSegment1US') {
    //    ColValue = $('#txtUSPhoneSearch1').val() + $('#txtUSPhoneSearch2').val() + $('#txtUSPhoneSearch3').val();
    //}
    //else {
    //    ColValue = $('#txtSearchbyExistingCustomer').val()
    //}

    var model = {
        Name: $('#txtSearchbyExistingCustomer').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Repair/searchExitingCustomer',
        headers: { "requestType": "client" },
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            $('#divExistingCustomerList').show();
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

function EditExistingCustomer(customerNumber) {
    createCookie('cookieCustomerNumber', customerNumber, 2);
    createCookie('cookieRepairNumber', 0, 2);
    
    getCustomer(customerNumber);
    resetItemvalues();
   // getItem(0);
    $('.close').click();

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
        var _index = parseInt($('#hdnPageNoExsitingCustomer').val()) + 1;
        getListExistingCustomer(_index);
    }
});

$('#sample_1_previous').click(function () {

    var _index = parseInt($('#hdnPageNoExsitingCustomer').val()) - 1;
    if (_index > 0) {
        _lock = true;
        getListExistingCustomer(_index);
    }
});

$('#drpSearchbyExistingCustomer').change(function () {

    $('#txtUSPhoneSearch1').val('');
    $('#txtUSPhoneSearch2').val('');
    $('#txtUSPhoneSearch3').val('');
    if ($(this).val() == 'priTelephoneSegment1US') {
        $('#divforUSPhoneseacrh').show();
        $('#divCommonforseacrh').hide();
    }
    else {
        $('#divforUSPhoneseacrh').hide();
        $('#divCommonforseacrh').show();
    }
});



function ChangeDueDateSupplier() {
    var numStr = /^-?(\d+\.?\d*)$|(\d*\.?\d+)$/;
    var temp = numStr.test($('#txtTab2DueDateSupplierDays').val().toString());
    $('#txtTab2DueDateSupplierDays2').val('');
    var d = null;
    if ($('#txtTab2DueDateSupplierDays').val() != '' && temp == true) {
        d = new Date();
        if ($('#drpTab2DueDateSupplier').val() == 'Week') {
            d.setDate(d.getDate() + (parseInt($('#txtTab2DueDateSupplierDays').val()) * 7));
        }
        if ($('#drpTab2DueDateSupplier').val() == 'Day') {
            d.setDate(d.getDate() + parseInt($('#txtTab2DueDateSupplierDays').val()));
        }
        if (Date.parse(d)) {
            $('#txtTab2DueDateSupplierDays2').val(d.toLocaleDateString());
        }
    }
}



function ChangeDueDateFactory() {
    var numStr = /^-?(\d+\.?\d*)$|(\d*\.?\d+)$/;
    var temp = numStr.test($('#txtTab2DueDateFactoryDays').val().toString());
    $('#txtTab2DueDateFactoryDays2').val('');
    var d = null;
    if ($('#txtTab2DueDateFactoryDays').val() != '' && temp == true) {
        d = new Date();
        if ($('#drpTab2DueDateFactory').val() == 'Week') {
            d.setDate(d.getDate() + (parseInt($('#txtTab2DueDateFactoryDays').val()) * 7));
        }
        if ($('#drpTab2DueDateFactory').val() == 'Day') {
            d.setDate(d.getDate() + parseInt($('#txtTab2DueDateFactoryDays').val()));
        }
        if (Date.parse(d)) {
            $('#txtTab2DueDateFactoryDays2').val(d.toLocaleDateString());
        }
    }
}
function ChangeDueDateCustomer() {

    var numStr = /^-?(\d+\.?\d*)$|(\d*\.?\d+)$/;
    var temp = numStr.test($('#txtTab2DueDateCUSTOMERDays').val().toString());
    $('#txtTab2DueDateCUSTOMERDays2').val('');
    var d = null;
    if ($('#txtTab2DueDateCUSTOMERDays').val() != '' && temp == true) {
        
        //alert( $('#idCUSTOMERStartdate').val());
        if ($('#idCUSTOMERStartdate').val() == "") {
            d = new Date();
        }
        else {
            d = new Date($('#idCUSTOMERStartdate').val());
        }

        if ($('#drpTab2DueDateCUSTOMER').val() == 'Week') {
            d.setDate(d.getDate() + (parseInt($('#txtTab2DueDateCUSTOMERDays').val()) * 7));
        }
        if ($('#drpTab2DueDateCUSTOMER').val() == 'Day') {
            d.setDate(d.getDate() + parseInt($('#txtTab2DueDateCUSTOMERDays').val()));
        }
        if (Date.parse(d)) {
            $('#txtTab2DueDateCUSTOMERDays2').val(d.toLocaleDateString());
        }
    }
}

// Location Popup

function ShowPopupSearchByLocation() {
   // resetControl();

    if ($('#drpTab2Location').val() == '') {
        alert('Select Location');
        return;
    }
    else {
        jQuery.ajax({
            url: siteUrl + 'Repair/getDatabyLocationId',
            dataType: 'json',
            headers: { "requestType": "client" },
            data: { "id": $('#drpTab2Location').val() },
            contentType: 'application/json;charset=utf-8',
            async: false,
            success: function (_data) {
                $('#txtLocationSearchSupplier').val(_data.name);
                $('#txtLocationSearchCountry').val(_data.mainCountry);
                $('#txtLocationSearchCompany').val(_data.mainCompany);
                $('#txtLocationSearchLine1').val(_data.mainAddressLine1);
                $('#txtLocationSearchStickerCode').val(_data.mainStickerCode);
                $('#txtLocationSearchLine2').val(_data.mainAddressLine2);
                $('#txtLocationSearchWempeAcct').val(_data.mainWempeAccount);
                $('#txtLocationSearchCity').val(_data.mainCity);
                $('#txtLocationSearchContact').val(_data.mainContact);
                $('#txtLocationSearchStateZip').val(_data.mainState);
                $('#txtLocationSearchPhone1').val(_data.mainTelephoneSegment1);
                $('#txtLocationSearchPhone2').val(_data.mainTelephoneSegment2);
                $('#txtLocationSearchFax1').val(_data.mainFaxSegment1);
                $('#txtLocationSearchFax2').val(_data.mainFaxSegment2);
                $('#txtLocationSearchEmail1').val(_data.mainEMailAddress);
                $('#txtLocationSearchEmail2').val(_data.mainEMailAddress);
                //SPAREPARTS

                $('#txtSPAREPARTSLocationSearchCountry').val(_data.sparePartsCountry);
                $('#txtSPAREPARTSLocationSearchCompany').val(_data.sparePartsCompany);
                $('#txtSPAREPARTSLocationSearchLine1').val(_data.sparePartsAddressLine1);
                $('#txtSPAREPARTSLocationSearchStickerCode').val(_data.sparePartsStickerCode);
                $('#txtSPAREPARTSLocationSearchLine2').val(_data.sparePartsAddressLine2);
                $('#txtSPAREPARTSLocationSearchWempeAcct').val(_data.sparePartsWempeAccount);
                $('#txtSPAREPARTSLocationSearchCity').val(_data.sparePartsCity);
                $('#txtSPAREPARTSLocationSearchContact').val(_data.sparePartsContact);
                $('#txtSPAREPARTSLocationSearchStateZip').val(_data.sparePartsState);
                $('#txtSPAREPARTSLocationSearchPhone1').val(_data.sparePartsContactTelephoneSegment1);
                $('#txtSPAREPARTSLocationSearchPhone2').val(_data.sparePartsContactTelephoneSegment2);
                $('#txtSPAREPARTSLocationSearchFax1').val(_data.sparePartsContactFaxSegment1);
                $('#txtSPAREPARTSLocationSearchFax2').val(_data.sparePartsContactFaxSegment2);
                $('#txtSPAREPARTSLocationSearchEmail1').val(_data.sparePartsContactEMailAddress);
                $('#txtSPAREPARTSLocationSearchEmail2').val(_data.sparePartsContactEMailAddress);

                // strap
                $('#txtSTRAPLocationSearchCountry').val(_data.strapCountry);
                $('#txtSTRAPLocationSearchCompany').val(_data.strapCompany);
                $('#txtSTRAPLocationSearchLine1').val(_data.strapAddressLine1);
                $('#txtSTRAPLocationSearchStickerCode').val(_data.strapStickerCode);
                $('#txtSTRAPLocationSearchLine2').val(_data.strapAddressLine1);
                $('#txtSTRAPLocationSearchWempeAcct').val(_data.strapWempeAccount);
                $('#txtSTRAPLocationSearchCity').val(_data.strapCity);
                $('#txtSTRAPLocationSearchContact').val(_data.strapContact);
                $('#txtSTRAPLocationSearchStateZip').val(_data.strapState);
                $('#txtSTRAPLocationSearchPhone1').val(_data.strapTelephoneSegment1);
                $('#txtSTRAPLocationSearchPhone2').val(_data.strapTelephoneSegment2);
                $('#txtSTRAPLocationSearchFax1').val(_data.strapFaxSegment1);
                $('#txtSTRAPLocationSearchFax2').val(_data.strapFaxSegment2);
                $('#txtSTRAPLocationSearchEmail1').val(_data.strapEMailAddress);
                $('#txtSTRAPLocationSearchEmail2').val(_data.strapEMailAddress);

                // BILLING

                $('#txtBILLINGLocationSearchCountry').val(_data.billingCountry);
                $('#txtBILLINGLocationSearchCompany').val(_data.billingCompany);
                $('#txtBILLINGLocationSearchLine1').val(_data.billingAddressLine1);
                $('#txtBILLINGLocationSearchStickerCode').val(_data.billingStickerCode);
                $('#txtBILLINGLocationSearchLine2').val(_data.billingAddressLine2);
                $('#txtBILLINGLocationSearchWempeAcct').val(_data.billingWempeAccount);
                $('#txtBILLINGLocationSearchCity').val(_data.billingCity);
                $('#txtBILLINGLocationSearchContact').val(_data.billingContact);
                $('#txtBILLINGLocationSearchStateZip').val(_data.billingState);
                $('#txtBILLINGLocationSearchPhone1').val(_data.billingTelephoneSegment1);
                $('#txtBILLINGLocationSearchPhone2').val(_data.billingTelephoneSegment2);
                $('#txtBILLINGLocationSearchFax1').val(_data.billingFaxSegment1);
                $('#txtBILLINGLocationSearchFax2').val(_data.billingFaxSegment2);
                $('#txtBILLINGLocationSearchEmail1').val(_data.billingEMailAddress);
                $('#txtBILLINGLocationSearchEmail2').val(_data.billingEMailAddress);


                $('#txtSearchLocationNotes').val(_data.notes);
            },
            error: function (_data) {
                showMessage('Oops', 'error', _data, 'toast-top-right');
            }
        });
        var $modal = $('#divSearchbyLocation');
        $modal.modal();
        return;
    }
}
// purchase popup

function ShowPopupSearchByPurchase() {
   // resetControl();
    if ($('#drpTab2PurchaseLocation').val() == '') {
        alert('Select Purchase Location');
        return;
    }
    else {
        jQuery.ajax({
            url: siteUrl + 'Repair/getDatabyPurchaseLocationId',
            dataType: 'json',
            headers: { "requestType": "client" },
            data: { "id": $('#drpTab2PurchaseLocation').val() },
            contentType: 'application/json;charset=utf-8',
            async: false,
            success: function (_data) {
                $('#txtPurchaseSearchStore').val(_data.name);
                $('#txtPurchaseSearchPhone').val(_data.mainTelephoneSegment1);
                $('#txtPurchaseSearchCountry').val(_data.mainCountry);
                $('#txtPurchaseSearchFax').val(_data.mainContactFaxSegment1);
                $('#txtPurchaseSearchLine1').val(_data.mainAddressLine1);
                $('#txtPurchaseSearchEmail').val(_data.mainEMailAddress);
                $('#txtPurchaseSearchLine2').val(_data.mainAddressLine2);
                $('#txtPurchaseSearchContact').val(_data.mainContact);
                $('#txtPurchaseSearchCity').val(_data.mainCity);
                $('#txtPurchaseSearchPhone1').val(_data.mainTelephoneSegment1);
                $('#txtPurchaseSearchStateZip').val(_data.mainState);
                $('#txtPurchaseSearchFax1').val(_data.mainFaxSegment1);
                $('#txtPurchaseSearchEmail1').val(_data.mainContactEMailAddress);
                $('#txtSearchLocationNotes').val(_data.notes);
            },
            error: function (_data) {
                showMessage('Oops', 'error', _data, 'toast-top-right');
            }
        });
        var $modal = $('#divSearchbyPurchase');
        $modal.modal();
        return;
    }
}


// tab3 Repair section
function CalculateRepairTotalAmount()
{
    var total = 0;
    $(".Tab3Amount:enabled").each(function (index, box) {
        
        if ($(box).val() != '') {
           
            total += parseFloat($(box).val(), 10);
        }
    });
    $('#txtTab3RepairPriceSubTotal').val(total);
    var grandTotal = 0;
    grandTotal = total;
    if ($('#txtTab3RepairPriceShipping').val() != '') {
        grandTotal = grandTotal + parseFloat($('#txtTab3RepairPriceShipping').val());
    }
    if ($('#txtTab3RepairPriceTax').val() != '') {
        grandTotal = grandTotal + parseFloat($('#txtTab3RepairPriceTax').val());
    }
   // grandTotal = total + parseFloat($('#txtTab3RepairPriceShipping').val()) + parseFloat($('#txtTab3RepairPriceTax').val());
    $('#txtTab3RepairPriceTotal').val(grandTotal);
}
$(document).on('change', '.Tab3Amount', function () {
    CalculateRepairTotalAmount();
});
$(document).on('change', '.chkTab3DisableEnableAmount', function () {
    if ($(this).is(":checked")) {
        $(this).parents('.divTab3Repair').find('.Tab3Amount').attr("disabled", "disabled");
    }
    else {
        $(this).parents('.divTab3Repair').find('.Tab3Amount').attr("disabled", false);
    }
    CalculateRepairTotalAmount();
});
$(document).on('change', '#chkTab3RepairCheck1', function () {
    if ($(this).is(":checked")) {
        $('#txtTab3RepairPrice1').attr("disabled", "disabled");
        $('#txtTab3RepairPrice2').attr("disabled", "disabled");
    }
    else {
        $('#txtTab3RepairPrice1').attr("disabled", false);
        $('#txtTab3RepairPrice2').attr("disabled", false);
    }
    CalculateRepairTotalAmount();
});
$(document).on('change', '#chkTab3Tax', function () {
    if ($(this).is(":checked")) {
        // fix value for testing
        $('#txtTab3RepairPriceTax').val("1.07");
        // tax calculation logic here
    }
    else {
        $('#txtTab3RepairPriceTax').val("0.00");
    }
    CalculateRepairTotalAmount();
});


//Sagar- Tab 3 Save Repair function
function addRepair() {
    //if (!validateItem()) {
    //    return;
    //}
    // validation pending
    var model =
      {
          task1Description: $('#txtTab3RepairNumber').val(),
          task1Price: $('#txtTab3RepairPrice1').val(),
          task1Decline: $('#chkTab3RepairCheck1').is(":checked"),
          task2Description: $('#txtTab3RepairNumber2').val(),
          task2Price: $('#txtTab3RepairPrice2').val(),
          task2Decline: $('#chkTab3RepairCheck2').is(":checked"),
          task3Description: $('#txtTab3RepairNumber3').val(),
          task3Price: $('#txtTab3RepairPrice3').val(),
          task3Decline: $('#chkTab3RepairCheck3').is(":checked"),
          task4Description: $('#txtTab3RepairNumber4').val(),
          task4Price: $('#txtTab3RepairPrice4').val(),
          task4Decline: $('#chkTab3RepairCheck4').is(":checked"),
          task5Description: $('#txtTab3RepairNumber5').val(),
          task5Price: $('#txtTab3RepairPrice5').val(),
          task5Decline: $('#chkTab3RepairCheck5').is(":checked"),
          task6Description: $('#txtTab3RepairNumber6').val(),
          task6Price: $('#txtTab3RepairPrice6').val(),
          task6Decline: $('#chkTab3RepairCheck6').is(":checked"),
          task7Description: $('#txtTab3RepairNumber7').val(),
          task7Price: $('#txtTab3RepairPrice7').val(),
          task7Decline: $('#chkTab3RepairCheck7').is(":checked"),
          shipping: $('#txtTab3RepairPriceShipping').val(),
          subtotal: $('#txtTab3RepairPriceSubTotal').val(),
          tax: $('#txtTab3RepairPriceTax').val(),
          taxable: $('#chkTab3Tax').is(":checked"),
          total: $('#txtTab3RepairPriceTotal').val(),
          repairComments: $('#txtRepairComments').val(),
          notesToCustomer: $('#txtTab3NotesToCustomer').val(),
          notesToFactory: $('#txtTab3NotesToFactory').val(),
          letterToCustomer: $('#txtTab3LetterToCustomer').val(),
          shippingToCustomerValue: $('#txtTab3ShippingCustomerValue').val(),
          shippingToCustomerInsurance: $('#txtTab3ShippingCustomerInsurance').val(),
          shippingToCustomerSalesperson: $('#txtTab3ShippingCustomerSalesPerson').val(),
          shippingToCustomerDestinationType: $('#drpTab3ToCustomerDestinationType').val(),
          shippingToCustomerCarrier: $('#txtTab3ShippingCustomerCarrier').val(),
          shippingToCustomerShippingType: $('#txtTab3ShippingCustomerDestinationType').val(),
          shippingToCustomerMailingDate: $('#txtTab3ToCustomeMailingDate').val(),
          shippingToCustomerExpectedArrivalDate: $('#txtTab3ToCustomerExpectedArrival').val(),
          shippingToSupplierValue: $('#txtTab3ShippingSupplierValue').val(),
          shippingToSupplierInsurance: $('#txtTab3ShippingSupplierInsurance').val(),
          shippingToSupplierSalesperson: $('#txtTab3ShippingSupplierSalesPerson').val(),
          shippingToSupplierDestinationType: $('#drpTab3ToSupplierDestinationType').val(),
          shippingToSupplierCarrier: $('#txtTab3ShippingSupplierCarrier').val(),
          shippingToSupplierShippingType: $('#txtTab3ShippingSupplierDestinationType').val(),
          shippingToSupplierMailingDate: $('#txtTab3ToSupplierMailingDate').val(),
          shippingToSupplierExpectedArrivalDate: $('#txtTab3ToSupplierExpectedArrival').val(),
          sparePartsQuantity1: $('#txtTab3SparePartQty1').val(),
          sparePartsPartNumber1: $('#txtTab3SparePart1').val(),
          sparePartsDescription1: $('#txtTab3SparePartDescription1').val(),
          sparePartsQuantity2: $('#txtTab3SparePartQty2').val(),
          sparePartsPartNumber2: $('#txtTab3SparePart2').val(),
          sparePartsDescription2: $('#txtTab3SparePartDescription2').val(),
          sparePartsQuantity3: $('#txtTab3SparePartQty3').val(),
          sparePartsPartNumber3: $('#txtTab3SparePart3').val(),
          sparePartsDescription3: $('#txtTab3SparePartDescription3').val(),
          sparePartsQuantity4: $('#txtTab3SparePartQty4').val(),
          sparePartsPartNumber4: $('#txtTab3SparePart4').val(),
          sparePartsDescription4: $('#txtTab3SparePartDescription4').val(),
          sparePartsQuantity5: $('#txtTab3SparePartQty5').val(),
          sparePartsPartNumber5: $('#txtTab3SparePart5').val(),
          sparePartsDescription5: $('#txtTab3SparePartDescription5').val(),
          sparePartsQuantity6: $('#txtTab3SparePartQty6').val(),
          sparePartsPartNumber6: $('#txtTab3SparePart6').val(),
          sparePartsDescription6: $('#txtTab3SparePartDescription6').val(),
          sparePartsQuantity7: $('#txtTab3SparePartQty7').val(),
          sparePartsPartNumber7: $('#txtTab3SparePart7').val(),
          sparePartsDescription7: $('#txtTab3SparePartDescription7').val(),
          sparePartsQuantity8: $('#txtTab3SparePartQty8').val(),
          sparePartsPartNumber8: $('#txtTab3SparePart8').val(),
          sparePartsDescription8: $('#txtTab3SparePartDescription8').val(),
          sparePartsOrderNotes: $('#txtTab3SparePartOrderNotes').val(),
          strapMaterialType: $('#txtTab3StrapOrderMaterialType').val(),
          strapColor: $('#txtTab3StrapOrderColor').val(),
          strapWidth: $('#txtTab3StrapOrderWidth').val(),
          strapBuckle: $('#txtTab3StrapOrderBuckle').val(),
          strapLength: $('#txtTab3StrapOrderLength').val(),
          strapTextureType: $('#txtTab3StrapOrderTextureType').val(),
          strapLusterType: $('#txtTab3StrapOrderLusterType').val(),
          strapStitchType: $('#txtTab3StrapOrderStitchType').val(),
          strapThicknessType: $('#txtTab3StrapOrderThicknessType').val(),
          strapAccessories: $('#txtTab3StrapOrderAccessories').val(),
          strapSpecOrd600: $('#txtTab3StrapOrderLengthSO6').val(),
          strapSpecOrd1200: $('#txtTab3StrapOrderLengthSO12').val(),
          strapOrderNotes: $('#txtTab3StrapOrderNotes').val()
      };

    $.ajax({

       
        url: siteUrl + 'Repair/AddItem',  // same method is called for item-- need to discuss
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status) {
                createCookie('cookieRepairNumber', data.Message, 2);
                showMessage('Success', 'success', "Item added successfully!", 'toast-top-right');
            }
            else {
                showMessage('Oops', 'error', data.Message, 'toast-top-right');
            }
        },
        error: function (data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
        }
    });
}
