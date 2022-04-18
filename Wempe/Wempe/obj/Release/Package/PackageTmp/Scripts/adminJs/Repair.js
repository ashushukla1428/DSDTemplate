var _lock = true;

var _SearchType = 'fixed';

var TempValues = { PriState: '', PriCity: '', AltState: '', AltCity: '', thirdState: '', thirdCity: '', StatusAttr: '' };
$('#divRepairTab').click(function (e) {

    if ($('#hdnStatus').val() == "19" || $('#hdnStatus').val() == "1") {
        alert('This repair is locked.  No changes may be made.');
        $(e.target).blur()
    }
});
function createModel() {
    var dt = new Date();
    var task1Decline1, task1Decline2, task1Decline3, task1Decline4, task1Decline5, task1Decline6, task1Decline7, taxAble = '0';
    
    if ($('#chkTab3RepairCheck1').is(":checked")) {
        task1Decline1 = '1';
    }
    if ($('#chkTab3RepairCheck2').is(":checked")) {
        task1Decline2 = '1';
    }
    if ($('#chkTab3RepairCheck3').is(":checked")) {
        task1Decline3 = '1';
    }
    if ($('#chkTab3RepairCheck4').is(":checked")) {
        task1Decline4 = '1';
    }
    if ($('#chkTab3RepairCheck5').is(":checked")) {
        task1Decline5 = '1';
    }
    if ($('#chkTab3RepairCheck6').is(":checked")) {
        task1Decline6 = '1';
    }
    if ($('#chkTab3RepairCheck7').is(":checked")) {
        task1Decline7 = '1';
    }
    if ($('#chkTab3Tax').is(":checked")) {
        taxAble = '1';
    }
    var model =
       {
           customerNumber: $('#hdnCustomerNumber').val(),
           repairNumber: $('#hdnRepairNumber').val(),
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
           OrderNumber: $('#txtOrderNumber').val(),

           // tab 3




           task1Description: $('#txtTab3RepairNumber').val(),
           task1Price: $('#txtTab3RepairPrice1').val(),
           task1Decline: task1Decline1,
           task2Description: $('#txtTab3RepairNumber2').val(),
           task2Price: $('#txtTab3RepairPrice2').val(),
           task2Decline: task1Decline2,
           task3Description: $('#txtTab3RepairNumber3').val(),
           task3Price: $('#txtTab3RepairPrice3').val(),
           task3Decline: task1Decline3,
           task4Description: $('#txtTab3RepairNumber4').val(),
           task4Price: $('#txtTab3RepairPrice4').val(),
           task4Decline: task1Decline4,
           task5Description: $('#txtTab3RepairNumber5').val(),
           task5Price: $('#txtTab3RepairPrice5').val(),
           task5Decline: task1Decline5,
           task6Description: $('#txtTab3RepairNumber6').val(),
           task6Price: $('#txtTab3RepairPrice6').val(),
           task6Decline: task1Decline6,
           task7Description: $('#txtTab3RepairNumber7').val(),
           task7Price: $('#txtTab3RepairPrice7').val(),
           task7Decline: task1Decline7,
           shipping: $('#txtTab3RepairPriceShipping').val(),
           subtotal: $('#txtTab3RepairPriceSubTotal').val(),
           tax: $('#txtTab3RepairPriceTax').val(),
           taxable: taxAble,
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
}
function CheckCustomerNumber() {

}
function SetHideAllDivs() {
    $("#tab_0").removeClass("active");
    $("#tab_1").removeClass("active");
    $("#tab_2").removeClass("active");
    $("#tab_3").removeClass("active");
    $("#tab_4").removeClass("active");
    $("#ulTabList li").removeClass("active");
}
//------------------------ Customer Tab-----------------------------------//
function addCustomer(_callType) {

        //$(this).text('Saving...');
        //alert(siteUrl);
        // alert($('#txtPhone1').val() != '' ? $('#txtPhone1').val() : $('#txtPhone1NonUs').val());
        if (!validateCustomer()) {
            return;
        }
        //
        var _priCity;
        var _secCity;
        var _thirdCity;
        //#region
        _priCity = $("#drpPrimaryCites option:selected").text();
       
        _secCity = $("#drpAltCites option:selected").text();
       
        _thirdCity = $("#drpthirdCites option:selected").text();

        //#endregion
        var model =
           {

               priCountryCode: $('#txtPrimaryCountryCode').val(),
               AlternateCountryCode: $('#txtAlternateCountryCode').val(),
               ThirdCountryCode: $('#txtThirdCountryCode').val(),

               customerNumber: $('#hdnCustomerNumber').val(),
               title: $("#drpTitels1 option:selected").text(),
               firstName: $('#txtFirstName').val(),
               middleInitial: $('#txtMiddle').val(),
               lastName: $('#txtLast').val(),

               asstTitle: $("#drpTitels2 option:selected").text(),
               asstFirstName: $('#txtFirstName1').val(),
               asstMiddleInitial: $('#txtMiddle1').val(),
               asstLastName: $('#txtLast1').val(),
               asstRelation: $('#ddlAssistantRelation').val(),

               SpouseTitle: $("#drpTitelsSpouse option:selected").text(),
               SpousefirstName: $('#txtFirstNameSpouse').val(),
               SpousemiddleInitial: $('#txtMiddleSpouse').val(),
               SpouselastName: $('#txtLastSpouse').val(),
               SpouseRelation: $('#ddlSpouseRelation').val(),

               priCompany: $('#txtCompany').val(),
               priAddressLine1: $('#txtLine1').val(),
               priAddressLine2: $('#txtLine2').val(),

               priState: $("#drpPrimaryStates").val(),
               priZipCode: $('#txtStateZip').val(),
               priZipCodePlusFour: $('#txtStateZip1').val(),
               priCountry: $("#drpPrimaryCountry").val(),
               priCity: _priCity,
               //priCity: $('#drpPrimaryCites').val(),
               //priTelephoneSegment1: $('.txtPhone1:visible').val(),
               priTelephoneSegment1: $('#txtOfficePhone').val(),
               priTelephoneExtension: $('#txtOfficePhoneExtension').val(),


               priHomephoneSegment1: $('#txtHomePhone').val(),
               priHomephoneExtension: $('#txtHomePhoneExtension').val(),


               priAssistanatSegment1: $('#txtASSISTANTPhone').val(),
               priAssistanatExtension: $('#txtASSISTANTPhoneExtension').val(),

               priCellphoneSegment1: $('#txtCell').val(),
               priFaxSegment1: $('#txtFAX').val(),
               priFaxSegment2: $('#txtFAXExtension').val(),



               priEMailAddress: $('#txtEmail').val(),
               priEmailAddress2: $('#txtEmail2').val(),
               //ALTERNATE ADDRESS
               secCompany: $('#txtAltCompany').val(),
               secAddressLine1: $('#txtAltLine1').val(),
               secAddressLine2: $('#txtAltLine2').val(),
               secCity: _secCity,
              // secCity: $('#drpAltCites').val(),
               secState: $("#drpAltStates").val(),
               secZipCode: $('#txtAltStateZip').val(),
               secZipCodePlusFour: $('#txtAltStateZip1').val(),
               secCountry: $("#drpAltCountry").val(),

               secTelephoneSegment1: $("#txtAltOfficePhone").val(),

               secTelephoneExtension: $("#txtAltOfficePhoneExtension").val(),

               secHomephoneSegment1: $("#txtAltHomePhone").val(),
               secHomephoneExtension: $("#txtAltHomePhoneExtension").val(),

               secAssistanatSegment1: $("#txtAltAssistantPhone").val(),
               secAssistanatExtension: $("#txtAltAssistantPhoneExtension").val(),
               secCellphoneSegment1: $("#txtAltCell").val(),
               secFaxSegment1: $("#txtAltFAX").val(),
               secFaxSegment2: $("#txtAltFAXExtension").val(),
             


               secEMailAddress: $('#txtAltEmail').val(),
               secEMailAddress2: $('#txtAltEmail2').val(),
               // Third Address

               thirdCompany: $('#txtthirdCompany').val(),
               thirdAddressLine1: $('#txtthirdLine1').val(),
               thirdAddressLine2: $('#txtthirdLine2').val(),
                thirdCity: _thirdCity,
               //thirdCity: $('#drpthirdCites').val(),

               thirdState: $("#drpthirdStates").val(),
               thirdZipCode: $('#txtthirdStateZip').val(),
               thirdZipCodePlusFour: $('#txtthirdStateZip1').val(),
               thirdCountry: $("#drpthirdCountry").val(),

               thirdTelephoneSegment1: $("#txtthirdOfficePhone").val(),

               thirdTelephoneExtension: $("#txtthirdOfficePhoneExtension").val(),


               thirdHomephoneSegment1: $("#txtthirdHomePhone").val(),
               thirdHomephoneExtension: $("#txtthirdHomePhoneExtension").val(),

               thirdAssistanatSegment1: $("#txtthirdAssistantPhone").val(),

               thirdAssistanatExtension: $("#txtthirdAssistantPhoneExtension").val(),

               thirdCellphoneSegment1: $("#txtthirdCell").val(),
               thirdFaxSegment1: $("#txtthirdFAX").val(),
               thirdFaxSegment2: $("#txtthirdFAXExtension").val(),
           


               thirdEMailAddress: $('#txtthirdEmail').val(),
               thirdEmailAddress2: $('#txtthirdEmail2').val(),

               //PRIMARY CREDIT CARD
               priCreditCardSegment1: $('#txtCreditCard1').val(),
               priCreditCardSegment2: $('#txtCreditCard2').val(),
               priCreditCardSegment3: $('#txtCreditCard3').val(),
               priCreditCardSegment4: $('#txtCreditCard4').val(),
               priCreditCardExpMonth: $('#txtCardExpDay').val(),
               priCreditCardExpYear: $('#txtCardExpYear').val(),
               priCreditCardSecurity: $('#txtCardSecurity').val(),

               //ALTERNATE ADDRESS
               secCreditCardSegment1: $('#txtAltCreditCard1').val(),
               secCreditCardSegment2: $('#txtAltCreditCard2').val(),
               secCreditCardSegment3: $('#txtAltCreditCard3').val(),
               secCreditCardSegment4: $('#txtAltCreditCard4').val(),
               secCreditCardExpMonth: $('#txtAltCardExpDay').val(),
               secCreditCardExpYear: $('#txtAltCardExpYear').val(),
               secCreditCardSecurity: $('#txtAltCardSecurity').val(),
               // third

               //ALTERNATE ADDRESS
               thirdCreditCardSegment1: $('#txtthirdCreditCard1').val(),
               thirdCreditCardSegment2: $('#txtthirdCreditCard2').val(),
               thirdCreditCardSegment3: $('#txtthirdCreditCard3').val(),
               thirdCreditCardSegment4: $('#txtthirdCreditCard4').val(),
               thirdCreditCardExpMonth: $('#txtthirdCardExpDay').val(),
               thirdCreditCardExpYear: $('#txtthirdCardExpYear').val(),
               thirdCreditCardSecurity: $('#txtthirdCardSecurity').val(),

               //NOTE
               contactPreference: $("#drpContactPrefrences").val(),
               customerType: $("#drpCustomerType").val(),
               notes: $('#txtNote').val()
           };
        $.ajax({
            url: siteUrl + 'Repair/AddCustomer',
            type: 'POST',
            headers: { "requestType": "client" },
            data: JSON.stringify(model),
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                if (data.Status) {
                    $('#hdnCustomerNumber').val(data.Message);
                    if (_callType == 1) {
                        showMessage('Success', 'success', "record saved successfully!", 'toast-top-right');
                    }
                    //  getList(1);
                }
                else {
                    showMessage('Oops', 'error', data.Message, 'toast-top-right');
                }
            },
            error: function (_data) {



                showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
            }
        });
    
    //  $(this).text('Submit');
}
function getCustomer(id) {
    if (id == 0) {
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
            if (_data == null) {
                return;
            }
            if (_data.Status == false) {
                showMessage('Oops', 'error', _data.Message, 'toast-top-right');
                return;
            }
            // primary country, state, city
            //$("#drpPrimaryCountry option:contains(" + _data.priCountry + ")").attr('selected', true).trigger('change');
            //$("#drpAltCountry option:contains(" + _data.secCountry + ")").attr('selected', true).trigger('change');
            $("#drpPrimaryCountry").val(_data.priCountry).trigger('change');
            $("#drpAltCountry").val(_data.secCountry).trigger('change');
            $("#drpthirdCountry").val(_data.thirdCountry).trigger('change');
            TempValues.PriState = _data.priState;
            TempValues.PriCity = _data.priCity;

            $('#txtPrimaryCites').val(_data.priCity);
            $('#txtAltCites').val(_data.secCity);
            $('#txtthirdCites').val(_data.thirdCity);

            TempValues.AltState = _data.secState;
            TempValues.AltCity = _data.secCity;
            TempValues.thirdState = _data.thirdState;
            TempValues.thirdCity = _data.thirdCity;
            $('#drpTitelsSpouse option').map(function () {
                if ($(this).text() == _data.SpouseTitle) return this;
            }).attr('selected', 'selected');
            $('#txtFirstNameSpouse').val(_data.SpousefirstName);
            $('#txtMiddleSpouse').val(_data.SpousemiddleInitial);
            $('#txtLastSpouse').val(_data.SpouselastName);



            $('#ddlSpouseRelation').val(_data.SpouseRelation);
            $('#drpTitels1 option').map(function () {
                if ($(this).text() == _data.title) return this;
            }).attr('selected', 'selected');
            //  $("#drpTitels1 option[value='" + _data.title + "']").attr('selected', 'selected');



            $('#txtFirstName').val(_data.firstName);
            $('#txtMiddle').val(_data.middleInitial);
            $('#txtLast').val(_data.lastName);

            $('#spanCustomer').html(_data.firstName + ' ' + _data.lastName);

            $('#drpTitels2 option').map(function () {
                if ($(this).text() == _data.asstTitle) return this;
            }).attr('selected', 'selected');
            //$("#drpTitels2 option[value='" + _data.asstTitle + "']").attr('selected', 'selected');
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


            $('#txtPrimaryCountryCode').val(_data.priCountryCode);
            $('#txtAlternateCountryCode').val(_data.AlternateCountryCode);
            $('#txtThirdCountryCode').val(_data.ThirdCountryCode);

            //priCountry: $("#drpPrimaryCountry option:selected").text(),



            $('#txtOfficePhone').val(_data.priTelephoneSegment1);
            $('#txtOfficePhoneExtension').val(_data.priTelephoneExtension);

            $('#txtHomePhone').val(_data.priHomephoneSegment1);
            $('#txtHomePhoneExtension').val(_data.priHomePhoneExtension);
            $('#txtASSISTANTPhone').val(_data.priAssistanatSegment1);
            $('#txtASSISTANTPhoneExtension').val(_data.priAssistanatExtension);
            $('#txtCell').val(_data.priCellphoneSegment1);
            $('#txtFAX').val(_data.priFaxSegment1);
            $('#txtFAXExtension').val(_data.priFaxSegment2);



            if ($("#drpPrimaryCountry option:selected").text() == "United States") {

                $('#divPrimaryCountryCode').hide();


                $("#txtOfficePhone").mask("(999) 999-9999");
                $("#txtHomePhone").mask("(999) 999-9999");
                $("#txtASSISTANTPhone").mask("(999) 999-9999");
                $("#txtCell").mask("(999) 999-9999");
                $("#txtFAX").mask("(999) 999-9999");

              
            }
            else {

                $('#divPrimaryCountryCode').show();

                $("#txtOfficePhone").unmask("(999) 999-9999");
                $("#txtHomePhone").unmask("(999) 999-9999");
                $("#txtASSISTANTPhone").unmask("(999) 999-9999");
                $("#txtCell").unmask("(999) 999-9999");
                $("#txtFAX").unmask("(999) 999-9999");
              
            }
            //$('.txtHomePhone1:visible').val(_data.priHomephoneSegment1);
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




            $('#txtAltOfficePhone').val(_data.secTelephoneSegment1);
            $('#txtAltHomePhone').val(_data.secHomephoneSegment1);
            $('#txtAltAssistantPhone').val(_data.secAssistanatSegment1);
            $('#txtAltCell').val(_data.secCellphoneSegment1);
            $('#txtAltFAX').val(_data.secFaxSegment1);

            
            //alert(_data.secHomephoneExtension);
            $('#txtAltOfficePhoneExtension').val(_data.secTelephoneExtension);
            $('#txtAltHomePhoneExtension').val(_data.secHomePhoneExtension);
            $('#txtAltAssistantPhoneExtension').val(_data.secAssistanatExtension);
           // $('#txtAltCell').val(_data.secCellphoneSegment1);
            $('#txtAltFAXExtension').val(_data.secFaxSegment2);


            if ($("#drpAltCountry option:selected").text() == "United States") {


                $('#divAlternateCountryCode').hide();

                $("#txtAltOfficePhone").mask("(999) 999-9999");
                $("#txtAltHomePhone").mask("(999) 999-9999");
                $("#txtAltAssistantPhone").mask("(999) 999-9999");
                $("#txtAltCell").mask("(999) 999-9999");
                $("#txtAltFAX").mask("(999) 999-9999");

             
               
            }
            else {

                $('#divAlternateCountryCode').show();

             

                $("#txtAltOfficePhone").unmask("(999) 999-9999");
                $("#txtAltHomePhone").unmask("(999) 999-9999");
                $("#txtAltAssistantPhone").unmask("(999) 999-9999");
                $("#txtAltCell").unmask("(999) 999-9999");
                $("#txtAltFAX").unmask("(999) 999-9999");

            }


            $('#txtthirdOfficePhone').val(_data.thirdTelephoneSegment1);
            $('#txtthirdHomePhone').val(_data.thirdHomephoneSegment1);
            $('#txtthirdAssistantPhone').val(_data.thirdAssistanatSegment1);
            $('#txtthirdCell').val(_data.thirdCellphoneSegment1);
            $('#txtthirdFAX').val(_data.thirdFaxSegment1);


            

            $('#txtthirdOfficePhoneExtension').val(_data.thirdTelephoneExtension);
            $('#txtthirdHomePhoneExtension').val(_data.thirdHomePhoneExtension);
            $('#txtthirdAssistantPhoneExtension').val(_data.thirdAssistanatExtension);
            //$('#txtthirdCell').val(_data.thirdCellphoneSegment1);
            $('#txtthirdFAXExtension').val(_data.thirdFaxSegment2);


            if ($("#drpthirdCountry option:selected").text() == "United States") {


                $('#divThirdCountryCode').hide();

                $("#txtthirdOfficePhone").mask("(999) 999-9999");
                $("#txtthirdHomePhone").mask("(999) 999-9999");
                $("#txtthirdAssistantPhone").mask("(999) 999-9999");
                $("#txtthirdCell").mask("(999) 999-9999");
                $("#txtthirdFAX").mask("(999) 999-9999");

              
             
            }
            else {
                $('#divThirdCountryCode').show();
                $("#txtthirdOfficePhone").unmask("(999) 999-9999");
                $("#txtthirdHomePhone").unmask("(999) 999-9999");
                $("#txtthirdAssistantPhone").unmask("(999) 999-9999");
                $("#txtthirdCell").unmask("(999) 999-9999");
                $("#txtthirdFAX").unmask("(999) 999-9999");
            }
            $('#txtAltEmail').val(_data.secEMailAddress);
            $('#txtAltEmail2').val(_data.secEmailAddress2);
            // third address
            $('#txtthirdCompany').val(_data.thirdCompany);
            $('#txtthirdLine1').val(_data.thirdAddressLine1);
            $('#txtthirdLine2').val(_data.thirdAddressLine2);
            //secCity: $("#drpAltCites option:selected").text(),
            //secState: $("#drpAltStates option:selected").text(),
            $('#txtthirdStateZip').val(_data.thirdZipCode);
            $('#txtthirdStateZip1').val(_data.thirdZipCodePlusFour);
            //secCountry: $("#drpAltCountry option:selected").text(),
            $('#txtthirdEmail').val(_data.thirdEMailAddress);
            $('#txtthirdEmail2').val(_data.thirdEMailAddress2);
            //PRIMARY CREDIT CARD
            $('#txtCreditCard1').val(_data.priCreditCardSegment1);
            $('#txtCreditCard2').val(_data.priCreditCardSegment2);
            $('#txtCreditCard3').val(_data.priCreditCardSegment3);
            $('#txtCreditCard4').val(_data.priCreditCardSegment4);
            $('#txtCardExpDay').val(_data.priCreditCardExpMonth);
            $('#txtCardExpYear').val(_data.priCreditCardExpYear);
            $('#txtCardSecurity').val(_data.priCreditCardSecurity);
            //ALTERNATE ADDRESS
            $('#txtAltCreditCard1').val(_data.secCreditCardSegment1);
            $('#txtAltCreditCard2').val(_data.secCreditCardSegment2);
            $('#txtAltCreditCard3').val(_data.secCreditCardSegment3);
            $('#txtAltCreditCard4').val(_data.secCreditCardSegment4);
            $('#txtAltCardExpDay').val(_data.secCreditCardExpMonth);
            $('#txtAltCardExpYear').val(_data.secCreditCardExpYear);
            $('#txtAltCardSecurity').val(_data.secCreditCardSecurity);
            // THIRD Address
            $('#txtthirdCreditCard1').val(_data.thirdCreditCardSegment1);
            $('#txtthirdCreditCard2').val(_data.thirdCreditCardSegment2);
            $('#txtthirdCreditCard3').val(_data.thirdCreditCardSegment3);
            $('#txtthirdCreditCard4').val(_data.thirdCreditCardSegment4);
            $('#txtthirdCardExpDay').val(_data.thirdCreditCardExpMonth);
            $('#txtthirdCardExpYear').val(_data.thirdCreditCardExpYear);
            $('#txtthirdCardSecurity').val(_data.thirdCreditCardSecurity);
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
//On page load
$(function () {
    $("#ulTabList li a").click(function () {


        //if ($(this).parent().attr("id") == 'Li3') {



        //    $('#chkTab3Tax').prop('checked', true);

        //    $('#chkTab3Tax').change();
        //}


        if ($(this).parent().attr("id") == 'Li1') {
        }
        if ($(this).parent().attr("id") == 'Li2' || $(this).parent().attr("id") == undefined) {
            var x = !validateCustomer();
            if (x == true) {
                SetHideAllDivs();
                $('#Li1').addClass("active");
                $("#tab_0").addClass("active");
                //showMessage('Oops', 'error', 'Please Save Customer Tab data first!', 'toast-top-right');
                return false;
            }
            else if ($('#hdnCustomerNumber').val() == '0') {
                SetHideAllDivs();
                $('#Li1').addClass("active");
                $("#tab_0").addClass("active");
                showMessage('Oops', 'error', 'Please Save Customer Tab data first!', 'toast-top-right');
                return false;
            }
        }
        if ($(this).parent().attr("id") == undefined && $('#hdnCustomerNumber').val() != '0' && $(this).parent().attr("id") != 'Li2' && $(this).parent().attr("id") != 'Li1') {
            var ValidateItem = !validateItem();
            if (ValidateItem == true) {
                SetHideAllDivs();
                $('#Li2').addClass("active");
                $("#tab_1").addClass("active");
                //showMessage('Oops', 'error', 'Please Save Item Tab data first!', 'toast-top-right');
                return false;
            }
            else if ($('#hdnCustomerNumber').val() != '0' && $('#hdnRepairNumber').val() == '0' && $(this).parent().attr("id") != 'Li2' && $(this).parent().attr("id") != 'Li1') {
                SetHideAllDivs();
                $('#Li2').addClass("active");
                $("#tab_1").addClass("active");
                showMessage('Oops', 'error', 'Please Save Item Tab data first!', 'toast-top-right');
                return false;
            }
        }
        else {
            return true;
        }
    });
    $('#txtTab3ToCustomeMailingDate').datepicker();
    $('#txtTab3ToCustomerExpectedArrival').datepicker();
    $('#txtTab3ToSupplierMailingDate').datepicker();
    $('#txtTab3ToSupplierExpectedArrival').datepicker();
    $('#btnTab3CustomerMailingDateCalenderShow').click(function () {
        $('#txtTab3ToCustomeMailingDate').datepicker('show');
    });
    $('#btnTab3CustomerExpectdArrivalCalenderShow').click(function () {
        $('#txtTab3ToCustomerExpectedArrival').datepicker('show');
    });
    $('#btnTab3SupplierMailingDateCalenderShow').click(function () {
        $('#txtTab3ToSupplierMailingDate').datepicker('show');
    });
    $('#btnTab3SupplierExpectdArrivalCalenderShow').click(function () {
        $('#txtTab3ToSupplierExpectedArrival').datepicker('show');
    });
    $('#txtTab2PurchaseDate').datepicker();
    $('#btnCalenderShow').click(function () {
        $('#txtTab2PurchaseDate').datepicker('show');
    });
    $('#btnCalenderShow2').click(function () {
        $('#txtTab2WarrantyforthisRepair').datepicker('show');
    });

    $('#drptab2Brand').change(function () {

        $('#drptab2Items').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2Style').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2Movement').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2Case').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2Band').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2Buckle').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2Dial').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2CrystalCondition').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2BezelCondition').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2BackCondition').empty().append('<option selected="selected" value=""></option>');

        $('#drptab2LugsCondition').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2CaseCondition').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2BandCondition').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2BuckleCondition').empty().append('<option selected="selected" value=""></option>');
        $('#drptab2DialCondition').empty().append('<option selected="selected" value=""></option>');

        

        var id = $('#drptab2Brand').val();
        if (id == "") {
            id = "0";
        }
       
        $.ajax({
            url: siteUrl + 'Master/getItemByBrand',
            headers: { "requestType": "client" },
            //type: 'POST',
            data: { id: id },
            contentType: 'application/json;charset=utf-8',
            success: function (data) {

                


                $('#drptab2Items').empty().append('<option selected="selected" value=""></option>');
                
               
                $.each(data, function () {
                   // alert(this['Value']);
                    if (this['Type'] == 'item') {
                        $('#drptab2Items').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'style') {
                        $('#drptab2Style').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'movementType') {
                        $('#drptab2Movement').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'caseType') {
                        $('#drptab2Case').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'BandType') {
                        $('#drptab2Band').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'buckleType') {
                        $('#drptab2Buckle').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'dialType') {
                        $('#drptab2Dial').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'crystalCondition') {
                        $('#drptab2CrystalCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'bezelCondition') {
                        $('#drptab2BezelCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'backCondition') {
                        $('#drptab2BackCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'lugsCondition') {
                        $('#drptab2LugsCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'caseCondition') {
                        $('#drptab2CaseCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'bandCondition') {
                        $('#drptab2BandCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'buckleCondition') {
                        $('#drptab2BuckleCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    else if (this['Type'] == 'dialCondition') {
                        $('#drptab2DialCondition').append($("<option></option>").val(this['Id']).html(this['Value']));
                    }
                    
                });
            },
            error: function (_data) {
            }
        });



    });
    //**********************PRIMARY ADDRESS*****************************
    ///get  states



     function SaveNewStateName(openType) {

            var CountryId = '0';
            var StateName=''; 
            if (openType == '1') {
                CountryId = $('#drpPrimaryCountry').val();
                StateName=$('#txtNewPrimaryStateAdd').val();
            }
            if (openType == '2') {
                CountryId = $('#drpAltCountry').val();
                StateName=$('#txtNewAlternateStateAdd').val();
            }
            if (openType == '3') {
                CountryId = $('#drpthirdCountry').val();
                StateName=$('#txtNewThirdStateAdd').val();
            }
            if (CountryId == 0) {
                showMessage('Oops', 'error', "Please select country first", 'toast-top-right');
            }
            else {

                alert(CountryId);

                jQuery.ajax({
                    url: siteUrl + 'Repair/SaveNewState',
                    dataType: 'json',
                    headers: { "requestType": "client" },
                    data: { "CountryId": "abc" },
                    contentType: 'application/json;charset=utf-8',
                    success: function (data) {
                       
                    },
                    error: function (data) {
                    }
                });

                //$.ajax({
                //    url: siteUrl + 'Repair/SaveNewState',
                //    headers: { "requestType": "client" },
                //    //type: 'POST',
                //    data: { "CountryId": CountryId, "StateName": StateName },
                //    contentType: 'application/json;charset=utf-8',
                //    success: function (data) {
                       
                //    },
                //    error: function (_data) {
                //    }
                //});
            }

     }




    $("#drpPrimaryCountry").change(function () {
      
        if ($("#drpPrimaryCountry option:selected").text() == "United States") {


            $("#txtOfficePhone").mask("(999) 999-9999");
            $("#txtHomePhone").mask("(999) 999-9999");
            $("#txtASSISTANTPhone").mask("(999) 999-9999");
            $("#txtCell").mask("(999) 999-9999");
            $("#txtFAX").mask("(999) 999-9999");

            $('#txtPrimaryCites').hide();
            $('#divPrimaryCountryCode').hide();
        }
        else {
            $('#divPrimaryCountryCode').show();
            $("#txtOfficePhone").unmask("(999) 999-9999");

            $("#txtHomePhone").unmask("(999) 999-9999");
            $("#txtASSISTANTPhone").unmask("(999) 999-9999");
            $("#txtCell").unmask("(999) 999-9999");
            $("#txtFAX").unmask("(999) 999-9999");

            $('#txtPrimaryCites').show();

        }
        var id = $(this).val();
        if (id == 0) {
            showMessage('Oops', 'error', 'Please select primary country!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpPrimaryStates]");
        drpSates.empty().append('<option selected="selected" value="">Loading....</option>');


        $('#drpPrimaryCites').empty().append('<option selected="selected" value="">Please select</option>');


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
                    //alert(TempValues.PriState);
                    $("#drpPrimaryStates").val(TempValues.PriState).attr('selected', true);
                    TempValues.PriState=''
                    $("#drpPrimaryStates").change();
                }
            },
            error: function (data) {

                
                drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
            }
        });
    });
    ///Get cities
    $("#drpPrimaryStates").change(function () {

        //  var selectedText = $(this).find("option:selected").text();

        if ($("#drpPrimaryStates option:selected").text() != "Please select") {

            var id = $(this).val();
            if (id == null) {
                return;
            }
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
                        // $("#drpPrimaryCites option:contains(" + TempValues.PriCity + ")").attr('selected', true);
                        $("#drpPrimaryCites").val(TempValues.PriCity);
                        TempValues.PriCity = '';
                    }

                },
                error: function (data) {
                    // alert(JSON.stringify(data));
                    drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                    showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                    //  alert(_errorMessage);
                }
            });
        }
    });
    //**********************ALTERNATE ADDRESS*****************************
    ///get  states
    $("#drpAltCountry").change(function () {
        //  var selectedText = $(this).find("option:selected").text();

        if ($("#drpAltCountry option:selected").text() == "United States") {
           // $('#divZipAlt').show();

            $("#txtAltOfficePhone").mask("(999) 999-9999");
            $("#txtAltHomePhone").mask("(999) 999-9999");
            $("#txtAltAssistantPhone").mask("(999) 999-9999");
            $("#txtAltCell").mask("(999) 999-9999");
            $("#txtAltFAX").mask("(999) 999-9999");



            $('#txtAltCites').show();
            $('#divAlternateCountryCode').hide();
        }
        else {
           // $('#divZipAlt').hide();

            $("#txtAltOfficePhone").unmask("(999) 999-9999");
            $("#txtAltHomePhone").unmask("(999) 999-9999");
            $("#txtAltAssistantPhone").unmask("(999) 999-9999");
            $("#txtAltCell").unmask("(999) 999-9999");
            $("#txtAltFAX").unmask("(999) 999-9999");

            $('#txtAltCites').show();

            $('#divAlternateCountryCode').show();
        }


        var id = $(this).val();

        if (id == 0) {
            showMessage('Oops', 'error', 'Please select alternate country!', 'toast-top-right');
            return;
        }
        var drpSates = $("[id*=drpAltStates]");
        drpSates.empty().append('<option selected="selected" value="">Loading....</option>');

        $('#drpAltCites').empty().append('<option selected="selected" value="">Please select</option>');

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
                    TempValues.AltState = '';
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

        if ($("#drpAltStates option:selected").text() != "Please select") {
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
                         //$("#drpAltCites option:contains(" + TempValues.AltCity + ")").attr('selected', true);
                         $("#drpAltCites").val(TempValues.AltCity);
                        //$("#drpAltCites").val(TempValues.AltCity).attr('selected', true);
                        TempValues.AltCity = '';
                    }
                },
                error: function (data) {
                    // alert(JSON.stringify(data));
                    drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                    showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                    //  alert(_errorMessage);
                }
            });
        }
    });
    $("#drpthirdCountry").change(function () {
        //  var selectedText = $(this).find("option:selected").text();

        if ($("#drpthirdCountry option:selected").text() == "United States") {
            $('#divThirdCountryCode').hide();

            $("#txtthirdOfficePhone").mask("(999) 999-9999");
            $("#txtthirdHomePhone").mask("(999) 999-9999");
            $("#txtthirdAssistantPhone").mask("(999) 999-9999");
            $("#txtthirdCell").mask("(999) 999-9999");
            $("#txtthirdFAX").mask("(999) 999-9999");
        
            $('#txtthirdCites').hide();


        }
        else {


            $("#txtthirdOfficePhone").unmask("(999) 999-9999");
            $("#txtthirdHomePhone").unmask("(999) 999-9999");
            $("#txtthirdAssistantPhone").unmask("(999) 999-9999");
            $("#txtthirdCell").unmask("(999) 999-9999");
            $("#txtthirdFAX").unmask("(999) 999-9999");


            $('#divThirdCountryCode').show();

           // $('#divZipthird').hide();
            $('#divUSPhonethird').hide();
            $('#divUSCellthird').hide();
            $('#divUSFaxthird').hide();

            $('#divUSHomePhonethird').hide();
            $('#divUSAssistantPhonethird').hide();



            $('#divNonUSPhonethird').show();
            $('#divNonUSCellthird').show();
            $('#divNonUSFaxthird').show();


            $('#divNonUSHomePhonethird').show();
            $('#divNonUSAssistantPhonethird').show();
            $('#txtthirdCites').show();
        }

        if ($("#drpthirdCountry option:selected").text() != "Select Country") {
            var id = $(this).val();

            if (id == 0) {
                showMessage('Oops', 'error', 'Please select third address country!', 'toast-top-right');
                return;
            }
            var drpSates = $("[id*=drpthirdStates]");
            drpSates.empty().append('<option selected="selected" value="">Loading....</option>');


            $('#drpthirdCites').empty().append('<option selected="selected" value="">Please select</option>');

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

                    if (TempValues.thirdState != '') {
                        $("#drpthirdStates").val(TempValues.thirdState).attr('selected', true);
                        TempValues.thirdState = '';
                        $("#drpthirdStates").change();
                    }
                },
                error: function (data) {
                    // alert(JSON.stringify(data));
                    drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                    showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                    //  alert(_errorMessage);
                }
            });
        }
    });


    $("#drpthirdStates").change(function () {

        if ($("#drpthirdStates option:selected").text() != "Please select") {
            //  var selectedText = $(this).find("option:selected").text();
            var id = $(this).val();

            if (id == 0) {
                showMessage('Oops', 'error', 'Please select third address state!', 'toast-top-right');
                return;
            }
            var drpSates = $("[id*=drpthirdCites]");
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

                    if (TempValues.thirdCity != '') {
                        //$("#drpthirdCites option:contains(" + TempValues.thirdCity + ")").attr('selected', true);
                        $("#drpthirdCites").val(TempValues.thirdCity);
                        //$("#drpthirdCites").val(TempValues.thirdCity).attr('selected', true);
                        TempValues.thirdCity = '';
                    }
                },
                error: function (data) {
                    // alert(JSON.stringify(data));
                    drpSates.empty().append('<option selected="selected" value="">Please select</option>');
                    showMessage('Oops', 'error', data.responseJSON.Message, 'toast-top-right');
                    //  alert(_errorMessage);
                }
            });
        }
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
        $("#txtTab3RepairPrice1,#txtTab3RepairPrice2,#txtTab3RepairPrice3,#txtTab3RepairPrice4,#txtTab3RepairPrice5,#txtTab3RepairPrice6,#txtTab3RepairPrice7").keyup(function (e) {
            var charCode = e.keyCode;
            var inputString = $(this).val();
            if ((charCode > 64 && charCode < 91) || (charCode > 110 && charCode < 123)) {
                var shortenedString = inputString.substr(0, (inputString.length - 1));
                $(this).val(shortenedString);
                return false;
            }
            else {
                $(this).val(format($(this).val()));
                return true;
            }
        });
    });

    var _id = getParameterByName('id', null);
    if (_id != null && _id != 0) {

        // alert(_id);

        $('#hdnCustomerNumber').val(_id);
        getCustomer(_id);
    }
    var Rid = getParameterByName('Rid', null);
    if (Rid != null && Rid != 0) {
        $('#hdnRepairNumber').val(Rid);
        getItem(Rid);
    }
    // set Page title

    if (Rid != null) {
        //$(document).prop('title', $('#txtFirstName').val() + '-' + Rid);
        var temp = '';
        if ($('#txtRepairNumber').val() != '') {
            temp = $('#txtRepairNumber').val();
        }
        if ($('#txtTicketNumber').val() != '') {
            temp += '-' + $('#txtTicketNumber').val();
        }
        if ($('#txtOrderNumber').val() != '') {
            temp += '-' + $('#txtOrderNumber').val();
        }
        $(document).prop('title', temp);
    }
    else {
        $(document).prop('title', 'Repair');
    }

    if (_id == null && Rid == null) {
        $.ajax({
            url: siteUrl + 'Repair/GetLastCustomerAndRepairNumber',
            headers: { "requestType": "client" },
            type: 'POST',
            //data: JSON.stringify(d),
            contentType: 'application/json;charset=utf-8',
            success: function (_data) {
                if (_data != null) {
                    if (_data.CustomerNumber != undefined && _data.CustomerNumber.toString() != '0') {
                        $('#hdnCustomerNumber').val(_data.CustomerNumber);
                        getCustomer(_data.CustomerNumber);
                    }
                    if (_data.RepairNumber != undefined && _data.RepairNumber.toString() != '0') {
                        $('#hdnRepairNumber').val(_data.RepairNumber);
                        localStorage.setItem('_RepairNumber', _data.RepairNumber);
                        getItem(_data.RepairNumber);
                    }
                }
            },
            error: function (_data) {
            }
        });
    }


    // bindDocumentsGrid('1');
    //$('#drpPrimaryCountry').change();
    //$('#drpAltCountry').change();
    //$('#drpthirdCountry').change();

    $(".btnEmail").click(function () {

        //$(this).text("Sending...");

        var d = { RepairNumber: $('#hdnRepairNumber').val(), DocumentName: 'letterToCustomer' };
        $.ajax({
            url: siteUrl + 'Repair/GetEmails',
            type: 'POST',
            headers: { "requestType": "client" },
            data: JSON.stringify(d),
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                $('#DocumentType').html(data.DocumentType);
                $('#DocumentFullName').html(data.DocumentFullName);
                if (data.priEMailAddress == null) {
                    $('#chkBoxEmailPoupPrimary').attr('disabled', 'disabled');
                    $('#txtBoxEmailPoupPrimary').val('(Not Set)');
                    $('#txtBoxEmailPoupPrimary').attr('disabled', 'disabled');
                }
                else {
                    $('#txtBoxEmailPoupPrimary').val(data.priEMailAddress);
                    $("#chkBoxEmailPoupPrimary").removeAttr('disabled');
                    $('#txtBoxEmailPoupPrimary').attr('disabled', 'disabled');
                }
                if (data.secEMailAddress == null) {
                    $('#chkBoxEmailPoupSecondary').attr('disabled', 'disabled');
                    $('#txtBoxEmailPoupSecondary').val('(Not Set)');
                    $('#txtBoxEmailPoupSecondary').attr('disabled', 'disabled');
                }
                else {
                    $('#txtBoxEmailPoupSecondary').val(data.secEMailAddress);
                    $("#chkBoxEmailPoupSecondary").removeAttr('disabled');
                    $('#txtBoxEmailPoupPrimary').attr('disabled', 'disabled');
                }
                //if (data.thirdEMailAddress == null) {
                //    $('#chkBoxEmailPoupThird').attr('disabled', 'disabled');
                //    $('#txtBoxEmailPoupThird').val('(Not Set)');
                //    $('#txtBoxEmailPoupThird').attr('disabled', 'disabled');
                //}
                //else {
                //    $('#txtBoxEmailPoupThird').val(data.thirdEMailAddress);
                //}
            },
            error: function (_data) {
                showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
            }
        });
        var $modal = $('#responsiveEmailBox');
        $modal.modal();
    });

    $(".btnSendEmail").click(function () {
        if ($('.chkBoxSendEmailPopUp:checked').length < 1) {
            showMessage('Oops', 'error', 'Please choose atlease one recipient.', 'toast-top-right');
        }
        else {
            var Emails = [];
            if ($('#chkBoxEmailPoupPrimary').is(':checked') == true && $('#txtBoxEmailPoupPrimary').val() == '') {
                showMessage('Oops', 'error', 'Please enter a primary e-mail address or disable the primary e-mail address checkbox.', 'toast-top-right');
            }
            else if ($('#chkBoxEmailPoupSecondary').is(':checked') == true && $('#txtBoxEmailPoupSecondary').val() == '') {
                showMessage('Oops', 'error', 'Please enter a alternate e-mail address or disable the alternate e-mail address checkbox.', 'toast-top-right');
            }
            else if ($('#chkBoxEmailPoupThird').is(':checked') == true && $('#txtBoxEmailPoupThird').val() == '') {
                showMessage('Oops', 'error', 'Please enter a custom e-mail address or disable the custom e-mail address checkbox.', 'toast-top-right');
            }
            else {
                var x = !validateEmailIdsOfPdfPopUp();
                if (x == false) {
                    $.each($('.chkBoxSendEmailPopUp:checked'), function () {
                        Emails.push($(this).parents('.form-group').parent().find('input[type=text]').val());
                    });

                    $('.btnSendEmail').text("Sending...");
                    $('.btnSendEmail').attr('disabled', 'disabled');
                    var d = { RepairNumber: $('#hdnRepairNumber').val(), HTML: $('#divPrint').html(), Emails: Emails, DocumentName: 'letterToCustomer' };
                    //alert(JSON.stringify(d));
                    $.ajax({
                        url: siteUrl + 'Repair/sendEmail',
                        type: 'POST',
                        headers: { "requestType": "client" },
                        data: JSON.stringify(d),
                        contentType: 'application/json;charset=utf-8',
                        success: function (data) {
                            showMessage('Success', 'success', data.Message, 'toast-top-right');
                            $('.btnSendEmail').text("Email");
                            $('.btnSendEmail').attr('disabled', false);
                        },
                        error: function (_data) {
                            //alert(JSON.stringify(_data));
                            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
                            $('.btnSendEmail').text("Email");
                            $('.btnSendEmail').attr('disabled', false);
                        }
                    });
                }
            }
        }

    });
});
//------------------------ Item Tab-----------------------------------//
function addItem() {

   
    addCustomer(0);
    if (!validateItem()) {

        if ($('#drpTab2Location').val() == '' || $('#drpTab2RepairType').val() == '' || $('#drpTab2Status').val() == '') {
            $('#divItemPartial').css('display', 'block');
            $('#aDivItemPartial').removeClass('expand').addClass('collapse');

        }
        return;
    }
    //  for updating data in case user modify some data in another tab
    var dt = new Date();
    var task1Decline1, task1Decline2, task1Decline3, task1Decline4, task1Decline5, task1Decline6, task1Decline7, taxAble = '0';
    if ($('#chkTab3RepairCheck1').is(":checked")) {
        task1Decline1 = '1';
    }
    if ($('#chkTab3RepairCheck2').is(":checked")) {
        task1Decline2 = '1';
    }
    if ($('#chkTab3RepairCheck3').is(":checked")) {
        task1Decline3 = '1';
    }
    if ($('#chkTab3RepairCheck4').is(":checked")) {
        task1Decline4 = '1';
    }
    if ($('#chkTab3RepairCheck5').is(":checked")) {
        task1Decline5 = '1';
    }
    if ($('#chkTab3RepairCheck6').is(":checked")) {
        task1Decline6 = '1';
    }
    if ($('#chkTab3RepairCheck7').is(":checked")) {
        task1Decline7 = '1';
    }
    if ($('#chkTab3Tax').is(":checked")) {
        taxAble = '1';
    }
    var model =
       {
           customerNumber: $('#hdnCustomerNumber').val(),
           repairNumber: $('#hdnRepairNumber').val(),
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

           dueDateStartDate: $('#txtTab2DueDateCUSTOMERDays2').val(),//dueDateStartDate
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
           OrderNumber: $('#txtOrderNumber').val(),
           // tab 3
           task1Description: $('#txtTab3RepairNumber').val(),
           task1Price: $('#txtTab3RepairPrice1').val(),
           task1Decline: task1Decline1,
           task2Description: $('#txtTab3RepairNumber2').val(),
           task2Price: $('#txtTab3RepairPrice2').val(),
           task2Decline: task1Decline2,
           task3Description: $('#txtTab3RepairNumber3').val(),
           task3Price: $('#txtTab3RepairPrice3').val(),
           task3Decline: task1Decline3,
           task4Description: $('#txtTab3RepairNumber4').val(),
           task4Price: $('#txtTab3RepairPrice4').val(),
           task4Decline: task1Decline4,
           task5Description: $('#txtTab3RepairNumber5').val(),
           task5Price: $('#txtTab3RepairPrice5').val(),
           task5Decline: task1Decline5,
           task6Description: $('#txtTab3RepairNumber6').val(),
           task6Price: $('#txtTab3RepairPrice6').val(),
           task6Decline: task1Decline6,
           task7Description: $('#txtTab3RepairNumber7').val(),
           task7Price: $('#txtTab3RepairPrice7').val(),
           task7Decline: task1Decline7,
           shipping: $('#txtTab3RepairPriceShipping').val(),
           subtotal: $('#txtTab3RepairPriceSubTotal').val(),
           tax: $('#txtTab3RepairPriceTax').val(),
           taxable: taxAble,
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
        url: siteUrl + 'Repair/AddItem',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.Status) {
                $('#hdnRepairNumber').val(data.Message);
                localStorage.setItem("_RepairNumber", data.Message);
                //showMessage('Success', 'success', "Item added successfully!", 'toast-top-right');
                showMessage('Success', 'success', "Record saved successfully!", 'toast-top-right');
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
function checkDuplicateOrderNumber(Id) {
    var d = { OrderId: Id, RepairNumber: $('#hdnRepairNumber').val() };
    $.ajax({
        url: siteUrl + 'Repair/CheckDuplicateOrderNumber',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(d),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data == 'Duplicate') {
                showMessage('Oops', 'error', 'Order number already exists!', 'toast-top-right');
                $('#txtOrderNumber').val('');
            }
        },
        error: function (data) {
            showMessage('Oops', 'error', _errorMessage, 'toast-top-right');
        }
    });
}
function getItem(id) {
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
            if (_data != null) {
                if (_data.Status == false) {
                    showMessage('Oops', 'error', _data.Message, 'toast-top-right');
                    return;
                }
            }

            FillRepairData(_data);

            FillDocStatusData(_data);

            $('#hdnRepairNumber').val(_data.repairNumber);
            //$('#hdnRepairNumber').val(_data.ticketNumber);

            bindDocumentsGrid('1');


            $('#txtRepairNumber').val(_data.repairNumber)
            if (_data.repairNumber != null && _data.ticketNumber != null && _data.OrderNumber != null) {
                $('#spanNewRepair').html(_data.repairNumber + '-' + _data.ticketNumber + '-' + _data.OrderNumber);
            }
            else if (_data.repairNumber != null && _data.ticketNumber != null) {
                $('#spanNewRepair').html(_data.repairNumber + '-' + _data.ticketNumber);
            }
            else {
                $('#spanNewRepair').html(_data.repairNumber);
            }
            $('#drpTab2Status').val(_data.statusId);
            $('#hdnStatus').val(_data.statusId);
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

            // $('#idCUSTOMERStartdate').val(parseJsonDate(_data.dueDateStartDate));//date type
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

            //---Doc and Status tab
            // alert(_data.entryDate);
            if (_data.entryDate != null) {
                $('#docStatusRepairStart').html(parseJsonDate(_data.entryDate));
            }
            if (_data.dueDate != null) {
                $('#docStatusCustomerRepairDueDate').html(parseJsonDate(_data.dueDate));
            }
            //alert(_data.SupplierDueDate);
            if (_data.SupplierDueDate != null) {
                $('#docStatusFactoryRepairDueDate').html(_data.SupplierDueDate);
            }
            //
            if (_data.factoryDelayTime != null) {
                $('#docStatusFactoryDelay').html(_data.factoryDelayTime + ' ' + _data.factoryDelayType);
            }

        },

        error: function (_data) {
            // alert(JSON.stringify(data));
            showMessage('Oops', 'error', _data, 'toast-top-right');
            //  alert(_errorMessage);
        }
    });
}
function resetItemvalues() {
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
    if (jsonDateString == null) {
        return "";
    }
    var currentTime = new Date(parseInt(jsonDateString.replace('/Date(', '')));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "/" + day + "/" + year;

    return date;
}
function resetRepair() {



    if (confirm("Are you sure?") == true) {
        $.ajax({
            url: siteUrl + 'Repair/ResetLastCustomerAndRepairNumber',
            headers: { "requestType": "client" },
            type: 'POST',
            contentType: 'application/json;charset=utf-8',
            success: function (_data) {
                window.location.href = "NewRepair";
            },
            error: function (_data) {
            }
        });
    }


}
function resetRepairAndCustomerId() {
    $.ajax({
        url: siteUrl + 'Repair/ResetLastCustomerAndRepairNumber',
        headers: { "requestType": "client" },
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
        success: function (_data) {
        },
        error: function (_data) {
        }
    });
    window.location.href = "NewRepair";
}
function ShowPopupSearchByExistingCustomer() {
    $('#txtSearchbyExistingCustomer').val('');
    $('#divExistingCustomerList').hide();
    $('#tbodyTemplateExsitingCustomers').html('');
    var $modal = $('#divSearchbyExistingCustomer');
    getListExistingCustomer(1);
    $modal.modal();
    return;
}
function getListExistingCustomer(pageNo) {

    $('#hdnPageNo').val(pageNo);
    $('#hdnPageNoExsitingCustomer').val(pageNo);

    _SearchType = 'custom';


    _lock = true;
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

                $('#tbodyDetails').html('<tr><td colspan="6">No record found.</td></tr>');

            }
            else {
                setPagingDetail(data.length, data[0].TotalCount, pageNo);
            }
        },
        error: function (_data) {
            $('#tbodyDetails').html('<tr><td colspan="6">error in request</td></tr>');
        }
    });
}
function EditExistingCustomer(customerNumber) {
    $('#hdnCustomerNumber').val(data.customerNumber);
    getCustomer(customerNumber);
    //resetItemvalues();
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
        //getListExistingCustomer(_index);

        if (_SearchType == 'custom') {
            getListExistingCustomer(_index);
        }
        if (_SearchType == 'fixed') {

            searchCustomer(_index);
        }
    }
});
$('#sample_1_previous').click(function () {

    var _index = parseInt($('#hdnPageNoExsitingCustomer').val()) - 1;
    if (_index > 0) {
        _lock = true;
        //getListExistingCustomer(_index);
        // searchCustomer(_index);

        if ($('#divSearchbyExistingCustomer').css('display') == 'block') {
            getListExistingCustomer(_index);
        }
        else {

            searchCustomer(_index);
        }
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
$('#txtOrderNumber').focusout(function () {

    checkDuplicateOrderNumber($('#txtOrderNumber').val());
});
// Sagar - Tab 4 -- file upload
$("#file_upload").uploadify({
    'swf': '/uploadify.swf',
    'fileSizeLimit': '20MB',
    'uploader': '/Handler1.ashx',
    'cancelImg': 'cancel.png',
    'buttonText': 'Select Files',
    'fileDesc': 'Image Files',
    'fileExt': '*.jpg;*.jpeg;*.gif;*.png',
    'multi': true,
    'auto': true,
    'scriptData': { 'Temp': '1234' },
    'formData': { 'repairNumber': $('#hdnRepairNumber').val() },
    'onQueueComplete': function (queueData) {
        // bindDocumentsGrid('1');
    }
});
$('#sample_1_nextFileUpload').click(function () {
    if (_lock) {
        var _index = parseInt($('#hdnFileUploadPageNo').val()) + 1;
        bindDocumentsGrid(_index);
    }
});
$('#sample_1_previousFileUpload').click(function () {

    var _index = parseInt($('#hdnFileUploadPageNo').val()) - 1;
    if (_index > 0) {
        _lock = true;
        bindDocumentsGrid(_index);
    }
});
function DeleteAttachment(AttachmentId) {
    var d = { AttachmentId: AttachmentId };
    if (confirm("Do you want to delete?")) {
        $.ajax({
            url: siteUrl + 'Repair/DeleteAttachment',
            headers: { "requestType": "client" },
            type: 'POST',
            data: JSON.stringify(d),
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                bindDocumentsGrid(1);
                showMessage('Success', 'success', "File deleted successfully!", 'toast-top-right');
            },
            error: function (_data) {
            }
        });
    }
    return false;
}
function DownloadAttachment(fileName) {
    window.location.href = "~/FileUpload/" + fileName;
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)", "i"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function bindDocumentsGrid(pageNo) {


    _lock = true;
    $('#hdnFileUploadPageNo').val(pageNo);
    var model = {
        repairNumber: $('#hdnRepairNumber').val(),
        pageNo: pageNo,
        sortColumn: _sortColumn,
        sortOrder: _sortOrder
    };
    $('#tbodyDetails').html('<tr><td colspan="5">Loading....</td></tr>');
    $('#tbodyDetailsRepair').html('<tr><td colspan="5">Loading....</td></tr>');
    $.ajax({
        url: siteUrl + 'Repair/getUploadedFilesList',
        headers: { "requestType": "client" },
        type: 'POST',
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            $('#tbodyDetailsFileUpload').html('');
            $('#tbodyTemplateFileUpload').tmpl(data).appendTo('#tbodyDetailsFileUpload');
            if (data.length == 0) {
                setPagingDetailFileUpload(data.length, 0, pageNo);
            }
            else {
                setPagingDetailFileUpload(data.length, data[0].TotalCount, pageNo);
            }
        },
        error: function (_data) {
            $('#tbodyDetailsRepair').html('<tr><td colspan="6">error in request</td></tr>');
        }
    });
}
function setPagingDetailFileUpload(noOfrows, Total, pageNo) {
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
    $('#sample_1_infoFileUpload').html('Showing ' + _firstRecord + ' to ' + noOfrows + ' of ' + Total + ' entries');
}
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
function CalculateRepairTotalAmount() {
    var total = 0;
    $(".Tab3Amount:enabled").each(function (index, box) {
        if ($(box).val() != '') {
            total += parseFloat($(box).val().replace(',', ''), 10);
        }
    });
    if (total != 0) {
        if ($('#chkTab3Tax').is(":checked")) {
            FetchTaxByStateId();
        }
    }

    $('#txtTab3RepairPriceSubTotal').val(total);
    var grandTotal = 0;
    grandTotal = total;
    if ($('#txtTab3RepairPriceShipping').val() != '') {
        grandTotal = grandTotal + parseFloat($('#txtTab3RepairPriceShipping').val().replace(',', ''));
    }
    if ($('#txtTab3RepairPriceTax').val() != '') {
        grandTotal = grandTotal + parseFloat($('#txtTab3RepairPriceTax').val().replace(',', ''));
    }
    // grandTotal = total + parseFloat($('#txtTab3RepairPriceShipping').val()) + parseFloat($('#txtTab3RepairPriceTax').val());
    $('#txtTab3RepairPriceTotal').val(grandTotal);
}
$(document).on('keyup', '.Tab3Amount', function () {

    CalculateRepairTotalAmount();
});
$(document).on('change', '.chkTab3DisableEnableAmount', function () {
    if ($(this).is(":checked")) {
        $(this).parents('.divTab3Repair').find('.Tab3Amount').attr("disabled", false);
    }
    else {

        $(this).parents('.divTab3Repair').find('.Tab3Amount').attr("disabled", "disabled");
    }
    CalculateRepairTotalAmount();
});
//$(document).on('change', '#chkTab3RepairCheck1', function () {
//    if ($(this).is(":checked")) {
//        $('#txtTab3RepairPrice1').attr("disabled", false);
//        $('#txtTab3RepairPrice2').attr("disabled", false);
//    }
//    else {

//        $('#txtTab3RepairPrice1').attr("disabled", "disabled");
//        $('#txtTab3RepairPrice2').attr("disabled", "disabled");
//    }
//    CalculateRepairTotalAmount();
//});



$('#drpPrimaryCites').change(function () {

    FetchTaxByStateId();

});

$(document).on('change', '#chkTab3Tax', function () {
    if ($(this).is(":checked")) {
        FetchTaxByStateId();
    }
    else {
        $('#txtTab3RepairPriceTax').val("0.00");
    }
    CalculateRepairTotalAmount();
});
$(document).on('click', '#btnFileUpload', function () {


    //// Checking whether FormData is available in browser  
    //if (window.FormData !== undefined) {

    //    var fileUpload = $("#fileupload").get(0);
    //    var files = fileUpload.files;

    //    // Create FormData object  
    //    var fileData = new FormData();

    //    // Looping over all files and add it to FormData object  
    //    for (var i = 0; i < files.length; i++) {
    //        fileData.append(files[i].name, files[i]);
    //    }

    //    // Adding one more key to FormData object  
    //    fileData.append('RepairNumber', $('#hdnRepairNumber').val());

    //    fileData.append("Filedata", files[0]);
    //    fileData.append("UpdatedBy", files[0]);




    //    jQuery.ajax({
    //        url: siteUrl + "Repair/UploadFiles",
    //        type: "POST",
    //        dataType: 'json',

    //        headers: { "requestType": "client" },
    //        contentType: false, // Not to set any content header  
    //       // processData: false, // Not to process data 
    //        async: false,
    //        //data: '{}',
    //        success: function (result) {
    //            alert(result);
    //            bindDocumentsGrid('1');
    //            $("#fileupload").val('');
    //            showMessage('Success', 'success', "File uploaded successfully!", 'toast-top-right');
    //        },
    //        error: function (err) {
    //            alert(err.statusText);
    //        }
    //    });
    //} else {
    //    alert("FormData is not supported.");
    //}

    var fileUpload = $("#fileupload").get(0);
    var files = fileUpload.files;
    var test = new FormData();
    test.append('RepairNumber', $('#hdnRepairNumber').val());
    test.append("Filedata", files[0]);
    //test.append("UpdatedBy", files[0]);
    $.ajax({
        url: siteUrl + "Handler1.ashx",
        //url: siteUrl + "Repair/UploadFiles",
        //headers: { "requestType": "client" },
        type: "POST",
        contentType: false,
        processData: false,
        data: test,
        // dataType: "json",
        success: function (result) {
            //alert(result);
            bindDocumentsGrid('1');
            $("#fileupload").val('');
            showMessage('Success', 'success', "File uploaded successfully!", 'toast-top-right');
        },
        error: function (err) {
            showMessage('Oops', 'error', err.statusText, 'toast-top-right');
        }
    });
});




function FetchTaxByStateId() {
    var total = 0;
    $(".Tab3Amount:enabled").each(function (index, box) {
        if ($(box).val() != '') {
            total += parseFloat($(box).val(), 10);
        }
    });
    if ($('#drpPrimaryCites option:selected').text() == "Please select" && $('#txtPrimaryCites').val() == "") {
        $('#txtTab3RepairPriceTax').val("0.00");
        showMessage('Oops', 'error', 'Please select primary city from customer tab!', 'toast-top-right');
        $('#chkTab3Tax').parent().removeClass('checked');
    }
    else if (total != 0) {
        var CityId = $('#drpPrimaryCites').val();

        if (CityId == "" || CityId==undefined|| CityId == null || CityId=="Selected") {
            CityId = 0;
        }

        jQuery.ajax({
            url: siteUrl + 'Repair/GetTaxByStateId',
            dataType: 'json',
            headers: { "requestType": "client" },
            data: { "id": CityId },
            contentType: 'application/json;charset=utf-8',
            async: false,
            success: function (_data) {
                if (_data != null) {
                    $('#txtTab3RepairPriceTax').val(_data.taxRate);
                }
                else {
                    $('#txtTab3RepairPriceTax').val("0.00");
                }
            },
            error: function (_data) {
                showMessage('Oops', 'error', _data, 'toast-top-right');
            }
        });
    }
    else {

    }
}
//Sagar- Tab 3 Save Repair function
function addRepair() {
    if (confirm("Are you sure?") == true) {
        addCustomer(0);
        var CheckTaskDescriptionOK = true;
        $(".Tab3Amount:enabled").each(function (index, box) {
            $(box).parents('.divTab3Repair').find('textarea').css('border', '');
            if ($(box).val() != '') {
                if ($(box).parents('.divTab3Repair').find('textarea').val() == '') {
                    CheckTaskDescriptionOK = false;
                    validateEmpty(document.getElementById($(box).parents('.divTab3Repair').find('textarea').attr('id')), '');
                }
            }
        });
        if (CheckTaskDescriptionOK == false) {
            showMessage('Oops', 'error', 'Please fill selected task description first!', 'toast-top-right');
        }
        else {
            var dt = new Date();
            var task1Decline1, task1Decline2, task1Decline3, task1Decline4, task1Decline5, task1Decline6, task1Decline7, taxAble = '0';
            if ($('#chkTab3RepairCheck1').is(":checked")) {
                task1Decline1 = '1';
            }
            if ($('#chkTab3RepairCheck2').is(":checked")) {
                task1Decline2 = '1';
            }
            if ($('#chkTab3RepairCheck3').is(":checked")) {
                task1Decline3 = '1';
            }
            if ($('#chkTab3RepairCheck4').is(":checked")) {
                task1Decline4 = '1';
            }
            if ($('#chkTab3RepairCheck5').is(":checked")) {
                task1Decline5 = '1';
            }
            if ($('#chkTab3RepairCheck6').is(":checked")) {
                task1Decline6 = '1';
            }
            if ($('#chkTab3RepairCheck7').is(":checked")) {
                task1Decline7 = '1';
            }
            if ($('#chkTab3Tax').is(":checked")) {
                taxAble = '1';
            }
            var model =
               {
                   customerNumber: $('#hdnCustomerNumber').val(),
                   repairNumber: $('#hdnRepairNumber').val(),
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
                   OrderNumber: $('#txtOrderNumber').val(),
                   // tab 3
                   task1Description: $('#txtTab3RepairNumber').val(),
                   task1Price: $('#txtTab3RepairPrice1').val(),
                   task1Decline: task1Decline1,
                   task2Description: $('#txtTab3RepairNumber2').val(),
                   task2Price: $('#txtTab3RepairPrice2').val(),
                   task2Decline: task1Decline2,
                   task3Description: $('#txtTab3RepairNumber3').val(),
                   task3Price: $('#txtTab3RepairPrice3').val(),
                   task3Decline: task1Decline3,
                   task4Description: $('#txtTab3RepairNumber4').val(),
                   task4Price: $('#txtTab3RepairPrice4').val(),
                   task4Decline: task1Decline4,
                   task5Description: $('#txtTab3RepairNumber5').val(),
                   task5Price: $('#txtTab3RepairPrice5').val(),
                   task5Decline: task1Decline5,
                   task6Description: $('#txtTab3RepairNumber6').val(),
                   task6Price: $('#txtTab3RepairPrice6').val(),
                   task6Decline: task1Decline6,
                   task7Description: $('#txtTab3RepairNumber7').val(),
                   task7Price: $('#txtTab3RepairPrice7').val(),
                   task7Decline: task1Decline7,
                   shipping: $('#txtTab3RepairPriceShipping').val(),
                   subtotal: $('#txtTab3RepairPriceSubTotal').val(),
                   tax: $('#txtTab3RepairPriceTax').val(),
                   taxable: taxAble,
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
                        //showMessage('Success', 'success', "Item added successfully!", 'toast-top-right');
                        showMessage('Success', 'success', "Record saved successfully!", 'toast-top-right');
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
    }
    else { return false; }
}
function FillRepairData(_data) {    
    if (_data.task1Decline == null) {       
        $('#chkTab3RepairCheck1').prop('checked', false);
        $('#chkTab3RepairCheck1').removeAttr("checked");
        $('#chkTab3RepairCheck1').parent().removeClass('checked');
        $('#txtTab3RepairPrice1').attr('disabled', 'disabled');
        $('#txtTab3RepairPrice2').attr('disabled', 'disabled');
    }
    if (_data.task1Decline == null) {
        // $('#chkTab3RepairCheck2').prop('checked', false);
        //   $('#txtTab3RepairPrice2').attr('disabled', 'disabled');
    }
    if (_data.task3Decline == null) {
        $('#chkTab3RepairCheck3').prop('checked', false);
        $('#chkTab3RepairCheck3').parent().removeClass('checked');
        $('#txtTab3RepairPrice3').attr('disabled', 'disabled');
    }
    if (_data.task4Decline == null) {
        $('#chkTab3RepairCheck4').prop('checked', false);
        $('#chkTab3RepairCheck4').parent().removeClass('checked');
        $('#txtTab3RepairPrice4').attr('disabled', 'disabled');
    }
    if (_data.task5Decline == null) {
        $('#chkTab3RepairCheck5').prop('checked', false);
        $('#chkTab3RepairCheck5').parent().removeClass('checked');
        $('#txtTab3RepairPrice5').attr('disabled', 'disabled');
    }
    if (_data.task6Decline == null) {
        $('#chkTab3RepairCheck6').prop('checked', false);
        $('#chkTab3RepairCheck6').parent().removeClass('checked');
        $('#txtTab3RepairPrice6').attr('disabled', 'disabled');
    }
    if (_data.task7Decline == null) {
        $('#chkTab3RepairCheck7').prop('checked', false);
        $('#chkTab3RepairCheck7').parent().removeClass('checked');
        $('#txtTab3RepairPrice7').attr('disabled', 'disabled');
    }

    $('#txtTab3RepairNumber').val(_data.task1Description),
    $('#txtTab3RepairPrice1').val(_data.task1Price);
    // $('#chkTab3RepairCheck1').is(":checked"),


    $('#txtTab3RepairNumber2').val(_data.task2Description);


    $('#txtTab3RepairPrice2').val(_data.task2Price);
    //task2Decline: $('#chkTab3RepairCheck2').is(":checked"),
    $('#txtTab3RepairNumber3').val(_data.task3Description);
    $('#txtTab3RepairPrice3').val(_data.task3Price);
    //task3Decline: $('#chkTab3RepairCheck3').is(":checked"),
    $('#txtTab3RepairNumber4').val(_data.task4Description);
    $('#txtTab3RepairPrice4').val(_data.task4Price);
    //task4Decline: $('#chkTab3RepairCheck4').is(":checked"),
    $('#txtTab3RepairNumber5').val(_data.task5Description);
    $('#txtTab3RepairPrice5').val(_data.task5Price);
    //task5Decline: $('#chkTab3RepairCheck5').is(":checked"),
    $('#txtTab3RepairNumber6').val(_data.task6Description);
    $('#txtTab3RepairPrice6').val(_data.task6Price);
    // task6Decline: $('#chkTab3RepairCheck6').is(":checked"),
    $('#txtTab3RepairNumber7').val(_data.task7Description);
    $('#txtTab3RepairPrice7').val(_data.task7Price);
    //task7Decline: $('#chkTab3RepairCheck7').is(":checked"),
    $('#txtTab3RepairPriceShipping').val(_data.shipping);
    $('#txtTab3RepairPriceSubTotal').val(_data.subtotal);

    if (_data.tax != null && _data.tax != "0.00" && _data.tax != "") {
        $('#chkTab3Tax').prop('checked', true);
    }
    $('#txtTab3RepairPriceTax').val(_data.tax);
    //taxable: $('#chkTab3Tax').is(":checked"),
    $('#txtTab3RepairPriceTotal').val(_data.total);
    $('#txtRepairComments').val(_data.repairComments);
    $('#txtTab3NotesToCustomer').val(_data.notesToCustomer);
    $('#txtTab3NotesToFactory').val(_data.notesToFactory);
    $('#txtTab3LetterToCustomer').val(_data.letterToCustomer);
    $('#txtTab3ShippingCustomerValue').val(_data.shippingToCustomerValue);
    $('#txtTab3ShippingCustomerInsurance').val(_data.shippingToCustomerInsurance);
    $('#txtTab3ShippingCustomerSalesPerson').val(_data.shippingToCustomerSalesperson);
    $('#drpTab3ToCustomerDestinationType').val(_data.shippingToCustomerDestinationType);
    $('#txtTab3ShippingCustomerCarrier').val(_data.shippingToCustomerCarrier);
    $('#txtTab3ShippingCustomerDestinationType').val(_data.shippingToCustomerShippingType);
    $('#txtTab3ToCustomeMailingDate').val(parseJsonDate(_data.shippingToCustomerMailingDate)),
    $('#txtTab3ToCustomerExpectedArrival').val(parseJsonDate(_data.shippingToCustomerExpectedArrivalDate));
    $('#txtTab3ShippingSupplierValue').val(_data.shippingToSupplierValue);
    $('#txtTab3ShippingSupplierInsurance').val(_data.shippingToSupplierInsurance);
    $('#txtTab3ShippingSupplierSalesPerson').val(_data.shippingToSupplierSalesperson);
    $('#drpTab3ToSupplierDestinationType').val(_data.shippingToSupplierDestinationType);
    $('#txtTab3ShippingSupplierCarrier').val(_data.shippingToSupplierCarrier);
    $('#txtTab3ShippingSupplierDestinationType').val(_data.shippingToSupplierShippingType);
    $('#txtTab3ToSupplierMailingDate').val(parseJsonDate(_data.shippingToSupplierMailingDate));
    $('#txtTab3ToSupplierExpectedArrival').val(parseJsonDate(_data.shippingToSupplierExpectedArrivalDate));
    $('#txtTab3SparePartQty1').val(_data.sparePartsQuantity1);
    $('#txtTab3SparePart1').val(_data.sparePartsPartNumber1);
    $('#txtTab3SparePartDescription1').val(_data.sparePartsDescription1);
    $('#txtTab3SparePartQty2').val(_data.sparePartsQuantity2);
    $('#txtTab3SparePart2').val(_data.sparePartsPartNumber2);
    $('#txtTab3SparePartDescription2').val(_data.sparePartsDescription2);
    $('#txtTab3SparePartQty3').val(_data.sparePartsQuantity3);
    $('#txtTab3SparePart3').val(_data.sparePartsPartNumber3);
    $('#txtTab3SparePartDescription3').val(_data.sparePartsDescription3);
    $('#txtTab3SparePartQty4').val(_data.sparePartsQuantity4);
    $('#txtTab3SparePart4').val(_data.sparePartsPartNumber4);
    $('#txtTab3SparePartDescription4').val(_data.sparePartsDescription4);
    $('#txtTab3SparePartQty5').val(_data.sparePartsQuantity5);
    $('#txtTab3SparePart5').val(_data.sparePartsPartNumber5);
    $('#txtTab3SparePartDescription5').val(_data.sparePartsDescription5);
    $('#txtTab3SparePartQty6').val(_data.sparePartsQuantity6);
    $('#txtTab3SparePart6').val(_data.sparePartsPartNumber6);
    $('#txtTab3SparePartDescription6').val(_data.sparePartsDescription6);
    $('#txtTab3SparePartQty7').val(_data.sparePartsQuantity7);
    $('#txtTab3SparePart7').val(_data.sparePartsPartNumber7);
    $('#txtTab3SparePartDescription7').val(_data.sparePartsDescription7);
    $('#txtTab3SparePartQty8').val(_data.sparePartsQuantity8);
    $('#txtTab3SparePart8').val(_data.sparePartsPartNumber8);
    $('#txtTab3SparePartDescription8').val(_data.sparePartsDescription8);
    $('#txtTab3SparePartOrderNotes').val(_data.sparePartsOrderNotes);
    $('#txtTab3StrapOrderMaterialType').val(_data.strapMaterialType);
    $('#txtTab3StrapOrderColor').val(_data.strapColor);
    $('#txtTab3StrapOrderWidth').val(_data.strapWidth);
    $('#txtTab3StrapOrderBuckle').val(_data.strapBuckle);
    $('#txtTab3StrapOrderLength').val(_data.strapLength);
    $('#txtTab3StrapOrderTextureType').val(_data.strapTextureType);
    $('#txtTab3StrapOrderLusterType').val(_data.strapLusterType);
    $('#txtTab3StrapOrderStitchType').val(_data.strapStitchType);
    $('#txtTab3StrapOrderThicknessType').val(_data.strapThicknessType);
    $('#txtTab3StrapOrderAccessories').val(_data.strapAccessories);
    $('#txtTab3StrapOrderLengthSO6').val(_data.strapSpecOrd600);
    $('#txtTab3StrapOrderLengthSO12').val(_data.strapSpecOrd1200);
    $('#txtTab3StrapOrderNotes').val(_data.strapOrderNotes);
}
function FillDocStatusData(_data) {
    if (_data.dateReceivedByMail != null) {
        $('#docStatusReceivedByMail').html(new Date(parseInt(_data.dateReceivedByMail.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateReceivedByMessenger != null) {
        $('#docStatusReceivedByMessenger').html(new Date(parseInt(_data.dateReceivedByMessenger.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateEstimateRequestStatus != null) {
        $('#docStatusEstimateRequestStatus').html(new Date(parseInt(_data.dateEstimateRequestStatus.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateServiceRequest != null) {
        $('#docStatusServiceRequest').html(new Date(parseInt(_data.dateServiceRequest.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateServiceRequestStatus != null) {
        $('#docStatusServiceRequestStatus').html(new Date(parseInt(_data.dateServiceRequestStatus.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateAuthToFactoryCharge != null) {
        $('#docStatusAuthToFactory').html(new Date(parseInt(_data.dateAuthToFactoryCharge.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateAuthToFactoryNoCharge != null) {
        $('#docStatusAuthToFactoryNC').html(new Date(parseInt(_data.dateAuthToFactoryNoCharge.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateReminderToFactory != null) {
        $('#docStatusReminderToFactory').html(new Date(parseInt(_data.dateReminderToFactory.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateProceedToFactory != null) {
        $('#docStatusProceedToFactory').html(new Date(parseInt(_data.dateProceedToFactory.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateRepairOffer != null) {
        $('#docStatusRepairOffer').html(new Date(parseInt(_data.dateRepairOffer.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateRepairOfferReminder != null) {
        $('#docStatusRepairOfferReminder').html(new Date(parseInt(_data.dateRepairOfferReminder.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateReturnUndone != null) {
        $('#docStatusReturnUndone').html(new Date(parseInt(_data.dateReturnUndone.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateProceedWithService != null) {
        $('#docStatusProceedWithService').html(new Date(parseInt(_data.dateProceedWithService.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateRepairStatus != null) {
        $('#docStatusRepairStatus').html(new Date(parseInt(_data.dateRepairStatus.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateRepairDelay != null) {
        $('#docStatusRepairDelay').html(new Date(parseInt(_data.dateRepairDelay.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.datePickupNotification != null) {
        $('#docStatusPickupNotification').html(new Date(parseInt(_data.datePickupNotification.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.datePickedUp != null) {
        $('#docStatusPickedUp').html(new Date(parseInt(_data.datePickedUp.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
    if (_data.dateShipped != null) {
        $('#docStatusShipped').html(new Date(parseInt(_data.dateShipped.replace("/Date(", "").replace(")/", ""), 10)).toDateString() || "N/A");
    }
}

function validateEmailIdsOfPdfPopUp() {
    var reason = "";
    if ($('#txtBoxEmailPoupPrimary').val() != '' && $('#chkBoxEmailPoupPrimary').is(':checked') == true) {
        reason += validateEmail(document.getElementById('txtBoxEmailPoupPrimary'), 'primary');
    }
    if ($('#txtBoxEmailPoupSecondary').val() != '' && $('#chkBoxEmailPoupSecondary').is(':checked') == true) {
        reason += validateEmail(document.getElementById('txtBoxEmailPoupSecondary'), 'alternate');
    }
    if ($('#txtBoxEmailPoupThird').val() != '' && $('#chkBoxEmailPoupThird').is(':checked') == true) {
        reason += validateEmail(document.getElementById('txtBoxEmailPoupThird'), 'third');
    }

    if (reason != "") {
        //alert("All fields marked with an asterisk are required.:\n" + reason);
        return false;
    }

    return true;
}

function showSampleTask(_taskID) {
    $('#hdnClickId').val(_taskID);
    var $modal = $('#responsiveTask');
    $modal.modal();

    //responsiveTask
}



function showSampleTaskOptional(_taskID) {
    $('#hdnClickIdOptional').val(_taskID);
    var $modal = $('#responsiveTaskOptional');
    $modal.modal();

    //responsiveTask
}



function selectTask() {
    if ($('#drpSampleTask :selected').text() != "Select") {
        $('#' + $('#hdnClickId').val()).val($('#drpSampleTask :selected').text());
        $('#responsiveTask').modal('hide');
    }

}



function selectTaskOptional() {
    if ($('#drpSampleTaskOptional :selected').text() != "Select") {
        $('#' + $('#hdnClickIdOptional').val()).val($('#drpSampleTaskOptional :selected').text());
        $('#responsiveTaskOptional').modal('hide');
    }

}



function selectTaskForTable(Text) {
    //if ($('#drpSampleTask :selected').text() != "Select") {
    $('#' + $('#hdnClickId').val()).val(Text);
        $('#responsiveTask').modal('hide');
    //}

}



function selectTaskForTableOptional(Text) {
    //if ($('#drpSampleTask :selected').text() != "Select") {
    $('#' + $('#hdnClickIdOptional').val()).val(Text);
    $('#responsiveTaskOptional').modal('hide');
    //}

}


/// New functions 26 October 2016


function searchCustomer(pageNo) {

    _lock = true;
    _lockCustomerSearch = true;
    _SearchType = 'fixed';



    $('#hdnPageNo').val(pageNo);
    $('#hdnPageNoExsitingCustomer').val(pageNo);


    var _SearchFields = '';

    var _SearchValues = '';

    var _SearchPopupValues = '';


    $.each($(".searchValue"), function () {
        if ($(this).val() != '') {
            if ($(this).val() != '0') {
                _SearchFields += $(this).attr('fieldName') + '|';
                _SearchValues += $(this).val() + '|';

                _SearchPopupValues += $(this).val() + ', ';
            }
        }
    });






    //  $('#pidSeacrhValues').html(_SearchPopupValues);



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



    $('#txtSearchbyExistingCustomer').val('');
    $('#divExistingCustomerList').hide();
    $('#tbodyTemplateExsitingCustomers').html('');
    var $modal = $('#divSearchbyExistingCustomer');
    // getListExistingCustomer(1);
    $('#divExistingCustomerList').show();

    $modal.modal();
    return;


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




function SaveNewStateName(openType) {

    var CountryId = '0';
    var StateName = '';
    if (openType == '1') {
        CountryId = $('#drpPrimaryCountry').val();
        StateName = $('#txtNewPrimaryStateAdd').val();
    }
    if (openType == '2') {
        CountryId = $('#drpAltCountry').val();
        StateName = $('#txtNewAlternateStateAdd').val();
    }
    if (openType == '3') {
        CountryId = $('#drpthirdCountry').val();
        StateName = $('#txtNewThirdStateAdd').val();
    }
    if (CountryId == 0) {
        showMessage('Oops', 'error', "Please select country first", 'toast-top-right');
    }
    else if (StateName == '') {
        showMessage('Oops', 'error', "state name is requried", 'toast-top-right');
    }
    else {

      //  alert(CountryId);
        //var d = 
        jQuery.ajax({
            url: siteUrl + 'Repair/SaveNewState',
            dataType: 'json',
            headers: { "requestType": "client" },
            // data: { "CountryId": CountryId, StateName: StateName },

            data: { CountryId: CountryId, StateName: StateName },
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                if (data == '0') {
                    showMessage('Oops', 'error', "State name is already exists", 'toast-top-right');
                }
                if (data == '1')
                {
                    showMessage('Success', 'success', "Record saved successfully!", 'toast-top-right');
                }

                if (openType == '1') {
                    $('#txtNewPrimaryStateAdd').val('');
                    $("#drpPrimaryCountry").change();

                }
                if (openType == '2') {
                    $('#txtNewAlternateStateAdd').val('');
                    $("#drpAltCountry").change();
                }
                if (openType == '3') {
                    $('#txtNewThirdStateAdd').val('');
                    $("#drpthirdCountry").change();

                }
            },
            error: function (data) {
            }
        });
    }

}







function SaveNewCityName(openType) {


    

    var StateId = '0';
    var County = '';
    var CityName = '';
    var Zipcode = '';

    if (openType == '1') {
        StateId = $('#drpPrimaryStates').val();
        County = $('#txtNewPrimaryCountyAdd').val();
        CityName = $('#txtNewPrimaryCityAdd').val();
        Zipcode = $('#txtNewPrimaryZipcodeAdd').val();
    }
    if (openType == '2') {
        StateId = $('#drpAltStates').val();
        County = $('#txtNewAlternateCountyAdd').val();
        CityName = $('#txtNewAlternateCityAdd').val();
        Zipcode = $('#txtNewAlternateZipcodeAdd').val();
    }
    if (openType == '3') {
        StateId = $('#drpthirdStates').val();
        County = $('#txtNewThirdCountyAdd').val();
        CityName = $('#txtNewThirdCityAdd').val();
        Zipcode = $('#txtNewThirdZipcodeAdd').val();
    }
    if (StateId == 0) {
        showMessage('Oops', 'error', "Please select state first", 'toast-top-right');
    }
    else if (County == '')
    {
        showMessage('Oops', 'error', "county is requried", 'toast-top-right');
    }
    else if (CityName == '') {
        showMessage('Oops', 'error', "city name is requried", 'toast-top-right');
    }

    else if (Zipcode == '') {
        showMessage('Oops', 'error', "zip code is requried", 'toast-top-right');
    }


    else {

        //  alert(CountryId);
        //var d = 
        jQuery.ajax({
            url: siteUrl + 'Repair/SaveNewCity',
            dataType: 'json',
            headers: { "requestType": "client" },
            // data: { "CountryId": CountryId, StateName: StateName },

            data: { StateId: StateId, County: County, CityName: CityName, ZipCode: Zipcode },
            contentType: 'application/json;charset=utf-8',
            success: function (data) {
                if (data == '0') {
                    showMessage('Oops', 'error', "city name is already exists. Please select from dropdown.", 'toast-top-right');
                }
                if (data == '01') {
                    showMessage('Oops', 'error', "county is already exists", 'toast-top-right');
                }
                if (data == '02') {
                    showMessage('Oops', 'error', "zip code is already exists", 'toast-top-right');
                }
                if (data == '1') {
                    showMessage('Success', 'success', "Record saved successfully!", 'toast-top-right');


                    if (openType == '1') {
                        $('#txtNewPrimaryCountyAdd').val('');
                        $('#txtNewPrimaryCityAdd').val('');
                        $('#txtNewPrimaryZipcodeAdd').val('');
                        $('#drpPrimaryStates').change();

                    }
                    if (openType == '2') {
                        $('#txtNewAlternateCountyAdd').val('');
                        $('#txtNewAlternateCityAdd').val('');
                        $('#txtNewAlternateZipcodeAdd').val('');
                        $('#drpAltStates').change();
                    }
                    if (openType == '3') {
                        $('#txtNewThirdCountyAdd').val('');
                        $('#txtNewThirdCityAdd').val('');
                        $('#txtNewThirdZipcodeAdd').val('');
                        $('#drpthirdStates').change();
                    }
                }



               


            },
            error: function (data) {
            }
        });

        //$.ajax({
        //    url: siteUrl + 'Repair/SaveNewState',
        //    headers: { "requestType": "client" },
        //    //type: 'POST',
        //    data: { "CountryId": CountryId, "StateName": StateName },
        //    contentType: 'application/json;charset=utf-8',
        //    success: function (data) {

        //    },
        //    error: function (_data) {
        //    }
        //});
    }

}


function FillCountryStateCityByZipCode(openType) {

    var ZipCode = '';
    if (openType == '1') {
        ZipCode = $('#txtStateZip').val();
    }
    if (openType == '2') {
        ZipCode = $('#txtAltStateZip').val();
    }
    if (openType == '3') {
        ZipCode = $('#txtthirdStateZip').val();
    }
    jQuery.ajax({
        url: siteUrl + 'Repair/GetDataByZipcode',
        dataType: 'json',
        headers: { "requestType": "client" },
        data: { ZipCode: ZipCode },
        contentType: 'application/json;charset=utf-8',
        success: function (data) {

            if (data.CountryId == null) {
                showMessage('Oops', 'error', "Zip code not found in records", 'toast-top-right');
            }
            else {
                if (openType == '1') {
                    $('#drpPrimaryCountry').val(data.CountryId);
                    $('#drpPrimaryCountry').change();
                    TempValues.PriState = data.StateId;
                    TempValues.PriCity = data.CityId;
                }
                if (openType == '2') {
                    $('#drpAltCountry').val(data.CountryId);
                    $('#drpAltCountry').change();
                    TempValues.AltState = data.StateId;
                    TempValues.AltCity = data.CityId;
                }
                if (openType == '3') {
                    $('#drpthirdCountry').val(data.CountryId);
                    $('#drpthirdCountry').change();
                    TempValues.thirdState = data.StateId;
                    TempValues.thirdCity = data.CityId;
                }
            }
        },
        error: function (data) {
        }
    });
}