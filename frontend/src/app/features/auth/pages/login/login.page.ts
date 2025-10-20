import { Component } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { AuthService } from "../../../../core/services/auth.service";
import { Router } from "@angular/router";
import { NgIf } from "@angular/common";
import { LoginDto } from "../../../../core/models/auth.model";

@Component({
    selector: 'app-login',
    imports: [FormsModule, NgIf],
    templateUrl: './login.page.html',
})

export class LoginPage {

    email = '';
    password = '';
    errorMessage = '';

    constructor(private authService: AuthService, private router: Router) { }

    login() {
        const dto: LoginDto = {
            email: this.email,
            password: this.password
        };

        this.authService.login(dto).subscribe({
            next: () => this.router.navigate(['']),
            error: () => this.errorMessage = 'Invalid email or password'
        });
    }
}