try {
    const actualBtn = document.getElementById('actual-btn');
    console.log(actualBtn);
    const fileChosen = document.getElementById('file-chosen');

    actualBtn.addEventListener('change', function () {
        for (int i = 0; i < files.length ; i++){
            fileChosen.textContent = this.files[i].name;
        }
        fileChosen.style.color = "black";
        fileChosen.style.fontSize = "large"
    })
  
}
catch {
    console.log("erro")
}  window.onload = function () {

        try {
            const myModal = new bootstrap.Modal(document.getElementById('exampleModal'));
            myModal.show();
            const closeBtn = document.getElementById("closeBtn");
            closeBtn.addEventListener('click', function () {
                myModal.hide();
            });
        } catch { }
    }