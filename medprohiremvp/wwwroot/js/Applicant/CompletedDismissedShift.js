$(document).ready(function () {
    var applieddates = $('.applieddays').data('applieddates');
    if (applieddates != "") {

        $('.applieddayslist').datepicker({
            beforeShowDay: function () {
                return false;
            }
        }).datepicker('setDates', applieddates.split(","));

    }
});