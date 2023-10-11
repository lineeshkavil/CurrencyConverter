import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { CurrencyComponent } from './feature/home/currency/currency.component';
import { HttpClientModule } from '@angular/common/http';
import { ReportComponent } from './feature/home/report/report.component';
import { DatePipe,AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
// import { NgChartsModule } from 'ng2-charts'; // Import ChartsModule
import {NgFor} from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    CurrencyComponent,
    ReportComponent
  ],
  imports: [
    [BrowserModule, FormsModule],
    AppRoutingModule,
    HttpClientModule,
    MatAutocompleteModule,
    MatInputModule,
    ReactiveFormsModule,
    NgFor,BrowserAnimationsModule
  ],
  providers: [DatePipe,AsyncPipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
