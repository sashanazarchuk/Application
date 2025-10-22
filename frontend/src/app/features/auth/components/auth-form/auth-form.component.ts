import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterLink } from "@angular/router";

@Component({
    selector: 'app-auth-form',
    standalone: true,
    imports: [FormsModule, RouterLink],
    templateUrl: './auth-form.component.html',
})
export class AuthFormComponent {
    @Input() title = ''; // "Login" or "Registration"
    @Input() showFullname = false;
    @Input() buttonText = '';
    @Input() errorMessage: string | null = null;
    @Input() linkText = ''; // "No account?" or "Already have an account?"
    @Input() linkUrl = '';  // "/register" or "/login"
    @Input() linkActionText = ''; // "Register here" or "Login here"

    @Output() submitForm = new EventEmitter<any>();

    fullname = '';
    email = '';
    password = '';

    onSubmit() {
        const dto = {
            fullname: this.fullname,
            email: this.email,
            password: this.password,
        };
        this.submitForm.emit(dto);
    }
}