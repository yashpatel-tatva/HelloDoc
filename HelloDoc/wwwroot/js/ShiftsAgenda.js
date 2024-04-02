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
        $('.drawbyregiondropdown').val(localStorage.getItem('region') || 0);
    }
});
