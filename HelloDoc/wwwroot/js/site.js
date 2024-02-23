try {
    var actualBtn = document.getElementById('actual-btn');
    console.log(actualBtn);
    var fileChosen = document.getElementById('file-chosen');

    actualBtn.addEventListener('change', function () {
        var filesnames = "";
        for (var i = 0; i < this.files.length; i++) {
            filesnames = filesnames + ' + ' + this.files[i].name;
            console.log(this.files[i].name)
        }
        fileChosen.textContent = filesnames;
        fileChosen.style.color = "black";
        fileChosen.style.fontSize = "large"
    })
}
catch { }