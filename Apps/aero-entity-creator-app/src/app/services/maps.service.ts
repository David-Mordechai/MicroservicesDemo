import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MapsService {

  private URL: string = environment.baseApiUrl
  constructor(private http: HttpClient) { }

  getMaps(): Observable<string[]> {
    return this.http.get<string[]>(`${this.URL}maps`)
  }
}
