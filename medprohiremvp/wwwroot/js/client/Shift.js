var Shift = function () {

    return {

        init: function () {
            Shift.Create();
        },

        Create: function () {
            $(document).ready(function () {
                $("#datetimepicker").datetimepicker({
                    format: 'LT', pickDate: false,
                    defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
                    stepping: 15


                });
                $("#datetimepicker1").datetimepicker({
                    format: 'LT', pickDate: false,
                    defaultDate: new Date(0, 0, 0, 18, 0, 0, 0),
                    stepping: 15

                });
            });

            $('#calendarview').datepicker({ startDate: '0' }).on('changeDate', dateChanged);
            var startdate = $('#StartDate').val();
            var enddate = $('#EndDate').val();

            $(".selectpicker").selectpicker({
                style: 'btn-default'
            });
            $(document).ready(function () {
                var dates = $('#ShiftsDates').val();
                if (dates != "") {
                    $('#calendarview').datepicker('setDates', dates.split(','));
                }
            });
            $('.shiftdatepicker').datepicker({ startDate: '0' });
            $(document).on('change', '#ShiftLabel_ID', function () {
                var id = $(this).val();
                if (id == 1) {
                    $("#datetimepicker").datetimepicker("destroy");
                    $("#datetimepicker1").datetimepicker("destroy");
                    $("#datetimepicker").find('input').val('');
                    $("#datetimepicker1").find('input').val('');
                    $("#datetimepicker").datetimepicker({
                        format: 'LT', pickDate: false,
                        defaultDate: new Date(0, 0, 0, 6, 0, 0, 0),
                        stepping: 15,
                        useCurrent: false,
                        disabledHours: [0, 1, 2, 3, 4, 5, 16, 17, 18, 19, 20, 21, 22, 23, 24],
                        enabledHours: [6, 7, 8, 9, 10, 11, 12, 13, 14, 15]

                    });
                    $("#datetimepicker1").datetimepicker({
                        format: 'LT', pickDate: false,
                        defaultDate: new Date(0, 0, 0, 15, 0, 0, 0),
                        stepping: 15,
                        useCurrent: false,
                        disabledHours: [0, 1, 2, 3, 4, 5, 16, 17, 18, 19, 20, 21, 22, 23, 24],
                        enabledHours: [6, 7, 8, 9, 10, 11, 12, 13, 14, 15]

                    });
                }
                else {
                    if (id == 2) {
                        $("#datetimepicker").datetimepicker("destroy");
                        $("#datetimepicker1").datetimepicker("destroy");
                        $("#datetimepicker").find('input').val('');
                        $("#datetimepicker1").find('input').val('');

                        $("#datetimepicker").datetimepicker({
                            format: 'LT', pickDate: false,
                            defaultDate: new Date(0, 0, 0, 14, 0, 0, 0),
                            stepping: 15,
                            useCurrent: false,
                            disabledHours: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 24],
                            enabledHours: [14, 15, 16, 17, 18, 19, 20, 21, 22, 23]

                        });
                        $("#datetimepicker1").datetimepicker({
                            format: 'LT', pickDate: false,
                            defaultDate: new Date(0, 0, 0, 22, 0, 0, 0),
                            stepping: 15,
                            useCurrent: false,
                            disabledHours: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 24],
                            enabledHours: [14, 15, 16, 17, 18, 19, 20, 21, 22, 23]

                        });

                    }
                    else {
                        if (id == 3) {
                            $("#datetimepicker").datetimepicker("destroy");
                            $("#datetimepicker1").datetimepicker("destroy");
                            $("#datetimepicker").find('input').val('');
                            $("#datetimepicker1").find('input').val('');
                            $("#datetimepicker").datetimepicker({
                                format: 'LT', pickDate: false,
                                defaultDate: new Date(0, 0, 0, 22, 0, 0, 0),
                                stepping: 15,
                                useCurrent: false,
                                disabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21],
                                enabledHours: [22, 23, 24, 0, 1, 2, 3, 4, 5, 6, 7]

                            });
                            $("#datetimepicker1").datetimepicker({
                                format: 'LT', pickDate: false,
                                defaultDate: new Date(0, 0, 0, 7, 0, 0, 0),
                                stepping: 15,
                                useCurrent: false,
                                disabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21],
                                enabledHours: [22, 23, 24, 0, 1, 2, 3, 4, 5, 6, 7]

                            });

                        }
                        else {
                            $("#datetimepicker").datetimepicker("destroy");
                            $("#datetimepicker1").datetimepicker("destroy");
                            $("#datetimepicker").find('input').val('');
                            $("#datetimepicker1").find('input').val('');
                            $("#datetimepicker").datetimepicker({
                                format: 'LT', pickDate: false,
                                defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
                                stepping: 15


                            });
                            $("#datetimepicker1").datetimepicker({
                                format: 'LT', pickDate: false,
                                defaultDate: new Date(0, 0, 0, 18, 0, 0, 0),
                                stepping: 15

                            });
                        }
                    }


                }



            });
            $(document).on('click', '#SelectAlldays', function () {
                if ($(this).prop('checked')) {

                    var between = [];
                    var currentDate;
                    currentDate = new Date(startdate);
                    while (currentDate <= new Date(enddate)) {
                        between.push(currentDate.toLocaleDateString());
                        currentDate.setDate((new Date(currentDate)).getDate() + 1);
                    }
                    $('.datepickershift').datepicker('setDates', between);


                }
                else {
                    $('.datepickershift').datepicker('update', '');
                }
            });
            $(document).on('change', '#StartDate', function () {
                var value = $('#StartDate').val();
                $('#EndDate').datepicker('setDate', null);
                $('#EndDate').datepicker('setStartDate', value);
            });
            $(document).on('click', '.removedate', function () {
                var shift_id = $(this).parent().data('id');
                var text = $(this).parent().remove();
                var date = $('#pickeddays').find('.pickday');
                var between = [];
                date.each(function () {
                    between.push($(this).text());
                });
                if (between.length > 0) {
                    $('#calendarview').datepicker('setDates', between);
                }
                else {
                    $('#calendarview').datepicker('setDates', '');

                }
            });

        },
    };
}();
Shift.Create();
function dateChanged(ev) {

    var date = $("#calendarview").datepicker("getDates");
    var selecteddays = [];

    $('#ShiftsDates').val('');

    $('#pickeddays').empty();
    if (date.length > 0) {


        $('#pickeddays').attr('hidden', false);

        $.each(date, function (i, val) {
            var year = val.getFullYear();
            var month = (1 + val.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;
            var day = val.getDate().toString();
            day = day.length > 1 ? day : '0' + day;
            selecteddays.push(month + '/' + day + '/' + year);
            $('#pickeddays').append('<span class=pickday>' + month + '/' + day + '/' + year + ' <a class="removedate"> <i class="mdi mdi-window-close"></i></a></span>')
        });
        if (selecteddays.length > 0) {
            $('#ShiftsDates').val(selecteddays);
        }
        else {
            $('#ShiftsDates').val('');
        }
    }



}
function AutoGrowTextArea(textField) {
    if (textField.clientHeight < textField.scrollHeight) {
        textField.style.height = textField.scrollHeight + "px";
        if (textField.clientHeight < textField.scrollHeight) {
            textField.style.height =
                (textField.scrollHeight * 2 - textField.clientHeight) + "px";
        }
    }
}


