import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { fadeAnimation } from './animations/fade.animation';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [fadeAnimation]
})
export class AppComponent {
  showTopBar: boolean = true;

  get ShowTopBar() {
    return this.showTopBar;
  }

  constructor(translate: TranslateService, private router: Router) {    
    router.events.subscribe(event => {
      console.log(router.url);
      this.showTopBar = router.url !== '' && router.url !== '/login';
    });
    translate.setDefaultLang('en-US');
  }
}