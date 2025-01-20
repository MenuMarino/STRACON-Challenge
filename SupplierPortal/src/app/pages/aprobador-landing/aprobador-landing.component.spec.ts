import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobadorLandingComponent } from './aprobador-landing.component';

describe('AprobadorLandingComponent', () => {
  let component: AprobadorLandingComponent;
  let fixture: ComponentFixture<AprobadorLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AprobadorLandingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AprobadorLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
