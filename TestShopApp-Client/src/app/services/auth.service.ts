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
      const token = data['token'];
      this.setToken(token);
      const payload = JSON.parse(atob(token.split('.')[1]));
      var userDetails = {
        firstName: payload['firstname'],
        lastName: payload['lastname']
      }
      this.saveUserDetails(userDetails);
      this.saveUserName(creds.username);
    }));
  }

  setToken(token: string) {
    localStorage.setItem('testshopapp-token', token);
  }

  getToken() {
    return localStorage.getItem('testshopapp-token');
  }

  logOut() {
    localStorage.removeItem('testshopapp-token');
    localStorage.removeItem('testshopapp-username');
  }

  isAuthenticated() {
    const token = this.getToken();
    return token && token.length > 0;
  } 

  saveUserName(username: string) {
    localStorage.setItem('testshopapp-username', username);
  }

  getUserName() {
    return localStorage.getItem('testshopapp-username');
  }

  saveUserDetails(userDetails: any) {
    localStorage.setItem('testshopapp-userDetails', JSON.stringify(userDetails));
  }

  getUserDetails(): any {
    return JSON.parse(localStorage.getItem('testshopapp-userDetails') || '{}');
  }
}
