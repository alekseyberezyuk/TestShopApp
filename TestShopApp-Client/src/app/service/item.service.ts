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
  
  getFiltered(filterParameters: FilterParameters) {
    let url = environment.baseUrl + '/items?';
    url += `minPrice=${filterParameters.fromPrice || 0}`;
    if (filterParameters.toPrice) {
      url += `&maxPrice=${filterParameters.toPrice}`;
    }
    if(filterParameters.categoryId && filterParameters.categoryId != 'all') {
      url += `&categories=${filterParameters.categoryId}`;
    } 
    if(filterParameters.searchParam) {
      url += `&searchParam=${filterParameters.searchParam}`;
    } 
    return this.http.get<Item[]>(url);  
  }
} 
