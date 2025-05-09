// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function play(element,stationUrl) {
    var player = document.getElementById("player");
 
    document.querySelectorAll('.station').forEach(function (row) {
   //     row.style.background = "#ffffff";
                row.classList.remove("table-info");
    });


    var selectedElement = element.closest('.station');   
    if (!player.paused && player.src == stationUrl) {
 //     selectedElement.style.background = "#ffffff";
        selectedElement/*.parentNode*/.classList.remove("table-info");
        player.pause();
    } else {
        player.src = stationUrl;
   //     selectedElement.style.backgroundColor = "#e2d9fa";
        selectedElement.classList.add("table-info");
        player.play();
    }
  
}