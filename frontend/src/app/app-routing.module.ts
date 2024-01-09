import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MemberiFeedbackComponent } from './memberi-feedback/memberi-feedback.component';
import { HomepageComponent } from './homepage/homepage.component';
import { DetailpageComponent } from './detailpage/detailpage.component';
import { ViewReportTableComponent } from './view-report-table/view-report-table.component';
import { ManageAssignmentComponent } from './manage-assignment/manage-assignment.component';
import { MemberiRatingComponent } from './memberi-rating/giv-rat.component';
import { AppreciationpageComponent } from './appreciationpage/appreciationpage.component';
import { ViewReportCustomerComponent } from './view-report-customer/view-report-customer.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'giverating/:kode', component: MemberiRatingComponent },
  { path: 'givefeedback/:assignment_id/:assignment_name', component: MemberiFeedbackComponent },
  { path: 'thankyou', component: AppreciationpageComponent },
  { path: 'home', component: HomepageComponent },
  { path: 'detailpage', redirectTo: 'detailpage/P1/PID1', pathMatch: 'full' },
  { path: 'detailpage/:projectId/:personId', component: DetailpageComponent },
  { path: 'view-report', component: ViewReportTableComponent },
  { path: 'view-report/:proj_id',
    children: [
      { path: '', component: ViewReportTableComponent },
      { path: ':customer_id', component: ViewReportCustomerComponent },
    ]
  },
  { path: 'manage-assignment', component: ManageAssignmentComponent },

];
@NgModule({
  imports: [
    RouterModule.forRoot(routes, {useHash : true})
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {

}
