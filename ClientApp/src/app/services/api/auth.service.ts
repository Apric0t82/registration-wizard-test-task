import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  register(userData: any) {
    return this.http.post(this.baseUrl + 'account/register', userData);
  }
}
