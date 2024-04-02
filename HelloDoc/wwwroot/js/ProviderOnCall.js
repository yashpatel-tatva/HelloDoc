$('#calenderviewbtn').on('click', function () {
    $.ajax({
        url: 'AdminArea/AdminProviderTab/Scheduling',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});