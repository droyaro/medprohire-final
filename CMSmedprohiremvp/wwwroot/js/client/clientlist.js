$(document).on('click', '#boardingactivate_client', function () {
    var id = $('#ModalBoarding').find('#client_id').val();
    if (id) {
        $.ajax({
            url: "/Clinical/ActivateBoarding",
            type: "POST",
            data: { ClinicalIntitution_ID: id },
            success: function (response) {

                $('#ModalBoarding').modal('hide');
                location.reload();
            }

        });
    }

});