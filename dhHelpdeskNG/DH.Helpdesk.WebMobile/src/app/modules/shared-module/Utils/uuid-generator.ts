import * as uuid from 'uuid';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UuidGenerator {
    static createUuid(): string { // Public Domain/MIT
      return uuid.v4();
    }
}
