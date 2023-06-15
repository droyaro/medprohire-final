
$(document).on('click', '#InviteApplicant', function () {
    var App_id = $(this).data('app_id');
    var ClientShift_ID = $(this).data('clientshift_id');
    $.ajax({
        url: "/Clinical/InviteApplicant",
        type: "POST",
        data: {
            App_ID: App_id,
            ClientShift_ID: ClientShift_ID
        },
        success: function (response) {
            $('#ModalInvite').find('#app_id').val(App_id);
            $('#ModalInvite').find('#clientshift_id').val(ClientShift_ID);
            var shiftdates = $('#ShiftsDates').val();
            if (shiftdates != "") {
                var shiftdateslist = shiftdates.split(',');

                if (shiftdateslist.length > 0) {
                    var betweenfull = [];
                    currentDate = new Date(shiftdateslist[0]);
                    while (currentDate <= new Date(shiftdateslist[shiftdateslist.length - 1])) {

                        var day = currentDate.toLocaleDateString('es-PA');
                        if (!shiftdateslist.includes(day)) {
                            betweenfull.push(currentDate.toLocaleDateString('es-PA'));
                        }
                        currentDate.setDate((new Date(currentDate)).getDate() + 1);
                    }
                }
                $('.datepickershift').datepicker({
                    

                }).datepicker('setDatesDisabled', betweenfull).on('changeDate', dateChanged).datepicker("setDates", shiftdateslist);
            }
            $('#pickeddays').attr('hidden', false);

            $('#ModalInvite').modal('show');
           
            //$('.datepickershift').datepicker('show');
        }
    });
});

$(document).on('click', '#ApplyInvitation', function () {
    var App_id = $('#ModalInvite').find('#app_id').val();
    var ClientShift_ID = $('#ModalInvite').find('#clientshift_id').val();
    var Remarks = $('#Remarks').val();
    var AppliedDays = $('#AppliedDaysList').val();
    $.ajax({
        url: "/Clinical/AcceptInvitation",
        type: "POST",
        data: {
            Applicant_ID: App_id,
            ClientShift_ID: ClientShift_ID,
            AppliedDaysList: AppliedDays,
            Remarks: Remarks,
            Status:1
        },

        success: function (response) {
            $('#ModalInvite').find('#app_id').val('');
            $('#ModalInvite').find('#clientshift_id').val('');

            $('.datepickershift').datepicker('setDates', '');
            $('.datepickershift').datepicker('refresh');
            $('#ModalInvite').modal('hide');
            location.reload();
        }
    });
});
$(document).on('click', '.removedate', function () {
    var text = $(this).parent().remove();
    var date = $('#pickeddays').find('.pickday');
    var between = [];
    date.each(function () {
        between.push($(this).text());
    });
    if (between.length > 0) {
        $('.datepickershift').datepicker('setDates', between);
    }
    else {
        $('.datepickershift').datepicker('setDates', '');
        {

            var betweenfull = [];
            var days = $('#Shift_ShiftsDates').val();
            var currentDate;
            var startdate = $('#StartDate').val();
            var enddate = $('#EndDate').val();
            var dayslist = days.split(',')
            currentDate = new Date(startdate);
            while (currentDate <= new Date(enddate)) {

                var day = currentDate.toLocaleDateString('es-PA');
                if (!dayslist.includes(day)) {
                    betweenfull.push(currentDate.toLocaleDateString('es-PA'));
                }
                currentDate.setDate((new Date(currentDate)).getDate() + 1);
            }
            $('.datepickershift').datepicker({

                startDate: new Date(startdate).toLocaleDateString('es-PA'),
                endDate: new Date(enddate).toLocaleDateString('es-PA')
            }).datepicker('setDatesDisabled', betweenfull);
            $('.datepickershift').datepicker('setDates', days.split(','));
            $('.datepickershift').datepicker('refresh');

        }
    }
});
$(document).ready(function () {
    var shiftdates = $('#ShiftsDates').val();
    if (shiftdates != "") {
        var shiftdateslist = shiftdates.split(',');
      
        if (shiftdateslist.length > 0) {
            var betweenfull = [];
            currentDate = new Date(shiftdateslist[0]);
            while (currentDate <= new Date(shiftdateslist[shiftdateslist.length-1])) {

                var day = currentDate.toLocaleDateString('es-PA');
                if (!shiftdateslist.includes(day)) {
                    betweenfull.push(currentDate.toLocaleDateString('es-PA'));
                }
                currentDate.setDate((new Date(currentDate)).getDate() + 1);
            }
        }
        $('#calendarview').datepicker({
            beforeShowDay: function () {
                return false;
            }

        }).datepicker('setDatesDisabled', betweenfull).datepicker("setDates", shiftdateslist);
    }
});
$('#calendarview').datepicker({
    beforeShowDay: function () {
        return false;
    }
});
function dateChanged(ev) {

    var date = $(".datepickershift").datepicker("getDates");
    var selecteddays = [];
   
    $('#AppliedDaysList').val('');
    $('#pickeddays').attr('hidden', false);
    $('#pickeddays').empty();
    if (date.length > 0) {
   
        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            selecteddays.push(month + '/' + day + '/' + year);

            $('#pickeddays').append('<span class=pickday>' + month + '/' + day + '/' + year + ' <a  class="removedate"><i class="fa fa-times"></i></span>')
        });
        if (selecteddays.length > 0) {
            $('#AppliedDaysList').val(selecteddays);
        }
        else {
            $('#AppliedDaysList').val('');
        }
    }
}