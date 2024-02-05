try {
    const actualBtn = document.getElementById('actual-btn');
    console.log(actualBtn);
    const fileChosen = document.getElementById('file-chosen');

    actualBtn.addEventListener('change', function () {
        fileChosen.textContent = this.files[0].name;
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