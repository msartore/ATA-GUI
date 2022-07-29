function openAndCloseNav() {
    var sidenav = document.getElementById("sidenav")
    var navButtonClose = document.getElementById("navButtonClose")
    var visibility = sidenav.style.visibility == "visible" ? 'collapse' : "visible"
    sidenav.style.visibility = visibility
    navButtonClose.style.visibility = visibility
}