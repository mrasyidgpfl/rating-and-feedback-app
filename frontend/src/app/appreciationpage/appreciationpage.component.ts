import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../navbar.service';

@Component({
  selector: 'app-appreciationpage',
  templateUrl: './appreciationpage.component.html',
  styleUrls: ['./appreciationpage.component.css']
})
export class AppreciationpageComponent implements OnInit {

  constructor(
    private nav: NavbarService,
  ) { }

  ngOnInit() {
    this.nav.hide();
  }

}
