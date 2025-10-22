import { Component } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { AuthService } from "../../../../core/services/auth.service";
import { Router, RouterModule } from "@angular/router";
import { LoginDto } from "../../../../core/models/auth.model";
import { AuthFormComponent } from "../../components/auth-form/auth-form.component";
import { ErrorService } from "../../../../core/services/error.service";

@Component({
    selector: 'app-login',
    imports: [FormsModule, RouterModule, AuthFormComponent],
    templateUrl: './login.page.html',
})

export class LoginPage {
    errorMessage = '';

    constructor(private authService: AuthService, private errorService: ErrorService, private router: Router) { }

    login(formData: { email: string; password: string }) {
        const dto: LoginDto = {
            email: formData.email,
            password: formData.password,
        };

        this.authService.login(dto).subscribe({
            next: () => this.router.navigate(['']),
            error: (err) => this.errorMessage = this.errorService.parseValidationErrors(err)
        });
    }
}