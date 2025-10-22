import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateEventDto, EventDto } from '../models/event.model';
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

  getEventById(id: string): Observable<EventDto> {
    return this.http.get<EventDto>(`${this.baseUrl}/events/${id}`);
  }

  createEvent(event: CreateEventDto): Observable<EventDto> {
    return this.http.post<EventDto>(`${this.baseUrl}/events`, event);
  }

  editEvent(id: string, patch: any[]): Observable<EventDto> {
    return this.http.patch<EventDto>(`${this.baseUrl}/events/${id}`, patch);
  }

  deleteEvent(id: string) {
    return this.http.delete<void>(`${this.baseUrl}/events/${id}`);
  }
}
