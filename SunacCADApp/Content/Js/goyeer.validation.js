// JavaScript Document
$.fn.ValidationInput = function () {
    var rtv = true;
    $(this).find(".required").each(function () {
        var v = $(this).val();
        var error = $(this).data('error');
        error = error == undefined ? '内容不能为空' : (error.length == 0 ? '内容不能为空' : error);
        if (_isNull(v)) {
            rtv = false;
            layer.msg(error);
            $(this).focus();
            return false;
        }
    });
    return rtv;
}


var _isNull = function (val) {
    val = val == undefined ? '' : val;
    if (val.length == 0) {
        return true;
    }
    return false;
}