$('.events').on('click', function () {
    console.log($(this).data('id'));
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ViewShiftPopUp',
        data: { ShiftDetailId: $(this).data('id') },
        type: 'POST',
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        }
    });
})