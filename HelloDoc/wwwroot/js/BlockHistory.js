var pname = $('#pname').val();
var email = $('#emailid').val();
var createddate = $('#createddate').val();
var mobile = $('#mobile').val();

var pagesize = 5;
var currentpage = 1;
var order = true;
var buttoncount;

function getdata(pname, email, createddate, mobile , currentpage, pagesize , order) {
    filterDatawithoutpagination(pname, email, createddate, mobile) 
    $.ajax({
        url: '/AdminArea/AdminRecordsTab/BlockHistorydata',
        data: { pname: pname, email: email, createddate: createddate, mobile: mobile, currentpage: currentpage, pagesize: pagesize, order: order },
        type: 'POST',
        success: function (response) {
            $('#_records').html(response);
            if (order) {
                $('.bi').addClass("bi-arrow-up")
                $('.bi').removeClass("bi-arrow-down")
            }
            else {
                $('.bi').addClass("bi-arrow-down")
                $('.bi').removeClass("bi-arrow-up")
            }
        }
    });
}

getdata("", "", "", "", 1, pagesize , order);


$('#submitdate').on('click', function () {
    createddate = $('#createddate').val(); 
    email = $('#emailid').val();
    pname = $('#pname').val();
    mobile = $('#mobile').val();
    currentpage = 1;
    getdata(pname, email, createddate, mobile , currentpage, pagesize , order);
})
$('#clearbtn').on('click', function () {
    getdata("", "", "", "", 1, pagesize, order);
});
$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    getdata(pname, email, createddate, mobile , currentpage, pagesize , order);
});
$('.paginate_Previousbutton').on('click', function () {
    if (currentpage != 1) {
        currentpage = currentpage - 1;
        getdata(pname, email, createddate, mobile , currentpage, pagesize , order);
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (buttoncount != currentpage) {
        currentpage = currentpage + 1;
        getdata(pname, email, createddate, mobile , currentpage, pagesize , order);
    }
});

function filterDatawithoutpagination(pname, email, createddate, mobile) {
    $.ajax({
        url: 'AdminArea/AdminRecordsTab/BlockHistorydataCount',
        type: 'POST',
        data: { pname: pname, email: email, createddate: createddate, mobile: mobile },
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
    if (buttoncount <= 5) {
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
