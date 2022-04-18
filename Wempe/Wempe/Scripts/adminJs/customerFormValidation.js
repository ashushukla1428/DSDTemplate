function validateCustomer() {
    var reason = "";
    reason += validateEmpty(document.getElementById('txtFirstName'), 'Customer first name is required !');
    //  reason += validateEmpty(document.getElementById('txtFirstName1'), 'Assistant first name required !');
    reason += validateEmpty(document.getElementById('txtLast'), 'Customer last name is required !');
    // reason += validateEmpty(document.getElementById('txtLast1'), 'Assistant last name required !');

    reason += validateDropdown(document.getElementById('drpPrimaryCountry'), 'Please select primary country!');
    //reason += validateDropdown(document.getElementById('drpAltCountry'), 'Please select alternate country!');
    //, 'Primary Email Address required!'//, 'Alternate Email Address required !'
    reason += validateEmail(document.getElementById('txtEmail'), 'primary');
    reason += validateEmail(document.getElementById('txtAltEmail'), 'alternate');
    
    //reason += validateDropdown(document.getElementById('drpPrimaryStates'), 'Please select state!');
    //PRIMARY ADDRESS
   

    reason += validateNumericValue(document.getElementById('txtOfficePhone'), 'Only Numeric value is allowed. (Primary Phone)');
    reason += validateNumericValue(document.getElementById('txtHomePhone'), 'Only Numeric value is allowed. (Primary Home Phone)');
    reason += validateNumericValue(document.getElementById('txtCell'), 'Only Numeric value is allowed. (Primary Cell)');
    reason += validateNumericValue(document.getElementById('txtFAX'), 'Only Numeric value is allowed. (Primary Fax)');
    reason += validateNumericValue(document.getElementById('txtASSISTANTPhone'), 'Only Numeric value is allowed. (Primary assistant phone number)');




    reason += validateNumericValue(document.getElementById('txtAltOfficePhone'), 'Only Numeric value is allowed. (Alternate Phone)');
    reason += validateNumericValue(document.getElementById('txtAltHomePhone'), 'Only Numeric value is allowed. (Alternate Home Phone)');
    reason += validateNumericValue(document.getElementById('txtAltCell'), 'Only Numeric value is allowed. (Alternate Cell)');
    reason += validateNumericValue(document.getElementById('txtAltFAX'), 'Only Numeric value is allowed. (Alternate Fax)');
    reason += validateNumericValue(document.getElementById('txtAltAssistantPhone'), 'Only Numeric value is allowed. (Alternate assistant phone number)');



    reason += validateNumericValue(document.getElementById('txtthirdOfficePhone'), 'Only Numeric value is allowed. (Third address Phone)');
    reason += validateNumericValue(document.getElementById('txtthirdHomePhone'), 'Only Numeric value is allowed. (Third address Home Phone)');
    reason += validateNumericValue(document.getElementById('txtthirdCell'), 'Only Numeric value is allowed. (Third address Cell)');
    reason += validateNumericValue(document.getElementById('txtthirdFAX'), 'Only Numeric value is allowed. (Third address Fax)');
    reason += validateNumericValue(document.getElementById('txtthirdAssistantPhone'), 'Only Numeric is value allowed. (Third address assistant phone number)');

  


    reason += validateUS4BoxPhone('txtCreditCard1', 'txtCreditCard2', 'txtCreditCard3', 'txtCreditCard4', 'Please enter a valid primary credit card.'); //validatePrimaryCreditCard();
    reason += validateUS4BoxPhone('txtAltCreditCard1', 'txtAltCreditCard2', 'txtAltCreditCard3', 'txtAltCreditCard4', 'Please enter a valid alternate credit card.'); //validateAlternateCreditCard();
    reason += validateUS4BoxPhone('txtthirdCreditCard1', 'txtthirdCreditCard2', 'txtthirdCreditCard3', 'txtthirdCreditCard4', 'Please enter a valid third address credit card.'); //validateAlternateCreditCard();

    reason += validateCreditCardEXP('txtCardExpDay', 'txtCardExpYear', 'Please enter a valid primary credit card expiry date.');//validatePrimaryCreditCardEXP();
    reason += validateCreditCardlength('txtCreditCard1','txtCreditCard2','txtCreditCard3','txtCreditCard4','Please enter a valid primary credit card number.');//.validatePrimaryCreditCardlength();

    reason += validateCreditCardEXP('txtAltCardExpDay', 'txtAltCardExpYear', 'Please enter a valid alternate credit card expiry date.');
    reason += validateCreditCardlength('txtAltCreditCard1', 'txtAltCreditCard2', 'txtAltCreditCard3', 'txtAltCreditCard4', 'Please enter a valid alternate credit card number.');

    reason += validateCreditCardEXP('txtthirdCardExpDay', 'txtthirdCardExpYear', 'Please enter a valid third address credit card expiry date.');
    reason += validateCreditCardlength('txtthirdCreditCard1', 'txtthirdCreditCard2', 'txtthirdCreditCard3', 'txtthirdCreditCard4', 'Please enter a valid third credit card number.');


    if (reason != "") {
        //alert("All fields marked with an asterisk are required.:\n" + reason);
        alert(reason);
        return false;
    }
    return true;
}