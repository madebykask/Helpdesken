function applyIndexViewBehavior() {
    $('#save_button').click(function () {
        $('#settings_form').submit();
    });

    toggleMenu(1);
}

function toggleMenu(number) {
    $('#nav > li').hide();
    var menuId = '#menu' + number;
    $(menuId).show();
}