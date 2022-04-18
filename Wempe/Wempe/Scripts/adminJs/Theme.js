function show() {
        
    //if (validateCartValues()) {
    var model =
        {
            //AppraiserID: parseInt($('#hdnid').val()),
            BodyColor: $('#txtBodyColor').val(),
            PageSidebar: $('#txtPageSidebar').val(),
            PageHeader: $('#txtPageHeader').val(),
            PageFooter: $('#txtPageFooter').val(),
            PageContent: $('#txtPageContent').val(),
            PageBar: $('#txtPageBar').val(),
            TabContent: $('#txtTabContent').val(),
            PortletTitle: $('#txtPortletTitle').val(),
            PortletBody: $('#txtPortletBody').val(),
            FormActions: $('#txtFormActions').val(),
            FormControl: $('#txtFormControl').val(),
            ModalPopUp: $('#txtModalPopUp').val(),
            SidebarSubMenu: $('#txtSidebarSubMenu').val(),
            Button: $('#txtButton').val(),
            ActiveTab: $('#txtActiveTab').val(),
            ActiveSideBar: $('#txtActiveSideBar').val(),
            SideBarHover: $('#txtSideBarHover').val(),
            InnerTable: $('#txtInnerTable').val(),
            InnerTableHover: $('#txtInnerTableHover').val(),
            ButtonHover: $('#txtButtonHover').val(),
        };
    // alert(JSON.stringify(model));
    $.ajax({
        url: siteUrl + 'Theme/UpdateThemeSetting',
        type: 'POST',
        headers: { "requestType": "client" },
        data: JSON.stringify(model),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            alert(data);
            if (data.Status) {
                //resetControl();
                //$('#responsive').modal('toggle');
                //showMessage('Success', 'success', data.Message, 'toast-top-right');
                //getList(1);
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
    //}
}