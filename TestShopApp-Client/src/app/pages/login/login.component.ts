import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthResponse } from 'src/app/models/authResponse';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  creds = {
    username: '',
    password: ''
  };
 
  constructor(private toastr: ToastrService, private authService: AuthService) {
  }
  
  getUserName(event) {
     this.creds.username = event.target.value;
  }
  
  getUserPassword(event) {
    this.creds.password = event.target.value;
  }

  btnClicked() {
    this.authService.authenticate(this.creds).subscribe({
      next: data => {
        const authResponse: AuthResponse = data;

        if(authResponse.isSuccess) {
          console.log(authResponse.token);
          this.toastr.success('Success', '', {
            positionClass: 'toast-top-center',
            closeButton: true
          });
        } else {
          this.toastr.warning('Invalid username or password', '', {
            positionClass: 'toast-top-center',
            closeButton: true
          });
        }
      },
      error: (error) => console.error(error)
    });
  }
  
  ngOnInit(): void {
  }
}
