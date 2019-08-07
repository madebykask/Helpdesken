import { Subscription } from 'rxjs';
import { KeyedCollection } from './keyed-collection';

// don't use - replace with take(1), takeUntil($) strategy
export class SubscriptionManager {
    private items: KeyedCollection<Subscription>;

    constructor() {
        this.items = new KeyedCollection<Subscription>();
    }

    addSingle(key: string, subscription: Subscription): void {
        if (this.items.containsKey(key)) { this.items.getItem(key).unsubscribe(); }
        this.items.add(key, subscription);
    }

    remove(key: string): void {
        if (this.items.containsKey(key)) {
            if (this.items[key] != null) { this.items[key].unsubscribe(); }
            this.items.remove(key);
        }
    }

    removeAll() {
        this.items.getKeys().forEach((key: string) => {
            this.remove(key);
        });
    }
}