$('span.pie').peity('pie', {
    fill: ['#1ab394', '#d7d7d7', '#ffffff']
});

$(document).on('click', '#shiftquearter', function () {
    var querter = $(this).find('.active').find('input').prop('id');
    //$.ajax({
    //    url: "/ClinicalInstitution/Shi",
    //    type: "POST",
    //    data: {
    //        countryid: Id,
    //    },
    //    success: function (response) {
    //        $("#State_ID option").slice(1).remove();
    //        $("#City_ID option").slice(1).remove();
    //        $.each(response, function (i, d) {
    //            $("#State_ID").append("<option value=" + d.id + ">" + d.state_name + "</option>");
    //        });
    //        $("#State_ID").selectpicker("refresh");
    //        $("#City_ID").selectpicker('refresh');

    //    }
    //});
    $('.allshift').prop('hidden', true);
    $('.yearshift').prop('hidden', true);
    $('.Q1shift').prop('hidden', true);
    $('.Q2shift').prop('hidden', true);
    $('.Q3shift').prop('hidden', true);
    $('.Q4shift').prop('hidden', true);
    $('.' + querter + 'shift').prop('hidden', false);
});
$(document).on('click', '#locationsquerter', function () {
    var querter = $(this).find('.active').find('input').prop('id');
    //$.ajax({
    //    url: "/ClinicalInstitution/Shi",
    //    type: "POST",
    //    data: {
    //        countryid: Id,
    //    },
    //    success: function (response) {
    //        $("#State_ID option").slice(1).remove();
    //        $("#City_ID option").slice(1).remove();
    //        $.each(response, function (i, d) {
    //            $("#State_ID").append("<option value=" + d.id + ">" + d.state_name + "</option>");
    //        });
    //        $("#State_ID").selectpicker("refresh");
    //        $("#City_ID").selectpicker('refresh');

    //    }
    //});
    $('.allLocations').prop('hidden', true);
    $('.yearLocations').prop('hidden', true);
    $('.Q1Locations').prop('hidden', true);
    $('.Q2Locations').prop('hidden', true);
    $('.Q3Locations').prop('hidden', true);
    $('.Q4Locations').prop('hidden', true);
    $('.' + querter + 'Locations').prop('hidden', false);
});