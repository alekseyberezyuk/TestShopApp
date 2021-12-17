import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { ItemService, TranslationService } from 'src/app/services/index';
import { environment } from "src/app/environments/environment";
import { Item, OrderBy } from 'src/app/models/index';
import { FilterParameters } from 'src/app/models/filterParameters';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, AfterViewInit {
  @ViewChild('drawer') drawer!: MatSlideToggle;
  @ViewChild('toggleFiltersBtn') toggleFiltersBtn!: MatButton;
  @ViewChild('paginator') paginator!: MatPaginator;

  filterOpened: boolean = environment.appSettings.filterOpenedByDefault;
  items!: Item[];
  categories!: {};
  filterParameters: FilterParameters = new FilterParameters();
  orderByTranslations!: {};
  itemsTotal!: number;
  currentPage: number = 1;
  pageSize = 5;

  get FilterOpened() {
    return this.filterOpened;
  }

  get KeysForOrderBy() {
    return Object.keys(OrderBy);
  }

  get TotalPages() { 
    return Math.ceil(this.itemsTotal / this.pageSize) || this.currentPage;
  }

  constructor(private itemsService: ItemService, private translateService: TranslationService, private router: Router) {
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
    filterParameters.orderBy = this.filterParameters.orderBy;
    this.filterParameters = filterParameters;
    this.getItems();
  }

  orderByChanged() {
    this.getItems();
  }
  
  async updateOrderbyOptions() {
    this.orderByTranslations = {};
    for (const key of Object.keys(OrderBy)) {
      this.orderByTranslations[key] = await this.translateService.getSingleTranslation(`sorting-options.${key}`);
    }
  }

  getItemThumbnailSrc(item: Item) {
    if (item.thumbnailBase64) {
      return `data:image/png;base64,${item.thumbnailBase64}`;
    } else {
      return `../../../assets/images/thumbnails/${item.categoryId}.png`;
    }
  }

  paginatorParametersChanged(paginatorEventData) {
    this.currentPage = paginatorEventData.pageIndex + 1;
    this.pageSize = paginatorEventData.pageSize;
    this.getItems();
  }
  
  getItems() {
    this.itemsService.get(this.filterParameters, this.currentPage, this.pageSize).subscribe(itemResponseFromApi => {
      this.items = itemResponseFromApi.items;
      this.itemsTotal = itemResponseFromApi.totalItems;  
    });
  }
  
  async translatePaginator() {
    this.paginator._intl.itemsPerPageLabel = await this.translateService.getSingleTranslation('paginator.itemsPerPage');
    this.paginator._intl.nextPageLabel = await this.translateService.getSingleTranslation('paginator.nextPage');
    this.paginator._intl.previousPageLabel = await this.translateService.getSingleTranslation('paginator.prevPage');
    const getRangeLabel: string = await this.translateService.getSingleTranslation('paginator.rangeLabel');
    this.paginator._intl.getRangeLabel = (currentPage, pSize, total) => `${getRangeLabel} ${this.currentPage}/${this.TotalPages}`;
    this.paginator._changePageSize(this.pageSize);
   }
  
  ngOnInit(): void {
    this.categories = {};
    this.itemsService.getCategories().subscribe(categories => {
      for (const c of categories) {
        this.categories[c.id] = c.name;
      }
    });
    this.getItems();
    this.updateOrderbyOptions();
    this.translateService.onDefaultLangChange.subscribe(l => {
      this.updateOrderbyOptions();
      this.translatePaginator();
    });
  }

  ngAfterViewInit(): void {
    this.translatePaginator();
  }
}
