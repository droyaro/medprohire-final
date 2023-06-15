$(".selectpicker").selectpicker({

    style: 'btn-select flat'
});
var firstclick = 0;
$(document).on('change', '#Search_Location', function () {
    var selectedtext = $('#Search_Location option:selected').text();

    $('h6.distance').text("Distance from Location " + selectedtext);
   
});
$(document).on('change', '.selectpicker', function () {
    selectedvalues = $(this).val();
    if (selectedvalues) {
        if (selectedvalues.length == 0) {
            $(this).parent().find('.bs-select-all').attr('hidden', false);
            $(this).parent().find('.bs-deselect-all').attr('hidden', true);
        }
        else {
            $(this).parent().find('.bs-deselect-all').attr('hidden', false);
            $(this).parent().find('.bs-select-all').attr('hidden', true);
        }
    }

});
$('.datepicker').daterangepicker({
    format: "MM/dd/yyyy",
    "autoApply": true,
    "opens": "center",
    "minDate": moment(),
    "locale": {
        "customRangeLabel": "Custom",
        "daysOfWeek": [
            "S",
            "M",
            "T",
            "W",
            "T",
            "F",
            "S"
        ],
        "monthNames": [
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        ],
    },
});

//$(document).on('click', '#calendar', function () {
//    var shift_id = $(this).data('id');
   
//        $('#pickeddays_' + shift_id).empty();
//        $('#AppliedAllDays_' + shift_id).val(false);
//    //$('#AppliedDaysList').val('');
 
//    $('.datepickershift_' + shift_id).datepicker('destroy');
//    var betweenfull = [];
//    var days = $('#AppliedDaysList_' + shift_id).val();
//    var currentDate;
//    var startdate = $('#StartDate_' + shift_id).val();
//    var enddate = $('#EndDate_' + shift_id).val();
//    var dayslist = days.split(',');
//    currentDate = new Date(startdate);
//    while (currentDate <= new Date(enddate)) {

//        var day = currentDate.toLocaleDateString('es-PA');
//        if (!dayslist.includes(day)) {
//            betweenfull.push(currentDate.toLocaleDateString('es-PA'));
//        }
//        currentDate.setDate((new Date(currentDate)).getDate() + 1);
//    }
//    $('.datepickershift_' + shift_id).datepicker({
//        orientation: "top auto",
//        startDate: new Date(startdate).toLocaleDateString('es-PA'),
//        endDate: new Date(enddate).toLocaleDateString('es-PA'),
       
//    }).datepicker('setDatesDisabled', betweenfull).datepicker('update', '');
     
  
//});
$(document).on('click', '.removedate', function () {
    var shift_id = $(this).parent().data('id');
    var text = $(this).parent().remove();
    var date = $('#pickeddays_' + shift_id).find('.pickday');
    var between = [];
    date.each(function () {
        between.push($(this).text());
    });
    if (between.length > 0) {
        $('.datepickershift_' + shift_id).datepicker('setDates', between);
    }
    else {
        $('.datepickershift_' + shift_id).datepicker('setDates', '');
        {

            var betweenfull = [];
            var days = $("#freedays_" + shift_id).val();
            var currentDate;
            var startdate = $('#StartDate_' + shift_id).val();
            var enddate = $('#EndDate_' + shift_id).val();
            var dayslist = days.split(',')
            currentDate = new Date(startdate);
            while (currentDate <= new Date(enddate)) {

                var day = currentDate.toLocaleDateString('es-PA');
                if (!dayslist.includes(day)) {
                    betweenfull.push(currentDate.toLocaleDateString('es-PA'));
                }
                currentDate.setDate((new Date(currentDate)).getDate() + 1);
            }
            $('.datepickershift_' + shift_id).datepicker({
                orientation: "top auto",
                startDate: new Date(startdate).toLocaleDateString('es-PA'),
                endDate: new Date(enddate).toLocaleDateString('es-PA')
            }).datepicker('setDatesDisabled', betweenfull);
            $('.datepickershift_' + shift_id).datepicker('setDates', days.split(','));
            $('.datepickershift_' + shift_id).datepicker('refresh');

        }
    }
});
$(document).ready(function () {


    var selectpickers = $('.selectpicker');
    selectpickers.each(function () {
        selectedvalues = $(this).val();
        if (selectedvalues) {
            if (selectedvalues.length == 0) {
                $(this).parent().find('.bs-select-all').attr('hidden', false);
                $(this).parent().find('.bs-deselect-all').attr('hidden', true);
            }
            else {
                $(this).parent().find('.bs-deselect-all').attr('hidden', false);
                $(this).parent().find('.bs-select-all').attr('hidden', true);
            }
        }
    });


});
$(document).on('change', "#shifts_select", function () {

    var shift_id = $(this).val();
    $(".shift_tab_pane").removeClass("active").removeClass("show");
    $("#" + shift_id).addClass("active").addClass("show");
   
});
$(document).on('click', '.ReadMore', function () {
    var applicantid = $(this).data('id');


        $.ajax({
            url: "/ClinicalInstitution/_inviteshiftpartial",
            type: "POST",
            data: {
                Applicant_ID: applicantid
            },
            success: function (response) {
                $("#InviteShift").find('.modal-body').empty();
                $("#InviteShift").find('.modal-body').append(response);
              
                $("#InviteShift").modal('show');
                var calendars = $(".Pickdays");
                calendars.each(function () {
                    var shift_id = $(this).find("#shift_id").data("id");


                    var betweenfull = [];
                    var days = $('#AppliedDaysList_' + shift_id).val();
                    var currentDate;
                    var startdate = $('#StartDate_' + shift_id).val();
                    var enddate = $('#EndDate_' + shift_id).val();
                    var dayslist = days.split(',');
                    currentDate = new Date(startdate);
                    while (currentDate <= new Date(enddate)) {

                        var day = currentDate.toLocaleDateString('es-PA');
                        if (!dayslist.includes(day)) {
                            betweenfull.push(currentDate.toLocaleDateString('es-PA'));
                        }
                        currentDate.setDate((new Date(currentDate)).getDate() + 1);
                    }
                    $('.datepickershift_' + shift_id).datepicker({
                        orientation: "top auto",
                        startDate: new Date(startdate).toLocaleDateString('es-PA'),
                        endDate: new Date(enddate).toLocaleDateString('es-PA'),
                       
                    }).datepicker('setDatesDisabled', betweenfull).on('changeDate', dateChanged);
                    $('.datepickershift_' + shift_id).datepicker('setDates', days.split(','));
                    $('.datepickershift_' + shift_id).datepicker('refresh');

                });
                $(".registerselect").selectpicker('refresh');
            }
                
                    });
});

$(document).on('click', '.invite', function () {

    $(this).prop("disabled", true);
    $(this).css({ 'cursor': 'not-allowed' });
        $.ajax({
            url: "/ClinicalInstitution/InviteApplicant",
            type: "POST",
            data:
                $(this).parents("form").serialize(),
            
            success: function (response) {
                if (response) {
                    $("#InviteShift").modal('hide');
                    $('#AnswerModel').modal('show');
                    setTimeout(location.reload(), 3000);
                   
                }
                else {
                    $("#InviteShift").modal('hide');
                    $('#AnswerModel').find('.modal-body').find('.success').attr('hidden', true);
                    $('#AnswerModel').find('.modal-body').find('.danger').attr('hidden', false);
                    $('#AnswerModel').find('.modal-body').find('.danger span').append("Changes error, please try again");
              
                    $('#AnswerModel').modal('show');
                    setTimeout(location.reload(), 3000);
                }

               
            }
        });
    
});

function dateChanged(ev) {
    var activeshift = $('a[data-toggle="tab"].active');
    var shift_id = activeshift.data('shiftid');
    var date = $(".datepickershift_" + shift_id).datepicker("getDates");
    var selecteddays = [];
    var freedays = $("#freedays_" + shift_id);
    var firstdates = freedays.val().split(',');
    $('#AppliedDaysList_' + shift_id).val('');
  
    $('#pickeddays_' + shift_id).empty();
    if (date.length > 0) {
      
       
            $('#pickeddays_' + shift_id).attr('hidden', false);
        
        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            selecteddays.push(month + '/' + day + '/' + year);
            $('#pickeddays_' + shift_id).append('<span class=pickday data-id=' + shift_id + '>' + month + '/' + day + '/' + year + ' <a href="#" class="removedate"> <i class="mdi mdi-window-close"></i></a></span>')
        });
        if (selecteddays.length > 0) {
            $('#AppliedDaysList_' + shift_id).val(selecteddays);
        }
        else {
            $('#AppliedDaysList_' + shift_id).val('');
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