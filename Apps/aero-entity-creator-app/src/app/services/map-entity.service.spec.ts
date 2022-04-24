import { TestBed } from '@angular/core/testing';

import { MapEntityService } from './map-entity.service';

describe('MapEntityService', () => {
  let service: MapEntityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MapEntityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
