﻿function rt(t) {
    return typeof t == "object" && t !== null && "x" in t && "y" in t && "unit" in t && typeof t.unit == "string" && typeof t.x == "object" && typeof t.y == "object" && "topLeft" in t.x && "topRight" in t.x && "bottomRight" in t.x && "bottomLeft" in t.x && "topLeft" in t.y && "topRight" in t.y && "bottomRight" in t.y && "bottomLeft" in t.y;
}
function gt(t) {
    var h;
    const e = t.match(/(\d+(?:\.\d+)?)(px|%)/g);
    if (!e)
        return {
            x: { topLeft: 0, topRight: 0, bottomRight: 0, bottomLeft: 0 },
            y: { topLeft: 0, topRight: 0, bottomRight: 0, bottomLeft: 0 },
            unit: "px"
        };
    const n = e.map((c) => {
        const [s, f, y] = c.match(/(\d+(?:\.\d+)?)(px|%)/) ?? [];
        return { value: parseFloat(f), unit: y };
    }), o = ((h = n[0]) == null ? void 0 : h.unit) || "px";
    if (n.some((c) => c.unit !== o))
        throw new Error("Inconsistent units in border-radius string.");
    const [l, i, d, u] = n.map((c) => c.value), a = {
        topLeft: l ?? 0,
        topRight: i ?? l ?? 0,
        bottomRight: d ?? l ?? 0,
        bottomLeft: u ?? i ?? l ?? 0
    };
    return {
        x: { ...a },
        y: { ...a },
        unit: o
    };
}
function ht({ x: t, y: e, unit: n }, o, l) {
    if (n === "px") {
        const i = {
            topLeft: t.topLeft / o,
            topRight: t.topRight / o,
            bottomLeft: t.bottomLeft / o,
            bottomRight: t.bottomRight / o
        }, d = {
            topLeft: e.topLeft / l,
            topRight: e.topRight / l,
            bottomLeft: e.bottomLeft / l,
            bottomRight: e.bottomRight / l
        };
        return { x: i, y: d, unit: "px" };
    } else if (n === "%")
        return { x: t, y: e, unit: "%" };
    return { x: t, y: e, unit: n };
}
function G(t) {
    return `
    ${t.x.topLeft}${t.unit} ${t.x.topRight}${t.unit} ${t.x.bottomRight}${t.unit} ${t.x.bottomLeft}${t.unit}
    /
    ${t.y.topLeft}${t.unit} ${t.y.topRight}${t.unit} ${t.y.bottomRight}${t.unit} ${t.y.bottomLeft}${t.unit}
  `;
}
function it(t) {
    return t.x.topLeft === 0 && t.x.topRight === 0 && t.x.bottomRight === 0 && t.x.bottomLeft === 0 && t.y.topLeft === 0 && t.y.topRight === 0 && t.y.bottomRight === 0 && t.y.bottomLeft === 0;
}
function at(t) {
    return typeof t == "object" && "x" in t && "y" in t;
}
function R(t, e) {
    return { x: t, y: e };
}
function xt(t, e) {
    return R(t.x + e.x, t.y + e.y);
}
function vt(t, e) {
    return R(t.x - e.x, t.y - e.y);
}
function It(t, e) {
    return R(t.x * e, t.y * e);
}
function C(t, e, n) {
    return t + (e - t) * n;
}
function At(t, e, n) {
    return xt(t, It(vt(e, t), n));
}
function pt(t, e, n) {
    return {
        x: {
            topLeft: C(t.x.topLeft, e.x.topLeft, n),
            topRight: C(t.x.topRight, e.x.topRight, n),
            bottomRight: C(t.x.bottomRight, e.x.bottomRight, n),
            bottomLeft: C(t.x.bottomLeft, e.x.bottomLeft, n)
        },
        y: {
            topLeft: C(t.y.topLeft, e.y.topLeft, n),
            topRight: C(t.y.topRight, e.y.topRight, n),
            bottomRight: C(t.y.bottomRight, e.y.bottomRight, n),
            bottomLeft: C(t.y.bottomLeft, e.y.bottomLeft, n)
        },
        unit: t.unit
    };
}
function Et(t, e, n) {
    return V((n - t) / (e - t), 0, 1);
}
function U(t, e, n, o, l) {
    return C(n, o, Et(t, e, l));
}
function V(t, e, n) {
    return Math.min(Math.max(t, e), n);
}
const Xt = {
    duration: 350,
    easing: (t) => t
};
function mt(t, e, n, o) {
    let l = !1;
    const i = () => {
        l = !0;
    }, d = { ...Xt, ...o };
    let u;
    function a(h) {
        u === void 0 && (u = h);
        const c = h - u, s = V(c / d.duration, 0, 1), f = Object.keys(t), y = Object.keys(e);
        if (!f.every((g) => y.includes(g))) {
            console.error("animate Error: `from` keys are different than `to`");
            return;
        }
        const v = {};
        f.forEach((g) => {
            typeof t[g] == "number" && typeof e[g] == "number" ? v[g] = C(
                t[g],
                e[g],
                d.easing(s)
            ) : rt(t[g]) && rt(e[g]) ? v[g] = pt(
                t[g],
                e[g],
                d.easing(s)
            ) : at(t[g]) && at(e[g]) && (v[g] = At(
                t[g],
                e[g],
                d.easing(s)
            ));
        }), n(v, s >= 1, s), s < 1 && !l && requestAnimationFrame(a);
    }
    return requestAnimationFrame(a), i;
}
const Tt = {
    startDelay: 0,
    targetEl: null
};
function Mt(t, e) {
    const n = { ...Tt, ...e };
    let o = t.el(), l = !1, i = null, d = null, u = null, a = null, h = 0, c = 0, s = 0, f = 0, y = 0, v = 0, g = 0, T = 0, r = 0, I = 0, E = null, p;
    o.addEventListener("pointerdown", w), document.body.addEventListener("pointerup", x), document.body.addEventListener("pointermove", X), document.body.addEventListener("touchmove", M, { passive: !1 });
    function w(m) {
        if (n.targetEl && m.target !== n.targetEl && !n.targetEl.contains(m.target) || l || !m.isPrimary) return;
        n.startDelay > 0 ? (u == null || u({ el: m.target }), p = setTimeout(() => {
            B();
        }, n.startDelay)) : B();
        function B() {
            E = m.target;
            const L = t.boundingRect(), F = t.layoutRect();
            y = F.x, v = F.y, s = L.x - y, f = L.y - v, h = m.clientX - s, c = m.clientY - f, g = m.clientX, T = m.clientY, r = (m.clientX - L.x) / L.width, I = (m.clientY - L.y) / L.height, l = !0, X(m);
        }
    }
    function A() {
        const m = t.layoutRect();
        h -= y - m.x, c -= v - m.y, y = m.x, v = m.y;
    }
    function x(m) {
        if (!l) {
            p && (clearTimeout(p), p = null, a == null || a({ el: m.target }));
            return;
        }
        if (!m.isPrimary) return;
        l = !1;
        const B = m.clientX - g, L = m.clientY - T;
        d == null || d({
            x: s,
            y: f,
            pointerX: m.clientX,
            pointerY: m.clientY,
            width: B,
            height: L,
            relativeX: r,
            relativeY: I,
            el: E
        }), E = null;
    }
    function X(m) {
        if (!l) {
            p && (clearTimeout(p), p = null, a == null || a({ el: m.target }));
            return;
        }
        if (!m.isPrimary) return;
        const B = m.clientX - g, L = m.clientY - T, F = s = m.clientX - h, Z = f = m.clientY - c;
        i == null || i({
            width: B,
            height: L,
            x: F,
            y: Z,
            pointerX: m.clientX,
            pointerY: m.clientY,
            relativeX: r,
            relativeY: I,
            el: E
        });
    }
    function M(m) {
        if (!l) return !0;
        m.preventDefault();
    }
    function D(m) {
        i = m;
    }
    function H(m) {
        d = m;
    }
    function _(m) {
        u = m;
    }
    function P(m) {
        a = m;
    }
    function $() {
        o.removeEventListener("pointerdown", w), o = t.el(), o.addEventListener("pointerdown", w);
    }
    function N() {
        t.el().removeEventListener("pointerdown", w), document.body.removeEventListener("pointerup", x), document.body.removeEventListener("pointermove", X), document.body.removeEventListener("touchmove", M), i = null, d = null, u = null, a = null;
    }
    return {
        onDrag: D,
        onDrop: H,
        onHold: _,
        onRelease: P,
        onElementUpdate: $,
        destroy: N,
        readjust: A
    };
}
function Dt(t) {
    return 1 + 2.70158 * Math.pow(t - 1, 3) + 1.70158 * Math.pow(t - 1, 2);
}
function bt(t) {
    return 1 - Math.pow(1 - t, 3);
}
function z(t) {
    return {
        x: t.x,
        y: t.y,
        width: t.width,
        height: t.height
    };
}
function Lt(t) {
    const e = t.getBoundingClientRect();
    let n = 0, o = 0, l = t.parentElement;
    for (; l;) {
        const d = getComputedStyle(l).transform;
        if (d && d !== "none") {
            const u = d.match(/matrix.*\((.+)\)/);
            if (u) {
                const a = u[1].split(", ").map(Number);
                n += a[4] || 0, o += a[5] || 0;
            }
        }
        l = l.parentElement;
    }
    return {
        y: e.top - o,
        x: e.left - n,
        width: e.width,
        height: e.height
    };
}
function J(t) {
    let e = t, n = 0, o = 0;
    for (; e;)
        n += e.offsetTop, o += e.offsetLeft, e = e.offsetParent;
    return {
        x: o,
        y: n,
        width: t.offsetWidth,
        height: t.offsetHeight
    };
}
function st(t, e) {
    return t.x >= e.x && t.x <= e.x + e.width && t.y >= e.y && t.y <= e.y + e.height;
}
function Yt(t) {
    let e = t, n = 0, o = 0;
    for (; e;) {
        const l = (i) => {
            const d = getComputedStyle(i);
            return /(auto|scroll)/.test(
                d.overflow + d.overflowY + d.overflowX
            );
        };
        if (e === document.body) {
            o += window.scrollX, n += window.scrollY;
            break;
        }
        l(e) && (o += e.scrollLeft, n += e.scrollTop), e = e.parentElement;
    }
    return { x: o, y: n };
}
function Q(t) {
    let e = "unread", n, o, l, i, d, u, a, h, c, s, f;
    function y() {
        n = t.currentTransform(), o = Lt(t.el()), l = Yt(t.el()), f = ct(t.el()).map(({ parent: I, children: E }) => ({
            parent: {
                el: I,
                initialRect: z(I.getBoundingClientRect())
            },
            children: E.filter((p) => p instanceof HTMLElement).map((p) => {
                const w = p;
                return w.originalBorderRadius || (w.originalBorderRadius = getComputedStyle(p).borderRadius), {
                    el: p,
                    borderRadius: gt(w.originalBorderRadius),
                    initialRect: z(
                        p.getBoundingClientRect()
                    )
                };
            })
        })), e = "readInitial";
    }
    function v() {
        if (e !== "readInitial")
            throw new Error(
                "FlipView: Cannot read final values before reading initial values"
            );
        c = t.layoutRect(), u = o.width / c.width, a = o.height / c.height, i = o.x - c.x - n.dragX + l.x, d = o.y - c.y - n.dragY + l.y, h = ht(
            t.borderRadius(),
            u,
            a
        );
        const r = ct(t.el());
        f = f.map(
            ({ parent: E, children: p }, w) => {
                const A = r[w].parent;
                return {
                    parent: {
                        ...E,
                        el: A,
                        finalRect: J(A)
                    },
                    children: p.map((x, X) => {
                        const M = r[w].children[X];
                        let D = J(M);
                        return M.hasAttribute("data-swapy-text") && (D = {
                            ...D,
                            width: x.initialRect.width,
                            height: x.initialRect.height
                        }), {
                            ...x,
                            el: M,
                            finalRect: D
                        };
                    })
                };
            }
        );
        const I = {
            translateX: i,
            translateY: d,
            scaleX: u,
            scaleY: a
        };
        t.el().style.transformOrigin = "0 0", t.el().style.borderRadius = G(
            h
        ), t.setTransform(I), s = [], f.forEach(({ parent: E, children: p }) => {
            const w = p.map(
                ({ el: A, initialRect: x, finalRect: X, borderRadius: M }) => Ct(
                    A,
                    x,
                    X,
                    M,
                    E.initialRect,
                    E.finalRect
                )
            );
            s.push(...w);
        }), e = "readFinal";
    }
    function g() {
        if (e !== "readFinal")
            throw new Error("FlipView: Cannot get transition values before reading");
        return {
            from: {
                width: o.width,
                height: o.height,
                translate: R(i, d),
                scale: R(u, a),
                borderRadius: h
            },
            to: {
                width: c.width,
                height: c.height,
                translate: R(0, 0),
                scale: R(1, 1),
                borderRadius: t.borderRadius()
            }
        };
    }
    function T() {
        if (e !== "readFinal")
            throw new Error(
                "FlipView: Cannot get children transition values before reading"
            );
        return s;
    }
    return {
        readInitial: y,
        readFinalAndReverse: v,
        transitionValues: g,
        childrenTransitionData: T
    };
}
function Ct(t, e, n, o, l, i) {
    t.style.transformOrigin = "0 0";
    const d = l.width / i.width, u = l.height / i.height, a = e.width / n.width, h = e.height / n.height, c = ht(
        o,
        a,
        h
    ), s = e.x - l.x, f = n.x - i.x, y = e.y - l.y, v = n.y - i.y, g = (s - f * d) / d, T = (y - v * u) / u;
    return t.style.transform = `translate(${g}px, ${T}px) scale(${a / d}, ${h / u})`, t.style.borderRadius = G(c), {
        el: t,
        fromTranslate: R(g, T),
        fromScale: R(a, h),
        fromBorderRadius: c,
        toBorderRadius: o,
        parentScale: { x: d, y: u }
    };
}
function ct(t) {
    const e = [];
    function n(o) {
        const l = Array.from(o.children).filter(
            (i) => i instanceof HTMLElement
        );
        l.length > 0 && (e.push({
            parent: o,
            children: l
        }), l.forEach((i) => n(i)));
    }
    return n(t), e;
}
function yt(t) {
    const e = [];
    let n = t, o = {
        dragX: 0,
        dragY: 0,
        translateX: 0,
        translateY: 0,
        scaleX: 1,
        scaleY: 1
    };
    const l = gt(
        window.getComputedStyle(n).borderRadius
    ), i = {
        el: () => n,
        setTransform: d,
        clearTransform: u,
        currentTransform: () => o,
        borderRadius: () => l,
        layoutRect: () => J(n),
        boundingRect: () => z(n.getBoundingClientRect()),
        usePlugin: h,
        destroy: c,
        updateElement: s
    };
    function d(f) {
        o = { ...o, ...f }, a();
    }
    function u() {
        o = {
            dragX: 0,
            dragY: 0,
            translateX: 0,
            translateY: 0,
            scaleX: 1,
            scaleY: 1
        }, a();
    }
    function a() {
        const { dragX: f, dragY: y, translateX: v, translateY: g, scaleX: T, scaleY: r } = o;
        f === 0 && y === 0 && v === 0 && g === 0 && T === 1 && r === 1 ? n.style.transform = "" : n.style.transform = `translate(${f + v}px, ${y + g}px) scale(${T}, ${r})`;
    }
    function h(f, y) {
        const v = f(i, y);
        return e.push(v), v;
    }
    function c() {
        e.forEach((f) => f.destroy());
    }
    function s(f) {
        if (!f) return;
        const y = n.hasAttribute("data-swapy-dragging"), v = n.style.cssText;
        n = f, y && n.setAttribute("data-swapy-dragging", ""), n.style.cssText = v, e.forEach((g) => g.onElementUpdate());
    }
    return i;
}
function Rt(t, e, n) {
    return n.map((o) => ({
        slotId: o.slot,
        itemId: o.item,
        item: o.item === "" ? null : t.find((l) => o.item === l[e])
    }));
}
function Ht(t, e) {
    return t.map((n) => ({
        item: n[e],
        slot: n[e]
    }));
}
function $t(t, e, n, o, l, i = !1) {
    const d = e.filter(
        (h) => !o.some((c) => c.item === h[n])
    ).map((h) => ({
        slot: h[n],
        item: h[n]
    }));
    let u;
    i ? u = o.map((h) => e.some((c) => c[n] === h.item) ? h : { slot: h.slot, item: "" }) : u = o.filter(
        (h) => e.some((c) => c[n] === h.item) || !h.item
    );
    const a = [
        ...u,
        ...d
    ];
    l(a), (d.length > 0 || u.length !== o.length) && requestAnimationFrame(() => {
        t == null || t.update();
    });
}
const jt = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
    __proto__: null,
    dynamicSwapy: $t,
    initSlotItemMap: Ht,
    toSlottedItems: Rt
}, Symbol.toStringTag, { value: "Module" })), Bt = {
    animation: "dynamic",
    enabled: !0,
    swapMode: "hover",
    dragOnHold: !1,
    autoScrollOnDrag: !1,
    dragAxis: "both",
    manualSwap: !1
};
function wt(t) {
    switch (t) {
        case "dynamic":
            return { easing: bt, duration: 300 };
        case "spring":
            return { easing: Dt, duration: 350 };
        case "none":
            return { easing: (e) => e, duration: 1 };
    }
}
function kt(t, e) {
    const n = { ...Bt, ...e }, o = Ot({ slots: [], items: [], config: n });
    let l = [], i = [];
    d();
    function d() {
        if (!qt(t))
            throw new Error(
                "Cannot create a Swapy instance because your HTML structure is invalid. Fix all above errors and then try!"
            );
        l = Array.from(t.querySelectorAll("[data-swapy-slot]")).map(
            (r) => _t(r, o)
        ), o.setSlots(l), i = Array.from(t.querySelectorAll("[data-swapy-item]")).map(
            (r) => Pt(r, o)
        ), o.setItems(i), o.syncSlotItemMap(), i.forEach((r) => {
            r.onDrag(({ pointerX: I, pointerY: E }) => {
                a();
                let p = !1;
                l.forEach((w) => {
                    const A = w.rect();
                    st({ x: I, y: E }, A) && (p = !0, w.isHighlighted() || w.highlight());
                }), !p && o.config().swapMode === "drop" && r.slot().highlight(), n.swapMode === "hover" && u(r, { pointerX: I, pointerY: E });
            }), r.onDrop(({ pointerX: I, pointerY: E }) => {
                h(), n.swapMode === "drop" && u(r, { pointerX: I, pointerY: E });
            }), r.onHold(() => {
                a();
            }), r.onRelease(() => {
                h();
            });
        });
    }
    function u(r, { pointerX: I, pointerY: E }) {
        l.forEach((p) => {
            const w = p.rect();
            if (st({ x: I, y: E }, w)) {
                if (r.id() === p.itemId()) return;
                o.config().swapMode === "hover" && r.setContinuousDrag(!0);
                const A = r.slot(), x = p.item();
                if (!o.eventHandlers().onBeforeSwap({
                    fromSlot: A.id(),
                    toSlot: p.id(),
                    draggingItem: r.id(),
                    swapWithItem: (x == null ? void 0 : x.id()) || ""
                }))
                    return;
                if (o.config().manualSwap) {
                    const X = structuredClone(o.slotItemMap());
                    o.swapItems(r, p);
                    const M = o.slotItemMap(), D = Q(r.view());
                    D.readInitial();
                    const H = x ? Q(x.view()) : null;
                    H == null || H.readInitial();
                    let _ = 0, P = 0;
                    const $ = et(
                        r.view().el()
                    );
                    $ instanceof Window ? (_ = $.scrollY, P = $.scrollX) : (_ = $.scrollTop, P = $.scrollLeft), o.eventHandlers().onSwap({
                        oldSlotItemMap: X,
                        newSlotItemMap: M,
                        fromSlot: A.id(),
                        toSlot: p.id(),
                        draggingItem: r.id(),
                        swappedWithItem: (x == null ? void 0 : x.id()) || ""
                    }), requestAnimationFrame(() => {
                        const N = t.querySelectorAll("[data-swapy-item]");
                        o.items().forEach((m) => {
                            const B = Array.from(N).find(
                                (L) => L.dataset.swapyItem === m.id()
                            );
                            m.view().updateElement(B);
                        }), o.syncSlotItemMap(), D.readFinalAndReverse(), H == null || H.readFinalAndReverse(), tt(r, D), x && H && tt(x, H), $.scrollTo({
                            left: P,
                            top: _
                        });
                    });
                } else {
                    let X = 0, M = 0;
                    const D = et(
                        r.view().el()
                    );
                    D instanceof Window ? (X = D.scrollY, M = D.scrollX) : (X = D.scrollTop, M = D.scrollLeft), ft(r, p, !0), x && ft(x, A), D.scrollTo({
                        left: M,
                        top: X
                    });
                    const H = o.slotItemMap();
                    o.syncSlotItemMap();
                    const _ = o.slotItemMap();
                    o.eventHandlers().onSwap({
                        oldSlotItemMap: H,
                        newSlotItemMap: _,
                        fromSlot: A.id(),
                        toSlot: p.id(),
                        draggingItem: r.id(),
                        swappedWithItem: (x == null ? void 0 : x.id()) || ""
                    });
                }
            }
        });
    }
    function a() {
        t.querySelectorAll("img").forEach((r) => {
            r.style.pointerEvents = "none";
        }), t.style.userSelect = "none", t.style.webkitUserSelect = "none";
    }
    function h() {
        t.querySelectorAll("img").forEach((r) => {
            r.style.pointerEvents = "";
        }), t.style.userSelect = "", t.style.webkitUserSelect = "";
    }
    function c(r) {
        o.config().enabled = r;
    }
    function s(r) {
        o.eventHandlers().onSwapStart = r;
    }
    function f(r) {
        o.eventHandlers().onSwap = r;
    }
    function y(r) {
        o.eventHandlers().onSwapEnd = r;
    }
    function v(r) {
        o.eventHandlers().onBeforeSwap = r;
    }
    function g() {
        T(), requestAnimationFrame(() => {
            d();
        });
    }
    function T() {
        i.forEach((r) => r.destroy()), l.forEach((r) => r.destroy()), o.destroy(), i = [], l = [];
    }
    return {
        enable: c,
        slotItemMap: () => o.slotItemMap(),
        onSwapStart: s,
        onSwap: f,
        onSwapEnd: y,
        onBeforeSwap: v,
        update: g,
        destroy: T
    };
}
function Ot({
    slots: t,
    items: e,
    config: n
}) {
    const o = {
        slots: t,
        items: e,
        config: n,
        slotItemMap: { asObject: {}, asMap: /* @__PURE__ */ new Map(), asArray: [] },
        zIndexCount: 1,
        eventHandlers: {
            onSwapStart: () => {
            },
            onSwap: () => {
            },
            onSwapEnd: () => {
            },
            onBeforeSwap: () => !0
        },
        scrollOffsetWhileDragging: { x: 0, y: 0 },
        scrollHandler: null
    };
    let l = {
        ...o
    };
    const i = (s) => {
        var f;
        (f = l.scrollHandler) == null || f.call(l, s);
    };
    window.addEventListener("scroll", i);
    function d(s) {
        return l.slots.find((f) => f.id() === s);
    }
    function u(s) {
        return l.items.find((f) => f.id() === s);
    }
    function a() {
        const s = {}, f = /* @__PURE__ */ new Map(), y = [];
        l.slots.forEach((v) => {
            var r;
            const g = v.id(), T = ((r = v.item()) == null ? void 0 : r.id()) || "";
            s[g] = T, f.set(g, T), y.push({ slot: g, item: T });
        }), l.slotItemMap = { asObject: s, asMap: f, asArray: y };
    }
    function h(s, f) {
        var p;
        const y = l.slotItemMap, v = s.id(), g = ((p = f.item()) == null ? void 0 : p.id()) || "", T = f.id(), r = s.slot().id();
        y.asObject[T] = v, y.asObject[r] = g, y.asMap.set(T, v), y.asMap.set(r, g);
        const I = y.asArray.findIndex(
            (w) => w.slot === T
        ), E = y.asArray.findIndex(
            (w) => w.slot === r
        );
        y.asArray[I].item = v, y.asArray[E].item = g;
    }
    function c() {
        window.removeEventListener("scroll", i), l = { ...o };
    }
    return {
        slots: () => l.slots,
        items: () => l.items,
        config: () => n,
        setItems: (s) => l.items = s,
        setSlots: (s) => l.slots = s,
        slotById: d,
        itemById: u,
        zIndex: (s = !1) => s ? ++l.zIndexCount : l.zIndexCount,
        resetZIndex: () => {
            l.zIndexCount = 1;
        },
        eventHandlers: () => l.eventHandlers,
        syncSlotItemMap: a,
        slotItemMap: (s = !1) => s ? structuredClone(l.slotItemMap) : l.slotItemMap,
        onScroll: (s) => {
            l.scrollHandler = s;
        },
        swapItems: h,
        destroy: c
    };
}
function _t(t, e) {
    const n = yt(t);
    function o() {
        return n.el().dataset.swapySlot;
    }
    function l() {
        const c = n.el().children[0];
        return (c == null ? void 0 : c.dataset.swapyItem) || null;
    }
    function i() {
        return z(n.el().getBoundingClientRect());
    }
    function d() {
        const c = n.el().children[0];
        if (c)
            return e.itemById(c.dataset.swapyItem);
    }
    function u() {
        e.slots().forEach((c) => {
            c.view().el().removeAttribute("data-swapy-highlighted");
        });
    }
    function a() {
        u(), n.el().setAttribute("data-swapy-highlighted", "");
    }
    function h() {
    }
    return {
        id: o,
        view: () => n,
        itemId: l,
        rect: i,
        item: d,
        highlight: a,
        unhighlightAllSlots: u,
        isHighlighted: () => n.el().hasAttribute("data-swapy-highlighted"),
        destroy: h
    };
}
function Pt(t, e) {
    const n = yt(t), o = {};
    let l = null, i = null, d = !1, u = !0, a;
    const h = Nt();
    let c = () => {
    }, s = () => {
    }, f = () => {
    }, y = () => {
    };
    const { onDrag: v, onDrop: g, onHold: T, onRelease: r } = n.usePlugin(Mt, {
        startDelay: e.config().dragOnHold ? 400 : 0,
        targetEl: P()
    }), I = R(0, 0), E = R(0, 0), p = R(0, 0), w = R(0, 0);
    let A = null, x = null;
    T((S) => {
        e.config().enabled && (N() && !$(S.el) || L() && B(S.el) || f == null || f(S));
    }), r((S) => {
        e.config().enabled && (N() && !$(S.el) || L() && B(S.el) || y == null || y(S));
    });
    function X(S) {
        var q;
        F(), K().highlight(), (q = o.drop) == null || q.call(o);
        const Y = e.slots().map((O) => O.view().boundingRect());
        e.slots().forEach((O, W) => {
            const j = Y[W];
            O.view().el().style.width = `${j.width}px`, O.view().el().style.maxWidth = `${j.width}px`, O.view().el().style.flexShrink = "0", O.view().el().style.height = `${j.height}px`;
        });
        const b = e.slotItemMap(!0);
        e.eventHandlers().onSwapStart({
            draggingItem: nt(),
            fromSlot: ot(),
            slotItemMap: b
        }), i = b, n.el().style.position = "relative", n.el().style.zIndex = `${e.zIndex(!0)}`, A = et(S.el), e.config().autoScrollOnDrag && (l = Wt(
            A,
            e.config().dragAxis
        ), l.updatePointer({
            x: S.pointerX,
            y: S.pointerY
        })), I.x = window.scrollX, I.y = window.scrollY, p.x = 0, p.y = 0, A instanceof HTMLElement && (E.x = A.scrollLeft, E.y = A.scrollTop, x = () => {
            w.x = A.scrollLeft - E.x, w.y = A.scrollTop - E.y, n.setTransform({
                dragX: ((a == null ? void 0 : a.width) || 0) + p.x + w.x,
                dragY: ((a == null ? void 0 : a.height) || 0) + p.y + w.y
            });
        }, A.addEventListener("scroll", x)), e.onScroll(() => {
            p.x = window.scrollX - I.x, p.y = window.scrollY - I.y;
            const O = w.x || 0, W = w.y || 0;
            n.setTransform({
                dragX: ((a == null ? void 0 : a.width) || 0) + p.x + O,
                dragY: ((a == null ? void 0 : a.height) || 0) + p.y + W
            });
        });
    }
    v((S) => {
        var Y;
        if (e.config().enabled) {
            if (!d) {
                if (N() && !$(S.el) || L() && B(S.el))
                    return;
                X(S);
            }
            d = !0, l && l.updatePointer({
                x: S.pointerX,
                y: S.pointerY
            }), a = S, (Y = o.drop) == null || Y.call(o), h(() => {
                n.el().style.position = "relative";
                const b = S.width + p.x + w.x, q = S.height + p.y + w.y;
                e.config().dragAxis === "y" ? n.setTransform({
                    dragY: q
                }) : e.config().dragAxis === "x" ? n.setTransform({
                    dragX: b
                }) : n.setTransform({
                    dragX: b,
                    dragY: q
                }), c == null || c(S);
            });
        }
    }), g((S) => {
        if (!d) return;
        Z(), d = !1, u = !1, a = null, A && (A.removeEventListener("scroll", x), x = null), A = null, w.x = 0, w.y = 0, p.x = 0, p.y = 0, l && (l.destroy(), l = null), K().unhighlightAllSlots(), s == null || s(S), e.eventHandlers().onSwapEnd({
            slotItemMap: e.slotItemMap(),
            hasChanged: i != null && i.asMap ? !Ft(
                i == null ? void 0 : i.asMap,
                e.slotItemMap().asMap
            ) : !1
        }), i = null, e.onScroll(null), e.slots().forEach((b) => {
            b.view().el().style.width = "", b.view().el().style.maxWidth = "", b.view().el().style.flexShrink = "", b.view().el().style.height = "";
        }), e.config().manualSwap && e.config().swapMode === "drop" ? requestAnimationFrame(Y) : Y();
        function Y() {
            const b = n.currentTransform(), q = b.dragX + b.translateX, O = b.dragY + b.translateY;
            o.drop = mt(
                { translate: R(q, O) },
                { translate: R(0, 0) },
                ({ translate: W }, j) => {
                    j ? d || (n.clearTransform(), n.el().style.transformOrigin = "") : n.setTransform({
                        dragX: 0,
                        dragY: 0,
                        translateX: W.x,
                        translateY: W.y
                    }), j && (e.items().forEach((lt) => {
                        lt.isDragging() || (lt.view().el().style.zIndex = "");
                    }), e.resetZIndex(), n.el().style.position = "", u = !0);
                },
                wt(e.config().animation)
            );
        }
    });
    function M(S) {
        c = S;
    }
    function D(S) {
        s = S;
    }
    function H(S) {
        f = S;
    }
    function _(S) {
        y = S;
    }
    function P() {
        return n.el().querySelector("[data-swapy-handle]");
    }
    function $(S) {
        const Y = P();
        return Y ? Y === S || Y.contains(S) : !1;
    }
    function N() {
        return P() !== null;
    }
    function m() {
        return Array.from(n.el().querySelectorAll("[data-swapy-no-drag]"));
    }
    function B(S) {
        const Y = m();
        return !Y || Y.length === 0 ? !1 : Y.includes(S) || Y.some((b) => b.contains(S));
    }
    function L() {
        return m().length > 0;
    }
    function F() {
        n.el().setAttribute("data-swapy-dragging", "");
    }
    function Z() {
        n.el().removeAttribute("data-swapy-dragging");
    }
    function St() {
        c = null, s = null, f = null, y = null, a = null, i = null, l && (l.destroy(), l = null), A && x && A.removeEventListener("scroll", x), n.destroy();
    }
    function nt() {
        return n.el().dataset.swapyItem;
    }
    function K() {
        return e.slotById(n.el().parentElement.dataset.swapySlot);
    }
    function ot() {
        return n.el().parentElement.dataset.swapySlot;
    }
    return {
        id: nt,
        view: () => n,
        slot: K,
        slotId: ot,
        onDrag: M,
        onDrop: D,
        onHold: H,
        onRelease: _,
        destroy: St,
        isDragging: () => d,
        cancelAnimation: () => o,
        dragEvent: () => a,
        store: () => e,
        continuousDrag: () => u,
        setContinuousDrag: (S) => u = S
    };
}
function ft(t, e, n = !1) {
    if (n) {
        const l = e.item();
        l && (e.view().el().style.position = "relative", l.view().el().style.position = "absolute");
    } else {
        const l = t.slot();
        l.view().el().style.position = "", t.view().el().style.position = "";
    }
    if (!t)
        return;
    const o = Q(t.view());
    o.readInitial(), e.view().el().appendChild(t.view().el()), o.readFinalAndReverse(), tt(t, o);
}
function Nt() {
    let t = !1;
    return (e) => {
        t || (t = !0, requestAnimationFrame(() => {
            e(), t = !1;
        }));
    };
}
function tt(t, e) {
    var u, a, h, c;
    (a = (u = t.cancelAnimation()).moveToSlot) == null || a.call(u), (c = (h = t.cancelAnimation()).drop) == null || c.call(h);
    const n = wt(t.store().config().animation), o = e.transitionValues();
    let l = t.view().currentTransform(), i = 0, d = !1;
    t.cancelAnimation().moveToSlot = mt(
        {
            translate: o.from.translate,
            scale: o.from.scale,
            borderRadius: o.from.borderRadius
        },
        {
            translate: o.to.translate,
            scale: o.to.scale,
            borderRadius: o.to.borderRadius
        },
        ({ translate: s, scale: f, borderRadius: y }, v, g) => {
            if (t.isDragging()) {
                i !== 0 && (d = !0);
                const r = t.dragEvent().relativeX, I = t.dragEvent().relativeY;
                t.continuousDrag() ? t.view().setTransform({
                    translateX: C(
                        l.translateX,
                        l.translateX + (o.from.width - o.to.width) * r,
                        n.easing(g - i)
                    ),
                    translateY: C(
                        l.translateY,
                        l.translateY + (o.from.height - o.to.height) * I,
                        n.easing(g - i)
                    ),
                    scaleX: f.x,
                    scaleY: f.y
                }) : t.view().setTransform({ scaleX: f.x, scaleY: f.y });
            } else
                l = t.view().currentTransform(), i = g, d ? t.view().setTransform({
                    scaleX: f.x,
                    scaleY: f.y
                }) : t.view().setTransform({
                    dragX: 0,
                    dragY: 0,
                    translateX: s.x,
                    translateY: s.y,
                    scaleX: f.x,
                    scaleY: f.y
                });
            const T = e.childrenTransitionData();
            T.forEach(
                ({
                    el: r,
                    fromTranslate: I,
                    fromScale: E,
                    fromBorderRadius: p,
                    toBorderRadius: w,
                    parentScale: A
                }) => {
                    const x = C(
                        A.x,
                        1,
                        n.easing(g)
                    ), X = C(
                        A.y,
                        1,
                        n.easing(g)
                    );
                    r.style.transform = `translate(${I.x + (0 - I.x / x) * n.easing(g)}px, ${I.y + (0 - I.y / X) * n.easing(g)}px) scale(${C(
                        E.x / x,
                        1 / x,
                        n.easing(g)
                    )}, ${C(
                        E.y / X,
                        1 / X,
                        n.easing(g)
                    )})`, it(p) || (r.style.borderRadius = G(
                        pt(
                            p,
                            w,
                            n.easing(g)
                        )
                    ));
                }
            ), it(y) || (t.view().el().style.borderRadius = G(y)), v && (t.isDragging() || (t.view().el().style.transformOrigin = "", t.view().clearTransform()), t.view().el().style.borderRadius = "", T.forEach(({ el: r }) => {
                r.style.transform = "", r.style.transformOrigin = "", r.style.borderRadius = "";
            }));
        },
        n
    );
}
function k(...t) {
    console.error("Swapy Error:", ...t);
}
function qt(t) {
    const e = t;
    let n = !0;
    const o = e.querySelectorAll("[data-swapy-slot]");
    e || (k("container passed to createSwapy() is undefined or null"), n = !1), o.forEach((u) => {
        const a = u, h = a.dataset.swapySlot, c = a.children, s = c[0];
        (!h || h.length === 0) && (k(a, "does not contain a slotId using data-swapy-slot"), n = !1), c.length > 1 && (k("slot:", `"${h}"`, "cannot contain more than one element"), n = !1), s && (!s.dataset.swapyItem || s.dataset.swapyItem.length === 0) && (k(
            "slot",
            `"${h}"`,
            "does not contain an element with an item id using data-swapy-item"
        ), n = !1);
    });
    const l = Array.from(o).map(
        (u) => u.dataset.swapySlot
    ), i = e.querySelectorAll("[data-swapy-item]"), d = Array.from(i).map(
        (u) => u.dataset.swapyItem
    );
    if (ut(l)) {
        const u = dt(l);
        k(
            "your container has duplicate slot ids",
            `(${u.join(", ")})`
        ), n = !1;
    }
    if (ut(d)) {
        const u = dt(d);
        k(
            "your container has duplicate item ids",
            `(${u.join(", ")})`
        ), n = !1;
    }
    return n;
}
function ut(t) {
    return new Set(t).size !== t.length;
}
function dt(t) {
    const e = /* @__PURE__ */ new Set(), n = /* @__PURE__ */ new Set();
    for (const o of t)
        e.has(o) ? n.add(o) : e.add(o);
    return Array.from(n);
}
function Ft(t, e) {
    if (t.size !== e.size) return !1;
    for (const [n, o] of t)
        if (e.get(n) !== o) return !1;
    return !0;
}
function et(t) {
    let e = t;
    for (; e;) {
        const n = window.getComputedStyle(e), o = n.overflowY, l = n.overflowX;
        if ((o === "auto" || o === "scroll") && e.scrollHeight > e.clientHeight || (l === "auto" || l === "scroll") && e.scrollWidth > e.clientWidth)
            return e;
        e = e.parentElement;
    }
    return window;
}
function Wt(t, e) {
    let l = !1, i, d = 0, u = 0, a = 0, h = 0, c = 0, s = 0, f = null;
    t instanceof HTMLElement ? (i = z(t.getBoundingClientRect()), d = t.scrollHeight - i.height, u = t.scrollWidth - i.width) : (i = {
        x: 0,
        y: 0,
        width: window.innerWidth,
        height: window.innerHeight
    }, d = document.documentElement.scrollHeight - window.innerHeight, u = document.documentElement.scrollWidth - window.innerWidth);
    function y() {
        t instanceof HTMLElement ? (a = t.scrollTop, h = t.scrollLeft) : (a = window.scrollY, h = window.scrollX);
    }
    function v(r) {
        l = !1;
        const I = i.y, E = i.y + i.height, p = i.x, w = i.x + i.width, A = Math.abs(I - r.y) < Math.abs(E - r.y), x = Math.abs(p - r.x) < Math.abs(w - r.x);
        if (y(), e !== "x")
            if (A) {
                const X = I - r.y;
                if (X >= -100) {
                    const M = V(X, -100, 0);
                    c = -U(-100, 0, 0, 5, M), l = !0;
                }
            } else {
                const X = E - r.y;
                if (X <= 100) {
                    const M = V(X, 0, 100);
                    c = U(100, 0, 0, 5, M), l = !0;
                }
            }
        if (e !== "y")
            if (x) {
                const X = p - r.x;
                if (X >= -100) {
                    const M = V(X, -100, 0);
                    s = -U(-100, 0, 0, 5, M), l = !0;
                }
            } else {
                const X = w - r.x;
                if (X <= 100) {
                    const M = V(X, 0, 100);
                    s = U(100, 0, 0, 5, M), l = !0;
                }
            }
        l && (f && cancelAnimationFrame(f), g());
    }
    function g() {
        y(), e !== "x" && (c = a + c >= d ? 0 : c), e !== "y" && (s = h + s >= u ? 0 : s), t.scrollBy({ top: c, left: s }), l && (f = requestAnimationFrame(g));
    }
    function T() {
        l = !1;
    }
    return {
        updatePointer: v,
        destroy: T
    };
}

// Attach Swapy to window for non-module usage
window.Swapy = {
    createSwapy: kt,
    getClosestScrollableContainer: et,
    utils: jt
};