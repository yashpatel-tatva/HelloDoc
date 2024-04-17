$(document).ready(function () {
    function handleTabClick(target) {
        var url;
        switch (target) {
            case '#nav-home':
                url = '/ProviderArea/Dashboard/Dashboard';
                break;
            case '#nav-invoice':
                url = '/ProviderArea/Invoice/Invoice';
                break;
            case '#nav-schedule':
                url = '/ProviderArea/Schedule/Schedule';
                break;
            case '#nav-profile':
                url = '/ProviderArea/ProviderProfile/ProviderProfile';
                break;
            default:
                url = '/ProviderArea/Dashboard/Dashboard';
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

    }
    function handleTabTriggers() {
        var target = localStorage.getItem('target3');
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
        localStorage.setItem('target3', target);
        handleTabClick(target);
    });

    $('.accordianmaintabs').on('click', function (e) {
        e.preventDefault();
        $('.accordianmaintabs').removeClass('active');
        $(this).addClass('active');
        var target = $(this).data('bs-target');
        localStorage.setItem('target3', target);
        handleTabClick(target);
    });

    //handleTabTriggers();
});