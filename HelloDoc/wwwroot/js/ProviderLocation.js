var locationdata;
$.ajax({
    url: '/AdminArea/ProviderLocation/ProviderLocationJson',
    async: false,
    success: function (data) {
        locationdata = JSON.parse(data);
    }
});


var sumLat = 0, sumLong = 0;
for (var i = 0; i < locationdata.length; i++) {
    sumLat += locationdata[i].Lat;
    sumLong += locationdata[i].Long;
}
var avgLat = sumLat / locationdata.length;
var avgLong = sumLong / locationdata.length;


var map = L.map('map').setView([avgLat, avgLong], 5);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {}).addTo(map);

for (var i = 0; i < locationdata.length; i++) {
    var iconHtml = '<div class="d-flex" style="width: 30px; height: 30px; border-radius: 50%; overflow: hidden; border: 4px solid #008000;">' +
        '<img src="' + locationdata[i].Photo + '" style="width: 100%; height: auto;" />' +
        '</div>' +
        '<div style="width: 0; height: 0; border-left: 10px solid transparent; border-right: 10px solid transparent; border-top: 15px solid #008000; margin-left : 5px ;margin-top: -4px;"></div>';

    var customIcon = L.divIcon({
        className: 'custom-icon',
        html: iconHtml,
        iconSize: [30, 45], // size of the icon
        iconAnchor: [15, 45], // point of the icon which will correspond to marker's location
    });

    var popupContent = '<img class="openeditphy" data-id="' + locationdata[i].Physicianid + '" width = "60%" src="' + locationdata[i].Photo + '" />' +
        '<p>Physician: ' + locationdata[i].Name + '</p>';
    var marker = L.marker([locationdata[i].Lat, locationdata[i].Long], { icon: customIcon }).addTo(map)
        .bindPopup(popupContent);

    marker.on('popupopen', function (e) {
        $('.openeditphy').on('click', function () {
            var physicianid = ($(this).data('id'));
            $.ajax({
                url: '/AdminArea/AdminProviderTab/EditProviderPage',
                type: 'POST',
                data: { physicianid: physicianid },
                success: function (result) {
                    $('#nav-tabContent').html(result);
                }
            });
        });
    });
}