import { IMap } from './common-types';

export interface IKeyedCollection<T> {
    getKeys(): string[];
    containsKey(key: string): boolean;
    add(key: string, value: T): void;
    setItemSafe(key: string, item: T): void;
    setItem(key: string, item: T): void;
    remove(key: string): T;
    getItem(key: string): T;
    getItemSafe(key: string): T;
    getItems(): T[];
    getCount(): number;

}

export class KeyedCollection<T> implements IKeyedCollection<T> {

    private items: IMap<T> = {};

    constructor(items?: IMap<T>) {
        this.items = items || {};
    }

    init(items: IMap<T>) {
        this.items = items || {};
    }

    getKeys(): string[] {
        return Object.keys(this.items);
    }

    containsKey(key: string): boolean {
        return this.items.hasOwnProperty(key);
    }

    add(key: string, value: T) {

        this.items[key] = value;
    }

    remove(key: string): T {
        const val = this.items[key];
        delete this.items[key];
        return val;
    }

    setItem(key: string, item: T) {
        this.items[key] = item;
    }

    setItemSafe(key:string, item: T) {
        if (this.containsKey(key)) {
            this.items[key] = item;
        }
    }

    getItem(key: string): T {
        return this.items[key];
    }

    getItemSafe(key: string): T {
        if (this.containsKey(key)) {
            return this.items[key];
        }
        return undefined;
    }

    getItems(): T[] {
        //used to return new collection
        var values: T[] = [];
        Object.keys(this.items)
            .forEach((key: string) => values.push(this.items[key]));

        return values;
    }

    getEntries(): IMap<T> {
        var entries: IMap<T> = {};
        Object.keys(this.items).forEach((key: string) => {
                entries[key] = this.items[key];
            });

        return entries;
    }

    getCount(): number {
        return Object.keys(this.items).length;
    }
}