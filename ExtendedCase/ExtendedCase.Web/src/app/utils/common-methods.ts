import { Injectable, Inject } from '@angular/core';
import { ItemModel } from '../models/form.model';
import { IMap } from '../shared/common-types';
var deepEqual = require('deep-equal');
import * as commonMethods from '../utils/common-methods';

export function areItemModelArraysEqual(array1: ItemModel[], array2: ItemModel[]): boolean {
        if (!array1 || !array2) {
            return false;
        }

        let sortComparer = (x: ItemModel, y: ItemModel) => {
            if (x.value < y.value) return -1;
            if (x.value > y.value) return 1;
            return 0;
        };

        let a = array1.slice().sort(sortComparer);
        let b = array2.slice().sort(sortComparer);

        return a.length === b.length && a.every((el, ix) => el.value === b[ix].value && el.text === b[ix].text);
    }

    export function areArraysEqual(array1: any[], array2: any[]): boolean {
        if (!array1 || !array2) {
            return false;
        }

        let a = array1.slice().sort();
        let b = array2.slice().sort();

        return a.length === b.length && a.every((el, ix) => el === b[ix]);
    }

    export function isArray(obj: any): boolean {
        return (obj instanceof Array) || Array.isArray(obj);
    }

    export function areValuesEqual(value1: any, value2: any): boolean {
        if (value1 instanceof Array && value2 instanceof Array) {
            return commonMethods.areArraysEqual(value1 as any[], value2 as any[]);
        }

        return value1 === value2;
    }

    export function areEqualsDeep(val1: any, val2: any) {
        return deepEqual(val1, val2, { strict : true });
    }

    export function convertAnyToString(obj: any):string {
        if (isUndefinedOrNull(obj)) {
            return '';
        }

        if (isArray(obj) && obj) {
            return obj.join();
        }

        return obj.toString();
    }

    export function clone(obj: any, copyNotOwnProperties?: boolean): any {
        if (commonMethods.isArray(obj))
            return obj.slice();

        let newObj = {};
        for (let prop in obj) {
            if (copyNotOwnProperties || obj.hasOwnProperty(prop)) {
                newObj[prop] = obj[prop];
            }
        }

        return newObj;
    }

    export function hasKey<T>(items: T, key: string): boolean {
        if (!items || items.constructor !== Array && items.constructor !== Object)
            return false;
        
        return items.hasOwnProperty(key);
    }

    export function getMapValueSafe<T>(items: IMap<T>, key: string): T {
        if (!commonMethods.hasKey(items, key))
            return undefined;

        return items[key];
    }


    export function toNumber(strNumber: string): number {
        if (!strNumber.length) {
            throw new Error(`toNumber: value is empty`);
        }
        return parseFloat(strNumber.replace(',', '.').replace(' ', ''));
    }

    export function isUndefinedOrNull(value:any) {
        return value === null || value === undefined;
    }

    export function isUndefinedNullOrEmpty(value:any) {
        return value === '' || value === undefined || value === null;
    }

    export function areUndefinedNullOrEmpty(val1: any, val2: any) {
        return isUndefinedNullOrEmpty(val1) && isUndefinedNullOrEmpty(val2);
    }

    export function areDictionariesEqual(a: IMap<any>, b: IMap<any>) : boolean {
        if ((isUndefinedNullOrEmpty(a) && !isUndefinedNullOrEmpty(b)) ||
           (!isUndefinedNullOrEmpty(a) && isUndefinedNullOrEmpty(b)))
            return false;

        var aKeys = Object.keys(a).sort();
        var bKeys = Object.keys(b).sort();

        if (aKeys.length !== bKeys.length) {
            return false;
        }

        return !aKeys.some((key: any) => (a[key] !== b[key]));
    }

    export function anyToBoolean(val: any): boolean {
        if (isUndefinedNullOrEmpty(val))
            return false;
        
        switch (val.toString().toLowerCase().trim()) {
            case 'true':
            case 'yes':
            case '1':
                return true;

            case 'false':
            case 'no':
            case '0':
            case null:
                return false;

            default:
                return Boolean(val);
        }
    }

    export function removeAt(str: string, startIndex: number, count: number): string {
        return str.substr(0, startIndex) + str.substr(startIndex + count);
    }

    export function removeMultipleSpaces(val: string, trimTrailing:boolean = true): string {
        if (typeof val !== 'string') return val;

        let regex = /\s{2,}/igm; //with multiline (m)

        while (regex.test(val)) {
            val = val.replace(regex, ' ');
        }

        if (trimTrailing) {
            val = val.trim();
        }

        return val;
    }

    export function formatString(template: string, args: any[]): string {
        let result = template.replace(/{(\d+)}/g, (match: string, index: number) => {
            return typeof args[index] !== 'undefined'
                ? args[index].toString()
                : match;

        });
        return result;
    }

    export function formatObject(item: any):string {
        if (isUndefinedNullOrEmpty(item)) return '';

        //stringify arrays and objects
        if (isArray(item) || typeof item === 'object') {
            return JSON.stringify(item);
        }

        //numbers, boolean, string
        return item.toString();
    }

    export function parseIntOrDefault(val:any, defaultVal:number = 0) {
        if (isUndefinedNullOrEmpty(val))
            return defaultVal;
        
        let res = parseInt(val.toString());        
        if (isNaN(res) || res === undefined)
            return defaultVal;

        return res;
    }
