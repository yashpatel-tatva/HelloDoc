$('#backtoedit').on('click', function () {
    var physicianid = $('#physicianid').val();
    $.ajax({
        url: '/AdminArea/AdminProviderTab/EditProviderPage',
        type: 'POST',
        data: { physicianid: physicianid },
        success: function (result) {
            $('#nav-tabContent').html(result);
        }
    });
})

$('.choosefield').on('click', function () {
    if ($(this).text() == "Submit") {
        console.log('hit')
        var physicianid = $('#physicianid').val();
        var rate = $('.' + $(this).attr('id')).val();
        console.log(physicianid , rate)
        $.ajax({
            url: '/AdminArea/AdminProviderTab/Edit' + $(this).attr('id') +'payrate',
            data: { physicianid, rate },
            async:false,
            type: 'POST',
            success: function (response) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Payrate updated",
                    showConfirmButton: false,
                    timer: 1000
                });
                $('#nav-tabContent').html(response);
            }
        })
    }
    else if ($(this).text() == "Edit") {
        $('.' + $(this).attr('id')).prop('readonly', false);
        $(this).text("Submit")
    }
})