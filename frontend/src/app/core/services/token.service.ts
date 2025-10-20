import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable, tap, throwError } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { LoginResponse } from "../models/auth.model";

@Injectable({
    providedIn: 'root'
})

export class TokenService {

    private baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    refreshToken(): Observable<LoginResponse> {
        const refresh = this.getRefreshToken();
        if (!refresh) return throwError(() => new Error('No refresh token'));

        return this.http.post<LoginResponse>(`${this.baseUrl}/auth/refresh`, { refreshToken: refresh }).pipe(
            tap(res => this.storeTokens(res.accessToken, res.refreshToken))
        );
    }

    storeTokens(accessToken: string, refreshToken: string) {
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);
    }

    getAccessToken(): string | null {
        return localStorage.getItem('accessToken');
    }

    getRefreshToken(): string | null {
        return localStorage.getItem('refreshToken');
    }

    setAccessToken(accessToken: string | null) {
        if (accessToken) {
            localStorage.setItem('accessToken', accessToken);
        } else {
            localStorage.removeItem('accessToken');
        }
    }
    
}