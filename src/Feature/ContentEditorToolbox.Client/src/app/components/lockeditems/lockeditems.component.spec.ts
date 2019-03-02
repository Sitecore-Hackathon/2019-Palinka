import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LockeditemsComponent } from './lockeditems.component';

describe('LockeditemsComponent', () => {
  let component: LockeditemsComponent;
  let fixture: ComponentFixture<LockeditemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LockeditemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LockeditemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
