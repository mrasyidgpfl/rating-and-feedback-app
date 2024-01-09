import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ViewChild, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatTab } from '@angular/material';
import { Observable } from 'rxjs';
import { DataSource } from '@angular/cdk/table';
import { NavbarService } from '../navbar.service';


const ROOT_URL = 'https://ecomratingfeedback.azurewebsites.net/api/';


export interface ProjectStuff {
  member_name: string;
  period: string;
}

export interface ProjElement {
  assignment_name: string;
  customer_name: string;
  assignment_rating: number;
  my_rating: number;
  state: string;
  project_id: string;
}

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css'],
})
export class HomepageComponent implements OnInit {
  dataSourceTemp: ProjElement[] = [];
  dataSource;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['assignment_name', 'customer_name', 'assignment_rating', 'my_rating', 'state'];
  projectElements: Observable<ProjectStuff[]>;
  name: string;
  isReady = false;

  constructor(
    private http: HttpClient,
    private changeDetector: ChangeDetectorRef,
    private nav: NavbarService,
  ) { }

  ngOnInit() {
    this.nav.show();
    this.changeDetector.detectChanges();
    this.http.post(ROOT_URL + 'HomePage' + '?PersonId=' + 'PID1',
      {}, { responseType: 'text' })
      .subscribe(
        (res) => {
          //// console.log(res);
          const newData = JSON.parse(res);
          for (const data of newData) {
            // if (data.yourRating === 'NaN') {
            //   data.yourRating = 'NaN';
            // }
            this.dataSourceTemp.push(
              {
                assignment_name: data.namaProject,
                customer_name: data.namaClient,
                assignment_rating: data.customerRating,
                my_rating: data.yourRating,
                state: data.active,
                project_id: data.projectId
              }
            );
          }
          this.dataSource = new MatTableDataSource(this.dataSourceTemp);
          this.dataSource.paginator = this.paginator;
          //// console.log(this.paginator);
          this.isReady = true;
        });
  }


  applyFilter(filterValue: string) {
    //// console.log(this.dataSource);
    this.dataSource.filter = filterValue.trim();
  }


  /*getTrueState() {
    for (var i = 0; i < this.dataSource.length; i++) {
      if (this.dataSource['state'] == 'true') {
        this.dataTrue.push ({
            assignment_name: this.dataSource['namaProject'],
            customer_name: this.dataSource['namaClient'],
            assignment_rating: newData[i]['customerRating'],
            my_rating: newData[i]['yourRating'],
            state: newData[i]['active'],
            project_id: this.dataSource['projectId']
        });
      }
    }
  }*/
}

  // getPersonAndPeriod(): string {
  //   this.name = this.route.snapshot.paramMap.get('assignment_name');
  //   this.projectElements = this.http.get<ProjectStuff[]>(ROOT_URL+'GetPersonbyPeriod/' + name)
  //   //// console.log(this.dataSource);
  //   return this.dataSource.toString();
  // }

