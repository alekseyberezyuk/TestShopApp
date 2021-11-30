export class FilterParameters {
    categoryId!: string;
    fromPrice!: string;
    toPrice!: string;
    searchParam!: string;

    constructor() {
        this.categoryId = 'all';
        this.fromPrice = '';
        this.toPrice = '';
        this.searchParam = '';
    }
}