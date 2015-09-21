"use strict";

$('.clickmultimenu .dropdown-submenu').click(function (ev) {
    var isChildSubMenuVisible = $(this).children('.dropdown-menu').css('display') == 'block';
    var clickedOn = ev.target;
    var isHasSubMenu = $(clickedOn).nextAll('.dropdown-menu').length !== 0;
    var $container = $(this).parents('.clickmultimenu:first');
    if (isChildSubMenuVisible) {
        if (!isHasSubMenu) {
            //***** click on a menu item (!submenu) ******
            // do not perform any actions towards to menu, just return true
            // and bootstrap code will do itself
            return true;
        } else {
            // click on another submenu
            $(this).find('.dropdown-menu').css('display', 'none');
        }
    } else {
        $(this).parent().find('.dropdown-menu').css('display', 'none');
        $(this).children('.dropdown-menu').css('display', 'block');
    }
    return false;
});
