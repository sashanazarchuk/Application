import { Routes } from '@angular/router';
import { LoginPage } from './features/auth/pages/login/login.page';
import { MyEventsPage } from './features/events/pages/my-events/my-events.page';

export const routes: Routes = [
    { path: '', component: MyEventsPage },
    { path: 'login', component: LoginPage },
];
