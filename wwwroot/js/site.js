// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const toggle = document.getElementById("themeToggle")

toggle.addEventListener("click", () => {

    document.body.classList.toggle("dark-mode")

    if (document.body.classList.contains("dark-mode")) {
        toggle.innerHTML = "☀️"
    } else {
        toggle.innerHTML = "🌙"
    }

})