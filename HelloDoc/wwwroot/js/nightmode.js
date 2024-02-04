const btn = document.getElementById("btn1");
const imgmode = document.getElementById('imgmode');
const bg = document.getElementsByClassName('bggray');
btn.addEventListener('click', changetheme);
var flag = 1;

window.onload = function() {
    console.log(sessionStorage.getItem('flag'));
    if (sessionStorage.getItem('flag')) {
        flag = parseInt(sessionStorage.getItem('flag'));
        console.log(flag);
    }
    changetheme();
    try {
        const myModal = new bootstrap.Modal(document.getElementById('exampleModal'));
        myModal.show();
        const closeBtn = document.getElementById("closeBtn");
        closeBtn.addEventListener('click', function() {
            myModal.hide();
        });
    } catch {}
}

function changetheme() {
    if (flag === 0) {
        document.querySelector('body').setAttribute('data-bs-theme', 'dark');
        imgmode.style.color = "#00b8e6";
        btn.style.borderColor = "#00b8e6";
        if (document.querySelector('body').style.backgroundColor === "rgb(235, 235, 235)") {
            document.querySelector('body').style.backgroundColor = "black";
        }
        try {
            document.getElementById("bg").style.backgroundColor = "transparent";
        } catch {}
        try {
            document.getElementById('content').classList.remove('bg-light');
            document.getElementById('content').classList.add('bg-dark');
        } catch {}
        try {
            document.querySelector('nav').classList.replace('bg-light', 'bg-dark');
        } catch {}
        try {
            document.getElementById('iti-0__country-listbox').style.backgroundColor = 'black';
        } catch {}
        sessionStorage.setItem('flag', flag);
        flag = 1;
    } else {
        document.querySelector('body').setAttribute('data-bs-theme', 'light');
        imgmode.style.color = "black";
        btn.style.borderColor = "black";
        sessionStorage.setItem('flag', flag);
        flag = 0;
        if (document.querySelector('body').style.backgroundColor === "black") {
            document.querySelector('body').style.backgroundColor = "rgb(235, 235, 235)";
        }
        try {
            document.getElementById("bg").style.backgroundColor = "transparent";
        } catch {}
        try {
            document.getElementById('content').classList.remove('bg-dark');
            document.getElementById('content').classList.add('bg-light');
        } catch {}
        try {
            document.querySelector('nav').classList.replace('bg-dark', 'bg-light');
        } catch {}
        try {
            document.getElementById('iti-0__country-listbox').style.backgroundColor = 'white';
        } catch {}
    }
}