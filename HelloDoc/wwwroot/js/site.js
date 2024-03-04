try {
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
}
catch { }