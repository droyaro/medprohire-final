var branchid = 0;
function RemoveLocation(Branch_ID, element) {
    branchid = Branch_ID;
    var data = $('form').find(element);

    $('#RemoveModal').find('#modaladdress').text(data.data("address"));
    $('#RemoveModal').find('#modalcontactname').text(data.data("contactname"));
    $('#RemoveModal').find('#modalphone').text(data.data("phone"));
    $('#RemoveModal').find('#modalname').text(data.data("name"));
    $('#RemoveModal').find('#branchid').text(Branch_ID);

    $('#RemoveModal').modal('show');
};
$(".selectpicker").selectpicker({
    style: 'btn-default'
});
$('#removecomfirmed').on('click',function ()
{
    var id = $('#RemoveModal').find('#branchid').text();
    if (id == branchid)
    {
        $.ajax({
            url: "/ClinicalInstitution/RemoveLocaton",
            type: "POST",
            data: {
                Branch_ID: branchid,
            },
            success: function (response) {
                if (response) {
                    $('#RemoveModal').modal('hide');
                    $('#AnswerModel').modal('show');
                    setTimeout(function () { location.reload() }, 3000); 
                   
                }
                else {

                    $('#RemoveModal').modal('hide');
                    $('#AnswerModel').find('.modal-body').find('.success').attr('hidden', true);
                    $('#AnswerModel').find('.modal-body').find('.danger').attr('hidden', false);
                    $('#AnswerModel').modal('show');
                    setTimeout(function () { location.reload() }, 3000); 
                }
            }
        });
};
});
$(document).on('click', '#shiftcategories', function () {
    var Id = $(this).find('.active').find('input').prop('id');
    $('#shiftlist').empty();
    $('#shiftlist').append('<i class="fa fa-spinner fa-spin"></i>');
    $.ajax({
        url: "/ClinicalInstitution/_ClinetShiftpartial",
        type: "POST",
        data: {
            Category_ID: Id,
        },
        success: function (response) {
            $('#shiftlist').empty();
            $('#shiftlist').append(response);
        }
    });
});
$(document).on('click', '#savechanges', function () {


    if ($('#Profile').valid()) {
        $('#savechanges').prop("disabled", true);
        $('#myModal').find('#formid').val('#Profile');
        $('#myModal').modal('show');

    }

});
$(document).on('click', '#cancelchanges', function () {


    location.reload();
});
$(document).on('change', '#Profile_Address', function () {
    var state_id = $("#Profile_State_ID").val();
    var city_id = $("#Profile_City_ID").val();
    var address = $(this).val();
    if (address != "") {
        $.ajax({
            url: "/Home/GetZipCode",
            type: "POST",
            data: {
                City_ID: city_id,
                address: address
            },
            success: function (response) {
                $('#Profile_ZipCode').val(response.zipcode);
                $('#Profile_Longitude').val(response.longitude);
                $('#Profile_Latitude').val(response.latitude);
            }
        });

    }


}); 
$(document).on('click', '#editchanges', function () {
    var inputs = $(document).find('input');
    var selects = $(document).find('select');
   var textareas= $(document).find('textarea');

    $('.changesbuttons').attr("hidden", false);
    $('#editchanges').remove();
    $(this).attr('hidden', false);
    inputs.each(function () {
        $(this).prop('disabled', false);
    });
    $('#Profile_Email').prop('disabled', true);
    $('#Profile_InstitutionTaxId').prop('disabled', true);
    $('#OldPhoneNumber').prop('disabled', true);
    
    Profile.InstitutionTaxId
    selects.each(function () {
        $(this).prop('disabled', false);
    });
    textareas.each(function () {
        $(this).prop('disabled', false);
    });
    $(".selectpicker").selectpicker('refresh');
});
$(document).on('click', '#saveimage', function () {


    if ($('#SaveProfileImage').valid()) {
        $('#saveimage').prop("disabled", true);
        $('#myModal').find('#formid').val('#SaveProfileImage');
        $('#myModal').modal('show');

    }

});

$(document).on('click', '#phoneverify', function () {


    if ($('#NewPhoneNumber').val()) {
        var newphonenumber = $('#NewPhoneNumber').val().toString();
        var oldphonenumber = $(this).data('number').toString();
        if ($.trim(newphonenumber) === $.trim(oldphonenumber)) {
            $('.phonenumerror').append('<span class="text-danger">Error, please try again</span>');
        }
        else {
            $('#phoneverify').prop("disabled", true);

            if ($("#phoneverify").find('i'))
                $("#phoneverify").find('i').remove();
            if ($('#PhoneVerifyModel').find('.modal-body').find('.error')) {
                $('#PhoneVerifyModel').find('.modal-body').find('.error').remove();
            }
            $("#phoneverify").append('<i class="fa fa-spinner fa-spin"></i>');
            var number = $('#NewPhoneNumber').val();
            $.ajax({
                url: "/Account/SendSms",
                type: "POST",
                data: {
                    phonenumber: number
                },
                success: function (response) {
                    if (!response) {
                        $('.phonenumerror').empty();
                        $('.phonenumerror').append('<span class="text-danger">Error, please try again</span>');
                        $('#PhoneVerifyModel').modal('hide');
                        $('#phoneverify').prop("disabled", false);
                        if ($("#phoneverify").find('i'))
                            $("#phoneverify").find('i').remove();

                    }
                    else {
                        $('#PhoneVerifyModel').modal('show');
                        $('.phonenumerror').empty();
                    }
                }
            });
        }
    }
});
$(document).on('click', '#verify', function () {
    var keyanswer = $('#verifykey').val();
    var number = $('#NewPhoneNumber').val();
    $.ajax({
        url: "/Account/VerifySms",
        type: "POST",
        data: {
            verifykey: keyanswer,
            phonenumber: number
        },
        success: function (response) {
            if (response) {
                $("#phoneverify").find('i').remove();
                $('.phonenumerror').empty();
                $('#PhoneVerifyModel').find('.modal-body').empty();
                $("#verify").remove();
                $('#phoneverify').prop("disabled", false);
                $('#PhoneVerifyModel').find('.modal-body').append('<div class="error"><hr/><span class="text-success  text-center">Phone Number successfully Changed</span></div>');
                setTimeout(function () { $("#PhoneVerifyModel").modal('hide'); }, 3000);
                location.reload();

            }
            else {
                if ($('#PhoneVerifyModel').find('.modal-body').find('.error')) {
                    $('#PhoneVerifyModel').find('.modal-body').find('.error').remove();
                }
                $('#PhoneVerifyModel').find('.modal-body').append('<div class="error"><hr/><span class="text-danger  text-center"> Code is not valid, please try again or change your phone number</span></div>');

            }


        },


    });


});
$(document).on('click', '#save', function () {

    var form = $('#myModal').find('#formid').val().toString();
    $('#myModal').find('#save').attr('disabled', true);
    $('#myModal').find('#save').text('');
    $('#myModal').find('#save').append('<i class="fa fa-spinner fa-spin"></i>');
    if ($(form).valid()) {
        $(form).submit()
    }

});
$(document).on('change', '#Profile_State_ID', function () {

    $("#Profile_City_ID option").slice(0).remove();
    $("#Profile_City_ID").selectpicker('refresh');
    value = $(this).val();
    if (value != "") {
        $.ajax({
            url: "/Home/GetCities",
            type: "POST",
            data: {
                stateid: value
            },
            success: function (response) {

                $.each(response, function (i, d) {
                    $("#Profile_City_ID").append("<option value=" + d.id + ">" + d.city_name + "  </option>");
                });

                $("#Profile_City_ID").selectpicker('refresh');
            }
        });
    }



});
$(document).on('click', '#uploadimg', function () {
    $('#Logo').click();
});

$('#SaveModal').on('hidden.bs.modal', function () {
    $('#savechanges').prop("disabled", false);
});
$(document).on('click', '#add-provider', function () {
    var index = $(".bprovider tr").length;
    var selects = $(".bprovider tr td").find('select');
    var array = [];
    selects.each(function () {
        array.push($(this).val());
    });

    var isvalid = true;

    if (isvalid) {
        $.ajax({
            url: "/ClinicalInstitution/_client_profile_speciaties",
            type: "POST",
            data: {
                model: index,
                specIds: array
            },
            success: function (response) {
           
                $(".bprovider").append(response.replace("z", "Specialties_"));
                $(".bprovider tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });
                $('.selectpicker').selectpicker({

                    style: 'btn-default flat'
                });
                $('.selectpicker').selectpicker('refresh');

            }

        });
    }

});
$(document).ready(function () {
    //$('#NewPhoneNumber').usPhoneFormat({
    //    format: '(xxx) xxx-xxxx',
    //});
});