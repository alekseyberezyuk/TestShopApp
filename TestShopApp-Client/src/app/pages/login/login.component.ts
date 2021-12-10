import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { AuthResponse } from 'src/app/models/authResponse';
import { Credentials } from 'src/app/models/index';
import { AuthService } from 'src/app/services/auth.service';
import { FormControl, Validators, } from "@angular/forms";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  // TODO: later remove hardcoded credentials
  public creds: Credentials = new Credentials('user@testshopapp.com', 'User12#');
  public usernameFormControl!: FormControl;
  public passwordFormControl!: FormControl;

  constructor(private toastr: ToastrService,
    private authService: AuthService,
    private translateService: TranslateService,
    private router: Router, 
  ) {
  } 
  
  btnClicked() {
    if (this.usernameFormControl.valid && this.passwordFormControl.valid) {
      this.creds.username = this.usernameFormControl.value;
      this.creds.password = this.passwordFormControl.value;
      this.authService.authenticate(this.creds).subscribe({
        next: async data => {
          const authResponse: AuthResponse = data;
          if(authResponse.isSuccess) {
            this.toastr.clear();
            this.router.navigateByUrl('main');
          } else {
            const errorMsg = await firstValueFrom(this.translateService.get('auth-fail'));
            this.toastr.warning(errorMsg);
          }
        },
        error: (error) => console.error(error)
      });
    }
  }
  
  ngOnInit(): void {
    if(this.authService.isAuthenticated()) {
      this.router.navigateByUrl('main'); 
    }
    this.usernameFormControl =  new FormControl(this.creds.username, [Validators.required, Validators.email, Validators.minLength(6), Validators.maxLength(64)]);
    this.passwordFormControl = new FormControl(this.creds.password, [Validators.required, Validators.maxLength(32)]);
  }
}
