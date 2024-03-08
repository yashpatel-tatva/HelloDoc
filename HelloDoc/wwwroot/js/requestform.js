try {
    var actualBtn = document.getElementById('actual-btn');
    var fileChosen = document.getElementById('file-chosen');

    actualBtn.addEventListener('change', function () {
        for (var i = 0; i < this.files.length ; i++){
            fileChosen.textContent = this.files[i].name;
        }
        fileChosen.style.color = "black";
        fileChosen.style.fontSize = "large"
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