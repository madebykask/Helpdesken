
if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

dhHelpdesk.UserModules = {

    _columns: [],

    AddColumn: function(column) {
        this._columns.push(column);
        column.Number = this._columns.length;
    },

    Column : function() {
        this.Number = 0;
        this.Container = null;

        this._items = [];

        this.AddItem = function(item) {
            this._items.push(item);
            item.Number = this._items.length;
            item.Column = this;
        }

        this.RemoveItem = function(item) {
            this._items.pop(item);
            item.Number = null;
            item.Column = null;
        },

        this.MoveItemsToColumn = function() {
            for (var i = 0; i < this._items.length; i++) {
                var item = this._items[i];
                this.Container.append(item.Container);
            }
        }
    },
    
    ColumnItem : function() {
        this.Column = null;
        this.Number = 0;
        this.StartPosition = 0;
        this.Container = null;

        this.GetFullNumber = function() {
            return this.Column.Number * 100 + this.Number;
        },

        this.IsStartPositionForColumn = function(column) {
            return parseInt(this.StartPosition / 100) == column.Number;
        }
    },

    SortItemsByStartPosition: function(items) {

        function compare(item1, item2) {
            if (item1.StartPosition < item2.StartPosition)
                return -1;
            if (item1.StartPosition > item2.StartPosition)
                return 1;
            return 0;
        }

        return items.sort(compare);
    }
}


    var items = [];

    $('[data-usermodules="item"]').each(function () {
        var $this = $(this);
        var item = new dhHelpdesk.UserModules.ColumnItem();
        item.Container = $this;
        item.StartPosition = parseInt($this.attr("data-usermodules-position"));
        items.push(item);
    });

    items = dhHelpdesk.UserModules.SortItemsByStartPosition(items);

    $("[data-usermodules-column]").each(function () {
        var $this = $(this);
        var column = new dhHelpdesk.UserModules.Column();
        column.Number = parseInt($this.attr("data-usermodules-column"));
        column.Container = $this;

        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (item.IsStartPositionForColumn(column)) {
                column.AddItem(item);
            }
        }

        column.MoveItemsToColumn();

        dhHelpdesk.UserModules.AddColumn(column);
    });

    $(document).on("OnUserModuleItemMoved", function(event, p) {
        
    });
});