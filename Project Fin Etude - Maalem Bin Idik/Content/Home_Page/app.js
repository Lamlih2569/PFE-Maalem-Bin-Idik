var btnFAQ = document.querySelectorAll("#FAQ .btn-FAQ");
var icon = document.querySelectorAll("#FAQ i");

btnFAQ[0].addEventListener("click",function(){
    icon[0].classList.toggle("hide");
    icon[1].classList.toggle("hide");
});
btnFAQ[1].addEventListener("click",function(){
    icon[2].classList.toggle("hide");
    icon[3].classList.toggle("hide");
});
btnFAQ[2].addEventListener("click",function(){
    icon[4].classList.toggle("hide");
    icon[5].classList.toggle("hide");
});