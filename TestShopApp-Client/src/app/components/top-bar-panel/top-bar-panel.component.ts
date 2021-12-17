import { ChangeDetectorRef, AfterContentChecked, Component, ElementRef, OnInit, ViewChild, Input } from '@angular/core';
import { Router } from '@angular/router';
import { trigger, transition, animate, style } from '@angular/animations'
import { AuthService, TranslationService } from 'src/app/services';

@Component({
  selector: 'top-bar-panel',
  templateUrl: './top-bar-panel.component.html',
  styleUrls: ['./top-bar-panel.component.css'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({transform: 'translateY(-100%)'}),
        animate('400ms ease-in', style({transform: 'translateY(0%)'}))
      ]),
      transition(':leave', [
        animate('400ms ease-in', style({transform: 'translateY(-100%)'}))
      ])
    ])
  ]
})
export class TopBarPanelComponent implements OnInit, AfterContentChecked {
  @Input() showMenu!: boolean;
  
  language!: string;
  user!: string;

  get languageLabel() {
    return this.language.split('-')[0].toUpperCase();
  }  

  constructor(
    public authService: AuthService,
    private translationService: TranslationService,
    private router: Router,
    private ref: ChangeDetectorRef
    ) {
  }

  languageChanged() {
    this.translationService.setDefaultLang(this.language);
    localStorage.setItem('testshopapp-lang', this.language);
    // const currentUrl = this.router.url;
    // this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
    //     this.router.navigate([currentUrl]);
    // });
  }

  logOutBtnClicked() {
    this.authService.logOut();
    this.router.navigateByUrl(''); 
  }

  redirectTo(url: string) {
    this.router.navigateByUrl(url);
  }

  getUserName() {
    const userDetails = this.authService.getUserDetails();
    if (userDetails && userDetails.firstName) {
      return userDetails.firstName;
    }
    let userName = this.authService.getUserName();
    userName = userName ? userName.split('@')[0] : '';

    if (userName.length > 0) {
      userName = userName[0].toUpperCase() + userName.substr(1).toLowerCase();
    }
    return userName;
  }

  ngOnInit(): void {
    this.language = localStorage.getItem('testshopapp-lang') || this.translationService.getDefaultLang();
    this.translationService.setDefaultLang(this.language);
    this.user = this.getUserName();
  }

  ngAfterContentChecked() : void {
    this.ref.detectChanges();
  }
}
