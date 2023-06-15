$(document).on('click', '#Complete', function () {
    var id = $(this).data('id');
    var shiftcount = $(this).data('shiftcount');
    var completedshiftcount = $(this).data('completedshiftcount');
    var percentage = 0;
    if (shiftcount > 0) {
        percentage = completedshiftcount / shiftcount * 100;
    }
    $('#CompleteShift').find('.modal-body').find('#shiftcount').val(shiftcount);
    $('#CompleteShift').find('.modal-body').find('#completedshiftcount').val(completedshiftcount);
    $('#CompleteShift').find('.modal-body').find('#percentage').text(percentage.toString() + "%");
    $('#CompleteShift').find('.modal-body').find('#completeshiftaccept').data('id', id);
    $('#CompleteShift').modal('show');
});
$(document).on('click', '#completeshiftaccept', function () {
    var shiftid = $(this).data('id');
    if (shiftid>0) {
        $.ajax({
            url: "/Applicant/CompleteShift",
            type: "POST",
            data: { AppliedShift_ID: shiftid },
            success: function (response) {

                $('#CompleteShift').modal('hide');
                location.reload();
            }

        });
    }
   
});