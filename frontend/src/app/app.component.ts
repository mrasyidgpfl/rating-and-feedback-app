import { Component } from '@angular/core';
import { NavbarService } from './navbar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Welcome to Test Apps';

  constructor(public nav: NavbarService) {
    this.nav.show();
  }

}
