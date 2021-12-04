import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { ItemService } from 'src/app/services/index';
import { environment } from "src/app/environments/environment";
import { Item } from 'src/app/models/index';
import { FilterParameters } from 'src/app/models/filterParameters';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  @ViewChild('drawer') drawer!: MatSlideToggle;
  @ViewChild('toggleFiltersBtn') toggleFiltersBtn!: MatButton;

  filterOpened: boolean = environment.appSettings.filterOpenedByDefault;
  items!: Item[];
  categories!: {};

  get FilterOpened() {
    return this.filterOpened;
  }

  constructor(private itemsService: ItemService, private router: Router) {
  }

  toggleFilterBtnClicked() {
    this.drawer.toggle();
    const nativeElement = this.toggleFiltersBtn._elementRef.nativeElement;

    if (nativeElement.className.includes('show-filter-btn')) {
      setTimeout(() => {
        nativeElement.className = nativeElement.className.replace('show-filter-btn', 'hide-filter-btn');  
        this.filterOpened = true;
      }, 130);
    } else {
      setTimeout(() => {
        nativeElement.className = nativeElement.className.replace('hide-filter-btn', 'show-filter-btn');
        this.filterOpened = false;
      }, 130);
    }
  }

  filterParametersChanged(filterParameters: FilterParameters) {
    this.itemsService.get(filterParameters).subscribe(itemsFromApi => {
      this.items = itemsFromApi; 
    });
  }

  ngOnInit(): void {
    this.categories = {};
    this.itemsService.getCategories().subscribe(categories => {
      for (const c of categories) {
        this.categories[c.id] = c.name;
      }
    });
    this.itemsService.get(new FilterParameters()).subscribe(itemsFromApi => {
      this.items = itemsFromApi;
    });
  }
}
