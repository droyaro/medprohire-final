var apptable;
var certificates_datatable = $('.certificates_datatable').DataTable({
    autoWidth: false,
    ordering: false,
    searching: false,
    info: false,
    pageLength: 5,
    "pagingType": "numbers",
    lengthChange: false,
    "dom": '<"d-flex "><"bottom">r<"tablebackground nwtb" t><"p-2 mx-auto datatablepagination pagging"<p>><"clear">',
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
                $('.certificates_datatable tr').find('td').removeClass('lasttd');
                $('.certificates_datatable tr').find('td:visible:last').addClass('lasttd');
                var data = $.map(columns, function (col, i) {
                    return col.hidden ?
                        '<div class="col-md-6 col-sm-12 my-group text-left" data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' +
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
        $('.certificates_datatable').parent().parent().find('.datapageDetails').remove();
        $('.certificates_datatable').parent().parent().find('.paginate_button.next').before($('<span>', {
            'text': (api.page() + 1),
            class: 'datapageDetails'
        }));
        if ($('.certificates_datatable').parent().parent().find('.dataTables_paginate li.page-item').length <= 1) {
            $('.certificates_datatable').parent().parent().find('.dataTables_paginate').hide();
        }

    }

});
var workhistory_datatable = $('.workhistory_datatable').DataTable({
    autoWidth: false,
    ordering: false,
    searching: false,
    info: false,
    pageLength: 5,
    "pagingType": "numbers",
    lengthChange: false,
    "dom": '<"d-flex "><"bottom">r<"tablebackground nwtb" t><"p-2 mx-auto datatablepagination pagging"<p>><"clear">',
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
                $('.workhistory_datatable tr').find('td').removeClass('lasttd');
                $('.workhistory_datatable tr').find('td:visible:last').addClass('lasttd');
                var data = $.map(columns, function (col, i) {
                    return col.hidden ?
                        '<div class="col-md-6 col-sm-12 my-group text-left" data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' +
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
        $('.workhistory_datatable').parent().parent().find('.datapageDetails').remove();
        $('.workhistory_datatable').parent().parent().find('.paginate_button.next').before($('<span>', {
            'text': (api.page() + 1),
            class: 'datapageDetails'
        }));
        if ($('.workhistory_datatable').parent().parent().find('.dataTables_paginate li.page-item').length <= 1) {
            $('.workhistory_datatable').parent().parent().find('.dataTables_paginate').hide();
        }

    }

});
var specalities_datatable = $('.specalities_datatable').DataTable({
    autoWidth: false,
    ordering: false,
    searching: false,
    info: false,
    pageLength: 5,
    "pagingType": "numbers",
    lengthChange: false,
    "dom": '<"d-flex "><"bottom">r<"tablebackground nwtb" t><"p-2 mx-auto datatablepagination pagging"<p>><"clear">',
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
                $('.specalities_datatable tr').find('td').removeClass('lasttd');
                $('.specalities_datatable tr').find('td:visible:last').addClass('lasttd');
                var data = $.map(columns, function (col, i) {
                    return col.hidden ?
                        '<div class="col-md-6 col-sm-12 my-group text-left" data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' +
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
        $('.specalities_datatable').parent().parent().find('.datapageDetails').remove();
        $('.specalities_datatable').parent().parent().find('.paginate_button.next').before($('<span>', {
            'text': (api.page() + 1),
            class: 'datapageDetails'
        }));
        if ($('.specalities_datatable').parent().parent().find('.dataTables_paginate li.page-item').length <= 1) {
            $('.specalities_datatable').parent().parent().find('.dataTables_paginate').hide();
        }

    }

});

$(document).ready(function () {
    var clockindays = $('#notclockindays').val();
    if (clockindays.length > 0) {
        var dates = clockindays.split(',');
        $('.fullpickdates').datepicker({
            beforeShowDay: function () {
                return false;
            }
        });
        $('.fullpickdates').datepicker('setDates', dates);
            //your code here
      
    }
  
    $(".selectpicker").selectpicker({
        style: 'btn-default'
    });
   
        $('span.pie').peity('pie', {
            fill: ['#1a85c7', '#d7d7d7', '#ffffff'],
            height: 30,
            radius: 20,
            width: 30

    });
    var betweenfull = [];
    var currentDate;
    var startdate = $('#StartDate').val();
    var enddate = $('#EndDate').val();
    var dates = $('#ShiftsDates').val();
    if (dates != "") {
        var dayslist = dates.split(',');
        currentDate = new Date(startdate);
        while (currentDate <= new Date(enddate)) {

            var day = currentDate.toLocaleDateString('es-PA');
            if (!dayslist.includes(day)) {
                betweenfull.push(currentDate.toLocaleDateString('es-PA'));
            }
            currentDate.setDate((new Date(currentDate)).getDate() + 1);
        }
        $('.shiftdatelist').datepicker({
            beforeShowDay: function () {
                return false;
            }
        });
        $('.shiftdatelist').datepicker('setDatesDisabled', betweenfull).datepicker('setDates', dayslist);



    }

});

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    var href = $(this).attr('href').replace('#', '.');
    $(href + '_datatable').DataTable()
        .columns.adjust()
        .responsive.recalc();
 
});


//new Chart(document.getElementById("bar-chart"), {
//    type: 'pie',
//    data: {
//        labels: ["Work","Must Work"],
//        datasets: [
//            {
//                fill: true,
//                backgroundColor: [
//                    'green',
//                    'white'],
                
//                data: [75,25]
//            }
//        ]
//    },
//    options: {
//        legend: { display: false },
//        title: {
//            display: false,
           
//        }
//    }
//});