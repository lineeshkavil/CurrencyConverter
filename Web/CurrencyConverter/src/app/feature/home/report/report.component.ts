import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { ReportSearchModel } from '../currency/models/reportsearchmodel';
import { CurrencyService } from '../currency/services/currency.service';
import { ExchangeRateView } from '../currency/models/exchangerateview';
import { countries } from '../currency/models/countries';


@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
chart : any;
model: ReportSearchModel;
exchangeRate:ExchangeRateView;
countryList = countries
constructor(private currencyService : CurrencyService) { 
    this.model = {
        from : 'EUR',
        to : '',
      };
      this.exchangeRate = {
            dates :'',
            fromCurrency : '',
            rates:'',
            toCurrency:''
      };
}
onFormSubmit(){
    console.log(this.model);
    this.destroyChart();
     this.currencyService.getAllExchangeRates(this.model)
     .subscribe((result : ExchangeRateView) =>(
        this.exchangeRate = result, console.log(result),
        this.chart = new Chart("myChart", {
            type: 'line',
            data: {
                labels: this.exchangeRate.dates.split(','),
                datasets: [{
                    label: `${this.exchangeRate.fromCurrency} to ${this.exchangeRate.toCurrency} Chart`,
                    data: this.exchangeRate.rates.split(','),
                                     
        backgroundColor: 'transparent',
        borderColor: 'purple',
        borderWidth: 2,
        pointBackgroundColor: 'purple'

                }]
            },
            options: {
                plugins: {
                    legend: {
                      display: true
                    },
                    tooltip: {
                      boxPadding: 3,

                    }
                  }
            }
        })
        ))
      }
         
ngOnInit(): void {
 
}
destroyChart(): void {
    if (this.chart) {
      this.chart.destroy(); // Destroy the chart if it exists
    }
  }
}
