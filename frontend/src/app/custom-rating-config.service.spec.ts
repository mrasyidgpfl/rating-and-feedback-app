import { TestBed } from '@angular/core/testing';

import { CustomRatingConfigService } from './custom-rating-config.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('StarRatingConfigService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      CustomRatingConfigService,
    ],
    schemas: [
      NO_ERRORS_SCHEMA,
    ]
  }).compileComponents()
  );

  it('should be created', () => {
    const service: CustomRatingConfigService = TestBed.get(CustomRatingConfigService);
    expect(service).toBeTruthy();
  });
  it('should have 6 stars as one of its properties', () => {
    const service: CustomRatingConfigService = TestBed.get(CustomRatingConfigService);
    expect(service.numOfStars).toEqual(6);
  });
});
