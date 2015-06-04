"use strict";
/**
* fix for expanding submenu inside submenu when having loong names 
* http://redmine.fastdev.se/issues/12662
*/
$(document).ready(function() {
    $('.dropdown-menu .dropdown-submenu').on('mouseenter', function () {
        var $submenu = $(this).find('.dropdown-menu').first();
        if ($submenu.length > 0) {
            if ($submenu.offset().left + $submenu.width() > $(window).width()) {
                $submenu.css('left', '-98%');
            }
        }
    });
});