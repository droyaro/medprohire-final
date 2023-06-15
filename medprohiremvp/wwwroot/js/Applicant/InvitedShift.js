$(".selectpicker").selectpicker({
    style: 'btn-default'
});
var firstclick = 0;
var firstdates = "";
$(document).ready(function () {

  
    var between = [];
    var days = $('#accepteddays').val();



    var currentDate;
    var startdate = $('#StartDate').val();
    var enddate = $('#EndDate').val();
    var dayslist = days.split(',')
    firstdates = dayslist;
    currentDate = new Date(startdate);
    while (currentDate <= new Date(enddate)) {

        var day = currentDate.toLocaleDateString('es-PA');
        if (!dayslist.includes(day)) {
            between.push(currentDate.toLocaleDateString('es-PA'));
        }
        currentDate.setDate((new Date(currentDate)).getDate() + 1);
    }
    $('.datepickershift').datepicker({
        format: 'mm/dd/yyyy',
        startDate: new Date(startdate).toLocaleDateString('es-PA'),
        endDate: new Date(enddate).toLocaleDateString('es-PA')
    }).datepicker('setDatesDisabled', between).on('changeDate', dateChanged);
    $('.datepickershift').datepicker('setDates', days.split(','));
    $('.datepickershift').datepicker('refresh');
    //$('.datepickershift').datepicker('show');


});
$(document).on('click', '#Selectdays', function () {

    if ($(this).prop('checked')) {
        $('#AppliedAllDays').val(false);
        $('#AppliedDaysList').val('');
        $('.datepickershift').datepicker('update', '');
        //var picker = $('.datepickershift').datepicker();
        //var widget = picker.data('datepicker');
        //widget.place = function () {
        //    var offset = this.component ? this.component.offset() : this.element.offset();
        //    this.picker.css({
        //        top: offset.top + this.height + 10,
        //        left: offset.left + 50
        //    });
        //} 
        $('.datepickershift').datepicker({
            container: '#AppliedDaysList',
            showOnFocus: true
        });
        $(this).parent().parent().find(".Pickdays").removeAttr("hidden");
        $('.datepickershift').datepicker('show');

    }
    else {
        $('#AppliedAllDays').val(true);
        var between = [];
        var currentDate;
        var startdate = $('#StartDate').val();
        var enddate = $('#EndDate').val();
        currentDate = new Date(startdate);
        while (currentDate <= new Date(enddate)) {
            between.push(currentDate.toLocaleDateString());
            currentDate.setDate((new Date(currentDate)).getDate() + 1);
        }
        $('.datepickershift').datepicker('setDates', between);
        $(this).parent().parent().find(".Pickdays").attr("hidden", true);
    }
});
//$(document).on('click', '#calendar', function () {
   
//    if (firstclick == 0) {
//        $('#pickeddays').empty();
//        $('#AppliedAllDays').val(false);
//        //$('#AppliedDaysList').val('');
//        $('.datepickershift').datepicker('update', '');
//        firstclick = 1;
//    }

//});
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
            var days = $('#accepteddays').val();
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
function dateChanged(ev) {

    var date = $(".datepickershift").datepicker("getDates");
   var selecteddays = [];
    $('#AppliedDaysList').val('');
    $('#pickeddays').attr('hidden', false);
    $('#pickeddays').empty();
    if (date.length > 0) {
        if (date.length == firstdates.length) {
            firstclick = 0;
        }
        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            selecteddays.push(month + '/' + day + '/' + year);
            $('#pickeddays').append('<span class=pickday>' + month + '/' + day + '/' + year + ' <a  class="removedate"> <i class="mdi mdi-window-close"></i></a></span>')
        });
        if (selecteddays.length > 0) {
            $('#AppliedDaysList').val(selecteddays);
        }
        else {
            $('#AppliedDaysList').val('');
        }
    }



}