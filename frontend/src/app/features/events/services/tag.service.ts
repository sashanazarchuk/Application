import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { TagDto } from "../models/tag.model";
 
@Injectable({
  providedIn: 'root'
})
export class TagService {

  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }


  getAllTags(): Observable<TagDto[]> {
    return this.http.get<TagDto[]>(`${this.baseUrl}/tags`);
  }

}