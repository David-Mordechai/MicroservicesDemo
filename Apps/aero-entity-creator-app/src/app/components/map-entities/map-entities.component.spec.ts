import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MapEntitiesComponent } from './map-entities.component';

describe('MapEntitiesComponent', () => {
  let component: MapEntitiesComponent;
  let fixture: ComponentFixture<MapEntitiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MapEntitiesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MapEntitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
