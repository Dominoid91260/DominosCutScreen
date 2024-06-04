let audio;

function playOvenAlert(file)
{
    // If audio exists and is playing, do nothing
    if (audio != null && !audio.paused)
        return;

    audio = new Audio("/audio/" + file);
    audio.play();
}
