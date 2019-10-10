var mTips = {
    c: {
        x: 10,
        y: 10,
        style: { 'position': 'fixed', 'padding': '8px 12px', 'color': '#fff', 'border-radius': '5px', 'font-family': "微软雅黑", 'z-index': '999', 'display': 'inline', 'font-size': '14px', 'background-color': 'rgba(0, 0, 0, 0.5)', 'color': '#fff' }
    },

    s: function (text, a, b) {
        var style;
        var fun;
        if (typeof (a) == 'string') {
            style = a; fun = b;
        }
        else if (typeof (a) == 'function') {
            style = b; fun = a;
        }
        if (style == 'undefined' || style == null) {
            style = 'default';
        }
        var doc = $('<div></div>').addClass('mTips mTips-' + style).html(text).appendTo('body');
        if (doc.css('z-index') !== '999') {
            doc.css(this.c.style);
        }
        $(document).on('mousemove', function (e) {
            $(".mTips").offset({ top: e.pageY + mTips.c.x, left: e.pageX + mTips.c.y })
        });
        if (fun != null && typeof (fun) != 'undefined') {
            fun();
        }
    },

    h: function (fun) {
        $('.mTips').remove();
        if (fun != 'undefined' && fun != null) {
            fun();
        }
    },

    m: function () {
        $(document).on('mouseenter', '[data-mtpis]', function (e) {
            mTips.s($(this).attr('data-mtpis'), $(this).attr('data-mtpis-style'));
        });
        $(document).on('mouseleave', '[data-mtpis]', function (e) {
            mTips.h();
        });
    }
}
mTips.m();