//$.ajax({
//    url: '/AdminArea/AdminProviderTab/SetAllPhysican',
//    success: function () { }
//});




















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

$('#createnewshift').on('click', function () {
    $.ajax({
        url: '/AdminArea/AdminProviderTab/CreateShiftPopUp',
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
    });
});
function getfromregion(regionid) {
    $.ajax({
        url: '/AdminArea/AdminProviderTab/GetSchedulerData',
        data: { regionid: regionid },
        type: 'POST',
        success: function (response) {
            $('#SchedulerData').html(response);
        }
    });
}
getfromregion($('.drawbyregiondropdown').val());
$('.drawbyregiondropdown').on('change', function () {
    getfromregion($('.drawbyregiondropdown').val());
});


//var currentDate = new Date();
//var showby = $('input[name="showby"]:checked').attr('id');
//GetData(showby, currentDate);

//$('input[name="showby"]').on('change', function () {
//    showby = $('input[name="showby"]:checked').attr('id');
//    GetData(showby, currentDate);
//})
//function updateText() {
//    var textElement = document.getElementById('timesheet');
//    if (showby == "DayWiseData") {
//        var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
//        textElement.innerText = currentDate.toLocaleDateString('en-US', options);
//    } else if (showby == "WeekWiseData") {
//        var startOfWeek = new Date(currentDate);
//        startOfWeek.setDate(startOfWeek.getDate() - startOfWeek.getDay());
//        var endOfWeek = new Date(startOfWeek);
//        endOfWeek.setDate(endOfWeek.getDate() + 6);
//        var options = { year: 'numeric', month: 'short', day: 'numeric' };
//        textElement.innerText = startOfWeek.toLocaleDateString('en-US', options) + ' - ' + endOfWeek.toLocaleDateString('en-US', options);
//    } else if (showby == "MonthWiseData") {
//        textElement.innerText = currentDate.toLocaleString('default', { month: 'long' }) + ' ' + currentDate.getFullYear();
//    }
//}

//$('#previousbtn').on('click', function () {
//    if (showby == "DayWiseData") {
//        currentDate.setDate(currentDate.getDate() - 1);
//    } else if (showby == "WeekWiseData") {
//        currentDate.setDate(currentDate.getDate() - 7);
//    } else if (showby == "MonthWiseData") {
//        currentDate.setMonth(currentDate.getMonth() - 1);
//    }
//    GetData(showby, currentDate);
//});
//$('#nextbtn').on('click', function () {
//    if (showby == "DayWiseData") {
//        currentDate.setDate(currentDate.getDate() + 1);
//    } else if (showby == "WeekWiseData") {
//        currentDate.setDate(currentDate.getDate() + 7);
//    } else if (showby == "MonthWiseData") {
//        currentDate.setMonth(currentDate.getMonth() + 1);
//    }
//    GetData(showby, currentDate);
//});


//$('#selectday').on('change', function () {
//    var selectedDate = new Date($('#selectday').val());
//    currentDate = selectedDate;
//    console.log(currentDate);
//    GetData(showby, currentDate);
//});


//function GetData(showby, currentDate) {
//    if (showby == "DayWiseData") {
//        $('#selectday').attr('type', 'date');
//    } else if (showby == "WeekWiseData") {
//        $('#selectday').attr('type', 'date');
//    } else if (showby == "MonthWiseData") {
//        $('#selectday').attr('type', 'month');
//    }
//    $.ajax({
//        url: '/AdminArea/AdminProviderTab/' + showby,
//        data: { currentDate: currentDate.toISOString() },
//        type: 'POST',
//        success: function (response) {
//            updateText();
//            $('#timewisedata').html(response);
//        }
//    });
//}