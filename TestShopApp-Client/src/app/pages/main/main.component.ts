import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { ItemService } from 'src/app/service/index';
import { environment } from "src/app/environments/environment";

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  @ViewChild('drawer') drawer!:MatSlideToggle;
  @ViewChild('toggleFiltersBtn') toggleFiltersBtn!:MatButton;

  filterOpened: boolean = environment.appSettings.filterOpenedByDefault;

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

  ngOnInit(): void {
    this.itemsService.get().subscribe(items => {
      console.log(items);
    });
  }
}
