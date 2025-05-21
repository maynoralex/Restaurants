import { Component, inject, OnInit } from '@angular/core';
import { Restaurant } from '../models/Restaurant';
import { RestaurantService } from '../services/restaurant.service';

@Component({
  selector: 'app-restaurant-list',
  imports: [],
  templateUrl: './restaurant-list.component.html',
  styleUrl: './restaurant-list.component.css'
})
export class RestaurantListComponent implements OnInit {
  
  restaurants: Restaurant[] = [];

  restaurantService: RestaurantService = inject(RestaurantService);

  ngOnInit(): void {
    this.loadRestaurants();
  }

  loadRestaurants(): void {
    this.restaurantService.getRestaurants().subscribe(
      (response: Restaurant[]) => {
        this.restaurants = response;
      }
    );
  }

}
