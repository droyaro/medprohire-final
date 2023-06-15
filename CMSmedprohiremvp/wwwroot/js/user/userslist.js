$(document).on('click', '#boardingactivate', function () {
        var id = $('#ModalBoarding').find('#applicant_id').val();
        if (id) {
            $.ajax({
                url: "/Applicant/ActivateBoarding",
                type: "POST",
                data: { Applicant_ID: id },
                success: function (response) {

                    $('#ModalBoarding').modal('hide');
                    location.reload();
                }

            });
        }
    
});
$(document).on('click', '#boardingcomplete', function () {
    var id = $(this).data('id');
    if (id) {
        $.ajax({
            url: "/Applicant/CompleteBoarding",
            type: "POST",
            data: { Applicant_ID: id },
            success: function (response) {

                $('#ModalBoarding').modal('hide');
                location.reload();
            }

        });
    }

});
$(document).on('click', '#acceptedShifts', function () {
    var id = $(this).data('id');
    var Remarks = $("#Remark_"+id).val();
    $(this).text('').append('<i class="fa fa-spinner fa-spin" style="font-size: 24px"></i>');
    if (id ) {
        $.ajax({
            url: "/Applicant/AcceptShift",
            type: "POST",
            data: {
                AppliedShift_ID: id,
                Remarks: Remarks
            },
            success: function (response) {

                $('#ModalBoarding').modal('hide');
                location.reload();
            }

        });
    }
    

});
$(document).on('click', '#dismissedShifts', function () {
    var id = $(this).data('id');
    var Remarks = $("#Remark_" + id).val();
    $(this).text('').append('<i class="fa fa-spinner fa-spin" style="font-size: 24px"></i>');
    if (id) {
        $.ajax({
            url: "/Applicant/DismissShift",
            type: "POST",
            data: {
                AppliedShift_ID: id,
                Remarks: Remarks
            },
            success: function (response) {

                $('#ModalBoarding').modal('hide');
                location.reload();
            }

        });
    }


});

$(document).on('click', '#paychecks', function () {
    var id = $(this).data('id');
    var app_id = $(this).data('app_id');
    $('#ModalPayCheck').find('.personalinfo').empty();
   
    $.ajax({
        url: "/Applicant/_ApplicantPersonalInfo",
        type: "POST",
        data: { User_ID: id },
        success: function (response) {
            $('#ModalPayCheck').find('.personalinfo').append(response);

        }
    });
    $('#ModalPayCheck').find('#PayCheck_Applicant_ID').val(app_id);
  
    $('#ModalPayCheck').modal('show');

});