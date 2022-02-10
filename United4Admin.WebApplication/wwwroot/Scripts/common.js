$(function () {
    $(".datefield").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true });
});

$(function () {
    $('.datePicker').each(function (index, element) {
        var date = $(this).val();
        $(this).datepicker({
            dateFormat: 'dd/mm/yy'
        }).datepicker('setDate', date)
    });
});

//$(function () {
//    $.validator.addMethod('date',
//        function (value, element) {
//            if (this.optional(element)) {
//                return true;
//            }
//            var ok = true;
//            try {
//                $.datepicker.parseDate('dd/mm/yy', value);
//            }
//            catch (err) {
//                ok = false;
//            }
//            return ok;
//        });
//    $(".datefield").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true });
//});

// ensure all ajax requests are set with a unique id to prevent browser caching
$.ajaxSetup({ cache: false });

pathArray = location.href.split('/');
protocol = pathArray[0];
host = pathArray[2];

if (host.indexOf('localhost') >= 0) {
    url = protocol + '//' + host + '/';
} else {
    url = protocol + '//' + host + '/';
}

$(document).ajaxError(function (event, jqXHR, settings, exception) {
    if (jqXHR.status == 401) {  //Unathorised
        window.location.replace(url + "Account/Login?timeout");
    }
});

var isSubmitting = false

$(document).ready(function () {
    
    var form = $('form');
    if (form.length > 0) {
        $('form').submit(function () {
            isSubmitting = true
        })

        $('form').data('initial-state', $('form').serialize());

        $(window).on('beforeunload', function () {

            if (!isSubmitting && $('form').serialize() != $('form').data('initial-state')) {
                return 'You have unsaved changes which will not be saved.'
            }
        });
    }

    $("body").css("padding-top", $(".navbar").height());
})