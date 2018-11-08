import { ErrorHandler, Inject, Injector, forwardRef } from '@angular/core';
import { ErrorHandlingService } from '../../services/errorhandling/error-handling.service';

export class GlobalErrorHandler implements ErrorHandler {

    // ErrorHandler is created before the providers - need to use the Injector to get it
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




