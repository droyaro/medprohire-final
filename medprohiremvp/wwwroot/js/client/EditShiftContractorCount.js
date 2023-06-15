var Shift = function () {

    return {

        init: function () {
            Shift.Create();
        },

        Create: function () {
           
           
            var startdate = $('#StartDate').val();
            var enddate = $('#EndDate').val();

           
            $(document).ready(function () {
                var dates = $('#ShiftsDates').val();
                if (dates != "") {
                    $('#calendarview').datepicker({
                        beforeShowDay: function () {
                            return false;
                        }
                    });
                    $('#calendarview').datepicker('setDates', dates.split(','));
                }
            });
         
     
        },
    };
}();
Shift.Create();
function dateChanged(ev) {

    var date = $("#calendarview").datepicker("getDates");
    var selecteddays = [];

    $('#ShiftsDates').val('');

    $('#pickeddays').empty();
    if (date.length > 0) {


        $('#pickeddays').attr('hidden', false);

        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            selecteddays.push(month + '/' + day + '/' + year);
            $('#pickeddays').append('<span class=pickday>' + month + '/' + day + '/' + year + ' <a class="removedate"> <i class="mdi mdi-window-close"></i></a></span>')
        });
        if (selecteddays.length > 0) {
            $('#ShiftsDates').val(selecteddays);
        }
        else {
            $('#ShiftsDates').val('');
        }
    }



}
function AutoGrowTextArea(textField) {
    if (textField.clientHeight < textField.scrollHeight) {
        textField.style.height = textField.scrollHeight + "px";
        if (textField.clientHeight < textField.scrollHeight) {
            textField.style.height =
                (textField.scrollHeight * 2 - textField.clientHeight) + "px";
        }
    }
}


