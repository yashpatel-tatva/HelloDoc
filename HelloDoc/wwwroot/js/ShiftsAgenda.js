var region;
var date;
var datetoshow;
var showby;
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
        $('.drawbyregiondropdown').val($('#regionfm').val());
        region = $('.drawbyregiondropdown').val();
    }
});

$('#monthview').on('click', function () {
    localStorage.setItem('showby', "MonthWiseData");
    $.ajax({
        url: '/AdminArea/AdminProviderTab/Scheduling',
        success: function (response) {
            $('#nav-tabContent').html(response);
        }
    });
});

region = $('#regionfm').val();
date = $('#datetoshowfm').val();
datetoshow = new Date(date);
showby = $('#showbyfm').val();

function filterdata(datetoshow, region, showby, currentpage) {
    localStorage.setItem('region', region);
    localStorage.setItem('currentDate', datetoshow);
    localStorage.setItem('showby', showby); 
    console.log(region);
    console.log(date);
    console.log(datetoshow);
    console.log(showby);
    filterDatawithoutpagination(datetoshow, region, showby)
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ShiftDataForAgenda',
        data: { datetoshow: datetoshow.toISOString(), region: region, showby: showby, currentpage: currentpage },
        type: 'POST',
        success: function (response) {
            $('#_RequestedShifts').html(response);
        },
    });
}

// pagination
var pagesize = 10;
var currentpage = 1;
var buttoncount;
$('.drawbyregiondropdown').on('change', function () {
    region = $(this).val();
    currentpage = 1;
    filterdata(datetoshow, region, showby, currentpage);
})


filterdata(datetoshow, region, showby, currentpage);
$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    filterdata(datetoshow, region, showby, currentpage);
});
$('.paginate_Previousbutton').on('click', function () {
    if (currentpage != 1) {
        currentpage = currentpage - 1;
        filterdata(datetoshow, region, showby, currentpage);
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (buttoncount != currentpage) {
        currentpage = currentpage + 1;
        filterdata(datetoshow, region, showby, currentpage);
    }
});

function filterDatawithoutpagination(datetoshow, region, showby) {
    console.log(datetoshow, region, showby);
    var status = 1;
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ShiftCountbyFilter',
        type: 'POST',
        data: { datetoshow: datetoshow.toISOString(), region: region, showby: showby, status: status },
        success: function (data) {
            printbuttons(data);
            $('.paginate_button').removeClass("current");
            console.log(currentpage);
            $('.paginate_button[data-id="' + currentpage + '"]').addClass("current");
        }
    });
}

function printbuttons(data) {
    buttoncount = Math.ceil(data / pagesize);
    $('.dataTables_paginate').html("");
    var btn = Math.min(5, buttoncount);
    var j = currentpage;
    if (buttoncount == 0) {
        $('.dataTables_paginate').html("No Records Found");
    }
    else if (buttoncount <= 5) {
        for (var i = 1; i <= btn; i++) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + i + '">' + i + '</div > ');
        }
    }
    else {
        if (currentpage >= buttoncount - 3) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 3) + '">' + (buttoncount - 3) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 2) + '">' + (buttoncount - 2) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 1) + '">' + (buttoncount - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else if (currentpage < 4) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 2 + '">' + 2 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 3 + '">' + 3 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 4 + '">' + 4 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" >' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage - 1) + '">' + (currentpage - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + currentpage + '">' + currentpage + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage + 1) + '">' + (currentpage + 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
    }
    if (currentpage == 1) {
        $('.paginate_Previousbutton').addClass("d-none");
    }
    else {
        $('.paginate_Previousbutton').removeClass("d-none");
    }
    if (buttoncount == currentpage) {
        $('.paginate_Nextbutton').addClass("d-none");
        $('#tothisshift').html(data);
    }
    else {
        $('.paginate_Nextbutton').removeClass("d-none");
        if (data == 0) {
            $('#tothisshift').html(0);
        }
        else {
            $('#tothisshift').html(((currentpage) * pagesize));
        }
    }
    if (data == 0) {
        $('#fromthisshift').html(0);
    }
    else {
        $('#fromthisshift').html((((currentpage - 1) * pagesize) + 1));
    }
    $('#totalcountofshift').html(data);
}

$("#deleteselected").on('click', function () {
    var shiftdetailsid = [];
    $.each($('input[name="selectedshift"]:checked'), function (index, item) {
        shiftdetailsid.push($(this).data('id'));
    });
    $.ajax({
        url: '/AdminArea/AdminProviderTab/DeleteSelecetdShifts',
        data: { shiftdetailsid: shiftdetailsid},
        type: 'POST',
        success: function (response) {
            filterdata(datetoshow, region, showby, currentpage);
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Shifts Deleted",
                showConfirmButton: false,
                timer: 1000
            });
        },
    })
})

$("#approveselected").on('click', function () {
    var shiftdetailsid = [];
    $.each($('input[name="selectedshift"]:checked'), function (index, item) {
        shiftdetailsid.push($(this).data('id'));
    });
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ApproveSelecetdShifts',
        data: { shiftdetailsid: shiftdetailsid},
        type: 'POST',
        success: function (response) {
            filterdata(datetoshow, region, showby, currentpage);
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Shifts Approved",
                showConfirmButton: false,
                timer: 1000
            });
        },
    })
})