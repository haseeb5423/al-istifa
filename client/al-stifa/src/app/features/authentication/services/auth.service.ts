import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_BASE_URL } from '@core/constants/api.endpoints';
import { LoginModel } from '@features/authentication/models/login.model';
import { Step1, Step2 } from '@features/authentication/models/register.model';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = API_BASE_URL;
  AuthCheck = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.AuthCheck.asObservable();
  userId = new BehaviorSubject<string>('');
  UserId$ = this.userId.asObservable();
  constructor(private http: HttpClient) {
     this.checkInitialStateLogin();
  }
  checkInitialStateLogin() {
    const token = localStorage.getItem('token');
    const userId = localStorage.getItem('userId');
    if (token && userId) {
      this.AuthCheck.next(true);
      this.userId.next(userId);
    } else {
      this.AuthCheck.next(false);
      this.userId.next('');
    }
  }
  get getUserId(): string {
    return this.userId.value; 
  }
  
  setUserId(id: string) {
    this.userId.next(id);
  }
  login(userCredentials: LoginModel) {
    return this.http
      .post(`${this.baseUrl}/auth/login`, userCredentials).pipe(
        tap ((res:any)=>{
          localStorage.setItem('token', res.token);
          localStorage.setItem('userId', res.user.id);
          this.AuthCheck.next(true);
          this.userId.next(res.user.id);
        }
      ))
  }

  registerStep1(userCredentials : Step1) {
    return this.http.post(`${this.baseUrl}/auth/register/step1`, userCredentials)
  }

  registerStep2(userCredentials : FormData) {
    return this.http.post(`${this.baseUrl}/auth/register/step2`, userCredentials)
  }
}
