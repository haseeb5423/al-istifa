import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingSpinnerService {

   private loadingSpinner = new BehaviorSubject<boolean>(false)
  loadingSpinner$ = this.loadingSpinner.asObservable()
  constructor() { }

  show(){
    this.loadingSpinner.next(true)
  }
  hide(){
    this.loadingSpinner.next(false)
  }
}
