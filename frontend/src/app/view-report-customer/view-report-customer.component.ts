import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { Title } from '@angular/platform-browser';
import { ViewReportCustomerService } from '../services/view-report-customer/view-report-customer.service';
import { element } from '@angular/core/src/render3';
import { async } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

interface TableElements {
  ratingList;
  criticismCategoryCount;
}


export interface TableComponent {
  period: string;
  name: string[];
  rating: string[];
  most_feedback: string;
}

export interface TableValues {
  period: string;
  daterange: string;
  name: string;
  feedbacks: string[];
  badge: {
    link: string,
    tooltip: string,
  };
  comment: string;
}

export interface StatisticsData {
  label: string;
  value: number;
}

@Component({
  selector: 'app-view-report-customer',
  templateUrl: './view-report-customer.component.html',
  styleUrls: ['./view-report-customer.component.scss']
})

export class ViewReportCustomerComponent implements OnInit {
  month  = {1 : 'Jan', 2 : 'Feb', 3 : 'Mar', 4 : 'Apr', 5 : 'Mei', 6 : 'Jun',
            7 : 'Jul', 8 : 'Agu', 9 : 'Sep', 10 : 'Okt', 11 : 'Nov', 12 : 'Des'};
  title = 'View report customer component';
  displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];
  displayedColumns2: string[] = ['period', 'daterange', 'name', 'badge', 'feedbacks', 'comment'];
  hasil: TableElements[];
  namaProject: string;
  namaCust: string;
  dummyCustomer: TableValues[] = [
    // {period: '1', daterange: '12 Jan 2019 - 19 Jan 2019', name: 'Cust 1', badge: '',
    //   feedbacks: ['ayy', 'ayy'], comment: 'nice'},
    // {period: '2', daterange: '19 Jan 2019 - 26 Jan 2019', name: 'Cust 1', badge: 'asdf',
    //   feedbacks: [], comment: 'nice'},
    // {period: '3', daterange: '26 Jan 2019 - 1 Feb 2019', name: 'Cust 1', badge: 'asdf',
    //   feedbacks: [], comment: 'nice'},
    // {period: '4', daterange: '1 Feb 2019 - 8 Feb 2019', name: 'Cust 1', badge: '',
    //   feedbacks: ['lmao', 'lmao'], comment: 'nice'},
  ];
  dataSource2;
  dummyRating: string[] = [
    '5', '6', '6', '4'
  ];
  details;
  ELEMENT_DATA: TableComponent[] = [];
  STATISTIC_DATA: StatisticsData[] = [];
  dataSource;
  dataSourceStatistik;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  isReadyStatistik = false;
  isReady = false;

  kosong = false;

  constructor(
    public http: HttpClient,
    private titleService: Title,
    private customerReportHttp: ViewReportCustomerService,
    private cdRef: ChangeDetectorRef,
    private route: ActivatedRoute,
    ) {
    this.dataSource = new MatTableDataSource<TableValues>();
    this.dataSourceStatistik = {
      chart: {
        caption: 'Rating Statistics : ',
        subCaption: '',
        xAxisName: 'Periods',
        yAxisName: 'Rating',
        numberSuffix: '',
        theme: 'fusion',
        usePlotGradientColor: '0',
        palettecolors: '0047B9'
      },
    };
  }

  ngOnInit() {
    this.getTableData();
    // this.seedingDummy();
  }
  extractDate(dateRange) {
    const dates = dateRange.split(' - ');
    const resDates = [];
    let newDate = '';
    for (const date of dates) {
      const splittedDate = date.split('/');
      let tmp = splittedDate[0];
      splittedDate[0] = this.month[tmp];
      tmp = splittedDate[1];
      splittedDate[1] = splittedDate[0];
      splittedDate[0] = tmp;
      newDate = splittedDate.join(' ');
      resDates.push(newDate);
    }
    const changedDate = resDates.join(' - ');
    return changedDate;
  }

  async getTableData() {
    const custId = this.route.snapshot.paramMap.get('customer_id');
    this.customerReportHttp.getTableValues(custId)
      .subscribe(
       async (res) => {
          const keysProject = Object.values(res[0]);
          const perProject = res[1];
          const totalVals: TableValues[] = [];
          const tempStatistik = [];
          let dataStatistik = {};
          let badgee = {};
          let label = [];
          for (const data of keysProject) {
            // console.log(data);
            if (data.split('#')[5] === 'kosong') {
              dataStatistik = {
                label: data.split('#')[2],
                value: 0,
              };
              tempStatistik.push(dataStatistik);
            } else {
              const splittedData = data.split('#');
              this.namaProject = splittedData[0];
              this.namaCust = splittedData[1];
              const dataFeedbackorBadge: string[] = perProject[splittedData[2]].split('#');
              if (dataFeedbackorBadge[dataFeedbackorBadge.length - 1] === '') {
                dataFeedbackorBadge.pop();
              }
              const checkBadge = (dataFeedbackorBadge.length === 2 &&
                dataFeedbackorBadge[1].includes('http'));
              if (checkBadge) {
                badgee = dataFeedbackorBadge;
              } else {
                label = dataFeedbackorBadge;
              }

              const tempVal: TableValues = {
                period: splittedData[2],
                daterange: this.extractDate(splittedData[3]),
                name: splittedData[1],
                comment: splittedData[5],
                badge: {tooltip: badgee[0], link: badgee[1]},
                feedbacks: label,
              };

              const dataStatistik = {
                label: splittedData[2],
                value: Number(splittedData[4]),
              };
              tempStatistik.push(dataStatistik);
              totalVals.push(tempVal);
            }
          }
          this.dataSource.data = totalVals;
          this.dataSource.paginator = this.paginator;
          this.dataSourceStatistik.data = tempStatistik;
          this.isReadyStatistik = true;
          this.isReady = true;
        });
  }

  /*getBadgesAndLabels(period: string, tableVals: TableValues, totalTable: TableValues[]) {
    this.customerReportHttp.getBadgesAndLabels('C1')
      .subscribe(
        (res) => {
          const feedbacksAndBadges = res[period];
          let splittedFeedbacksBadges = feedbacksAndBadges.split('#');
          splittedFeedbacksBadges = splittedFeedbacksBadges.filter((el) => {
            return el !== '';
          });
          const checkBadge = (splittedFeedbacksBadges.length === 2 &&
            splittedFeedbacksBadges[1].includes('http'));
          if (checkBadge) {
            tableVals.badge = splittedFeedbacksBadges;
          } else {
            tableVals.feedbacks = splittedFeedbacksBadges;
          }
          totalTable.push(tableVals);
          // for (const data of res) {
          //   const splittedData = data.split('#');
          //   // console.log(splittedData);
          // }
      });
  }*/

  seedingDummy() {
    const newData = [
      {
        label: 'Period ' + this.dummyCustomer[0].period,
        value: this.dummyRating[0]
      },

      // {
      //   label: 'Period ' + this.dummyCustomer[1].period,
      //   value: this.dummyRating[1]
      // },

      // {
      //   label: 'Period ' + this.dummyCustomer[2].period,
      //   value: this.dummyRating[2]
      // },

      // {
      //   label: 'Period ' + this.dummyCustomer[3].period,
      //   value: this.dummyRating[3]
      // }
    ];
    this.dataSourceStatistik.data = newData;
    this.dataSource.data = this.dummyCustomer;
    // console.log(this.dataSource.data.slice());
    this.dataSource.paginator = this.paginator;
    this.isReadyStatistik = true;
    this.isReady = true;
  }

  getDetailsFromDB() {
  }

  getStatisticsData() {
  }

  getCustomerDataFromDB() {
  }

  getDataFromDB() {
  }
}
