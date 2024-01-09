import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpBackend, HttpRequest, HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { NavbarService } from '../navbar.service';


const ROOT_URL = 'https://ecomratingfeedback.azurewebsites.net/api/';

export interface ProjElement {
  member_name: string;
  period: string;
}

interface Hasil {
  hasil: string;
}


@Component({
  selector: 'app-memberi-rating',
  templateUrl: './giv-rat.component.html',
  styleUrls: ['./giv-rat.component.css']
})
export class MemberiRatingComponent implements OnInit {
  dataSource: Observable<ProjElement[]>;
  kembalian: string;
  nama: string;
  customerName: string;
  period: object;
  rating: number;
  id: string;
  ratingDict: object;
  ratingID: string;
  badges: object[];
  badgepicked: object;
  labelfeedback: object[];
  feedback: object[] = [];
  comment: object;
  namaOrang: object[];
  badgesKeys: string[];
  labelfeedbackKeys: string[];
  kembalianKey: string[];
  currentPersonIndex = 0;
  currentPeriodIndex = 0;
  initialPeriodIndex = 0;
  balikPertama = false;
  load = false;
  kode: string;
  gagalSend: boolean;
  month  = {'1' : 'Jan', '2' : 'Feb', '3' : 'Mar', '4' : 'Apr', '5' : 'Mei', '6' : 'Jun',
            '7' : 'Jul', '8' : 'Agu', '9' : 'Sep', '10' : 'Okt', '11' : 'Nov', '12' : 'Des',
            '01' : 'Jan', '02' : 'Feb', '03' : 'Mar', '04' : 'Apr', '05' : 'Mei', '06' : 'Jun',
            '07' : 'Jul', '08' : 'Agu', '09' : 'Sep'};
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    private cdRef: ChangeDetectorRef,
    public nav: NavbarService,
  ) { }

  ngOnInit() {
    this.nav.hide();
    this.getRatingFromDB();
    this.getBadgeAndLabelFeedback();
  }

  getRatingFromDB() {
    this.kode = this.route.snapshot.paramMap.get('kode');
    this.http.get(ROOT_URL + 'validateKode' + '?kode=' + this.kode, { responseType: 'text' })
      .subscribe((res) => {
        this.getAssignmentName(res);
      });
  }

  getAssignmentName(custRatingID) {
    this.http.get(ROOT_URL + 'GetPeriodforProject' + '?KodeRating=' + custRatingID, { responseType: 'text' })
      .subscribe((res) => {
        if (res === '') {
          this.redirectToAppreciation();
        }
        this.kembalian = res;
        this.ratingID = this.kembalian.split('#')[2];
        this.customerName = this.kembalian.split('#')[3];
        this.load = true;
      });
  }

  initiatePeriodIndex(periodIndex: string) {
    this.initialPeriodIndex = parseInt(periodIndex, 10);
    this.currentPeriodIndex = parseInt(periodIndex, 10);
  }

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

  getRatingID(): void {
    this.ratingID = this.kembalian[this.kembalianKey[this.currentPeriodIndex]][this.currentPersonIndex].split(',')[2];
  }

  getStuff() {
    // // console.log(this.rating, this.feedback, this.comment, this.badgepicked);
    // console.log(this.feedback);
  }

  getBadgeAndLabelFeedback() {
    this.http.get(ROOT_URL + 'GetBadgeandLabel?kode=cust', { responseType: 'text' }).subscribe((res) => {
      this.labelfeedback = JSON.parse(res)[0];
      this.labelfeedbackKeys = Object.keys(this.labelfeedback).sort();
      this.badgesKeys = Object.keys(JSON.parse(res)[1]).sort();
      this.badges = JSON.parse(res)[1];
    });
  }

  redirectToAppreciation() {
    this.router.navigateByUrl('thankyou');
  }

  tambahKeFeedback(event) {
    // console.log(event);
    if (this.feedback.includes(event)) {
      this.feedback.splice(this.feedback.indexOf(event), 1);
    } else {
      this.feedback.push(event);
    }
  }

  postIntoDB() {
    const JSONDikirim = {
      rating_id: this.ratingID,
      scale: this.rating,
      comment: this.comment,
      rcf: [],
    };
    if (Number(this.rating) === 6) {
      JSONDikirim.rcf.push(this.badgepicked);
    } else {
      JSONDikirim.rcf = this.feedback;
    }
    this.http.post<Hasil>(ROOT_URL + 'RatingFeedback', JSONDikirim)
      .subscribe((res) => {
        if (res.hasil === 'Sukses') {
          // console.log(res);
          this.gagalSend = false;
          this.redirectToAppreciation();
        } else {
          this.gagalSend = true;
        }
      });
  }
}
