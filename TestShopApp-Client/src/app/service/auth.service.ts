import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from "src/app/environments/environment";
import { Credentials } from '../models/credentials';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }
  
  authenticate(creds: Credentials): Observable<any> {
    // TODO: Use rxjs methods 'pipe' and 'tap' to set the token here
    return this.http.post(environment.baseUrl + '/Auth', creds).pipe(tap(data => {
      this.setToken(data['token']);
    }));
  }

  setToken(token: string) {
    localStorage.setItem('testshopapp-token', token);
  }

  getToken() {
    return localStorage.getItem('testshopapp-token');
  }

  logOut() {
    localStorage.setItem('testshopapp-token', '');
  }

  isAuthenticated() {
    const token = this.getToken();
    return token && token.length > 0;
  } 
}
