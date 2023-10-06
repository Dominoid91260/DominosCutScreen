function randomNumber(min, max)
{
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

let audio;
function playOvenAlert()
{
    // If audio exists and is playing, do nothing
    if (audio != null && !audio.paused)
        return;
    
    audio = new Audio("/audio/alert-" + randomNumber(1, 4) + ".mp3");
    audio.play();
}