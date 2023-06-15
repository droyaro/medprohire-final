$(document).on('click', '#sendssms', function () {
    if ($('#ApplicantRegister').valid()) {
        $.ajax({
            url: "/Home/SendSms",
            type: "POST",
            data: {

            },
            success: function (response) {
                if (!response) {
                    $('#myModal6').modal('hide');
                    $('#notvalid').prop("hidden", false);
                    $('#notvalid').append('<hr/><span class="text-danger"> Cant send sms, please go back, and change your phone number</span>');
                    $('#sendssms').prop("disabled", true);

                }
                else {
                    $('#myModal6').modal('show');
                }
            }
        });
    }

});
$(document).on('click', '#verify', function () {
    var key = $('#verifykey').val();
    $.ajax({
        url: "/Home/VerifySms",
        type: "POST",
        data: {
            verifykey: key,
        },
        success: function (response) {
            if (response) {
                $('#myModal6').modal('hide');
                $('#notvalid').remove();
                $('#sendssms').remove();
                $('#submit').append('<hr/><span class="text-success"> thank you for verification! Now you can create account</span>');
                $('#submit').prop("hidden", false);
                $('#create').prop("hidden", false);

            }
            else {

                $('.modal-body').append('<hr/><span class="text-danger"> Code is not valid, please try again or change your phone number</span>');
            }

        },


    });

});
$(".selectpicker").selectpicker({
    style: 'btn-default'
});