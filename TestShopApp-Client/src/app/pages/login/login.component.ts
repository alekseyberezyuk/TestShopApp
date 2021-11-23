import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { AuthResponse } from 'src/app/models/authResponse';
import { Credentials } from 'src/app/models/credentials';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  // TODO: later remove hardcoded credentials
  public creds: Credentials = new Credentials('user@testshopapp.com', 'User12#');
 
  constructor(private toastr: ToastrService, private authService: AuthService, private translateService: TranslateService, private router: Router) {
  }
  
  btnClicked() {
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
  
  ngOnInit(): void {
    if(this.authService.isAuthenticated()) {
      this.router.navigateByUrl('main'); 
    }
  }
}
