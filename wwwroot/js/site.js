"use strict";

var intervalId = null;
var globalUrl = "";
var connection = new signalR.HubConnectionBuilder().withUrl("/Signal").build();
connection.start();

connection.on("MetaResponse", function (meta) {
    document.getElementById("playButton").innerText = `${meta}`;
});


function playToggle(url) {

    if (url) {
        globalUrl = url;
    }

    var player = document.getElementById("player");

    if (!player.paused && (player.src === url || !url)) {
        player.pause();
        clearInterval(intervalId);
    }


    else if (player.src !== globalUrl || player.paused){

        player.src = globalUrl;
        clearInterval(intervalId);
        updateMeta(globalUrl);
        intervalId = setInterval(() => { updateMeta(globalUrl); }, 5000);
        player.play();

    }
   
}

function updateMeta(url) {

    connection.invoke("MetaRequest", url).catch(function (err) {
        return console.error(err.toString());
    });


}