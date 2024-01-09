import { Component, OnInit, ChangeDetectorRef, Inject, Pipe, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { share } from 'rxjs/operators';
import { MatPaginator, MatTableDataSource, MAT_DIALOG_DATA } from '@angular/material';
import {} from 'angular-star-rating';
import { NavbarService } from '../navbar.service';
import { MatDialog, MatDialogRef } from '@angular/material';
import { ActivatedRoute } from '@angular/router';


export interface Feedbacks {
  name: string;
  date: string;
  rating: string;
  feedback: string;
  progressBar: string;
}

var FEEDBACK_DATA: { [name : string]: Feedbacks } = {};

@Component({
  selector: 'app-detailpage',
  templateUrl: './detailpage.component.html',
  styleUrls: ['./detailpage.component.css']
})

export class DetailpageComponent implements OnInit {
  displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];
  dataSource : Feedbacks[] = [];
  month  = {"1" : "Jan", "2" : "Feb", "3" : "Mar", "4" : "Apr", "5" : "Mei", "6" : "Jun",
            "7" : "Jul", "8" : "Agu", "9" : "Sep", "10" : "Okt", "11" : "Nov", "12" : "Des",
            "01" : "Jan", "02" : "Feb", "03" : "Mar", "04" : "Apr", "05" : "Mei", "06" : "Jun",
            "07" : "Jul", "08" : "Agu", "09" : "Sep",};
  dataSourceFinal;
  badgeSource = [];
  myRating = 0;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private nav: NavbarService,
    public dialog: MatDialog,
    ){}

  baseUrl = 'https://ecomratingfeedback.azurewebsites.net/';
  strOut = '';
  json = null;
  projectID = this.route.snapshot.paramMap.get('projectId') ? this.route.snapshot.paramMap.get('projectId') : 'P1';
  personID = this.route.snapshot.paramMap.get('personId') ? this.route.snapshot.paramMap.get('personId') : 'PID1';
  value = '25';
  projectName = '';
  clientName  = '';
  tableIsReady = false;
  badgeIsReady = false;
  starIsReady = false;
  isReady: boolean = this.tableIsReady && this.badgeIsReady && this.starIsReady;

  displayModal(feedback) {
    const dialogRef = this.dialog.open(DetailpageDialog, {
      width: '751px',
      data: feedback
    });

    dialogRef.afterClosed().subscribe(result => {
      // console.log('The dialog was closed');
    });
  }

  ngOnInit() {
    this.nav.show();
    this.get_feedback();
    this.get_badge();
    this.get_stars();
    this.get_client();
  }

  extractDate(date) {
    date = date.split("/");
    var tmp = date[0];
    date[0] = date[1];
    date[1] = tmp;
    date[1] = this.month[date[1]];
    date = date.join(" ");
    return date;
  }

  get_feedback(){
    this.httpClient.post(this.baseUrl + 'api/RekapitulasiSprint?ProjectID=' + this.projectID + 
    '&PersonID=' + this.personID, {"key" : "value"}, {responseType : 'text'}).pipe(share()).subscribe(res => {
        this.json = JSON.parse(res);
        for(var i = 0; i < this.json.length; i++) {
          if (this.json[i] == null) {
            var feed = {name: "Period " + (i+1).toString(), date: "empty", rating : "0", ratingNum : "empty", 
                        isCompleted : false, feedback : "", feedback_comment : [], progressBar : "0"};
            FEEDBACK_DATA[i.toString()] = feed;
          } else {
            var rate: string = parseFloat(this.json[i]["averageRating"]).toFixed(2).toString();
            var commentlist = this.json[i]["commentaryList"] ? this.json[i]["commentaryList"] : null;
            var startDate = this.extractDate(this.json[i]["sprintStart"]);
            var endDate = this.extractDate(this.json[i]["sprintEnd"]);
            var expectedNumOfRating = this.json[i]["expectedNumberOfRating"];
            var numOfRating = this.json[i]["numberOfRating"];
            if(expectedNumOfRating == numOfRating && expectedNumOfRating != 0) {
              var ratingNum = "Completed";
              var isCompleted = true;
            } else {
              var ratingNum = numOfRating + " of " + expectedNumOfRating;
              var isCompleted = false;
            }
            var date = startDate + " - " + endDate;
            var comments = "";
            var commentsNum = "";
            var feedback_comment = [];
            if(rate != "6.00") {
              for(var keys in commentlist) {
                comments += keys + ";";
              }
            }
            for(var value in commentlist) {
              commentsNum += commentlist[value].length + ";";
              for(var feedback in commentlist[value]) {
                if(commentlist[value][feedback]) {
                  feedback_comment.push(commentlist[value][feedback]);
                }
              }
            }
            comments = comments.substr(0,comments.length-1)
            var feed = {name: "Period " + (i+1).toString(), date: date, rating : rate, ratingNum : ratingNum,
                        isCompleted : isCompleted, feedback : comments, feedback_comment : feedback_comment, progressBar : commentsNum};
            FEEDBACK_DATA[i.toString()] = feed;
          }
        }
        for(var key in FEEDBACK_DATA) {
          this.dataSource.push(FEEDBACK_DATA[key]);
        }
        this.dataSource.reverse();
        this.dataSourceFinal = new MatTableDataSource(this.dataSource);
        this.dataSourceFinal.paginator = this.paginator;
        this.tableIsReady = true;
        this.isReady = this.tableIsReady && this.badgeIsReady && this.starIsReady;
    });
  }

  get_badge() {
    this.httpClient.post(this.baseUrl + 'api/BadgeTabulation?ProjectID=' + this.projectID +
    '&PersonID=' + this.personID, {'key' : 'value'}, {responseType : 'text'}).pipe(share()).subscribe(res => {
      const source = JSON.parse(res);
      source.splice(6);
      this.badgeSource = source;
      for(var i = 0; i < this.badgeSource.length; i++) {
        if(this.badgeSource[i][0] === "") this.badgeSource[i][0] = "https://imagizer.imageshack.com/v2/304x388q90/r/923/CgAsHT.png";
      }
      this.badgeIsReady = true;
      this.isReady = this.tableIsReady && this.badgeIsReady && this.starIsReady;
    });
  }

  get_stars(){
    this.httpClient.post(this.baseUrl + 'api/TotalTabulation?ProjectID=' + this.projectID + 
    '&PersonID=' + this.personID, {"key" : "value"}, {responseType : 'text'}).pipe(share()).subscribe(res => {
      var source = JSON.parse(res);
      this.myRating = source["personalAverage"]
      this.projectName = source["name"]
      this.starIsReady = true;
      this.isReady = this.tableIsReady && this.badgeIsReady && this.starIsReady;
    });
  }
  get_client() {
    this.httpClient.post(this.baseUrl + 'api/ProjectDetails?ProjectId=' + this.projectID, 
    {'key' : 'value'}, {responseType : 'text'}).pipe(share()).subscribe(res => {
      const hasil = JSON.parse(res);
      this.clientName = hasil[1];
    });
  }
}

@Component({
  selector: 'app-detailpage',
  templateUrl: './detailpage-modal.component.html',
  styleUrls: ['./detailpage.component.css']
})
export class DetailpageDialog {

  constructor(
    public dialogRef: MatDialogRef<DetailpageDialog>,
    @Inject(MAT_DIALOG_DATA) public data : any
  ){}

  onNoClick(): void {
    this.dialogRef.close();
  }

}
