import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { ItemService } from 'src/app/service/index';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  @ViewChild('drawer') drawer!:MatSlideToggle;
  @ViewChild('toggleFiltersBtn') toggleFiltersBtn!:MatButton;

  filterOpened: boolean = false;

  constructor(private itemsService: ItemService, private router: Router) {
  }

  toggleFilterBtnClicked() {
    this.drawer.toggle();
    const nativeElement = this.toggleFiltersBtn._elementRef.nativeElement;

    if (nativeElement.className.includes('show-filters-btn')) {
      setTimeout(() => {
        nativeElement.className = nativeElement.className.replace('show-filters-btn', 'hide-filters-btn');  
        this.filterOpened = true;
      }, 130);
    } else {
      setTimeout(() => {
        nativeElement.className = nativeElement.className.replace('hide-filters-btn', 'show-filters-btn');
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
