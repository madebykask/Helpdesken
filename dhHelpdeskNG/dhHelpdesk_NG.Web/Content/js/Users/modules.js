
if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

    dhHelpdesk.UserModules = {

        _columns: [],
        _items: [],

        GetItems: function () {
            return this._items;
        },

        AddColumn: function (column) {
            this._columns.push(column);
            column.Number = this._columns.length;
        },

        AddItem: function (item) {
            this._items.push(item);
        },

        SortItems: function () {
            this._items = dhHelpdesk.UserModules.SortItemsByStartPosition(this._items);
        },

        Column: function () {
            this.Number = 0;
            this.Container = null;

            this._items = [];

            this.AddItem = function (item) {
                this._items.push(item);
                item.Number = this._items.length;
                item.Column = this;
                this._recalcNumbers();
            }

            this.RemoveItem = function (item) {
                this._items.pop(item);
                item.Number = null;
                item.Column = null;
                this._recalcNumbers();
            },

                this.MoveItemsToColumn = function () {
                    for (var i = 0; i < this._items.length; i++) {
                        var item = this._items[i];
                        this.Container.append(item.Container);
                    }
                },

                this._recalcNumbers = function () {
                    var children = this.Container.find('[data-usermodules="item"]');
                    for (var i = 0; i < children.length; i++) {
                        var child = $(children[i]);
                        var item = this._getItemByModuleId(child.attr("data-usermodules-moduleid"));
                        if (item != null)
                            item.Number = i + 1;
                    }
                },

                this._getItemByModuleId = function (moduleId) {
                    for (var i = 0; i < this._items.length; i++) {
                        var item = this._items[i];
                        if (item.ModuleId == moduleId)
                            return item;
                    }
                    return null;
                }
        },

        ColumnItem: function () {
            this.UserId = null;
            this.ModuleId = null;
            this.Column = null;
            this.Number = 0;
            this.StartPosition = 0;
            this.Container = null;

            this.GetFullNumber = function () {
                return this.Column.Number * 100 + this.Number;
            },

                this.IsStartPositionForColumn = function (column) {
                    return parseInt(this.StartPosition / 100) == column.Number;
                }
        },

        SortItemsByStartPosition: function (items) {

            function compare(item1, item2) {
                if (item1.StartPosition < item2.StartPosition)
                    return -1;
                if (item1.StartPosition > item2.StartPosition)
                    return 1;
                return 0;
            }

            return items.sort(compare);
        },

        GetItemByModuleId: function (moduleId) {
            for (var i = 0; i < this._items.length; i++) {
                var item = this._items[i];
                if (item.ModuleId == moduleId)
                    return item;
            }
            return null;
        },

        GetColumnByNumber: function (number) {
            for (var i = 0; i < this._columns.length; i++) {
                var column = this._columns[i];
                if (column.Number == number)
                    return column;
            }
            return null;
        }
    }

    var userId = parseInt($('[data-usermodules="items"]').attr("data-usermodules-userid"));

    $('[data-usermodules="item"]').each(function () {
        var $this = $(this);
        var moduleId = parseInt($this.attr("data-usermodules-moduleid"));
        if (moduleId == 0)
            return;
        var item = new dhHelpdesk.UserModules.ColumnItem();
        item.Container = $this;
        item.UserId = userId;
        item.ModuleId = moduleId;
        item.StartPosition = parseInt($this.attr("data-usermodules-position"));
        dhHelpdesk.UserModules.AddItem(item);
    });

    dhHelpdesk.UserModules.SortItems();

    $("[data-usermodules-column]").each(function () {
        var $this = $(this);
        var column = new dhHelpdesk.UserModules.Column();
        column.Number = parseInt($this.attr("data-usermodules-column"));
        column.Container = $this;

        var items = dhHelpdesk.UserModules.GetItems();

        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (item.IsStartPositionForColumn(column)) {
                column.AddItem(item);
            }
        }

        column.MoveItemsToColumn();
        dhHelpdesk.UserModules.AddColumn(column);
    });

    $(document).on("OnUserModuleItemMoved", function (event, p) {
        var item = dhHelpdesk.UserModules.GetItemByModuleId(p.item.attr("data-usermodules-moduleid"));
        if (item != null) {
            item.Column.RemoveItem(item);
            var column = dhHelpdesk.UserModules.GetColumnByNumber(p.item.parent().attr("data-usermodules-column"));
            if (column != null) {
                column.AddItem(item);
                $.post("/Home/UpdateUserModulePosition",
                    {
                        userId: item.UserId,
                        moduleId: item.ModuleId,
                        position: item.GetFullNumber()
                    });
            }
        }
    });
});