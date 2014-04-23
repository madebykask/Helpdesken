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
            this.Container = null;
            this.Model = null;

            this.CreateContainer = function(element) {
                this.Container = element;
                var groups = this.GetGroups();
                for (var i = 0; i < groups.length; i++) {
                    var group = groups[i];
                    if (group.Container != null) {
                        this.Container.before(group.Container);
                    }
                }
            };

            this.GetGroups = function() {
                return this._groups;
            };

            this.GetGroupById = function(id) {
                var groups = this.GetGroups();
                for (var i = 0; i < groups.length; i++) {
                    var group = groups[i];
                    if (group.Id == id) {
                        return group;
                    }
                }
                return null;
            };

            this.SetModelValue = function(value) {
                this.Model.val(value);
            };

            this.AddGroup = function (group) {
                if (this._groups.length > 0) {
                    group.Parent = this._groups[this._groups.length - 1];
                }
                group.List = this;
                this._groups.push(group);
                group.Id = this._groups.length - 1;
            };

            this.Recalc = function(selected) {
                if (selected == null) {
                    return;
                }

                if (selected.NotEmpty()) {
                    this.SetModelValue(selected.Id);
                } else {
                    var selectedParentGroup = selected.Group.Parent;
                    if (selectedParentGroup != null) {
                        var si = selectedParentGroup.GetSelectedItem();
                        if (si != null) {                            
                            this.SetModelValue(si.Id);                        
                        }
                    } else {
                        this.SetModelValue("");
                    }
                }

                var childGroup = selected.Group.GetChildGroup();
                if (!selected.NotEmpty()) {
                    var child = childGroup;
                    do {
                        if (child != null) {
                            child.Hide();
                            child = child.GetChildGroup();
                        }
                    } while (child != null);
                    return;
                }

                var selectedItems = [];
                var item = selected;
                do {

                    selectedItems.push(item);
                    item = item.Parent;

                } while (item != null);

                var groups = this.GetGroups();
                for (var i = 0; i < groups.length; i++) {
                    var group = groups[i];
                    var found = false;
                    for (var j = 0; j < selectedItems.length; j++) {
                        var selectedItem = selectedItems[j];
                        if (selectedItem.Group.Id == group.Id) {
                            if (selectedItem.Parent != null) {
                                group.ShowByParent(selectedItem.Parent.Id);
                            } else {
                                group.ShowByParent(null);
                            }
                            group.Select(selectedItem.Id);
                            found = true;
                        }
                    }
                    if (found) {
                        group.Show();
                    } else {
                        group.Hide();
                    }
                }
                if (childGroup != null) {
                    childGroup.ShowByParent(selected.Id);
                    childGroup.Select(null);
                }
            };
        },


        Group: function() {
            this._items = [];

            this.List = null;
            this.Container = null;
            this.Parent = null;
            this.Id = null;

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

            this.GetEmptyItem = function() {
                var items = this.GetItems();
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (!item.NotEmpty()) {
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
                var container = $(document.createElement("select"))
                                .attr("group-id", this.Id);
                var items = this.GetItems();
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.Container != null) {
                        container.append(item.Container);
                    }
                }

                var list = this.List;
                container.change(function() {
                    var $this = $(this);
                    var group = list.GetGroupById($this.attr("group-id"));
                    var selectedItem = group.GetItemById($this.val());
                    list.Recalc(selectedItem);
                });

                this.Container = container;
            };

            this.Show = function () {
                this.Container.show();
            };

            this.Hide = function() {
                this.Container.hide();
            };

            this.Select = function(value) {
                this.Container.val(value);
                var items = this.GetItems();
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.Id == value) {
                        item.IsSelected = true;
                        return;
                    }
                }
            };

            this.GetSelectedItem = function() {
                var items = this.GetItems();
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.IsSelected) {
                        return item;
                    }
                }
                return null;
            }

            this.ShowByParent = function(parentId) {
                var items = this.GetItems();
                var onlyEmpty = true;
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.ParentId == parentId || !item.NotEmpty()) {
                        item.Show();

                        if (item.NotEmpty()) {
                            onlyEmpty = false;
                        }

                    } else {
                        item.Hide();
                    }
                }
                if (onlyEmpty) {
                    this.Hide();
                } else {
                    this.Show();
                }
            };

            this.GetChildGroup = function () {
                var groups = this.List.GetGroups();
                for (var i = 0; i < groups.length; i++) {
                    var group = groups[i];
                    if (group.Parent != null && group.Parent.Id == this.Id) {
                        return group;
                    }
                }
                return null;
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
            this.IsSelected = false;

            this.CreateContainer = function () {
                var container = $(document.createElement("option"))
                            .attr("value", this.Id)
                            .text(this.Name);
                this.Container = container;
            };

            this.NotEmpty = function() {
                return parseInt(this.Id) > 0;
            };

            this.Show = function() {
                this.Group.Container.append(this.Container);
            };

            this.Hide = function() {
                this.Container.remove();
            };
        }
    }


    $("[data-hierarchylist-list]").each(function () {
        var $this = $(this);

        var list = new dhHelpdesk.HierarchyList.List();
        list.SelectedValue = $this.attr("data-hierarchylist-list-selectedvalue");
        list.GetItemsUrl = $this.attr("data-hierarchylist-list-getitemsurl");
        list.Model = $(document).find("[ModelId=" + $this.attr("data-hierarchylist-list-modelid") + "]");
        var selectedItem = null;

        var empty = $(document.createElement("select"))
                    .attr("disabled", "disabled");
        $this.before(empty);

        $.getJSON(list.GetItemsUrl, function(data) {
            for (var i = 0; i < data.Groups.length; i++) {
                var listGroup = new dhHelpdesk.HierarchyList.Group();
                list.AddGroup(listGroup);

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

                    if (selectedItem == null && listItem.Id == list.SelectedValue) {
                        selectedItem = listItem;
                    }
                }
                listGroup.CreateContainer();
            }
            list.CreateContainer($this);
        })
        .always(function() {
            dhHelpdesk.HierarchyList.AddList(list);
            list.Recalc(selectedItem);
            empty.remove();
        });

    });

});