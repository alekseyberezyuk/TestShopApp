import { Component, OnInit } from '@angular/core';
import { ItemService } from 'src/app/service/index';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  constructor(private itemsService: ItemService) {
  }

  ngOnInit(): void {
    this.itemsService.get().subscribe(items => {
      console.log(items);
    });
  }
}
