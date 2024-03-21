$('#NoteSaveSubmitButton').on('click', function () {

    var requestid = $('#hiddenrequestid').val();
    var Notes = $('#AdminNotes').val();

    var model = {
        requestid: requestid,
        AdminNotes: Notes
    };


    $.ajax({
        url: 'AdminArea/Dashboard/SaveAdminNotes',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (response) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Notes Saved",
                showConfirmButton: false,
                timer: 1000
            });
            $('#nav-tabContent').html(response);
        },
        error: function (error) {
            console.error('Error saving admin notes:', error);
        }
    });

});
