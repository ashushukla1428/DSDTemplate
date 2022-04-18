var onBeginSaving = function () {
    var doSubmit = true;
  //  alert("dsfds");
    $('form').find('input[type=text], input[type=email], input[type=password], select, textarea').each(function () { //add "txt_required" class and set placeholder to the control
        if ($(this).hasClass("txt_required")) {
            if ($(this).val() === "") {
                $.toaster({ priority: 'warning', title: 'Warning', message: $(this).attr("placeholder") + " is required." });
                doSubmit = false;
            }

        }


    });
    return doSubmit;
};

var onSaveComplete = function (content) {
    //$.toaster({ priority: 'success', title: 'Success', message: content });
};

var onSuccessSaving = function (content) {
    if (content === "Data saved successfully.")
    {
        $.toaster({ priority: 'success', title: 'Success', message: content });
        //$("#PageModal").modal("hide");
    }
    else
        $.toaster({ priority: 'warning', title: 'Warning', message: content });
};

var onException = function (content) {
    //$.toaster({ priority: 'warning', title: 'warning', message: content });
};