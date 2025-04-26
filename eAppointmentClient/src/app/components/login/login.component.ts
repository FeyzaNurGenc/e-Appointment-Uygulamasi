import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { LoginModel } from '../../models/login.model';
import { FormValidateDirective } from 'form-validate-angular';
import { HttpService } from '../../services/http.service';
import { LoginResponseModel } from '../../models/login-response.model';
import { Route, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, FormValidateDirective ,CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  login: LoginModel = new LoginModel();
  userNameOrEmail: string = ''; // userNameOrEmail değişkeni burada tanımlandı
  @ViewChild('password') password: ElementRef<HTMLInputElement> | undefined;

  constructor(private http: HttpService, private router: Router) {}

  showOrHidePassword() {
    if (this.password === undefined) return;

    if (this.password.nativeElement.type === 'password') {
      this.password.nativeElement.type = 'text';
    } else {
      this.password.nativeElement.type = 'password';
    }
  }

  SignIn(form: NgForm) {
    if (form.valid) {
      this.http.post<LoginResponseModel>(
        'Auth/Login', // API URL
        this.login, // Gönderilen veri
        (res) => {
          // Başarı durumunda çalışacak fonksiyon
          localStorage.setItem('token', res.data!.token); // Token'ı localStorage'a kaydet
          this.router.navigateByUrl('/'); // Anasayfaya yönlendir
        },
        (err) => {
          // Hata durumunda çalışacak fonksiyon
          console.error('Giriş başarısız:', err.message); // Hata mesajını konsola yazdır
        }
      );
    }
  }

  /*  SignIn(form: NgForm) {
    if (form.valid) {
      this.http.post("Auth/Login", this.login, (res)=>{

      })
    }
  }*/
}
