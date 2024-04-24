$('.edit').on('click', function () {
    var id = $(this).data('id');
    console.log(id);
    $.ajax({
        url: '/Home/OpenPatientModel',
        data: { id },
        type: 'POST',
        success: function (result) {
            $('#PopUps').html(result);
            var my = new bootstrap.Modal(document.getElementById('ModalToOpen'));
            my.show();
        }
    })
})

$('.delete').on('click', function () {
    var id = $(this).data('id');
    console.log(id);
    $.ajax({
        url: '/Home/Deletethispatient',
        data: { id },
        type: 'POST',
        success: function (result) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Deleted",
                showConfirmButton: false,
                timer: 1500
            });
            var currentpage = $('.current').data('id');
            var pagesize = $('#pagesizedropdown').val();
            var search = $('#search').val();
            filter(currentpage, pagesize, search) 
        }
    })
})
