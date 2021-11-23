import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from "src/app/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ItemService { 
  constructor(private http: HttpClient) { }
  
  get(): Observable<any> {
    // TODO: Use rxjs methods 'pipe' and 'tap' to set the token here
    return this.http.get(environment.baseUrl + '/items');
  }
} 
