import { ChangeDetectorRef, AfterContentChecked, Component, ElementRef, OnInit, ViewChild, Input } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { trigger, transition, animate, style } from '@angular/animations'
import { AuthService } from 'src/app/service';

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
  language!: string;

  @Input() showMenu!: boolean;

  get languageLabel() {
    return this.language.split('-')[0].toUpperCase();
  }

  get getUserName() {
    let userName = this.authService.getUserName();
    userName = userName ? userName.split('@')[0] : '';

    if (userName.length > 0) {
      userName = userName[0].toUpperCase() + userName.substr(1).toLowerCase();
    }
    return userName;
  }

  constructor(
    public authService: AuthService,
    private translateService: TranslateService,
    private router: Router,
    private ref: ChangeDetectorRef
    ) {
  }

  languageChanged() {
    this.translateService.setDefaultLang(this.language);
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

  ngOnInit(): void {
    this.language = this.translateService.getDefaultLang();
  }

  ngAfterContentChecked() : void {
    this.ref.detectChanges();
  }
}
