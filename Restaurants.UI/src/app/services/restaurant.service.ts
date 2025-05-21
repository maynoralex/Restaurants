import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Restaurant } from '../models/Restaurant';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {

  constructor(private httpClient: HttpClient) { }

  getRestaurants() : Observable<Restaurant[]> {
    return this.httpClient.get<Restaurant[]>('http://localhost:5207/api/restaurants?PageNumber=1&PageSize=10');
  }
}
