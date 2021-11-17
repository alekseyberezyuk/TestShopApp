import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor( private toastr: ToastrService) {

  }

  btnClicked() {
    this.toastr.success('Hello world!', 'Toastr fun!', {
      positionClass: 'toast-top-center',
      closeButton: true
    });
  }

  ngOnInit(): void {
  }
}
