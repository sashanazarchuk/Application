import { ApplicationConfig, isDevMode, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { aiAssistantReducer } from './features/ai-assistant/store/ai-assistant.reducer';
import { provideEffects } from '@ngrx/effects';
import { AIAssistantEffects } from './features/ai-assistant/store/ai-assistant.effects';
import { authReducer } from './features/auth/store/auth.reducer';
import { AuthEffects } from './features/auth/store/auth.effects';
import { eventReducer } from './features/events/store/event.reducer';
import { EventEffects } from './features/events/store/event.effects';


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    provideStore(
      {
        aiAssistant: aiAssistantReducer,
        auth: authReducer,
        event: eventReducer
      }
    ),
    provideEffects([
      AIAssistantEffects,
      AuthEffects,
      EventEffects
    ]),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() })
  ]
};