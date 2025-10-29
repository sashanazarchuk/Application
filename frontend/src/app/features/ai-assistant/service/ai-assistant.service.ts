import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class AIAssistantService {

    private baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    askAI(message: string): Observable<{ reply: string }> {
        return this.http.post<{ reply: string }>(`${this.baseUrl}/ai/ask`, { messageToAI: message }
        );
    }
}