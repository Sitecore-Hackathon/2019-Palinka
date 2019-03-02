import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentActivityPageComponent } from './recent-activity-page.component';

describe('RecentActivityPageComponent', () => {
  let component: RecentActivityPageComponent;
  let fixture: ComponentFixture<RecentActivityPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecentActivityPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecentActivityPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
