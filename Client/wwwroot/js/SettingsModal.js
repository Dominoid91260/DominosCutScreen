function setupSettingsModalEvents() {
    let modalDialog = document.querySelector("#settingsModal .modal-dialog");
    let tabs = document.querySelectorAll('#settingsModal a[data-bs-toggle="tab"]');
    tabs.forEach(function (e) {
        e.addEventListener('shown.bs.tab', function (event) {
            event.target // newly activated tab
            event.relatedTarget // previous active tab

            if (event.target.getAttribute("href") == "#settingsPostbakes") {
                modalDialog.classList.add("modal-lg");
            }
            else {
                modalDialog.classList.remove("modal-lg");
            }
        })
    });
};
