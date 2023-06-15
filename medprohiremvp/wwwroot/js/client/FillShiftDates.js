$(document).ready(function () {

    var betweenfull = [];
    var currentDate;
    var startdate = $('#StartDate').val();
    var enddate = $('#EndDate').val();
    var dates = $('#ShiftsDates').val();
    if (dates != "") {
        var dayslist = dates.split(',');
        currentDate = new Date(startdate);
        while (currentDate <= new Date(enddate)) {

            var day = currentDate.toLocaleDateString('es-PA');
            if (!dayslist.includes(day)) {
                betweenfull.push(currentDate.toLocaleDateString('es-PA'));
            }
            currentDate.setDate((new Date(currentDate)).getDate() + 1);
        }
        $('.shiftdatelist').datepicker({
            beforeShowDay: function () {
                return false;
            }
        });
        $('.shiftdatelist').datepicker('setDatesDisabled', betweenfull).datepicker('setDates', dayslist);
       
     
       
    }


});

