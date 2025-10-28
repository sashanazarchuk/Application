import { Routes } from '@angular/router';
import { LoginPage } from './features/auth/pages/login/login.page';
import { MyEventsPage } from './features/events/pages/my-events/my-events.page';
import { RegisterPage } from './features/auth/pages/register/register.page';
import { AllEventPage } from './features/events/pages/all-events/all-events.page';
import { EventDetailsPage } from './features/events/pages/event-details/event-details.page';
import { CreateEventPage } from './features/events/pages/create-event/create-event.page';
import { EditEventPage } from './features/events/pages/edit-event/edit-event.page';
import { AIAssistantPage } from './features/ai-assistant/page/ai-assistant.page';

export const routes: Routes = [
    { path: '', component: MyEventsPage },
    { path: 'login', component: LoginPage },
    { path: 'register', component: RegisterPage },
    { path: 'events', component: AllEventPage },
    { path: 'events/:id', component: EventDetailsPage },
    { path: 'create', component: CreateEventPage },
    { path: 'events/:id/edit', component: EditEventPage },
    { path: 'ai-assistant', component: AIAssistantPage }
];
