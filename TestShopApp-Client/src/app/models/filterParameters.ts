import { OrderBy } from "./index";

export class FilterParameters {
    categoryId!: string;
    fromPrice!: string;
    toPrice!: string;
    searchParam!: string;
    orderBy!: OrderBy;

    constructor() {
        this.categoryId = 'all';
        this.fromPrice = '';
        this.toPrice = '';
        this.searchParam = '';
        this.orderBy = OrderBy.Default;
    }
}