'use strict';

 function ShowToastMessage(message, msgType, isSticky) {
    var _Sticky = false;
    if (isSticky)
        _Sticky = true;
    $().toastmessage('showToast', {
        text: message,
        sticky: _Sticky,
        position: 'top-center',
        type: msgType,
        closeText: '',
        stayTime: 3000,
        inEffectDuration: 1000,
        close: function () {
            //console.log("toast is closed ...");
        }
    });
}
