const actualBtn = document.getElementById('actual-btn');
console.log(actualBtn);
const fileChosen = document.getElementById('file-chosen');

actualBtn.addEventListener('change', function() {
    fileChosen.textContent = this.files[0].name;
    fileChosen.style.color = "black";
    fileChosen.style.fontSize = "large"
})