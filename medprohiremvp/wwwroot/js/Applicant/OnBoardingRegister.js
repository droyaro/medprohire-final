

            $(".phonenumber").mask('(999) 999-9999');
//            $(".SSN").mask(
//                '999-99-9999'
//);
//$("#TIN").mask(
//    '99-9999999'
//);
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
            $(document).on('click', '#trash', function () {
                var trashindex = $(this).closest('tr')[0].sectionRowIndex;
                var tablelength = $(this).closest('tbody').find('tr').length;
                var url = window.location.pathname;
                var action = url.indexOf("Profile");
                var table = $(this).closest('tbody tr').find('tr:gt(' + trashindex + ')').find('input,select,span,button');

                if (tablelength == trashindex + 1) {
                    if (trashindex != 0) {
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
