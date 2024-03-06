﻿try {
    var actualBtn = document.getElementById('actual-btn');
    console.log(actualBtn);
    var fileChosen = document.getElementById('file-chosen');

    actualBtn.addEventListener('change', function () {
        var filesnames = this.files[0].name;
        for (var i = 1; i < this.files.length; i++) {
            filesnames = filesnames + ' + ' + this.files[i].name;
            console.log(this.files[i].name)
        }
        fileChosen.textContent = filesnames;
        fileChosen.style.color = "black";
        fileChosen.style.fontSize = "large"
    })
    $('.uploadbtn').on('click', function (e) {
        e.preventDefault();

        var formData = new FormData();
        for (var i = 0; i < actualBtn.files.length; i++) {
            formData.append('files', actualBtn.files[i]); // Append each selected file
        }
        formData.append('RequestsId', $('.RequestsId').val());
        console.log(formData);
        // Add any other data you need (e.g., RequestsId)

        $.ajax({
            url: 'AdminArea/Dashboard/UploadFiles', // Replace with your controller action URL
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
    });
}
catch { }