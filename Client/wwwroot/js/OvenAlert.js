let audio;
function playOvenAlert()
{
    // If audio exists and is playing, do nothing
    if (audio != null && !audio.paused)
        return;
    
    audio = new Audio("/audio/alarm.wav");
    audio.play();
}