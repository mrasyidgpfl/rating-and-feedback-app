import { Component, OnInit, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NavbarService } from '../navbar.service';
import { MatButtonToggleGroup, MatButtonToggle } from '@angular/material';


const ROOT_URL = 'https://ecomratingfeedback.azurewebsites.net/api/';

export interface Rating {
  'rating_id': object;
  'scale': number;
  'comment': object;
  'rcf': object[];
}

interface Hasil {
  'hasil': string;
}


@Component({
  selector: 'app-memberi-feedback',
  templateUrl: './memberi-feedback.component.html',
  styleUrls: ['./memberi-feedback.component.css']
})
export class MemberiFeedbackComponent implements OnInit {
  kembalian: object;
  nama: string;
  rating: number;
  id: string;
  ratingDict: object;
  ratingID: object;
  badges: object[];
  badgepicked: object;
  labelfeedback: object[];
  feedback: object[] = [];
  comment: object;
  badgesKeys: string[];
  labelfeedbackKeys: string[];
  kembalianKey: string[];
  currentPersonIndex = 0;
  currentPeriodIndex = 0;
  initialPeriodIndex = 0;
  balikPertama = false;
  gagalSend = false;
  @ViewChild(MatButtonToggleGroup) group: MatButtonToggleGroup;
  @ViewChildren(MatButtonToggle) toggles: QueryList<MatButtonToggle>;
  month  = {'1' : 'Jan', '2' : 'Feb', '3' : 'Mar', '4' : 'Apr', '5' : 'Mei', '6' : 'Jun',
            '7' : 'Jul', '8' : 'Agu', '9' : 'Sep', '10' : 'Okt', '11' : 'Nov', '12' : 'Des',
            '01' : 'Jan', '02' : 'Feb', '03' : 'Mar', '04' : 'Apr', '05' : 'Mei', '06' : 'Jun',
            '07' : 'Jul', '08' : 'Agu', '09' : 'Sep'};
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    public nav: NavbarService,
  ) { }

  ngOnInit() {
    this.nav.show();
    this.getAssignment();
    this.getBadgeAndLabelFeedback();
  }

  /*ngAfterContentChecked() {
    setTimeout(() => {
      this.toggles.forEach(toggle => toggle.buttonToggleGroup = this.group);
    });
  }*/

  extractDate(dateRange) {
    const dates = dateRange.split(' - ');
    const resDates = [];
    let newDate = '';
    for (const date of dates) {
      const splittedDate = date.split('/');
      let tmp = splittedDate[0];
      tmp = splittedDate[1];
      splittedDate[1] = splittedDate[0];
      splittedDate[0] = tmp;
      splittedDate[1] = this.month[splittedDate[1]];
      newDate = splittedDate.join(' ');
      resDates.push(newDate);
    }
    const changedDate = resDates.join(' - ');
    return changedDate;
  }

  getAssignment(): void {
    this.id = this.route.snapshot.paramMap.get('assignment_id');
    this.nama = this.route.snapshot.paramMap.get('assignment_name');
    this.http.get(ROOT_URL + 'GetPersonbyPeriod' + '?proyekName=' + this.id,
      { responseType: 'text' }).subscribe((res) => {
        // console.log(res);
        if (res === '{}') {
          this.router.navigateByUrl('');
        }
        this.kembalian = JSON.parse(res);
        this.kembalianKey = Object.keys(this.kembalian);
        this.initiatePeriodIndex('0');
      });
  }

  initiatePeriodIndex(periodIndex: string) {
    this.initialPeriodIndex = parseInt(periodIndex, 10);
    this.currentPeriodIndex = parseInt(periodIndex, 10);
  }

  getRatingID(): void {
    this.ratingID = this.kembalian[this.kembalianKey[this.currentPeriodIndex]]
    [this.currentPersonIndex].split(',')[2];
  }

  getStuff() {
    // // console.log(this.rating, this.comment, this.feedback, this.badgepicked);
    // console.log(this.feedback);
  }

  getBadgeAndLabelFeedback() {
    this.http.get(ROOT_URL + 'GetBadgeandLabel?kode=dev', { responseType: 'text' })
      .subscribe((res) => {
        this.labelfeedback = JSON.parse(res)[0];
        this.labelfeedbackKeys = Object.keys(this.labelfeedback).sort();
        this.badgesKeys = Object.keys(JSON.parse(res)[1]).sort();
        this.badges = JSON.parse(res)[1];
      });
  }

  resetValues() {
    this.rating = 0;
    this.feedback = [];
    this.badgepicked = null;
    this.comment = null;
  }

  incrementPersonIndex() {
    this.resetValues();
    this.currentPersonIndex += 1;
    if (this.currentPersonIndex > (this.kembalian[this.kembalianKey[this.currentPeriodIndex]]
      .length - 1)) {
      this.incrementPeriodIndex();
    }
  }

  tambahKeFeedback(event) {
    // console.log(event);
    if (this.feedback.includes(event)) {
      this.feedback.splice(this.feedback.indexOf(event), 1);
    } else {
      this.feedback.push(event);
    }
  }

  incrementPeriodIndex() {
    this.currentPeriodIndex += 1;
    this.currentPersonIndex = 0;
    if (this.currentPeriodIndex > (this.kembalianKey.length - 1)) {
      this.currentPeriodIndex = 0;
      this.balikPertama = true;
    }
    if ((this.currentPeriodIndex === this.initialPeriodIndex) && this.balikPertama) {
      this.router.navigateByUrl('');
    }
  }
  postIntoDB() {
    const JSONDikirim: Rating = {
      rating_id: this.ratingID,
      scale: this.rating,
      comment: this.comment,
      rcf: []
    };
    if (Number(this.rating) === 6) {
      JSONDikirim.rcf.push(this.badgepicked);
      this.http.post<Hasil>(ROOT_URL + 'RatingFeedback', JSONDikirim).subscribe(
        (res) => {
          if (res.hasil === 'Sukses') {
            this.gagalSend = false;
            this.incrementPersonIndex();
          } else {
            this.gagalSend = true;
          }
      });
    } else {
      JSONDikirim.rcf = this.feedback;
      this.http.post<Hasil>(ROOT_URL + 'RatingFeedback', JSONDikirim).subscribe(
        (res) => {
          if (res.hasil === 'Sukses') {
            this.gagalSend = false;
            // // console.log(res);
            this.incrementPersonIndex();
          } else {
            // // console.log(res);
            this.gagalSend = true;
          }
        });
    }
  }
}
