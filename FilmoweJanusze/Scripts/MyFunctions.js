﻿$(document).ready(function () {
    var modals = ['#Register', '#'];
    if (window.location.hash && ~modals.indexOf(window.location.hash)) {
        $(window.location.hash).modal();
    }

    var login = '#LogIn';
    if (window.location.hash && ~login.indexOf(window.location.hash)) {
        openLogIn();
    }
})

function UnHide() {
    var x = document.getElementsByName("editcast");
    for (var i = 0; i < x.length; i++) {
        if (x[i].style.display === "none") {
            x[i].style.display = "block";
        }
        else {
            x[i].style.display = "none";
        }
    }
}

function ShowPassword() {
    var x = document.getElementById("RegisterPassword");
    var z = document.getElementById("ConfirmPassword");
    var y = document.getElementById("showpass");
    if (x.type === "password") {
        x.type = "text";
        z.type = "text";
        y.checked = true;
    } else {
        x.type = "password";
        z.type = "password";
        y.checked = false;
    }
}

function openLogIn() {
    var x = document.getElementById("LogIn");
    if (x.style.display === "block") {
        x.style.display = "none";
    }
    else { 
        x.style.display = "block";
    }
}

function ToggleRadio() {
    var RadioButtons = document.getElementsByName("RadioPhotoBtn");
    var checked, i;
    var filebox = document.getElementById("file");
    var inputbox = document.getElementById("url");

    for (i = 0; i < RadioButtons.length; i++) {
        if (RadioButtons[i].checked) {
            checked = RadioButtons[i].value;
        }
    }

    if (checked == "FromFile") {
        filebox.style.display = "block";
        inputbox.style.display = "none";
    }
    else if (checked == "FromURL") {
        filebox.style.display = "none";
        inputbox.style.display = "block";
    }
    else {
        filebox.style.display = "none";
        inputbox.style.display = "none";
    }
}
