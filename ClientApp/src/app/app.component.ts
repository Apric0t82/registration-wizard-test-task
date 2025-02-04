import { Component } from '@angular/core';
//import { RouterOutlet } from '@angular/router';
import { MainComponent } from './components/main/main.component';
//import { RegisterComponent } from './components/register/register.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [MainComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Registration Wizard';
}
