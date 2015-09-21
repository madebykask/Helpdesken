'use strict';

function Case(initParam) {
    initParam = initParam || {};
    this.id = initParam.id || 0;
    this.customerId = initParam.customerId;
    this.parentCaseId = initParam.parentCaseId || 0;
    // temporary solution. 
    // Better to have array of subcases and estimate them states
    this.hasAnyNotClosedChild = initParam.isAnyNotClosedChild;
}

Case.prototype.isNew = function() {
    return this.id === 0;
};

Case.prototype.isAnyNotClosedChild = function() {
    return this.hasAnyNotClosedChild;
};

Case.prototype.isChildCase = function() {
    return this.parentCaseId !== 0;
};