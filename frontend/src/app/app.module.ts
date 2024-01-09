import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MemberiRatingComponent } from './memberi-rating/giv-rat.component';
import { AppreciationpageComponent } from './appreciationpage/appreciationpage.component';
import { AppComponent } from './app.component';
import { HomepageComponent } from './homepage/homepage.component';
import { StarRatingModule, StarRatingConfigService } from 'angular-star-rating';
import { AppRoutingModule } from './app-routing.module';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { MemberiFeedbackComponent } from './memberi-feedback/memberi-feedback.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { FooterComponent } from './footer/footer.component';
import { CustomRatingConfigService } from './custom-rating-config.service';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatBadgeModule } from '@angular/material/badge';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ViewReportTableComponent } from './view-report-table/view-report-table.component';
import { DetailpageComponent, DetailpageDialog } from './detailpage/detailpage.component';
import { ManageAssignmentComponent } from './manage-assignment/manage-assignment.component';
import { MatChipsModule } from '@angular/material/chips';
import { FusionChartsModule } from 'angular-fusioncharts';
import { MatDialogModule } from '@angular/material/dialog';
import FusionCharts from 'fusioncharts/core';
import Column2D from 'fusioncharts/viz/column2d';
import { ViewReportCustomerComponent } from './view-report-customer/view-report-customer.component';
import { MatGridListModule } from '@angular/material';

FusionChartsModule.fcRoot(FusionCharts, Column2D);

@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    MemberiFeedbackComponent,
    NavBarComponent,
    FooterComponent,
    ViewReportTableComponent,
    DetailpageComponent,
    ManageAssignmentComponent,
    MemberiRatingComponent,
    AppreciationpageComponent,
    ViewReportCustomerComponent,
    DetailpageDialog
  ],

  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MatSelectModule,
    MatTableModule,
    MatButtonModule,
    BrowserAnimationsModule,
    StarRatingModule.forRoot(),
    AppRoutingModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    MDBBootstrapModule.forRoot(),
    MatBadgeModule,
    MatProgressBarModule,
    MatChipsModule,
    FusionChartsModule,
    MatGridListModule,
    MatDialogModule
  ],
  entryComponents : [
    DetailpageDialog,
  ],
  providers: [{
    provide: StarRatingConfigService, useClass: CustomRatingConfigService,
  }, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule { }

