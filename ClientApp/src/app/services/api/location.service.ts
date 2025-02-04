import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getCountries(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'location/countries');
  }

  getProvinces(countryId: number): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + `location/provinces/${countryId}`);
  }
}
