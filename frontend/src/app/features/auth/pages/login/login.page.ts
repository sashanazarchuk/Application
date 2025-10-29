import { Component } from "@angular/core";
import { FormsModule } from '@angular/forms';
import { RouterModule } from "@angular/router";
import { AuthFormComponent } from "../../components/auth-form/auth-form.component";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { login } from "../../store/auth.actions";
import { Observable } from "rxjs";
import { selectAuthError } from "../../store/auth.selectors";
import { AsyncPipe } from "@angular/common";

@Component({
    selector: 'app-login',
    imports: [FormsModule, RouterModule, AuthFormComponent, AsyncPipe],
    templateUrl: './login.page.html',
})

export class LoginPage {

    errorMessage$: Observable<string | null>;

    constructor(private store: Store<AppState>) {
        this.errorMessage$ = this.store.select(selectAuthError);
    }

    login(formData: { email: string; password: string }) {
        this.store.dispatch(login({ dto: { email: formData.email, password: formData.password } }));
    }
}