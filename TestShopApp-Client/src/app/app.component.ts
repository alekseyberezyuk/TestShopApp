import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { fadeAnimation } from './animations/fade.animation';
import { AuthService } from './service';

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

  constructor(translate: TranslateService, private authService: AuthService, private router: Router) {    
    router.events.subscribe(event => {
      if (router.url !== '' && router.url !== 'login' && router.url !== '/login' && authService.isAuthenticated()) {
        this.showTopBar = true;
      } else {
        this.showTopBar = false;
      } 
    });
    translate.setDefaultLang('en-US');
  }
}