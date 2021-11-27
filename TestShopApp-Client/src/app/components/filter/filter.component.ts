import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'shop-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})
export class FilterComponent implements OnInit {
  categories = ['Ruby', 'Diamond'];

  constructor() {
  }

  ngOnInit(): void {
  }
}
