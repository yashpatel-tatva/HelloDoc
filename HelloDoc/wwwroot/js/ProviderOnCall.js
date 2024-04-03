$('#calenderviewbtn').on('click', function () {
    $.ajax({
        url: 'AdminArea/AdminProviderTab/Scheduling',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});

$('.drawbyregiondropdown').on('change', function () {
    var region = $('.drawbyregiondropdown').val();
    localStorage.setItem('region', region);
    var currentDate = localStorage.getItem('currentDate') ? new Date(localStorage.getItem('currentDate')) : new Date();
    var showby = localStorage.getItem('showby') || $('input[name="showby"]:checked').attr('id');
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ProviderOnCall',
        data: { datetoshow: currentDate.toISOString(), region: region, showby: showby },
        type: 'POST',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});

$('.physiciandetail').on('click', function () {
    var str = $(this).data('id');
    var parse = str.split("_");
    var physicianid = parse[1];
    $.ajax({
        url: '/AdminArea/AdminProviderTab/EditProviderPage',
        type: 'POST',
        data: { physicianid: physicianid },
        success: function (result) {
            $('#nav-tabContent').html(result);
        }
    });
});


$('#agendaview').on('click', function () {
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ShiftsAgenda',
        data: { datetoshow: currentDate.toISOString(), region: region, showby: showby },
        type: 'POST',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});