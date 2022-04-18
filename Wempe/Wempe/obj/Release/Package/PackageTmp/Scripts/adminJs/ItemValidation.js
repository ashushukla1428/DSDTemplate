function validateItem() {
    var reason = "";
    reason += validateEmpty(document.getElementById('txtItemName'), 'item name required !');
    //  reason += validateEmpty(document.getElementById('txtFirstName1'), 'Assistant first name required !');
    reason += validateEmpty(document.getElementById('txtMainCompany'), 'main company name required !');
    // reason += validateEmpty(document.getElementById('txtLast1'), 'Assistant last name required !');
    reason += validateDropdown(document.getElementById('drpMainCountry'), 'Please select main country!');

    //reason += validateDropdown(document.getElementById('drpAltCountry'), 'Please select alternate country!');
    //, 'Primary Email Address required!'//, 'Alternate Email Address required !'
    reason += validateEmail(document.getElementById('txtmainEMailAddress'), 'primary');
    reason += validateEmail(document.getElementById('txtmainContactEMailAddress'), 'alternate');

    reason += validateEmail(document.getElementById('txtsparePartsEMailAddress'), 'spare parts email 1#');
    reason += validateEmail(document.getElementById('txtsparePartsContactEMailAddress'), 'spare parts email 2#');

    reason += validateEmail(document.getElementById('txtbillingEMailAddress'), 'billing email 1#');
    reason += validateEmail(document.getElementById('txtbillingContactEMailAddress'), 'alternate email 2#');


    //reason += validateDropdown(document.getElementById('drpPrimaryStates'), 'Please select state!');
    //PRIMARY ADDRESS
  
        reason += validateNumericValue(document.getElementById('txtMainTelephoneSegment1NonUS'), 'Only Numeric value allowed. (Mmain Telephone)');
        reason += validateNumericValue(document.getElementById('txtmainContactTelephoneSegment1NonUS'), 'Only Numeric value allowed. (Main Phone)');
        reason += validateNumericValue(document.getElementById('txtmainFaxSegment1NonUs'), 'Only Numeric value allowed. (Main Fax1#)');
        reason += validateNumericValue(document.getElementById('mainContactFaxSegment1NonUs'), 'Only Numeric value allowed. (Main Fax2#)');
  
    //SPARE PARTS
  
        reason += validateNumericValue(document.getElementById('txtsparePartsTelephoneSegment1NonUS'), 'Only Numeric value allowed. (spare parts Telephone)');
        reason += validateNumericValue(document.getElementById('txtsparePartsContactTelephoneSegment1NonUS'), 'Only Numeric value allowed. (spare parts Phone)');
        reason += validateNumericValue(document.getElementById('txtsparePartsFaxSegment1NonUs'), 'Only Numeric value allowed. (spare parts Fax1#)');
        reason += validateNumericValue(document.getElementById('sparePartsContactFaxSegment1NonUs'), 'Only Numeric value allowed. (spare parts Fax2#)');
   
    //third
 
        reason += validateNumericValue(document.getElementById('txtbillingTelephoneSegment1NonUS'), 'Only Numeric value allowed. (Billing Telephone)');
        reason += validateNumericValue(document.getElementById('txtbillingContactTelephoneSegment1NonUS'), 'Only Numeric value allowed. (Billing Phone)');
        reason += validateNumericValue(document.getElementById('txtbillingFaxSegment1NonUs'), 'Only Numeric value allowed. (Billing Fax1#)');
        reason += validateNumericValue(document.getElementById('billingContactFaxSegment1NonUs'), 'Only Numeric value allowed. (Billing Fax2#)');
  


    if (reason != "") {
        //alert("All fields marked with an asterisk are required.:\n" + reason);
        alert(reason);

        return false;
    }

    return true;
}
