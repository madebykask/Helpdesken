function showComputerShortInfo(id) {

    if (!id) {
        return;
    }

    $.get('@Url.Action("SearchComputerShortInfo")', { computerId: id }, function (data) {
        $("#computer_short_info_dialog_container").html(data);
        $("#computer_short_info_select_div").parent().appendTo("#computer_short_info_dialog_container");
        $('#computer_short_info_select_div').dialog('open');
    });
};