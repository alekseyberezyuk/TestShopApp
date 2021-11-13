import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TestShopApp-Client';

  constructor(private translate: TranslateService) {
    translate.setDefaultLang('en-US');
  }

  langChanged(event: any) {
    console.log(`Language changed to: ${event.target.value}`);
    this.translate.setDefaultLang(event.target.value);
  }
}
