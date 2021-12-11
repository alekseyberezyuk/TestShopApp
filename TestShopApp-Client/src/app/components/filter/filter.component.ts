import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { FilterParameters } from '../../models/index'

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
  numbersOnlyRegex: RegExp = /^[1-9][0-9]*/;
  searchParamRegex: RegExp = /^[0-9a-zA-Z,.\-='!@#$%^&*]*$/;
  
  public fromPriceFormControl!: FormControl;
  public toPriceFormControl!: FormControl;
  public searchFormControl!: FormControl;

  get categoryKeys() {
    return Object.keys(this.categories || {});
  }

  constructor() {
  }

  filterValueChanged(isDelayed = false) {
    this.filter.fromPrice = this.fromPriceFormControl.value;
    this.filter.toPrice = this.toPriceFormControl.value;
    this.filter.searchParam = this.searchFormControl.value;
    if (this.validateFilterParams()) {
      if (!isDelayed) {
        this.filterParametersChange.emit(this.filter);
      } else {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
          this.filterParametersChange.emit(this.filter);
        }, 2000);
      }
    }
  }
  
  validateFilterParams() {    
    let result = this.fromPriceFormControl.valid && this.toPriceFormControl.valid && this.searchFormControl.valid;
    let withFromPrice = false;
    let withToPrice = false;
    let fromPrice = 0;
    let toPrice = 0;

    if (this.fromPriceFormControl.valid || this.fromPriceFormControl.hasError('tooBig')) {
      if (this.filter.fromPrice) {
        fromPrice = parseInt(this.filter.fromPrice);
        if (fromPrice >= 0) {
          withFromPrice = true;
          this.fromPriceFormControl.setErrors(null);
        } else {
          this.fromPriceFormControl.setErrors({ invalid: true });
          result = false;
        }
      }
    }
    if (this.toPriceFormControl.valid || this.toPriceFormControl.hasError('tooSmall')) {
      if (this.filter.toPrice) {
        toPrice = parseInt(this.filter.toPrice);
        if (toPrice >= 0) {
          this.toPriceFormControl.setErrors(null);
          withToPrice = true;
        } else {
          this.toPriceFormControl.setErrors({ invalid: true });
          result = false;
        }
      }
    }
    if (withFromPrice && withToPrice && fromPrice > toPrice) {
      this.fromPriceFormControl.setErrors({ tooBig: true });
      this.toPriceFormControl.setErrors({ tooSmall: true });
    } else {
      this.fromPriceFormControl.setErrors(null);
      this.toPriceFormControl.setErrors(null);
    }
    if (this.searchFormControl.valid) {
        this.searchFormControl.setErrors(null);
    } else {
      clearTimeout(this.timeout);
      this.searchFormControl.setErrors({ invalid: true });
      result = false;
    }
    return result;
  }

  clear() {
    if (this.fromPriceFormControl.dirty || this.toPriceFormControl.dirty || this.searchFormControl.dirty) {
      this.filter = new FilterParameters();
      this.fromPriceFormControl.reset();
      this.toPriceFormControl.reset();
      this.searchFormControl.reset();
      this.filterParametersChange.emit(this.filter);
    }
  }

  ngOnInit(): void {
    this.fromPriceFormControl =  new FormControl(this.filter.fromPrice, [Validators.pattern(this.numbersOnlyRegex), Validators.maxLength(9)]);
    this.toPriceFormControl =  new FormControl(this.filter.toPrice, [Validators.pattern(this.numbersOnlyRegex), Validators.maxLength(9)]);
    this.searchFormControl = new FormControl(this.filter.searchParam, [Validators.pattern(this.searchParamRegex), Validators.maxLength(30)]);
  }
}
