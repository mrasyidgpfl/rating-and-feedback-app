import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppreciationpageComponent } from './appreciationpage.component';

describe('AppreaciationpageComponent', () => {
  let component: AppreciationpageComponent;
  let fixture: ComponentFixture<AppreciationpageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppreciationpageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppreciationpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
