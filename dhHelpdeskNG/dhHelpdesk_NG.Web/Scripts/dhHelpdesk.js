

function deletePost(href, text, returnUrl) {

    $('#dialog-confirm').dialog({
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Ta bort": function () {
                $.post(href).success(function () { location.href = "/admin/status/"; });
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    return false;
}


var selectedRow = {
    c: "selected",
    elem: null,
    addClass: function () {
        $(this.elem).addClass(this.c);
    },
    removeClass: function () {
        $(this.elem).removeClass(this.c);
    }
};