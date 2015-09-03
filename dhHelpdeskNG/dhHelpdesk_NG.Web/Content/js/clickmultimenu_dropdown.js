"use strict";

$('.clickmultimenu .dropdown-submenu').click(function () {
    var isChildSubMenuVisible = $(this).children('.dropdown-menu').css('display') == 'block';
    var submenus = $(this).find('.dropdown-submenu');
    debugger
    if (isChildSubMenuVisible) {
        $(this).find('.dropdown-menu').css('display', 'none');

    } else {
        $(this).parent().find('.dropdown-menu').css('display', 'none');
        $(this).children('.dropdown-menu').css('display', 'block');
    }

    return false;
});
