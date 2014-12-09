function showComputers(container, computersUrl) {
    var productId = $(container).attr("data-productId");

    var url = computersUrl + "?productId=" + productId;

    window.open(url, '_blank', 'toolbar=0,location=0,menubar=0,width=500,height=350');

    return false;
}