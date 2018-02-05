"use strict";

function inventoryLogic() { };

var _numOfRowsLimit = 500;
var _rowNumsParamName = "numOfRows";
var _exportToExcelUrl = "exportToExcelUrl";


inventoryLogic.prototype.init = function (_params) {
    var self = this;
    self.params = _params;
};

inventoryLogic.prototype.setNumOfRows = function (nums) {
    var self = this;
    self.params[_rowNumsParamName] = nums;    
};

inventoryLogic.prototype.exportToExcel = function (_inventoryTypeId) {
    var self = this;
    if (self.params[_rowNumsParamName] >= _numOfRowsLimit.toString()) {
        var _$excelExportDialog = $("#exportToExcelDialog");
        _$excelExportDialog.modal('show');
    }
    else {
        window.location.href = self.params[_exportToExcelUrl] + "?inventoryTypeId=" + _inventoryTypeId;
    }    
};
