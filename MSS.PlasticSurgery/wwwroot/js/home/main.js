"use strict";
var Perfecto = Perfecto || {};
Perfecto.App = function(e, t) {
    function n() {
        TWEEN.update(), a.render(), s.render(), requestAnimationFrame(n)
    }

    var r = document.getElementById(e);
    if (null == r) return this;
    r.style.overflow = "hidden";
    var i = Perfecto.MoveListener.getInstance(),
        a = new Perfecto.GridRender(r, "middle-renderer", t),
        s = new Perfecto.PageRender(r, "front-renderer"),
        c = new Perfecto.PolicyPage;
    n(), window.addEventListener("resize",
        function() {
            i.resize(), a.resize(), s.resize(), c.resize()
        }), this.setLinks = function(e) {
        a.setLinks(e)
    }, this.setLogo = function(e, t, n, r, src) {
        a.setLogo(e, t, n, r, src)
    }
}, Perfecto.addClassMethods = function(e) {
    e.hasClass = function(t) {
        for (var n = e.className.split(/\s+/), r = t.trim().split(/\s+/), i = n.length - 1; i >= 0; i--)
            if (r.indexOf(n[i]) != -1) return !0;
        return !1
    }, e.hasClassString = function(t) {
        return e.className.indexOf(t) != -1
    }, e.addClass = function(t) {
        var n = e.className,
            r = t.trim().split(/\s+/);
        return r.forEach(function(e) {
            n.indexOf(e) == -1 && (n += "" == n ? e : " " + e)
        }), e.className = n, e
    }, e.removeClass = function(t) {
        var n = e.className,
            r = t.trim().split(/\s+/);
        return r.forEach(function(e) {
            n = n.replace(e, "")
        }), e.className = n.trim(), e
    }
}, Perfecto.arrayFill = function(e, t) {
    var n = [];
    if (void 0 != t)
        for (var r = 0; r < e; r++) n.push(t);
    else
        for (var r = 0; r < e; r++) n.push(r);
    return n
}, Perfecto.buildProportionalSize = function(e, t) {
    var n = new THREE.Vector2;
    return t >= 1 ? (n.x = e, n.y = e / t) : (n.x = e / t, n.y = e), n
}, Perfecto.createElement = function(e) {
    var t = e.tag || "div",
        n = document.createElement(t);
    if (Perfecto.addClassMethods(n), e.class && n.addClass(e.class), e.text
        && n.appendChild(document.createTextNode(e.text)), e.atts) {
        var r = e.atts;
        Object.keys(r).forEach(function(e) {
            n[e] = r[e]
        })
    }
    return n
}, Perfecto.createElements = function(e) {
    function t(e, t) {
        var n = Perfecto.createElement(e);
        return t.appendChild(n), e.alias && (t[e.alias] = n), n
    }

    if (!e || !e.length) return null;
    for (var n = Perfecto.createElement(e[0]), r = n, i = 1, o = e.length; i < o; i++) {
        var a = e[i];
        if (Array.isArray(a)) {
            var s = null;
            a.forEach(function(e) {
                s = t(e, r)
            }), r = s
        } else r = t(a, r)
    }
    return n
}, Perfecto.ctg = function(e) {
    var t = THREE.Math.degToRad(e);
    return Math.cos(t) / Math.sin(t)
}, Perfecto.limit = function(e, t, n) {
    return Math.max(e, Math.min(t, n))
}, Perfecto.preferredHeight = function(e) {
    var t = .9 * e;
    return t >= 669 ? 669 : t
}, Perfecto.preferredWidth = function(e) {
    var t = .9 * e;
    return t >= 681 ? 681 : t
}, Perfecto.shuffleArray = function(e, t) {
    t = t || 3 * e.length;
    for (var n = e.length - 1, r = 0, i = e[r], o = 0; o < t; o++) {
        var a = Perfecto.random(0, n);
        i = e[r], e[r] = e[a], e[a] = i, r = a
    }
}, Perfecto.random = function(e, t) {
    return Math.round(Perfecto.randomFloat(e, t))
}, Perfecto.randomFloat = function(e, t) {
    return t -= e, Math.random() * t + e
}, Perfecto.Tween = function(e, t) {
    var n = arguments.length;
    if (0 == n) return null;
    var r = null;
    r = 1 == n
        ? Array.isArray(e)
        ? new Perfecto.MultipleTweens(e)
        : "function" == typeof e
        ? new Perfecto.TweenMethod(e)
        : new TWEEN.Tween(e)
        : new Perfecto.TweenGroup(e, t);
    var i = ["from", "duration", "add", "queue"],
        o = function() {
            return this
        };
    return i.forEach(function(e) {
        e in r || (r[e] = o)
    }), r
}, Perfecto.MultipleTweens = function(e) {
    function t() {
        if (v++, v == g && (null != f && f(), null != d))
            for (var e = 0, t = d.length; e < t; e++) d[e].start()
    }

    function n(e) {
        if ("object" == typeof e && null != e) return e;
        var t = {};
        return Object.keys(E).forEach(function(n) {
            t[n] = e
        }), t
    }

    function r(e) {
        if ("clone" in e) return e.clone();
        var t = {};
        return Object.keys(e).forEach(function(n) {
            t[n] = e[n]
        }), t
    }

    var i = "position",
        o = null,
        a = null,
        s = 1e3,
        c = Perfecto.arrayFill(g, 0),
        u = TWEEN.Easing.Linear.None,
        l = null,
        f = null,
        d = null,
        h = null,
        v = 0,
        g = e.length,
        p = g > 0,
        E = p ? e[0][i] : null;
    this.from = function(t, r) {
        return p && (o = n(t), void 0 != r && (i = r, E = e[0][i])), this
    }, this.to = function(e, t) {
        return p && (a = n(e), void 0 != t && (s = t)), this
    }, this.duration = function(e) {
        return s = e, this
    }, this.delay = function(e, t) {
        if (!p) return this;
        if (1 == arguments.length)
            if (Array.isArray(e)) {
                for (var n = Math.min(g, e.length), r = 0; r < n; r++) c[r] = e[r];
                if (n < g)
                    for (var r = n; r < g; r++) c[r] = 0
            } else {
                e = +e || 0;
                for (var r = 0; r < g; r++) c[r] = e
            }
        else
            for (var r = 0; r < g; r++) c[r] = e, e += t;
        return this
    }, this.easing = function(e) {
        return u = e, this
    }, this.onStart = function(e) {
        return l = e, this
    }, this.onComplete = function(e) {
        return f = e, this
    }, this.chain = function() {
        return d = arguments, this
    }, this.start = function() {
        h = [], v = 0;
        for (var n = 0; n < g; n++) {
            var f = e[n],
                d = f[i],
                E = null == a ? r(d) : a;
            null != o && d.copy(o);
            var m = new TWEEN.Tween(d);
            m.to(E, s).delay(c[n]).easing(u).onComplete(t), h.push(m)
        }
        return null != l && l(), h.forEach(function(e) {
            e.start()
        }), p || t(), this
    }, this.stop = function() {
        return null != h
            && (h.forEach(function(e) {
                e.stop()
            }), h = null), this
    }
}, Perfecto.TweenMethod = function(e) {
    function t(e) {
        var t = +e;
        return isNaN(t) && (t = +!!e), t
    }

    var n = 0,
        r = 0,
        i = 1,
        o = !1,
        a = 1e3,
        s = 0,
        c = TWEEN.Easing.Linear.None,
        u = null,
        l = null,
        f = null;
    this.queue = function(e) {
        return n = e, this
    }, this.from = function(e) {
        return r = t(e), o = !0, this
    }, this.to = function(e, n) {
        return i = t(e), o || (r = 0 != i ? 0 : 1), void 0 != n && (a = n), this
    }, this.duration = function(e) {
        return a = e, this
    }, this.delay = function(e) {
        return s = e, this
    }, this.easing = function(e) {
        return c = e, this
    }, this.onStart = function(e) {
        return u = e, this
    }, this.onComplete = function(e) {
        return l = e, this
    }, this.start = function() {
        return e(r, n), f = new TWEEN.Tween({
            k: r
        }), f.to({
                k: i
            },
            a).delay(s).easing(c).onUpdate(function() {
            e(this.k, n)
        }), null != u && f.onStart(u), null != l && f.onComplete(l), f.start(), this
    }, this.stop = function() {
        return null != f && f.stop(), this
    }
}, Perfecto.TweenGroup = function(e, t) {
    function n() {
        if (u++, u == t && (null != s && s(), null != c))
            for (var e = 0, n = c.length; e < n; e++) c[e].start()
    }

    function r(t) {
        for (var n = [], r = 0; r < t; r++) {
            var i = new Perfecto.Tween(e);
            n.push(i)
        }
        return n
    }

    var i = r(t);
    t = i.length;
    var o = t > 0,
        a = null,
        s = null,
        c = null,
        u = 0;
    this.add = function(e) {
        var n = new Perfecto.Tween(e);
        return i.push(n), t++, o = !0, n
    }, this.from = function() {
        var e = arguments;
        return i.forEach(function(t) {
            t.from.apply(t, e)
        }), this
    }, this.to = function(e, t) {
        return i.forEach(function(n) {
            n.to(e, t)
        }), this
    }, this.duration = function(e) {
        return i.forEach(function(t) {
            t.duration(e)
        }), this
    }, this.delay = function(e, n) {
        if (!o) return this;
        if (1 == arguments.length)
            if (Array.isArray(e)) {
                for (var r = Math.min(t, e.length), a = 0; a < r; a++) i[a].delay(e[a]);
                if (r < t)
                    for (var a = r; a < t; a++) i[a].delay(0)
            } else {
                e = +e || 0;
                for (var a = 0; a < t; a++) i[a].delay(e)
            }
        else
            for (var a = 0; a < t; a++) i[a].delay(e), e += n;
        return this
    }, this.easing = function(e) {
        return i.forEach(function(t) {
            t.easing(e)
        }), this
    }, this.onStart = function(e) {
        return a = e, this
    }, this.onComplete = function(e) {
        return s = e, this
    }, this.chain = function() {
        return c = arguments, this
    }, this.start = function() {
        return null != a && a(), u = 0, i.forEach(function(e, t) {
            e.onComplete(n).queue(t).start()
        }), this
    }, this.stop = function() {
        return i.forEach(function(e) {
            e.stop()
        }), this
    }
}, Perfecto.Render = function(e, t, n) {
    var r = e.clientWidth,
        i = e.clientHeight,
        o = new THREE.Scene,
        a = new THREE.PerspectiveCamera(75, r / i, .1, 1e3),
        s = null;
    s = n && "canvas" == n
            ? new THREE.CanvasRenderer({
                alpha: !0
            })
            : new THREE.CSS3DRenderer({
                alpha: !0
            }), s.setSize(r, i), s.setClearColor(16777215, 1), Perfecto.addClassMethods(s.domElement),
        t && s.domElement.addClass(t), e.appendChild(s.domElement), this.render = function() {
            s.render(o, a)
        }, this.resize = function() {
            r = e.clientWidth, i = e.clientHeight, a.aspect = r / i, a.updateProjectionMatrix(), s.setSize(r, i)
        }, this.resizeCamera = function() {
            var e;
            e = r >= i ? Math.min(r, i) : Math.max(r, i);
            var t = e / 2,
                n = a.fov / 2,
                o = t * Perfecto.ctg(n);
            return a.position.z = o, o
        }, this.getScene = function() {
            return o
        }, this.getCamera = function() {
            return a
        }, this.getRenderer = function() {
            return s
        }, this.getWidth = function() {
            return r
        }, this.getHeight = function() {
            return i
        }, this.getLessSide = function() {
            return Math.min(r, i)
        }, this.getBiggerSide = function() {
            return Math.max(r, i)
        }, this.dispatchEvent = function(t, n) {
            try {
                var r = new CustomEvent(t,
                    {
                        detail: n
                    })
            } catch (e) {
                var r = document.createEvent("CustomEvent");
                r.initCustomEvent(t, !1, !1, n)
            }
            e.dispatchEvent(r)
        }
}, Perfecto.BackgroundRender = function(e, t) {
    function n() {
        var e = a * (1 + u),
            t = Perfecto.buildProportionalSize(e, g.aspect);
        t.x *= -1;
        var n = o / 2,
            i = (.7 + u) * a,
            s = r(n, t, i, 0, -90),
            c = r(n, t, i, 0, -90);
        c.forEach(function(e) {
            e.position.multiplyScalar(-1)
        });
        var l = s.concat(c);
        Perfecto.shuffleArray(l), l.forEach(function(e) {
            v.add(e)
        });
        var f = a;
        f /= g.aspect >= 1 ? g.aspect : 1 / g.aspect;
        var d = g.fov / 2;
        return g.position.z = f * Perfecto.ctg(d), l
    }

    function r(e, t, n, r, i) {
        r = void 0 != r ? THREE.Math.degToRad(r) : 0, i = void 0 != i ? THREE.Math.degToRad(i) : Math.PI / 2;
        for (var o = {
                     color: 12632256,
                     transparent: !0,
                     opacity: .5,
                     program: function(e) {
                         e.beginPath(), e.arc(0, 0, 1, 0, 2 * Math.PI, !1), e.fill()
                     }
                 },
            a = Object.keys(THREE.ColorKeywords),
            u = a.length,
            l = [],
            f = 0;
            f < e;
            f++) {
            o.color = a[f % u];
            var d = new THREE.SpriteCanvasMaterial(o),
                h = new THREE.Sprite(d),
                v = Perfecto.random(0, n),
                g = Perfecto.randomFloat(r, i);
            h.position.x = t.x + v * Math.cos(g), h.position.y = t.y + v * Math.sin(g);
            var p = 1 - v / n,
                E = s + (c - s) * p;
            h.scale.multiplyScalar(E), l.push(h)
        }
        return l
    }

    function i() {
        var e = new Perfecto.Tween(p);
        e.from(.001, "scale").duration(l).delay(f, d).start()
    }

    var o = 294,
        a = 100,
        s = .5,
        c = 3,
        u = .1,
        l = 750,
        f = 3e3,
        d = 5;
    Perfecto.Render.call(this, e, t, "canvas");
    var h = {
            render: this.render
        },
        v = this.getScene(),
        g = this.getCamera(),
        p = n(),
        E = new Perfecto.Parallax(p, u * a / this.getBiggerSide());
    i(), this.render = function() {
        E.update(), h.render()
    }
}, Perfecto.GridRender = function(e, t, n) {
    function r() {
        T = !0, C = !1, E.domElement.addClass("active")
    }

    function i() {
        T = !1, C = !0, E.domElement.removeClass("active")
    }

    function o(e) {
        for (var t = [], r = 1; r <= e; r++)
            for (var i = 1; i <= e; i++) {
                var o = Perfecto.createElements([
                        {
                            tag: "div",
                            class: "scene-item",
                            atts: {
                                id: "item-" + r + "-" + i
                            }
                        }, {
                            tag: "img",
                            atts: {
                                src: n
                            },
                            alias: "imageNode"
                        }
                    ]),
                    a = new THREE.CSS3DObject(o);
                g.add(a), t.push(a)
            }
        return t
    }

    function a() {
        for (var e = v.getLessSide(),
            t = (e - (u - 1) * f) * l,
            n = Math.floor(t / u),
            r = n + "px",
            i = n * u + "px",
            o = 0,
            a = 0;
            a < u;
            a++)
            for (var s = 0; s < u; s++, o++) {
                var c = y[o].element,
                    h = c.imageNode;
                c.style.width = r, c.style.height = r, h.style.width = i, h.style.height = i, h.style.top =
                    -a * n + "px", h.style.left = -s * n + "px"
            }
        if (!C) {
            var g = (1 - l) / 2 * e,
                p = -e / 2 + n / 2 + g,
                E = e / 2 - n / 2 - g,
                m = n + f,
                P = n + f,
                w = e - t,
                T = w / 2 - (w - d) / 2;
            T = Math.max(0, T), o = 0;
            for (var M = E + T, x = 0; x < u; x++) {
                for (var b = p, R = 0; R < u; R++, o++) y[o].position.set(b, M, 0), b += m;
                M -= P
            }
        }
    }

    function s(e) {
        for (var t = e || 1e3,
            n = y.length,
            o = 400,
            a = Perfecto.arrayFill(n, 0),
            s = v.getWidth(),
            c = v.getHeight(),
            l = new THREE.Vector3(0, -c / 8, 0),
            f = THREE.Math.degToRad(-75),
            d = new THREE.Quaternion(Math.sin(f / 2), 0, 0, Math.cos(f / 2)),
            h = 1;
            h < u;
            h++)
            for (var g = 0; g < u; g++) a[h * u + g] = h * o;
        i(), y.forEach(function(e) {
            e.seat = e.position.clone(), e.position.set(-s / 16, c, 0)
        });
        for (var p = new Perfecto.TweenGroup(null, 0), E = new Perfecto.TweenGroup(null, 0), h = 0; h < n; h++) {
            var m = y[h],
                o = a[h];
            l.y += 5;
            var T = THREE.Math.degToRad(Perfecto.random(-45, 45)),
                C = new THREE.Quaternion(0, 0, Math.sin(T / 2), Math.cos(T / 2)),
                M = d.clone().multiply(C);
            p.add(m.position).to(l.clone(), t).delay(o).easing(TWEEN.Easing.Quartic.Out), p.add(m.quaternion).to(M, t)
                .delay(o)
                .easing(TWEEN.Easing.Quartic.Out), E.add(m.position).to(m.seat, t).easing(TWEEN.Easing.Back.Out), E
                .add(m.rotation).to(new THREE.Vector3(0, 0, 0), t).easing(TWEEN.Easing.Back.Out)
        }
        E.onComplete(function() {
            P.enable(), r(), w && w.removeClass("hidden")
        }), P.disable(), p.chain(E), p.start()
    }

    function c(e) {
        if (e.preventDefault(), T) {
            for (var t = e.target; t.className.indexOf("scene-item") == -1;)
                if (t = t.parentNode, t === document.body) return;
            var n = t.getAttribute("data-url");
            n
                && v.dispatchEvent("statechange",
                    {
                        state: "loading",
                        url: n
                    })
        }
    }

    var u = 3,
        l = .75,
        f = 9,
        d = 63;
    Perfecto.Render.call(this, e, t, "css");
    var h = {
            render: this.render,
            resize: this.resize
        },
        v = this,
        g = this.getScene(),
        p = this.getCamera(),
        E = this.getRenderer(),
        m = new Perfecto.FlyControl(p, g),
        P = Perfecto.MoveListener.getInstance(),
        y = o(u),
        w = null;
    y.forEach(function(e) {
        e.element.addEventListener("click", c)
    });
    var T = !0,
        C = !1;
    this.resize = function() {
        h.resize(), a(), v.resizeCamera(), m.resize()
    }, this.resize(), e.addEventListener("statechange",
        function(e) {
            var t = e.detail.state;
            "paused" == t ? P.disable() : "active" == t && P.enable()
        }), s(), this.render = function() {
        m.update(), h.render()
    }, this.setLinks = function(e) {
        var t = y.length - 1;
        Object.keys(e).forEach(function(n) {
            var r = e[n];
            if (n = parseInt(n), !(!r || n < 0 || n > t)) {
                var i = y[n].element;
                i.hasOwnProperty("hoverNode")
                    ? i.removeChild(i.hoverNode)
                    : i.hasOwnProperty("logoNode") && i.removeChild(i.logoNode);
                var o = Perfecto.createElements([
                    {
                        tag: "div",
                        class: "item-mask"
                    }, {
                        tag: "span",
                        class: "link-text",
                        text: r.text.trim(),
                        alias: "linkText"
                    }
                ]);
                i.appendChild(o), i.hoverNode = o, i.setAttribute("data-url", r.url), i.addClass("navigation-item")
            }
        }), v.resize()
    }, this.setLogo = function(e, t, n, r, src) {
        if (t = t || "", n = n || "", r = r || "#", y.length > e) {
            var logoHeaderElement = {};

            if (src) {
                logoHeaderElement = {
                    tag: "img",
                    atts: {
                        src: src
                    },
                    class: "mss-signature",
                    alias: "imageNode"
                };
            } else {
                logoHeaderElement = {
                    tag: "div",
                    class: "logo-title",
                    text: t,
                    alias: "logoTitle"
                };
            }

            var elementsArray = [
                {
                    tag: "a",
                    class: "logo-mask hidden",
                    atts: {
                        href: r
                    }
                },
                [
                    {
                        tag: "div",
                        class: "logo-subtitle",
                        text: n,
                        alias: "logoSubtitle"
                    }
                ]
            ];

            elementsArray[1].unshift(logoHeaderElement);

            w = Perfecto.createElements(elementsArray);
            var i = y[e].element;
            i.hasOwnProperty("hoverNode") && i.removeChild(i.hoverNode), i.appendChild(w), i.logoNode = w
        }
    }
}, Perfecto.PageRender = function(e, t) {
    function n(e) {
        return e && "#" !== e
            ? T
            ? void v.dispatchEvent("statechange",
                {
                    state: "paused"
                })
            : (p.domElement.style.display = "block", T = !0, C = !1, M = !1, v.dispatchEvent("statechange",
                {
                    state: "paused"
                }), s(), v.resizeCamera(), y.pageFrame.src = e, !0)
            : void v.dispatchEvent("statechange",
                {
                    state: "active"
                })
    }

    function r() {
        M || (w.show(), M = !0), setTimeout(function() {
                y.addClass("loaded")
            },
            u)
    }

    function i() {
        C
            || (C = !0, y.removeClass("loaded"), w.hide(), setTimeout(function() {
                    p.domElement.style.display = "none", v.dispatchEvent("statechange",
                        {
                            state: "active"
                        }), T = !1
                },
                u + l))
    }

    function o() {
        var e = [];
        c();
        for (var t = 0; t < E; t++) {
            e.push([]);
            for (var n = 0; n < m; n++) {
                var r = Perfecto.createElement({
                        tag: "div",
                        class: "page-particle"
                    }),
                    i = new THREE.CSS3DObject(r);
                g.add(i), e[t].push(i)
            }
        }
        return e
    }

    function a() {
        var e = Perfecto.createElements([
                {
                    tag: "div",
                    class: "page-wrapper"
                },
                [
                    {
                        tag: "iframe",
                        class: "subpage",
                        alias: "pageFrame"
                    }, {
                        tag: "div",
                        class: "close-btn",
                        alias: "closeButton"
                    }
                ]
            ]),
            t = new THREE.CSS3DObject(e);
        return g.add(t), e
    }

    function s() {
        if (T && x) {
            var e = Perfecto.preferredWidth(v.getWidth()),
                t = Perfecto.preferredHeight(v.getHeight());
            y.style.width = e + "px", y.style.height = t + "px";
            for (var n = e / m, r = t / E, i = -e / 2 + n / 2, o = t / 2 - r / 2, a = o, s = 0; s < E; s++) {
                for (var c = i, u = 0; u < m; u++) {
                    var l = P[s][u];
                    l.position.set(c, a, -1);
                    var f = l.element.style;
                    f.width = n + "px", f.height = r + "px", c += n
                }
                a -= r
            }
            w.resize(n, r), v.resizeCamera(), x = !1
        }
    }

    function c() {
        var e = d / f,
            t = Perfecto.preferredWidth(v.getWidth()),
            n = Perfecto.preferredHeight(v.getHeight()),
            r = t / n,
            i = r / e;
        i < 1 ? E = Math.round(E / i) : (m = Math.round(m * i), m % 2 != 0 && (m += 1))
    }

    var u = 1e3,
        l = 500,
        f = 4,
        d = 4;
    Perfecto.Render.call(this, e, t, "css");
    var h = {
            render: this.render,
            resize: this.resize
        },
        v = this,
        g = this.getScene(),
        p = this.getRenderer();
    p.domElement.style.display = "none";
    var E = f,
        m = d,
        P = o(),
        y = a(),
        w = new Perfecto.ParticlesAnimation(P, u, l),
        T = !1,
        C = !0,
        M = !1,
        x = !0;
    e.addEventListener("statechange",
        function(e) {
            "loading" == e.detail.state && n(e.detail.url)
        }), y.pageFrame.addEventListener("load", r), y.closeButton.addEventListener("click", i), this.render =
        function() {
            T && h.render()
        }, this.resize = function() {
        h.resize(), x = !0, s()
    }
}, Perfecto.PolicyPage = function() {
    function e(e, t) {
        o.style.backgroundColor = "rgba(0, 0, 0, " + .75 * e + ")", a.style.top = -90 + 140 * e + "%"
    }

    function t() {
        f.disable(), o.style.display = "block"
    }

    function n() {
        f.enable(), o.style.display = "none"
    }

    for (var r = 500,
        i = new Perfecto.Tween(e),
        o = document.getElementById("policy"),
        a = null,
        s = 0,
        c = o.childNodes.length;
        s < c && (a = o.childNodes[s], a.nodeType == Node.TEXT_NODE);
        s++);
    var u = document.getElementById("show-policy-btn"),
        l = document.getElementById("hide-policy-btn"),
        f = Perfecto.MoveListener.getInstance(),
        d = !1;
    this.resize = function() {
        var e = Perfecto.preferredWidth(window.innerWidth),
            t = Perfecto.preferredHeight(window.innerHeight);
        a.style.width = e + "px", a.style.height = t + "px", a.style.marginTop = -t / 2 + "px", a.style.marginLeft =
            -e / 2 + "px"
    }, this.resize(), u.addEventListener("click",
        function(e) {
            e.preventDefault(), d
                || (i.from(0).to(1, r).easing(TWEEN.Easing.Circular.Out).onStart(t).onComplete(null).start(), d = !0)
        }), l.addEventListener("click",
        function(e) {
            e.preventDefault(), d
                && (i.from(1).to(0, r).easing(TWEEN.Easing.Circular.In).onStart(null).onComplete(n).start(), d = !1)
        })
}, Perfecto.Parallax = function(e, t) {
    function n() {
        s = !0, c = o, g = !0
    }

    function r() {
        g = !1
    }

    function i(e, t) {
        f = e, d = t
    }

    var o = .05,
        a = 1.125,
        s = !1,
        c = o,
        u = .5 * t,
        l = e.length - 1,
        f = 0,
        d = 0,
        h = window.innerWidth / 2,
        v = window.innerHeight / 2,
        g = !0;
    e.forEach(function(e) {
        e.basePosition = e.position.clone()
    }), document.addEventListener("mouseleave", r), document.addEventListener("mouseenter", n), Perfecto.MoveListener
        .getInstance().onDisable(r).onEnable(n).addMoveHandler(i), this.update = function() {
        g
            && (s ? (h += (f - h) * c, v += (d - v) * c, c *= a, c >= 1 && (s = !1)) : (h = f, v = d), e.forEach(
                function(e, n) {
                    var r = u + n / l * t,
                        i = e.basePosition.clone();
                    i.x += h * r, i.y += v * r, e.position.copy(i)
                }))
    }
}, Perfecto.ParticlesAnimation = function(e, t, n) {
    function r(e) {
        for (var t = e.length, n = e[0].length, r = Math.floor(n / 2), i = r - 1; i >= 0; i--)
            for (var o = 0; o < t; o++) u.push(e[o][i]);
        for (var i = r; i < n; i++)
            for (var o = 0; o < t; o++) l.push(e[o][i]);
        d = u.length, f = Math.max(d, l.length);
        for (var o = 0; o < t; o++)
            for (var i = 0; i < n; i++) c.push(e[o][i])
    }

    function i() {
        u.forEach(function(e) {
            e.scale.setScalar(0)
        }), l.forEach(function(e) {
            e.scale.setScalar(0)
        })
    }

    function o(e, t) {
        var n = Math.max(0, e),
            r = 60 - 150 * e;
        r = Perfecto.limit(-90, r, 60);
        var i = THREE.Math.degToRad(r),
            o = Math.cos(i) * h,
            s = Math.floor(187 + 64 * e);
        s > 255 && (s = 255), s = s.toString(16);
        var c = "#" + s + s + s,
            f = Math.cos(i);
        a(l[t], n, o, c, f), t < d && a(u[t], n, -o, c, -f)
    }

    function a(e, t, n, r, i) {
        e.scale.setScalar(t);
        var o = e.basePosition.clone();
        o.x += n, e.position.copy(o), e.element.style.backgroundColor = r, e.rotation.set(0, i, 0)
    }

    function s(e) {
        var t = e / (2 * f);
        return t
    }

    var c = [],
        u = [],
        l = [],
        f = 0,
        d = 0,
        h = 0;
    r(e), i();
    var v = new Perfecto.Tween(o, f);
    v.easing(TWEEN.Easing.Linear.None);
    var g = s(t);
    t -= (f - 1) * g, this.resize = function(e, t) {
        h = e, c.forEach(function(e) {
            e.basePosition = e.position.clone()
        })
    }, this.show = function() {
        v.from(0).to(1.01, t).delay(0, g).start()
    }, this.hide = function() {
        v.from(1.01).to(0, t).delay(n, g).start()
    }
}, Perfecto.MoveListener = function() {
    function e(e) {
        f = e.clientX - u, d = e.clientY - l, s && i && n(r, f, d)
    }

    function t(e) {
        f = u - e.touches[0].pageX, d = l - e.touches[0].pageY, s && i && (e.preventDefault(), n(r, f, d))
    }

    function n(e) {
        var t = arguments.length;
        if (1 == t)
            e.forEach(function(e) {
                e()
            });
        else {
            for (var n = [], r = 1; r < t; r++) n.push(arguments[r]);
            e.forEach(function(e) {
                e.apply(null, n)
            })
        }
    }

    var r = [],
        i = !1,
        o = [],
        a = [],
        s = !0,
        c = 0,
        u = window.innerWidth / 2,
        l = window.innerHeight / 2,
        f = 0,
        d = 0;
    this.resize = function() {
        u = window.innerWidth / 2, l = window.innerHeight / 2
    }, this.addMoveHandler = function(n) {
        return i || (document.addEventListener("mousemove", e), document.addEventListener("touchmove", t)), r.push(n),
            i = !0, this
    }, this.onEnable = function(e) {
        return o.push(e), this
    }, this.onDisable = function(e) {
        return a.push(e), this
    }, this.enable = function() {
        c -= 1, 0 == c && (s = !0, n(o), n(r, f, d)), c = Math.max(0, c)
    }, this.disable = function() {
        s = !1, c++, n(a)
    }
}, Perfecto.MoveListener.getInstance = function() {
    var e = null;
    return function() {
        return null == e && (e = new Perfecto.MoveListener), e
    }
}(), Perfecto.FlyControl = function(e, t) {
    function n(e) {
        i = e
    }

    var r = .05,
        i = 0,
        o = 0;
    Perfecto.MoveListener.getInstance().addMoveHandler(n), this.update = function() {
        0 != o && (e.position.x += (i - e.position.x / o) * o * r, e.lookAt(t.position))
    }, this.resize = function() {
        o = e.position.z / window.innerWidth
    }
};