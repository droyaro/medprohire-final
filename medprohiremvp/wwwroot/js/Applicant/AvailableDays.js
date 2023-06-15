var availabledatatable = $('.availabledatatable').DataTable({
    searching: false,
    info: false,
    pageLength: 5,
    "pagingType": "simple",
    lengthChange: false,
    "dom": '<"d-flex "><"bottom">r<"tablebackground nwtb" t><"p-2 mx-auto pagging"<p>><"clear">',
    "language": {
        "paginate": {
            "previous": '<i class="far fa-angle-left"></i>',
            "next": '<i class="far fa-angle-right"></i>',
        }
    },
    responsive: {
        details: {
            type: 'column',
            renderer: function (api, rowIdx, columns) {
                return false;
            }
        }

    },
    drawCallback: function () {
        var api = this.api();
        $('.datatable').parent().parent().find('.datapageDetails').remove();
        $('.datatable').parent().parent().find('.paginate_button.next').before($('<span>', {
            'text': (api.page() + 1),
            class: 'datapageDetails'
        }));

    },


});
$(".selectpicker").selectpicker({
    style: 'btn-default'
});
$(document).ready(function () {
    $("#StartTime").datetimepicker({

        format: 'LT', pickDate: false,
        defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
        stepping: 15


    });
    $("#EndTime").datetimepicker({

        format: 'LT', pickDate: false,
        defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
        stepping: 15


    });
});

$(".datetimepicker").datetimepicker({




});
$(".datetimepickerday").datepicker({

});

$(document).on('change', '#ShiftLabelId', function () {
    var id = $(this).val();
    if (id == 1) {
        $("#StartTime").datetimepicker("destroy");
        $("#EndTime").datetimepicker("destroy");
        $("#StartTime").find('input').val('');
        $("#EndTime").find('input').val('');
        $("#StartTime").datetimepicker({
            format: 'LT', pickDate: false,
            defaultDate: new Date(0, 0, 0, 6, 0, 0, 0),
            stepping: 15,
            useCurrent: false,
            disabledHours: [0, 1, 2, 3, 4, 5, 16, 17, 18, 19, 20, 21, 22, 23, 24],
            enabledHours: [6, 7, 8, 9, 10, 11, 12, 13, 14, 15]

        });
        $("#EndTime").datetimepicker({
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
            $("#StartTime").datetimepicker("destroy");
            $("#EndTime").datetimepicker("destroy");
            $("#StartTime").find('input').val('');
            $("#EndTime").find('input').val('');

            $("#StartTime").datetimepicker({
                format: 'LT', pickDate: false,
                defaultDate: new Date(0, 0, 0, 14, 0, 0, 0),
                stepping: 15,
                useCurrent: false,
                disabledHours: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 24],
                enabledHours: [14, 15, 16, 17, 18, 19, 20, 21, 22, 23]

            });
            $("#EndTime").datetimepicker({
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
                $("#StartTime").datetimepicker("destroy");
                $("#EndTime").datetimepicker("destroy");
                $("#StartTime").find('input').val('');
                $("#EndTime").find('input').val('');
                $("#StartTime").datetimepicker({
                    format: 'LT', pickDate: false,
                    defaultDate: new Date(0, 0, 0, 22, 0, 0, 0),
                    stepping: 15,
                    useCurrent: false,
                    disabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21],
                    enabledHours: [22, 23, 24, 0, 1, 2, 3, 4, 5, 6, 7]

                });
                $("#EndTime").datetimepicker({
                    format: 'LT', pickDate: false,
                    defaultDate: new Date(0, 0, 0, 7, 0, 0, 0),
                    stepping: 15,
                    useCurrent: false,
                    disabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21],
                    enabledHours: [22, 23, 24, 0, 1, 2, 3, 4, 5, 6, 7]

                });

            }
            else {
                $("#StartTime").datetimepicker("destroy");
                $("#EndTime").datetimepicker("destroy");
                $("#StartTime").find('input').val('');
                $("#EndTime").find('input').val('');
                $("#StartTime").datetimepicker({
                    format: 'LT', pickDate: false,
                    defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
                    stepping: 15


                });
                $("#EndTime").datetimepicker({
                    format: 'LT', pickDate: false,
                    defaultDate: new Date(0, 0, 0, 18, 0, 0, 0),
                    stepping: 15

                });
            }
        }


    }



});