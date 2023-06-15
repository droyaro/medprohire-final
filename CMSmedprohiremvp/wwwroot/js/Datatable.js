var table = $('.dataTable').DataTable({
    "ordering": false,
    pageLength: 10,
    responsive: true,
    info: false,
    lengthChange: false,
    searching: true,
    "autoWidth": false,
   dom: '<"top"i><"w-100"rt><"bottom"p><"clear">'


});
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var min = $('#insertdate').datepicker("getDate");
            var max = $('#enddate').datepicker("getDate");
            var startDate = new Date(data[0]);

            if (min == null && max == null) { return true; }
            if (min == null && startDate <= max) { return true; }
            if (max == null && startDate >= min) { return true; }
            if (startDate <= max && startDate >= min) { return true; }
            return false;

        }
    );
    $('#searchtext').on('keyup click', function () {
        filterGlobal();
});
$('.dropdown-alerts li').on('click', function () {
    var id = $(this).find('a').data('id');
    if (id == "Not Started") {

        table.columns().search('').draw();
        table.column(3).search(id).draw();
    }
    else
        if (id == "empagreement") {

            table.columns().search('').draw();
            table.column(6).search(id).draw();
        }
        else
            if (id == "appliedshifts") {
                table.search('').columns().search('').draw();
                table.column(6).search('Activate Shifts').draw();
            }
});
$('.clients-search-list li').on('click', function () {
    var id = $(this).find('a').data('id');
    table.columns().search('').draw();
    table.column(5).search(id).draw();
});
$(".count-info").on("click", function () {
    table.columns().search('').draw();
});

    $(document).on('click', '#boarding', function () {
        var id = $(this).data('id');
      
        $('#ModalBoarding').find('.modal-body').empty();
        $.ajax({
            url: "/Applicant/_ApplicantPersonalInfo",
            type: "POST",
            data: { User_ID: id },
            success: function (response) {
               
                $('#ModalBoarding').find('.modal-body').append(response);
                $('#ModalBoarding').find('.modal-body').append('<button type="submit" id="boardingactivate" class="btn btn - primary">Activate</button>');
                $('#ModalBoarding').modal('show');
            }
        });

      
});
$(document).on('click', '#completeboarding', function () {
    var id = $(this).data('id');

    $('#ModalBoarding').find('.modal-body').empty();
    $.ajax({
        url: "/Applicant/_ApplicantBoardingProcess",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {

            $('#ModalBoarding').find('.modal-body').append(response);
            $('#ModalBoarding').modal('show');
        }
    });


});
$(document).on('click', '#boarding_client', function () {
    var id = $(this).data('id');

    $('#ModalBoarding').find('.modal-body').empty();
    $.ajax({
        url: "/Clinical/_clientPersonalInfo",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {

            $('#ModalBoarding').find('.modal-body').append(response);
            $('#ModalBoarding').find('.modal-body').append('<button type="submit" id="boardingactivate_client" class="btn btn - primary">Activate</button>');
            $('#ModalBoarding').modal('show');
        }
    });


});
$(document).on('click', '#completeboarding_client', function () {
    var id = $(this).data('id');

    $('#ModalBoarding').find('.modal-body').empty();
    $.ajax({
        url: "/Clinical/_clientPersonalInfo",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {

            $('#ModalBoarding').find('.modal-body').append(response);
          
        }
    });

    $.ajax({
        url: "/Clinical/_clientSpecialtiesCost",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {

            $('#ModalBoarding').find('.modal-body').append(response);
            $('#ModalBoarding').modal('show');
        }
    });



});
$(document).on('click', '#ComposeMail', function () {
    var id = $(this).data('userid');

    $('#ModalBoarding').find('.modal-body').empty();
    $.ajax({
        url: "/Home/_ComposeEmail",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {

            $('#ModalBoarding').find('.modal-body').append(response);
            $('#ModalBoarding').modal('show');
            $('.summernote').summernote();

        }
    });


});
$(document).on('click', '#appliedshifts', function () {
    var id = $(this).data('id');
   
    $('#ModalShifts').find('.personalinfo').empty();
    $('#ModalShifts').find('.specialities').empty();
    $('#ModalShifts').find('.appliedShifts').empty();

    $.ajax({
        url: "/Applicant/_ApplicantPersonalInfo",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {
            $('#ModalShifts').find('.personalinfo').append(response);
           
        }
    });
    $.ajax({
        url: "/Applicant/_ApplicantSpecialities",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {
            $('#ModalShifts').find('.specialities').append(response);

        }
    });
    $.ajax({
        url: "/Applicant/_ApplicantAplliedShifts",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {
            $('#ModalShifts').find('.appliedShifts').append(response);
            var seeshiftdates = $('.seeshiftdates');
            seeshiftdates.each(function () {
                var shiftdates = $(this).data('shiftdates');
                var id = $(this).data('id');
                if (shiftdates != "") {

                    $('.shiftdatelist_'+id).datepicker({
                        beforeShowDay: function () {
                            return false;
                        }
                    }).datepicker('setDates', shiftdates.split(","));

                }
            });
            var applieddays = $('.applieddays');
            applieddays.each(function () {
                var applieddates = $(this).data('applieddates');
                var id = $(this).data('id');
                if (applieddates != "") {

                    $('.applieddayslist_'+id).datepicker({
                        beforeShowDay: function () {
                            return false;
                        }
                    }).datepicker('setDates', applieddates.split(","));

                }
            });
        }
    });

    $('#ModalShifts').modal('show');
  


});

    $('#insertdate, #enddate').change(function () {
        table.draw();
});

$(document).on('click', '#agreement', function () {
    var datarow = table.row($(this).closest('tr')).data();
    $('#ModalEmployment').find('#employee_name').val($(this).data('name'));
    $('#ModalEmployment').find('#employee_address').val($(this).data('address'));
    $('#ModalEmployment').find('#employee_id').val( $(this).data('id'));
    var Id = $(this).data('id');
    $.ajax({
        url: "/Home/GetEmpAgreementSpecialties",
        type: "POST",
        data: {
            user_id: Id,
        },
        success: function (response) {
            $('#ModalEmployment').find('#Employment_specialities').slice(1).remove();
         
            $.each(response, function (i, d) {
                $('#ModalEmployment').find('#Employment_specialities').append("<option value=" + d.speciality_ID + ">" + d.specialityName + "</option>");
            });
            $('#ModalEmployment').find('#Employment_specialities').selectpicker("refresh");
            

        }
    });
    $('#ModalEmployment').modal('show');
    $('.datepicker').datepicker();
});
$('#insertdate, #enddate').change(function () {
    table.draw();
});

function filterGlobal() {
    table.search(
        $('#searchtext').val()).draw();
}