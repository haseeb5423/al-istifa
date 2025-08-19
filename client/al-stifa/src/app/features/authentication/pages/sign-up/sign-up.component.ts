import { Component } from '@angular/core';
import { Step1Component } from "../../components/sign-up-steps/step1/step1.component";
import { Step2Component } from '../../components/sign-up-steps/step2/step2.component';

@Component({
  selector: 'app-sign-up',
  imports: [Step1Component],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.scss'
})
export class SignUpComponent {

  
}
