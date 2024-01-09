import { Component } from '@angular/core';
import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { HttpClientModule } from '@angular/common/http';
import { StarRatingModule } from 'angular-star-rating';
import { MatBadgeModule } from '@angular/material';
import { MatPaginatorModule } from '@angular/material';
import { MatChipsModule } from '@angular/material/chips';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MatSelectModule,
        BrowserAnimationsModule,
        MatTableModule,
        MatProgressBarModule,
        MatBadgeModule,
        MatPaginatorModule,
        HttpClientModule,
        StarRatingModule.forRoot(),
        MatChipsModule,
      ],
      declarations: [
        AppComponent,
        MockRouterOutletComponent,
        MockNavBarComponent,
        MockFooterComponent,
      ],
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

});
@Component({
  selector: 'app-nav-bar',
  template: ''
})
class MockNavBarComponent {
}

@Component({
  selector: 'app-footer',
  template: ''
})
class MockFooterComponent {
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'router-outlet',
  template: ''
})
class MockRouterOutletComponent {
}
