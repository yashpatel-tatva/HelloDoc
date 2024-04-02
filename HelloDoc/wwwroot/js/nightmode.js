var btn = document.getElementById("btn1");
var imgmode = document.getElementById('imgmode');
imgmode.classList.remove("fa-spin");
var bg = document.getElementsByClassName('bggray');
btn.addEventListener('click', changetheme);
var flag = 1;
    if (sessionStorage.getItem('flag')) {
        flag = parseInt(sessionStorage.getItem('flag'));
    }
    changetheme();


function changetheme() {
    if (flag === 0) {
        document.querySelector('body').setAttribute('data-bs-theme', 'dark');
        imgmode.classList.replace("fa-sun", "fa-moon")
        imgmode.style.color = "rgb(0, 184, 230)"
        btn.style.borderColor = "#0dcaf0";
        if (document.querySelector('body').style.backgroundColor === "rgb(240, 240, 240)") {
            document.querySelector('body').style.backgroundColor = "black";
        }
        try {
            document.getElementById("bg").style.backgroundColor = "transparent";
        } catch {}
        try {
            document.getElementById('content').classList.remove('bg-light');
            document.getElementById('content').classList.add('bg-dark');
            document.getElementById('content1').classList.remove('bg-light');
            document.getElementById('content1').classList.add('bg-dark');
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
        imgmode.classList.replace("fa-moon", "fa-sun")
        imgmode.style.color = "black"
        btn.style.borderColor = "black";
        sessionStorage.setItem('flag', flag);
        flag = 0;
        if (document.querySelector('body').style.backgroundColor === "black") {
            document.querySelector('body').style.backgroundColor = "rgb(240, 240, 240)";
        }
        try {
            document.getElementById("bg").style.backgroundColor = "transparent";
        } catch {}
        try {
            document.getElementById('content').classList.remove('bg-dark');
            document.getElementById('content').classList.add('bg-light');
            document.getElementById('content1').classList.remove('bg-dark');
            document.getElementById('content1').classList.add('bg-light');
        } catch {}
        try {
            document.querySelector('nav').classList.replace('bg-dark', 'bg-light');
        } catch {}
        try {
            document.getElementById('iti-0__country-listbox').style.backgroundColor = 'white';
        } catch {}
    }
}