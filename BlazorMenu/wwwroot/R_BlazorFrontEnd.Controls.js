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
    try {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);

        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName ?? '';
        document.body.appendChild(anchorElement);
        anchorElement.click();
        anchorElement.remove();

        URL.revokeObjectURL(url);
    } catch (err) {
        console.error("JS error in downloadFileFromStream", err);
        throw err;
    }
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
    if (!elm) return;

    if (enabled) {
        elm.removeAttribute('disabled');
    }
    else {
        elm.setAttribute('disabled', 'true');
    }
}

export function setElementTargetable(elm, enabled) {
    if (!elm) return;

    const currentTabIndex = elm.getAttribute('tabindex');
    const oldTabIndexAttr = elm.getAttribute('data-old-tabindex');
    const oldTabIndex = oldTabIndexAttr !== null ? oldTabIndexAttr : (enabled ? -1 : 0);

    if (enabled) {
        // Already enabled, do nothing
        if (currentTabIndex != -1) return;

        // Enable: swap values
        elm.removeAttribute('tabindex');
        elm.removeAttribute('data-old-tabindex');
        elm.setAttribute('tabindex', oldTabIndex);
        elm.setAttribute('data-old-tabindex', -1);
    } else {
        // Already disabled, do nothing
        if (currentTabIndex == -1) return;

        // Disable: swap values
        elm.removeAttribute('tabindex');
        elm.removeAttribute('data-old-tabindex');
        elm.setAttribute('tabindex', -1);
        elm.setAttribute('data-old-tabindex', currentTabIndex);
    }
}

export function changeAllControlStatus(elementId, status) {
    const container = document.getElementById(elementId);

    if (!container) return;

    setElementEnabledState(container, status)

    // Query all fields inside DIV.
    const allFields = container.querySelectorAll('input, textarea[id^="TextArea_"], button, select, span[id], div.k-editor');

    // Iterate over all elements
    // If Parent Group Box is enabled and Current Group Box is enabled => enabled field
    // Else, disabled field
    [...allFields].forEach(elm => {
        // check all parents, not just immediate parents
        let parent = elm.closest('div[id^="GroupBox_"]');

        let isDisabledByAncestor = false;

        while (parent) {
            if (parent.hasAttribute('disabled')) {
                isDisabledByAncestor = true;
                break;
            }
            parent = parent.parentElement?.closest('div[id^="GroupBox_"]');
        }

        // Final status: only enabled if status == true and no parent disables it
        const enabled = status && !isDisabledByAncestor;

        setElementTargetable(elm, enabled);
    });
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


export function adjustCustomPagerVisibility(gridId) {
    const grid = document.getElementById(gridId);
    if (!grid) return;

    const parent = grid.querySelector("#PagerContainer");
    const pager = grid.querySelector("#CustomPager");
    const info = grid.querySelector("#CustomPagerInfo");
    const itemsPerPage = grid.querySelector("#CustomPagerItemsPerPage");
    const buttons = grid.querySelector("#CustomPagerButtons");
    const buttonsText = grid.querySelector("#CustomPagerButtonsText");

    if (!pager || !parent) return;

    // Reset all to visible first
    if (info) info.style.display = "flex";
    if (itemsPerPage) itemsPerPage.style.display = "flex";
    if (buttonsText) buttonsText.style.display = "flex";
    if (buttons) buttons.style.display = "flex";

    // Measure again after reset
    const fits = () => pager.getBoundingClientRect().width <= parent.getBoundingClientRect().width;

    if (!fits() && info) {
        info.style.display = "none";
    }

    if (!fits() && itemsPerPage) {
        itemsPerPage.style.display = "none";
    }

    if (!fits() && buttonsText) {
        buttonsText.style.display = "none";
    }

    if (!fits() && buttons) {
        buttons.style.display = "none";
    }
}

function debounce(fn, delay) {
    let timeout;
    return () => {
        clearTimeout(timeout);
        timeout = setTimeout(fn, delay);
    };
}

const resizeObservers = {};

export function initializeCustomPagerResize(gridId) {
    const handler = debounce(() => {
        const grid = document.getElementById(gridId);

        if (!grid) {
            disposeCustomPagerResize(gridId);
            return;
        }

        adjustCustomPagerVisibility(gridId);
    }, 100);
    const gridElement = document.getElementById(gridId);
    if (gridElement) {
        // Create and store ResizeObserver
        const observer = new ResizeObserver(handler);
        observer.observe(gridElement);
        resizeObservers[gridId] = observer;
        requestAnimationFrame(() => handler());
    }
}

export function disposeCustomPagerResize(gridId) {
    const observer = resizeObservers[gridId];
    if (observer) {
        observer.disconnect();
        delete resizeObservers[gridId];
    }
}