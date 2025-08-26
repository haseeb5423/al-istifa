import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '@core/constants/api.endpoints';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  baseUrl = API_BASE_URL
  private http = inject(HttpClient);
  constructor( ) { }

  getProfile(id: string) {
    return this.http.get(`${this.baseUrl}/profile/${id}`);
  }
  updateProfile(id: string, formData: FormData) {
    return this.http.patch(`${this.baseUrl}/profile/${id}`, formData);
  }
}
