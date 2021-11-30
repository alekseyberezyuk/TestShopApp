import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from "src/app/environments/environment";
import { FilterParameters } from '../models/filterParameters';
import { Item } from '../models/item';

@Injectable({
  providedIn: 'root'
})
export class ItemService { 
  constructor(private http: HttpClient) { }
  
  getAll(): Observable<Item[]> {
    return this.http.get<Item[]>(environment.baseUrl + '/items');
  }

  // Add here a new method named 'getFiltered' or sth like that, it should accept a parameter of type ':FilterParameters'
  // Check swagger to see how does the 'GET /items' api work with query parameters ?xxx&yyy
  // When you understand how it works add a get request to the api in your new 'getFiltered' method and use parameters this time
} 
