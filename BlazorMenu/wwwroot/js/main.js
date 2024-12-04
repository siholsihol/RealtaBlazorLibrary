blazorMenuBootstrap = {
    promptUser: function () {
        var name = prompt("eaa");
    },

    toasts: {
        show: (elementId, autohide, delay, dotNetHelper) => {
            let toastEl = document.getElementById(elementId);

            toastEl.addEventListener('show.bs.toast', function () {
                dotNetHelper.invokeMethodAsync('bsShowToast');
            });
            toastEl.addEventListener('shown.bs.toast', function () {
                dotNetHelper.invokeMethodAsync('bsShownToast');
            });
            toastEl.addEventListener('hide.bs.toast', function () {
                dotNetHelper.invokeMethodAsync('bsHideToast');
            });
            toastEl.addEventListener('hidden.bs.toast', function () {
                dotNetHelper.invokeMethodAsync('bsHiddenToast');
            });

            let options = { animation: true, autohide: autohide, delay: delay };
            bootstrap?.Toast?.getOrCreateInstance(toastEl, options)?.show();
        },
        hide: (elementId) => {
            bootstrap?.Toast?.getOrCreateInstance(document.getElementById(elementId))?.hide();
        },
        dispose: (elementId) => {
            bootstrap?.Toast?.getOrCreateInstance(document.getElementById(elementId))?.dispose();
        }
    },

    modal: {
        initialize: (elementId, useStaticBackdrop, closeOnEscape, dotNetHelper) => {
            let modalEl = document.getElementById(elementId);

            modalEl.addEventListener('show.bs.modal', function () {
                dotNetHelper.invokeMethodAsync('bsShowModal');
            });
            modalEl.addEventListener('shown.bs.modal', function () {
                dotNetHelper.invokeMethodAsync('bsShownModal');
            });
            modalEl.addEventListener('hide.bs.modal', function () {
                dotNetHelper.invokeMethodAsync('bsHideModal');
            });
            modalEl.addEventListener('hidden.bs.modal', function () {
                dotNetHelper.invokeMethodAsync('bsHiddenModal');
            });
            modalEl.addEventListener('hidePrevented.bs.modal', function () {
                dotNetHelper.invokeMethodAsync('bsHidePreventedModal');
            });

            let options = { backdrop: useStaticBackdrop ? 'static' : true, keyboard: closeOnEscape };
            bootstrap?.Modal?.getOrCreateInstance(modalEl, options);
        },
        show: (elementId) => {
            bootstrap?.Modal?.getOrCreateInstance(document.getElementById(elementId))?.show();
        },
        hide: (elementId) => {
            bootstrap?.Modal?.getOrCreateInstance(document.getElementById(elementId))?.hide();
        },
        dispose: (elementId) => {
            bootstrap?.Modal?.getOrCreateInstance(document.getElementById(elementId))?.dispose();
        }
    },

    // Disable the back button
    disableBackButton: function () {
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };

        // Call the disable back button function
        disableBackButton();
    },

    observeElement: function (elementId, dotnetObjectReference) {
        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutationRecord) {
                var element = mutationRecord.target
                var classList = element.classList;

                dotnetObjectReference.invokeMethodAsync("ObserverNotification", classList.contains('show'))
            });
        });

        var target = document.getElementById(elementId);
        observer.observe(target, { attributes: true, attributeFilter: ['class'] });
    },

    changeThemeToggle: function (id) {
        const element = document.getElementById(id);
        if (element != null) {
            if (localStorage.getItem('theme') === 'dark') {
                element.setAttribute('checked', true);
            }
        }
    }
}