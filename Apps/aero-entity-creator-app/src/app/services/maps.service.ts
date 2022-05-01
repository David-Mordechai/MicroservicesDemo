import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { mapListItem } from '../models/mapListItem';

interface getMapResponse {
  success: boolean,
  errorMessage: string,
  mapFileAsBase64String: {
    imageMetaData: string,
    imageBase64: string
  },
}

interface uploadMapResponse {
  success: boolean,
  errorMessage: string
}

@Injectable({
  providedIn: 'root'
})
export class MapsService {
  
  private URL: string = environment.baseApiUrl
  constructor(private http: HttpClient) { }

  getMaps(): Observable<mapListItem[]> {
    return this.http.get<mapListItem[]>(`${this.URL}maps`)
  }

  getMap(mapName: string): Observable<getMapResponse> {
    return this.http.get<getMapResponse>(`${this.URL}maps/${mapName}`)
  }

  getMissionMap(): Observable<getMapResponse> {
    return this.http.get<getMapResponse>(`${this.URL}mission`)
  }

  uploadMap(formData: FormData) :Observable<uploadMapResponse> {
    return this.http.post<uploadMapResponse>(`${this.URL}maps`, formData);
  }

  deleteMap(mapName: string) : Observable<string> {
    return this.http.delete<string>(`${this.URL}maps/${mapName}`)
  }

  setMissionMap(mapName: string)  {
    const formData = new FormData();
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
    formData.append('mapName', mapName);
    return this.http.post(`${this.URL}mission`,{mapName: mapName},httpOptions)
  }
}
