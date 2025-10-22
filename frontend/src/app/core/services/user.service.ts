import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, tap } from "rxjs";
import { UserDto } from "../models/user.model";

@Injectable({
    providedIn: 'root'
})

export class UserService {

    private baseUrl = environment.apiUrl;
    private currentUserSubject = new BehaviorSubject<UserDto | null>(null);

    constructor(private http: HttpClient) { }

    loadCurrentUser(): Observable<UserDto> {
        return this.http.get<UserDto>(`${this.baseUrl}/users/me`).pipe(
            tap(user => this.currentUserSubject.next(user))
        );
    }

    getCurrentUser(): Observable<UserDto | null> {
        return this.currentUserSubject.asObservable();
    }

    clearCurrentUser() {
        this.currentUserSubject.next(null);
    }
}