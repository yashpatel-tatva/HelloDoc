
console.log("Homejshere");
$(document).ready(function () {
    $('#dashtables').hide();

    var target1 = localStorage.getItem('target1')
    $('.dashboardtab').on('click', function (e) {
        e.preventDefault();
        $('.dashboardtab').removeClass('active');
        $(this).addClass('active');
        var target1 = $(this).data('bs-target');
        localStorage.setItem('target1', target1);
        var url;
        switch (target1) {
            case '#s_new':
                $('#status_text').html("(New)")
                url = 'AdminArea/StatusWiseData/Status_New';
                break;
            case '#s_pending':
                $('#status_text').html("(Pending)")
                url = 'AdminArea/StatusWiseData/Status_Pending';
                break;
            case '#s_active':
                $('#status_text').html("(Active)")
                url = 'AdminArea/StatusWiseData/Status_Active';
                break;
            case '#s_conclude':
                $('#status_text').html("(Conclude)")
                url = 'AdminArea/StatusWiseData/Status_Conclude';
                break;
            case '#s_toclose':
                $('#status_text').html("(To Close)")
                url = 'AdminArea/StatusWiseData/Status_Toclose';
                break;
            case '#s_unpaid':
                $('#status_text').html("(Unpaid)")
                url = 'AdminArea/StatusWiseData/Status_Unpaid';
                break;

            default:
                url = '../Home/AdminLogin';
        }

        $.ajax({
            url: url,

            success: function (response) {
                $('#dashtables').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    $('.dashboardtab[data-bs-target="' + target1 + '"]').trigger('click');

    if (target1 == null) {
        $('.dashboardtab[data-bs-target="#s_new"]').trigger('click');

    }
    $('#dashtables').show();
});

$('#export').on('click', function () {
    var thisstate = $(".dashboardtab.active");
    var statuscollection = thisstate.map(function () {
        return this.id;
    }).get();
    var status = statuscollection[0];
    var link = document.createElement('a');
    link.href = 'AdminArea/Dashboard/Export?status=' + status;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
});


$('#exportall').on('click', function () {
    var link = document.createElement('a');
    link.href = 'AdminArea/Dashboard/ExportAll';
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
});

$.ajax({
    url: 'AdminArea/Dashboard/GetRegion',
    success: function (data) {
        var drawbyregiondropdown = $('.drawbyregiondropdown');
        $.each(data, function (index, regions) {
            drawbyregiondropdown.append($('<option>', {
                value: regions.name,
                text: regions.name
            }))
        });
    }
});