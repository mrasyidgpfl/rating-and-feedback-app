import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailpageComponent } from './detailpage.component';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule } from '@angular/router';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressBarModule} from '@angular/material/progress-bar';
import { HttpClientModule } from '@angular/common/http';
import { StarRatingModule } from 'angular-star-rating';
import { MatBadgeModule } from '@angular/material';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule } from '@angular/material/paginator';

describe('DetailpageComponentBeforeRequest', () => {
  let component: DetailpageComponent;
  let fixture: ComponentFixture<DetailpageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports : [
        MatSelectModule,
        BrowserAnimationsModule,
        RouterModule.forRoot([]),
        MatDialogModule,
        MatTableModule,
        MatProgressBarModule,
        MatPaginatorModule,
        HttpClientModule,
        StarRatingModule.forRoot(),
        MatBadgeModule,
        MatChipsModule
      ],
      declarations: [ DetailpageComponent ]
    })
    .compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(DetailpageComponent);
    component = fixture.componentInstance;
    component.isReady = false;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render Loading', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('p').textContent).toContain('Loading...');
  });

});

xdescribe('DetailpageComponentAfterRequest', () => {
  let component: DetailpageComponent;
  let fixture: ComponentFixture<DetailpageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports : [
        MatSelectModule,
        BrowserAnimationsModule,
        MatTableModule,
        MatProgressBarModule,
        MatPaginatorModule,
        HttpClientModule,
        StarRatingModule.forRoot(),
        MatChipsModule
      ],
      declarations: [ DetailpageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailpageComponent);
    component = fixture.componentInstance;
    component.isReady = true;
    fixture.detectChanges();
  });

  it('should render period name in a th tag', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelectorAll('th')[0].textContent).toContain('Period Name');
  });

  it('should render data range in a th tag', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelectorAll('th')[1].textContent).toContain('Date Range');
  });

  it('should render rating in a th tag', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelectorAll('th')[2].textContent).toContain('Rating');
  });

  it('should render feedback in a th tag', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelectorAll('th')[3].textContent).toContain('Feedback');
  });

  it('should render My Average Rating in a star-rating tag', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelectorAll('star-rating')[0].textContent).toContain('My Average Rating');
  });

  it('should render My Average Rating in a star-rating tag with 0 stars at beginning', () => {
    expect(component.myRating === 0);
  });
});
