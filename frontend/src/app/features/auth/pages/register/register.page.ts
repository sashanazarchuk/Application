import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { AuthFormComponent } from "../../components/auth-form/auth-form.component";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { register } from "../../store/auth.actions";
import { Observable } from "rxjs";
import { selectAuthError } from "../../store/auth.selectors";
import { AsyncPipe } from "@angular/common";

@Component({
    selector: 'app-register',
    imports: [FormsModule, RouterModule, AuthFormComponent, AsyncPipe],
    templateUrl: './register.page.html',
})

export class RegisterPage {

    errorMessage$: Observable<string | null>;

    constructor(private store: Store<AppState>) { 
        this.errorMessage$ = this.store.select(selectAuthError);
    }

    register(formData: { fullname: string; email: string; password: string }) {
        this.store.dispatch(register({ dto: { fullname: formData.fullname, email: formData.email, password: formData.password } }));
    }
}