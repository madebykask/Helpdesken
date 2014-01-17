$(function() {
    $('#statuses_dropdown, #objects_dropdown, #owners_dropdown, #working_groups_dropdown, #administrators_dropdown, #show_dropdown').change(function() {
        $('#search_form').submit();
    });
    
    $('#search_textbox, #records_on_page_textbox').typing({
        stop: function () {
            $('#search_form').submit();
        },
        delay: 300
    });
});