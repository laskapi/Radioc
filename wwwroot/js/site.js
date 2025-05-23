"use strict";

var intervalId = null;
var globalUrl = "";
var connection = new signalR.HubConnectionBuilder().withUrl("/Signal").build();
connection.start();

connection.on("MetaResponse", function (meta) {
    document.getElementById("playText").innerText = `${meta}`;
});


function togglePlay(url) {

    if (url) {
        globalUrl = url;
    }

    var player = document.getElementById("player");
    var icon = document.getElementById("playIcon");

    if (!player.paused && (player.src === url || !url)) {
        player.pause();
        clearInterval(intervalId);
        icon.classList.remove("bi-stop-circle");
        icon.classList.add("bi-play-circle");

    }


    else if (player.src !== globalUrl || player.paused){

        player.src = globalUrl;
        clearInterval(intervalId);
        updateMeta(globalUrl);
        if (globalUrl) {
            intervalId = setInterval(() => { updateMeta(globalUrl); }, 5000);
            player.play();
            player.onloadedmetadata=(event) => { console.log(event); };
            icon.classList.remove("bi-play-circle");
            icon.classList.add("bi-stop-circle");
        }

    }
   
}

function updateMeta(url) {

    connection.invoke("MetaRequest", url).catch(function (err) {
        return console.error(err.toString());
    });


}