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

//export function setValueById(id, value) {
//    document.getElementById(id).value = value;
//}

//export function scrollToSelectedRow(gridSelector) {
//    var gridWrapper = document.querySelector(gridSelector);
//    if (gridWrapper) {
//        var selectedRow = gridWrapper.querySelector("tr.k-selected");
//        if (selectedRow) {
//            selectedRow.scrollIntoView();
//        }
//    }
//}

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

export function focusToElement(id) {
    const element = document.getElementById(id);
    if (element != null) {
        element.focus();
    }
}

export function clickComponent(componentId) {
    document.getElementById(componentId).click();
}

export function tabToButton(args, id) {
    if (args.key == "Tab" && !args.shiftKey) {
        focusToElement(id);
        args.preventDefault();
        args.stopImmediatePropagation();
    }
}

// Helper function to change disabled state of single element
export function changeDisabledState(elm, disabled) {
    if (!disabled) {
        elm.removeAttribute('disabled');
    }
    else {
        elm.setAttribute('disabled', disabled);
    }
}

export function disableAllControl(containerClass, disabled) {
    // Get DIV container to be disabled
    const container = document.querySelector(containerClass);
    // Check if helper class is there
    //const isDisabled = container.classList.contains('disabled');
    const isDisabled = disabled;

    // Query all fields inside DIV.
    const allFields = container.querySelectorAll('input, textarea, button, select');

    // Iterate over all elements and set the opposite state
    [...allFields].forEach(elm => {
        changeDisabledState(elm, !isDisabled);
    });

    // Toggle helper class
    container.classList.toggle('disabled');
}