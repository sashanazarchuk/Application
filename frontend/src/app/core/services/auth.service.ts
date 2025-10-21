import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { environment } from "../../../environments/environment";
import { TokenService } from "./token.service";
import { UserService } from "./user.service";
import { LoginDto, AuthResponse, RegisterDto } from "../models/auth.model";

@Injectable({
    providedIn: 'root'
})

export class AuthService {

    private baseUrl = environment.apiUrl;

    constructor(private http: HttpClient, private tokenService: TokenService, private userService: UserService) { }

    login(dto: LoginDto): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.baseUrl}/auth/login`, dto).pipe(
            tap(res => {
                this.tokenService.storeTokens(res.accessToken, res.refreshToken);
                this.userService.loadCurrentUser().subscribe();
            })
        );
    }

    logout(skipServer: boolean = false) {
        this.tokenService.setAccessToken(null);
        localStorage.removeItem('refreshToken');

        this.userService.clearCurrentUser();

        if (!skipServer) {
            this.http.post(`${this.baseUrl}/auth/logout`, {}).subscribe({
                next: () => console.log('Logged out on server'),
                error: () => console.warn('Server logout failed')
            });
        }
    }

    isLoggedIn(): boolean {
        return !!this.tokenService.getAccessToken();
    }

    register(dto: RegisterDto): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.baseUrl}/auth/register`, dto).pipe(
            tap(res => {
                
                this.tokenService.storeTokens(res.accessToken, res.refreshToken);
                
                this.userService.loadCurrentUser().subscribe();
            })
        );
    }
}