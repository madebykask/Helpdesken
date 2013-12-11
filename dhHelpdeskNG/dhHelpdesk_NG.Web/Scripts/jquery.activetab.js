
jQuery(function ($) {
    var activeTab = $("#activeTab");

    $('#tabs li').click(function (e) {
        e.preventDefault();
        activeTab.val($(this).attr('id'));
    })

    if (activeTab != undefined) {
        $(activeTab.val()).click();
    }

    ////FINDS THE FIRST TAB!
    //var tabindex = $('#tabs li').attr('id');
    //alert(tabindex + 'start');
    ////FINDS ALL TABS!
    //$('#tabs li').click(function () {
    //    var Tabposition = (this.id);
    //    alert(Tabposition);
    //});

    //ORIGINAL, IT WORKS!
    //$('#tabs li').click(function () {
    //    var tabposition = (this.id);
    //    alert(tabposition);
    //alert(this.id); // id of clicked li by directly accessing DOM Element property
    //alert($(this).attr('id')); // jQuery's .attr() method, same but more verbose
    //alert($(this).html()); // gets innerHTML of clicked li
    //alert($(this).text()); // gets text contents of clicked li
    //});

});