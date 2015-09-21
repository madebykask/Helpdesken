/**
* Small snippet of code helps to switch tab by hashtag in URL
*   hashtag must be the same as you have in tab`s "href" attribute
*/
$(document).ready(function() {
    'use strict';
    var activeTab = window.location.hash;
    var $tabContainer = $('.nav.nav-tabs');
    var tabSearchTpl = 'a[href=' + activeTab + ']';
    $tabContainer.find(tabSearchTpl).tab('show');
});
