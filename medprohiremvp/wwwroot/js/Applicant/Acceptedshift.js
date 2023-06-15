var visiblecount = 6;
$(".selectpicker").selectpicker({

    style: 'btn-select flat'
});
$(document).on('click', '#clockin', function () {
    var id = $(this).data('id');
    $.ajax({
        url: "/Applicant/_clockinpartial",
        type: "POST",
        data: { id: id },
        success: function (response) {
            $("#ClockinModal").find('.modal-body').empty();
            $("#ClockinModal").find('.modal-body').append(response);
            $("#ClockinModal").modal('show');
        }
    });
});
$(document).on('click', '#clockout', function () {
    var id = $(this).data('id');
    $.ajax({
        url: "/Applicant/_clockoutpartial",
        type: "POST",
        data: { id: id },
        success: function (response) {
            $("#ClockinModal").find('.modal-body').empty();
            $("#ClockinModal").find('.modal-body').append(response);
            $("#ClockinModal").modal('show');
        }
    });
});
$(document).on('click', '#addmanually', function () {
    var id = $(this).data('id');
    $.ajax({
        url: "/Applicant/_clockinclockoutmanually",
        type: "POST",
        data: { id: id },
        success: function (response) {
            $("#ClockinModal").find('.modal-body').empty();
            $("#ClockinModal").find('.modal-body').append(response);
            $("#ClockinModal").modal('show');
        }
    });
});
$('#ClockinModal').on('shown.bs.modal', function () {
    date = new Date();
    $("#datetimepicker").datetimepicker({
        format: 'HH:mm', pickDate: false
    });
    $("#datetimepicker1").datetimepicker({
        format: 'HH:mm', pickDate: false
    });
});
$(document).on('click', '#clockoutmanually', function () {
    var id = $(this).parent().parent().find('span').text();
    var clockouttime = $('#ClockOutTime').val();
    $.ajax({
        url: "/Applicant/ConfirmClockoutManually",
        type: "POST",
        data: {
            id: id,
            clockouttime: clockouttime
        },
        success: function (response) {
            $("#ClockinModal").find('.modal-body').empty();
            $("#ClockinModal").find('.modal-body').append(' <h6 class="text-center"> Shift instance completed</h6 >');
            setTimeout(function () { $("#ClockinModal").modal('hide'); }, 3000);           
            location.reload();
        }
            
    });
  
});
$(document).on('click', '#clockoutnow', function () {
    var id = $(this).data('id');
  
    $.ajax({
        url: "/Applicant/ConfirmClockout",
        type: "POST",
        data: {
            id: id,
            
        },
        success: function (response) {
            $("#ClockinModal").find('.modal-body').empty();
            $("#ClockinModal").find('.modal-body').append(' <h6 class="text-center"> Shift instance completed</h6 >');
            setTimeout(function () { $("#ClockinModal").modal('hide'); }, 3000);
            location.reload();
        }

    });

});
$(document).on('click', '#clockinclockoutmanually', function () {
    var id = $(this).parent().find('span').text();
    var clockouttime = $('#ClockOutTime').val();
    var clockintime = $('#ClockinTime').val();
    $.ajax({
        url: "/Applicant/ConfirmClockinClockoutManually",
        type: "POST",
        data: {
            id: id,
            clockouttime: clockouttime,
            clockintime: clockintime
        },
        success: function (response) {
            $("#ClockinModal").find('.modal-body').empty();
            $("#ClockinModal").find('.modal-body').append(' <h6 class="text-center"> Shift instance completed</h6 >');
            setTimeout(function () { $("#ClockinModal").modal('hide'); }, 10000);      
            location.reload();
        }
    });

});
$('.input-daterange').datepicker({
     
})/*.on('changeDate', dateChanged)*/;
 
$('.selectpicker').on('change', function () {

    //var city = $("#Locations").val();
    //var client = $("#Clincal").val();
    //var startdate = $("#StartDate").datepicker("getDate");;
    //var enddate = $("#EndDate").datepicker("getDate");
    //filter(startdate, enddate, client, city);
    //var inputlength = $('.filter:visible').length;
    //$('#pagination').twbsPagination('destroy');
    //if (inputlength > visiblecount) {
    //    var pagecount = inputlength / visiblecount + 1;
    //    $('#pagination').twbsPagination('destroy');
    //    $('#pagination').twbsPagination({
    //        totalPages: pagecount,
    //        visiblePages: 1,
    //        next: '<i class="far fa-angle-right"></i>',
    //        prev: '<i class="far fa-angle-left"></i>',
    //        first: '',
    //        last: '',
            
    //    });
    //}
    //$('.filter.filtered').slice(visiblecount, $('.filter').length).prop("hidden");
});
$(document).on('click', '.next .page-link',function () {
    var activepage = $('#pagination .active .page-link').text();

    $('.filtered').slice((activepage - 2) * visiblecount, (activepage - 1) * visiblecount).attr("hidden", true);
    $('.filtered').slice((activepage - 1) * visiblecount, (activepage) * visiblecount).attr("hidden", false);
   
});
$(document).on('click', '.prev .page-link', function () {
    var activepage = $('#pagination .active .page-link').text();
    $('.filtered').slice((activepage - 1) * visiblecount, (activepage) * visiblecount).attr("hidden", false);
    $('.filtered').slice((activepage) * visiblecount, (activepage + 1) * visiblecount).attr("hidden", true);


});
function dateChanged(ev) {
    var city = $("#Locations").val();
    var client = $("#Clincal").val();
    var startdate = $("#StartDate").datepicker("getDate");;
    var enddate = $("#EndDate").datepicker("getDate");;
    filter(startdate, enddate, client, city);
    var inputlength = $('.filter:visible').length;
    $('#pagination').twbsPagination('destroy');
    if (inputlength > visiblecount) {
        var pagecount = inputlength / visiblecount + 1;
        $('#pagination').twbsPagination('destroy');
        $('#pagination').twbsPagination({
            totalPages: pagecount,
            visiblePages: 1,
            next: '<i class="far fa-angle-right"></i>',
            prev: '<i class="far fa-angle-left"></i>',
            first: '',
            last: '',

        });
    }
    $('.filter.filtered').slice(visiblecount, $('.filter').length).attr('hidden', true);
}
$('#SearchShifts').click(function () {
    var city = $("#Locations").val();
    var client = $("#Clincal").val();
    var startdate = $("#StartDate").datepicker("getDate");;
    var enddate = $("#EndDate").datepicker("getDate");;
    filter(startdate, enddate, client, city);
    var inputlength = $('.filtered').length;
    $('#pagination').twbsPagination('destroy');
    if (inputlength > visiblecount) {
        var pagecount = inputlength / visiblecount + 1;
        $('#pagination').twbsPagination('destroy');
        $('#pagination').twbsPagination({
            totalPages: pagecount,
            visiblePages: 1,
            next: '<i class="far fa-angle-right"></i>',
            prev: '<i class="far fa-angle-left"></i>',
            first: '',
            last: '',

        });
    }
    $('.filtered').slice(visiblecount, $('.filtered').length).attr('hidden', true);
});
function filter(startday, endday, client, city) {
    var input = $('.filter');

    var array = [];
    input.each(function () {
        $(this).removeAttr("hidden");
        $(this).removeClass('filtered');
        if (city != " ") {
            if ($(this).find("#cityid").val() != city) {
                $(this).removeClass('filtered');
                $(this).attr("hidden", "hidden");
                return true;
            }
        }
        if (client != " ") {
            if ($(this).find("#client").val() != client) {
                $(this).removeClass('filtered');
                $(this).attr("hidden", "hidden");
                return true;
            }
        }
        var Workdate = new Date($(this).find("#workingday").val());
        if (startday == null && endday == null) {
            $(this).addClass('filtered');
            return true;
        }
        if (startday == null && Workdate <= max) {
            $(this).addClass('filtered');
            return true;
        }
        if (endday == null && Workdate >= startday) {
            $(this).addClass('filtered');
            return true;
        }
        if (Workdate <= endday && Workdate >= startday) {
            $(this).addClass('filtered');
            return true;
        }
        $(this).attr("hidden", "hidden");
        $(this).removeClass('filtered');
       
    });
   

}