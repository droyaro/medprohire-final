$(".selectpicker").selectpicker({
    style: 'btn-default'
});
$(document).ready(function () {
    $('span.pie').peity('pie', {
        fill: ['#1a85c7', '#d7d7d7', '#ffffff'],
        height: 30,
        radius: 20,
        width: 30

    });
    var datatable = $('.datatable').DataTable({
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
                    return false;}
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
    var paycheckdatatable = $('.paycheckdatatable').DataTable({
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
                type: 'inline',
                renderer: function (api, rowIdx, columns) {
                    $('.paycheckdatatable tr').find('td').removeClass('lasttd');
                    $('.paycheckdatatable tr').find('td:visible:last').addClass('lasttd');
                    var data = $.map(columns, function (col, i) {
                        return col.hidden ?
                            '<div class="col-6 my-group text-left" data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' +
                            '<div class=""><label class="control-label">' + col.title + ':' + '</label></div> ' +
                            '<div class="my-group-sm flex-wrap"><label class="main-label">' + col.data + '</label></div>' +
                            '</div>' :
                            '';
                    }).join('');

                    return data ?
                        $('<div class="row text-left">').append(data) :
                        false;
                }
            }
        },
        drawCallback: function () {
            var api = this.api();
            $('.paycheckdatatable').parent().parent().find('.peycheckpageDetails').remove();
            $('.paycheckdatatable').parent().parent().find('.paginate_button.next').before($('<span>', {
                'text': (api.page() + 1),
                class: 'peycheckpageDetails'
            }));

        }

    });
});


$('.dashboardReadMore').click(function () {
    var id = $(this).data('id');
    $.ajax({
        url: "/Applicant/_pickedshiftdetail",
        type: "POST",
        data: {
            id: id
        },
        success: function (response) {
            $("#ShiftModal").find('.modal-body').empty();
            var size = $(document).width();
            var right = -((size * 16.7 / 100) - 170);
            if (size < 1600) {
                right = -((size * 16.7 / 100) - 220)
            }
            if (size < 1200) {
                right = 0;
            }
            $("#ShiftModal").find('.modal-dialog').attr('style', "right: " + right + "px;");
            $("#ShiftModal").find('.modal-body').append(response);
            $("#ShiftModal").modal('show');
        }
    });
});
$('.activeshiftview').click(function () {
    var id = $(this).data('id');
    $.ajax({
        url: "/Applicant/_activeshiftdetail",
        type: "POST",
        data: {
            id: id
        },
        success: function (response) {
            $("#ActiveShiftModal").find('.modal-body').empty();
            $("#ActiveShiftModal").find('.modal-body').append(response);
            $("#ActiveShiftModal").modal('show');
        }
    });
});
$('.countrow.availabledates').click(function () {
  
    $.ajax({
        url: "/Applicant/_applicantAvailableDays",
        type: "POST",
        data: {

        },
        success: function (response) {
            $("#AvailableDays").find('.modal-body').empty();
            $("#AvailableDays").find('.modal-body').append(response);
        ;
            var availabledatatable = $('.availabledatatable').DataTable({
                searching: false,
                info: false,
                pageLength: 5,
                lengthChange: false,
                    "dom": '<"d-flex "><"bottom">r<"tablebackground nwtb" t><"p-2 mx-auto pagging"<p>><"clear">',
                "language": {
                    "paginate": {
                        "previous": '<i class="far fa-angle-left"></i>',
                        "next": '<i class="far fa-angle-right"></i>',
                    }
                }
            });
            $(".selectpicker").selectpicker({
                style: 'btn-default'
            });
            $("#StartTime").datetimepicker({
                template: 'modal',
                format: 'LT', pickDate: false,
                defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
                stepping: 15


            });
            $("#EndTime").datetimepicker({
                template: 'modal',
                format: 'LT', pickDate: false,
                defaultDate: new Date(0, 0, 0, 9, 0, 0, 0),
                stepping: 15


            });
            $(".datetimepickerday").datepicker({
               
            });
            $("#AvailableDays").modal('show');
        }
    });
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
$(document).on('click', '.addavailableday', function () {
    if ('#AddAvailableDays'.valid()) {
        '#AddAvailableDays'.submit();
    } 
});
$(".countrow").not(".availabledates").click(function () {
    var href = $(this).data("href");
    document.location = href;
});