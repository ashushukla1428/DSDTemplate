function validateItem() {
    var reason = "";
    reason += validateDropdown(document.getElementById('drptab2Employee'), 'Please select employee!');
    reason += validateDropdown(document.getElementById('drptab2Items'), 'Please select item!');
    reason += validateDropdown(document.getElementById('drpTab2Location'), 'Please select location!');
    reason += validateDropdown(document.getElementById('drpTab2RepairType'), 'Please select repair type!');
    reason += validateDropdown(document.getElementById('drpTab2Status'), 'Please select status!');
    reason += IsNumeric(document.getElementById('txtTab2DueDateCUSTOMERDays'), 'Due date (customer) must be numeric data only!');
    reason += IsNumeric(document.getElementById('txtTab2DueDateFactoryDays'), 'Due date (factory) must be numeric data only!');
    reason += validateEmpty(document.getElementById('txtOrderNumber'), 'Please enter order number!');
    reason += validateEmpty(document.getElementById('txtTicketNumber'), 'Please enter ticket number!');

    if (reason != "") {
        alert("Some fields need correction:\n" + reason);
        return false;
    }

    return true;
}


