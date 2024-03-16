
console.log("Homejshere");

$('#dashtables').hide();

var target1 = localStorage.getItem('target1')
var state;
var currentpage = 1;
var pagesize = $('#pagesizedropdown').val();
var region = $('.drawbyregiondropdown').val();
var requesttype = $("input[name='requestby']:checked").attr('id');
var search = $('#my-search-input').val();
var filteredcount;
var buttoncount;

console.log("state          :" + state);
console.log("pagesize       :" + pagesize);
console.log("region         :" + region);
console.log("requesttype    :" + requesttype);
console.log("search         :" + search);
console.log("filteredcount  :" + filteredcount);



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
        case '#s_toclose':
            $('#status_text').html("(To Close)")
            state = "Toclose";
            break;
        case '#s_unpaid':
            $('#status_text').html("(Unpaid)")
            state = "Unpaid";
            break;

        default:
            url = '../Home/AdminLogin';
    }
    currentpage = 1;
    filter(state, currentpage, pagesize, requesttype, search, region);

});

$('.dashboardtab[data-bs-target="' + target1 + '"]').trigger('click');

if (target1 == null) {
    $('.dashboardtab[data-bs-target="#s_new"]').trigger('click');

}
$('#dashtables').show();

$('#export').on('click', function () {
    var link = document.createElement('a');
    link.href = 'AdminArea/Dashboard/Export?state=' + state + "&requesttype=" + requesttype + "&search=" + search + "&region=" + region;
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

$('#requestDTY').on('click', function () {
    $.ajax({
        url: 'AdminArea/Dashboard/RequestDTYSupport',
        success: function (result) {

            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});
$("#sendlinkbtn").on('click', function () {
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

$('#SendLinkSubmitBtn').on('click', function () {
    console.log('clicked');
    var firstname = $('#sendlinkfirstname').val();
    var lastname = $('#sendlinklastname').val();
    var mobile = $('#sendlinkmobile').val();
    var email = $('#sendlinkemail').val();
    var flage = true;
    console.log(email);
    if (email == null || email=="") {
        $('#sendlinkemail').css("border", "1px solid red");
        flage = false;
    }
    if (mobile == null || mobile == "") {
        $('#sendlinkmobile').css("border", "1px solid red");
        flage = false;
    }
    if (flage == true) {
        $.ajax({
            url: '/AdminArea/Dashboard/SendEmailFromSendLinkPopUp',
            type: 'POST',
            data: { firstname: firstname, lastname: lastname, email: email, mobile: mobile },
            success: function () {
                location.reload();
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    }
});

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

$('.drawbyregiondropdown').on('change', function () {
    region = $('.drawbyregiondropdown').val();
    currentpage = 1
    filter(state, currentpage, pagesize, requesttype, search, region);
});
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
    if (buttoncount <= 5) {
        for (var i = 1; i <= btn; i++) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + i + '">' + i + '</div > ');
        }
    }
    else {
        if (currentpage >= buttoncount - 3) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + '...' + '">' + '...' + '</div > ');
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
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage - 1) + '">' + (currentpage - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + currentpage + '">' + currentpage + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage + 1) + '">' + (currentpage + 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + '...' + '">' + '...' + '</div > ');
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
    filterwithoutpagination(state, requesttype, search, region);
    $.ajax({
        url: 'AdminArea/StatusWiseData/StatuswiseData',
        type: 'POST',
        data: { state, currentpage, pagesize, requesttype, search, region },
        success: function (response) {
            $('#dashtables').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
    console.log("Filtered");
    console.log("state          :" + state);
    console.log("pagesize       :" + pagesize);
    console.log("region         :" + region);
    console.log("requesttype    :" + requesttype);
    console.log("search         :" + search);
    console.log("filteredcount  :" + filteredcount);
    console.log("currentpage    :" + currentpage)
    console.log("buttoncount    :" + buttoncount)
}
