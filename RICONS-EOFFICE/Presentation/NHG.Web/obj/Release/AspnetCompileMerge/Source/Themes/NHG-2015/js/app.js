(jQuery); +function ($) {
    'use strict'; var backdrop = '.dropdown-backdrop'
    var toggle = '[data-toggle="dropdown"]'
    var Dropdown = function (element) { $(element).on('click.bs.dropdown', this.toggle) }
    Dropdown.VERSION = '3.2.0'
    Dropdown.prototype.toggle = function (e) {
        var $this = $(this)
        if ($this.is('.disabled, :disabled')) return
        var $parent = getParent($this)
        var isActive = $parent.hasClass('open')
        clearMenus()
        if (!isActive) {
            if ('ontouchstart' in document.documentElement && !$parent.closest('.navbar-nav').length) { $('<div class="dropdown-backdrop"/>').insertAfter($(this)).on('click', clearMenus) }
            var relatedTarget = { relatedTarget: this }
            $parent.trigger(e = $.Event('show.bs.dropdown', relatedTarget))
            if (e.isDefaultPrevented()) return
            $this.trigger('focus')
            $parent
            .toggleClass('open')
            .trigger('shown.bs.dropdown', relatedTarget)
        }
        return false
    }
    Dropdown.prototype.keydown = function (e) {
        if (!/(38|40|27)/.test(e.keyCode)) return
        var $this = $(this)
        e.preventDefault()
        e.stopPropagation()
        if ($this.is('.disabled, :disabled')) return
        var $parent = getParent($this)
        var isActive = $parent.hasClass('open')
        if (!isActive || (isActive && e.keyCode == 27)) {
            if (e.which == 27) $parent.find(toggle).trigger('focus')
            return $this.trigger('click')
        }
        var desc = ' li:not(.divider):visible a'
        var $items = $parent.find('[role="menu"]' + desc + ', [role="listbox"]' + desc)
        if (!$items.length) return
        var index = $items.index($items.filter(':focus'))
        if (e.keyCode == 38 && index > 0) index--
        if (e.keyCode == 40 && index < $items.length - 1) index++
        if (!~index) index = 0
        $items.eq(index).trigger('focus')
    }
    function clearMenus(e) {
        if (e && e.which === 3) return
        $(backdrop).remove()
        $(toggle).each(function () {
            var $parent = getParent($(this))
            var relatedTarget = { relatedTarget: this }
            if (!$parent.hasClass('open')) return
            $parent.trigger(e = $.Event('hide.bs.dropdown', relatedTarget))
            if (e.isDefaultPrevented()) return
            $parent.removeClass('open').trigger('hidden.bs.dropdown', relatedTarget)
        })
    }
    function getParent($this) {
        var selector = $this.attr('data-target')
        if (!selector) {
            selector = $this.attr('href')
            selector = selector && /#[A-Za-z]/.test(selector) && selector.replace(/.*(?=#[^\s]*$)/, '')
        }
        var $parent = selector && $(selector)
        return $parent && $parent.length ? $parent : $this.parent()
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.dropdown')
            if (!data) $this.data('bs.dropdown', (data = new Dropdown(this)))
            if (typeof option == 'string') data[option].call($this)
        })
    }
    var old = $.fn.dropdown
    $.fn.dropdown = Plugin
    $.fn.dropdown.Constructor = Dropdown
    $.fn.dropdown.noConflict = function () {
        $.fn.dropdown = old
        return this
    }
    $(document)
    .on('click.bs.dropdown.data-api', clearMenus)
    .on('click.bs.dropdown.data-api', '.dropdown form', function (e) { e.stopPropagation() })
    .on('click.bs.dropdown.data-api', toggle, Dropdown.prototype.toggle)
    .on('keydown.bs.dropdown.data-api', toggle + ', [role="menu"], [role="listbox"]', Dropdown.prototype.keydown)
}(jQuery); +function ($) {
    'use strict'; var Tooltip = function (element, options) {
        this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null
        this.init('tooltip', element, options)
    }
    Tooltip.VERSION = '3.2.0'
    Tooltip.DEFAULTS = { animation: true, placement: 'top', selector: false, template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>', trigger: 'hover focus', title: '', delay: 0, html: false, container: false, viewport: { selector: 'body', padding: 0 } }
    Tooltip.prototype.init = function (type, element, options) {
        this.enabled = true
        this.type = type
        this.$element = $(element)
        this.options = this.getOptions(options)
        this.$viewport = this.options.viewport && $(this.options.viewport.selector || this.options.viewport)
        var triggers = this.options.trigger.split(' ')
        for (var i = triggers.length; i--;) {
            var trigger = triggers[i]
            if (trigger == 'click') { this.$element.on('click.' + this.type, this.options.selector, $.proxy(this.toggle, this)) } else if (trigger != 'manual') {
                var eventIn = trigger == 'hover' ? 'mouseenter' : 'focusin'
                var eventOut = trigger == 'hover' ? 'mouseleave' : 'focusout'
                this.$element.on(eventIn + '.' + this.type, this.options.selector, $.proxy(this.enter, this))
                this.$element.on(eventOut + '.' + this.type, this.options.selector, $.proxy(this.leave, this))
            }
        }
        this.options.selector ? (this._options = $.extend({}, this.options, { trigger: 'manual', selector: '' })) : this.fixTitle()
    }
    Tooltip.prototype.getDefaults = function () { return Tooltip.DEFAULTS }
    Tooltip.prototype.getOptions = function (options) {
        options = $.extend({}, this.getDefaults(), this.$element.data(), options)
        if (options.delay && typeof options.delay == 'number') { options.delay = { show: options.delay, hide: options.delay } }
        return options
    }
    Tooltip.prototype.getDelegateOptions = function () {
        var options = {}
        var defaults = this.getDefaults()
        this._options && $.each(this._options, function (key, value) { if (defaults[key] != value) options[key] = value })
        return options
    }
    Tooltip.prototype.enter = function (obj) {
        var self = obj instanceof this.constructor ? obj : $(obj.currentTarget).data('bs.' + this.type)
        if (!self) {
            self = new this.constructor(obj.currentTarget, this.getDelegateOptions())
            $(obj.currentTarget).data('bs.' + this.type, self)
        }
        clearTimeout(self.timeout)
        self.hoverState = 'in'
        if (!self.options.delay || !self.options.delay.show) return self.show()
        self.timeout = setTimeout(function () { if (self.hoverState == 'in') self.show() }, self.options.delay.show)
    }
    Tooltip.prototype.leave = function (obj) {
        var self = obj instanceof this.constructor ? obj : $(obj.currentTarget).data('bs.' + this.type)
        if (!self) {
            self = new this.constructor(obj.currentTarget, this.getDelegateOptions())
            $(obj.currentTarget).data('bs.' + this.type, self)
        }
        clearTimeout(self.timeout)
        self.hoverState = 'out'
        if (!self.options.delay || !self.options.delay.hide) return self.hide()
        self.timeout = setTimeout(function () { if (self.hoverState == 'out') self.hide() }, self.options.delay.hide)
    }
    Tooltip.prototype.show = function () {
        var e = $.Event('show.bs.' + this.type)
        if (this.hasContent() && this.enabled) {
            this.$element.trigger(e)
            var inDom = $.contains(document.documentElement, this.$element[0])
            if (e.isDefaultPrevented() || !inDom) return
            var that = this
            var $tip = this.tip()
            var tipId = this.getUID(this.type)
            this.setContent()
            $tip.attr('id', tipId)
            this.$element.attr('aria-describedby', tipId)
            if (this.options.animation) $tip.addClass('fade')
            var placement = typeof this.options.placement == 'function' ? this.options.placement.call(this, $tip[0], this.$element[0]) : this.options.placement
            var autoToken = /\s?auto?\s?/i
            var autoPlace = autoToken.test(placement)
            if (autoPlace) placement = placement.replace(autoToken, '') || 'top'
            $tip
            .detach()
            .css({ top: 0, left: 0, display: 'block' })
            .addClass(placement)
            .data('bs.' + this.type, this)
            this.options.container ? $tip.appendTo(this.options.container) : $tip.insertAfter(this.$element)
            var pos = this.getPosition()
            var actualWidth = $tip[0].offsetWidth
            var actualHeight = $tip[0].offsetHeight
            if (autoPlace) {
                var orgPlacement = placement
                var $parent = this.$element.parent()
                var parentDim = this.getPosition($parent)
                placement = placement == 'bottom' && pos.top + pos.height + actualHeight - parentDim.scroll > parentDim.height ? 'top' : placement == 'top' && pos.top - parentDim.scroll - actualHeight < 0 ? 'bottom' : placement == 'right' && pos.right + actualWidth > parentDim.width ? 'left' : placement == 'left' && pos.left - actualWidth < parentDim.left ? 'right' : placement
                $tip
                .removeClass(orgPlacement)
                .addClass(placement)
            }
            var calculatedOffset = this.getCalculatedOffset(placement, pos, actualWidth, actualHeight)
            this.applyPlacement(calculatedOffset, placement)
            var complete = function () {
                that.$element.trigger('shown.bs.' + that.type)
                that.hoverState = null
            }
            $.support.transition && this.$tip.hasClass('fade') ? $tip
            .one('bsTransitionEnd', complete)
            .emulateTransitionEnd(150) : complete()
        }
    }
    Tooltip.prototype.applyPlacement = function (offset, placement) {
        var $tip = this.tip()
        var width = $tip[0].offsetWidth
        var height = $tip[0].offsetHeight
        var marginTop = parseInt($tip.css('margin-top'), 10)
        var marginLeft = parseInt($tip.css('margin-left'), 10)
        if (isNaN(marginTop)) marginTop = 0
        if (isNaN(marginLeft)) marginLeft = 0
        offset.top = offset.top + marginTop
        offset.left = offset.left + marginLeft
        $.offset.setOffset($tip[0], $.extend({ using: function (props) { $tip.css({ top: Math.round(props.top), left: Math.round(props.left) }) } }, offset), 0)
        $tip.addClass('in')
        var actualWidth = $tip[0].offsetWidth
        var actualHeight = $tip[0].offsetHeight
        if (placement == 'top' && actualHeight != height) { offset.top = offset.top + height - actualHeight }
        var delta = this.getViewportAdjustedDelta(placement, offset, actualWidth, actualHeight)
        if (delta.left) offset.left += delta.left
        else offset.top += delta.top
        var arrowDelta = delta.left ? delta.left * 2 - width + actualWidth : delta.top * 2 - height + actualHeight
        var arrowPosition = delta.left ? 'left' : 'top'
        var arrowOffsetPosition = delta.left ? 'offsetWidth' : 'offsetHeight'
        $tip.offset(offset)
        this.replaceArrow(arrowDelta, $tip[0][arrowOffsetPosition], arrowPosition)
    }
    Tooltip.prototype.replaceArrow = function (delta, dimension, position) { this.arrow().css(position, delta ? (50 * (1 - delta / dimension) + '%') : '') }
    Tooltip.prototype.setContent = function () {
        var $tip = this.tip()
        var title = this.getTitle()
        $tip.find('.tooltip-inner')[this.options.html ? 'html' : 'text'](title)
        $tip.removeClass('fade in top bottom left right')
    }
    Tooltip.prototype.hide = function () {
        var that = this
        var $tip = this.tip()
        var e = $.Event('hide.bs.' + this.type)
        this.$element.removeAttr('aria-describedby')
        function complete() {
            if (that.hoverState != 'in') $tip.detach()
            that.$element.trigger('hidden.bs.' + that.type)
        }
        this.$element.trigger(e)
        if (e.isDefaultPrevented()) return
        $tip.removeClass('in')
        $.support.transition && this.$tip.hasClass('fade') ? $tip
        .one('bsTransitionEnd', complete)
        .emulateTransitionEnd(150) : complete()
        this.hoverState = null
        return this
    }
    Tooltip.prototype.fixTitle = function () {
        var $e = this.$element
        if ($e.attr('title') || typeof ($e.attr('data-original-title')) != 'string') { $e.attr('data-original-title', $e.attr('title') || '').attr('title', '') }
    }
    Tooltip.prototype.hasContent = function () { return this.getTitle() }
    Tooltip.prototype.getPosition = function ($element) {
        $element = $element || this.$element
        var el = $element[0]
        var isBody = el.tagName == 'BODY'
        return $.extend({}, (typeof el.getBoundingClientRect == 'function') ? el.getBoundingClientRect() : null, { scroll: isBody ? document.documentElement.scrollTop || document.body.scrollTop : $element.scrollTop(), width: isBody ? $(window).width() : $element.outerWidth(), height: isBody ? $(window).height() : $element.outerHeight() }, isBody ? { top: 0, left: 0 } : $element.offset())
    }
    Tooltip.prototype.getCalculatedOffset = function (placement, pos, actualWidth, actualHeight) { return placement == 'bottom' ? { top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 } : placement == 'top' ? { top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2 } : placement == 'left' ? { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth } : { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width } }
    Tooltip.prototype.getViewportAdjustedDelta = function (placement, pos, actualWidth, actualHeight) {
        var delta = { top: 0, left: 0 }
        if (!this.$viewport) return delta
        var viewportPadding = this.options.viewport && this.options.viewport.padding || 0
        var viewportDimensions = this.getPosition(this.$viewport)
        if (/right|left/.test(placement)) {
            var topEdgeOffset = pos.top - viewportPadding - viewportDimensions.scroll
            var bottomEdgeOffset = pos.top + viewportPadding - viewportDimensions.scroll + actualHeight
            if (topEdgeOffset < viewportDimensions.top) { delta.top = viewportDimensions.top - topEdgeOffset } else if (bottomEdgeOffset > viewportDimensions.top + viewportDimensions.height) { delta.top = viewportDimensions.top + viewportDimensions.height - bottomEdgeOffset }
        } else {
            var leftEdgeOffset = pos.left - viewportPadding
            var rightEdgeOffset = pos.left + viewportPadding + actualWidth
            if (leftEdgeOffset < viewportDimensions.left) { delta.left = viewportDimensions.left - leftEdgeOffset } else if (rightEdgeOffset > viewportDimensions.width) { delta.left = viewportDimensions.left + viewportDimensions.width - rightEdgeOffset }
        }
        return delta
    }
    Tooltip.prototype.getTitle = function () {
        var title
        var $e = this.$element
        var o = this.options
        title = $e.attr('data-original-title') || (typeof o.title == 'function' ? o.title.call($e[0]) : o.title)
        return title
    }
    Tooltip.prototype.getUID = function (prefix) {
        do prefix += ~~(Math.random() * 1000000)
        while (document.getElementById(prefix))
        return prefix
    }
    Tooltip.prototype.tip = function () { return (this.$tip = this.$tip || $(this.options.template)) }
    Tooltip.prototype.arrow = function () { return (this.$arrow = this.$arrow || this.tip().find('.tooltip-arrow')) }
    Tooltip.prototype.validate = function () {
        if (!this.$element[0].parentNode) {
            this.hide()
            this.$element = null
            this.options = null
        }
    }
    Tooltip.prototype.enable = function () { this.enabled = true }
    Tooltip.prototype.disable = function () { this.enabled = false }
    Tooltip.prototype.toggleEnabled = function () { this.enabled = !this.enabled }
    Tooltip.prototype.toggle = function (e) {
        var self = this
        if (e) {
            self = $(e.currentTarget).data('bs.' + this.type)
            if (!self) {
                self = new this.constructor(e.currentTarget, this.getDelegateOptions())
                $(e.currentTarget).data('bs.' + this.type, self)
            }
        }
        self.tip().hasClass('in') ? self.leave(self) : self.enter(self)
    }
    Tooltip.prototype.destroy = function () {
        clearTimeout(this.timeout)
        this.hide().$element.off('.' + this.type).removeData('bs.' + this.type)
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.tooltip')
            var options = typeof option == 'object' && option
            if (!data && option == 'destroy') return
            if (!data) $this.data('bs.tooltip', (data = new Tooltip(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.tooltip
    $.fn.tooltip = Plugin
    $.fn.tooltip.Constructor = Tooltip
    $.fn.tooltip.noConflict = function () {
        $.fn.tooltip = old
        return this
    }
}(jQuery); +function ($) {
    'use strict'; var Popover = function (element, options) { this.init('popover', element, options) }
    if (!$.fn.tooltip) throw new Error('Popover requires tooltip.js')
    Popover.VERSION = '3.2.0'
    Popover.DEFAULTS = $.extend({}, $.fn.tooltip.Constructor.DEFAULTS, { placement: 'right', trigger: 'click', content: '', template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>' })
    Popover.prototype = $.extend({}, $.fn.tooltip.Constructor.prototype)
    Popover.prototype.constructor = Popover
    Popover.prototype.getDefaults = function () { return Popover.DEFAULTS }
    Popover.prototype.setContent = function () {
        var $tip = this.tip()
        var title = this.getTitle()
        var content = this.getContent()
        $tip.find('.popover-title')[this.options.html ? 'html' : 'text'](title)
        $tip.find('.popover-content').empty()[this.options.html ? (typeof content == 'string' ? 'html' : 'append') : 'text'](content)
        $tip.removeClass('fade top bottom left right in')
        if (!$tip.find('.popover-title').html()) $tip.find('.popover-title').hide()
    }
    Popover.prototype.hasContent = function () { return this.getTitle() || this.getContent() }
    Popover.prototype.getContent = function () {
        var $e = this.$element
        var o = this.options
        return $e.attr('data-content') || (typeof o.content == 'function' ? o.content.call($e[0]) : o.content)
    }
    Popover.prototype.arrow = function () { return (this.$arrow = this.$arrow || this.tip().find('.arrow')) }
    Popover.prototype.tip = function () {
        if (!this.$tip) this.$tip = $(this.options.template)
        return this.$tip
    }
    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('bs.popover')
            var options = typeof option == 'object' && option
            if (!data && option == 'destroy') return
            if (!data) $this.data('bs.popover', (data = new Popover(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }
    var old = $.fn.popover
    $.fn.popover = Plugin
    $.fn.popover.Constructor = Popover
    $.fn.popover.noConflict = function () {
        $.fn.popover = old
        return this
    }
}(jQuery); (function (h, j, e) { var a = "placeholder" in j.createElement("input"); var f = "placeholder" in j.createElement("textarea"); var k = e.fn; var d = e.valHooks; var b = e.propHooks; var m; var l; if (a && f) { l = k.placeholder = function () { return this }; l.input = l.textarea = true } else { l = k.placeholder = function () { var n = this; n.filter((a ? "textarea" : ":input") + "[placeholder]").not(".placeholder").bind({ "focus.placeholder": c, "blur.placeholder": g }).data("placeholder-enabled", true).trigger("blur.placeholder"); return n }; l.input = a; l.textarea = f; m = { get: function (o) { var n = e(o); var p = n.data("placeholder-password"); if (p) { return p[0].value } return n.data("placeholder-enabled") && n.hasClass("placeholder") ? "" : o.value }, set: function (o, q) { var n = e(o); var p = n.data("placeholder-password"); if (p) { return p[0].value = q } if (!n.data("placeholder-enabled")) { return o.value = q } if (q == "") { o.value = q; if (o != j.activeElement) { g.call(o) } } else { if (n.hasClass("placeholder")) { c.call(o, true, q) || (o.value = q) } else { o.value = q } } return n } }; if (!a) { d.input = m; b.value = m } if (!f) { d.textarea = m; b.value = m } e(function () { e(j).delegate("form", "submit.placeholder", function () { var n = e(".placeholder", this).each(c); setTimeout(function () { n.each(g) }, 10) }) }); e(h).bind("beforeunload.placeholder", function () { e(".placeholder").each(function () { this.value = "" }) }) } function i(o) { var n = {}; var p = /^jQuery\d+$/; e.each(o.attributes, function (r, q) { if (q.specified && !p.test(q.name)) { n[q.name] = q.value } }); return n } function c(o, p) { var n = this; var q = e(n); if (n.value == q.attr("placeholder") && q.hasClass("placeholder")) { if (q.data("placeholder-password")) { q = q.hide().next().show().attr("id", q.removeAttr("id").data("placeholder-id")); if (o === true) { return q[0].value = p } q.focus() } else { n.value = ""; q.removeClass("placeholder"); n == j.activeElement && n.select() } } } function g() { var r; var n = this; var q = e(n); var p = this.id; if (n.value == "") { if (n.type == "password") { if (!q.data("placeholder-textinput")) { try { r = q.clone().attr({ type: "text" }) } catch (o) { r = e("<input>").attr(e.extend(i(this), { type: "text" })) } r.removeAttr("name").data({ "placeholder-password": q, "placeholder-id": p }).bind("focus.placeholder", c); q.data({ "placeholder-textinput": r, "placeholder-id": p }).before(r) } q = q.removeAttr("id").hide().prev().attr("id", p).show() } q.addClass("placeholder"); q[0].value = q.attr("placeholder") } else { q.removeClass("placeholder") } } }(this, document, jQuery));; window.Modernizr = function (a, b, c) { function w(a) { j.cssText = a } function x(a, b) { return w(m.join(a + ";") + (b || "")) } function y(a, b) { return typeof a === b } function z(a, b) { return !!~("" + a).indexOf(b) } function A(a, b, d) { for (var e in a) { var f = b[a[e]]; if (f !== c) return d === !1 ? a[e] : y(f, "function") ? f.bind(d || b) : f } return !1 } var d = "2.6.2", e = {}, f = !0, g = b.documentElement, h = "modernizr", i = b.createElement(h), j = i.style, k, l = {}.toString, m = " -webkit- -moz- -o- -ms- ".split(" "), n = {}, o = {}, p = {}, q = [], r = q.slice, s, t = function (a, c, d, e) { var f, i, j, k, l = b.createElement("div"), m = b.body, n = m || b.createElement("body"); if (parseInt(d, 10)) while (d--) j = b.createElement("div"), j.id = e ? e[d] : h + (d + 1), l.appendChild(j); return f = ["&#173;", '<style id="s', h, '">', a, "</style>"].join(""), l.id = h, (m ? l : n).innerHTML += f, n.appendChild(l), m || (n.style.background = "", n.style.overflow = "hidden", k = g.style.overflow, g.style.overflow = "hidden", g.appendChild(n)), i = c(l, a), m ? l.parentNode.removeChild(l) : (n.parentNode.removeChild(n), g.style.overflow = k), !!i }, u = {}.hasOwnProperty, v; !y(u, "undefined") && !y(u.call, "undefined") ? v = function (a, b) { return u.call(a, b) } : v = function (a, b) { return b in a && y(a.constructor.prototype[b], "undefined") }, Function.prototype.bind || (Function.prototype.bind = function (b) { var c = this; if (typeof c != "function") throw new TypeError; var d = r.call(arguments, 1), e = function () { if (this instanceof e) { var a = function () { }; a.prototype = c.prototype; var f = new a, g = c.apply(f, d.concat(r.call(arguments))); return Object(g) === g ? g : f } return c.apply(b, d.concat(r.call(arguments))) }; return e }), n.touch = function () { var c; return "ontouchstart" in a || a.DocumentTouch && b instanceof DocumentTouch ? c = !0 : t(["@media (", m.join("touch-enabled),("), h, ")", "{#modernizr{top:9px;position:absolute}}"].join(""), function (a) { c = a.offsetTop === 9 }), c }; for (var B in n) v(n, B) && (s = B.toLowerCase(), e[s] = n[B](), q.push((e[s] ? "" : "no-") + s)); return e.addTest = function (a, b) { if (typeof a == "object") for (var d in a) v(a, d) && e.addTest(d, a[d]); else { a = a.toLowerCase(); if (e[a] !== c) return e; b = typeof b == "function" ? b() : b, typeof f != "undefined" && f && (g.className += " " + (b ? "" : "no-") + a), e[a] = b } return e }, w(""), i = k = null, e._version = d, e._prefixes = m, e.testStyles = t, g.className = g.className.replace(/(^|\s)no-js(\s|$)/, "$1$2") + (f ? " js " + q.join(" ") : ""), e }(this, this.document); Modernizr.addTest('android', function () { return !!navigator.userAgent.match(/Android/i) }); Modernizr.addTest('chrome', function () { return !!navigator.userAgent.match(/Chrome/i) }); Modernizr.addTest('firefox', function () { return !!navigator.userAgent.match(/Firefox/i) }); Modernizr.addTest('iemobile', function () { return !!navigator.userAgent.match(/IEMobile/i) }); Modernizr.addTest('ie', function () { return !!navigator.userAgent.match(/MSIE/i) }); Modernizr.addTest('ie8', function () { return !!navigator.userAgent.match(/MSIE 8/i) }); Modernizr.addTest('ie10', function () { return !!navigator.userAgent.match(/MSIE 10/i) }); Modernizr.addTest('ie11', function () { return !!navigator.userAgent.match(/Trident.*rv:11\./) }); Modernizr.addTest('ios', function () { return !!navigator.userAgent.match(/iPhone|iPad|iPod/i) }); Modernizr.addTest('ios7 ipad', function () { return !!navigator.userAgent.match(/iPad;.*CPU.*OS 7_\d/i) }); (function (a, b) { "use strict"; var c = "undefined" != typeof Element && "ALLOW_KEYBOARD_INPUT" in Element, d = function () { for (var a, c, d = [["requestFullscreen", "exitFullscreen", "fullscreenElement", "fullscreenEnabled", "fullscreenchange", "fullscreenerror"], ["webkitRequestFullscreen", "webkitExitFullscreen", "webkitFullscreenElement", "webkitFullscreenEnabled", "webkitfullscreenchange", "webkitfullscreenerror"], ["webkitRequestFullScreen", "webkitCancelFullScreen", "webkitCurrentFullScreenElement", "webkitCancelFullScreen", "webkitfullscreenchange", "webkitfullscreenerror"], ["mozRequestFullScreen", "mozCancelFullScreen", "mozFullScreenElement", "mozFullScreenEnabled", "mozfullscreenchange", "mozfullscreenerror"]], e = 0, f = d.length, g = {}; f > e; e++) if (a = d[e], a && a[1] in b) { for (e = 0, c = a.length; c > e; e++) g[d[0][e]] = a[e]; return g } return !1 }(), e = { request: function (a) { var e = d.requestFullscreen; a = a || b.documentElement, /5\.1[\.\d]* Safari/.test(navigator.userAgent) ? a[e]() : a[e](c && Element.ALLOW_KEYBOARD_INPUT) }, exit: function () { b[d.exitFullscreen]() }, toggle: function (a) { this.isFullscreen ? this.exit() : this.request(a) }, onchange: function () { }, onerror: function () { }, raw: d }; return d ? (Object.defineProperties(e, { isFullscreen: { get: function () { return !!b[d.fullscreenElement] } }, element: { enumerable: !0, get: function () { return b[d.fullscreenElement] } }, enabled: { enumerable: !0, get: function () { return !!b[d.fullscreenEnabled] } } }), b.addEventListener(d.fullscreenchange, function (a) { e.onchange.call(e, a) }), b.addEventListener(d.fullscreenerror, function (a) { e.onerror.call(e, a) }), a.screenfull = e, void 0) : a.screenfull = !1 })(window, document); +function ($) {
    "use strict"; var Shift = function (element) {
        this.$element = $(element)
        this.$prev = this.$element.prev()
        !this.$prev.length && (this.$parent = this.$element.parent())
    }
    Shift.prototype = {
        constructor: Shift, init: function () {
            var $el = this.$element, method = $el.data()['toggle'].split(':')[1], $target = $el.data('target')
            $el.hasClass('in') || $el[method]($target).addClass('in')
        }, reset: function () {
            this.$parent && this.$parent['prepend'](this.$element)
            !this.$parent && this.$element['insertAfter'](this.$prev)
            this.$element.removeClass('in')
        }
    }
    $.fn.shift = function (option) {
        return this.each(function () {
            var $this = $(this), data = $this.data('shift')
            if (!data) $this.data('shift', (data = new Shift(this)))
            if (typeof option == 'string') data[option]()
        })
    }
    $.fn.shift.Constructor = Shift
}(jQuery); Date.now = Date.now || function () { return +new Date; }; +function ($) {
    $(function () {
        $(document).on('click', "[data-toggle=fullscreen]",
            function (e) {
                e.preventDefault();
                if (screenfull.enabled) {
                    screenfull.request();
                }
            });
        $('input[placeholder], textarea[placeholder]').placeholder();
        $("[data-toggle=popover]").popover();
        $(document).on('click', '.popover-title .close', function (e) {
            var $target = $(e.target), $popover = $target.closest('.popover').prev();
            $popover && $popover.popover('hide');
        }); $(document).on('click', '[data-toggle="ajaxModal"]',
            function (e) { $('#ajaxModal').remove(); e.preventDefault(); var $this = $(this), $remote = $this.data('remote') || $this.attr('href'), $modal = $('<div class="modal fade" id="ajaxModal"><div class="modal-body"></div></div>'); $('body').append($modal); $modal.modal(); $modal.load($remote); }); $.fn.dropdown.Constructor.prototype.change = function (e) { e.preventDefault(); var $item = $(e.target), $select, $checked = false, $menu, $label; !$item.is('a') && ($item = $item.closest('a')); $menu = $item.closest('.dropdown-menu'); $label = $menu.parent().find('.dropdown-label'); $labelHolder = $label.text(); $select = $item.find('input'); $checked = $select.is(':checked'); if ($select.is(':disabled')) return; if ($select.attr('type') == 'radio' && $checked) return; if ($select.attr('type') == 'radio') $menu.find('li').removeClass('active'); $item.parent().removeClass('active'); !$checked && $item.parent().addClass('active'); $select.prop("checked", !$select.prop("checked")); $items = $menu.find('li > a > input:checked'); if ($items.length) { $text = []; $items.each(function () { var $str = $(this).parent().text(); $str && $text.push($.trim($str)); }); $text = $text.length < 4 ? $text.join(', ') : $text.length + ' selected'; $label.html($text); } else { $label.html($label.data('placeholder')); } }
        $(document).on('click.dropdown-menu', '.dropdown-select > li > a', $.fn.dropdown.Constructor.prototype.change); $("[data-toggle=tooltip]").tooltip(); $(document).on('click', '[data-toggle^="class"]', function (e) {
            e && e.preventDefault(); var $this = $(e.target), $class, $target, $tmp, $classes, $targets; !$this.data('toggle') && ($this = $this.closest('[data-toggle^="class"]')); $class = $this.data()['toggle']; $target = $this.data('target') || $this.attr('href'); $class && ($tmp = $class.split(':')[1]) && ($classes = $tmp.split(',')); $target && ($targets = $target.split(',')); $classes && $classes.length && $.each($targets, function (index, value) {
                if ($classes[index].indexOf('*') !== -1) {
                    var patt = new RegExp('\\s' + $classes[index].
                    replace(/\*/g, '[A-Za-z0-9-_]+').
                    split(' ').
                    join('\\s|\\s') + '\\s', 'g'); $($this).each(function (i, it) {
                        var cn = ' ' + it.className + ' '; while (patt.test(cn)) { cn = cn.replace(patt, ' '); }
                        it.className = $.trim(cn);
                    });
                }
                ($targets[index] != '#') && $($targets[index]).toggleClass($classes[index]) || $this.toggleClass($classes[index]);
            }); $this.toggleClass('active');
        }); $(document).on('click', '.panel-toggle', function (e) { e && e.preventDefault(); var $this = $(e.target), $class = 'collapse', $target; if (!$this.is('a')) $this = $this.closest('a'); $target = $this.closest('.panel'); $target.find('.panel-body').toggleClass($class); $this.toggleClass('active'); }); $(document).on('click.button.data-api', '[data-loading-text]', function (e) { var $this = $(e.target); $this.is('i') && ($this = $this.parent()); $this.button('loading'); }); var $window = $(window); var mobile = function (option) {
            if (option == 'reset') { $('[data-toggle^="shift"]').shift('reset'); return true; }
            $('[data-toggle^="shift"]').shift('init'); return true;
        }; $window.width() < 768 && mobile(); var $resize, $width = $window.width(); $window.resize(function () { if ($width !== $window.width()) { clearTimeout($resize); $resize = setTimeout(function () { setHeight(); $window.width() < 767 && mobile(); $window.width() >= 768 && mobile('reset') && fixVbox(); $width = $window.width(); }, 500); } }); var setHeight = function () { $('.app-fluid #nav > *').css('min-height', $(window).height()); return true; }
        setHeight(); var fixVbox = function () { $('.ie11 .vbox').each(function () { $(this).height($(this).parent().height()); }); return true; }
        fixVbox(); $(document).on('click', '[data-ride="collapse"] a', function (e) { var $this = $(e.target), $active; $this.is('a') || ($this = $this.closest('a')); $active = $this.parent().siblings(".active"); $active && $active.toggleClass('active').find('> ul:visible').slideUp(200); ($this.parent().hasClass('active') && $this.next().slideUp(200)) || $this.next().slideDown(200); $this.parent().toggleClass('active'); $this.next().is('ul') && e.preventDefault(); setTimeout(function () { $(document).trigger('updateNav'); }, 300); }); $(document).on('click.bs.dropdown.data-api', '.dropdown .on, .dropup .on, .open .on', function (e) { e.stopPropagation() });
    });
}(jQuery); (function (f) { jQuery.fn.extend({ slimScroll: function (h) { var a = f.extend({ width: "auto", height: "250px", size: "7px", color: "#000", position: "right", distance: "1px", start: "top", opacity: 0.4, alwaysVisible: !1, disableFadeOut: !1, railVisible: !1, railColor: "#333", railOpacity: 0.2, railDraggable: !0, railClass: "slimScrollRail", barClass: "slimScrollBar", wrapperClass: "slimScrollDiv", allowPageScroll: !1, wheelStep: 20, touchScrollStep: 200, borderRadius: "7px", railBorderRadius: "7px" }, h); this.each(function () { function r(d) { if (s) { d = d || window.event; var c = 0; d.wheelDelta && (c = -d.wheelDelta / 120); d.detail && (c = d.detail / 3); f(d.target || d.srcTarget || d.srcElement).closest("." + a.wrapperClass).is(b.parent()) && m(c, !0); d.preventDefault && !k && d.preventDefault(); k || (d.returnValue = !1) } } function m(d, f, h) { k = !1; var e = d, g = b.outerHeight() - c.outerHeight(); f && (e = parseInt(c.css("top")) + d * parseInt(a.wheelStep) / 100 * c.outerHeight(), e = Math.min(Math.max(e, 0), g), e = 0 < d ? Math.ceil(e) : Math.floor(e), c.css({ top: e + "px" })); l = parseInt(c.css("top")) / (b.outerHeight() - c.outerHeight()); e = l * (b[0].scrollHeight - b.outerHeight()); h && (e = d, d = e / b[0].scrollHeight * b.outerHeight(), d = Math.min(Math.max(d, 0), g), c.css({ top: d + "px" })); b.scrollTop(e); b.trigger("slimscrolling", ~~e); v(); p() } function C() { window.addEventListener ? (this.addEventListener("DOMMouseScroll", r, !1), this.addEventListener("mousewheel", r, !1), this.addEventListener("MozMousePixelScroll", r, !1)) : document.attachEvent("onmousewheel", r) } function w() { u = Math.max(b.outerHeight() / b[0].scrollHeight * b.outerHeight(), D); c.css({ height: u + "px" }); var a = u == b.outerHeight() ? "none" : "block"; c.css({ display: a }) } function v() { w(); clearTimeout(A); l == ~~l ? (k = a.allowPageScroll, B != l && b.trigger("slimscroll", 0 == ~~l ? "top" : "bottom")) : k = !1; B = l; u >= b.outerHeight() ? k = !0 : (c.stop(!0, !0).fadeIn("fast"), a.railVisible && g.stop(!0, !0).fadeIn("fast")) } function p() { a.alwaysVisible || (A = setTimeout(function () { a.disableFadeOut && s || (x || y) || (c.fadeOut("slow"), g.fadeOut("slow")) }, 1E3)) } var s, x, y, A, z, u, l, B, D = 30, k = !1, b = f(this); if (b.parent().hasClass(a.wrapperClass)) { var n = b.scrollTop(), c = b.parent().find("." + a.barClass), g = b.parent().find("." + a.railClass); w(); if (f.isPlainObject(h)) { if ("height" in h && "auto" == h.height) { b.parent().css("height", "auto"); b.css("height", "auto"); var q = b.parent().parent().height(); b.parent().css("height", q); b.css("height", q) } if ("scrollTo" in h) n = parseInt(a.scrollTo); else if ("scrollBy" in h) n += parseInt(a.scrollBy); else if ("destroy" in h) { c.remove(); g.remove(); b.unwrap(); return } m(n, !1, !0) } } else { a.height = "auto" == a.height ? b.parent().height() : a.height; n = f("<div></div>").addClass(a.wrapperClass).css({ position: "relative", overflow: "hidden", width: a.width, height: a.height }); b.css({ overflow: "hidden", width: a.width, height: a.height }); var g = f("<div></div>").addClass(a.railClass).css({ width: a.size, height: "100%", position: "absolute", top: 0, display: a.alwaysVisible && a.railVisible ? "block" : "none", "border-radius": a.railBorderRadius, background: a.railColor, opacity: a.railOpacity, zIndex: 90 }), c = f("<div></div>").addClass(a.barClass).css({ background: a.color, width: a.size, position: "absolute", top: 0, opacity: a.opacity, display: a.alwaysVisible ? "block" : "none", "border-radius": a.borderRadius, BorderRadius: a.borderRadius, MozBorderRadius: a.borderRadius, WebkitBorderRadius: a.borderRadius, zIndex: 99 }), q = "right" == a.position ? { right: a.distance } : { left: a.distance }; g.css(q); c.css(q); b.wrap(n); b.parent().append(c); b.parent().append(g); a.railDraggable && c.bind("mousedown", function (a) { var b = f(document); y = !0; t = parseFloat(c.css("top")); pageY = a.pageY; b.bind("mousemove.slimscroll", function (a) { currTop = t + a.pageY - pageY; c.css("top", currTop); m(0, c.position().top, !1) }); b.bind("mouseup.slimscroll", function (a) { y = !1; p(); b.unbind(".slimscroll") }); return !1 }).bind("selectstart.slimscroll", function (a) { a.stopPropagation(); a.preventDefault(); return !1 }); g.hover(function () { v() }, function () { p() }); c.hover(function () { x = !0 }, function () { x = !1 }); b.hover(function () { s = !0; v(); p() }, function () { s = !1; p() }); b.bind("touchstart", function (a, b) { a.originalEvent.touches.length && (z = a.originalEvent.touches[0].pageY) }); b.bind("touchmove", function (b) { k || b.originalEvent.preventDefault(); b.originalEvent.touches.length && (m((z - b.originalEvent.touches[0].pageY) / a.touchScrollStep, !0), z = b.originalEvent.touches[0].pageY) }); w(); "bottom" === a.start ? (c.css({ top: b.outerHeight() - c.outerHeight() }), m(0, !0)) : "top" !== a.start && (m(f(a.start).position().top, null, !0), a.alwaysVisible || c.hide()); C() } }); return this } }); jQuery.fn.extend({ slimscroll: jQuery.fn.slimScroll }) })(jQuery);