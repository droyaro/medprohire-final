$(document).on('click', '#seeshiftdates', function () {
    var id = $(this).data('id');
    if (id != "") {
        $.ajax({
            url: "/ClinicalInstitution/GetShiftDates",
            type: "POST",
            data: {
                ClientShift_ID: id
            },
            success: function (response) {
                if (response != "") {
                    $('.shiftdatelist').datepicker({
                        beforeShowDay: function () {
                            return false;
                        }
                    });
                    $('.shiftdatelist').datepicker('setDates', response.split(","));
                    $("#ShiftDateModel").modal('show');
                }

            }
        });
    }
});