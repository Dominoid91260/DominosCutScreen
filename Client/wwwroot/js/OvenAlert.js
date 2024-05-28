let audio;

function playOvenAlert(bShort)
{
    // If audio exists and is playing, do nothing
    if (audio != null && !audio.paused)
        return;

    audio = new Audio("/audio/alarm" + (bShort ? "_short" : "") + ".wav");
    audio.play();
}
