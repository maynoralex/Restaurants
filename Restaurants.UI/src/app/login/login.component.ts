import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { LoginRequest } from '../models/login-request';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
   credentials: LoginRequest = {
      email: '',
      password: ''
    };
  
  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
    this.isLoggedIn();
  }
  
  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  login() {
    

    this.authService.login(this.credentials).subscribe(
      response => {
        console.log('Login successful: ', response);
      });
  } 
    
}
