var role = $('#selectrole').val();
var rname = $('#rname').val();
var mobile = $('#mobile').val();
var createddate = $('#createddate').val();
var sentdate = $('#sentdate').val();

var pagesize = 10;
var currentpage = 1;
var buttoncount;

function getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize) {
    filterDatawithoutpagination(role, rname, mobile, createddate, sentdate);
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/SMSLogsdata',
        data: { role: role, rname: rname, mobile: mobile, createddate: createddate, senddate: sentdate, currentpage: currentpage, pagesize: pagesize },
        type: 'POST',
        success: function (response) {
            $('#_records').html(response);
        }
    });
}

getdata(0, "", "", "", "", 1, pagesize);

$('#selectrole').on('change', function () {
    role = $(this).val();
    currentpage = 1;
    getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
});

$('#mobile').on('input', function () {
    mobile = $(this).val();
    currentpage = 1;
    getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
})

$('#rname').on('input', function () {
    rname = $(this).val();
    currentpage = 1;
    getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
})

$('#submitdate').on('click', function () {
    createddate = $('#createddate').val(); 
    sentdate = $('#sentdate').val();
    currentpage = 1;
    getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
})



$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
});
$('.paginate_Previousbutton').on('click', function () {
    if (currentpage != 1) {
        currentpage = currentpage - 1;
        getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (buttoncount != currentpage) {
        currentpage = currentpage + 1;
        getdata(role, rname, mobile, createddate, sentdate, currentpage, pagesize);
    }
});

function filterDatawithoutpagination(role, rname, mobile, createddate, sentdate) {
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/SMSContbyFilter',
        type: 'POST',
        data: { role: role, rname: rname, mobile: mobile, createddate: createddate, senddate: sentdate },
        success: function (data) {
            printbuttons(data);
            $('.paginate_button').removeClass("current");
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
