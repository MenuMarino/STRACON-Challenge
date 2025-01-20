import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColocadorLandingComponent } from './colocador-landing.component';

describe('ColocadorLandingComponent', () => {
  let component: ColocadorLandingComponent;
  let fixture: ComponentFixture<ColocadorLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColocadorLandingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColocadorLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
