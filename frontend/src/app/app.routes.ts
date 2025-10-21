import { Routes } from '@angular/router';
import { LoginPage } from './features/auth/pages/login/login.page';
import { MyEventsPage } from './features/events/pages/my-events/my-events.page';
import { RegisterPage } from './features/auth/pages/register/register.page';
import { AllEventPage } from './features/events/pages/all-events/all-events.page';

export const routes: Routes = [
    { path: '', component: MyEventsPage },
    { path: 'login', component: LoginPage },
    { path: 'register', component: RegisterPage },
    { path: 'events', component: AllEventPage },

];
