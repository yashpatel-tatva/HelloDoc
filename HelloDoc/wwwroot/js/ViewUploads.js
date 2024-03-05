$('.deletebtn').on('click', function () {
    var id = $(this).attr('id');
    console.log(id);
    $.ajax({
        type: 'POST',
        url: '../Dashboard/Delete',
        data: { id: id },
        success: function (result) {
            $('#nav-tabContent').html(result);
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error);
        },
    });
});
$('.deleteall').on('click', function () {
    $('.childCheckbox:checked').each(function () {
        var id = $(this).val();
        console.log(id);
        $.ajax({
            type: 'POST',
            url: '../Dashboard/Delete',
            data: { id: id },
            success: function (result) {
                $('#nav-tabContent').html(result);
            },
            error: function (xhr, status, error) {
                console.error('Error: ' + error);
            },
        });
    });
});
$('.downloadall').on('click', function () {
    console.log($('.childCheckbox:checked').length);
    if ($('.childCheckbox:checked').length === 0) {
        alert("Please select at least one item.");
    }
    else {
        $(this).closest('form').attr('action', '/AdminArea/Dashboard/Download');
        $(this).closest('form').submit();
    }
});
$('.sendemail').on('click', function () {
    console.log($('.childCheckbox:checked').length);
    if ($('.childCheckbox:checked').length === 0) {
        alert("Please select at least one item.");
    }
    else {

        var formData = new FormData();
        $('input[name="downloadselect"]:checked').each(function () {
            formData.append('RequestWiseFileId', $(this).val());
        });

        formData.append('RequestsId', $('.RequestsId').val());
        console.log(formData);

        $.ajax({
            url: '../Dashboard/SendMail', 
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                $('#nav-tabContent').html(response);
            },
            error: function (error) {
                console.error('Error uploading files:', error);
            }
        });
    }
});

$('.deleteall').on('click', function () {
    console.log($('.childCheckbox:checked').length);
    if ($('.childCheckbox:checked').length === 0) {
        alert("Please select at least one item.");
    }
});

$(document).ready(function () {
    $('#dtBasicExample1').DataTable({
        info: false,
        ordering: true,
        paging: false,
        searching: false,

    });
});
