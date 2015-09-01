'use strict';

function Case(initParam) {
    initParam = initParam || {};
    this.id = initParam.id;
}

Case.prototype.isNew = function() {
    return this.id === 0;
};
