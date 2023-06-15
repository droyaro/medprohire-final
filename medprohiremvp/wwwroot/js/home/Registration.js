var Registration = function () {
    return {

        init: function () {
            Registration.toggleView();
        },


        toggleView: function () {
            if (document.getElementById("applicant")) {
                var appchecked = document.getElementById("applicant").checked;
                ShowAppNames(appchecked);
            }
            if (document.getElementById("clinical")) {
                var clincalchecked = document.getElementById("clinical").checked;
                ShowClNames(clincalchecked);
            }
            $(".selectpicker").selectpicker({
                style: 'btn-default'
            });
            $(document).on('change', '#Country_ID', function () {

                var Id = $(this).val();
                $.ajax({
                    url: "/Home/GetStates",
                    type: "POST",
                    data: {
                        countryid: Id,
                    },
                    success: function (response) {
                        $("#State_ID option").slice(1).remove();
                        $("#City_ID option").slice(1).remove();
                        $.each(response, function (i, d) {
                            $("#State_ID").append("<option value=" + d.id + ">" + d.state_name + "</option>");
                        });
                        $("#State_ID").selectpicker("refresh");
                        $("#City_ID").selectpicker('refresh');

                    }
                });


            });
            $(document).on('change', '#State_ID', function () {
                var Id = $(this).val();
                $.ajax({
                    url: "/Home/GetCities",
                    type: "POST",
                    data: {
                        stateid: Id,
                    },
                    success: function (response) {
                        $("#City_ID option").slice(1).remove();
                        $.each(response, function (i, d) {
                            $("#City_ID").append("<option value=" + d.id + ">" + d.city_name + "</option>");
                        });
                        $("#City_ID").selectpicker('refresh');

                    }
                });


            });
 
            $(document).on('change', '#Address', function () {
                var state_id = $("#State_ID").val();
                var city_id = $("#City_ID").val();
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
                            if (response != null) {
                                $('#ZipCode').val(response.zipcode);
                                $('#Longitude').val(response.longitude);
                                $('#Latitude').val(response.latitude);
                            }
                            else {
                                $('#ZipCode').val("");
                                $('#Longitude').val(0);
                                $('#Latitude').val(0);
                            }
                            
                        },
                    });

                }


            });
            $(document).on('click', ".navigation", function () {
                var href = $(this).data('link');
                var lasthref = $(this).data('lastlink');
                var input = $(this).closest('.active-fileds').find('input:visible').not('.not-reuqired').add($(this).closest('.active-fileds').find('select:visible'));

                var isvalid = $('#Registration').valid();

                //input.each(function () {
                //    if ($(this).val()=="") {
                //        $(this).valid();
                //        isvalid = false;
                //    }
                //});
                if (isvalid) {
                    
                   
                    $(".active-fileds-text").prop('hidden', true);
                   
                    setTimeout(function () { $("." + href + "-text").prop('hidden', false) }, 1000);
                    setTimeout(function () { $("#" + lasthref).prop("hidden", true)}, 1000);
                    setTimeout(function () { $("#" + href).prop("hidden", false) }, 1000);
                    setTimeout(function () { $("." + lasthref + "-back").prop('hidden', false); }, 1000);
                    $("." + lasthref + "-text").prop('hidden', false);
                    $("." + href + "-text").addClass("active");
                    $("." + lasthref + "-text").removeClass("active");
                   
                }
            });
            $(document).on('click', ".lastnavigation", function () {
                var href = $(this).data('link');
                var lasthref = $(this).data('lastlink');
                $(".active-fileds-text").prop('hidden', true);
                $("." + href + "-text").prop('hidden', false);
               
                $("." + href + "-text").addClass("active");
                var last = $("#" + href).data('lastlink');
                $("." + last + "-text").prop('hidden', false);
                $("." + last + "-back").prop('hidden', false);
                    $("#" + href).prop("hidden", false);
                    $("#" + lasthref).prop("hidden", true);
                
            });
            //$(".selectpicker").selectpicker({
            //    style: 'btn-default flat'
            //});

            $(document).on('click', '#applicant', function () {
                var appchecked = $(this).prop('checked');
                ShowAppNames(appchecked);
            });
            $(document).on('click', '#clinical', function () {
                var clincalchecked = $(this).prop('checked');
                ShowClNames(clincalchecked);
            });
            //$(document).ready(function () {
            //    var temp = $("#tempdata").val();
            //    if (temp == "Applicant") {
            //        $("#applicant").attr('disabled', true);
            //        $("#applicant").attr('checked', true);
            //        $("#applicant").parent().removeClass("text-right");
            //        $("#applicant").parent().addClass("text-left");
            //        $("#clinical").parent().remove();
                    
            //    }
            //    if (temp == "Clinical") {
            //        $("#applicant").parent().remove();
            //        $("#clinical").attr('disabled', true);
            //        $("#clinical").attr('checked', true);
            //    }
            //});


            //$(document).on('click', '#sendsms', function () {

            //    if ($('#Registration').valid()) {
            //        $('#notvalid').prop("hidden", true);
            //        $('#sendsms').prop("disabled", true);

            //        if ($("#sendsms").find('i'))
            //            $("#sendsms").find('i').remove();
            //        if ($('.modal-body').find('.error')) {
            //            $('.modal-body').find('.error').remove();
            //        }
            //        $("#sendsms").append('<i class="fa fa-spinner fa-spin"></i>');
            //        var number = $('#Phone').val();
            //        $.ajax({
            //            url: "/Home/SendSms",
            //            type: "POST",
            //            data: {
            //                phonenumber: number
            //            },
            //            success: function (response) {
            //                if (!response) {
            //                    $('#myModal').modal('hide');
            //                    $('#notvalid').prop("hidden", false);
            //                    $('#sendsms').prop("disabled", false);
            //                    if ($("#sendsms").find('i'))
            //                        $("#sendsms").find('i').remove();

            //                }
            //                else {
            //                    $('#myModal').modal('show');
            //                }
            //            }
            //        });
            //    }

            //});
            $(document).on("change", ".Specialty_Types", function () {
                if ($(this).prop('checked')) {
                    $(this).parent().find('.checkboxovallabel').addClass('checked');

                }
                else {
                    $(this).parent().find('.checkboxovallabel').removeClass('checked');

                }
                var Specialty_Types = $(document).find('.Specialty_Types');
                $('#PreferredSpecialites').val('');
                Specialty_Types.each(function () {
                    if ($(this).prop('checked')) {
                        $('#PreferredSpecialites').val($('#PreferredSpecialites').val() + $(this).val() + ';');
                    }
                });
            });
            //$.validator.setDefaults({ ignore: '' });
            $('#myModal').on('show.bs.modal', function () {
                $('#sendsms').prop("disabled", false);
                if ($("#sendsms").find('i'))
                    $("#sendsms").find('i').remove();
                $('#myModal').find('input').val('');
                if ($('.modal-body').find('.error')) {
                    $('.modal-body').find('.error').remove();
                }
            });

            $('.close').on('click', function () {
                $('#sendsms').prop("disabled", false);
                if ($("#sendsms").find('i'))
                    $("#sendsms").find('i').remove();
                if ($('.modal-body').find('.error')) {
                    $('.modal-body').find('.error').remove();
                }
            });
            $('#Phone').on('keyup click', function () {
                $('#notvalid').prop("hidden", true);

            });


            $(document).on('click', '#verify', function () {
                var keyanswer = $('#verifykey').val();
                var number = $('#Phone').val();

                $.ajax({
                    url: "/Home/VerifySms",
                    type: "POST",
                    data: {
                        verifykey: keyanswer,
                        phonenumber: number
                    },
                    success: function (response) {
                        if (response) {
                            $('#submit').click();
                            //$('#myModal').modal('hide');
                            //$('#verifying').remove();
                            //$('#valid').prop("hidden", false);
                            //$('#submit').prop("hidden", false);

                        }
                        else {
                            if ($('.modal-body').find('.error')) {
                                $('.modal-body').find('.error').remove();
                            }
                            $('.modal-body').append('<div class="error"><hr/><span class="text-danger"> Code is not valid, please try again or change your phone number</span></div>');

                        }


                    },


                });


            });
           


        },
    };

}();

Registration.init();
function GetStates(country) {

    $.ajax({
        url: "/Home/GetStates",
        type: "POST",
        data: {
            countryid: country,
        },
        success: function (response) {
            $("#State_ID option").slice(1).remove();
            $("#City_ID option").slice(1).remove();
            $.each(response, function (i, d) {
                $("#State_ID").append("<option value=" + d.id + ">" + d.state_name + "</option>");
            });

        }
    });
}
function GetCities(state) {

    $.ajax({
        url: "/Home/GetCities",
        type: "POST",
        data: {
            stateid: state,
        },
        success: function (response) {
            $("#City_ID option").slice(1).remove();
            $.each(response, function (i, d) {
                $("#City_ID").append("<option value=" + d.id + ">" + d.city_name + "</option>");
            });

        }
    });
}
function ShowAppNames(appchecked) {
    if (appchecked) {
        $('.applicantnames').attr('hidden', false);
        $('.clinicalnames').attr('hidden', true);
  
    }
}
function ShowClNames(clincalchecked) {
    if (clincalchecked) {
        $('.applicantnames').attr('hidden', true);
        $('.clinicalnames').attr('hidden', false);

    }
}
$(document).ready(function () {
    var state = $("#State_ID").val();
    var city = $("#City_ID").val();
    var number = "";
    if ($('#Phone').val()) {
        number = $('#Phone').val();
    }
    //$('#Phone').usPhoneFormat({
    //    format: '(xxx) xxx-xxxx',
    //}); 
    if (state != "" & city == "") {
        $.ajax({
            url: "/Home/GetCities",
            type: "POST",
            data: {
                stateid: state,
            },
            success: function (response) {
                $("#City_ID option").slice(1).remove();
                $.each(response, function (i, d) {
                    $("#City_ID").append("<option value=" + d.id + ">" + d.city_name + "</option>");
                });
                $("#City_ID").selectpicker('refresh');

            }
        });
    }

    //if (number.length>0) {
    //    $.ajax({
    //        url: "/Home/IsVerified",
    //        type: "POST",
    //        data: {
    //            phonenumber: number,
    //        },
    //        success: function (response) {
    //            if (response) {
    //                $('#myModal').modal('hide');
    //                $('#verifying').remove();
    //                $('#valid').prop("hidden", false);
    //                $('#submit').prop("hidden", false);


    //            }
    //            else {

    //                $('#notvalid').prop("hidden", false);
    //                $('#submit').prop("hidden", true);
    //            }

    //        }
    //    });
    //}

});
$(document).ready(function () {

    $(window).keydown(function (event) {

        if (event.keyCode == 13) {

            event.preventDefault();

            return false;

        }

    });

});