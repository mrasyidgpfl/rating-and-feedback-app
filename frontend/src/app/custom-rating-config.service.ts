import { Injectable } from '@angular/core';
import { StarRatingConfigService } from 'angular-star-rating';

@Injectable()
export class CustomRatingConfigService extends StarRatingConfigService {

  constructor() {
    super();
    this.numOfStars = 6;
    this.staticColor = 'ok';
   }
}
