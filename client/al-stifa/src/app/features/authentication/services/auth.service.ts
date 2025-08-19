import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_BASE_URL } from '@core/constants/api.endpoints';
import { LoginModel } from '@features/authentication/models/login.model';
import { Step1, Step2 } from '@features/authentication/models/register.model';
import { BehaviorSubject, Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = API_BASE_URL;
  AuthCheck = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.AuthCheck.asObservable();
  userId = new BehaviorSubject<string>('');
  // UserId$ = this.userId.asObservable();
  constructor(private http: HttpClient) {}

  get getUserId(): string {
    return this.userId.value; // expose current value
  }
  
  setUserId(id: string) {
    this.userId.next(id);
  }
  login(userCredentials: LoginModel) {
    return this.http
      .post(`${this.baseUrl}/auth/login`, userCredentials).pipe(
        tap ((res:any)=>{
          localStorage.setItem('token', res.token);
          this.AuthCheck.next(true);
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
