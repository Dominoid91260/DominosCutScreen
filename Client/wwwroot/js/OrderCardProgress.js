function SetAllOrderCardProgress()
{
    let liveOrderCards = document.querySelectorAll("#liveOrders .card .progress-bar[data-time]");

    if (liveOrderCards === null)
        return;

    liveOrderCards.forEach(e => {
        let datatime = Date.parse(e.dataset.time);
        let timeleft = (datatime - Date.now()) / 1000; // ms to s
        e.style.animationDelay = `${timeleft}s`;
    });
}
