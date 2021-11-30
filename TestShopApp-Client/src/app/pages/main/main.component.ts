import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { ItemService } from 'src/app/service/index';
import { environment } from "src/app/environments/environment";
import { Item } from 'src/app/models/item';
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
    // Remove later when no longer required
    console.log(filterParameters);

    // Add here new code that uses the new method 'getFiltered' in itemsService and send filterParameters to the server    
  }

  ngOnInit(): void {
    this.itemsService.getAll().subscribe(itemsFromApi => {
      this.items = itemsFromApi;
      this.categories = {};

      for (const i of itemsFromApi) {
        this.categories[i.categoryId] = i.categoryName;
      }
    });
  }
}
