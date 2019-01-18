$(document).ready(function () {
    var RateValue = document.getElementById("Rate").value;
    SetStar(RateValue);
})

function SetStar(n) {
    var i;

    document.getElementById("Rate").value = n;
    var stars = document.getElementsByClassName("star-rating");

    for (i = 0; i < stars.length; i++) {
        if (i < n)
            stars[i].className = stars[i].className.replace("glyphicon-star-empty", "glyphicon-star ")
        else
            stars[i].className = stars[i].className.replace("glyphicon-star ", "glyphicon-star-empty")
    }
}