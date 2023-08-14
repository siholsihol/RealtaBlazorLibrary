// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}

export function selectText(tbId) {
    var tb = document.querySelector("#" + tbId);

    if (tb.select) {
        tb.select();
    }
}

export function setValueById(id, value) {
    document.getElementById(id).value = value;
}

export function scrollToSelectedRow(gridSelector) {
    var gridWrapper = document.querySelector(gridSelector);
    if (gridWrapper) {
        var selectedRow = gridWrapper.querySelector("tr.k-selected");
        if (selectedRow) {
            selectedRow.scrollIntoView();
        }
    }
}

export async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(URL);
}

let result;
let dotNetInstance;

let observer = new MutationObserver(function () {
    return dotNetInstance.invokeMethodAsync('AutoFitAllColumns');
});

let options = {
    childList: true,
    subtree: true,
};

export function observeTarget(dotNetObj, gridClass) {
    result = document.querySelector(`.${gridClass} .k-grid-table:first-of-type`);
    dotNetInstance = dotNetObj;

    if (!result || !window.DotNet) {
        window.setTimeout(observeTarget, 500);
        return;
    }
    observer.observe(result, options);

    if (window.DotNet) {
        dotNetInstance.invokeMethodAsync('AutoFitAllColumns');
        observer.disconnect();
    }
}

export function hasWhiteSpace() {
    const grid = document.querySelector(".k-grid");
    const gridTable = document.querySelector(".k-grid-table");

    return grid.offsetWidth > gridTable.offsetWidth;
}

export function showBootstrapToast() {
    var toastLiveExample = document.getElementById('liveToast')
    if (toastLiveExample != null) {
        var toast = new bootstrap.Toast(toastLiveExample)
        toast.show()
    }
}

export function addClassToElement(elementId, addClass) {
    var element = document.getElementById(elementId);
    if (element != null) {
        element.classList.add(addClass);
    }
}

export function removeClassToElement(elementId, removeClass) {
    var element = document.getElementById(elementId);
    if (element != null) {
        element.classList.remove(removeClass);
    }
}

export function addClassToQuerySelector(querySelector, addClass) {
    var container = document.querySelector(querySelector);
    if (container != null) {
        container.classList.add(addClass);
    }
}

export function removeClassToQuerySelector(querySelector, removeClass) {
    var container = document.querySelector(querySelector);
    if (container != null) {
        container.classList.remove(removeClass);
    }
}