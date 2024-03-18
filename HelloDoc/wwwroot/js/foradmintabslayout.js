$(document).ready(function () {
    function handleTabClick(target) {
        var url;
        switch (target) {
            case '#nav-home':
                url = 'AdminArea/Dashboard/Dashboard';
                break;
            case '#nav-ProviderLocation':
                url = 'AdminArea/Dashboard/Status_Pending';
                break;
            case '#nav-Profile':
                url = 'AdminArea/AdminProfile/AdminProfile';
                break;
            case '#providers':
                url = 'AdminArea/AdminProviderTab/Providers';
                break;
            case '#scheduling':
                url = 'AdminArea/AdminProviderTab/Scheduling';
                break;
            case '#invoicing':
                url = 'AdminArea/AdminProviderTab/Invoicing';
                break;
            case '#nav-Partners':
                url = 'AdminArea/Dashboard/Status_Toclose';
                break;
            case '#nav-Access':
                url = 'AdminArea/Dashboard/Status_Unpaid';
                break;
            case '#nav-Records':
                url = 'AdminArea/Dashboard/Status_Unpaid';
                break;
            default:
                url = 'AdminArea/Home/AdminTabsLayout';
        }

        $.ajax({
            url: url,
            success: function (response) {
                $('#nav-tabContent').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
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

    handleTabTriggers();
    $('.dropdown-item').on('click', function () {
        if ($(this).hasClass('active')) {
            $('.dropdown').addClass('active');
        } else {
            $('.dropdown').removeClass('active');
        }
    });

    if ($('.dropdown-item').hasClass('active')) {
        $('.dropdown').addClass('active');
    } else {
        $('.dropdown').removeClass('active');
    }

    //$(window).resize(function () {
    //    handleTabTriggers();
    //});
});