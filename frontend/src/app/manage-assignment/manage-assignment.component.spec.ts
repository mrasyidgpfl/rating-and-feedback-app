import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageAssignmentComponent } from './manage-assignment.component';

describe('ManageAssignmentComponent', () => {
  let component: ManageAssignmentComponent;
  let fixture: ComponentFixture<ManageAssignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageAssignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
