import { Component } from '@angular/core';
import { SearchCurrencyRequest } from './models/searchcurrencyrequest.model';
import { CurrencyService } from './services/currency.service';
import { Subscription } from 'rxjs';
import { CurrencyViewModel } from './models/currencyview.model';
import { DatePipe } from '@angular/common';
import { FormControl } from '@angular/forms';
import { countries } from './models/countries';


@Component({
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  styleUrls: ['./currency.component.css'],

})
export class CurrencyComponent {
  model: SearchCurrencyRequest;
  currencyView: CurrencyViewModel;
  countryList = countries
  dataLoaded: string;
  private searchCurrencyRequestSubscription?: Subscription;

  constructor(private currencyService: CurrencyService, private datePipe: DatePipe) {
    this.dataLoaded = '';
    this.model = {
      from: 'EUR',
      to: '',
      money: 1,
      date: new Date(),
      formatedDate: ''
    };
    this.currencyView = {
      errorMessage: '',
      exchangeRate: 0
    }
  }
  onSubmit() {
    console.log(this.model);
    
    this.clear();
    const dateformated: string = this.datePipe.transform(this.model.date, 'yyyy-MM-dd') ?? 'Default';
    this.model.formatedDate = dateformated;
    this.currencyService.convertMoney(this.model)
      .subscribe((result: CurrencyViewModel) => (
        this.currencyView = result,
        this.dataLoaded = this.currencyView.errorMessage == null?`${this.model.money} ${this.model.from} equalent to ${result.exchangeRate}  ${this.model.to}`:this.currencyView.errorMessage
        
        ))
  }
  clear() {

    this.currencyView.exchangeRate = 0;

  }

}


