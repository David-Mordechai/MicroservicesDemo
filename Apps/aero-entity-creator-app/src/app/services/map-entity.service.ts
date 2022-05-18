import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MapEntityService {

  private URL: string = environment.baseApiUrl
  constructor(private http: HttpClient) { }

  public postMapEntity(formData: any) {
    return this.http.post(`${this.URL}mapEntity`, formData).subscribe({
      next: (result) => console.log(result),
      error: error => console.log(error)
    });
  }
}
