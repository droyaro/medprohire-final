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
            $(document).on('change', '#VisaStatus_ID', function () {
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
            //$(".phonenumber").usPhoneFormat({
            //    format: '(xxx) xxx-xxxx',
            //});
                
            //#endregion show Iseligble
            //#region add-speciality
            $(document).on('click', '#add-speciality', function () {

                var index = $("#bspecalities tr").length;
               
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

                                style: 'btn-default flat'
                            });
                            $('.selectpicker').selectpicker('refresh');
                            $(".datepickerday-issue").datepicker({
                              
                                endDate: 'M',
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
                            $("#bcertificates tr:last").find('input.hasdatepicker').each(function () { $(this).addClass('datepickercert'); });
                            $("#bcertificates tr:last").find('select').each(function () { $(this).addClass('selectpicker'); });
                            $('.selectpicker').selectpicker({
                                style: 'btn-default flat'
                            });
                            $('.selectpicker').selectpicker('refresh');
                            $(".datepickercert").datepicker("refresh");

                        }

                    });
                }

            });
            //#endregion add certificate

            //#region add workhistory
            $(document).on('click', '#add-workhistory', function () {
                var index = $("#bworkhistory tr").length;
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
                                endDate: 'M',
                            });
                            $(".datepicker").datepicker("refresh");
                        }

                    });
                }

            });
            $(document).on('change', "input[name*='StartDate']", function () {
                var value = $(this).val();
                if ($(this).hasClass('datepickercert')) {
                    var endid = $(this).prop('id').replace("StartDate", "EndDate");
                    $("#" + endid).datepicker('destroy');

                    $("#" + endid).datepicker({

                        startDate: value,
                        
                    });
                }
                else {
                    var endid = $(this).prop('id').replace("StartDate", "EndDate");
                    $("#" + endid).datepicker('destroy');

                    $("#" + endid).datepicker({

                        startDate: value,
                        endDate: 'M',
                    });
                }
            });
            //#endregion add workhistory
            $(document).on('click', '.until', function () {
                if ($(this).prop('checked')) {
                    var index = $(this).closest('tr')[0].sectionRowIndex;
                    ($(this).closest('tr').find("input[name*='EndDate']")).val('');
                    $(this).closest('tr').find("input[name*='EndDate']").attr('disabled', true);
                    $(this).closest('tr').find("input[name*='EndDate']").closest(".form-group").find("span.text-danger").find("span").remove();
                    
                    var rows = $(this).closest('tbody').find('tr').not($(this).closest('tr')).find('input[type=checkbox]');
                    rows.each(function () {
                        if ($(this).prop('checked')) {
                            $(this).prop('checked', false);
                            $(this).closest('tr').find("input[name*='EndDate']").prop('disabled', false);
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

                                style: 'btn-default flat'
                            });
                            $('.selectpicker').selectpicker('refresh');
                            $(".phonenumber").usPhoneFormat({
                                format: '(xxx) xxx-xxxx',
                            });
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
  
            $(document).on('click', 'tbody tr td .bootstrap-select', function () {


                $(this).closest(".form-group").find("span.text-danger").hide();

            });
            //#endregion removing 
            //#region document.ready
            $(document).ready(function () {
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
//function myBlurFunction() {
//    if ($(this).val()) {
//        $(this).focus();
//        $(this).next("span").remove();
//    }
//    else {
//        $(this).after('<span class="text-danger">This field is required</span>');
//    }
//    }


