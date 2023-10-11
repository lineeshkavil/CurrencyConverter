import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CurrencyComponent} from './feature/home/currency/currency.component'
import { ReportComponent } from './feature/home/report/report.component';

const routes: Routes = [
{
  path: 'admin',
  component:CurrencyComponent
 
},
{
  path: 'report',
  component:ReportComponent
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
