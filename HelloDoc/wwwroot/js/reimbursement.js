
$('.fileinput').on('change', function () {
    var id = $(this).data('id');
    var fileInput = document.querySelector('#file_' + id);
    var fileChosen = document.getElementById('file-chosen_' + id);
    var file = fileInput.files[0];
    var maxSize = 2 * 1024 * 1024; // 2MB
    if (file.size > maxSize) {
        Swal.fire('File size must be less than 2MB');
        this.value = '';
        return;
    }
    filesnames = file.name;
    fileChosen.textContent = filesnames;
    filesname = fileChosen.textContent;
    fileChosen.style.color = "black";
    fileChosen.style.fontSize = "small"
    var reader = new FileReader();
    reader.onloadend = function () {
        var base64String = reader.result;
        $('#filestring_' + id).val(base64String)
    };
    reader.readAsDataURL(file);

})

$('.form').on('change', function () {
    var id = $(this).data('id');
    $('#submittd_' + id).removeClass('d-none');
})

$('.Cancel').on('click', function () {
    var id = $(this).data('id');
    $('#submittd_' + id).addClass('d-none');
    if ($('.form').hasClass('exist')) {
        $('.form_' + id).addClass('d-none');
        $('.span_' + id).removeClass('d-none');
    } else {

    }
})

$('.submit').on('click', function () {
    var id = $(this).data('id');
    var date = $(this).data('name');
    var item = $('.item_' + id).val();
    var amount = $('.amount_' + id).val();
    var file = $('#filestring_' + id).val();
    if (item == '' || amount == 0 || file == '') {
        Swal.fire("fill all");
    }
    else {
        console.log(id, item, amount, file);
        var filesname = document.getElementById('file-chosen_' + id).textContent;
        $.ajax({
            url: '/AdminArea/AdminProviderTab/EditReimbursement',
            data: { id, item, amount, file, filesname, date },
            type: 'POST',
            success: function (result) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Your work has been saved",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#receipts').html(result);
            }
        })
    }
})

$('.viewbill').on('click', function () {
    var id = $(this).data('id');
    var link = document.createElement('a');
    link.href = '/AdminArea/AdminProviderTab/DownloadReimbursementBill?id=' + id;
    link.click();
    Swal.fire({
        position: "top-end",
        icon: "success",
        title: "File Downloaded",
        showConfirmButton: false,
        timer: 1500
    });
})


$('.editbill').on('click', function () {
    var id = $(this).data('id');
    $('.span_' + id).addClass('d-none');
    $('.form_' + id).removeClass('d-none');
    $('#submittd_' + id).removeClass('d-none');
})

$('.deletebill').on('click', function () {
    var id = $(this).data('id');
    $.ajax({
        url: '/AdminArea/AdminProviderTab/DeleteReimbursement',
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
            $('#receipts').html(result);
        }
    })
})



$('#bonus').on('blur', function () {
    var id = $(this).data('id');
    var bonus = $(this).val();
    $.ajax({
        url: '/AdminArea/AdminProviderTab/EditBonus',
        data: { id , bonus },
        type: 'POST',
        success: function (result) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Bonus Saved",
                showConfirmButton: false,
                timer: 1500
            });
            $('#receipts').html(result);
        }
    })
})

$('#description').on('blur', function () {
    var id = $(this).data('id');
    var description = $(this).val();
    $.ajax({
        url: '/AdminArea/AdminProviderTab/Editdescription',
        data: { id, description },
        type: 'POST',
        success: function (result) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "description Saved",
                showConfirmButton: false,
                timer: 1500
            });
            $('#receipts').html(result);
        }
    })
})


$('#approvethis').on('click', function () {
    var id = $(this).data('id');
    $.ajax({
        url: '/AdminArea/AdminProviderTab/ApproveThisBiweek',
        data: { id },
        type: 'POST',
        success: function (result) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "description Saved",
                showConfirmButton: false,
                timer: 1500
            });
            location.reload();
        }
    })
})