let selectedFiles = [];

export function initializeFileDropZone(dropZoneElement) {

    // Handle the paste and drop events
    function onDrop(e) {
        e.preventDefault();
        selectedFiles = evt.dataTransfer.files;
    }

    // Register all events
    dropZoneElement.addEventListener("drop", onDrop);

    // The returned object allows to unregister the events when the Blazor component is destroyed
    return {
        dispose: () => {
            dropZoneElement.removeEventListener('drop', onDrop);
        }
    }
}

export function GetFiles() {
    var paths = [];

    for (var i = 0; i < selectedFiles.length; i++) {
        paths.push(selectedFiles[i].path)
    }
    return paths;
}