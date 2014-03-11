function SortSettings() {
    this._settings = [];
}

SortSettings.prototype.addSetting = function (setting) {
    if (Enumerable.From(this._settings).Any(function(s) { return s.fieldName == setting.fieldName; })) {
        throw new Error('Setting already exists.');
    }

    this._settings.push(setting);
};

SortSettings.prototype.findSetting = function(fieldName) {
    return Enumerable.From(this._settings).SingleOrDefault(null, function(s) { return s.fieldName == fieldName; });
};

SortSettings.prototype.revertSetting = function(fieldName) {
    var setting = Enumerable.From(this._settings).Single(function(s) { return s.fieldName == fieldName; });
    setting.sortBy = setting.sortBy == sortBy.ASCENDING ? sortBy.DESCENDING : sortBy.ASCENDING;
};