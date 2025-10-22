import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AuthService } from "../../../../core/services/auth.service";
import { Router, RouterModule } from "@angular/router";
import { RegisterDto } from "../../../../core/models/auth.model";
import { AuthFormComponent } from "../../components/auth-form/auth-form.component";
import { ErrorService } from "../../../../core/services/error.service";

@Component({
    selector: 'app-register',
    imports: [FormsModule, RouterModule, AuthFormComponent],
    templateUrl: './register.page.html',
})

export class RegisterPage {

    errorMessage = '';

    constructor(private authService: AuthService, private errorService: ErrorService, private router: Router) { }

    register(formData: { fullname: string; email: string; password: string }) {
        const dto: RegisterDto = {
            fullname: formData.fullname,
            email: formData.email,
            password: formData.password,
        };

        this.authService.register(dto).subscribe({
            next: () => this.router.navigate(['']),
            error: (err) => this.errorMessage = this.errorService.parseValidationErrors(err)

        });
    }
}