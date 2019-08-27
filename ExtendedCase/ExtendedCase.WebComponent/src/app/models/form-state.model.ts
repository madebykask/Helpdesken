
/// FormStateModel
export class FormStateModel {
    private items: FormStateItem[] = [];

    constructor(items?: FormStateItem[]) {
        this.items = items || [];
    }

    setStateData(items:FormStateItem[]) {
        this.items = items || [];
    }

    get Items() {
        return this.items;
    }

    getStateItem(itemPath: FormItemPath, key:string): FormStateItem {
        if (!this.items || this.items.length === 0)
            return null;

        let foundItem =
            this.items.find((item: FormStateItem) =>
                item.tabId === itemPath.tabId  &&
                item.sectionId === itemPath.sectionId &&
                item.sectionIndex === itemPath.sectionIndex &&
                item.key === key);

        return foundItem;
    }

    addStatItem(item: FormStateItem) {
        this.items.push(item);
    }

    updateStateItem(item: FormStateItem) {

        if (!this.items) this.items = [];

        let foundItem =
            this.items.find((el: FormStateItem) =>
                item.tabId === el.tabId &&
                item.sectionId === el.sectionId &&
                item.sectionIndex === el.sectionIndex &&
                item.key === el.key);
        

        if (!foundItem) {
            this.items.push(item); // clone?
        } else {
            foundItem.value = item.value;
        }
    }

    addOrUpdate(itemPath: FormItemPath, key: string, value: string): void {

        if (!this.items) this.items = [];

        let foundItem =
            this.items.find((item: FormStateItem) =>
                item.tabId === itemPath.tabId &&
                item.sectionId === itemPath.sectionId &&
                item.sectionIndex === itemPath.sectionIndex &&
                item.key === key);

        if (!foundItem) {
            //create new 
            foundItem = new FormStateItem(itemPath.tabId,
                itemPath.sectionId,
                itemPath.sectionIndex,
                key,
                value);
            this.items.push(foundItem);
        }

        foundItem.value = value;
    }
}

/// FormStateItem
export class FormStateItem {
    constructor(
        public tabId: string,
        public sectionId: string,
        public sectionIndex: number,
        public key: string,
        public value: string) {
    }
}

/// FormItemPath
export class FormItemPath {

    constructor(public tabId: string,
        public sectionId: string,
        public sectionIndex: number = 0) {
    }

    /// Factory Methods
    static createTabPath(tabId: string) {
        return new FormItemPath(tabId, '');
    }

    static createSectionPath(tabId: string, sectionId: string, sectionIndex: number = 0) {
        return new FormItemPath(tabId, sectionId, sectionIndex);
    }
}

/// FormState Keys
export class FormStateKeys {
    static enableStateSelection = "Section.EnableStateSelection";
}

