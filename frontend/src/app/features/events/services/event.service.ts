import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventDto } from '../models/event.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPublicEvents(): Observable<EventDto[]> {
    return this.http.get<EventDto[]>(`${this.baseUrl}/events`);
  }

  getMyEvents(): Observable<EventDto[]> {
    return this.http.get<EventDto[]>(`${this.baseUrl}/users/me/events`);
  }

  joinEvent(id: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/events/${id}/join`, {});
  }

  leaveEvent(id: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/events/${id}/leave`, {});
  }
}
