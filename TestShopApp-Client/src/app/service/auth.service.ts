import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from "src/app/environments/environment";
import { Credentials } from '../models/credentials';

@Injectable({
  providedIn: 'root'
})
export class AuthService { 
  constructor(private http: HttpClient) { }
  
  authenticate(creds: Credentials): Observable<any> {
    // TODO: Use rxjs methods 'pipe' and 'tap' to set the token here
    return this.http.post(environment.baseUrl + '/Auth', creds);
  }

  setToken(token: string) {
    // TODO: Set token in localocalStorage here
  }

  getToken() {
    // TODO: Get token from localStorage here
  }
}
