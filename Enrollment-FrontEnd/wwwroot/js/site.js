// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const body = document.querySelector("body"),
    dashboard = body.querySelector(".dashboard"),
    toggle = body.querySelector(".toggle");

toggle.addEventListener("click", () => {
    dashboard.classList.toggle("close");
});