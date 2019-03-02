import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LockedItemPageComponent } from './locked-item-page.component';

describe('LockedItemPageComponent', () => {
  let component: LockedItemPageComponent;
  let fixture: ComponentFixture<LockedItemPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LockedItemPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LockedItemPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
