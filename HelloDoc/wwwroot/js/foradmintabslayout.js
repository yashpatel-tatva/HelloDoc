$(document).ready(function () {
    function handleTabClick(target) {
        var url;
        switch (target) {
            case '#nav-home':
                url = '/AdminArea/Dashboard/Dashboard';
                break;
            case '#nav-ProviderLocation':
                url = '/AdminArea/ProviderLocation/ProviderLocation';
                break;
            case '#nav-Profile':
                url = '/AdminArea/AdminProfile/AdminProfile';
                break;
            case '#providers':
                url = '/AdminArea/AdminProviderTab/Providers';
                break;
            case '#scheduling':
                url = '/AdminArea/AdminProviderTab/Scheduling';
                break;
            case '#invoicing':
                url = '/AdminArea/AdminProviderTab/Invoicing';
                break;
            case '#nav-Partners':
                url = '/AdminArea/AdminPartnersTab/Vendors';
                break;
            case '#nav-Access':
                url = '/AdminArea/AccessTab/AccessPage';
                break;
            case '#nav-createadmin':
                url = '/AdminArea/AdminProfile/CreateAdmin';
                break;
            case '#nav_useraccess':
                url = '/AdminArea/AccessTab/UserAcess';
                break;
            case '#patient-records':
                url = '/AdminArea/AdminRecordsTab/PatientHistory';
                break;
            case '#search-records':
                url = '/AdminArea/AdminRecordsTab/SearchRecords';
                break;
            case '#email-logs':
                url = '/AdminArea/AdminRecordsTab/EmailLogs';
                break;
            case '#sms-logs':
                url = '/AdminArea/AdminRecordsTab/SmsLogs';
                break;
            case '#block-history':
                url = '/AdminArea/AdminRecordsTab/BlockHistory';
                break;

            default:
                url = '/AdminArea/Dashboard/Dashboard';
        }

        //$.ajax({
        //    url: url,
        //    success: function (response) {
        //        $('#nav-tabContent').html(response);
        //    },
        //    error: function (xhr, status, error) {
        //        console.error(error);
        //    }
        //});

        var link = document.createElement('a');
        link.href = url;
        link.click();


      

        $('.pdi').on('click', function () {
            if ($(this).hasClass('active')) {
                $('.pd').addClass('active');
            } else {
                $('.pd').removeClass('active');
            }
        });

        if ($('.pdi').hasClass('active')) {
            $('.pd').addClass('active');
        } else {
            $('.pd').removeClass('active');
        }
        $('.rdi').on('click', function () {
            if ($(this).hasClass('active')) {
                $('.rd').addClass('active');
            } else {
                $('.rd').removeClass('active');
            }
        });

        if ($('.rdi').hasClass('active')) {
            $('.rd').addClass('active');
        } else {
            $('.rd').removeClass('active');
        }
        $('.adi').on('click', function () {
            if ($(this).hasClass('active')) {
                $('.ad').addClass('active');
            } else {
                $('.ad').removeClass('active');
            }
        });

        if ($('.adi').hasClass('active')) {
            $('.ad').addClass('active');
        } else {
            $('.ad').removeClass('active');
        }
    }
    function handleTabTriggers() {
        var target = localStorage.getItem('target');
        if (target == null) {
            if ($(window).width() < 760) {
                $('.accordianmaintabs[data-bs-target="#nav-home"]').trigger('click');
            } else {
                $('.maintabs[data-bs-target="#nav-home"]').trigger('click');
            }
        } else {
            if ($(window).width() < 760) {
                $('.accordianmaintabs[data-bs-target="' + target + '"]').trigger('click');
            } else {
                $('.maintabs[data-bs-target="' + target + '"]').trigger('click');
            }
        }
    }
    $('.maintabs').on('click', function (e) {
        e.preventDefault();
        $('.maintabs').removeClass('active');
        $(this).addClass('active');
        var target = $(this).data('bs-target');
        localStorage.setItem('target', target);
        handleTabClick(target);
    });

    $('.accordianmaintabs').on('click', function (e) {
        e.preventDefault();
        $('.accordianmaintabs').removeClass('active');
        $(this).addClass('active');
        var target = $(this).data('bs-target');
        localStorage.setItem('target', target);
        handleTabClick(target);
    });



    console.log(  $('.maintabs[data-bs-target="' + target + '"]'))
    //handleTabTriggers();
    $('.pdi').on('click', function () {
        if ($(this).hasClass('active')) {
            $('.pd').addClass('active');
        } else {
            $('.pd').removeClass('active');
        }
    });

    if ($('.pdi').hasClass('active')) {
        $('.pd').addClass('active');
    } else {
        $('.pd').removeClass('active');
    }
    $('.rdi').on('click', function () {
        if ($(this).hasClass('active')) {
            $('.rd').addClass('active');
        } else {
            $('.rd').removeClass('active');
        }
    });

    if ($('.rdi').hasClass('active')) {
        $('.rd').addClass('active');
    } else {
        $('.rd').removeClass('active');
    }
    $('.adi').on('click', function () {
        if ($(this).hasClass('active')) {
            $('.ad').addClass('active');
        } else {
            $('.ad').removeClass('active');
        }
    });

    if ($('.adi').hasClass('active')) {
        $('.ad').addClass('active');
    } else {
        $('.ad').removeClass('active');
    }
});