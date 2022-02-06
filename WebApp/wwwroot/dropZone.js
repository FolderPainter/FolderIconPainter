let selectedFiles = [];

export function initializeFileDropZone(dropZoneElement) {

    // Handle the paste and drop events
    function onDrop(e) {
        e.preventDefault();
        selectedFiles = e.dataTransfer.files;
    }

    function dragEnter (e) {
        e.preventDefault();
        e.stopPropagation();
    }

    function dragOver (e) {
        e.preventDefault();
        e.stopPropagation();
    }

    // Register all events
    dropZoneElement.addEventListener("drop", onDrop);
    //dropZoneElement.addEventListener("dragenter", dragEnter);
    dropZoneElement.addEventListener("dragover", dragOver);

     //The returned object allows to unregister the events when the Blazor component is destroyed
    return {
        dispose: () => {
            dropZoneElement.removeEventListener("drop", onDrop);
            //dropZoneElement.removeEventListener("dragenter", dragEnter);
            dropZoneElement.removeEventListener("dragover", dragOver);

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