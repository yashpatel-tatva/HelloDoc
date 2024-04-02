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