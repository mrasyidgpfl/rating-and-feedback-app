import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatTableModule } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

const ROOT_URL = 'https://ecomratingfeedback.azurewebsites.net/api/';

interface TableElements {
  ratingList;
  criticismCategoryCount;
  pidDictionary;
}

export interface TableComponent {
  period: string;
  name: string[];
  rating: string[];
  most_feedback: string[];
  pidDict: {};
}


export interface StatisticsData {
  label: string;
  value: number;
}

@Component({
  selector: 'app-view-report-table',
  templateUrl: './view-report-table.component.html',
  styleUrls: ['./view-report-table.component.css']
})

export class ViewReportTableComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];
  hasil: TableElements[];
  details;
  idCust: string;
  projId: string;
  ELEMENT_DATA: TableComponent[] = [];
  STATISTIC_DATA: StatisticsData[] = [];
  dataSource = new MatTableDataSource<TableComponent>();
  dataSourceStatistik;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  isReady = false;

  constructor(public http: HttpClient, private route: ActivatedRoute
    ) {
    this.dataSourceStatistik = {
      chart: {
        caption: 'Feedback Statistics : ',
        subCaption: '',
        xAxisName: 'Periods',
        yAxisName: 'Average Ratings',
        numberSuffix: '',
        theme: 'fusion',
        usePlotGradientColor: '0',
        palettecolors: '0047B9'
      },
      // Chart Data
    };
  }

  ngOnInit() {
    this.getDataFromDB();
    this.getDetailsFromDB();
    this.getStatisticsData();
  }

  getDetailsFromDB() {
    this.projId = this.route.snapshot.paramMap.get('proj_id');
    // // console.log(projId);
    if (this.projId === null) {
      this.projId = 'P1';
    }
    this.http.get(ROOT_URL + 'ProjectDetails?ProjectId=' + this.projId)
      .subscribe((res) => {
        // console.log(res);
        this.details = res;
        this.idCust = this.details[3];
      });
  }

  getStatisticsData() {
    this.projId = this.route.snapshot.paramMap.get('proj_id');
    if (this.projId === null) {
      this.projId = 'P1';
    }
    this.http.post<TableElements[]>(ROOT_URL + 'ManagerReportBreakdown?ProjectId=' + this.projId, {})
      .subscribe((res) => {
        this.hasil = res;
        let counter = 1;

        for (var i = 0; i < res.length; i++) {
          var nama1 = res[i].ratingList;
          let jumlahValue = 0;
          var namaArray = Object.keys(nama1);
          var ratingArray = [];
          for (const nama of namaArray) {
            jumlahValue += nama1[nama];
          }
          jumlahValue /= namaArray.length;
          this.STATISTIC_DATA.push(
            {
              label: "Period " + counter,
              value: jumlahValue,
            });
          counter += 1;
        }
        this.dataSourceStatistik.data = this.STATISTIC_DATA;
        ;
      });
  }

  getDataFromDB() {
    this.projId = this.route.snapshot.paramMap.get('proj_id');
    if (this.projId === null) {
      this.projId = 'P1';
    }
    this.http.post<TableElements[]>(ROOT_URL + 'ManagerReportBreakdown?ProjectId=' + this.projId, {})
      .subscribe((res) => {
        this.hasil = res;
        // console.log(res);
        let counter = res.length - 1;
        for (var i = res.length; i > 0; i--) {
          var nama1 = res[counter].ratingList;
          var nama2 = res[counter].pidDictionary;
          var curr = res[counter].criticismCategoryCount;
          var namaArray = Object.keys(nama1);
          var pidArray = Object.keys(nama2);
          var ratingArray = [];
          for (const nama of namaArray) {
            ratingArray.push(parseFloat(nama1[nama]).toFixed(2));  
          }
          var keyss = Object.keys(curr);
          var valuess = Object.values(curr);
          var arrRes = [];
          let max = valuess[0];
          for (let i = 1; i < valuess.length; i+=1)
          {
            if (valuess[i] > max) {
              max = valuess[i];
            }
          }
          for (let j = 0; j < valuess.length; j += 1) {
            if (max === valuess[j]) {
              arrRes.push(keyss[j]);
            }
          }

          this.ELEMENT_DATA.push(
            {
              period: "Period " + (counter + 1),
              name: namaArray,
              rating: ratingArray,
              most_feedback: arrRes.sort(),
              pidDict: nama2,
            }
          );
          counter -= 1;
        }
        
        this.dataSource = new MatTableDataSource(this.ELEMENT_DATA);
        this.dataSource.paginator = this.paginator;
        this.isReady = true;
      });
  }

}

