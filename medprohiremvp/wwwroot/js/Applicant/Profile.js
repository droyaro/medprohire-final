var Applicant = function () {
    var workrow;
    var certrow;
    var specrow;
    first = true;
    return {

        init: function () {
            Applicant.Create();
        },

        Create: function () {
            //#region show Iseligble
            $(document).on('change', '#Profile_VisaStatus_ID', function () {
                if ($(this).val() == 3) {
                    $('#othervisa').prop('hidden', false);
                }
                else {
                    $('#othervisa').prop('hidden', true);
                    $('#yes').prop('checked', false);
                    $('#no').prop('checked', false);

                }
            });
            $(".selectpicker").selectpicker({
                style: 'btn-default'
            });
            //#endregion show Iseligble
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
            //#region add-speciality
            $(document).on('click', '#add-speciality', function () {

                var index = $("#bspecalities tr").length;
                var length = 0;
                if ($("#bspecalities tr").find('input[type=hidden]').length > 0) {
                    length = $("#bspecalities tr").find('input[type=hidden]').last().data('count');
                    index = index - length;
                };

                var input = $("#bspecalities tr td").find('input').add($("#bspecalities tr td").find('select'));
                var isvalid = true;
                input.each(function () {
                    if (!$(this).val()) {
                        $(this).focus();
                        $(this).closest(".form-group").find("span.text-danger").find("span").remove();
                        $(this).closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                        $(this).closest(".form-group").find("span.text-danger").show();
                        isvalid = false;
                    }

                });
                if (isvalid) {
                    $.ajax({
                        url: "/Applicant/_ApplicantSpeciality",
                        type: "POST",
                        data: { model: index },
                        success: function (response) {
                            $("#bspecalities").append(response);
                            $("#bspecalities tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });

                            $('.selectpicker').selectpicker({

                                style: 'btn-default'
                            });
                            $('.selectpicker').selectpicker('refresh');
                            $(".datepickerday-issue").datepicker({
                                format: "mm-yyyy",
                                startView: "months",
                                minViewMode: "months",

                            });
                            $(".datepickerday-issue").datepicker("refresh");

                        }

                    });
                }

            });
            //#endregion add-speciality

            //#region add certificate
            $(document).on('click', '#add-certificate', function () {

                var index = $("#bcertificates tr").length;
                var length = 0;
                if ($("#bcertificates tr").find('input[type=hidden]').length > 0) {
                    length = $("#bcertificates tr").find('input[type=hidden]').last().data('count');
                    index = index - length;
                };
                var input = $("#bcertificates tr td").find('input').add($("#bcertificates tr td").find('select'));
                var isvalid = true;
                input.each(function () {
                    if (!$(this).val()) {
                        $(this).focus();
                        $(this).closest(".form-group").find("span.text-danger").find("span").remove();
                        $(this).closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                        $(this).closest(".form-group").find("span.text-danger").show();
                        isvalid = false;
                    }
                    else { $(this).next("span").remove(); }
                });
              
                if (isvalid) {
                    $.ajax({
                        url: "/Applicant/_ApplicantCeritificates",
                        type: "POST",
                        data: { model: index },
                        success: function (response) {
                            $("#bcertificates").append(response);
                            $("#bcertificates tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });
                            $('.selectpicker').selectpicker({
                                style: 'btn-default '
                            });
                            $('.selectpicker').selectpicker('refresh');

                        }

                    });
                }

            });
            //#endregion add certificate

            //#region add workhistory
            $(document).on('click', '#add-workhistory', function () {
                var index = $("#bworkhistory tr").length;
                var length = 0;
                if ($("#bworkhistory tr").find('input[type=hidden]').length > 0) {
                    length = $("#bworkhistory tr").find('input[type=hidden]').last().data('count');
                    index = index - length;
                };
                
                var input = $("#bworkhistory tr td").find('input').add($("#bworkhistory tr td").find('select'));
                var isvalid = true;
                input.each(function () {
                    if (!$(this).val()) {
                        if ($(this).attr('id').match('EndDate')) {
                            if ($(this).closest('tr').find('.until').prop('checked')) { return true; }
                        };
                        $(this).focus();
                        $(this).closest(".form-group").find("span.text-danger").find("span").remove();
                        $(this).closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                        $(this).closest(".form-group").find("span.text-danger").show();
                        isvalid = false;
                    }
                    else { $(this).closest(".form-group").find("span.text-danger").find("span").remove(); }
                });
                if (isvalid) {
                    $.ajax({
                        url: "/Applicant/_ApplicantWorkHistory",
                        type: "POST",
                        data: { model: index },
                        success: function (response) {

                            $("#bworkhistory").append(response);
                            $("#bworkhistory tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });
                            $("#bworkhistory tr:last").find('input.hasdatepicker').each(function () { $(this).addClass('datepicker'); });
                            $('.selectpicker').selectpicker({

                                style: 'btn-default flat'
                            });
                            $('.selectpicker').selectpicker('refresh');
                            $(".datepicker").datepicker({
                                format: "mm-yyyy",
                                startView: "months",
                                minViewMode: "months",
                                endDate: 'M',
                            });
                            $(".datepicker").datepicker("refresh");
                        }

                    });
                }

            });
           
            $(document).on('change', "input[name*='StartDate']", function () {
                var value = $(this).val();
                var endid = $(this).prop('id').replace("StartDate", "EndDate");
                $("#" + endid).datepicker('destroy');
                $("#" + endid).datepicker({
                    format: "mm-yyyy",
                    startView: "months",
                    minViewMode: "months",
                    endDate: 'M',
                    startDate: value,
                });
            });
            //#endregion add workhistory
            $(document).on('click', '.until', function () {
                if ($(this).prop('checked')) {
                    var index = $(this).closest('tr')[0].sectionRowIndex;
                    ($(this).closest('tr').find("input[name*='EndDate']")).val('');
                    $(this).closest('tr').find("input[name*='EndDate']").attr('disabled', true);
                    var rows = $(this).closest('tbody').find('tr').not($(this).closest('tr')).find('input[type=checkbox]');
                    rows.each(function () {
                        if ($(this).prop('checked')) {
                            $(this).prop('checked', false);
                            if ($(this).closest('tr').find("input[type=hidden]").length>0) {
                                $(this).closest('tr').find("input[type=hidden]").prop('', false);
                            };
                            $(this).closest('tr').find("input[name*='EndDate']").prop('disabled', false);
                            $(this).closest('tr').find("input[type=checkbox]").prop('disabled', false);
                            $(this).closest('tr').find("input[name*='EndDate']").focus();
                            $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").find("span").remove();
                            $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                            $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").show();
                        };
                    });
                }
                else {
                    $(this).closest('tr').find("input[name*='EndDate']").prop('disabled', false);
                    $(this).closest('tr').find("input[name*='EndDate']").focus();
                    $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").find("span").remove();
                    $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                    $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").show();
                }

            });
            $(document).on('click', "#available", function () {
                if ($(this).prop('checked')) {
                    $('.availablelist').attr('hidden', false);
                    
                }
                else {
                    
                    $('#myModal').find('#formid').val('#MakeAvailable');
                    $('#myModal').find('.modal-body').find('p').remove();
                    $('#myModal').find('.modal-body').append('<p>Are you sure you want to make yourself unavailable ? All your previously claimed available days will be deleted.</p>');
                    $('#myModal').modal('show');
                  
                }
                $('#changes').attr('hidden', false);
            });
            $(document).on('click', '.AvailableType_ID', function () {
                var id = $(this).data('id');
                var value = $(this).data('value').toLowerCase();
                if ($(this).prop('checked')) {
                    $('.makechanges_' + id).prop('hidden', false);
                    $('#calendarview_' + id).prop('hidden', false);
                    $('#changes_' + id).prop('hidden', false);
                }
                else {
                    if (value!="false") {
                        $('.makechanges_' + id).prop('hidden', false);
                        $('#calendarview_' + id).prop('hidden', true);
                        $('#changes_' + id).prop('hidden', false);
                    }
                    else {
                        $('.makechanges_' + id).prop('hidden', true);
                        $('#calendarview_' + id).prop('hidden', true);
                        $('#changes_' + id).prop('hidden', true);
                    }
                }

            });
            $(document).on("click", "#submitavailable", function () {
                if ($('#MakeAvailable').valid()) {
                    $('#submitavailable').prop("disabled", true);
                    $('#AvailableModel').find('#formid').val('#MakeAvailable');
                    $('.datepickershift').datepicker({
                        startDate: 'd',

                    });
                    var dates = $('#calendarview').datepicker('getFormattedDate');
                    if (dates.length > 0) {
                        $('.datepickershift').datepicker('setDates', dates.split(','));
                        var datearray = dates.split(',');
                        for (var key in datearray) {
                            $('#pickeddays').append('<span class=pickday>' + datearray[key] + ' <a href="#" class="removedate"> <i class="mdi mdi-window-close"></i></a></span>');
                        };
                    }
                    else {
                        $('.datepickershift').datepicker('setDates', 'd');
                        var d = new Date();

                        var month = d.getMonth() + 1;
                        var day = d.getDate();

                        var output = (month < 10 ? '0' : '') + month + '/' +
                            (day < 10 ? '0' : '') + day + '/' + d.getFullYear();
                        $('#pickeddays').append('<span class=pickday>' + output + ' <a href="#" class="removedate"> <i class="mdi mdi-window-close"></i></a></span>');
                    }
                    $("#pickeddays").attr("hidden", false);
                  
                    $('#AvailableModel').modal('show');

                }
            });
            $(document).on("click", "#removeavailable", function () {
                if ($('#MakeAvailable').valid()) {
                    $('#removeavailable').prop("disabled", true);
                    $('#myModal').find('#formid').val('#MakeAvailable');
                    $('#myModal').modal('show');

                }
            });
            $(document).on('click', '#IsOneDayAvailable', function () {
                $('#IsTodayAvailableOnly').prop('checked', $(this).prop('checked'));
            });
            $(document).on('click', '#cancelchanges', function () {
                location.reload();
            });
            //referenc table add button
            //#region referenc
            $(document).on('click', '#add-reference', function () {
                var index = $("#breferences tr").length;
                var input = $("#breferences tr td").find('input').add($("#breferences tr td").find('select'));
                var isvalid = true;
                input.each(function () {
                    if (!$(this).val()) {
                        $(this).focus();
                        $(this).closest(".form-group").find("span.text-danger").find("span").remove();
                        $(this).closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                        $(this).closest(".form-group").find("span.text-danger").show();

                        isvalid = false;
                    }
                    else { $(this).next("span").remove(); }
                });
                if (isvalid) {
                    $.ajax({
                        url: "/Applicant/_ApplicantReference",
                        type: "POST",
                        data: { model: index },
                        success: function (response) {

                            $("#breferences").append(response);
                            $("#breferences tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });
                            $('.selectpicker').selectpicker({

                                style: 'btn-default '
                            });
                            $('.selectpicker').selectpicker('refresh');
                        }

                    });
                }

            });
            //#endregion referenc
            //removing span from table on input change
            //#region removing 
           
            $(document).on('keyup', 'tbody tr td input', function () {
                if (!$(this).val()) {
                        $(this).closest(".form-group").find("span.text-danger").find("span").remove();
                    $(this).closest(".form-group").find("span.text-danger").append('<span>' + $(this).attr("data-val-required") + '</span>');
                    $(this).closest(".form-group").find("span.text-danger").show();
                }
                else {
                    $(this).closest(".form-group").find("span.text-danger").hide();
                }
            });
            $(document).on('blur','input[id*=Date]', function () {
                if (!$(this).val()) {

                }
                else {
                    $(this).closest(".form-group").find("span.text-danger").hide();
                }
            });

            //$(document).on('click', '#calendar', function () {
            //    $('.datepickershift').datepicker({
            //        startDate: 'd',
                   
            //    });
            //    $('.datepickershift').on('changeDate', dateChanged);
            //    $('.datepickershift').datepicker('show');
            //    $("#pickeddays").attr("hidden", false);
            
                

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
                    $('.datepickershift').datepicker('setDates', '');
                }
         
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
            $('.modal').on('hidden.bs.modal', function (e) {
                location.reload();
            });
            $(document).on('click', 'tbody tr td .bootstrap-select', function () {


                $(this).closest(".form-group").find("span.text-danger").hide();

            });
            //#endregion removing 
            //#region document.ready
            $(document).ready(function () {
                //$('#NewPhoneNumber').usPhoneFormat({
                //    format: '(xxx) xxx-xxxx',
                //});
                $(".selectpicker").selectpicker({
                    style: 'btn-default'
                });
                var days = $('#ApplicantAvailableDays').val();
                $('.datepickershift').datepicker({
                    startDate: 'd',
                });
                var datepickershift = $('.datepickershift');
                datepickershift.each(function () {
                    var days = $(this).data('value');
                    if (days) {
                        if (days.length > 0) {
                            $(this).datepicker('setDates', days.split(',')).on('changeDate', dateChanges);
                        }
                    }
                  
                });
              
              
                //$('#calendarview').datepicker('setDates', days.split(',')).on('changeDate', dateChanges);
                //$('#calendarview').datepicker('refresh');

                if ($('#VisaStatus_ID').val() == 3) {
                    $('#othervisa').prop('hidden', false);
                }
                var workindex = $("#bworkhistory tr").length;
                if (workindex == 1) {
                    var inputwork = $("#bworkhistory tr td").find('input').add($("#bworkhistory tr td").find('select'));

                    inputwork.each(function () {
                        $(this).attr('data-val', false);
                        $(this).attr('aria-required', false);

                    });
                }
                var certindex = $("#bcertificates tr").length;
                if (certindex == 1) {
                    var inputcert = $("#bcertificates tr td").find('input').add($("#bcertificates tr td").find('select'));

                    inputcert.each(function () {
                        //$(this).attr('data-val', false);
                        $(this).attr('aria-required', false);

                    });

                }

            });
            //#endregion document.ready
            $(document).on('click', '#uploadimg', function () {
                $('#ProfileImage').click();
            });

            $(document).on('click', '#cropimg', function () {
                $('#cropimg').attr('hidden', true);
                $('#savecrop').attr('hidden', false);
                $('.resizing').attr('style', 'margin-left:' + 10 + 'px; margin-top:' + 10 + 'px;')
                  var cropper = $('#image-cropper').cropper({
                    viewMode: 2,
                    movable: false,
                    zoomable: false,
                    maxContainerHeight: 350,
                    maxContainerWidth: 350,
                    minCanvasHeight: 100,
                    minCanvasWidth: 100,
                    preview: ".crop_preview",
                });
            });
            // #region trash
            $(document).on('click', '#trash', function () {
                var trashindex = $(this).closest('tr')[0].sectionRowIndex;
                var tablelength = $(this).closest('tbody').find('tr').length;
                var url = window.location.pathname;
                var action = url.indexOf("Profile");
                var table = $(this).closest('tbody tr').find('tr:gt(' + trashindex + ')').find('input,select,span,button');

                if (tablelength == trashindex + 1) {
                    if (trashindex != 0 ) {
                        $(this.closest('tbody')).find($(this).closest('tr')).remove();
                    }
                    else {
                        if (action >= 0) {
                            $(this.closest('tbody')).find($(this).closest('tr')).remove();
                        }
                        else {
                            var row = $(this).closest('tbody').find('tr:eq(' + trashindex + ')').find('input,select');
                            row.each(function () {
                                if ($(this).val()) {
                                    $(this).val('');
                                }

                            });
                    
                            $('.selectpicker').selectpicker('refresh');
                            $(".datepicker").datepicker("refresh");
                        }
                    }
                }
                else {
                    $(this.closest('tbody')).find($(this).closest('tr')).remove();
                    table.each(function () {
                        if ($(this).attr('id')) {
                            $(this).attr('id', $(this).attr('id').replace(/\d+/, ($(this).attr('id').match(/\d+/)) - 1));
                        };
                        if ($(this).attr('name')) {
                            $(this).attr('name', $(this).attr('name').replace(/\d+/, ($(this).attr('name').match(/\d+/)) - 1));
                        };

                        if ($(this).attr('data-valmsg-for')) {
                            $(this).attr('data-valmsg-for', $(this).attr('data-valmsg-for').replace(/\d+/, ($(this).attr('data-valmsg-for').match(/\d+/)) - 1));
                        };
                    });
                };

            });
            //#endregion trash
            $(document).on('click', '#savechanges', function () {


                if ($('#Profile').valid()) {
                    $('#savechanges').prop("disabled", true);
                    $('#myModal').find('#formid').val('#Profile');
                    $('#myModal').modal('show');
                   
                }

            });
            $(document).on('click', '#savespacialities', function () {


                if ($('#AddSpeciality').valid()) {
                    $('#savechanges').prop("disabled", true);
                    $('#myModal').find('#formid').val('#AddSpeciality');
                    $('#myModal').modal('show');

                }

            });
            $(document).on('click', '#saveworkhistory', function () {


                if ($('#AddWorkHistory').valid()) {
                    $('#savechanges').prop("disabled", true);
                    $('#myModal').find('#formid').val('#AddWorkHistory');
                    $('#myModal').modal('show');

                }

            });
            $(document).on('click', '#savecertificates', function () {


                if ($('#AddCertificates').valid()) {
                    $('#savechanges').prop("disabled", true);
                    $('#myModal').find('#formid').val('#AddCertificates');
                    $('#myModal').modal('show');

                }

            });
            $(document).on('click', '#dismissmodal', function () {
                location.reload();
            });
            $(document).on('click', '#saveimage', function () {


                if ($('#SaveProfileImage').valid()) {
                    $('#saveimage').prop("disabled", true);
                    $('#myModal').find('#formid').val('#SaveProfileImage');
                    $('#myModal').modal('show');

                }

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
            $(document).on('click', '.availabledays', function () {
                var id = $(this).data('id');
               
                var days = $('#calendarview_' + id).datepicker('getFormattedDate');
                var value = $('#ApplicantAvailableTypes_' + id).prop('checked');
                $.ajax({
                    url: "/Applicant/UpdateAvailableDates",
                    type: "POST",
                    data: {
                        days: days,
                        AvailableType_ID: id,
                        Value: value
                    },
                    success: function (response) {

                        if (response) {
                            $('#AnswerModel').modal('show');
                        }
                        else {
                            $('#AnswerModel').find('.modal-body').find('.success').attr('hidden', true);
                            $('#AnswerModel').find('.modal-body').find('.danger').attr('hidden', false);
                            $('#AnswerModel').find('.modal-body').find('.danger span').append("Changes error, please try again");
                            $('#AnswerModel').modal('show');
                        }
                       
                    }
                });

            });
            $(document).on('click', '#saveavailable', function () {
                if ($('#AppliedDaysList').val() != "") {
                    $('#ApplicantAvailableDays').val($('#AppliedDaysList').val());
                    var form = $('#AvailableModel').find('#formid').val().toString();
                    $('#AvailableModel').find('#saveavailable').attr('disabled', true);
                    $('#AvailableModel').find('#saveavailable').text('');
                    $('#AvailableModel').find('#saveavailable').append('<i class="fa fa-spinner fa-spin"></i>');
                if ($(form).valid()) {
                    $(form).submit()
                    }
                }

            });
            $(document).on('click', '.ApplicantAvailability', function () {
                var id = $(this).data('id');
                var ApplicantAvailability = $('.ApplicantAvailability');
                ApplicantAvailability.each(function () {
                    $(this).removeClass('ApplicantAvailableActive');
                });
                $(this).addClass('ApplicantAvailableActive');
                $(".makechanges_" + id).attr('hidden', false);
                var datepickershift = $('.datepickershift');
                datepickershift.each(function () {
                    $(this).attr('hidden', true);
                });
                var checked = $('#ApplicantAvailableTypes_' + id).prop('checked');
                if (checked) {
                    $("#calendarview_" + id).attr('hidden', false);
                }

            });
            $('#myModal').on('hidden.bs.modal', function () {
                $('#savechanges').prop("disabled", false);
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
            //$(document).on('click', '#save', function () {
            //    $('#Profile').submit();
            //});
        },
    };
}();
Applicant.Create();

function GetSpecality() {
    var options = "";
    $.ajax({
        url: "/Home/GetSpecalities",
        type: "POST",
        data: {},
        success: function (response) {

            $.each(response, function (i, d) {
                options = options + "<option value=" + d.specialty_ID + ">" + d.specialty_Name + "</option>";
            });
        }

    });

}
function dateChanged(ev) {

    var date = $(".datepickershift").datepicker("getDates");

    $('#pickeddays').empty();
    if (date.length > 0) {
        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;

            $('#pickeddays').append('<span class=pickday>' + month + '/' + day + '/' + year + ' <a href="#" class="removedate"> <i class="mdi mdi-window-close"></i></a></span>')
        });
    }



}
function dateChanges(ev) {
    var id = ev.target.dataset.id;
    $('.makechanges_' + id).prop('hidden', false);
    $('#calendarview_' + id).prop('hidden', false);
    $('#changes_' + id).prop('hidden', false);

}
//function myBlurFunction() {
//    if ($(this).val()) {
//        $(this).focus();
//        $(this).next("span").remove();
//    }
//    else {
//        $(this).after('<span class="text-danger">This field is required</span>');
//    }
//    }


