$.ajax({
    url: '/AdminArea/Dashboard/GetRegion',
    success: function (data) {
        var drawbyregiondropdown = $('.drawbyregiondropdown');
        $.each(data, function (index, regions) {
            drawbyregiondropdown.append($('<option>', {
                value: regions.regionid,
                text: regions.name
            }))
        });
    }
});

var region = 0;
var order = 0;

$.ajax({
    url: '/AdminArea/AdminProviderTab/Providersfilter?region=' + region + '&order=' + order,
    success: function (response) {
        $('#providerslist').html(response);
    },
    error: function (xhr, status, error) {
        console.error(error);
    }
});

$('.drawbyregiondropdown').on('change', function () {
    region = $('.drawbyregiondropdown').val();
    $.ajax({
        url: '/AdminArea/AdminProviderTab/Providersfilter?region=' + region + '&order=' + order,
        success: function (response) {
            $('#providerslist').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('#ordericon').on('click', function () {
    if (order == 0) {
        order = 1;
        $('#ordericon').html('Provider Name <i class="bi bi-arrow-down"></i>');
    }
    else {
        order = 0;
        $('#ordericon').html('Provider Name <i class="bi bi-arrow-up"></i>');

    }
    $.ajax({
        url: '/AdminArea/AdminProviderTab/Providersfilter?region=' + region + '&order=' + order,
        success: function (response) {

            $('#providerslist').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
});

$('#createaccountpage').on('click', function () {
    console.log('clicked')
    $.ajax({
        url: '/AdminArea/AdminProviderTab/CreateAccountPage',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});