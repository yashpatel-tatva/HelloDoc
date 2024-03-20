try {
    var actualBtn = document.getElementById('actual-btn');
    var fileChosen = document.getElementById('file-chosen');
    actualBtn.addEventListener('change', function () {
        var filesnames = this.files[0].name;
        for (var i = 1; i < this.files.length; i++) {
            filesnames = filesnames + ' + ' + this.files[i].name;
        }
        fileChosen.textContent = filesnames;
        fileChosen.style.color = "black";
        fileChosen.style.fontSize = "large"
    })
    var actualBtn1 = document.getElementById('actual-btn1');
    var fileChosen1 = document.getElementById('file-chosen1');
    actualBtn1.addEventListener('change', function () {
        var filesnames = this.files[0].name;
        for (var i = 1; i < this.files.length; i++) {
            filesnames = filesnames + ' + ' + this.files[i].name;
        }
        fileChosen1.textContent = filesnames;
        fileChosen1.style.color = "black";
        fileChosen1.style.fontSize = "large"
    })
}
catch {
}  window.onload = function () {

        try {
            var myModal = new bootstrap.Modal(document.getElementById('exampleModal'));
            myModal.show();
            var closeBtn = document.getElementById("closeBtn");
            closeBtn.addEventListener('click', function () {
                myModal.hide();
            });
        } catch { }
    }