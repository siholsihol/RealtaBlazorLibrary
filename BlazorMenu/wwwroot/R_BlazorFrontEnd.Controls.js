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
export function setElementEnabledState(elm, enabled) {
    if (enabled) {
        elm.removeAttribute('disabled');
    }
    else {
        elm.setAttribute('disabled', 'true');
    }
}

export function setElementEnabledClass(elm, enabled) {
    let element = getEnabledElement(elm);

    if (enabled) {
        element.classList.remove('k-disabled');
    }
    else {
        element.classList.add('k-disabled');
    }
}

export function setElementTargetable(elm, enabled) {
    if (enabled) {
        // Restore old tabindex if it was saved
        if (elm.hasAttribute('data-old-tabindex')) {
            elm.setAttribute('tabindex', elm.getAttribute('data-old-tabindex'));
            elm.removeAttribute('data-old-tabindex');
        } else {
            elm.removeAttribute('tabindex');
        }
    } else {
        // Save current tabindex if it's not already saved
        if (elm.hasAttribute('tabindex') && elm.getAttribute('tabindex') != -1) {
            elm.setAttribute('data-old-tabindex', elm.getAttribute('tabindex'));
        }

        elm.setAttribute('tabindex', '-1');
    }
}

export function getEnabledElement(elm) {
    const tag = elm.tagName.toLowerCase();

    let returnElement = elm;

    if (tag === 'input' && elm.type !== 'radio' && elm.type !== 'checkbox') {
        returnElement = elm.closest('.k-input');
    } else if (tag === 'textarea' && elm.id && elm.id.startsWith('ta')) {
        returnElement = elm.closest('.k-input');
    } else {
        returnElement = elm;
    }

    return returnElement;
}

export function changeAllControlStatus(elementId, status) {
    // Get DIV container to be disabled
    //const container = document.querySelector(containerClass);
    const container = document.getElementById(elementId);
    // Check if helper class is there
    //const isDisabled = container.classList.contains('disabled');
    setElementEnabledState(container, status)

    // Query all fields inside DIV.
    const allFields = container.querySelectorAll('input, textarea[id^="ta"], button, select, span[id], div.k-editor');

    // Iterate over all elements
    // If Parent Group Box is enabled and Current Group Box is enabled => enabled field
    // Else, disabled field
    [...allFields].forEach(elm => {
        // check all parents, not just immediate parents
        let parent = elm.closest('div[id*="grpbox"]');
        let isFieldDisabled = elm.getAttribute('aria-disabled') === 'true' ? true : false;

        if ([...elm.classList].some(cls => cls.startsWith('k-spinner'))) {
            const spinInput = elm.closest('span.k-numerictextbox')?.querySelector('input[role="spinbutton"]');

            if (spinInput) {
                isFieldDisabled = spinInput.getAttribute('aria-disabled') === 'true';
            }
        }

        let isDisabledByAncestor = false;

        while (parent) {
            if (parent.hasAttribute('disabled')) {
                isDisabledByAncestor = true;
                break;
            }
            parent = parent.parentElement?.closest('div[id*="grpbox"]');
        }

        const enabled = status && !isDisabledByAncestor && !isFieldDisabled;

        // Final status: only enabled if status == true and no parent disables it
        setElementTargetable(elm, enabled);
        setElementEnabledState(elm, enabled);
        setElementEnabledClass(elm, enabled);
    });

    // Toggle helper class
    //container.classList.toggle('disabled');
}

export function setAriaDisabled(elementId, enabled) {
    let element = document.getElementById(elementId);
    element.setAttribute('aria-disabled', !enabled);
}

export function addValidationMessage(inputWrapperId, iconId) {
    const container = document.getElementById(inputWrapperId);
    if (!container) return;
    container.className = 'r-input-invalid';

    const target = container.querySelector("span.k-input");

    if (target) {
        const div = document.createElement('div');
        div.id = iconId;
        div.className = 'r-icon-container';
        div.innerHTML = '<div class="r-icon"><i class="fas fa-exclamation-circle text-danger"></i></div>';
        target.appendChild(div, target.lastChild);
    }
}

export function removeValidationMessage(inputWrapperId, iconId) {
    const container = document.getElementById(inputWrapperId);
    if (!container) return;
    container.className = '';

    const target = container.querySelector(`#${iconId}`);

    if (target && target.parentNode) {
        target.parentNode.removeChild(target);
    }
}