import { ErrorHandler, Inject, Injector, forwardRef } from '@angular/core';
import { ErrorHandlingService } from '../services/error-handling.service';

export class GlobalErrorHandler implements ErrorHandler {

    constructor(@Inject(forwardRef(() => Injector)) private injector: Injector) {
    }

    handleError(error: any): void {
        let errorHandlingService = this.injector.get(ErrorHandlingService);
        if (errorHandlingService) {
            errorHandlingService.handleError(error, 'Unknown error occured.');
            return;
        } else {
            throw error;
        }
    }
}




