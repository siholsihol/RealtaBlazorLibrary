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

    overrideDefaultKey: (dotNetHelper) => {
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

        var util = {};

        document.addEventListener('keydown', function (e) {

            var key = util.key[e.which];
            if (key) {
                e.preventDefault();
            }

            if (key === 'F1') {
                // do stuff
                dotNetHelper.invokeMethodAsync('DefaultKeyDown', serializeEvent(e));
            }
            else if (e.key === 'f' && e.ctrlKey) {
                e.preventDefault();
                // set focus to search bar
                dotNetHelper.invokeMethodAsync('FindKeyDown', serializeEvent(e));
            }
        })

        util.key = {
            112: "F1"
        }
    },

    blazorOpen: (args) => {
        window.open(args);
    },

    attachFocusHandler: (dotNetHelper, elementId) => {
        var element = document.getElementById(elementId);
        if (element) {
            element.addEventListener("focus", (event) => {
                dotNetHelper.invokeMethodAsync("OpenComponent");
            });
        }
    },

    invisiblePopup: function (elementId) {
        let element = document.getElementById(elementId);
        if (element != null) {
            element.classList.add('d-none');
        }
    },

    visiblePopup: function (elementId) {
        let element = document.getElementById(elementId);
        if (element != null) {
            element.classList.remove('d-none');
        }
    },

    /* Enable Horizontal Scroll in XTabs */
    enableHorizontalScroll: function (elementClass) {
        const element = document.getElementsByClassName(elementClass)[0];
        if (!element) return;

        element.addEventListener('wheel', function (e) {
            // Only if there's a horizontal scrollbar
            if (element.scrollWidth > element.clientWidth) {
                e.preventDefault();
                element.scrollLeft += e.deltaY;
            }
        }, { passive: false });
    },

    scrollToActiveTab: function () {
        const xtabsHeader = document.querySelector('.xtabs-header');
        if (!xtabsHeader) return;

        // Find the active tab (adjust selector if needed)
        const activeTab = xtabsHeader.querySelector('.active') 
        if (!activeTab) return;

        // Get bounding rectangles
        const containerRect = xtabsHeader.getBoundingClientRect();
        const activeRect = activeTab.getBoundingClientRect();

        // Calculate how much to scroll to bring active tab fully into view
        if (activeRect.left < containerRect.left) {
            // Scroll left if active tab is hidden on left side
            xtabsHeader.scrollBy({ left: activeRect.left - containerRect.left, behavior: 'smooth' });
        } else if (activeRect.right > containerRect.right) {
            // Scroll right if active tab is hidden on right side
            xtabsHeader.scrollBy({ left: activeRect.right - containerRect.right, behavior: 'smooth' });
        }
    },

    navbar: {
        toggleFooter: function (footerId) {
            const footer = document.getElementById(footerId);
            const html = document.querySelector("html");

            // Attach listener to the custom event fired by theme.js
            const toggleButton = document.querySelector(".navbar-vertical-toggle");
            if (toggleButton && footer) {
                toggleButton.addEventListener("navbar.vertical.toggle", function () {
                    const isCollapsed = html.classList.contains("navbar-vertical-collapsed");
                    footer.style.display = isCollapsed ? "none" : "block";
                });

                // Set initial state on page load
                footer.style.display = html.classList.contains("navbar-vertical-collapsed")
                    ? "none"
                    : "block";
            }
        },
        toggleMenuOverlay: function (plVisible) {
            const html = document.querySelector("html");
            const visibleClassName = "r-menu-overlay-show";
            const isVisible = html.classList.contains(visibleClassName);

            if (plVisible && !isVisible) {
                html.classList.add(visibleClassName);
            }

            if (!plVisible && isVisible) {
                html.classList.remove(visibleClassName);
            }
        }
    },

    svg: {
        injectSvgToBody: function (svgContent) {
            const wrapper = document.createElement("div");
            wrapper.style.display = "none";
            wrapper.innerHTML = svgContent;
            document.body.insertBefore(wrapper, document.body.firstChild);
        }
    }
}