﻿
//console.log("Homejshere");

$('#dashtables').hide();

var target1 = localStorage.getItem('target1')
var state;
var currentpage = 1;
var pagesize = $('#pagesizedropdown').val();
var region = 0 ;
var requesttype = $("input[name='requestby']:checked").attr('id');
var search = $('#my-search-input').val();
var filteredcount;
var buttoncount;

//console.log("state          :" + state);
//console.log("pagesize       :" + pagesize);
//console.log("region         :" + region);
//console.log("requesttype    :" + requesttype);
//console.log("search         :" + search);
//console.log("filteredcount  :" + filteredcount);



$('.dashboardtab').on('click', function (e) {
    e.preventDefault();
    $('.dashboardtab').removeClass('active');
    $(this).addClass('active');
    var target1 = $(this).data('bs-target');
    localStorage.setItem('target1', target1);
    switch (target1) {
        case '#s_new':
            $('#status_text').html("(New)")
            state = "New";
            break;
        case '#s_pending':
            $('#status_text').html("(Pending)")
            state = "Pending";
            break;
        case '#s_active':
            $('#status_text').html("(Active)")
            state = "Active";
            break;
        case '#s_conclude':
            $('#status_text').html("(Conclude)")
            state = "Conclude";
            break;
        default:
            url = '/AdminArea/Home/AdminLogin';
    }
    currentpage = 1;
    filter(state, currentpage, pagesize, requesttype, search, region);

});

$('.dashboardtab[data-bs-target="' + target1 + '"]').trigger('click');

if (target1 == null) {
    $('.dashboardtab[data-bs-target="#s_new"]').trigger('click');

}
$('#dashtables').show();


$("#sendlinkbtn").on('click', function () {
    console.log("sendlinkclicked");
    $.ajax({
        url: '/AdminArea/Dashboard/Sendlinktorequest',
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
        error: function (error) {
            console.error('Error saving admin notes:', error);
        }
    });
});


$("#createrequestbtn").on('click', function () {
    $.ajax({
        url: '/AdminArea/Dashboard/CreateRequest',
        success: function (response) {
            $('#nav-tabContent').html(response);
        },
        error: function (error) {
            console.error('Error saving admin notes:', error);
        }
    });
});


// Pagination Functions and Filteration

//$('.drawbyregiondropdown').on('change', function () {
//    region = $('.drawbyregiondropdown').val();
//    currentpage = 1
//    filter(state, currentpage, pagesize, requesttype, search, region);
//});
$('#pagesizedropdown').on('change', function () {
    pagesize = $('#pagesizedropdown').val();
    currentpage = 1
    filter(state, currentpage, pagesize, requesttype, search, region);
});
$("input[name='requestby']").on('change', function () {
    requesttype = $(this).attr('id');
    currentpage = 1
    filter(state, currentpage, pagesize, requesttype, search, region);
});
$("#my-search-input").on('keyup', function () {
    search = $(this).val();
    currentpage = 1
    filter(state, currentpage, pagesize, requesttype, search, region);
})
$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    filter(state, $(this).data('id'), pagesize, requesttype, search, region);
});
$('.paginate_Previousbutton').on('click', function () {
    if (currentpage != 1) {
        currentpage = currentpage - 1;
        filter(state, currentpage, pagesize, requesttype, search, region);
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (buttoncount != currentpage) {
        currentpage = currentpage + 1;
        filter(state, currentpage, pagesize, requesttype, search, region);
    }
});

function filterwithoutpagination(state, requesttype, search, region) {
    $.ajax({
        url: 'AdminArea/StatuswiseData/CountbyFilter',
        type: 'POST',
        data: { state, requesttype, search, region },
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
        $('#tothisrequest').html(data);
    }
    else {
        $('.paginate_Nextbutton').removeClass("d-none");
        if (data == 0) {
            $('#tothisrequest').html(0);
        }
        else {
            $('#tothisrequest').html(((currentpage) * pagesize));
        }
    }
    if (data == 0) {
        $('#fromthisrequest').html(0);
    }
    else {
        $('#fromthisrequest').html((((currentpage - 1) * pagesize) + 1));
    }
    $('#totalcountofrequest').html(data);
}
function filter(state, currentpage, pagesize, requesttype, search, region) {
    //filterwithoutpagination(state, requesttype, search, region);
    //$.ajax({
    //    url: 'AdminArea/StatusWiseData/StatuswiseData',
    //    type: 'POST',
    //    data: { state, currentpage, pagesize, requesttype, search, region },
    //    success: function (response) {
    //        $('#dashtables').html(response);
    //    },
    //    error: function (xhr, status, error) {
    //        console.error(error);
    //    }
    //});
}
