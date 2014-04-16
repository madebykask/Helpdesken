if (window.dhHelpdesk == null)
    dhHelpdesk = {};

$(function () {

    dhHelpdesk.HierarchyList = {
        
        _lists: [],

        GetLists: function() {
            return this._lists;
        },

        AddList: function(list) {
            this._lists.push(list);
        },

        List: function() {
            this._groups = [];

            this.SelectedValue = null; 
            this.GetItemsUrl = null;
            this.GetChildrenItemsUrl = null;
            this.Container = null;

            this.CreateContainer = function(element) {
                this.Container = element;
                var groups = this.GetGroups();
                for (var i = 0; i < groups.length; i++) {
                    var group = groups[i];
                    if (group.Container != null) {
                        this.Container.after(group.Container);
                    }
                }
            };

            this.GetGroups = function() {
                return this._groups;
            };

            this.AddGroup = function (group) {
                if (this._groups.length > 0) {
                    group.Parent = this._groups[this._groups.length - 1];
                }
                group.List = this;
                this._groups.push(group);
            };
        },


        Group: function() {
            this._items = [];

            this.List = null;
            this.Container = null;
            this.Parent = null;

            this.GetItems = function() {
                return this._items;
            };

            this.GetItemById = function(id) {
                var items = this.GetItems();
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.Id == id) {
                        return item;
                    }
                }
                return null;
            };

            this.AddItem = function (item) {
                if (item.ParentId != null) {
                    item.Parent = this.Parent.GetItemById(item.ParentId);
                }
                item.Group = this;
                this._items.push(item);
            };

            this.CreateContainer = function () {
                var container = $(document.createElement("select"));
                var items = this.GetItems();
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.Container != null) {
                        container.append(item.Container);
                    }
                }
                this.Container = container;
            };
        },

        Item: function() {
            this.Id = null;
            this.ParentId = null;
            this.Name = null;
            this.Description = null;

            this.Parent = null;
            this.Group = null;
            this.Container = null;

            this.CreateContainer = function () {
                var container = $(document.createElement("option"))
                            .attr("value", this.Id)
                            .text(this.Name);
                this.Container = container;
            };
        }
    }


    $("[data-hierarchylist-list]").each(function () {
        var $this = $(this);

        var list = new dhHelpdesk.HierarchyList.List();
        list.SelectedValue = $this.attr("data-hierarchylist-list-selectedvalue");
        list.GetItemsUrl = $this.attr("data-hierarchylist-list-getitemsurl");
        list.GetChildrenItemsUrl = $this.attr("data-hierarchylist-list-getchildrenitemsurl");

        $.getJSON(list.GetItemsUrl, function(data) {
            for (var i = 0; i < data.Groups.length; i++) {
                var listGroup = new dhHelpdesk.HierarchyList.Group();

                var group = data.Groups[i];
                for (var j = 0; j < group.Items.length; j++) {
                    var item = group.Items[j];
                    var listItem = new dhHelpdesk.HierarchyList.Item();
                    listItem.Id = item.Id;
                    listItem.ParentId = item.ParentId;
                    listItem.Name = item.Name;
                    listItem.Description = item.Description;
                    listItem.CreateContainer();
                    listGroup.AddItem(listItem);
                }
                listGroup.CreateContainer();
                list.AddGroup(listGroup);
            }
            list.CreateContainer($this);
        })
        .always(function() {
            dhHelpdesk.HierarchyList.AddList(list);
        });

    });

});