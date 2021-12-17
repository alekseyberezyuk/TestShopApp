import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from "src/app/environments/environment";
import { Category, ItemsResponse, OrderBy } from '../models';
import { FilterParameters } from '../models/filterParameters';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  constructor(private http: HttpClient) { }
  
  // TODO: Add new pagination parameters here. Check Swagger to see parameters responsible for pagination
  get(filterParameters: FilterParameters, currentPage: number, itemsPerPage: number) {
    let url = `${environment.baseUrl}/items?minPrice=${filterParameters.fromPrice || 0}`;
    if (filterParameters.toPrice) {
      url += `&maxPrice=${filterParameters.toPrice}`;
    }
    if(filterParameters.categoryId && filterParameters.categoryId != 'all') {
      url += `&categories=${filterParameters.categoryId}`;
    } 
    if(filterParameters.searchParam) {
      url += `&searchParam=${filterParameters.searchParam}`;
    } 
    if(filterParameters.orderBy != OrderBy.Default) {
      url += `&orderBy=${filterParameters.orderBy}`;
    }
    if (itemsPerPage < 5) {
      itemsPerPage = 5;
    }
    url += `&itemsPerPage=${itemsPerPage}`;
    url += `&pageNumber=${currentPage}`;
    return this.http.get<ItemsResponse>(url);  
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(environment.baseUrl + '/categories');
  } 
} 
