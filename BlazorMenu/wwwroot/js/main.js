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
    },

    hotKey: {
        addKeyboardListenerEvent: (elementId, dotNetHelper) => {
            let serializeEvent = function (e) {
                if (e) {
                    return {
                        key: e.key,
                        code: e.keyCode.toString(),
                        location: e.location,
                        repeat: e.repeat,
                        ctrlKey: e.ctrlKey,
                        shiftKey: e.shiftKey,
                        altKey: e.altKey,
                        metaKey: e.metaKey,
                        type: e.type
                    };
                }
            };

            window.document.addEventListener('keydown', function (e) {
                dotNetHelper.invokeMethodAsync('JsKeyDown', elementId, serializeEvent(e));
            });
        }
    },

    qrCode: {
        getImageFromCanvas: (selector) => {
            const canvas = document.querySelector(`${selector} canvas`);

            if (canvas) {
                return canvas.toDataURL("image/png");
            }
        },

        getImageFromSvg: (selector) => {
            const dpr = window.devicePixelRatio;

            const svg = document.querySelector(`${selector} svg`);
            if (!svg) {
                return;
            }

            const svgBox = svg.getBBox();
            const svgW = svgBox.width;
            const svgH = svgBox.height;

            const svgData = (new XMLSerializer()).serializeToString(svg);
            const svgBlob = new Blob([svgData], {
                type: "image/svg+xml;charset=utf-8"
            });
            const blobUrl = URL.createObjectURL(svgBlob);

            return getBlobImage(blobUrl, svgW, svgH).then((img) => {
                const canvas = document.createElement("canvas");
                canvas.width = svgW * dpr;
                canvas.height = svgH * dpr;

                const context = canvas.getContext("2d");
                context.imageSmoothingEnabled = false;
                context.drawImage(img, 0, 0, svgW * dpr, svgH * dpr);

                URL.revokeObjectURL(blobUrl);
                img.parentElement.removeChild(img);

                return canvas.toDataURL("image/png");
            });

            function getBlobImage(blobUrl, imageWidth, imageHeight) {
                return new Promise(function (resolve) {
                    const img = new Image();

                    img.addEventListener("load", () => {
                        setTimeout(() => resolve(img));
                    });

                    img.style.cssText = "visibility:hidden;position:absolute;top:0;left:0;";
                    img.width = imageWidth;
                    img.height = imageHeight;
                    document.body.appendChild(img);

                    img.src = blobUrl;
                });
            }
        }
    },

    addStyleToElement: function (elementId, style) {
        const element = document.getElementById(elementId);
        if (element != null) {
            element.style.cssText = style;
        }
    },
}