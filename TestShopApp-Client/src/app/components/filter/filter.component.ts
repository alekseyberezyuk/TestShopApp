import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';
import { FilterParameters } from 'src/app/models/filterParameters';

@Component({
  selector: 'shop-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})
export class FilterComponent implements OnInit {  
  @Input() categories;

  @Output() filterParametersChange = new EventEmitter<FilterParameters>();
  
  filter: FilterParameters = new FilterParameters();
  timeout: any = null;
  paramsChanged: boolean = false;

  get categoryKeys() {
    return Object.keys(this.categories || {});
  }

  constructor() {
  }

  filterValueChanged(isDelayed = false) {
    if (!isDelayed) {
      this.filterParametersChange.emit(this.filter);
      this.paramsChanged = true;
    } else {
      clearTimeout(this.timeout);
      this.timeout = setTimeout(() => {
        this.filterParametersChange.emit(this.filter);
        this.paramsChanged = true;
      }, 2000);
    }
  }

  clear() {
    if (this.paramsChanged) {
      this.filter = new FilterParameters();      
      this.paramsChanged = false;
      this.filterValueChanged();
    }
  }

  ngOnInit(): void {
  }
}
