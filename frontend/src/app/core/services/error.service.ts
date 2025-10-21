import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })

export class ErrorService {

    parseValidationErrors(error: any): string {
        if (error?.error?.errors) {
            const messages: string[] = [];
            for (const key in error.error.errors) {
                if (error.error.errors.hasOwnProperty(key)) {
                    messages.push(...error.error.errors[key]);
                }
            }
            return messages.join(' ');
        }
        return 'Something went wrong. Please try again.';
    }
}