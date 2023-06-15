var selectedvalues = [];
$(".selectpicker").selectpicker({

    style: 'btn-select flat'
});
$(document).on('change', '#Search_States', function () {
    //selectedvalues = $(this).val();
    //if (selectedvalues) {
    //    if (selectedvalues.length == 0) {
    //        $(this).parent().find('.bs-select-all').attr('hidden', false);
    //        $(this).parent().find('.bs-deselect-all').attr('hidden', true);
    //    }
    //    else {
    //        $(this).parent().find('.bs-deselect-all').attr('hidden', false);
    //        $(this).parent().find('.bs-select-all').attr('hidden', true);
    //    }
    //}
    $("#Search_Cities option").slice(0).remove();
    $("#Search_Cities").selectpicker('refresh');
    ($(this).val()).forEach(function (value) {
        if (value != "") {
            $.ajax({
                url: "/Applicant/GetShiftCities",
                type: "POST",
                data: {
                    stateid: value
                },
                success: function (response) {

                    $.each(response, function (i, d) {
                        $("#Search_Cities").append("<option value=" + d.id + ">" + d.city_name + "  </option>");
                    });

                    $("#Search_Cities").selectpicker('refresh');
                }
            });
        }

    });
   
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

$(document).on('click', '.ReadMore', function () {
    var shiftid = $(this).data('id');
    var picked = $('#pickedlist').hasClass('active');
    if (!picked) {
        $.ajax({
            url: "/Applicant/_shiftdetailpartial",
            type: "POST",
            data: {
                id: shiftid
            },
            success: function (response) {
                $("#AvailableShift").find('.modal-body').empty();
                $("#AvailableShift").find('.modal-body').append(response);
                $('.datepickershift').datepicker();
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

                    $('.datepickershift').datepicker('setDatesDisabled', betweenfull).on('changeDate', dateChanged);
                    $('.datepickershift').datepicker('setDates', dayslist);
                    $("#pickeddays").attr("hidden", false);
                }
                $("#AvailableShift").modal('show');
            }
        });
    }
    else {
        $.ajax({
            url: "/Applicant/_pickedshiftdetail",
            type: "POST",
            data: {
                id: shiftid
            },
            success: function (response) {
                $("#AvailableShift").find('.modal-body').empty();
                $("#AvailableShift").find('.modal-body').append(response);
                $("#AvailableShift").modal('show');
            }
        });
    }
    
});
$('.datepicker').datepicker({ startDate: '0' });

$('.datepickershift').datepicker();
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
            orientation: "top",
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
//    if ($('#AppliedDaysList').val()!='') {
//    $('#AppliedAllDays').val(false);
//    $('#AppliedDaysList').val('');
//    $('.datepickershift').datepicker('update', '').on('changeDate', dateChanged);
        
//        $("#pickeddays").empty();
//        $("#pickeddays").attr("hidden", false);
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
        var dates = $('#ShiftsDates').val();
            if (dates != "") {
                var dayslist = dates.split(',');
                $('#AppliedAllDays').val(true);
               
                $('.datepickershift').datepicker('setDates', dayslist);
               
            }
        }    
    });
$(document).ready( function () {
    
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
function dateChanged(ev) {

    var date = $(".datepickershift").datepicker("getDates");
    var dates = $(".datepickershift").datepicker("getFormattedDates");
    var selecteddays = [];
    $('#pickeddays').empty();
    if (date.length > 0) {
        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            selecteddays.push(month + '/' + day + '/' + year);
            $('#pickeddays').append('<span class=pickday>' + month + '/' + day + '/' + year + ' <a href="#" class="removedate"> <i class="mdi mdi-window-close"></i></a></span>')
        });
        if (selecteddays.length > 0) {
            $('#AppliedDaysList').val(selecteddays);
        }
        else {
            $('#AppliedDaysList').val('');
        }
    }



}